namespace TechnoSchool
{
    partial class RapportinscriptSection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RapportinscriptSection));
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.InscriptSection1 = new TechnoSchool.InscriptSection();
            this.RapportListinscript1 = new TechnoSchool.RapportListinscript();
            this.infoDatagridviewNewEtablissement = new TechnoSchool.panelDegrader();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.infoTableData = new System.Windows.Forms.Label();
            this.infoDatagridviewNewEtablissement.SuspendLayout();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = 0;
            this.crystalReportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 47);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.ReportSource = this.InscriptSection1;
            this.crystalReportViewer1.Size = new System.Drawing.Size(1069, 566);
            this.crystalReportViewer1.TabIndex = 0;
            // 
            // infoDatagridviewNewEtablissement
            // 
            this.infoDatagridviewNewEtablissement.colorbottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(191)))));
            this.infoDatagridviewNewEtablissement.colorcenter = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.infoDatagridviewNewEtablissement.colortop = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(31)))), ((int)(((byte)(68)))));
            this.infoDatagridviewNewEtablissement.Controls.Add(this.label3);
            this.infoDatagridviewNewEtablissement.Controls.Add(this.label2);
            this.infoDatagridviewNewEtablissement.Controls.Add(this.label1);
            this.infoDatagridviewNewEtablissement.Controls.Add(this.comboBox1);
            this.infoDatagridviewNewEtablissement.Controls.Add(this.infoTableData);
            this.infoDatagridviewNewEtablissement.Location = new System.Drawing.Point(9, 3);
            this.infoDatagridviewNewEtablissement.Name = "infoDatagridviewNewEtablissement";
            this.infoDatagridviewNewEtablissement.Size = new System.Drawing.Size(1051, 38);
            this.infoDatagridviewNewEtablissement.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(723, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "session...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(623, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Session active :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(256, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Section :";
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.Highlight;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.Font = new System.Drawing.Font("Lucida Fax", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(325, 11);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(155, 23);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            // 
            // infoTableData
            // 
            this.infoTableData.AutoSize = true;
            this.infoTableData.BackColor = System.Drawing.Color.Transparent;
            this.infoTableData.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoTableData.ForeColor = System.Drawing.SystemColors.Window;
            this.infoTableData.Location = new System.Drawing.Point(18, 13);
            this.infoTableData.Name = "infoTableData";
            this.infoTableData.Size = new System.Drawing.Size(113, 15);
            this.infoTableData.TabIndex = 0;
            this.infoTableData.Text = "Filtrer le rapport";
            // 
            // RapportinscriptSection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1072, 613);
            this.Controls.Add(this.infoDatagridviewNewEtablissement);
            this.Controls.Add(this.crystalReportViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RapportinscriptSection";
            this.Text = "Rapport liste des élèves inscrits par session";
            this.infoDatagridviewNewEtablissement.ResumeLayout(false);
            this.infoDatagridviewNewEtablissement.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private RapportListinscript RapportListinscript1;
        private InscriptSection InscriptSection1;
        private panelDegrader infoDatagridviewNewEtablissement;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label infoTableData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}