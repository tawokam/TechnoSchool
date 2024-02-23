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
    public partial class detailEmployer : Form
    { // ---------------
        private int borderRadius = 20;
        private int borderSize = 1;
        private Color borderColor = Color.RoyalBlue;

        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static string connectionstring = "";
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
        }
        // Récupération des informations dépuis la BD grace à l'id
        public class Detail
        {
            private TextBox nom, email, localisation;
            private Label phone1, phone2, sexe, datenaiss, numurgence, cv, fonction, diplome, specialite, datecrea, datemodif,btnOpencv;
            private PictureBox logo;

            public static string nomcv = "az";
            public string Nomcv
            {
                set { nomcv = value; }
                get { return nomcv; }
            }
            private TextBox Nom { set; get; }
            private TextBox Email { set; get; }
            private TextBox Localisation { set; get; }
            private Label Phone1 { set; get; }
            private Label Phone2 { set; get; }
            private Label Sexe { set; get; }
            private Label Datenaiss { set; get; }
            private Label Numurgence { set; get; }
            private Label Cv { set; get; }
            private Label Fonction { set; get; }
            private Label Diplome { set; get; }
            private Label Specialite { set; get; }
            private Label Datecrea { set; get; }
            private Label Datemodif { set; get; }
            private PictureBox Logo { set; get; }
            // Constructeur
            private Label BtnOpencv { set; get; }
            public Detail(TextBox nom, TextBox email, TextBox localisation, Label Phone1, Label Phone2, Label Sexe, Label Datenaiss, Label Numurgence, Label Cv, Label Fonction, Label Diplome, Label Specialite, Label Datecrea, Label Datemodif, PictureBox logo,Label btnOpencv)
            {
                this.Nom = nom; this.Email = email; this.Localisation = localisation; this.Phone1 = Phone1; this.Phone2 = Phone2;
                this.Sexe= Sexe; this.Datenaiss = Datenaiss; this.Numurgence = Numurgence; this.Cv = Cv; this.Fonction = Fonction; this.Diplome = Diplome;
                this.Specialite = Specialite; this.Datecrea = Datecrea; this.Datemodif = Datemodif; this.Logo = logo; this.BtnOpencv = btnOpencv;
            }
            public Detail() { }

            // Récuperation des informations dans la base de données
            public void detailinfo(int idline)
            {
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "SELECT employer.id_employer,employer.nom_employer,employer.telephone1_employer,employer.telephone2_employer,employer.sexe_employer,employer.adresseMail_employer,employer.quartier_employer,employer.id_fonction,employer.datenaiss_employer,employer.grandiplome,employer.specialitediplome,employer.cv_employer,employer.photo_employer,employer.numerourgence,employer.date_creation,employer.date_modification,fonction.id_fonction,fonction.nom_fonction FROM employer inner join fonction on employer.id_fonction = fonction.id_fonction where employer.id_employer = @id";
                Dictionary<string, object> parametre = new Dictionary<string, object>();
                parametre.Add("@id", idline);
                command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parameter in parametre)
                {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));

                }
                command.Prepare();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Nom.Text = reader.GetValue(1).ToString();
                    Phone1.Text = reader.GetValue(2).ToString();
                    Phone2.Text = reader.GetValue(3).ToString();
                    Sexe.Text = reader.GetValue(4).ToString();
                    Email.Text = reader.GetValue(5).ToString();
                    Localisation.Text = reader.GetValue(6).ToString();
                    Fonction.Text = reader.GetValue(17).ToString();
                    Datenaiss.Text = string.Format("{0:dd / MM / yyyy}", reader.GetValue(8));
                    Diplome.Text = reader.GetValue(9).ToString();
                    Specialite.Text = reader.GetValue(10).ToString();
                    Nomcv = reader.GetValue(11).ToString();
                    if (reader.GetValue(11).ToString() != "")
                    {
                        Cv.Text = "Oui";
                        BtnOpencv.Visible = true;
                    }
                    else
                    {
                        // nom cv : reader.GetValue(11).ToString()
                        Cv.Text = "Non";
                        BtnOpencv.Visible = false;
                    }
                    
                    Numurgence.Text = reader.GetValue(13).ToString();
                    //DateTime datecreer = new DateTime(reader.GetValue(13).ToString("dd/MM/yyyy"));

                    Datecrea.Text = string.Format("{0:dd / MM / yyyy}", reader.GetValue(15));
                    Datemodif.Text = string.Format("{0:dd / MM / yyyy}", reader.GetValue(14));

                    // Affichage de mon image converti par la methode convertByteArrayToImage
                    Logo.Image = CovertByteArrayToImage((byte[])(reader.GetValue(12)));
                   
                }
                reader.Close();
            }
            private Image CovertByteArrayToImage(byte[] data)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        return Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    return null;
                    string messag = "Nous nous sommes heurtés à un problème lors de l'affichage du logo de l'établissement scolaire. ";
                    string titre = "Modification ";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            // recuperation du nom du cv pour l'ouverture

            public void openCv()
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = nomcv;
                process.Start();
                process.Close();
                /*string chainec = nomcv;
                string chemin = Environment.CurrentDirectory + "/img/" + chainec;
                string filename = @chemin;
                openbrowser.Navigate(filename);*/
            }
        }
        public detailEmployer(int idligne)
        {
            InitializeComponent();
            connexionDB();
            Detail newdet = new Detail(textBoxNom, textBoxEmail, textBoxLocalisation, labelPhone1, labelPhone2, sexe, datenaiss, numurgence, cv, fonction, diplome, specialite, datecreation, datemodif, pictureBoxphoto, labelopen);
            newdet.detailinfo(idligne);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void detailEmployer_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 8, e.Graphics, borderColor, borderSize);
        }

        private void panelDegrader1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelDegrader1, 8, e.Graphics, borderColor, borderSize);
        }

        private void label17_Click(object sender, EventArgs e)
        {
            //string filename = @"file:///C:/Users/MULUH/Documents/CLASSEURS BRTS.docx";
            //webBrowser1.Navigate(filename);
            Detail newdet = new Detail();
            newdet.openCv();
            
        }
    }
}
