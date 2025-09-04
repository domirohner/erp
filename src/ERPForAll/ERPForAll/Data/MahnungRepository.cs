using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ERPForAll.Models;

namespace ERPForAll.Data
{
    /// <summary>
    /// Liefert die Mahnliste (alle Bestellungen, die nicht "Bezahlt" sind)
    /// aus den Tabellen Kunde, Bestellung und Bestellung_Artikel.
    /// </summary>
    public class MahnungRepository
    {
        public List<MahnlisteEintragDto> GetAll()
        {
            var result = new List<MahnlisteEintragDto>();

            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = @"
SELECT 
    k.PKey_Kunde        AS KundeID,
    k.Vorname,
    k.Nachname,
    b.PKey_Bestellung   AS BestellungID,
    b.Datum,
    b.[Fälligkeit]      AS Faelligkeit,
    b.Status,
    SUM(CAST(ba.Menge AS decimal(18,2)) * CAST(ba.[Preis Pro Stück] AS decimal(18,2))) AS Betrag
FROM Kunde k
INNER JOIN Bestellung b       ON k.PKey_Kunde = b.FKey_Kunde
INNER JOIN Bestellung_Artikel ba ON b.PKey_Bestellung = ba.FKey_Bestellung
WHERE ISNULL(b.Status,'') <> 'Bezahlt'
GROUP BY k.PKey_Kunde, k.Vorname, k.Nachname, b.PKey_Bestellung, b.Datum, b.[Fälligkeit], b.Status
ORDER BY b.[Fälligkeit] ASC;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new MahnlisteEintragDto
                            {
                                KundeID = (int)reader["KundeID"],
                                Vorname = reader["Vorname"].ToString(),
                                Nachname = reader["Nachname"].ToString(),
                                BestellungID = (int)reader["BestellungID"],
                                Datum = (DateTime)reader["Datum"],
                                Faelligkeit = (DateTime)reader["Faelligkeit"],
                                Status = reader["Status"].ToString(),
                                Betrag = reader["Betrag"] == DBNull.Value ? 0m : (decimal)reader["Betrag"]
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
