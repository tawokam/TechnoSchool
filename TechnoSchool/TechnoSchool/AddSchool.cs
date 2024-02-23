using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnoSchool
{
    public partial class AddSchool : Form
    {
        // ---------------
        private int borderRadius = 20;
        private int borderSize = 1;
        private Color borderColor = Color.RoyalBlue;
        // connection a la base de données en recuperant les variable de connexion de la fenetre index
        Index pageprincipal;
        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataReader reader2;
        public static string connectionstring = "";
        // Donnees de connexion
        public void connexionDB()
        {
            string cheminfichierConfig = Path.Combine(Environment.CurrentDirectory, "FileConfig/FileConfig.ini");
            // Vérifiez si le fichier existe

            if (File.Exists(cheminfichierConfig))
            {
                // Si oui, rien ne se passe
                GestionFileIni ger = new GestionFileIni(cheminfichierConfig);
                // lecture des informations dans le fichier
                string server = ger.ReadIni("Server", "server");
                string user = ger.ReadIni("User", "user");
                string mdp = ger.ReadIni("Mdp", "mdp");
                string DB = ger.ReadIni("DataBase", "base de donnees");
                connectionstring = "server=" + server + ";user id=" + user + ";password=" + mdp + ";database=" + DB + "; SslMode=none";

            }
            else
            {
                // si non créer le fichier ini

                string messag = "Nou nous somme heurté à un problème lors de la connexion au serveur";
                string titre = "Achat licence ";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        // class pour la lecture et l'écriture dans mon fichier ini pour les données de licence
        public class GestionFileIni
        {
            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string name, string key, string val, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            [DllImport("kernel32")]
            private static extern int WritePrivateProfileSection(string section, string IpString, string filePath);

            public string path;
            public GestionFileIni(string inipath)
            {
                path = inipath;
            }
            // methode pour ecrire dans le fichier ini
            public void WriteIni(string name, string key, string value)
            {
                WritePrivateProfileString(name, key, value, this.path);
            }
            // Methode pour lire dans un fichier ini
            public string ReadIni(string name, string key)
            {
                StringBuilder sb = new StringBuilder(255);
                int ini = GetPrivateProfileString(name, key, "", sb, 255, this.path);
                return sb.ToString();
            }
            // methode pour supprimer une section et ces valeurs dans un fichier ini
            public int RemoveSection(string name)
            {
                return WritePrivateProfileSection(name, null, this.path);
            }
        }
        // creation de la sous class pour gerer l'insertion des etablissement
        public class InsertEtablissement
        {
            

            private string nomEtabli, mailEtabli, localisationEtabli, bpEtabli, niuEtabli, nrcEtabli, paieEnLigne;
            private long phone1Etabli, phone2Etabli, omEtabli, momoEtabli;
            private byte[] logoEtabli;
            public int siDonneesValide = 0;
            private string NomEtabli { get; set; }
            private string MailEtabli { get; set; }
            private string LocalisationEtabli { get; set; }
            private string BpEtabli { get; set; }
            private string nIUEtabli { get; set; }
            private string nRCEtabli { get; set; }
            private byte[] LogoEtabli { get; set; }
            private string PaieEnLigne { get; set; }
            private long Phone1Etabli { get; set; }
            private long Phone2Etabli { get; set; }
            private long oMEtabli { get; set; }
            private long mOMOEtabli { get; set; }
            // Constructeur
            public InsertEtablissement(string nom, string localisation, string mail, string bp, long phone1, long phone2, string NIU, string NRC, long OM, long MOMO, byte[] logo)
            {
                this.NomEtabli = nom; this.MailEtabli = mail; this.LocalisationEtabli = localisation; this.BpEtabli = bp;
                this.nIUEtabli = NIU; this.nRCEtabli = NRC; this.LogoEtabli = logo; this.Phone1Etabli = phone1;
                this.Phone2Etabli = phone2; this.oMEtabli = OM; this.mOMOEtabli = MOMO;
                if(oMEtabli == 0 && mOMOEtabli == 0)
                {
                    this.PaieEnLigne = "Non";
                }else
                {
                    this.PaieEnLigne = "Oui";
                }
                
                insertdb();
            }
            private void insertdb()
            {
                try
                {
                    long nbreetabli = 0;
                    // verifions si un établissement ayant ce nom a déja été crée
                    string req = "Select nom_etabli,count(nom_etabli) as nbreschool,mail,phone1 from etablissement";
                    
                    var connect = new MySqlConnection(connectionstring);
                    connect.Open();
                    var commande = new MySqlCommand(req, connect);
                    commande.Prepare();
                    reader2 = commande.ExecuteReader();
                    while (reader2.Read())
                    {
                        nbreetabli = long.Parse(reader2.GetValue(1).ToString());
                    }
                    if (nbreetabli < 1)
                    {
                        string requete = "INSERT INTO etablissement(nom_etabli,mail,localisation,bp,phone1,phone2,NIU,NRC,logo,paieEnLigne,Numom,nummm,date_creation,dernier_modification) values(@nom,@mail,@localisation,@bp,@phone1,@phone2,@niu,@nrc,@logo,@paie,@numom,@nummm,@creation,@modification)";
                        string datec = DateTime.Now.ToString("yyyy-MM-dd");
                        string datem = DateTime.Now.ToString("yyyy-MM-dd");
                        Dictionary<string, object> parametre = new Dictionary<string, object>();
                        parametre.Add("@nom", NomEtabli);
                        parametre.Add("@mail", MailEtabli);
                        parametre.Add("@localisation", LocalisationEtabli);
                        parametre.Add("@bp", BpEtabli);
                        parametre.Add("@phone1", Phone1Etabli);
                        parametre.Add("@phone2", Phone2Etabli);
                        parametre.Add("@niu", nIUEtabli);
                        parametre.Add("@nrc", nRCEtabli);
                        parametre.Add("@logo", LogoEtabli);
                        parametre.Add("@paie", PaieEnLigne);
                        parametre.Add("@numom", oMEtabli);
                        parametre.Add("@nummm", mOMOEtabli);
                        parametre.Add("@creation", datec);
                        parametre.Add("@modification", datem);
                        //  var connectionstring = "server=localhost;user id=root;database=technosoft; SslMode=none";
                        var connection = new MySqlConnection(connectionstring);
                        connection.Open();
                        var command = new MySqlCommand(requete, connection);
                        foreach (KeyValuePair<string, object> parametres in parametre)
                        {
                            command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                        }
                        command.Prepare();
                        if (command.ExecuteNonQuery() >= 1)
                        {
                            string messag = "Votre établissement scolaire a été enregistré avec succès. ";
                            string titre = "Insertion ";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            siDonneesValide = 1;
                        }
                        else
                        {
                            string messag = "Nous nous sommes heurtés à un problème lors de l'enregistrement de votre établissement scolaire. ";
                            string titre = "Insertion ";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            siDonneesValide = 0;
                        }
                    }
                    else
                    {
                        string messag = "Vous ne pouvez créer qu'un seul établissement scolaire";
                        string titre = "Erreur";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        siDonneesValide = 0;
                    }
                    connection.Close();
                }
                catch(Exception ex)
                {
                    string messag = @"Nous nous sommes heurtés à un problème lors de l'enregistrement de votre établissement scolaire. 
cause probable: 
Le logo de l'établissement n'est pas accepté,
Problème de communication avec le serveur,
...
";
                    string titre = "Erreur d'insertion";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }

        }

        // Classe contenant les fonction du formulaire et les arrondi de bordure
        public class designBordure
        {
           // private int borderRadius = 20;
            private int borderSize = 1;

            // Constructeur

            // Methode pour arrondir les bordure
            public GraphicsPath GetRoundedPath(Rectangle rect, float radius)
            {
                GraphicsPath path = new GraphicsPath();
                float curveSize = radius * 2F;

                path.StartFigure();
                path.AddArc(rect.X, rect.Y, curveSize, curveSize, 165, 90);
                path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
                path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
                path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
                return path;
            }

            // Methode pour arrondir uniquement les formulaire
            public void FormRegionAndBorder(Form form, float radius, Graphics graph, Color borderColor, float bordersize)
            {
                if (form.WindowState != FormWindowState.Minimized)
                {
                    using (GraphicsPath roundPath = GetRoundedPath(form.ClientRectangle, radius))
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    using (Matrix transform = new Matrix())
                    {
                        graph.SmoothingMode = SmoothingMode.AntiAlias;
                        form.Region = new Region(roundPath);
                        if (borderSize > 1)
                        {
                            Rectangle rect = form.ClientRectangle;
                            float scaleX = 1.0F - ((borderSize + 1) / rect.Width);
                            float scaleY = 1.0F - ((borderSize + 1) / rect.Height);
                            transform.Scale(scaleX, scaleY);
                            transform.Translate(borderSize / 1.6F, borderSize / 1.6F);
                            graph.Transform = transform;
                            graph.DrawPath(penBorder, roundPath);
                        }
                    }
                }
            }
            //Methode pour arrondir les panels 
            public void FormRegionAndBorderpanel(Panel panel, float radius, Graphics graph, Color borderColor, float bordersize)
            {
                using (GraphicsPath roundPath = GetRoundedPath(panel.ClientRectangle, radius))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (Matrix transform = new Matrix())
                {
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    panel.Region = new Region(roundPath);
                    if (borderSize > 1)
                    {
                        Rectangle rect = panel.ClientRectangle;
                        float scaleX = 0.5F - ((borderSize + 1) / rect.Width);
                        float scaleY = 0.5F - ((borderSize + 1) / rect.Height);
                        transform.Scale(scaleX, scaleY);
                        transform.Translate(borderSize / 1.6F, borderSize / 1.6F);
                        graph.Transform = transform;
                        graph.DrawPath(penBorder, roundPath);
                    }
                }

            }
            //Methode pour arrondir les textbox
            public void FormRegionAndBordertextbox(TextBox textbox, float radius, Graphics graph, Color borderColor, float bordersize)
            {
                using (GraphicsPath roundPath = GetRoundedPath(textbox.ClientRectangle, radius))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (Matrix transform = new Matrix())
                {
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    textbox.Region = new Region(roundPath);
                    if (borderSize > 1)
                    {
                        Rectangle rect = textbox.ClientRectangle;
                        float scaleX = 0.5F - ((borderSize + 1) / rect.Width);
                        float scaleY = 0.5F - ((borderSize + 1) / rect.Height);
                        transform.Scale(scaleX, scaleY);
                        transform.Translate(borderSize / 1.6F, borderSize / 1.6F);
                        graph.Transform = transform;
                        graph.DrawPath(penBorder, roundPath);
                    }
                }

            }
            //Methode pour arrondir les labels 
            public void FormRegionAndBorderlabel(Label panel, float radius, Graphics graph, Color borderColor, float bordersize)
            {
                using (GraphicsPath roundPath = GetRoundedPath(panel.ClientRectangle, radius))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (Matrix transform = new Matrix())
                {
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    panel.Region = new Region(roundPath);
                    if (borderSize > 1)
                    {
                        Rectangle rect = panel.ClientRectangle;
                        float scaleX = 0.5F - ((borderSize + 1) / rect.Width);
                        float scaleY = 0.5F - ((borderSize + 1) / rect.Height);
                        transform.Scale(scaleX, scaleY);
                        transform.Translate(borderSize / 1.6F, borderSize / 1.6F);
                        graph.Transform = transform;
                        graph.DrawPath(penBorder, roundPath);
                    }
                }

            }
            //Methode pour arrondir les bouton
            public void FormRegionAndBorderbouton(Button bouton, float radius, Graphics graph, Color borderColor, float bordersize)
            {
                using (GraphicsPath roundPath = GetRoundedPath(bouton.ClientRectangle, radius))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (Matrix transform = new Matrix())
                {
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    bouton.Region = new Region(roundPath);
                    if (borderSize > 1)
                    {
                        Rectangle rect = bouton.ClientRectangle;
                        float scaleX = 0.5F - ((borderSize + 1) / rect.Width);
                        float scaleY = 0.5F - ((borderSize + 1) / rect.Height);
                        transform.Scale(scaleX, scaleY);
                        transform.Translate(borderSize / 1.6F, borderSize / 1.6F);
                        graph.Transform = transform;
                        graph.DrawPath(penBorder, roundPath);
                    }
                }

            }
        }

        public Boolean clickEnfonce = false;
        // Methode pour déplacé ma fenetre
        public void deplaceWindow(Form form)
        {
            if(clickEnfonce == true)
            {
                Point position = Cursor.Position;
               
                if ( position.X+10 >= Screen.PrimaryScreen.Bounds.Width)
                {
                    form.Location = new Point(position.X-(position.X/2), position.Y);
                }else if(position.Y-20 <= Screen.PrimaryScreen.Bounds.Height)
                {
                    form.Location = new Point(position.X, position.Y-(position.Y/2));
                }
                else
                {
                    form.Location = new Point(position.X, position.Y);
                    
                }
                
            }
           
        }
        public AddSchool()
        {
            InitializeComponent();
            connexionDB();
        }

        private void nomnewetablissement_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddSchool_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this,10, e.Graphics, Color.Blue, 1);

        }

        private void panelDegrader1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelModePaiement, 10, e.Graphics, Color.Blue, 1);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
          
        }

        private void button3_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button2, 7, e.Graphics, Color.Blue, 1);
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(0, 89, 191);
            button2.ForeColor = Color.White;
        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
            button2.BackColor = Color.FromArgb(1, 31, 68);
            button2.ForeColor = Color.White;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel2, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel3, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel4, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel5, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel6, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel7, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel8, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel9, 5, e.Graphics, Color.Blue, 1);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            MOMOnewetablissement.Visible = true;
            nomMM.Visible = true;
            OMnewetablissement.Visible = true;
            nomOM.Visible = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            MOMOnewetablissement.Visible = false;
            MOMOnewetablissement.Text = "";
            nomMM.Visible = false;
            OMnewetablissement.Visible = false;
            OMnewetablissement.Text = "";
            nomOM.Visible = false;
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button3, 7, e.Graphics, Color.Blue, 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialogLogoSchool.Filter = "Extension autorisee|*.jpg;*.jpeg;*.png;*.bmp;*.ico";
            if (openFileDialogLogoSchool.ShowDialog() == DialogResult.OK)
            {
                imageSchool.Image = Image.FromFile(openFileDialogLogoSchool.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            nomSchool.Text = ""; mail.Text = ""; localisation.Text = "";
            BP.Text = ""; phone1.Text = ""; phone2.Text = ""; niu.Text = ""; nrc.Text = "";
            MOMOnewetablissement.Text = ""; OMnewetablissement.Text = "";

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel10, 10, e.Graphics, Color.Blue, 1);
        }

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 8, e.Graphics, Color.Blue, 1);
        }

        private void phone1_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void phone1_KeyPress(object sender, KeyPressEventArgs e)
        {
                 // Code pour empeché la saisi des lettres dans le champ
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void phone2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void MOMOnewetablissement_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void OMnewetablissement_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        // methode de conversion d'image en tableau de byte pour inserer dans la base de données
        byte[] convertImageToBytes(Image Img)
        {
            using(MemoryStream sm = new MemoryStream())
            {
                Img.Save(sm, System.Drawing.Imaging.ImageFormat.Png);
                return sm.ToArray();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
           // Récupération et traitement des informations saisi par l'utilisateur pour la création d'un établissement scolaire
            if (nomSchool.Text == "")
            {
                string messag = "Veuillez entrer le nom de l'établissement scolaire. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (mail.Text == "")
            {
                string messag = "Veuillez inscrire l'adresse électronique de l'établissement scolaire. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (localisation.Text == "")
            {
                string messag = "Veuillez renseigner la localisation de l'établissement scolaire.";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (phone1.Text == "")
            {
                string messag = "Veuillez saisir au minimum un numéro de téléphone. Le téléphone n º 1 est obligatoire. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else if (imageSchool.Image == null)
            {
                string messag = "Veuillez importer le logo de l'établissement scolaire ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                long telephone2, momo, om;
                if (phone2.Text == ""){ telephone2 = 0; }else { telephone2 = long.Parse(phone2.Text); }
                if (MOMOnewetablissement.Text == "") { momo = 0; } else { momo= long.Parse(MOMOnewetablissement.Text); }
                if (OMnewetablissement.Text == "") { om = 0; } else { om = long.Parse(OMnewetablissement.Text); }

                string nom = nomSchool.Text; string email = mail.Text; string localisations = localisation.Text;
                string boitep = BP.Text; long telephone1 = long.Parse(phone1.Text);
                string nius = niu.Text; string nrcs = nrc.Text; byte[] logo = convertImageToBytes(imageSchool.Image);
                InsertEtablissement newschool = new InsertEtablissement(nom, localisations, email, boitep,telephone1, telephone2, nius, nrcs, om, momo, logo);
                int idvalue = newschool.siDonneesValide;
                if(idvalue == 1)
                {
                    nomSchool.Text = ""; mail.Text = ""; localisation.Text = ""; phone1.Text = 0.ToString(); BP.Text = "";
                    phone2.Text = 0.ToString(); niu.Text = ""; nrc.Text = ""; MOMOnewetablissement.Text = 0.ToString();
                    OMnewetablissement.Text = 0.ToString(); radioButton2.Checked = true;
                    imageSchool.Image = null;
                }
            }
        }

        private void AddSchool_Load(object sender, EventArgs e)
        {

        }

        private void nomSchool_TextChanged(object sender, EventArgs e)
        {

        }

        private void phone1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void AddSchool_MouseMove(object sender, MouseEventArgs e)
        {
            deplaceWindow(this);
        }

        private void panel11_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void AddSchool_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

        private void AddSchool_MouseUp(object sender, MouseEventArgs e)
        {
            clickEnfonce = false;
        }

        private void AddSchool_Click(object sender, EventArgs e)
        {

        }

        private void AddSchool_MouseDown(object sender, MouseEventArgs e)
        {
            clickEnfonce = true;
        }
    }
    }

