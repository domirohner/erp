using ERPForAll.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ERPForAll.Data
{
    public class ArtikelRepository
    {
        public List<Artikel> GetAll()
        {
            var artikelListe = new List<Artikel>();
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "SELECT PKey_Artikel, Name, Beschreibung, Kategorie, Menge FROM Artikel";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        artikelListe.Add(new Artikel
                        {
                            PKey_Artikel = (int)reader["PKey_Artikel"],
                            Name = reader["Name"].ToString(),
                            Beschreibung = reader["Beschreibung"].ToString(),
                            Kategorie = reader["Kategorie"].ToString(),
                            Menge = (int)reader["Menge"]
                        });
                    }
                }
            }
            return artikelListe;
        }

        public void Add(Artikel artikel)
        {
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string query = "INSERT INTO Artikel (Name, Beschreibung, Kategorie, Menge) " +
                               "VALUES (@Name, @Beschreibung, @Kategorie, @Menge)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", artikel.Name ?? "");
                cmd.Parameters.AddWithValue("@Beschreibung", artikel.Beschreibung ?? "");
                cmd.Parameters.AddWithValue("@Kategorie", artikel.Kategorie ?? "");
                cmd.Parameters.AddWithValue("@Menge", artikel.Menge);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<ArtikelBestandDto> GetByLagerort(int lagerortId)
        {
            var list = new List<ArtikelBestandDto>();

            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                string sql = @"
SELECT 
    a.PKey_Artikel            AS ArtikelID,
    a.Name,
    a.Beschreibung,
    a.Kategorie,
    al.Bestand,
    l.PKey_Lagerort           AS LagerortID,
    l.Name                    AS LagerortName
FROM Artikel a
INNER JOIN Artikel_Lagerort al ON al.FKey_Artikel = a.PKey_Artikel
INNER JOIN Lagerort l          ON l.PKey_Lagerort = al.FKey_Lagerort
WHERE l.PKey_Lagerort = @LagerortId
ORDER BY a.Name;";

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@LagerortId", lagerortId);
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new ArtikelBestandDto
                            {
                                ArtikelID = (int)r["ArtikelID"],
                                Name = r["Name"].ToString(),
                                Beschreibung = r["Beschreibung"].ToString(),
                                Kategorie = r["Kategorie"].ToString(),
                                Bestand = (int)r["Bestand"],
                                LagerortID = (int)r["LagerortID"],
                                LagerortName = r["LagerortName"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}
