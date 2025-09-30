namespace tableau_de_bord
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.grpmissions = new System.Windows.Forms.Panel();
            this.btnpompier = new System.Windows.Forms.Button();
            this.btn_vehicule = new System.Windows.Forms.Button();
            this.btnmission = new System.Windows.Forms.Button();
            this.btnquitter = new System.Windows.Forms.Button();
            this.pbxLogo = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chkencours = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // grpmissions
            // 
            this.grpmissions.AutoScroll = true;
            this.grpmissions.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpmissions.Location = new System.Drawing.Point(275, 66);
            this.grpmissions.Name = "grpmissions";
            this.grpmissions.Size = new System.Drawing.Size(1379, 850);
            this.grpmissions.TabIndex = 0;
            // 
            // btnpompier
            // 
            this.btnpompier.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnpompier.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnpompier.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btnpompier.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Navy;
            this.btnpompier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.btnpompier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpompier.Font = new System.Drawing.Font("Arial Black", 12.25F);
            this.btnpompier.ForeColor = System.Drawing.Color.White;
            this.btnpompier.Location = new System.Drawing.Point(18, 218);
            this.btnpompier.Name = "btnpompier";
            this.btnpompier.Size = new System.Drawing.Size(220, 94);
            this.btnpompier.TabIndex = 1;
            this.btnpompier.Text = "Gestion des pompiers";
            this.btnpompier.UseVisualStyleBackColor = false;
            this.btnpompier.Click += new System.EventHandler(this.pompier_Click);
            // 
            // btn_vehicule
            // 
            this.btn_vehicule.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_vehicule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_vehicule.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btn_vehicule.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Navy;
            this.btn_vehicule.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.btn_vehicule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_vehicule.Font = new System.Drawing.Font("Arial Black", 12.25F);
            this.btn_vehicule.ForeColor = System.Drawing.Color.White;
            this.btn_vehicule.Location = new System.Drawing.Point(22, 334);
            this.btn_vehicule.Name = "btn_vehicule";
            this.btn_vehicule.Size = new System.Drawing.Size(219, 94);
            this.btn_vehicule.TabIndex = 2;
            this.btn_vehicule.Text = "Visualisation des véhicules";
            this.btn_vehicule.UseVisualStyleBackColor = false;
            this.btn_vehicule.Click += new System.EventHandler(this.vehicule_Click);
            // 
            // btnmission
            // 
            this.btnmission.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnmission.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnmission.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btnmission.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Navy;
            this.btnmission.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.btnmission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnmission.Font = new System.Drawing.Font("Arial Black", 12.25F);
            this.btnmission.ForeColor = System.Drawing.Color.White;
            this.btnmission.Location = new System.Drawing.Point(21, 461);
            this.btnmission.Name = "btnmission";
            this.btnmission.Size = new System.Drawing.Size(220, 94);
            this.btnmission.TabIndex = 3;
            this.btnmission.Text = "Ajout d\'une nouvelle mission ";
            this.btnmission.UseVisualStyleBackColor = false;
            this.btnmission.Click += new System.EventHandler(this.mission_Click);
            // 
            // btnquitter
            // 
            this.btnquitter.BackColor = System.Drawing.Color.Crimson;
            this.btnquitter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnquitter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnquitter.FlatAppearance.BorderSize = 3;
            this.btnquitter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.btnquitter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnquitter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnquitter.Font = new System.Drawing.Font("Arial Black", 12.25F);
            this.btnquitter.ForeColor = System.Drawing.Color.White;
            this.btnquitter.Location = new System.Drawing.Point(21, 750);
            this.btnquitter.Name = "btnquitter";
            this.btnquitter.Size = new System.Drawing.Size(217, 94);
            this.btnquitter.TabIndex = 4;
            this.btnquitter.Text = "Quitter";
            this.btnquitter.UseVisualStyleBackColor = false;
            this.btnquitter.Click += new System.EventHandler(this.btnquitter_Click);
            // 
            // pbxLogo
            // 
            this.pbxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbxLogo.Image")));
            this.pbxLogo.ImageLocation = "";
            this.pbxLogo.Location = new System.Drawing.Point(18, 12);
            this.pbxLogo.Name = "pbxLogo";
            this.pbxLogo.Size = new System.Drawing.Size(220, 200);
            this.pbxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxLogo.TabIndex = 24;
            this.pbxLogo.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Navy;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial Black", 12.25F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(19, 601);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(220, 94);
            this.button1.TabIndex = 25;
            this.button1.Text = "Statistiques";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkencours
            // 
            this.chkencours.AutoSize = true;
            this.chkencours.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.chkencours.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.chkencours.Location = new System.Drawing.Point(325, 12);
            this.chkencours.Name = "chkencours";
            this.chkencours.Size = new System.Drawing.Size(137, 35);
            this.chkencours.TabIndex = 26;
            this.chkencours.Text = "en cours";
            this.chkencours.UseVisualStyleBackColor = true;
            this.chkencours.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(1690, 966);
            this.Controls.Add(this.chkencours);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnquitter);
            this.Controls.Add(this.btnmission);
            this.Controls.Add(this.btn_vehicule);
            this.Controls.Add(this.pbxLogo);
            this.Controls.Add(this.btnpompier);
            this.Controls.Add(this.grpmissions);
            this.Name = "Form1";
            this.Text = "accueil";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel grpmissions;
        private System.Windows.Forms.Button btnpompier;
        private System.Windows.Forms.Button btn_vehicule;
        private System.Windows.Forms.Button btnmission;
        private System.Windows.Forms.Button btnquitter;
        private System.Windows.Forms.PictureBox pbxLogo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkencours;
    }
}

