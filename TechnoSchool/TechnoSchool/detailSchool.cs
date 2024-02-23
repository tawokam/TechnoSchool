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
    public partial class detailSchool : Form
    {
        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
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
        // Class pour la gestion des arrondi

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

        // Récupération des informations dépuis la BD grace à l'id
        public class Detail
        {
            private TextBox nom, email, localisation, bp, niu, nrc;
            private Label phone1, phone2, om, momo, paie, addDate, editDate;
            private PictureBox logo;
            private TextBox Nom { set; get; }
            private TextBox Email { set; get; }
            private TextBox Localisation { set; get; }
            private TextBox Bp { set; get; }
            private TextBox Niu { set; get; }
            private TextBox Nrc { set; get; }
            private Label Phone1 { set; get; }
            private Label Phone2 { set; get; }
            private Label Om { set; get; }
            private Label Momo { set; get; }
            private Label Paie { set; get; }
            private Label AddDate { set; get; }
            private Label EditDate { set; get; }
            private PictureBox Logo { set; get; }
            // Constructeur
            public Detail(TextBox nom, TextBox email, TextBox localisation, TextBox bp, TextBox niu, TextBox nrc, Label phone1, Label phone2, Label om, Label momo, Label paie, Label adddate, Label editdate, PictureBox logo)
            {
                this.Nom = nom; this.Email = email; this.Localisation = localisation; this.Bp = bp; this.Niu = niu;
                this.Nrc = nrc; this.Phone1 = phone1; this.Phone2 = phone2; this.Om = om; this.Momo = momo; this.Paie = paie;
                this.AddDate = adddate; this.EditDate = editdate; this.Logo = logo;
            }

            // Récuperation des informations dans la base de données
            public void detailinfo(int idline)
            {
                
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "SELECT * FROM etablissement where id_etablissement = @id";
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
                    Email.Text = reader.GetValue(2).ToString();
                    Localisation.Text = reader.GetValue(3).ToString();
                    Bp.Text = reader.GetValue(4).ToString();
                    Phone1.Text = reader.GetValue(5).ToString();
                    Phone2.Text = reader.GetValue(6).ToString();
                    Niu.Text = reader.GetValue(7).ToString();
                    Nrc.Text = reader.GetValue(8).ToString();
                    Paie.Text = reader.GetValue(10).ToString();
                    Om.Text = reader.GetValue(11).ToString();
                    Momo.Text = reader.GetValue(12).ToString();
                    //DateTime datecreer = new DateTime(reader.GetValue(13).ToString("dd/MM/yyyy"));

                    AddDate.Text = string.Format("{0:dd / MM / yyyy}", reader.GetValue(13));
                    EditDate.Text = string.Format("{0:dd / MM / yyyy}", reader.GetValue(14));

                    // Affichage de mon image converti par la methode convertByteArrayToImage
                    Logo.Image = CovertByteArrayToImage((byte[])(reader.GetValue(9)));
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
            public detailSchool(int idligne)
        {
            InitializeComponent();
            connexionDB();
            Detail newdet = new Detail(textBoxNom, textBoxEmail, textBoxLocalisation, textBoxBp, textBoxNiu, textBoxNrc, labelPhone1, labelPhone2, labelOm, labelMomo, labelPaie, creationDate, EditDate,pictureBoxLgo);
            newdet.detailinfo(idligne);
        }


        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void detailSchool_Load(object sender, EventArgs e)
        {
         
        }

        private void detailSchool_Paint(object sender, PaintEventArgs e)
        {
            designBordure form = new designBordure();
            form.FormRegionAndBorder(this, 10, e.Graphics, Color.Blue, 1);
        }

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            designBordure form = new designBordure();
            form.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 5, e.Graphics, Color.Blue, 1);
        }
    }

  
}
