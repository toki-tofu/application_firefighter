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
    public partial class volet2 : Form
    {
        public volet2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        public volet2(string login, string mdp)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            txtlogin.Text = login;
            txtmdp.Text = mdp;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            while (progressBar1.Value != 100)
            {
                progressBar1.Value += 1;
                System.Threading.Thread.Sleep(10);
            }
            // fermer le formulaire
            this.DialogResult = DialogResult.OK;
            this.Close();

        }
        public string renvoi_login
        {
            get { return txtlogin.Text; }
            set { txtlogin.Text = value; }
        }
        public string renvoi_mdp
        {
            get { return txtmdp.Text; }
            set { txtmdp.Text = value; }
        }


    }
}
