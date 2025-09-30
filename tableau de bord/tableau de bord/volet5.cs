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
    public partial class volet5 : Form
    {
        private BindingSource bs = new BindingSource();
        private SQLiteDataAdapter adapter;
        private SQLiteDataAdapter adpt_cbcaserne;
        string fichierDb;
        string dbPath;

        public volet5()
        {
            InitializeComponent();
            fichierDb = Directory.GetFiles(".", "*.db").FirstOrDefault();
            dbPath = $"Data Source={fichierDb}";
            SQLiteConnection connec = new SQLiteConnection(dbPath);

            adapter = new SQLiteDataAdapter("SELECT * FROM Engin ORDER BY idCaserne ASC", connec);
            adapter.Fill(MesDatas.DsGlobal, "Engin");

            // Correction ici
            bs.DataSource = MesDatas.DsGlobal.Tables["Engin"];
            bs.PositionChanged += Bs_PositionChanged;

            pb_vehicule.SizeMode = PictureBoxSizeMode.StretchImage;
            AfficherImageCourante();


            cb_ChoixCaserne.SelectedIndexChanged += cb_ChoixCaserne_SelectedIndexChanged;

            this.Location = new Point(0, 0);

        }

        private void volet5_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Location = new Point(0, 0);
            pb_dernier.Image = Image.FromFile("dernier.png");
            pb_suivant.Image = Image.FromFile("suivant.png");
            pb_premier.Image = Image.FromFile("premier.png");
            pb_precedent.Image = Image.FromFile("precedent.png");
            pb_dernier.Cursor = Cursors.Hand;
            pb_suivant.Cursor = Cursors.Hand;
            pb_premier.Cursor = Cursors.Hand;
            pb_precedent.Cursor = Cursors.Hand;

            string connectionString = "Data Source=SDIS67.db";
            SQLiteConnection connec = new SQLiteConnection(connectionString);

            // remplir la combobox avec les noms des casernes
            adpt_cbcaserne = new SQLiteDataAdapter("SELECT DISTINCT * FROM Caserne", connec);
            adpt_cbcaserne.Fill(MesDatas.DsGlobal, "Caserne");

            cb_ChoixCaserne.Items.Add("Toutes");
            foreach (DataRow row in MesDatas.DsGlobal.Tables["Caserne"].Rows)
            {
                cb_ChoixCaserne.Items.Add(row["nom"].ToString());
            }
            cb_ChoixCaserne.SelectedIndex = 0;


            // Attribution de l'image aux PictureBox de navigation
            pb_premier.SizeMode = PictureBoxSizeMode.StretchImage;
            pb_dernier.SizeMode = PictureBoxSizeMode.StretchImage;
            pb_suivant.SizeMode = PictureBoxSizeMode.StretchImage;
            pb_precedent.SizeMode = PictureBoxSizeMode.StretchImage;

            if (bs.Current is DataRowView Row)
            {
                lbl_numid.Text = Row["idCaserne"].ToString() + "-" + Row["codeTypeEngin"].ToString() + "-" + Row["numero"].ToString();
                lbl_daterecep.Text = Row["dateReception"].ToString();

                if (Row["enMission"].ToString() == "1")
                {
                    ckb_mission.Checked = true;
                }

                if (Row["enPanne"].ToString() == "1")
                {
                    ckb_panne.Checked = true;
                }
            }


        }

        private void cb_ChoixCaserne_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        // Navigation
        private void pb_dernier_Click(object sender, EventArgs e)
        {
            bs.MoveLast();

        }

        private void pb_suivant_Click(object sender, EventArgs e)
        {
            bs.MoveNext();
        }

        private void pb_premier_Click(object sender, EventArgs e)
        {
            bs.MoveFirst();
        }

        private void pb_precedent_Click(object sender, EventArgs e)
        {
            bs.MovePrevious();
        }

        private void Bs_PositionChanged(object sender, EventArgs e)
        {
            if (cb_ChoixCaserne.SelectedIndex <= 0)
            {
                bs.RemoveFilter();
                AfficherImageCourante();
                AfficherNumID();
                AfficherDateRecep();
                AfficherMissionPanne();
                return;
            }
            else
            {
                AfficherImageCourante();
                AfficherNumID();
                AfficherDateRecep();
                AfficherMissionPanne();
            }

        }

        // Méthode pour afficher l’image liée à l’engin sélectionné
        private void AfficherImageCourante()
        {
            if (bs.Current is DataRowView row)
            {
                if (row.IsNew)
                {
                    if (pb_vehicule.Image != null)
                    {
                        pb_vehicule.Image.Dispose();
                        pb_vehicule.Image = null;
                    }
                    return;
                }

                string codeTypeEngin = row["codeTypeEngin"].ToString();
                string imagePath = $"./img/{codeTypeEngin}.png";

                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    FileInfo fi = new FileInfo(imagePath);
                    if (fi.Length == 0)
                    {
                        if (pb_vehicule.Image != null)
                        {
                            pb_vehicule.Image.Dispose();
                            pb_vehicule.Image = null;
                        }
                        MessageBox.Show("Le fichier image est vide : " + imagePath);
                        return;
                    }

                    if (pb_vehicule.Image != null)
                    {
                        pb_vehicule.Image.Dispose();
                        pb_vehicule.Image = null;
                    }

                    using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var img = Image.FromStream(fs))
                    {
                        pb_vehicule.Image = (Image)img.Clone();
                    }
                }
                else
                {
                    if (pb_vehicule.Image != null)
                    {
                        pb_vehicule.Image.Dispose();
                        pb_vehicule.Image = null;
                    }
                }
            }
        }

        private void AfficherNumID()
        {
            if (bs.Current is DataRowView row)
            {
                if (row.IsNew)
                {
                    lbl_numid.Text = "Nouveau";
                }
                else
                {
                    lbl_numid.Text = row["idCaserne"].ToString() + "-" + row["codeTypeEngin"].ToString() + "-" + row["numero"].ToString();
                }
            }
            else
            {
                lbl_numid.Text = "Aucun";
            }
        }

        private void AfficherDateRecep()
        {
            if (bs.Current is DataRowView row)
            {
                if (row.IsNew)
                {
                    lbl_daterecep.Text = "Nouveau";
                }
                else
                {
                    lbl_daterecep.Text = row["dateReception"].ToString();
                }
            }
            else
            {
                lbl_daterecep.Text = "Aucune";
            }
        }

        private void AfficherMissionPanne()
        {
            if (bs.Current is DataRowView row)
            {
                if (row.IsNew)
                {
                    ckb_mission.Checked = false;
                    ckb_panne.Checked = false;
                }
                else
                {
                    ckb_mission.Checked = row["enMission"].ToString() == "1";
                    ckb_panne.Checked = row["enPanne"].ToString() == "1";
                }
            }
            else
            {
                ckb_mission.Checked = false;
                ckb_panne.Checked = false;
            }

        }
        private void cb_ChoixCaserne_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Afficher tout si "Toutes" est sélectionné
            if (cb_ChoixCaserne.SelectedIndex == 0)
            {
                bs.RemoveFilter();
                return;
            }

            string nomCaserne = cb_ChoixCaserne.SelectedItem.ToString();
            DataTable caserneTable = MesDatas.DsGlobal.Tables["Caserne"];
            DataRow[] found = caserneTable.Select($"nom = '{nomCaserne.Replace("'", "''")}'");
            if (found.Length > 0)
            {
                string idCaserne = found[0]["id"].ToString();
                // Si idCaserne est un entier dans Engin, retire les quotes
                bs.Filter = $"idCaserne = {idCaserne}";
            }
            else
            {
                bs.RemoveFilter();
            }
            AfficherImageCourante();
            AfficherNumID();
            AfficherDateRecep();
            AfficherMissionPanne();
        }

        private void btn_retour_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void pbxLogo_Click(object sender, EventArgs e)
        {

        }

        private void btn_retour_Click(object sender, EventArgs e)
        {

        }
    }
}