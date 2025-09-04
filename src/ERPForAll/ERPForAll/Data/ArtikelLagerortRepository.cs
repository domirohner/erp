using System;
using System.Data.SqlClient;

namespace ERPForAll.Data
{
    public class ArtikelLagerortRepository
    {
        /// <summary>
        /// Setzt den Bestand eines Artikels an einem Lagerort (Upsert).
        /// </summary>
        public void SetBestand(int artikelId, int lagerortId, int menge)
        {
            using (var con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                // Upsert-Logik ohne MERGE (robust & klar):
                // Wenn vorhanden -> UPDATE; sonst -> INSERT
                const string sql = @"
IF EXISTS (SELECT 1 FROM Artikel_Lagerort WHERE FKey_Artikel = @A AND FKey_Lagerort = @L)
    UPDATE Artikel_Lagerort
       SET Bestand = @M
     WHERE FKey_Artikel = @A AND FKey_Lagerort = @L;
ELSE
    INSERT INTO Artikel_Lagerort (FKey_Artikel, FKey_Lagerort, Bestand)
    VALUES (@A, @L, @M);";

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@A", artikelId);
                    cmd.Parameters.AddWithValue("@L", lagerortId);
                    cmd.Parameters.AddWithValue("@M", menge);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Holt den aktuellen Bestand eines Artikels an einem Lagerort. Gibt 0 zurück, wenn kein Datensatz existiert.
        /// </summary>
        public int GetBestand(int artikelId, int lagerortId)
        {
            using (var con = new SqlConnection(DatabaseHelper.ConnectionString))
            using (var cmd = new SqlCommand(
                "SELECT Bestand FROM Artikel_Lagerort WHERE FKey_Artikel = @A AND FKey_Lagerort = @L;", con))
            {
                cmd.Parameters.AddWithValue("@A", artikelId);
                cmd.Parameters.AddWithValue("@L", lagerortId);
                con.Open();
                var obj = cmd.ExecuteScalar();
                return (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
            }
        }
    }
}
