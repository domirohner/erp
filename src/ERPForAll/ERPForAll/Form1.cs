using System;
using System.Windows.Forms;
using ERPForAll.Data;
using ERPForAll.Models;

namespace ERPForAll
{
    public partial class ERPForAll : Form
    {
        private readonly KundeRepository kundeRepo = new KundeRepository();
        private readonly LieferantRepository lieferantRepo = new LieferantRepository();
        private readonly ArtikelRepository artikelRepo = new ArtikelRepository();
        private readonly BestellungRepository bestellungRepo = new BestellungRepository();
        private readonly UmsatzRepository umsatzRepo = new UmsatzRepository();
        private readonly MahnungRepository mahnungRepo = new MahnungRepository();
        private readonly LagerortRepository lagerortRepo = new LagerortRepository();
        private readonly VerkaufService verkaufService = new VerkaufService();
        private readonly ReportRepository reportRepo = new ReportRepository();
        private readonly ArtikelLagerortRepository artikelLagerortRepo = new ArtikelLagerortRepository();


        public ERPForAll()
        {
            InitializeComponent();
        }

        private void ERPForAll_Load(object sender, EventArgs e)
        {
            fm_verkauf_status.Items.Clear();
            fm_verkauf_status.Items.Add("Bezahlt");
            fm_verkauf_status.Items.Add("Nicht bezahlt");
        }

        // -------------------------
        // Kreditoren (Lieferanten)
        // -------------------------
        private void fm_kreditoren_add_Click(object sender, EventArgs e)
        {
            var lieferant = new Lieferant
            {
                Vorname = fm_kreditoren_vorname.Text,
                Nachname = fm_kreditoren_name.Text,
                Email = fm_kreditoren_email.Text,
                Adresse = fm_kreditoren_adresse.Text,
                Ort = fm_kreditoren_ort.Text,
                PLZ = int.Parse(fm_kreditoren_plz.Text)
            };

            lieferantRepo.Add(lieferant);
            MessageBox.Show("Kreditor erfolgreich hinzugefügt!");
        }

        private void fm_kreditoren_show_Click(object sender, EventArgs e)
        {
            fm_kreditoren_gv.DataSource = lieferantRepo.GetAll();
        }

        // -------------------------
        // Debitoren (Kunden)
        // -------------------------
        private void fm_debitoren_add_Click(object sender, EventArgs e)
        {
            var kunde = new Kunde
            {
                Vorname = fm_debitoren_vorname.Text,
                Nachname = fm_debitoren_name.Text,
                Email = fm_debitoren_email.Text,
                Adresse = fm_debitoren_adresse.Text,
                Ort = fm_debitoren_ort.Text,
                PLZ = int.Parse(fm_debitoren_plz.Text)
            };

            kundeRepo.Add(kunde);
            MessageBox.Show("Debitor erfolgreich hinzugefügt!");
        }

        private void fm_debitoren_show_Click(object sender, EventArgs e)
        {
            fm_debitoren_gv.DataSource = kundeRepo.GetAll();
        }

        // -------------------------
        // Artikel
        // -------------------------
        private void fm_artikel_add_Click(object sender, EventArgs e)
        {
            var artikel = new Artikel
            {
                Name = fm_artikel_name.Text,
                Beschreibung = fm_artikel_beschreibung.Text,
                Kategorie = fm_artikel_kategorie.Text,
            };

            artikelRepo.Add(artikel);
            MessageBox.Show("Artikel erfolgreich hinzugefügt!");
        }

        // -------------------------
        // Bestellungen (Verkäufe)
        // -------------------------
        private void fm_verkauf_artikel_DropDown(object sender, EventArgs e)
        {
            fm_verkauf_artikel.DataSource = artikelRepo.GetAll();
            fm_verkauf_artikel.DisplayMember = nameof(Artikel.Name);
            fm_verkauf_artikel.ValueMember = nameof(Artikel.PKey_Artikel);
        }

        private void fm_verkauf_kunde_DropDown(object sender, EventArgs e)
        {
            fm_verkauf_kunde.DataSource = kundeRepo.GetAll();
            fm_verkauf_kunde.DisplayMember = nameof(Kunde.VollerName);
            fm_verkauf_kunde.ValueMember = nameof(Kunde.PKey_Kunde);
        }

        private void fm_verkauf_add_Click(object sender, EventArgs e)
        {
            if (fm_verkauf_kunde.SelectedValue == null || fm_verkauf_artikel.SelectedValue == null)
            {
                MessageBox.Show("Bitte Kunde und Artikel auswählen!");
                return;
            }
            if (!int.TryParse(fm_verkauf_menge.Text, out int menge) ||
                !decimal.TryParse(fm_verkauf_preis_ps.Text, out decimal preisProStueck))
            {
                MessageBox.Show("Bitte gültige Menge und Preis pro Stück eingeben!");
                return;
            }

            int kundeId = (int)fm_verkauf_kunde.SelectedValue;
            int artikelId = (int)fm_verkauf_artikel.SelectedValue;
            string status = fm_verkauf_status.SelectedItem?.ToString() ?? "Nicht bezahlt";

            int? lieferantId = fm_verkauf_verkaeufer.SelectedValue as int?;
            if (lieferantId == null)
            {
                MessageBox.Show("Bitte einen Verkäufer (Kreditor) auswählen.");
                return;
            }

            try
            {
                verkaufService.CreateVerkauf(
                    kundeId: kundeId,
                    artikelId: artikelId,
                    menge: menge,
                    preisProStueck: preisProStueck,
                    status: status,
                    lieferantIdOverride: lieferantId   // 👈 ausgewählten Kreditor speichern
                );

                MessageBox.Show("Verkauf erfolgreich hinzugefügt!");

                fm_verkauf_menge.Clear();
                fm_verkauf_preis_ps.Clear();
                fm_verkauf_status.SelectedIndex = -1;
                fm_verkauf_verkaeufer.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern des Verkaufs: " + ex.Message);
            }
        }


        // -------------------------
        // Umsätze
        // -------------------------
        private void fm_umsatz_show_Click(object sender, EventArgs e)
        {
            try
            {
                decimal gesamt = reportRepo.GetGesamtUmsatz();
                fm_umsatz.Text = gesamt.ToString("C2"); // hübsch formatiert
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Berechnen des Umsatzes: " + ex.Message);
            }
        }


        // -------------------------
        // Mahnliste
        // -------------------------
        private void fm_mahnliste_show_Click(object sender, EventArgs e)
        {
            fm_mahnliste_gv.DataSource = mahnungRepo.GetAll();
        }

        // Lagerort
        private void lm_lagerort_add_Click(object sender, EventArgs e)
        {
            // Validierung: Adresse, Ort und PLZ sind in der DB NOT NULL
            if (string.IsNullOrWhiteSpace(lm_lagerort_adresse.Text))
            {
                MessageBox.Show("Adresse ist erforderlich.");
                return;
            }
            if (string.IsNullOrWhiteSpace(lm_lagerort_ort.Text))
            {
                MessageBox.Show("Ort ist erforderlich.");
                return;
            }
            if (!int.TryParse(lm_lagerort_plz.Text, out int plz))
            {
                MessageBox.Show("PLZ muss eine Zahl sein.");
                return;
            }

            var lagerort = new Lagerort
            {
                Name = string.IsNullOrWhiteSpace(lm_lagerort_name.Text) ? null : lm_lagerort_name.Text,
                Adresse = lm_lagerort_adresse.Text,
                Ort = lm_lagerort_ort.Text,
                PLZ = plz
            };

            try
            {
                lagerortRepo.Add(lagerort);
                MessageBox.Show("Lagerort erfolgreich hinzugefügt!");

                // Felder leeren
                lm_lagerort_name.Clear();
                lm_lagerort_adresse.Clear();
                lm_lagerort_ort.Clear();
                lm_lagerort_plz.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern des Lagerorts: " + ex.Message);
            }
        }

        private void lm_lagerort_cb_DropDown(object sender, EventArgs e)
        {
            var lagerorte = lagerortRepo.GetAll();

            lm_lagerort_cb.DataSource = lagerorte;
            lm_lagerort_cb.DisplayMember = nameof(Lagerort.Name);        // Text, der angezeigt wird
            lm_lagerort_cb.ValueMember = nameof(Lagerort.PKey_Lagerort); // Wert im Hintergrund
        }

        private void lm_artikel_show_Click(object sender, EventArgs e)
        {
            try
            {
                if (lm_lagerort_cb.SelectedValue == null)
                {
                    MessageBox.Show("Bitte zuerst einen Lagerort wählen.");
                    return;
                }

                int lagerortId = (int)lm_lagerort_cb.SelectedValue;
                var daten = artikelRepo.GetByLagerort(lagerortId);

                lm_artikel_gv.DataSource = daten;
                lm_artikel_gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                lm_artikel_gv.RowHeadersVisible = false;

                // Optional: schönere Spaltenüberschriften
                if (lm_artikel_gv.Columns["ArtikelID"] != null) lm_artikel_gv.Columns["ArtikelID"].HeaderText = "Artikel-ID";
                if (lm_artikel_gv.Columns["LagerortName"] != null) lm_artikel_gv.Columns["LagerortName"].HeaderText = "Lagerort";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Artikel: " + ex.Message);
            }
        }

        private void fm_verkauf_verkaeufer_DropDown(object sender, EventArgs e)
        {
            var lieferanten = lieferantRepo.GetAll();
            fm_verkauf_verkaeufer.DataSource = lieferanten;
            fm_verkauf_verkaeufer.DisplayMember = nameof(Lieferant.VollerName);   // "Nachname, Vorname"
            fm_verkauf_verkaeufer.ValueMember = nameof(Lieferant.PKey_Lieferant);
        }

        private void fm_status_unbezahlt_DropDown(object sender, EventArgs e)
        {
            var daten = bestellungRepo.GetUnbezahlte();

            fm_status_unbezahlt.DataSource = daten;
            fm_status_unbezahlt.DisplayMember = nameof(BestellungStatusDto.Anzeige);         // Text im Dropdown
            fm_status_unbezahlt.ValueMember = nameof(BestellungStatusDto.PKey_Bestellung); // Wert im Hintergrund
        }

        private void fm_status_set_Click(object sender, EventArgs e)
        {
            if (fm_status_unbezahlt.SelectedValue == null)
            {
                MessageBox.Show("Bitte zuerst eine unbezahlte Bestellung auswählen.");
                return;
            }

            int bestellungId = (int)fm_status_unbezahlt.SelectedValue;

            try
            {
                int rows = bestellungRepo.SetBezahlt(bestellungId);
                if (rows == 1)
                {
                    MessageBox.Show("Bestellung wurde auf 'Bezahlt' gesetzt.");

                    // Dropdown neu laden, damit die bezahlte Bestellung verschwindet
                    var daten = bestellungRepo.GetUnbezahlte();
                    fm_status_unbezahlt.DataSource = null;          // reset binding
                    fm_status_unbezahlt.DataSource = daten;
                    fm_status_unbezahlt.DisplayMember = nameof(BestellungStatusDto.Anzeige);
                    fm_status_unbezahlt.ValueMember = nameof(BestellungStatusDto.PKey_Bestellung);
                    fm_status_unbezahlt.SelectedIndex = daten.Count > 0 ? 0 : -1;
                }
                else
                {
                    MessageBox.Show("Es wurde keine Bestellung aktualisiert. Ist die Auswahl korrekt?");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Aktualisieren: " + ex.Message);
            }
        }

        private void fm_umsatz_verkaeufer_DropDown(object sender, EventArgs e)
        {
            var verkaeufer = lieferantRepo.GetAll();
            fm_umsatz_verkaeufer.DataSource = verkaeufer;
            fm_umsatz_verkaeufer.DisplayMember = nameof(Lieferant.VollerName);
            fm_umsatz_verkaeufer.ValueMember = nameof(Lieferant.PKey_Lieferant);

            // Optional: ersten Eintrag selektieren
            if (fm_umsatz_verkaeufer.Items.Count > 0 && fm_umsatz_verkaeufer.SelectedIndex < 0)
                fm_umsatz_verkaeufer.SelectedIndex = 0;
        }

        private void lm_lagermenge_artikel_DropDown(object sender, EventArgs e)
        {
            var artikelListe = artikelRepo.GetAll();

            lm_lagermenge_artikel.DataSource = artikelListe;
            lm_lagermenge_artikel.DisplayMember = nameof(Artikel.Name);        // Anzeigename
            lm_lagermenge_artikel.ValueMember = nameof(Artikel.PKey_Artikel); // ID als Wert

            // optional: ersten Artikel gleich auswählen
            if (lm_lagermenge_artikel.Items.Count > 0 && lm_lagermenge_artikel.SelectedIndex < 0)
                lm_lagermenge_artikel.SelectedIndex = 0;
        }

        private void lm_lagermenge_lagerort_DropDown(object sender, EventArgs e)
        {
            var lagerorte = lagerortRepo.GetAll();

            lm_lagermenge_lagerort.DataSource = lagerorte;
            lm_lagermenge_lagerort.DisplayMember = nameof(Lagerort.Name);         // Name anzeigen
            lm_lagermenge_lagerort.ValueMember = nameof(Lagerort.PKey_Lagerort); // ID als Wert

            // optional: ersten auswählen
            if (lm_lagermenge_lagerort.Items.Count > 0 && lm_lagermenge_lagerort.SelectedIndex < 0)
                lm_lagermenge_lagerort.SelectedIndex = 0;
        }

        private void lm_lagermenge_set_Click(object sender, EventArgs e)
        {
            if (lm_lagermenge_artikel.SelectedValue == null)
            {
                MessageBox.Show("Bitte einen Artikel wählen.");
                return;
            }
            if (lm_lagermenge_lagerort.SelectedValue == null)
            {
                MessageBox.Show("Bitte einen Lagerort wählen.");
                return;
            }
            if (!int.TryParse(lm_lagermenge_menge.Text, out int menge))
            {
                MessageBox.Show("Bitte eine gültige Menge eingeben.");
                return;
            }

            int artikelId = (int)lm_lagermenge_artikel.SelectedValue;
            int lagerortId = (int)lm_lagermenge_lagerort.SelectedValue;

            try
            {
                artikelLagerortRepo.SetBestand(artikelId, lagerortId, menge);

                // Optional: aktuellen Bestand anzeigen/prüfen
                int neuerBestand = artikelLagerortRepo.GetBestand(artikelId, lagerortId);
                MessageBox.Show($"Bestand gesetzt. Neuer Bestand: {neuerBestand}");

                // Optional: Eingaben leeren
                // lm_lagermenge_menge.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Setzen der Lagermenge: " + ex.Message);
            }
        }

        private void em_einkaufe_debitor_DropDown(object sender, EventArgs e)
        {
            var kunden = kundeRepo.GetAll();
            em_einkaufe_debitor.DataSource = kunden;
            em_einkaufe_debitor.DisplayMember = nameof(Kunde.VollerName);
            em_einkaufe_debitor.ValueMember = nameof(Kunde.PKey_Kunde);

            if (em_einkaufe_debitor.Items.Count > 0 && em_einkaufe_debitor.SelectedIndex < 0)
                em_einkaufe_debitor.SelectedIndex = 0;
        }

        private void em_einkaufe_kreditor_DropDown(object sender, EventArgs e)
        {
            var kreditoren = lieferantRepo.GetAll();
            em_einkaufe_kreditor.DataSource = kreditoren;
            em_einkaufe_kreditor.DisplayMember = nameof(Lieferant.VollerName);
            em_einkaufe_kreditor.ValueMember = nameof(Lieferant.PKey_Lieferant);

            if (em_einkaufe_kreditor.Items.Count > 0 && em_einkaufe_kreditor.SelectedIndex < 0)
                em_einkaufe_kreditor.SelectedIndex = 0;
        }

        private void em_verkaufe_show_Click(object sender, EventArgs e)
        {
            if (em_einkaufe_kreditor.SelectedValue == null)
            {
                MessageBox.Show("Bitte zuerst einen Kreditor auswählen.");
                return;
            }

            int lieferantId = (int)em_einkaufe_kreditor.SelectedValue;

            try
            {
                var daten = reportRepo.GetVerkaeufeByLieferant(lieferantId);
                em_verkaufe_gv.DataSource = daten;

                // Optik
                em_verkaufe_gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                em_verkaufe_gv.RowHeadersVisible = false;

                // Kopfzeilen hübsch
                if (em_verkaufe_gv.Columns["BestellungID"] != null) em_verkaufe_gv.Columns["BestellungID"].HeaderText = "Bestellung";
                if (em_verkaufe_gv.Columns["Kunde"] != null) em_verkaufe_gv.Columns["Kunde"].HeaderText = "Kunde";
                if (em_verkaufe_gv.Columns["Datum"] != null) em_verkaufe_gv.Columns["Datum"].HeaderText = "Datum";
                if (em_verkaufe_gv.Columns["Faelligkeit"] != null) em_verkaufe_gv.Columns["Faelligkeit"].HeaderText = "Fälligkeit";
                if (em_verkaufe_gv.Columns["Status"] != null) em_verkaufe_gv.Columns["Status"].HeaderText = "Status";
                if (em_verkaufe_gv.Columns["Betrag"] != null)
                {
                    em_verkaufe_gv.Columns["Betrag"].HeaderText = "Betrag";
                    em_verkaufe_gv.Columns["Betrag"].DefaultCellStyle.Format = "C2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Verkäufe: " + ex.Message);
            }
        }

        private void em_einkaeufe_show_Click(object sender, EventArgs e)
        {
            if (em_einkaufe_debitor.SelectedValue == null)
            {
                MessageBox.Show("Bitte zuerst einen Debitor (Kunden) auswählen.");
                return;
            }

            int kundeId = (int)em_einkaufe_debitor.SelectedValue;

            try
            {
                var daten = reportRepo.GetEinkaeufeByKunde(kundeId);
                em_einkaufe_gv.DataSource = daten;

                // Optik
                em_einkaufe_gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                em_einkaufe_gv.RowHeadersVisible = false;

                // Spaltenbeschriftungen & Format
                if (em_einkaufe_gv.Columns["BestellungID"] != null) em_einkaufe_gv.Columns["BestellungID"].HeaderText = "Bestellung";
                if (em_einkaufe_gv.Columns["Lieferant"] != null) em_einkaufe_gv.Columns["Lieferant"].HeaderText = "Kreditor";
                if (em_einkaufe_gv.Columns["Datum"] != null) em_einkaufe_gv.Columns["Datum"].HeaderText = "Datum";
                if (em_einkaufe_gv.Columns["Faelligkeit"] != null) em_einkaufe_gv.Columns["Faelligkeit"].HeaderText = "Fälligkeit";
                if (em_einkaufe_gv.Columns["Betrag"] != null)
                {
                    em_einkaufe_gv.Columns["Betrag"].HeaderText = "Betrag";
                    em_einkaufe_gv.Columns["Betrag"].DefaultCellStyle.Format = "C2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Einkäufe: " + ex.Message);
            }
        }

    }
}
