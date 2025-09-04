using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ERPForAll.Models;

namespace ERPForAll.Data
{
    public class LagerortRepository
    {
        public void Add(Lagerort l)
        {
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string sql = @"INSERT INTO Lagerort (Name, Addresse, Ort, PLZ)
                               VALUES (@Name, @Addresse, @Ort, @PLZ)";

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Name", (object)l.Name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Addresse", l.Adresse ?? "");
                    cmd.Parameters.AddWithValue("@Ort", l.Ort ?? "");
                    cmd.Parameters.AddWithValue("@PLZ", l.PLZ);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Lagerort> GetAll()
        {
            var list = new List<Lagerort>();
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string sql = @"SELECT PKey_Lagerort, Name, Addresse, Ort, PLZ
                               FROM Lagerort
                               ORDER BY Ort, Name";

                using (var cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new Lagerort
                            {
                                PKey_Lagerort = (int)r["PKey_Lagerort"],
                                Name = r["Name"] as string,
                                Adresse = r["Addresse"] as string,
                                Ort = r["Ort"] as string,
                                PLZ = (int)r["PLZ"]
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
