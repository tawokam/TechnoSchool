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
    public partial class detailsEleve : Form
    {

        // ---------------
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
            private TextBox nom, prenom, matricule, adresse, infos;
            private Label datenaiss, lieunaiss, sexe, nationalite, maladie, eps,nompere,nommere,nomtuteur,phonep,phonem,phonet, editDate, adddate;
            private PictureBox photo;
            private TextBox Nom { set; get; }
            private TextBox Prenom { set; get; }
            private TextBox Matricule { set; get; }
            private TextBox Adresse { set; get; }
            private TextBox Infos { set; get; }
            private Label Datenaiss { set; get; }
            private Label Lieunaiss { set; get; }
            private Label Sexe { set; get; }
            private Label Nationalite { set; get; }
            private Label Maladie { set; get; }
            private Label Eps { set; get; }
            private Label Nompere { set; get; }
            private Label Nommere { set; get; }
            private Label Nomtuteur { set; get; }
            private Label Phonep { set; get; }
            private Label Phonem { set; get; }
            private Label Phonet { set; get; }
            private Label EditDate { set; get; }
            private Label Adddate { set; get; }
            private PictureBox Photo { set; get; }
            // Constructeur
            public Detail(TextBox nom, TextBox prenom, TextBox matricule, TextBox adresse, TextBox infos, Label datenaiss, Label lieunaiss, Label sexe, Label nationalite, Label maladie, Label eps, Label nomp, Label nomm, Label nomt, Label phonep, Label phonem, Label phonet, Label editdate, Label adddate, PictureBox photo)
            {
                this.Nom = nom; this.Prenom = prenom; this.Matricule = matricule; this.Adresse = adresse; this.Infos = infos;
                this.Datenaiss = datenaiss; this.Lieunaiss = lieunaiss; this.Sexe = sexe; this.Nationalite = nationalite; this.Maladie = maladie; this.Eps = eps;
                this.Adddate = adddate; this.EditDate = editdate; this.Nompere = nomp; this.Nommere = nomm; this.Nomtuteur = nomt;
                this.Phonep = phonep; this.Phonem = phonem; this.Phonet = phonet; this.Photo = photo;
            }

            // Récuperation des informations dans la base de données
            public void detailinfo(int idline)
            {

                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "SELECT * FROM eleves where id_eleve = @id";
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
                    Prenom.Text = reader.GetValue(2).ToString();
                    Matricule.Text = reader.GetValue(3).ToString();
                    Adresse.Text = reader.GetValue(8).ToString();
                    Infos.Text = reader.GetValue(11).ToString();
                    Lieunaiss.Text = reader.GetValue(5).ToString();
                    Sexe.Text = reader.GetValue(6).ToString();
                    Nationalite.Text = reader.GetValue(7).ToString();
                    Maladie.Text = reader.GetValue(9).ToString();
                    Eps.Text = reader.GetValue(10).ToString();
                    Nompere.Text = reader.GetValue(13).ToString();
                    Nommere.Text = reader.GetValue(15).ToString();
                    Nomtuteur.Text = reader.GetValue(17).ToString();
                    Phonep.Text = reader.GetValue(14).ToString();
                    Phonem.Text = reader.GetValue(16).ToString();
                    Phonet.Text = reader.GetValue(18).ToString();
                    //DateTime datecreer = new DateTime(reader.GetValue(13).ToString("dd/MM/yyyy"));
                    Datenaiss.Text = string.Format("{0:dd / MM / yyyy}", reader.GetValue(4));
                    Adddate.Text = string.Format("{0:dd / MM / yyyy}", reader.GetValue(19));
                    EditDate.Text = string.Format("{0:dd / MM / yyyy}", reader.GetValue(20));

                    // Affichage de mon image converti par la methode convertByteArrayToImage
                    if(reader.GetValue(12).ToString() == "")
                    {

                    }else
                    {
                        Photo.Image = CovertByteArrayToImage((byte[])(reader.GetValue(12)));
                    }
                    
                    /* 
                     * Code d'affichage d'une image depuis un repertoire grace a son nom stocker dans la base de données
                      string chainec = Path.GetFileName(reader.GetValue(9).ToString());
                      string chemin = Environment.CurrentDirectory + "\\img\\" + chainec;
                      bool fileexist = File.Exists(chemin);
                      if (fileexist)
                      {
                      Logo.Image = System.Drawing.Image.FromFile(Environment.CurrentDirectory+"\\img\\"+ chainec);
                      }*/
                }
                reader.Close();
            }
            //Methode de Convertion du byte Array provenant de la base de données en image à affiché dans ma picturebox
            private Image CovertByteArrayToImage(byte[] data)
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    return Image.FromStream(ms);
                }
            }
        }
        public detailsEleve(int idligne)
        {
            InitializeComponent();
            connexionDB();
            Detail newdet = new Detail(textBoxNom, textBoxEmail, textBoxLocalisation, textBox1, textBox2, sexe, labelPhone1, labelPhone2, datenaiss, label18, label20, cv, diplome, label22, fonction, specialite, label24, datemodif, datecreation, pictureBoxphoto);
            newdet.detailinfo(idligne);
        }

        private void numurgence_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void detailsEleve_Paint(object sender, PaintEventArgs e)
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
