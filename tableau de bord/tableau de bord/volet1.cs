using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace tableau_de_bord
{
    public partial class volet1 : Form
    {
        private SQLiteConnection cx;
        private DataSet ds = new DataSet();
        public volet1()
        {
            string fichierDb = Directory.GetFiles(".", "*.db").FirstOrDefault();
            InitializeComponent();
            string chaine = $"Data Source={fichierDb}";
            cx = new SQLiteConnection(chaine);
            cx.Open();
        }

        private void volet1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Location = new Point(0, 0);
            string query = "SELECT nom FROM Caserne";
            using (SQLiteCommand cmd = new SQLiteCommand(query, cx))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nomCaserne = reader["nom"].ToString();
                        cmbcaserne.Items.Add(nomCaserne);
                        cmbrattachement.Items.Add(nomCaserne);
                    }
                }
            }
            btninfo.FlatStyle = FlatStyle.Flat;
            btninfo.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            btninfo.FlatAppearance.BorderSize = 1;
            btninfo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            btninfo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Navy;
            btninfo.Cursor = Cursors.Hand;
        }
        private void cmbcaserne_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnconfirmation.Visible = false;
            //charger dans la cmbpompier les nom + prenom qui se trouvent dans la table Pompier , pour savoir qui est dans la caserne selectionnée il faut regarder quel id_caserne est dans la table Affectation et recupere les pompier dont le matricule figure dans la données 
            string selectedCaserne = cmbcaserne.SelectedItem.ToString();
            string query = "SELECT P.nom, P.prenom FROM Pompier P " +
                           "JOIN Affectation A ON P.matricule = A.matriculePompier " +
                           "JOIN Caserne C ON A.idCaserne = C.id " +
                           "WHERE C.nom = @nomCaserne AND  A.dateFin IS NULL";
            using (SQLiteCommand cmd = new SQLiteCommand(query, cx))
            {
                cmd.Parameters.AddWithValue("@nomCaserne", selectedCaserne);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    cmbpompier.Items.Clear(); // Clear previous items
                    while (reader.Read())
                    {
                        string nomPompier = reader["nom"].ToString();
                        string prenomPompier = reader["prenom"].ToString();
                        cmbpompier.Items.Add($"{nomPompier} {prenomPompier}");
                    }
                }
            }
        }

        private void cmbpompier_SelectedIndexChanged(object sender, EventArgs e)
        {
            //charge les données dans les labels dont les noms correspondent au nom des champs de la table Pompier uniquement celle du pompier selectionné
            grpbInfoModif.Visible = false;
            string selectedPompier = cmbpompier.SelectedItem.ToString();
            string query = "SELECT * FROM Pompier WHERE nom || ' ' || prenom = @nomComplet";
            using (SQLiteCommand cmd = new SQLiteCommand(query, cx))
            {
                cmd.Parameters.AddWithValue("@nomComplet", selectedPompier);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblmatricule1.Text = reader["matricule"].ToString();
                        lblmatricule2.Text = reader["matricule"].ToString();
                        lblnom1.Text = reader["nom"].ToString();
                        lblprenom1.Text = reader["prenom"].ToString();
                        // si sexe == m alors mettre lblsexe1.text = "Masculin" sinon mettre "Féminin"
                        if (reader["sexe"].ToString() == "m")
                        {
                            lblsexe1.Text = "Masculin";
                        }
                        else
                        {
                            lblsexe1.Text = "Féminin";
                        }
                        lblnaissance1.Text = reader["dateNaissance"].ToString();
                        if (reader["type"].ToString() == "P")
                        {
                            rdbprofessionnel.Checked = true;
                        }
                        else
                        {
                            rdbvolontaire.Checked = true;
                        }
                        lblembauche1.Text = reader["dateEmbauche"].ToString();
                        lblgrade1.Text = reader["codeGrade"].ToString();
                        //chercher une image dans le dossier image avec le nom du grade et l'afficher dans la picturebox avec un chemin relatif
                        string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImagesGrades", reader["codeGrade"].ToString() + ".png");

                        pcbgrade.Image = Image.FromFile(imagePath);

                        lblbip1.Text = reader["bip"].ToString();
                        lbltelephone1.Text = reader["portable"].ToString();
                        cmbrattachement.SelectedItem = cmbcaserne.SelectedItem;
                        if (reader["enConge"].ToString() == "1")
                        {
                            chkconge.Checked = true;
                        }
                        else
                        {
                            chkconge.Checked = false;
                        }
                        if (reader["enMission"].ToString() == "1")
                        {
                            chkmission.Checked = true;
                        }
                        else
                        {
                            chkmission.Checked = false;
                        }
                        string query2 = "SELECT H.libelle FROM Habilitation H " +
                                        "JOIN Passer P ON H.id = P.idHabilitation " +
                                        "JOIN Pompier Pom ON P.matriculePompier = Pom.matricule " +
                                        "WHERE Pom.matricule = @matricule";
                        using (SQLiteCommand cmd2 = new SQLiteCommand(query2, cx))
                        {
                            cmd2.Parameters.AddWithValue("@matricule", reader["matricule"].ToString());
                            using (SQLiteDataReader reader2 = cmd2.ExecuteReader())
                            {
                                lstbhabilitations.Items.Clear();
                                while (reader2.Read())
                                {
                                    string nomHabilitation = reader2["libelle"].ToString();
                                    lstbhabilitations.Items.Add(nomHabilitation);
                                }
                            }
                        }
                        string query3 = "SELECT C.nom, A.dateFin FROM Affectation A " +
                                        "JOIN Caserne C ON A.idCaserne = C.id " +
                                        "WHERE A.matriculePompier = @matricule";
                        using (SQLiteCommand cmd3 = new SQLiteCommand(query3, cx))
                        {
                            cmd3.Parameters.AddWithValue("@matricule", reader["matricule"].ToString());
                            using (SQLiteDataReader reader3 = cmd3.ExecuteReader())
                            {
                                lstbaffectations.Items.Clear();
                                while (reader3.Read())
                                {
                                    string nomCaserne = reader3["nom"].ToString();
                                    string dateFin = reader3["dateFin"].ToString();
                                    lstbaffectations.Items.Add($"Caserne : {nomCaserne} - Date de fin : {dateFin}");
                                }
                            }
                        }

                    }
                }
            }
            //si le truc selectionné est null groupebox1.Visible = false; et groupbox2.Visible = false;
            if (cmbpompier.SelectedItem == null)
            {
                grpbInfoFixe.Visible = false;
                groupBox2.Visible = false;
            }
            else
            {
                grpbInfoFixe.Visible = true;
            }
        }

        private void btninfo_Click(object sender, EventArgs e)
        {
            volet2 co = new volet2();
            //show dialog ce form puis recuper login et mdp avec des get set et les test
            co.ShowDialog();

            string login = co.renvoi_login;
            string mdp = co.renvoi_mdp;
            string query = "SELECT * FROM Admin WHERE login = @login AND mdp = @mdp";
            using (SQLiteCommand cmd = new SQLiteCommand(query, cx))
            {
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@mdp", mdp);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show("Login et mot de passe correct");
                        groupBox2.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Login ou mot de passe incorrect");
                    }
                }
            }

        }



        private void btnmaj_Click(object sender, EventArgs e)
        {
            //doit updater la table Pompier avec les nouvelles données qui ont été saisies dans les component de la groupbox4 et ceux de le groupebox2
            string query = "UPDATE Pompier SET nom = @nom, prenom = @prenom, sexe = @sexe, dateNaissance = @dateNaissance," +
                " type = @type, dateEmbauche = @dateEmbauche, codeGrade = @codeGrade, bip = @bip, portable = @portable , enMission=@mission , enConge=@conge WHERE matricule = @matricule";
            using (SQLiteCommand cmd = new SQLiteCommand(query, cx))
            {
                cmd.Parameters.AddWithValue("@nom", txtnom2.Text);
                cmd.Parameters.AddWithValue("@prenom", txtprenom2.Text);
                cmd.Parameters.AddWithValue("@sexe", comboBox1.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@dateNaissance", txtnaissance2.Text);
                if (rdbprofessionnel2.Checked)
                {
                    cmd.Parameters.AddWithValue("@type", "p");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@type", "v");
                }
                cmd.Parameters.AddWithValue("@dateEmbauche", txtembauche2.Text);
                cmd.Parameters.AddWithValue("@codeGrade", txtgrade2.Text);
                cmd.Parameters.AddWithValue("@bip", txtbip2.Text);
                cmd.Parameters.AddWithValue("@portable", txttel2.Text);
                cmd.Parameters.AddWithValue("@matricule", lblmatricule1.Text);
                if (chkmission.Checked)
                {
                    cmd.Parameters.AddWithValue("@mission", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@mission", 0);
                }
                if (chkconge.Checked)
                {
                    cmd.Parameters.AddWithValue("@conge", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@conge", 0);
                }
                cmd.ExecuteNonQuery();
                string query2 = "UPDATE Affectation SET idCaserne = (SELECT id FROM Caserne WHERE nom = @nomCaserne) WHERE matriculePompier = @matricule";
                using (SQLiteCommand cmd2 = new SQLiteCommand(query2, cx))
                {
                    cmd2.Parameters.AddWithValue("@nomCaserne", cmbrattachement.SelectedItem.ToString());
                    cmd2.Parameters.AddWithValue("@matricule", lblmatricule1.Text);
                    cmd2.ExecuteNonQuery();
                }
                string query3 = "DELETE FROM Passer WHERE matriculePompier = @matricule";
                using (SQLiteCommand cmd3 = new SQLiteCommand(query3, cx))
                {
                    cmd3.Parameters.AddWithValue("@matricule", lblmatricule1.Text);
                    cmd3.ExecuteNonQuery();
                }
                foreach (var item in lstbhabilitations.Items)
                {
                    string queryInsert = "INSERT INTO Passer (matriculePompier, idHabilitation) " +
                                         "VALUES (@matricule, (SELECT id FROM Habilitation WHERE libelle = @libelle))";
                    using (SQLiteCommand cmdInsert = new SQLiteCommand(queryInsert, cx))
                    {
                        cmdInsert.Parameters.AddWithValue("@matricule", lblmatricule1.Text);
                        cmdInsert.Parameters.AddWithValue("@libelle", item.ToString());
                        cmdInsert.ExecuteNonQuery();
                    }
                }
                System.Threading.Thread.Sleep(2000);
                MessageBox.Show("Modification effectuée");
                groupBox2.Visible = false;
                grpbInfoModif.Visible = false;
                btnmaj.Visible = false;
                cmbpompier.SelectedItem = "";
                cmbcaserne.SelectedItem = "";
                cmbpompier.Items.Clear();
            }
        }

        private void pcbnewfireman_Click(object sender, EventArgs e)
        {

            volet2 co = new volet2();
            //show dialog ce form puis recuper login et mdp avec des get set et les test
            co.ShowDialog();

            string login = co.renvoi_login;
            string mdp = co.renvoi_mdp;
            string query = "SELECT * FROM Admin WHERE login = @login AND mdp = @mdp";
            using (SQLiteCommand cmd = new SQLiteCommand(query, cx))
            {
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@mdp", mdp);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        MessageBox.Show("Login et mot de passe correct");
                        //ouvrir un nouveau volet qui contient une serie de groupe box qui recupere toutes les informations du nouveau pompier qui une fois toutes les informations recupérée essai de creer un pompier dans la base de données
                        //ouvrir le formulaire de creation de pompier
                        volet3 nf = new volet3();
                        nf.ShowDialog();
                        //recuperer les données du pompier
                        string nom = nf.renvoi_nom;
                        string prenom = nf.renvoi_prenom;
                        string sexe = nf.renvoi_sexe;
                        //si sexe = masculin alors sexe = m sinon sexe = f
                        if (sexe == "Masculin")
                        {
                            sexe = "m";
                        }
                        else
                        {
                            sexe = "f";
                        }
                        string dateNaissance = nf.renvoi_naissance;
                        string type = nf.renvoi_type;
                        //si type = professionnel alors type = p sinon type = v
                        if (type == "Professionnel")
                        {
                            type = "p";
                        }
                        else
                        {
                            type = "v";
                        }
                        string dateEmbauche = nf.renvoi_embauche;
                        string codeGrade = nf.renvoi_grade;
                        string bip = nf.renvoi_bip;
                        string portable = nf.renvoi_telephone;
                        string idCaserne = nf.renvoi_rattachement;
                        string enMission = "0";
                        string enConge = "0";
                        //si le formulaire fini correctement on construit la commande et on l'execute dans un try catch va y fais
                        // Récupérer le matricule maximum et incrémenter de 1
                        int nouveauMatricule = 1;
                        using (SQLiteCommand cmdMax = new SQLiteCommand("SELECT MAX(matricule) FROM Pompier", cx))
                        {
                            object result = cmdMax.ExecuteScalar();
                            if (result != DBNull.Value && result != null)
                                nouveauMatricule = Convert.ToInt32(result) + 1;
                        }

                        // Préparer la commande d'insertion
                        string insertPompier = "INSERT INTO Pompier (matricule, nom, prenom, sexe, dateNaissance, type, dateEmbauche, codeGrade, bip, portable, enMission, enConge) " +
                                               "VALUES (@matricule, @nom, @prenom, @sexe, @dateNaissance, @type, @dateEmbauche, @codeGrade, @bip, @portable, @enMission, @enConge)";
                        using (SQLiteCommand cmdInsert = new SQLiteCommand(insertPompier, cx))
                        {
                            cmdInsert.Parameters.AddWithValue("@matricule", nouveauMatricule);
                            cmdInsert.Parameters.AddWithValue("@nom", nom);
                            cmdInsert.Parameters.AddWithValue("@prenom", prenom);
                            cmdInsert.Parameters.AddWithValue("@sexe", sexe);
                            cmdInsert.Parameters.AddWithValue("@dateNaissance", dateNaissance);
                            cmdInsert.Parameters.AddWithValue("@type", type);
                            cmdInsert.Parameters.AddWithValue("@dateEmbauche", dateEmbauche);
                            cmdInsert.Parameters.AddWithValue("@codeGrade", codeGrade);
                            cmdInsert.Parameters.AddWithValue("@bip", bip);
                            cmdInsert.Parameters.AddWithValue("@portable", portable);
                            cmdInsert.Parameters.AddWithValue("@enMission", enMission);
                            cmdInsert.Parameters.AddWithValue("@enConge", enConge);

                            try
                            {
                                cmdInsert.ExecuteNonQuery();
                                // Ajouter l'affectation à la caserne
                                string insertAffectation = "INSERT INTO Affectation (matriculePompier, idCaserne, dateFin) VALUES (@matricule, (SELECT id FROM Caserne WHERE nom = @nomCaserne), NULL)";
                                using (SQLiteCommand cmdAffect = new SQLiteCommand(insertAffectation, cx))
                                {
                                    cmdAffect.Parameters.AddWithValue("@matricule", nouveauMatricule);
                                    cmdAffect.Parameters.AddWithValue("@nomCaserne", idCaserne);
                                    cmdAffect.ExecuteNonQuery();
                                }
                                MessageBox.Show("Nouveau pompier ajouté !");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Erreur lors de l'ajout : " + ex.Message);
                            }
                        }


                    }
                    else
                    {
                        MessageBox.Show("Login ou mot de passe incorrect");
                    }
                }
            }
        }

        private void btnsupp_Click(object sender, EventArgs e)
        {

        }

        private void btnconfirm_click(object sender, EventArgs e)
        {
            btnconfirmation.Visible = true;
        }

        private void txtgrade2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnplusdinfo(object sender, EventArgs e)
        {
            grpbInfoFixe.Visible = false;
            grpbInfoModif.Visible = true;
        }

        private void txttel2_TextChanged(object sender, EventArgs e)
        {

        }

        private void rdbvolontaire2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void grbcarriere_Enter(object sender, EventArgs e)
        {

        }

        private void grpbInfoFixe_Enter(object sender, EventArgs e)
        {

        }

        private void lblsexe_Click(object sender, EventArgs e)
        {

        }

        private void btnquitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pcbnewfireman_MouseHover(object sender, EventArgs e)
        {
            pcbnewfireman.BorderStyle = BorderStyle.FixedSingle;
        }

        private void pcbnewfireman_MouseLeave(object sender, EventArgs e)
        {
            pcbnewfireman.BorderStyle = BorderStyle.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cboaddh.Items.Clear();

            string query = @"
        SELECT H.libelle 
        FROM Habilitation H
        WHERE H.id NOT IN (
            SELECT H.id 
            FROM Habilitation H
            JOIN Passer P ON H.id = P.idHabilitation
            WHERE P.matriculePompier = @matricule
        )";

            using (SQLiteCommand cmd = new SQLiteCommand(query, cx))
            {
                cmd.Parameters.AddWithValue("@matricule", lblmatricule1.Text);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cboaddh.Items.Add(reader.GetString(0));
                    }
                }
            }
            cboaddh.Visible= true;
        }

        private void cboaddh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboaddh.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une habilitation.");
                return;
            }

            string libelleHabilitation = cboaddh.SelectedItem.ToString();

            int idHabilitation = -1;
            string queryId = "SELECT id FROM Habilitation WHERE libelle = @libelle";

            using (SQLiteCommand cmd = new SQLiteCommand(queryId, cx))
            {
                cmd.Parameters.AddWithValue("@libelle", libelleHabilitation);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    idHabilitation = Convert.ToInt32(result);
                }
                else
                {
                    MessageBox.Show("Habilitation non trouvée !");
                    return;
                }
            }

            string insertQuery = "INSERT INTO Passer (matriculePompier, idHabilitation) VALUES (@matricule, @idHabilitation)";

            using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, cx))
            {
                cmd.Parameters.AddWithValue("@matricule", lblmatricule1.Text);
                cmd.Parameters.AddWithValue("@idHabilitation", idHabilitation);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Habilitation ajoutée avec succès !");
            cboaddh.Visible = false;

        }
    }
}