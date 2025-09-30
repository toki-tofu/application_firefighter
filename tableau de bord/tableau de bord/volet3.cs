using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tableau_de_bord
{
    public partial class volet3 : Form
    {
        public volet3()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }
        private void btnsuivant_Click(object sender, EventArgs e)
        {
            //on verifie que le nom, le sexe , la date de naissance et le prenom ne sont pas vides
            if (txtnom.Text == "" || txtprenom.Text == "" || cmbsexe.Text == "" || txtnaissance.Text == "jj/mm/aaaa")
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                return;
            }



            //quand l'on clique sur le bouton suivant, on affiche la progress bar
            progressBar1.Visible = true;
            //on fait une boucle pour faire avancer la progress bar
            while (progressBar1.Value != 100)
            {
                progressBar1.Value += 1;
                System.Threading.Thread.Sleep(10);
            }
            //faire apparaitre groupebox2
            grpnew2.Visible = true;
            grpnew1.Visible = false;
        }


        private void txtprenom_KeyPress(object sender, KeyPressEventArgs e)
        {
            //on verifie que l'on ne peut pas ecrire de caracteres speciaux
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
            //on verifie que l'on ne peut pas ecrire de chiffres
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtnaissance_KeyPress(object sender, KeyPressEventArgs e)
        {
            //le texte commence en handle
            e.Handled = true;
            //si le texte est vide on affiche le format jj/mm/aaaa

            //si l'utilisateur entre un caractere tel qu'un chiffre ou un / ou une supprimation on autorise l'ecriture
            if (char.IsDigit(e.KeyChar) || e.KeyChar == '/' || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }


        }



        private void btnsuivant2_Click_1(object sender, EventArgs e)
        {
            //on verifie que le bip, le telephone , la date d'embauche et le type ne sont pas vides
            if (txtbip.Text == "" || txttelephone.Text == "" || txtembauche.Text == "jj/mm/aaaa" || txtembauche.Text == "")
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                return;
            }
            progressBar2.Visible = true;
            //on fait une boucle pour faire avancer la progress bar
            while (progressBar2.Value != 100)
            {
                progressBar2.Value += 1;
                System.Threading.Thread.Sleep(10);
            }
            //faire apparaitre groupebox2 <= 3 Jonathan regarde quand tu fais des copier coller ptn
            grpnew3.Visible = true;
            grpnew2.Visible = false;
        }




        private void txtbip_KeyPress(object sender, KeyPressEventArgs e)
        {
            //commence handle , si c'est un un chiffre ou un suppression on autorise l'ecriture
            e.Handled = true;
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //verifie que le  code grade et caserne rattachement ne sont pas vides puis ferme le formulaire a la fin de la progress bar3
            if (cmbgrade.Text == "" || cmbrattachement.Text == "")
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                return;
            }
            progressBar3.Visible = true;
            //on fait une boucle pour faire avancer la progress bar
            while (progressBar3.Value != 100)
            {
                progressBar3.Value += 1;
                System.Threading.Thread.Sleep(10);
            }
            //fermer le formulaire
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //on get tout les champs de texte et de combo box pour les renvoyer dans le formulaire principal
        public string renvoi_nom
        {
            get { return txtnom.Text; }
            set { txtnom.Text = value; }
        }
        public string renvoi_prenom
        {
            get { return txtprenom.Text; }
            set { txtprenom.Text = value; }
        }
        public string renvoi_sexe
        {
            get { return cmbsexe.Text; }
            set { cmbsexe.Text = value; }
        }
        public string renvoi_naissance
        {
            get { return txtnaissance.Text; }
            set { txtnaissance.Text = value; }
        }
        public string renvoi_bip
        {
            get { return txtbip.Text; }
            set { txtbip.Text = value; }
        }
        public string renvoi_telephone
        {
            get { return txttelephone.Text; }
            set { txttelephone.Text = value; }
        }
        public string renvoi_embauche
        {
            get { return txtembauche.Text; }
            set { txtembauche.Text = value; }
        }
        public string renvoi_grade
        {
            get { return cmbgrade.Text; }
            set { cmbgrade.Text = value; }
        }
        public string renvoi_rattachement
        {
            get { return cmbrattachement.Text; }
            set { cmbrattachement.Text = value; }
        }
        public string renvoi_type
        {
            //renvoi uniquement le premier caractere de cmbtype
            get { return cmbtype.Text; }
            set { cmbtype.Text = value; }
        }

        private void txtnaissance_Enter(object sender, EventArgs e)
        {
            txtnaissance.Text = "";
        }

        private void txtnaissance_Leave(object sender, EventArgs e)
        {
            if (txtnaissance.Text == "")
            {
                txtnaissance.Text = "jj/mm/aaaa";
                txtnaissance.SelectionStart = txtnaissance.Text.Length;
            }
        }
        private void txtembauche_enter(object sender, EventArgs e)
        {
            txtembauche.Text = "";
        }

        private void txtembauche_Leave(object sender, EventArgs e)
        {
            if (txtembauche.Text == "")
            {
                txtembauche.Text = "jj/mm/aaaa";
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void volet3_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;
            btnsuivant.FlatStyle = FlatStyle.Flat;
            btnsuivant.FlatAppearance.BorderSize = 1;
            btnsuivant.FlatAppearance.MouseDownBackColor = Color.DarkViolet;
            btnsuivant.FlatAppearance.MouseOverBackColor = Color.MediumVioletRed;
            btnsuivant.Cursor = Cursors.Hand;
            btnsuivant2.FlatStyle = FlatStyle.Flat;
            btnsuivant2.FlatAppearance.BorderSize = 1;
            btnsuivant2.FlatAppearance.MouseDownBackColor = Color.DarkViolet;
            btnsuivant2.FlatAppearance.MouseOverBackColor = Color.MediumVioletRed;
            btnsuivant2.Cursor = Cursors.Hand;
        }
    }
}
