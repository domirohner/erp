using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ERPForAll.Models;

namespace ERPForAll.Data
{
    public class UmsatzRepository
    {
        public void Add(Umsatz umsatz)
        {
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "INSERT INTO Umsatz (FKey_Bestellung, Betrag, Datum) " +
                               "VALUES (@FKey_Bestellung, @Betrag, @Datum)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FKey_Bestellung", umsatz.FKey_Bestellung);
                cmd.Parameters.AddWithValue("@Betrag", umsatz.Betrag);
                cmd.Parameters.AddWithValue("@Datum", umsatz.Datum);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Umsatz> GetAll()
        {
            var umsaetze = new List<Umsatz>();

            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "SELECT PKey_Umsatz, FKey_Bestellung, Betrag, Datum FROM Umsatz";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        umsaetze.Add(new Umsatz
                        {
                            PKey_Umsatz = (int)reader["PKey_Umsatz"],
                            FKey_Bestellung = (int)reader["FKey_Bestellung"],
                            Betrag = (decimal)reader["Betrag"],
                            Datum = (DateTime)reader["Datum"]
                        });
                    }
                }
            }

            return umsaetze;
        }
    }
}
