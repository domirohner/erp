using ERPForAll.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ERPForAll.Data
{
    public class BestellungRepository
    {
        public List<Bestellung> GetAll()
        {
            var bestellungen = new List<Bestellung>();
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "SELECT PKey_Bestellung, FKey_Lieferant, FKey_Kunde, Datum, Faelligkeit, Status FROM Bestellung";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bestellungen.Add(new Bestellung
                        {
                            PKey_Bestellung = (int)reader["PKey_Bestellung"],
                            FKey_Lieferant = (int)reader["FKey_Lieferant"],
                            FKey_Kunde = (int)reader["FKey_Kunde"],
                            Datum = (DateTime)reader["Datum"],
                            Faelligkeit = (DateTime)reader["Faelligkeit"],
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }
            return bestellungen;
        }

        public void Add(Bestellung bestellung)
        {
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "INSERT INTO Bestellung (FKey_Lieferant, FKey_Kunde, Datum, Faelligkeit, Status) " +
                               "VALUES (@FKey_Lieferant, @FKey_Kunde, @Datum, @Faelligkeit, @Status)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FKey_Lieferant", bestellung.FKey_Lieferant);
                cmd.Parameters.AddWithValue("@FKey_Kunde", bestellung.FKey_Kunde);
                cmd.Parameters.AddWithValue("@Datum", bestellung.Datum);
                cmd.Parameters.AddWithValue("@Faelligkeit", bestellung.Faelligkeit);
                cmd.Parameters.AddWithValue("@Status", bestellung.Status ?? "");

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<BestellungStatusDto> GetUnbezahlte()
        {
            var list = new List<BestellungStatusDto>();
            using (var con = new SqlConnection(DatabaseHelper.ConnectionString))
            using (var cmd = new SqlCommand(
                @"SELECT b.PKey_Bestellung, 
                         CONCAT('Bestellung ', b.PKey_Bestellung, ' - ', k.Nachname, ', ', k.Vorname) AS Anzeige
                  FROM Bestellung b
                  INNER JOIN Kunde k ON b.FKey_Kunde = k.PKey_Kunde
                  WHERE ISNULL(b.Status,'') = 'Nicht bezahlt'
                  ORDER BY b.[Fälligkeit];", con))
            {
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new BestellungStatusDto
                        {
                            PKey_Bestellung = (int)r["PKey_Bestellung"],
                            Anzeige = r["Anzeige"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public int SetStatus(int bestellungId, string status)
        {
            using (var con = new SqlConnection(DatabaseHelper.ConnectionString))
            using (var cmd = new SqlCommand(
                "UPDATE Bestellung SET Status = @Status WHERE PKey_Bestellung = @Id;", con))
            {
                cmd.Parameters.AddWithValue("@Status", status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", bestellungId);
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int SetBezahlt(int bestellungId)
        {
            return SetStatus(bestellungId, "Bezahlt");
        }
    }
}
