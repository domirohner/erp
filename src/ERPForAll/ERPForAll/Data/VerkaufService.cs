using System;
using System.Data;
using System.Data.SqlClient;

namespace ERPForAll.Data
{
    /// <summary>
    /// Kapselt das Anlegen eines Verkaufs (Bestellung + Position) in einer Transaktion.
    /// Nutzt optional einen vorgegebenen Lieferanten (Kreditor); wenn keiner übergeben wird,
    /// wird der erste vorhandene Lieferant verwendet oder ein Dummy angelegt.
    /// </summary>
    public class VerkaufService
    {
        /// <summary>
        /// Legt eine Bestellung mit einer Position an.
        /// </summary>
        /// <param name="kundeId">Pflicht: Kunde-ID (FKey_Kunde)</param>
        /// <param name="artikelId">Pflicht: Artikel-ID (FKey_Artikel)</param>
        /// <param name="menge">Pflicht: Menge</param>
        /// <param name="preisProStueck">Pflicht: Preis pro Stück</param>
        /// <param name="status">z.B. "Bezahlt" oder "Nicht bezahlt"</param>
        /// <param name="datum">Optional: Bestelldatum (Default: Now)</param>
        /// <param name="faelligkeit">Optional: Fälligkeit (Default: Now + 14 Tage)</param>
        /// <param name="lieferantIdOverride">Optional: explizit gewählter Lieferant (Kreditor). Wenn null, wird EnsureLieferant() verwendet.</param>
        public void CreateVerkauf(
            int kundeId,
            int artikelId,
            int menge,
            decimal preisProStueck,
            string status,
            DateTime? datum = null,
            DateTime? faelligkeit = null,
            int? lieferantIdOverride = null)
        {
            using (var con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    try
                    {
                        // 1) Lieferant bestimmen (aus Auswahl oder automatisch sicherstellen)
                        int lieferantId = lieferantIdOverride ?? EnsureLieferant(con, tx);

                        // 2) Bestellung anlegen (Achtung: Spalte [Fälligkeit] hat Umlaut!)
                        const string insertBestellungSql = @"
INSERT INTO Bestellung (FKey_Lieferant, FKey_Kunde, Datum, [Fälligkeit], Status)
OUTPUT INSERTED.PKey_Bestellung
VALUES (@FKey_Lieferant, @FKey_Kunde, @Datum, @Faelligkeit, @Status);";

                        int bestellungId;
                        using (var cmd = new SqlCommand(insertBestellungSql, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@FKey_Lieferant", lieferantId);
                            cmd.Parameters.AddWithValue("@FKey_Kunde", kundeId);
                            cmd.Parameters.AddWithValue("@Datum", (object)(datum ?? DateTime.Now));
                            cmd.Parameters.AddWithValue("@Faelligkeit", (object)(faelligkeit ?? DateTime.Now.AddDays(14)));
                            cmd.Parameters.AddWithValue("@Status", status ?? "Nicht bezahlt");

                            bestellungId = (int)cmd.ExecuteScalar();
                        }

                        // 3) Position einfügen (Achtung: Spalte [Preis Pro Stück] mit Leerzeichen & Umlaut!)
                        const string insertPosSql = @"
INSERT INTO Bestellung_Artikel (FKey_Bestellung, FKey_Artikel, Menge, [Preis Pro Stück])
VALUES (@FKey_Bestellung, @FKey_Artikel, @Menge, @PreisProStueck);";

                        using (var cmd = new SqlCommand(insertPosSql, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@FKey_Bestellung", bestellungId);
                            cmd.Parameters.AddWithValue("@FKey_Artikel", artikelId);
                            cmd.Parameters.AddWithValue("@Menge", menge);

                            // WICHTIG: Preis als Decimal mit Precision/Scale setzen, um implizite Typkonflikte zu vermeiden
                            var pPreis = new SqlParameter("@PreisProStueck", SqlDbType.Decimal)
                            {
                                Precision = 18,
                                Scale = 2,
                                Value = preisProStueck
                            };
                            cmd.Parameters.Add(pPreis);

                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Stellt sicher, dass es mindestens einen Lieferanten gibt und gibt dessen ID zurück.
        /// Falls keiner existiert, wird ein Dummy-Lieferant angelegt und dessen ID zurückgegeben.
        /// </summary>
        private int EnsureLieferant(SqlConnection con, SqlTransaction tx)
        {
            // Versuch: existierenden Lieferanten nehmen
            using (var cmd = new SqlCommand("SELECT TOP 1 PKey_Lieferant FROM Lieferant ORDER BY PKey_Lieferant", con, tx))
            {
                var obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                    return (int)obj;
            }

            // Keiner vorhanden → Dummy anlegen (Achtung: Spalte heißt in deiner DB "Addresse" mit Doppel-d)
            const string insertDummySql = @"
INSERT INTO Lieferant (Vorname, Nachname, Email, Addresse, Ort, PLZ)
OUTPUT INSERTED.PKey_Lieferant
VALUES ('-', 'System', 'system@example.com', '-', '-', NULL);";

            using (var cmd = new SqlCommand(insertDummySql, con, tx))
            {
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}
