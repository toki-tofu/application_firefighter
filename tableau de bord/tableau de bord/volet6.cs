using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tableau_de_bord
{
    public partial class volet6 : Form
    {
        private SQLiteConnection cx;
        private DataSet ds = new DataSet();
        private SQLiteDataAdapter adpt_cbcaserne;
        string fichierDb;
        string dbPath;
        public volet6()
        {
            InitializeComponent();
            fichierDb = Directory.GetFiles(".", "*.db").FirstOrDefault();
            dbPath = $"Data Source={fichierDb}";
            cx = new SQLiteConnection(dbPath);
            cx.Open();
        }

        private void volet6_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Location = new Point(0, 0);
            // remplir la combobox avec les noms des casernes et une option avec "toutes les casernes"
            adpt_cbcaserne = new SQLiteDataAdapter("SELECT DISTINCT nom FROM caserne", cx);
            adpt_cbcaserne.Fill(ds, "casernes");
            cb_choix.DataSource = ds.Tables["casernes"];
            cb_choix.DisplayMember = "nom";
            cb_choix.ValueMember = "nom";
            DataRow dr = ds.Tables["casernes"].NewRow();
            dr["nom"] = "Toutes";
            ds.Tables["casernes"].Rows.InsertAt(dr, 0);
            cb_choix.SelectedIndex = 0;

            Affichertypevehicule();
            Affichertbesttype();

            Affichervehicule();
            AfficherBestV();

            Afficherheure();
            AfficherBestHeure();

            Afficherintervpars();
            AfficherbestIntervParS();

            AfficherHabilitation();
            AfficherBestH();

            AfficherpompierParH();
            AfficherBestPompierParH();
        }

        private void cb_choix_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void Affichertypevehicule()
        {
            // Requête SQL pour compter le nombre d'utilisations de chaque type de véhicule
            string sql = @"
                SELECT codeTypeEngin AS [Type de véhicule], COUNT(*) AS [Nombre d'utilisations]
                FROM PartirAvec
                GROUP BY codeTypeEngin
                ORDER BY [Nombre d'utilisations] DESC, codeTypeEngin ASC
            ";
            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dgv_typevehicule.DataSource = dt;
                label1.Text = "";
                int ligne = 0;
                foreach (DataRow row in dt.Rows)
                {
                    label1.Text += row["Type de véhicule"].ToString() + " : " + row["Nombre d'utilisations"].ToString() + "\n";
                    ligne++;
                }
            }
        }

        private void Affichertbesttype()
        {
            // afficher le type de véhicule le plus utilisé dans lbl_bestType
            string sql = @"
                SELECT codeTypeEngin, COUNT(*) AS [Nombre d'utilisations]
                FROM PartirAvec
                GROUP BY codeTypeEngin
                ORDER BY [Nombre d'utilisations] DESC, codeTypeEngin ASC
                LIMIT 1";

            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    lbl_bestType.Text = dt.Rows[0]["codeTypeEngin"].ToString();
                }
                else
                {
                    lbl_bestType.Text = "Aucun type de véhicule trouvé";
                }
            }
        }

        private void Affichervehicule()
        {
            // Requête SQL pour compter le nombre d'utilisations de chaque véhicule
            string sql = @"
                SELECT idCaserne || '-' || codeTypeEngin || '-' || numeroEngin AS vehicule, COUNT(*) AS occurrences
                FROM PartirAvec
                GROUP BY vehicule
                ORDER BY occurrences DESC, vehicule ASC
            ";
            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dgv_vehicule.DataSource = dt;
                label2.Text = "";
                int ligne = 0;
                foreach (DataRow row in dt.Rows)
                {
                    label2.Text += row["vehicule"].ToString() + " : " + row["occurrences"].ToString() + "\n";
                    ligne++;
                }
            }
        }

        private void AfficherBestV()
        {
            // afficher le véhicule le plus utilisé dans lbl_bestV
            string sql = @"
                SELECT idCaserne || '-' || codeTypeEngin || '-' || numeroEngin AS vehicule, COUNT(*) AS occurrences
                FROM PartirAvec
                GROUP BY vehicule
                ORDER BY occurrences DESC, vehicule ASC
                LIMIT 1";

            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    lbl_bestV.Text = dt.Rows[0]["vehicule"].ToString();
                }
                else
                {
                    lbl_bestV.Text = "Aucun véhicule trouvé";
                }
            }
        }

        private void Afficherheure()
        {
            // Requête SQL pour compter le nombre d'utilisations par heure
            string sql = @"
                SELECT pa.idCaserne || '-' || pa.codeTypeEngin || '-' || pa.numeroEngin AS vehicule,
                SUM(
                    CAST(
                        (JULIANDAY(m.dateHeureRetour) - JULIANDAY(m.dateHeureDepart)) * 24 
                        AS INTEGER
                    )
                ) AS total_heures
                FROM PartirAvec pa
                JOIN Mission m ON pa.idMission = m.id
                WHERE m.terminee = 1
                GROUP BY vehicule
                ORDER BY total_heures DESC, vehicule ASC
            ";
            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dgv_heure.DataSource = dt;
                label4.Text = "";
                int ligne = 0;
                foreach (DataRow row in dt.Rows)
                {
                    label4.Text += row["vehicule"].ToString() + " : " + row["total_heures"].ToString() + "\n";
                    ligne++;
                }
            }
        }

        private void AfficherBestHeure()
        {
            // afficher le véhicule ayant le plus d'heures suivi de son nombre d'heures dans lbl_bestHeureV
            string sql = @"
                SELECT pa.idCaserne || '-' || pa.codeTypeEngin || '-' || pa.numeroEngin AS vehicule,
                SUM(
                    CAST(
                        (JULIANDAY(m.dateHeureRetour) - JULIANDAY(m.dateHeureDepart)) * 24 
                        AS INTEGER
                    )
                ) AS total_heures
                FROM PartirAvec pa
                JOIN Mission m ON pa.idMission = m.id
                WHERE m.terminee = 1
                GROUP BY vehicule
                ORDER BY total_heures DESC, vehicule ASC
                LIMIT 1";
            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string vehicule = dt.Rows[0]["vehicule"].ToString();
                    string heures = dt.Rows[0]["total_heures"].ToString();
                    lbl_bestHeureV.Text = $"{vehicule} : {heures} heures";
                }
                else
                {
                    lbl_bestHeureV.Text = "Aucune heure trouvée";
                }
            }
        }

        private void Afficherintervpars()
        {
            int? idCaserne = GetSelectedCaserneId();
            string sql = @"
                SELECT ns.libelle AS nomSinistre, COUNT(*) AS [Nombre d'interventions]
                FROM Mission m
                JOIN NatureSinistre ns ON m.idNatureSinistre = ns.id
                WHERE m.terminee = 1"
                + (idCaserne.HasValue ? " AND m.idCaserne = @idCaserne" : "") + @"
                GROUP BY ns.libelle
                ORDER BY [Nombre d'interventions] DESC, ns.libelle ASC
            ";
            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                if (idCaserne.HasValue)
                    adpt.SelectCommand.Parameters.AddWithValue("@idCaserne", idCaserne.Value);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dgv_intervParS.DataSource = dt;
                label5.Text = "";
                int ligne = 0;
                foreach (DataRow row in dt.Rows)
                {
                    label5.Text += row["nomSinistre"].ToString() + " : " + row["Nombre d'interventions"].ToString() + "\n";
                    ligne++;
                }
            }
        }

        private void AfficherbestIntervParS()
        {
            int? idCaserne = GetSelectedCaserneId();
            string sql = @"
                SELECT ns.libelle AS nomSinistre, COUNT(*) AS [Nombre d'interventions]
                FROM Mission m
                JOIN NatureSinistre ns ON m.idNatureSinistre = ns.id
                WHERE m.terminee = 1"
                + (idCaserne.HasValue ? " AND m.idCaserne = @idCaserne" : "") + @"
                GROUP BY ns.libelle
                ORDER BY [Nombre d'interventions] DESC, ns.libelle ASC
                LIMIT 1";
            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                if (idCaserne.HasValue)
                    adpt.SelectCommand.Parameters.AddWithValue("@idCaserne", idCaserne.Value);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string nomSinistre = dt.Rows[0]["nomSinistre"].ToString();
                    string nb = dt.Rows[0]["Nombre d'interventions"].ToString();
                    lbl_BestS.Text = $"{nomSinistre} : {nb}";
                }
                else
                {
                    lbl_BestS.Text = "Aucun type d'intervention trouvé";
                }
            }
        }

        private void AfficherHabilitation()
        {
            int? idCaserne = GetSelectedCaserneId();
            string sql = @"
                SELECT h.libelle AS Habilitation, COUNT(*) AS [Nombre d'habilité]
                FROM Passer p
                JOIN habilitation h ON h.id = p.idHabilitation
                JOIN Affectation a ON a.matriculePompier = p.matriculePompier
                JOIN Pompier pm ON pm.matricule = p.matriculePompier"
                + (idCaserne.HasValue ? " WHERE a.idCaserne = @idCaserne" : "") + @"
                GROUP BY h.libelle
                ORDER BY [Nombre d'habilité] DESC, h.libelle ASC
            ";
            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                if (idCaserne.HasValue)
                    adpt.SelectCommand.Parameters.AddWithValue("@idCaserne", idCaserne.Value);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dgv_habilitation.DataSource = dt;
                label6.Text = "";
                int ligne = 0;
                foreach (DataRow row in dt.Rows)
                {
                    label6.Text += row["Habilitation"].ToString() + " : " + row["Nombre d'habilité"].ToString() + "\n";
                    ligne++;
                }
            }
        }

        private void AfficherBestH()
        {
            int? idCaserne = GetSelectedCaserneId();
            string sql = @"
                SELECT h.libelle AS Habilitation, COUNT(*) AS [Nombre d'habilité]
                FROM Passer p
                JOIN habilitation h ON h.id = p.idHabilitation
                JOIN Affectation a ON a.matriculePompier = p.matriculePompier
                JOIN Pompier pm ON pm.matricule = p.matriculePompier"
                + (idCaserne.HasValue ? " WHERE a.idCaserne = @idCaserne" : "") + @"
                GROUP BY h.libelle
                ORDER BY [Nombre d'habilité] DESC, h.libelle ASC
                LIMIT 1";
            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                if (idCaserne.HasValue)
                    adpt.SelectCommand.Parameters.AddWithValue("@idCaserne", idCaserne.Value);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    lbl_BestH.Text = dt.Rows[0]["Habilitation"].ToString();
                }
                else
                {
                    lbl_BestH.Text = "Aucun véhicule trouvé";
                }
            }
        }

        private void AfficherpompierParH()
        {
            int? idCaserne = GetSelectedCaserneId();
            string sql = @"
                SELECT h.libelle AS Habilitation, COUNT(DISTINCT p.matricule) AS [Nombre de pompiers]
                FROM Passer pa
                JOIN Habilitation h ON h.id = pa.idHabilitation
                JOIN Affectation a ON a.matriculePompier = pa.matriculePompier
                JOIN Pompier p ON p.matricule = pa.matriculePompier"
                + (idCaserne.HasValue ? " WHERE a.idCaserne = @idCaserne" : "") + @"
                GROUP BY h.libelle
                ORDER BY [Nombre de pompiers] DESC, h.libelle ASC
            ";
            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                if (idCaserne.HasValue)
                    adpt.SelectCommand.Parameters.AddWithValue("@idCaserne", idCaserne.Value);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dgv_pompierParH.DataSource = dt;
                label7.Text = "";
                int ligne = 0;
                foreach (DataRow row in dt.Rows)
                {
                    label7.Text += row["Habilitation"].ToString() + " : " + row["Nombre de pompiers"].ToString() + "\n";
                    ligne++;
                }
            }
        }

        private void AfficherBestPompierParH()
        {
            int? idCaserne = GetSelectedCaserneId();
            string sql = @"
                SELECT h.libelle AS Habilitation, COUNT(DISTINCT p.matricule) AS [Nombre de pompiers]
                FROM Passer pa
                JOIN Habilitation h ON h.id = pa.idHabilitation
                JOIN Affectation a ON a.matriculePompier = pa.matriculePompier
                JOIN Pompier p ON p.matricule = pa.matriculePompier"
                + (idCaserne.HasValue ? " WHERE a.idCaserne = @idCaserne" : "") + @"
                GROUP BY h.libelle
                ORDER BY [Nombre de pompiers] DESC, h.libelle ASC
                LIMIT 1";
            using (var adpt = new SQLiteDataAdapter(sql, cx))
            {
                if (idCaserne.HasValue)
                    adpt.SelectCommand.Parameters.AddWithValue("@idCaserne", idCaserne.Value);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    lbl_BestPparH.Text = dt.Rows[0]["Habilitation"].ToString();
                }
                else
                {
                    lbl_BestPparH.Text = "Aucune habilitation trouvée";
                }
            }
        }

        private int? GetSelectedCaserneId()
        {
            string caserneNom = cb_choix.SelectedValue?.ToString();
            if (caserneNom == "Toutes") return null;

            using (var cmd = new SQLiteCommand("SELECT id FROM caserne WHERE nom = @nom", cx))
            {
                cmd.Parameters.AddWithValue("@nom", caserneNom);
                var result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                    return id;
            }
            return null;
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cb_choix_SelectedIndexChanged(object sender, EventArgs e)
        {
            Affichertypevehicule();
            Affichertbesttype();

            Affichervehicule();
            AfficherBestV();

            Afficherheure();
            AfficherBestHeure();

            Afficherintervpars();
            AfficherbestIntervParS();

            AfficherHabilitation();
            AfficherBestH();

            AfficherpompierParH();
            AfficherBestPompierParH();
        }

        private void lbl_choix_Click(object sender, EventArgs e)
        {

        }
    }
}