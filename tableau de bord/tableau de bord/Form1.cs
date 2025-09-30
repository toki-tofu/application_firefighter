using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace tableau_de_bord
{
    public partial class Form1 : Form
    {
        private SQLiteConnection cx;
        private DataSet missionDataSet = new DataSet();
        string fichierDb;
        string dbPath;
        public Form1()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            InitializeComponent();
            fichierDb = Directory.GetFiles(".", "*.db").FirstOrDefault();
            dbPath = $"Data Source={fichierDb}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            grpmissions.Controls.Clear();
            grpmissions.AutoScroll = true;
            this.WindowState = FormWindowState.Maximized;
            cx = new SQLiteConnection(dbPath);
            cx.Open();
            LoadMissions();
        }


        private void LoadMissions()
        {
            grpmissions.Controls.Clear();
            grpmissions.AutoScroll = true;

            using (var connection = new SQLiteConnection(dbPath))
            {
                connection.Open();
                var adapter = new SQLiteDataAdapter(
                    "SELECT id, dateHeureDepart, motifAppel, adresse, cp, ville, terminee, dateHeureRetour, compteRendu FROM Mission",
                    connection
                );
                missionDataSet.Clear();
                adapter.Fill(missionDataSet, "Mission");
            }

            DataTable missionTable = missionDataSet.Tables["Mission"];

            foreach (DataRow row in missionTable.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string motif = row["motifAppel"].ToString();
                string dateDepart = row["dateHeureDepart"].ToString();
                string adresse = $"{row["adresse"]}, {row["cp"]} {row["ville"]}";
                bool terminee = Convert.ToBoolean(row["terminee"]);
                string retour = row["dateHeureRetour"] != DBNull.Value ? row["dateHeureRetour"].ToString() : "";

                GroupBox gb = new GroupBox();
                gb.Text = $"Mission {id}";
                gb.Size = new System.Drawing.Size(grpmissions.Width - 30, terminee ? 260 : 230); // GroupBox plus grand
                gb.Tag = terminee;
                gb.ForeColor = System.Drawing.Color.White;
                gb.Font = new Font("Arial", 20F, FontStyle.Bold); // Police Arial taille 20 pour le titre du GroupBox

                string texte = $"Motif : {motif}\nDépart : {dateDepart}\nAdresse : {adresse}\nTerminée : {(terminee ? "Oui" : "Non")}";
                if (terminee && !string.IsNullOrEmpty(retour))
                {
                    texte += $"\nRetour : {retour}";
                }

                Label lbl = new Label();
                lbl.Text = texte;
                lbl.Location = new Point(15, 40);
                lbl.Size = new System.Drawing.Size(gb.Width - 30, 120);
                lbl.ForeColor = System.Drawing.Color.White;
                lbl.Font = new Font("Arial", 16F, FontStyle.Regular); // Label plus grand aussi
                gb.Controls.Add(lbl);

                if (!terminee)
                {
                    TextBox tbCompteRendu = new TextBox();
                    tbCompteRendu.Multiline = true;
                    tbCompteRendu.ScrollBars = ScrollBars.Vertical;
                    tbCompteRendu.Size = new System.Drawing.Size(gb.Width - 180, 90);
                    tbCompteRendu.Location = new Point(15, 150);
                    tbCompteRendu.Font = new Font("Arial", 16F);
                    tbCompteRendu.Tag = id;
                    gb.Controls.Add(tbCompteRendu);

                    Button btn = new Button();
                    btn.Text = "Terminer";
                    btn.Tag = tbCompteRendu;
                    btn.Location = new Point(gb.Width - 150, 170);
                    btn.Size = new System.Drawing.Size(130, 50);
                    btn.Font = new Font("Arial", 16F, FontStyle.Bold);
                    btn.Click += TerminerMission_Click;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
                    btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkBlue;
                    btn.Cursor = Cursors.Hand;
                    gb.Controls.Add(btn);
                }
                else
                {
                    Button pdfBtn = new Button();
                    pdfBtn.Text = "Générer PDF";
                    pdfBtn.Tag = id;
                    pdfBtn.Location = new Point(gb.Width - 150, 200);
                    pdfBtn.Size = new System.Drawing.Size(130, 50);
                    pdfBtn.Font = new Font("Arial", 16F, FontStyle.Bold);
                    pdfBtn.Click += GenererPdf_Click;
                    pdfBtn.FlatStyle = FlatStyle.Flat;
                    pdfBtn.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
                    pdfBtn.FlatAppearance.BorderSize = 1;
                    pdfBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
                    pdfBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkBlue;
                    pdfBtn.Cursor = Cursors.Hand;
                    gb.Controls.Add(pdfBtn);
                }

                gb.Visible = false;
                grpmissions.Controls.Add(gb);
            }


            // Étape 2 : Afficher et positionner les GroupBox en fonction du filtre
            int y = 10;
            foreach (Control ctrl in grpmissions.Controls)
            {
                if (ctrl is GroupBox gb)
                {
                    bool terminee = (bool)gb.Tag;
                    if (!chkencours.Checked || (chkencours.Checked && !terminee))
                    {
                        gb.Location = new Point(10, y);
                        gb.Visible = true;
                        y += gb.Height + 10;
                    }
                    else
                    {
                        gb.Visible = false;
                    }
                }
            }
        }

        private void TerminerMission_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            TextBox tbCompteRendu = button.Tag as TextBox;
            int missionId = (int)tbCompteRendu.Tag;
            string compteRendu = tbCompteRendu.Text;
            string dateRetour = DateTime.Now.ToString("dd-MM-yyyy 'à' HH'h'mm");

            using (var connection = new SQLiteConnection(dbPath))
            {
                connection.Open();
                var command = new SQLiteCommand("UPDATE Mission SET terminee = true, dateHeureRetour = @retour, compteRendu = @compte WHERE id = @id", connection);
                command.Parameters.AddWithValue("@retour", dateRetour);
                command.Parameters.AddWithValue("@compte", compteRendu);
                command.Parameters.AddWithValue("@id", missionId);
                command.ExecuteNonQuery();
            }

            MessageBox.Show($"Mission {missionId} terminée !");
            LoadMissions();
        }

        private void GenererPdf_Click(object sender, EventArgs e)
        {
            int missionId = (int)((Button)sender).Tag;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Fichier PDF (*.pdf)|*.pdf";
                sfd.FileName = $"Mission_{missionId}.pdf";
                sfd.Title = "Enregistrer le fichier PDF";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    GenererPdf(missionId, sfd.FileName);
                }
            }
        }

        private void GenererPdf(int missionId, string pdfPath)
        {
            DataRow mission = missionDataSet.Tables["Mission"].Select($"id = {missionId}")[0];
            string motif = mission["motifAppel"].ToString();
            string dateDepart = mission["dateHeureDepart"].ToString();
            string adresse = $"{mission["adresse"]}, {mission["cp"]} {mission["ville"]}";
            bool terminee = Convert.ToBoolean(mission["terminee"]);
            string retour = mission["dateHeureRetour"] != DBNull.Value ? mission["dateHeureRetour"].ToString() : "";
            string compteRendu = mission["compteRendu"].ToString();

            List<string[]> pompiers = new List<string[]>();
            List<string[]> engins = new List<string[]>();

            using (var connection = new SQLiteConnection(dbPath))
            {
                connection.Open();

                var pompierAdapter = new SQLiteDataAdapter($@"
                    SELECT p.nom, p.prenom, p.sexe, p.portable, p.codeGrade
                    FROM Mobiliser m
                    JOIN Pompier p ON m.matriculePompier = p.matricule
                    WHERE m.idMission = {missionId}", connection);

                DataTable pompierTable = new DataTable();
                pompierAdapter.Fill(pompierTable);
                foreach (DataRow row in pompierTable.Rows)
                {
                    pompiers.Add(new[] {
                        row["nom"].ToString(),
                        row["prenom"].ToString(),
                        row["sexe"].ToString(),
                        row["portable"].ToString(),
                        row["codeGrade"].ToString()
                    });
                }

                var enginAdapter = new SQLiteDataAdapter($@"
                    SELECT numeroEngin, codeTypeEngin, idCaserne
                    FROM PartirAvec
                    WHERE idMission = {missionId}", connection);

                DataTable enginTable = new DataTable();
                enginAdapter.Fill(enginTable);
                foreach (DataRow row in enginTable.Rows)
                {
                    engins.Add(new[] {
                        row["numeroEngin"].ToString(),
                        row["codeTypeEngin"].ToString(),
                        row["idCaserne"].ToString()
                    });
                }
            }

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(2, Unit.Centimetre);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Fiche de Mission #{missionId}").FontSize(20).Bold().Underline();
                        col.Item().LineHorizontal(1);

                        col.Item().Text($"📝 Motif d'appel : {motif}");
                        col.Item().Text($"🚒 Date de départ : {dateDepart}");
                        col.Item().Text($"📍 Adresse : {adresse}");
                        col.Item().Text($"✅ Terminée : {(terminee ? "Oui" : "Non")}");
                        if (terminee && !string.IsNullOrEmpty(retour))
                            col.Item().Text($"⏱️ Retour : {retour}");

                        if (!string.IsNullOrWhiteSpace(compteRendu))
                        {
                            col.Item().Text("\n🗒️ Compte rendu de mission :").FontSize(16).Bold();
                            col.Item().Text(compteRendu);
                        }

                        if (pompiers.Count > 0)
                        {
                            col.Item().Text("\n👨‍🚒 Pompiers Intervenus").FontSize(16).Bold();
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(c => { for (int i = 0; i < 5; i++) c.RelativeColumn(); });
                                table.Header(h =>
                                {
                                    h.Cell().Text("Nom").Bold();
                                    h.Cell().Text("Prénom").Bold();
                                    h.Cell().Text("Sexe").Bold();
                                    h.Cell().Text("Portable").Bold();
                                    h.Cell().Text("Grade").Bold();
                                });
                                foreach (var p in pompiers)
                                    foreach (var info in p)
                                        table.Cell().Text(info);
                            });
                        }

                        if (engins.Count > 0)
                        {
                            col.Item().Text("\n🚒 Engins Mobilisés").FontSize(16).Bold();
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(c => { for (int i = 0; i < 3; i++) c.RelativeColumn(); });
                                table.Header(h =>
                                {
                                    h.Cell().Text("Numéro d'engin").Bold();
                                    h.Cell().Text("Type d'engin").Bold();
                                    h.Cell().Text("Caserne").Bold();
                                });
                                foreach (var e in engins)
                                    foreach (var info in e)
                                        table.Cell().Text(info);
                            });
                        }
                    });
                });
            }).GeneratePdf(pdfPath);

            Process.Start(pdfPath);
        }




        private void pompier_Click(object sender, EventArgs e)
        {
            volet1 form = new volet1();
            DialogResult dr = form.ShowDialog();
        }

        private void vehicule_Click(object sender, EventArgs e)
        {
            volet5 form = new volet5();
            DialogResult dr = form.ShowDialog();
        }

        private void mission_Click(object sender, EventArgs e)
        {
            volet4 form = new volet4();
            DialogResult dr = form.ShowDialog();
            Form1_Load(this,EventArgs.Empty);
        }

        private void btnquitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            volet6 form = new volet6();
            DialogResult dr = form.ShowDialog();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMissions();
            
        }
    }
}

























































// J'ai perdu :D