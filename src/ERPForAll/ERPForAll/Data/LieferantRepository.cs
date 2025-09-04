using ERPForAll.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ERPForAll.Data
{
    public class LieferantRepository
    {
        public List<Lieferant> GetAll()
        {
            var lieferanten = new List<Lieferant>();
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "SELECT PKey_Lieferant, Vorname, Nachname, Email, Addresse, Ort, PLZ FROM Lieferant ORDER BY Nachname, Vorname";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lieferanten.Add(new Lieferant
                        {
                            PKey_Lieferant = (int)reader["PKey_Lieferant"],
                            Vorname = reader["Vorname"].ToString(),
                            Nachname = reader["Nachname"].ToString(),
                            Email = reader["Email"].ToString(),
                            Adresse = reader["Addresse"].ToString(),
                            Ort = reader["Ort"].ToString(),
                            PLZ = reader["PLZ"] == DBNull.Value ? null : (int?)reader["PLZ"]
                        });
                    }
                }
            }
            return lieferanten;
        }

        public void Add(Lieferant lieferant)
        {
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "INSERT INTO Lieferant (Vorname, Nachname, Email, Addresse, Ort, PLZ) " +
                               "VALUES (@Vorname, @Nachname, @Email, @Addresse, @Ort, @PLZ)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Vorname", lieferant.Vorname ?? "");
                cmd.Parameters.AddWithValue("@Nachname", lieferant.Nachname ?? "");
                cmd.Parameters.AddWithValue("@Email", lieferant.Email ?? "");
                cmd.Parameters.AddWithValue("@Addresse", lieferant.Adresse ?? "");
                cmd.Parameters.AddWithValue("@Ort", lieferant.Ort ?? "");
                cmd.Parameters.AddWithValue("@PLZ", (object)lieferant.PLZ ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
