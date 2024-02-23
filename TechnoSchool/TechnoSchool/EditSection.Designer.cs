namespace TechnoSchool
{
    partial class EditSection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditSection));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.nomsession = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.infoDatagridviewNewEtablissement = new TechnoSchool.panelDegrader();
            this.infoTableData = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fermFormAddSchool = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.infoDatagridviewNewEtablissement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fermFormAddSchool)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.infoDatagridviewNewEtablissement);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(6, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(383, 182);
            this.panel1.TabIndex = 21;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(337, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "label1";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(206)))), ((int)(((byte)(78)))));
            this.panel2.Controls.Add(this.nomsession);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Location = new System.Drawing.Point(9, 78);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(369, 31);
            this.panel2.TabIndex = 43;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // nomsession
            // 
            this.nomsession.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(206)))), ((int)(((byte)(78)))));
            this.nomsession.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nomsession.Font = new System.Drawing.Font("Lucida Fax", 9.75F, System.Drawing.FontStyle.Bold);
            this.nomsession.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.nomsession.Location = new System.Drawing.Point(36, 3);
            this.nomsession.Multiline = true;
            this.nomsession.Name = "nomsession";
            this.nomsession.Size = new System.Drawing.Size(330, 25);
            this.nomsession.TabIndex = 44;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(31, 31);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 44;
            this.pictureBox2.TabStop = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.Window;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(92, 133);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(193, 39);
            this.button2.TabIndex = 39;
            this.button2.Text = "Enregistrer / Save";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.Paint += new System.Windows.Forms.PaintEventHandler(this.button2_Paint);
            // 
            // infoDatagridviewNewEtablissement
            // 
            this.infoDatagridviewNewEtablissement.colorbottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(191)))));
            this.infoDatagridviewNewEtablissement.colorcenter = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.infoDatagridviewNewEtablissement.colortop = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.infoDatagridviewNewEtablissement.Controls.Add(this.infoTableData);
            this.infoDatagridviewNewEtablissement.Location = new System.Drawing.Point(8, 9);
            this.infoDatagridviewNewEtablissement.Name = "infoDatagridviewNewEtablissement";
            this.infoDatagridviewNewEtablissement.Size = new System.Drawing.Size(370, 38);
            this.infoDatagridviewNewEtablissement.TabIndex = 12;
            this.infoDatagridviewNewEtablissement.Paint += new System.Windows.Forms.PaintEventHandler(this.infoDatagridviewNewEtablissement_Paint);
            // 
            // infoTableData
            // 
            this.infoTableData.AutoSize = true;
            this.infoTableData.BackColor = System.Drawing.Color.Transparent;
            this.infoTableData.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoTableData.ForeColor = System.Drawing.SystemColors.Window;
            this.infoTableData.Location = new System.Drawing.Point(30, 15);
            this.infoTableData.Name = "infoTableData";
            this.infoTableData.Size = new System.Drawing.Size(191, 15);
            this.infoTableData.TabIndex = 0;
            this.infoTableData.Text = "Modifier le nom de la section";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lucida Fax", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label2.Location = new System.Drawing.Point(6, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Nom de la section";
            // 
            // fermFormAddSchool
            // 
            this.fermFormAddSchool.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fermFormAddSchool.Image = ((System.Drawing.Image)(resources.GetObject("fermFormAddSchool.Image")));
            this.fermFormAddSchool.Location = new System.Drawing.Point(346, 9);
            this.fermFormAddSchool.Name = "fermFormAddSchool";
            this.fermFormAddSchool.Size = new System.Drawing.Size(37, 29);
            this.fermFormAddSchool.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fermFormAddSchool.TabIndex = 20;
            this.fermFormAddSchool.TabStop = false;
            this.fermFormAddSchool.Click += new System.EventHandler(this.fermFormAddSchool_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Lucida Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(206)))), ((int)(((byte)(78)))));
            this.label14.Location = new System.Drawing.Point(12, 9);
            this.label14.Name = "label14";
            this.label14.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label14.Size = new System.Drawing.Size(147, 20);
            this.label14.TabIndex = 19;
            this.label14.Text = "Modifier une section";
            // 
            // EditSection
            // 
            this.AcceptButton = this.button2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.ClientSize = new System.Drawing.Size(397, 237);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.fermFormAddSchool);
            this.Controls.Add(this.label14);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EditSection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditSection";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.EditSection_Paint);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.infoDatagridviewNewEtablissement.ResumeLayout(false);
            this.infoDatagridviewNewEtablissement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fermFormAddSchool)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox nomsession;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button2;
        private panelDegrader infoDatagridviewNewEtablissement;
        private System.Windows.Forms.Label infoTableData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox fermFormAddSchool;
        private System.Windows.Forms.Label label14;
    }
}