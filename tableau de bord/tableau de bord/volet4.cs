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
    public partial class volet4 : Form
    {
        private SQLiteConnection cx;
        private List<Tuple<int, int>> affectationsPompiers = new List<Tuple<int, int>>(); // (matricule, idHabilitation)
        string dbPath;
        string fichierDb;
        public volet4()
        {
            InitializeComponent();
        }

        private void volet4_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Size = new Size(800, 894);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            btnValider.Visible = false;
            //remplissage d'un dataSet avec toutes les tables de la base de données
            try
            {
                cx = new SQLiteConnection(); 
                fichierDb = Directory.GetFiles(".", "*.db").FirstOrDefault();
                dbPath = $"Data Source={fichierDb}";
                cx.ConnectionString = dbPath;
                cx.Open();
            }
            catch (Exception erreur)
            {
                MessageBox.Show(erreur.ToString());
            }
            DataSet ds = new DataSet();
            DataTable schema = cx.GetSchema("Tables");
            foreach (DataRow row in schema.Rows)
            {
                string tableName = row["TABLE_NAME"].ToString();
                string requete = $"SELECT * FROM [{tableName}]";
                using (var da = new SQLiteDataAdapter(requete, cx))
                {
                    da.Fill(ds, tableName);
                }
            }
            cx.Close();

            // Initialisation des labels d'engins
            lblEngins1.Text = "";
            lblEngin2.Text = "";
            lblEngin3.Text = "";
            lblEngin4.Text = "";
            lblEngin5.Text = "";
            lblEngin6.Text = "";
            lblEngin7.Text = "";
            lblEngin8.Text = "";
            lblEngin9.Text = "";
            lblEngin10.Text = "";
            lblPompiers.Text = "";
            lblPompiers2.Text = "";

            // affichage de la date et de l'heure
            lblDate.Text = "Date : \n" + DateTime.Now.ToString();

            // affichage du numéro de mission
            int nbMissionMax = 0;
            foreach (DataRow dr in ds.Tables["Mission"].Rows)
            {
                if (Convert.ToInt32(dr["id"]) > nbMissionMax)
                {
                    nbMissionMax = Convert.ToInt32(dr["id"]);
                }
            }
            lblNumMission.Text = "Mission numéro " + (nbMissionMax + 1).ToString();


            // Remplissage de la ComboBox Nature
            cboNature.Items.Clear();
            foreach (DataRow row in ds.Tables["NatureSinistre"].Rows)
            {
                cboNature.Items.Add(row["libelle"].ToString());
            }


            // Remplissage de la ComboBox Caserne
            cboCaserne.Items.Clear();
            foreach (DataRow row in ds.Tables["Caserne"].Rows)
            {
                cboCaserne.Items.Add(row["nom"].ToString());
            }
        }

        private void cboNature_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboCaserne_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnEkip_Click(object sender, EventArgs e)
        {
            lblEngins1.Text = "";
            lblEngin2.Text = "";
            lblEngin3.Text = "";
            lblEngin4.Text = "";
            lblEngin5.Text = "";
            lblPompiers.Text = "";
            lblPompiers2.Text = "";
            flpEngins.Controls.Clear();

            // Vérification des champs
            if (txtRue.Text == "")
            {
                MessageBox.Show("adresse invalide");
                txtRue.Focus();
                return;
            }
            if (txtCodePostal.Text == "")
            {
                MessageBox.Show("adresse invalide");
                txtCodePostal.Focus();
                return;
            }
            if (txtVille.Text == "")
            {
                MessageBox.Show("adresse invalide");
                txtVille.Focus();
                return;
            }
            if (cboCaserne.Text == "")
            {
                MessageBox.Show("caserne : champs obligatoire");
                cboCaserne.Focus();
                return;
            }
            if (cboNature.Text == "")
            {
                MessageBox.Show("nature de sinistre : champs obligatoire");
                cboNature.Focus();
                return;
            }

            //remplissage d'un dataSet avec toutes les tables de la base de données
            try
            {
                cx = new SQLiteConnection();
                string chcon = @"Data Source = SDIS67.db";
                cx.ConnectionString = chcon;
                cx.Open();
            }
            catch (Exception erreur)
            {
                MessageBox.Show(erreur.ToString());
            }
            DataSet ds = new DataSet();
            DataTable schema = cx.GetSchema("Tables");
            foreach (DataRow row in schema.Rows)
            {
                string tableName = row["TABLE_NAME"].ToString();
                string requete = $"SELECT * FROM [{tableName}]";
                using (var da = new SQLiteDataAdapter(requete, cx))
                {
                    da.Fill(ds, tableName);
                }
            }
            cx.Close();

            // affichage de la date et de l'heure
            lblDate.Text = "Date : \n" + DateTime.Now.ToString();

            // affichage du numéro de mission
            int nbMissionMax = 0;
            foreach (DataRow dr in ds.Tables["Mission"].Rows)
            {
                if (Convert.ToInt32(dr["id"]) > nbMissionMax)
                {
                    nbMissionMax = Convert.ToInt32(dr["id"]);
                }
            }
            lblNumMission.Text = "Mission numéro " + (nbMissionMax + 1).ToString();

            int missionValide = 0; // servira pour afficher le bon message
            // 0 : Aucun problème
            // 1 : engins non-disponibles, affectation à une autre caserne
            // 2 : pompiers non-disponibles, mission générée en sous-effectif

            // Recherche des engins nécessaires pour la mission
            DataTable necessite = ds.Tables["Necessiter"];
            string nature = cboNature.Text;
            int idNature = ds.Tables["NatureSinistre"].AsEnumerable()
                .Where(r => r.Field<string>("libelle") == nature)
                .Select(r => Convert.ToInt32(r["id"]))
                .FirstOrDefault();
            for (int i = 0; i < necessite.Rows.Count; i++)
            {
                if (Convert.ToInt32(necessite.Rows[i]["idNatureSinistre"]) != idNature)
                {
                    necessite.Rows[i].Delete();
                }
            }


            // Recherche des engins disponibles dans la caserne
            DataTable engins = ds.Tables["Engin"];
            string caserne = cboCaserne.Text;
            int idCaserne = ds.Tables["Caserne"].AsEnumerable()
                .Where(r => r.Field<string>("nom") == caserne)
                .Select(r => Convert.ToInt32(r["id"]))
                .FirstOrDefault();
            for (int i = 0; i < engins.Rows.Count; i++)
            {
                bool delete = false;
                if (Convert.ToInt32(engins.Rows[i]["idCaserne"]) != idCaserne)
                {
                    delete = true;
                }
                if (Convert.ToInt32(engins.Rows[i]["enMission"]) == 1)
                {
                    delete = true;
                }
                if (delete == true)
                {
                    engins.Rows[i].Delete();
                }
            }

            // Recherche des engins disponibles dans la caserne et nécessaires pour la mission
            DataTable enginsChoisis = engins.Clone();
            for (int i = 0; i < necessite.Rows.Count; i++)
            {
                if (necessite.Rows[i].RowState != DataRowState.Deleted)
                {
                    int nbEnginsNecessaires = Convert.ToInt32(necessite.Rows[i]["nombre"]);
                    while (nbEnginsNecessaires > 0 && missionValide == 0)
                    {
                        bool trouve = false;
                        for (int j = 0; j < engins.Rows.Count; j++)
                        {
                            if (engins.Rows[j].RowState != DataRowState.Deleted)
                            {
                                if (engins.Rows[j]["codeTypeEngin"].ToString() == necessite.Rows[i]["codeTypeEngin"].ToString())
                                {
                                    enginsChoisis.ImportRow(engins.Rows[j]);
                                    engins.Rows[j]["enMission"] = 1;
                                    engins.Rows[j].Delete();
                                    trouve = true;
                                    nbEnginsNecessaires--;
                                    break;
                                }
                            }
                        }
                        if (trouve == false)
                        {
                            missionValide = 1;
                        }
                    }
                }
            }

            // Recherche des pompiers disponibles dans la caserne et rangement dans un DataTable
            DataTable pompiers = ds.Tables["Pompier"];
            DataTable affectation = ds.Tables["Affectation"];
            DataTable pompiersDisponibles = pompiers.Clone();

            foreach (DataRow aff in affectation.Rows)
            {
                if (aff.RowState != DataRowState.Deleted && Convert.ToInt32(aff["idCaserne"]) == idCaserne)
                {
                    int matricule = Convert.ToInt32(aff["matriculePompier"]);
                    DataRow[] pompiersTrouves = pompiers.Select(
                        "matricule = " + matricule + " AND enMission = 0 AND enConge = 0"
                    );
                    foreach (DataRow pompier in pompiersTrouves)
                    {
                        if (pompier.RowState != DataRowState.Deleted)
                        {
                            pompiersDisponibles.ImportRow(pompier);
                        }
                    }
                }
            }

            // Recherche des habilitations nécessaires et du nombre de pompier par habilitation pour chaque véhicule de enginsChoisis
            DataTable habilitationsNecessaires = ds.Tables["Embarquer"];
            DataTable habilitations = ds.Tables["Habilitation"];
            int[] nbPompierHabilitation = new int[15];
            foreach (DataRow habilitationNecessaire in habilitationsNecessaires.Rows)
            {
                if (habilitationNecessaire.RowState != DataRowState.Deleted)
                {
                    string codeTypeEngin = habilitationNecessaire["codeTypeEngin"].ToString();
                    foreach (DataRow engin in enginsChoisis.Rows)
                    {
                        if (engin.RowState != DataRowState.Deleted)
                        {
                            if (codeTypeEngin == engin["codeTypeEngin"].ToString())
                            {
                                nbPompierHabilitation[Convert.ToInt32(habilitationNecessaire["idHabilitation"]) - 1] += Convert.ToInt32(habilitationNecessaire["nombre"]);
                            }
                        }
                    }
                }
            }

            // Table des pompiers nécessaires pour chaque habilitation
            DataTable pompiersNecessaires = pompiers.Clone();
            DataTable passer = ds.Tables["Passer"];

            for (int i = 0; i < nbPompierHabilitation.Length; i++)
            {
                int idHabilitation = i + 1;
                int nbNecessaires = nbPompierHabilitation[i];

                if (nbNecessaires > 0)
                {
                    var pompiersAvecHabilitation = from p in pompiersDisponibles.AsEnumerable()
                                                   join pa in passer.AsEnumerable()
                                                   on Convert.ToInt32(p["matricule"]) equals Convert.ToInt32(pa["matriculePompier"])
                                                   where Convert.ToInt32(pa["idHabilitation"]) == idHabilitation
                                                   select p;
                    int j = 0;
                    foreach (var pompier in pompiersAvecHabilitation)
                    {
                        if (j >= nbNecessaires)
                        {
                            break;
                        }
                        // Vérification de l'unicité du matricule
                        var matricule = pompier["matricule"].ToString();
                        bool dejaAjoute = pompiersNecessaires.AsEnumerable()
                            .Any(row => row["matricule"].ToString() == matricule);
                        if (!dejaAjoute)
                        {
                            pompiersNecessaires.ImportRow(pompier);
                            affectationsPompiers.Add(Tuple.Create(Convert.ToInt32(pompier["matricule"]), idHabilitation));
                            j++;
                        }
                    }
                    if (j < nbNecessaires)
                    {
                        missionValide = 2;
                    }
                }
            }

            // Affichage amélioré
            // Supposons que flpEngins est un FlowLayoutPanel ajouté au formulaire
            flpEngins.Controls.Clear();
            flpEngins.WrapContents = false; // Pour un affichage vertical, sinon true pour horizontal
            flpEngins.FlowDirection = FlowDirection.TopDown; // Pour une colonne verticale

            string[] images = new string[5];
            int numImage = 0;
            foreach (DataRow engin in enginsChoisis.Rows)
            {
                string codeTypeEngin = engin["codeTypeEngin"].ToString();
                images[numImage] = "img/" + codeTypeEngin + ".png";
                if (numImage == 0)
                {
                    lblEngins1.Text = "\n" + codeTypeEngin + " " + engin["numero"];
                }
                if (numImage == 1)
                {
                    lblEngin2.Text = "\n" + codeTypeEngin + " " + engin["numero"];
                }
                if (numImage == 2)
                {
                    lblEngin3.Text = "\n" + codeTypeEngin + " " + engin["numero"];
                }
                if (numImage == 3)
                {
                    lblEngin4.Text = "\n" + codeTypeEngin + " " + engin["numero"];
                }
                if (numImage == 4)
                {
                    lblEngin5.Text = "\n" + codeTypeEngin + " " + engin["numero"];
                }
                if (numImage == 5)
                {
                    lblEngin6.Text = "\n" + codeTypeEngin + " " + engin["numero"];
                }
                if (numImage == 6)
                {
                    lblEngin7.Text = "\n" + codeTypeEngin + " " + engin["numero"];
                }
                if (numImage == 7)
                {
                    lblEngin8.Text = "\n" + codeTypeEngin + " " + engin["numero"];
                }
                if (numImage == 8)
                {
                    lblEngin9.Text = "\n" + codeTypeEngin + " " + engin["numero"];
                }
                if (numImage == 9)
                {
                    lblEngin10.Text = "\n" + codeTypeEngin + " " + engin["numero"];
                }
                numImage++;
            }

            foreach (var imgPath in images)
            {
                if (imgPath != null)
                {
                    PictureBox pb = new PictureBox();
                    pb.Image = Image.FromFile(imgPath);
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Width = flpEngins.ClientSize.Width; // Occupe toute la largeur
                    pb.Height = 66; // Hauteur fixe, à ajuster selon vos besoins
                    pb.Margin = new Padding(0); // Pas d'espacement entre les images
                    flpEngins.Controls.Add(pb);
                }
            }

            lblPompiers.Text = "";
            lblPompiers2.Text = "";
            int nbPompiers = 0;
            foreach (DataRow pompier in pompiersNecessaires.Rows)
            {
                if (nbPompiers < 10)
                {
                    lblPompiers.Text += pompier["matricule"] + " :    "
                        + pompier["nom"] + " " + pompier["prenom"] + " (" + pompier["codeGrade"] + ")\n\n\n";
                }
                else
                {
                    lblPompiers2.Text += pompier["matricule"] + " :    "
                        + pompier["nom"] + " " + pompier["prenom"] + " (" + pompier["codeGrade"] + ")\n\n\n";
                }
                nbPompiers++;
            }

            btnValider.Visible = true;
            btnAnnuler.Location = new Point(1401, 784);
            btnAnnuler.Text = "Annuler";
            grpResultatEngins.Visible = true;
            grpResultatPompiers.Visible = true;
            volet4.ActiveForm.Size = new Size(1858, 894);

            if (missionValide == 0)
            {
                MessageBox.Show("Mission générée");
            }
            if (missionValide == 1)
            {
                MessageBox.Show("Engins non-disponibles, affectation à une autre caserne");
            }
            if (missionValide == 2)
            {
                MessageBox.Show("Pompiers non-disponibles, mission générée en sous-effectif");
            }
        }

        private void btnAnnuler_Click_1(object sender, EventArgs e)
        {
            try
            {
                cx = new SQLiteConnection();
                string chcon = @"Data Source = SDIS67.db";
                cx.ConnectionString = chcon;
                cx.Open();
            }
            catch (Exception erreur)
            {
                MessageBox.Show(erreur.ToString());
            }
            DataSet ds = new DataSet();
            DataTable schema = cx.GetSchema("Tables");
            foreach (DataRow row in schema.Rows)
            {
                string tableName = row["TABLE_NAME"].ToString();
                string requete = $"SELECT * FROM [{tableName}]";
                using (var da = new SQLiteDataAdapter(requete, cx))
                {
                    da.Fill(ds, tableName);
                }
            }
            cx.Close();
            cboCaserne.Text = "";
            cboNature.Text = "";
            rtxtDetails.Text = "";
            txtRue.Text = "";
            txtCodePostal.Text = "";
            txtVille.Text = "";
            lblEngins1.Text = "";
            lblEngin2.Text = "";
            lblEngin3.Text = "";
            lblEngin4.Text = "";
            lblEngin5.Text = "";
            lblEngin6.Text = "";
            lblEngin7.Text = "";
            lblEngin8.Text = "";
            lblEngin9.Text = "";
            lblEngin10.Text = "";
            lblPompiers.Text = "";
            lblPompiers2.Text = "";
            flpEngins.Controls.Clear();
            grpResultatEngins.Visible = false;
            grpResultatPompiers.Visible = false;
            btnAnnuler.Location = new Point(657, 784);
            btnAnnuler.Text = "Effacer";
            btnValider.Visible = false;
            volet4.ActiveForm.Size = new Size(800, 894);
            // affichage de la date et de l'heure
            lblDate.Text = "Date : \n" + DateTime.Now.ToString();

            // affichage du numéro de mission
            int nbMissionMax = 0;
            foreach (DataRow dr in ds.Tables["Mission"].Rows)
            {
                if (Convert.ToInt32(dr["id"]) > nbMissionMax)
                {
                    nbMissionMax = Convert.ToInt32(dr["id"]);
                }
            }
            lblNumMission.Text = "Mission numéro " + (nbMissionMax + 1).ToString();
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            //remplissage d'un dataSet avec toutes les tables de la base de données
            try
            {
                cx = new SQLiteConnection();
                string chcon = @"Data Source = SDIS67.db";
                cx.ConnectionString = chcon;
                cx.Open();
            }
            catch (Exception erreur)
            {
                MessageBox.Show(erreur.ToString());
            }
            DataSet ds = new DataSet();
            DataTable schema = cx.GetSchema("Tables");
            foreach (DataRow row in schema.Rows)
            {
                string tableName = row["TABLE_NAME"].ToString();
                string requete = $"SELECT * FROM [{tableName}]";
                using (var da = new SQLiteDataAdapter(requete, cx))
                {
                    da.Fill(ds, tableName);
                }
            }
            cx.Close();

            // Enregistrement des informations de la mission
            int idMission;
            if (lblNumMission.Text.Length > 16)
            {
                idMission = Convert.ToInt32(lblNumMission.Text.Substring(15, 2));
            }
            else
            {
                idMission = Convert.ToInt32(lblNumMission.Text.Substring(15, 1));
            }
            string dateDepart = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string motifAppel = rtxtDetails.Text;
            string adresse = txtRue.Text;
            string cp = txtCodePostal.Text;
            string ville = txtVille.Text;
            int termine = 0;
            int idNatureSinistre = ds.Tables["NatureSinistre"].AsEnumerable()
                .Where(r => r.Field<string>("libelle") == cboNature.Text)
                .Select(r => Convert.ToInt32(r["id"]))
                .FirstOrDefault();
            int idCaserne = ds.Tables["Caserne"].AsEnumerable()
                .Where(r => r.Field<string>("nom") == cboCaserne.Text)
                .Select(r => Convert.ToInt32(r["id"]))
                .FirstOrDefault();

            string[] typesEngins = new string[10];
            int[] numerosEngins = new int[10];

            // Enregistrement de la mission dans la base de données
            string requeteInsertMission = $"INSERT INTO Mission (id, dateHeureDepart, dateHeureRetour, motifAppel, adresse, cp, ville, terminee, compteRendu, idNatureSinistre, idCaserne) " +
                $"VALUES ({idMission}, '{dateDepart}', null, '{motifAppel}', '{adresse}', '{cp}', '{ville}', {termine}, null, {idNatureSinistre}, {idCaserne})";
            string[] requeteUpdateEngins = new string[10];
            string[] requeteInsertPartirAvec = new string[10];
            int nb_engins = 0;
            for (int i = 0; i < 10; i++)
            {
                if (typesEngins[i] != "" || numerosEngins[i] != 0)
                {
                    nb_engins++;
                    requeteUpdateEngins[i] = $"UPDATE Engin SET EnMission = 1 WHERE idCaserne = {idCaserne} AND codeTypeEngin = '{typesEngins[i]}' AND numero = {numerosEngins[i]}";
                }
            }
            List<int> matriculesPompiers = new List<int>();
            Label[] pompierLabels = new Label[] { lblPompiers, lblPompiers2 };
            foreach (var lbl in pompierLabels)
            {
                string[] lignes = lbl.Text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var ligne in lignes)
                {
                    var parts = ligne.Split(':');
                    if (parts.Length > 0)
                    {
                        int matricule;
                        if (int.TryParse(parts[0].Trim(), out matricule))
                        {
                            matriculesPompiers.Add(matricule);
                        }
                    }
                }
            }
            try
            {
                cx = new SQLiteConnection();
                string chcon = @"Data Source = SDIS67.db";
                cx.ConnectionString = chcon;
                cx.Open();
                SQLiteCommand cmd = new SQLiteCommand(requeteInsertMission, cx);
                cmd.ExecuteNonQuery();
                for (int i = 0; i < nb_engins; i++)
                {
                    // Tableau des labels d'engins
                    Label[] enginLabels = new Label[] {
                        lblEngins1, lblEngin2, lblEngin3, lblEngin4, lblEngin5,
                        lblEngin6, lblEngin7, lblEngin8, lblEngin9, lblEngin10
                    };
                    for (int j = 0; j < enginLabels.Length; j++)
                    {
                        string labelText = enginLabels[j].Text.Trim();
                        if (labelText.Length > 0)
                        {
                            labelText = labelText.Replace("\n", "").Trim();
                            string[] parts = labelText.Split(' ');
                            if (parts.Length == 2)
                            {
                                typesEngins[j] = parts[0];
                                int.TryParse(parts[1], out numerosEngins[j]);
                            }
                            else
                            {
                                typesEngins[j] = "";
                                numerosEngins[j] = 0;
                            }
                        }
                        else
                        {
                            typesEngins[j] = "";
                            numerosEngins[j] = 0;
                        }
                    }
                    requeteUpdateEngins[i] = $"UPDATE Engin SET EnMission = 1 WHERE idCaserne = {idCaserne} AND codeTypeEngin = '{typesEngins[i]}' AND numero = {numerosEngins[i]}";
                    SQLiteCommand cmdUpdate = new SQLiteCommand(requeteUpdateEngins[i], cx);
                    cmdUpdate.ExecuteNonQuery();
                    if (!(string.IsNullOrEmpty(typesEngins[i])) || !(numerosEngins[i] == 0))
                    {
                        requeteInsertPartirAvec[i] = $"INSERT INTO PartirAvec (idCaserne, codeTypeEngin, numeroEngin, idMission, reparationsEventuelles) " +
                        $"VALUES ({idCaserne}, '{typesEngins[i]}', {numerosEngins[i]}, {idMission}, null)";
                        SQLiteCommand cmdInsertPartirAvec = new SQLiteCommand(requeteInsertPartirAvec[i], cx);
                        cmdInsertPartirAvec.ExecuteNonQuery();
                    }
                }
                foreach (var matricule in matriculesPompiers)
                {
                    string requeteUpdatePompier = $"UPDATE Pompier SET enMission = 1 WHERE matricule = {matricule}";
                    SQLiteCommand cmdUpdatePompier = new SQLiteCommand(requeteUpdatePompier, cx);
                    cmdUpdatePompier.ExecuteNonQuery();
                }
                foreach (var affectation in affectationsPompiers)
                {
                    int matricule = affectation.Item1;
                    int idHabilitation = affectation.Item2;
                    string requeteInsertMobiliser = $"INSERT INTO Mobiliser (matriculePompier, idMission, idHabilitation)" +
                        $" VALUES ({matricule}, {idMission}, {idHabilitation});";
                    SQLiteCommand cmdUpdatePompier = new SQLiteCommand(requeteInsertMobiliser, cx);
                    cmdUpdatePompier.ExecuteNonQuery();
                }
                cx.Close();
                MessageBox.Show("Mission enregistrée");
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur de connexion à la base");
            }
            btnValider.Visible = false;
        }

        private void btnRetour_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCodePostal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
            if (!char.IsControl(e.KeyChar) && txtCodePostal.Text.Length >= 5)
            {
                e.Handled = true;
            }
        }
    }
}
