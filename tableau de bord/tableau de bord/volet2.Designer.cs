namespace tableau_de_bord
{
    partial class volet2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(volet2));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_mdp = new System.Windows.Forms.Label();
            this.lbl_login = new System.Windows.Forms.Label();
            this.txtlogin = new System.Windows.Forms.TextBox();
            this.txtmdp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(119, 223);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(239, 18);
            this.progressBar1.TabIndex = 31;
            this.progressBar1.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(425, 245);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(174, 70);
            this.button1.TabIndex = 30;
            this.button1.Text = "Se connecter";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_mdp
            // 
            this.lbl_mdp.AutoSize = true;
            this.lbl_mdp.BackColor = System.Drawing.Color.Transparent;
            this.lbl_mdp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.lbl_mdp.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbl_mdp.Location = new System.Drawing.Point(71, 129);
            this.lbl_mdp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_mdp.Name = "lbl_mdp";
            this.lbl_mdp.Size = new System.Drawing.Size(120, 20);
            this.lbl_mdp.TabIndex = 29;
            this.lbl_mdp.Text = "Mot de passe :";
            // 
            // lbl_login
            // 
            this.lbl_login.AutoSize = true;
            this.lbl_login.BackColor = System.Drawing.Color.Transparent;
            this.lbl_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.lbl_login.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbl_login.Location = new System.Drawing.Point(131, 43);
            this.lbl_login.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_login.Name = "lbl_login";
            this.lbl_login.Size = new System.Drawing.Size(60, 20);
            this.lbl_login.TabIndex = 28;
            this.lbl_login.Text = "Login :";
            // 
            // txtlogin
            // 
            this.txtlogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.txtlogin.Location = new System.Drawing.Point(226, 43);
            this.txtlogin.Margin = new System.Windows.Forms.Padding(2);
            this.txtlogin.Name = "txtlogin";
            this.txtlogin.Size = new System.Drawing.Size(185, 26);
            this.txtlogin.TabIndex = 27;
            // 
            // txtmdp
            // 
            this.txtmdp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.txtmdp.Location = new System.Drawing.Point(226, 126);
            this.txtmdp.Margin = new System.Windows.Forms.Padding(2);
            this.txtmdp.Name = "txtmdp";
            this.txtmdp.PasswordChar = '☼';
            this.txtmdp.Size = new System.Drawing.Size(175, 26);
            this.txtmdp.TabIndex = 26;
            // 
            // volet2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(639, 362);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbl_mdp);
            this.Controls.Add(this.lbl_login);
            this.Controls.Add(this.txtlogin);
            this.Controls.Add(this.txtmdp);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "volet2";
            this.Text = "connexion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbl_mdp;
        private System.Windows.Forms.Label lbl_login;
        private System.Windows.Forms.TextBox txtlogin;
        private System.Windows.Forms.TextBox txtmdp;
    }
}