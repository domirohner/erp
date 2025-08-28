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

        private void ERPForAll_Load_1(object sender, EventArgs e)
        {
            fm_verkauf_status.Items.Clear();
            fm_verkauf_status.Items.Add("Bezahlt");
            fm_verkauf_status.Items.Add("Nicht bezahlt");
        }

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text; // nur der Text wird in der ComboBox angezeigt
            }
        }

        private void LoadArtikelComboBox()
        {
            string connectionString = @"Data Source=LAPTOP-QEUETPR0\SQLEXPRESS;Database=ERPFORALL;Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT PKey_Artikel, Name FROM Artikel ORDER BY Name";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        fm_verkauf_artikel.Items.Clear(); // ComboBox vorher leeren

                        while (reader.Read())
                        {
                            fm_verkauf_artikel.Items.Add(new ComboBoxItem
                            {
                                Text = reader["Name"].ToString(),
                                Value = reader["PKey_Artikel"]
                            });
                        }

                        if (fm_verkauf_artikel.Items.Count > 0)
                            fm_verkauf_artikel.SelectedIndex = 0; // ersten Artikel auswählen
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Laden der Artikel: " + ex.Message);
                    }
                }
            }
        }

        private void LoadKundenComboBox()
        {
            string connectionString = @"Data Source=LAPTOP-QEUETPR0\SQLEXPRESS;Database=ERPFORALL;Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT PKey_Kunde, Vorname, Nachname FROM Kunde ORDER BY Nachname, Vorname";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        fm_verkauf_kunde.Items.Clear(); // ComboBox leeren

                        while (reader.Read())
                        {
                            fm_verkauf_kunde.Items.Add(new ComboBoxItem
                            {
                                Text = reader["Nachname"].ToString() + ", " + reader["Vorname"].ToString(),
                                Value = reader["PKey_Kunde"]
                            });
                        }

                        if (fm_verkauf_kunde.Items.Count > 0)
                            fm_verkauf_kunde.SelectedIndex = 0; // ersten Kunden auswählen
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Laden der Kunden: " + ex.Message);
                    }
                }
            }
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

        private void fm_debitoren_add_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-QEUETPR0\SQLEXPRESS;
                                Database=ERPFORALL;
                                Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Kunde (Vorname, Nachname, Email, Adresse, Ort, PLZ) " +
                               "VALUES (@Vorname, @Nachname, @Email, @Adresse, @Ort, @PLZ)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Vorname", fm_debitoren_vorname.Text);
                    cmd.Parameters.AddWithValue("@Nachname", fm_debitoren_name.Text);
                    cmd.Parameters.AddWithValue("@Email", fm_debitoren_email.Text);
                    cmd.Parameters.AddWithValue("@Adresse", fm_debitoren_adresse.Text);
                    cmd.Parameters.AddWithValue("@Ort", fm_debitoren_ort.Text);
                    cmd.Parameters.AddWithValue("@PLZ", int.Parse(fm_debitoren_plz.Text));

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Debitor erfolgreich hinzugefügt!");

                        // Felder nach Erfolg zurücksetzen
                        fm_debitoren_vorname.Text = "";
                        fm_debitoren_name.Text = "";
                        fm_debitoren_email.Text = "";
                        fm_debitoren_adresse.Text = "";
                        fm_debitoren_ort.Text = "";
                        fm_debitoren_plz.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Speichern: " + ex.Message);
                    }
                }
            }
        }

        private void fm_debitoren_show_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-QEUETPR0\SQLEXPRESS;
                                Database=ERPFORALL;
                                Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT PKey_Kunde AS ID, Vorname, Nachname, Email, Adresse, Ort, PLZ FROM Kunde";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);  // Daten aus der DB in DataTable laden

                        fm_debitoren_gv.DataSource = dt; // DataGridView mit Daten füllen
                        fm_debitoren_gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Spaltenbreite anpassen
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Laden der Debitoren: " + ex.Message);
                    }
                }
            }
        }

        private void fm_kreditoren_show_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-QEUETPR0\SQLEXPRESS;
                                Database=ERPFORALL;
                                Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT PKey_Lieferant AS ID, Vorname, Nachname, Email, Addresse, Ort, PLZ FROM Lieferant";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);  // Daten aus der DB in DataTable laden

                        fm_kreditoren_gv.DataSource = dt; // DataGridView mit Daten füllen
                        fm_kreditoren_gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Spaltenbreite anpassen
                        fm_kreditoren_gv.ScrollBars = ScrollBars.Both; // Scrollbalken bei Bedarf
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Laden der Kreditoren: " + ex.Message);
                    }
                }
            }
        }

        private void fm_umsatz_show_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-QEUETPR0\SQLEXPRESS;
                                Database=ERPFORALL;
                                Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Umsatz pro Kunde berechnen
                string query = @"
            SELECT 
                k.PKey_Kunde AS KundeID,
                k.Vorname,
                k.Nachname,
                SUM(ba.Menge * ba.[Preis Pro Stück]) AS Umsatz
            FROM Kunde k
            INNER JOIN Bestellung b ON k.PKey_Kunde = b.Fkey_Kunde
            INNER JOIN Bestellung_Artikel ba ON b.PKey_Bestellung = ba.FKey_Bestellung
            GROUP BY k.PKey_Kunde, k.Vorname, k.Nachname
            ORDER BY Umsatz DESC";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        fm_umsatz_gv.DataSource = dt; // DataGridView füllen
                        fm_umsatz_gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Spaltenbreite anpassen
                        fm_umsatz_gv.ScrollBars = ScrollBars.Both; // Scrollbalken
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Laden der Umsätze: " + ex.Message);
                    }
                }
            }
        }

        private void fm_mahnliste_show_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-QEUETPR0\SQLEXPRESS;
                                Database=ERPFORALL;
                                Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Mahnliste: Kunden mit Bestellungen, die noch nicht bezahlt sind
                string query = @"
            SELECT 
                k.PKey_Kunde AS KundeID,
                k.Vorname,
                k.Nachname,
                b.PKey_Bestellung AS BestellungID,
                b.Datum,
                b.Fälligkeit,
                b.Status,
                SUM(ba.Menge * ba.[Preis Pro Stück]) AS Betrag
            FROM Kunde k
            INNER JOIN Bestellung b ON k.PKey_Kunde = b.Fkey_Kunde
            INNER JOIN Bestellung_Artikel ba ON b.PKey_Bestellung = ba.FKey_Bestellung
            WHERE b.Status <> 'Bezahlt' -- Nur unbezahlte Bestellungen
            GROUP BY k.PKey_Kunde, k.Vorname, k.Nachname, b.PKey_Bestellung, b.Datum, b.Fälligkeit, b.Status
            ORDER BY b.Fälligkeit ASC";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        fm_mahnliste_gv.DataSource = dt;
                        fm_mahnliste_gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        fm_mahnliste_gv.ScrollBars = ScrollBars.Both;

                        // Betrag als Währung formatieren
                        fm_mahnliste_gv.Columns["Betrag"].DefaultCellStyle.Format = "C2";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Laden der Mahnliste: " + ex.Message);
                    }
                }
            }
        }

        private void fm_artikel_add_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-QEUETPR0\SQLEXPRESS;
                                Database=ERPFORALL;
                                Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Artikel (Name, Beschreibung, Kategorie, Menge) " +
                               "VALUES (@Name, @Beschreibung, @Kategorie, @Menge)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", fm_artikel_name.Text);
                    cmd.Parameters.AddWithValue("@Beschreibung", fm_artikel_beschreibung.Text);
                    cmd.Parameters.AddWithValue("@Kategorie", fm_artikel_kategorie.Text);
                    cmd.Parameters.AddWithValue("@Menge", 0); // Anfangsbestand = 0

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Artikel erfolgreich hinzugefügt!");

                        // Felder nach Erfolg zurücksetzen
                        fm_artikel_name.Text = "";
                        fm_artikel_beschreibung.Text = "";
                        fm_artikel_kategorie.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Speichern des Artikels: " + ex.Message);
                    }
                }
            }
        }

        private void fm_verkauf_artikel_DropDown(object sender, EventArgs e)
        {
            LoadArtikelComboBox();
        }

        private void fm_verkauf_kunde_DropDown(object sender, EventArgs e)
        {
            LoadKundenComboBox();
        }

        private void fm_verkauf_add_Click(object sender, EventArgs e)
        {
            if (fm_verkauf_kunde.SelectedItem == null || fm_verkauf_artikel.SelectedItem == null)
            {
                MessageBox.Show("Bitte Kunde und Artikel auswählen!");
                return;
            }

            if (string.IsNullOrWhiteSpace(fm_verkauf_menge.Text) || string.IsNullOrWhiteSpace(fm_verkauf_preis_ps.Text))
            {
                MessageBox.Show("Bitte Menge und Preis pro Stück eingeben!");
                return;
            }

            int kundeId = (int)((ComboBoxItem)fm_verkauf_kunde.SelectedItem).Value;
            int artikelId = (int)((ComboBoxItem)fm_verkauf_artikel.SelectedItem).Value;
            int menge = int.Parse(fm_verkauf_menge.Text);
            decimal preisProStueck = decimal.Parse(fm_verkauf_preis_ps.Text);
            string status = fm_verkauf_status.SelectedItem?.ToString() ?? "Nicht bezahlt";

            string connectionString = @"Data Source=LAPTOP-QEUETPR0\SQLEXPRESS;Database=ERPFORALL;Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // 1️⃣ Bestellung einfügen
                    string insertBestellung = @"
                INSERT INTO Bestellung (FKey_Kunde, Datum, Fälligkeit, Status) 
                OUTPUT INSERTED.PKey_Bestellung
                VALUES (@FKey_Kunde, @Datum, @Faelligkeit, @Status)";

                    int bestellungId;
                    using (SqlCommand cmd = new SqlCommand(insertBestellung, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@FKey_Kunde", kundeId);
                        cmd.Parameters.AddWithValue("@Datum", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Faelligkeit", DateTime.Now.AddDays(14)); // z.B. 14 Tage Zahlungsziel
                        cmd.Parameters.AddWithValue("@Status", status);

                        bestellungId = (int)cmd.ExecuteScalar();
                    }

                    // 2️⃣ Artikel zur Bestellung hinzufügen
                    string insertBestellungArtikel = @"
                INSERT INTO Bestellung_Artikel (FKey_Bestellung, FKey_Artikel, Menge, [Preis Pro Stück]) 
                VALUES (@FKey_Bestellung, @FKey_Artikel, @Menge, @PreisProStueck)";

                    using (SqlCommand cmd = new SqlCommand(insertBestellungArtikel, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@FKey_Bestellung", bestellungId);
                        cmd.Parameters.AddWithValue("@FKey_Artikel", artikelId);
                        cmd.Parameters.AddWithValue("@Menge", menge);
                        cmd.Parameters.AddWithValue("@PreisProStueck", preisProStueck);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Verkauf erfolgreich hinzugefügt!");

                    // Eingabefelder zurücksetzen
                    fm_verkauf_menge.Text = "";
                    fm_verkauf_preis_ps.Text = "";
                    fm_verkauf_status.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Fehler beim Speichern des Verkaufs: " + ex.Message);
                }
            }
        }

    }
}
