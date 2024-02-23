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
    public partial class Connexion : Form
    {
        // ---------------
        private int borderRadius = 20;
        private int borderSize = 1;
        private Color borderColor = Color.RoyalBlue;

        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataReader reader3;
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
          //  private int borderRadius = 20;
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

        // Authentification d'un employer
        public class Connexion1
        {

            public long Phone1 { set; get; }
            public string Mdp { set; get; }
            public TextBox Nomemployer { set; get; }
            public Label Poste { set; get; }
            public PictureBox Photoemployer { set; get; }
            public Connexion1(long phone1, string mdp, TextBox nomem, Label poste, PictureBox photo)
            {
                this.Phone1 = phone1; this.Mdp = mdp; this.Nomemployer = nomem;
                this.Poste = poste; this.Photoemployer = photo;
            }

            // verifications 
            public void authentif()
            {
                int nbre = 0;
                string nomemployer = "";
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                string Idemployer = "";
                string requete = "Select telephone1_employer,mdpemployer,count(telephone1_employer) as nbre,nom_employer,id_employer from employer where telephone1_employer=@phone AND mdpemployer=@mdp ";
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@phone", Phone1);
                para.Add("@mdp", Mdp);
                command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parametres in para)
                {
                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                }
                command.Prepare();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nbre = int.Parse(reader.GetValue(2).ToString());
                    nomemployer = reader.GetValue(3).ToString();
                    Idemployer = reader.GetValue(4).ToString();
                }
                reader.Close();
                if(nbre < 1)
                {
                    string messag = "Nous n'avons pas trouvé d'employer ayant ces informations.  ";
                    string titre = "Authentification";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }else
                {
                    string messag = "Bienvenue dans l'application TechnoSchool Mr/Mme " + nomemployer + " ";
                    string titre = "Authentification";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var connect = new MySqlConnection(connectionstring);
                    connect.Open();
                    string ingfo = "select employer.nom_employer,employer.photo_employer,employer.id_fonction,fonction.id_fonction,fonction.nom_fonction from employer inner join fonction on employer.id_fonction=fonction.id_fonction where employer.id_employer=@idemployer";
                    Dictionary<string, object> param = new Dictionary<string, object>();
                    param.Add("@idemployer", Idemployer);
                    var command2 = new MySqlCommand(ingfo, connect);
                    foreach (KeyValuePair<string, object> parametres in param)
                    {
                        command2.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    command2.Prepare();
                    reader3 = command2.ExecuteReader();
                    while (reader3.Read())
                    {
                        Photoemployer.Image = CovertByteArrayToImage((byte[])(reader3.GetValue(1)));
                        Nomemployer.Text = reader3.GetValue(0).ToString();
                        Poste.Text = reader3.GetValue(4).ToString();
                        
                    }
                    reader3.Close();
                    
                }
        }
            //Methode de Convertion du byte Array provenant de la base de données en image à affiché dans ma picturebox
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
        }
        public static Index Form;
        public Connexion(Index form1)
        {
            InitializeComponent();
            connexionDB();
            Form = form1;

        }

        private void Connexion_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if(textBox2.PasswordChar == '*')
            {
                textBox2.PasswordChar = ' ';
            }else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 8, e.Graphics, borderColor, borderSize);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel2, 8, e.Graphics, borderColor, borderSize);
        }

        private void panelDegrader1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelDegrader1, 8, e.Graphics, borderColor, borderSize);
        }

        private void panelDegrader1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                string messag = "Veuillez entrer votre téléphone 1.";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(textBox2.Text == "")
            {
                string messag = "Veuillez entrer votre mot de passe.";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                long phone = 0;
                if (textBox1.Text == "")
                {
                    phone = 0;
                }
                else
                {
                    phone = long.Parse(textBox1.Text);
                }
                string mdp = textBox2.Text;
                Connexion1 connect = new Connexion1(phone, mdp,Form.textBox1, Form.label14, Form.pictureBox1);
                connect.authentif();
            }
           

        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                string messag = "Veuillez entrer votre téléphone 1.";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox2.Text == "")
            {
                string messag = "Veuillez entrer votre mot de passe.";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                long phone = 0;
                if (textBox1.Text == "")
                {
                    phone = 0;
                }
                else
                {
                    phone = long.Parse(textBox1.Text);
                }
                string mdp = textBox2.Text;
                Connexion1 connect = new Connexion1(phone, mdp, Form.textBox1, Form.label14, Form.pictureBox1);
                connect.authentif();
            }
        }
    }
}
