using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ERPForAll.Models;

namespace ERPForAll.Data
{
    public class ReportRepository
    {
        private static string Cs => DatabaseHelper.ConnectionString;

        /// <summary>
        /// Gesamtumsatz über alle Verkäufe (Summe Menge * Preis pro Stück).
        /// </summary>
        public decimal GetGesamtUmsatz()
        {
            using (var con = new SqlConnection(Cs))
            using (var cmd = new SqlCommand(@"
                SELECT SUM(CAST(ba.Menge AS decimal(18,2)) * CAST(ba.[Preis Pro Stück] AS decimal(18,2)))
                FROM Bestellung_Artikel ba;", con))
            {
                con.Open();
                var obj = cmd.ExecuteScalar();
                return (obj == null || obj == DBNull.Value) ? 0m : (decimal)obj;
            }
        }

        /// <summary>
        /// Umsatz pro Kunde (falls du eine Übersicht brauchst).
        /// </summary>
        public List<UmsatzProKundeDto> GetUmsatzProKunde()
        {
            var list = new List<UmsatzProKundeDto>();
            using (var con = new SqlConnection(Cs))
            using (var cmd = new SqlCommand(@"
                SELECT 
                    k.PKey_Kunde AS KundeID,
                    k.Vorname,
                    k.Nachname,
                    SUM(CAST(ba.Menge AS decimal(18,2)) * CAST(ba.[Preis Pro Stück] AS decimal(18,2))) AS Umsatz
                FROM Kunde k
                INNER JOIN Bestellung b        ON b.FKey_Kunde = k.PKey_Kunde
                INNER JOIN Bestellung_Artikel ba ON ba.FKey_Bestellung = b.PKey_Bestellung
                GROUP BY k.PKey_Kunde, k.Vorname, k.Nachname
                ORDER BY Umsatz DESC;", con))
            {
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new UmsatzProKundeDto
                        {
                            KundeID = (int)r["KundeID"],
                            Vorname = r["Vorname"].ToString(),
                            Nachname = r["Nachname"].ToString(),
                            Umsatz = r["Umsatz"] == DBNull.Value ? 0m : (decimal)r["Umsatz"]
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// (Optional) Umsatz gefiltert auf einen Lieferanten (als Verkäufer genutzt).
        /// </summary>
        public decimal GetUmsatzByLieferant(int lieferantId)
        {
            using (var con = new SqlConnection(Cs))
            using (var cmd = new SqlCommand(@"
                SELECT SUM(CAST(ba.Menge AS decimal(18,2)) * CAST(ba.[Preis Pro Stück] AS decimal(18,2)))
                FROM Bestellung_Artikel ba
                INNER JOIN Bestellung b ON b.PKey_Bestellung = ba.FKey_Bestellung
                WHERE b.FKey_Lieferant = @Lid;", con))
            {
                cmd.Parameters.AddWithValue("@Lid", lieferantId);
                con.Open();
                var obj = cmd.ExecuteScalar();
                return (obj == null || obj == DBNull.Value) ? 0m : (decimal)obj;
            }
        }

        public List<VerkaufListeDto> GetVerkaeufeByLieferant(int lieferantId)
        {
            var list = new List<VerkaufListeDto>();
            using (var con = new SqlConnection(DatabaseHelper.ConnectionString))
            using (var cmd = new SqlCommand(@"
                SELECT
                    b.PKey_Bestellung                 AS BestellungID,
                    CONCAT(k.Nachname, ', ', ISNULL(k.Vorname,'')) AS Kunde,
                    b.Datum,
                    b.[Fälligkeit]                   AS Faelligkeit,
                    b.Status,
                    SUM(CAST(ba.Menge AS decimal(18,2)) * CAST(ba.[Preis Pro Stück] AS decimal(18,2))) AS Betrag
                FROM Bestellung b
                INNER JOIN Kunde k              ON k.PKey_Kunde = b.FKey_Kunde
                INNER JOIN Bestellung_Artikel ba ON ba.FKey_Bestellung = b.PKey_Bestellung
                WHERE b.FKey_Lieferant = @L
                GROUP BY b.PKey_Bestellung, k.Nachname, k.Vorname, b.Datum, b.[Fälligkeit], b.Status
                ORDER BY b.Datum DESC;", con))
            {
                cmd.Parameters.AddWithValue("@L", lieferantId);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new VerkaufListeDto
                        {
                            BestellungID = (int)r["BestellungID"],
                            Kunde = r["Kunde"].ToString(),
                            Datum = (System.DateTime)r["Datum"],
                            Faelligkeit = (System.DateTime)r["Faelligkeit"],
                            Status = r["Status"].ToString(),
                            Betrag = r["Betrag"] == System.DBNull.Value ? 0m : (decimal)r["Betrag"]
                        });
                    }
                }
            }
            return list;
        }

        public List<EinkaufListeDto> GetEinkaeufeByKunde(int kundeId)
        {
            var list = new List<EinkaufListeDto>();
            using (var con = new SqlConnection(DatabaseHelper.ConnectionString))
            using (var cmd = new SqlCommand(@"
                SELECT
                    b.PKey_Bestellung AS BestellungID,
                    CONCAT(l.Nachname, ', ', ISNULL(l.Vorname,'')) AS Lieferant,
                    b.Datum,
                    b.[Fälligkeit]   AS Faelligkeit,
                    b.Status,
                    SUM(CAST(ba.Menge AS decimal(18,2)) * CAST(ba.[Preis Pro Stück] AS decimal(18,2))) AS Betrag
                FROM Bestellung b
                INNER JOIN Lieferant l        ON l.PKey_Lieferant = b.FKey_Lieferant
                INNER JOIN Bestellung_Artikel ba ON ba.FKey_Bestellung = b.PKey_Bestellung
                WHERE b.FKey_Kunde = @K
                GROUP BY b.PKey_Bestellung, l.Nachname, l.Vorname, b.Datum, b.[Fälligkeit], b.Status
                ORDER BY b.Datum DESC;", con))
            {
                cmd.Parameters.AddWithValue("@K", kundeId);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new EinkaufListeDto
                        {
                            BestellungID = (int)r["BestellungID"],
                            Lieferant = r["Lieferant"].ToString(),
                            Datum = (System.DateTime)r["Datum"],
                            Faelligkeit = (System.DateTime)r["Faelligkeit"],
                            Status = r["Status"].ToString(),
                            Betrag = r["Betrag"] == System.DBNull.Value ? 0m : (decimal)r["Betrag"]
                        });
                    }
                }
            }
            return list;
        }
    }
}
