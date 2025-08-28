using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERPForAll
{
    public partial class ERPForAll : Form
    {
        public ERPForAll()
        {
            InitializeComponent();
        }

        private void fm_kreditoren_add_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-QEUETPR0\SQLEXPRESS;
                                Database=ERPFORALL;
                                Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Lieferant (Vorname, Nachname, Email, Addresse, Ort, PLZ) " +
                               "VALUES (@Vorname, @Nachname, @Email, @Adresse, @Ort, @PLZ)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Vorname", fm_kreditoren_vorname.Text);
                    cmd.Parameters.AddWithValue("@Nachname", fm_kreditoren_name.Text);
                    cmd.Parameters.AddWithValue("@Email", fm_kreditoren_email.Text);
                    cmd.Parameters.AddWithValue("@Adresse", fm_kreditoren_adresse.Text);
                    cmd.Parameters.AddWithValue("@Ort", fm_kreditoren_ort.Text);
                    cmd.Parameters.AddWithValue("@PLZ", int.Parse(fm_kreditoren_plz.Text));

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Kreditor erfolgreich hinzugefügt!");

                        // Felder nach Erfolg zurücksetzen
                        fm_kreditoren_vorname.Text = "";
                        fm_kreditoren_name.Text = "";
                        fm_kreditoren_email.Text = "";
                        fm_kreditoren_adresse.Text = "";
                        fm_kreditoren_ort.Text = "";
                        fm_kreditoren_plz.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Speichern: " + ex.Message);
                    }
                }
            }
        }

    }
}
