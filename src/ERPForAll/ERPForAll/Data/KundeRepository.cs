using ERPForAll.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ERPForAll.Data
{
    public class KundeRepository
    {
        public List<Kunde> GetAll()
        {
            var kunden = new List<Kunde>();
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "SELECT PKey_Kunde, Vorname, Nachname, Email, Adresse, Ort, PLZ FROM Kunde";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        kunden.Add(new Kunde
                        {
                            PKey_Kunde = (int)reader["PKey_Kunde"],
                            Vorname = reader["Vorname"].ToString(),
                            Nachname = reader["Nachname"].ToString(),
                            Email = reader["Email"].ToString(),
                            Adresse = reader["Adresse"].ToString(),
                            Ort = reader["Ort"].ToString(),
                            PLZ = reader["PLZ"] == DBNull.Value ? null : (int?)reader["PLZ"]
                        });
                    }
                }
            }
            return kunden;
        }

        public void Add(Kunde kunde)
        {
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "INSERT INTO Kunde (Vorname, Nachname, Email, Adresse, Ort, PLZ) " +
                               "VALUES (@Vorname, @Nachname, @Email, @Adresse, @Ort, @PLZ)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Vorname", kunde.Vorname ?? "");
                cmd.Parameters.AddWithValue("@Nachname", kunde.Nachname ?? "");
                cmd.Parameters.AddWithValue("@Email", kunde.Email ?? "");
                cmd.Parameters.AddWithValue("@Adresse", kunde.Adresse ?? "");
                cmd.Parameters.AddWithValue("@Ort", kunde.Ort ?? "");
                cmd.Parameters.AddWithValue("@PLZ", (object)kunde.PLZ ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
