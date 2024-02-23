namespace TechnoSchool
{
    partial class ChoixElevePrDossier
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChoixElevePrDossier));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.nomSchool = new System.Windows.Forms.TextBox();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnPage5 = new System.Windows.Forms.Label();
            this.backPage1 = new System.Windows.Forms.Label();
            this.precedentPage = new System.Windows.Forms.Label();
            this.btnPage4 = new System.Windows.Forms.Label();
            this.btnPage3 = new System.Windows.Forms.Label();
            this.btnPage2 = new System.Windows.Forms.Label();
            this.dernierPage = new System.Windows.Forms.Label();
            this.suivantPage = new System.Windows.Forms.Label();
            this.btnPage1 = new System.Windows.Forms.Label();
            this.infoDatagridviewNewEtablissement = new TechnoSchool.panelDegrader();
            this.tetedonnees = new System.Windows.Forms.Label();
            this.tableaudonnees = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            this.panel8.SuspendLayout();
            this.infoDatagridviewNewEtablissement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableaudonnees)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox6
            // 
            this.pictureBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(415, 12);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(37, 29);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 26;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Click += new System.EventHandler(this.pictureBox6_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Lucida Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(206)))), ((int)(((byte)(78)))));
            this.label14.Location = new System.Drawing.Point(12, 12);
            this.label14.Name = "label14";
            this.label14.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label14.Size = new System.Drawing.Size(133, 20);
            this.label14.TabIndex = 27;
            this.label14.Text = "Dossier des élèves";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.infoDatagridviewNewEtablissement);
            this.panel1.Controls.Add(this.tableaudonnees);
            this.panel1.Location = new System.Drawing.Point(12, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(441, 565);
            this.panel1.TabIndex = 29;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(206)))), ((int)(((byte)(78)))));
            this.panel2.Controls.Add(this.nomSchool);
            this.panel2.Controls.Add(this.pictureBox13);
            this.panel2.Location = new System.Drawing.Point(21, 528);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(397, 31);
            this.panel2.TabIndex = 45;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // nomSchool
            // 
            this.nomSchool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(206)))), ((int)(((byte)(78)))));
            this.nomSchool.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nomSchool.Font = new System.Drawing.Font("Lucida Fax", 9.75F, System.Drawing.FontStyle.Bold);
            this.nomSchool.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.nomSchool.Location = new System.Drawing.Point(36, 3);
            this.nomSchool.Multiline = true;
            this.nomSchool.Name = "nomSchool";
            this.nomSchool.Size = new System.Drawing.Size(350, 25);
            this.nomSchool.TabIndex = 44;
            this.nomSchool.KeyUp += new System.Windows.Forms.KeyEventHandler(this.nomSchool_KeyUp);
            // 
            // pictureBox13
            // 
            this.pictureBox13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pictureBox13.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox13.Image")));
            this.pictureBox13.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(31, 31);
            this.pictureBox13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox13.TabIndex = 44;
            this.pictureBox13.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lucida Fax", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label2.Location = new System.Drawing.Point(18, 511);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(263, 14);
            this.label2.TabIndex = 44;
            this.label2.Text = "Filtrer la liste en entrant le nom de l\'élève";
            // 
            // panel8
            // 
            this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel8.Controls.Add(this.btnPage5);
            this.panel8.Controls.Add(this.backPage1);
            this.panel8.Controls.Add(this.precedentPage);
            this.panel8.Controls.Add(this.btnPage4);
            this.panel8.Controls.Add(this.btnPage3);
            this.panel8.Controls.Add(this.btnPage2);
            this.panel8.Controls.Add(this.dernierPage);
            this.panel8.Controls.Add(this.suivantPage);
            this.panel8.Controls.Add(this.btnPage1);
            this.panel8.Location = new System.Drawing.Point(698, 433);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(0, 35);
            this.panel8.TabIndex = 12;
            // 
            // btnPage5
            // 
            this.btnPage5.AutoSize = true;
            this.btnPage5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.btnPage5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnPage5.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPage5.ForeColor = System.Drawing.SystemColors.Window;
            this.btnPage5.Location = new System.Drawing.Point(155, 7);
            this.btnPage5.Name = "btnPage5";
            this.btnPage5.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnPage5.Size = new System.Drawing.Size(21, 25);
            this.btnPage5.TabIndex = 11;
            this.btnPage5.Text = "5";
            // 
            // backPage1
            // 
            this.backPage1.AutoSize = true;
            this.backPage1.Font = new System.Drawing.Font("Wide Latin", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backPage1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.backPage1.Location = new System.Drawing.Point(3, 11);
            this.backPage1.Name = "backPage1";
            this.backPage1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.backPage1.Size = new System.Drawing.Size(24, 24);
            this.backPage1.TabIndex = 3;
            this.backPage1.Text = "<<";
            // 
            // precedentPage
            // 
            this.precedentPage.AutoSize = true;
            this.precedentPage.Font = new System.Drawing.Font("Wide Latin", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.precedentPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.precedentPage.Location = new System.Drawing.Point(33, 11);
            this.precedentPage.Name = "precedentPage";
            this.precedentPage.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.precedentPage.Size = new System.Drawing.Size(16, 24);
            this.precedentPage.TabIndex = 4;
            this.precedentPage.Text = "<";
            // 
            // btnPage4
            // 
            this.btnPage4.AutoSize = true;
            this.btnPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.btnPage4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnPage4.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPage4.ForeColor = System.Drawing.SystemColors.Window;
            this.btnPage4.Location = new System.Drawing.Point(130, 7);
            this.btnPage4.Name = "btnPage4";
            this.btnPage4.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnPage4.Size = new System.Drawing.Size(21, 25);
            this.btnPage4.TabIndex = 8;
            this.btnPage4.Text = "4";
            // 
            // btnPage3
            // 
            this.btnPage3.AutoSize = true;
            this.btnPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.btnPage3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnPage3.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPage3.ForeColor = System.Drawing.SystemColors.Window;
            this.btnPage3.Location = new System.Drawing.Point(105, 7);
            this.btnPage3.Name = "btnPage3";
            this.btnPage3.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnPage3.Size = new System.Drawing.Size(21, 25);
            this.btnPage3.TabIndex = 9;
            this.btnPage3.Text = "3";
            // 
            // btnPage2
            // 
            this.btnPage2.AutoSize = true;
            this.btnPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.btnPage2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnPage2.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPage2.ForeColor = System.Drawing.SystemColors.Window;
            this.btnPage2.Location = new System.Drawing.Point(80, 7);
            this.btnPage2.Name = "btnPage2";
            this.btnPage2.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnPage2.Size = new System.Drawing.Size(21, 25);
            this.btnPage2.TabIndex = 10;
            this.btnPage2.Text = "2";
            // 
            // dernierPage
            // 
            this.dernierPage.AutoSize = true;
            this.dernierPage.Font = new System.Drawing.Font("Wide Latin", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dernierPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.dernierPage.Location = new System.Drawing.Point(204, 11);
            this.dernierPage.Name = "dernierPage";
            this.dernierPage.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.dernierPage.Size = new System.Drawing.Size(24, 24);
            this.dernierPage.TabIndex = 5;
            this.dernierPage.Text = ">>";
            // 
            // suivantPage
            // 
            this.suivantPage.AutoSize = true;
            this.suivantPage.Font = new System.Drawing.Font("Wide Latin", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suivantPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.suivantPage.Location = new System.Drawing.Point(182, 11);
            this.suivantPage.Name = "suivantPage";
            this.suivantPage.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.suivantPage.Size = new System.Drawing.Size(16, 24);
            this.suivantPage.TabIndex = 6;
            this.suivantPage.Text = ">";
            // 
            // btnPage1
            // 
            this.btnPage1.AutoSize = true;
            this.btnPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(191)))));
            this.btnPage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnPage1.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPage1.ForeColor = System.Drawing.SystemColors.Window;
            this.btnPage1.Location = new System.Drawing.Point(55, 7);
            this.btnPage1.Name = "btnPage1";
            this.btnPage1.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnPage1.Size = new System.Drawing.Size(21, 25);
            this.btnPage1.TabIndex = 7;
            this.btnPage1.Text = "1";
            // 
            // infoDatagridviewNewEtablissement
            // 
            this.infoDatagridviewNewEtablissement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoDatagridviewNewEtablissement.BackColor = System.Drawing.Color.Silver;
            this.infoDatagridviewNewEtablissement.colorbottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(191)))));
            this.infoDatagridviewNewEtablissement.colorcenter = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(191)))));
            this.infoDatagridviewNewEtablissement.colortop = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.infoDatagridviewNewEtablissement.Controls.Add(this.tetedonnees);
            this.infoDatagridviewNewEtablissement.Location = new System.Drawing.Point(21, 15);
            this.infoDatagridviewNewEtablissement.Name = "infoDatagridviewNewEtablissement";
            this.infoDatagridviewNewEtablissement.Size = new System.Drawing.Size(397, 38);
            this.infoDatagridviewNewEtablissement.TabIndex = 11;
            this.infoDatagridviewNewEtablissement.Paint += new System.Windows.Forms.PaintEventHandler(this.infoDatagridviewNewEtablissement_Paint);
            // 
            // tetedonnees
            // 
            this.tetedonnees.AutoSize = true;
            this.tetedonnees.BackColor = System.Drawing.Color.Transparent;
            this.tetedonnees.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tetedonnees.ForeColor = System.Drawing.SystemColors.Window;
            this.tetedonnees.Location = new System.Drawing.Point(19, 15);
            this.tetedonnees.Name = "tetedonnees";
            this.tetedonnees.Size = new System.Drawing.Size(289, 15);
            this.tetedonnees.TabIndex = 0;
            this.tetedonnees.Text = "Cliquer sur un élève pour ouvrir son dossier";
            // 
            // tableaudonnees
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableaudonnees.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tableaudonnees.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableaudonnees.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tableaudonnees.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            this.tableaudonnees.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableaudonnees.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tableaudonnees.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tableaudonnees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(206)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Lucida Fax", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tableaudonnees.DefaultCellStyle = dataGridViewCellStyle3;
            this.tableaudonnees.Location = new System.Drawing.Point(21, 58);
            this.tableaudonnees.MultiSelect = false;
            this.tableaudonnees.Name = "tableaudonnees";
            this.tableaudonnees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableaudonnees.Size = new System.Drawing.Size(397, 450);
            this.tableaudonnees.TabIndex = 2;
            this.tableaudonnees.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tableaudonnees_CellClick);
            // 
            // ChoixElevePrDossier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.ClientSize = new System.Drawing.Size(464, 621);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.pictureBox6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChoixElevePrDossier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChoixElevePrDossier";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ChoixElevePrDossier_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.infoDatagridviewNewEtablissement.ResumeLayout(false);
            this.infoDatagridviewNewEtablissement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableaudonnees)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label btnPage5;
        private System.Windows.Forms.Label backPage1;
        private System.Windows.Forms.Label precedentPage;
        private System.Windows.Forms.Label btnPage4;
        private System.Windows.Forms.Label btnPage3;
        private System.Windows.Forms.Label btnPage2;
        private System.Windows.Forms.Label dernierPage;
        private System.Windows.Forms.Label suivantPage;
        private System.Windows.Forms.Label btnPage1;
        private panelDegrader infoDatagridviewNewEtablissement;
        private System.Windows.Forms.Label tetedonnees;
        private System.Windows.Forms.DataGridView tableaudonnees;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox nomSchool;
        private System.Windows.Forms.PictureBox pictureBox13;
        private System.Windows.Forms.Label label2;
    }
}