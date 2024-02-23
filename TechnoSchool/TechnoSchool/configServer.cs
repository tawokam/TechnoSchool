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
    public partial class configServer : Form
    {
        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
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
        // class de récupération des données de connexion dans le fichier pour l'affiché dans le formulaire de configuration
        public class Infoconnect
        {
            public TextBox Servername { set; get; }
            public TextBox Username { set; get; }
            public TextBox Mdp { set; get; }
            public Infoconnect(TextBox name, TextBox user, TextBox mdp)
            {
                this.Servername = name; this.Username = user; this.Mdp = mdp;
            }
            // données provenant du fichier de configuration du serveur
            public void datacinfig()
            {
                try
                {
                
                    // recuperation des informations de connexion dans le fichier ini
                    string cheminfichierConfig = Path.Combine(Environment.CurrentDirectory, "FileConfig/FileConfig.ini");
                    if (File.Exists(cheminfichierConfig))
                    {
                        // Si oui, rien ne se passe
                        GestionFileIni ger = new GestionFileIni(cheminfichierConfig);
                        // lecture des informations dans le fichier
                        Servername.Text = ger.ReadIni("Server", "server");
                        Username.Text = ger.ReadIni("User", "user");
                        Mdp.Text = ger.ReadIni("Mdp", "mdp");
                      
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
                catch(Exception ex)
                {
                    string messag = "Le fichier de configuration est absent. Veuillez contacter votre fournisseur" +ex;
                    string titre = "Configuration";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               

            }
        }
        // methode pour avoir la liste des bases de données creer
        public void listdb(ComboBox ListDB)
        {
            string cheminfichierConfig = Path.Combine(Environment.CurrentDirectory, "FileConfig/FileConfig.ini");
            GestionFileIni ger = new GestionFileIni(cheminfichierConfig);
            string db = ger.ReadIni("DataBase", "base de donnees");
            string connectionstring = "server=localhost;user id=root;password=; SslMode=none";
            var connection = new MySqlConnection(connectionstring);
            connection.Open();
            string li = "SHOW DATABASES";
            command = new MySqlCommand(li, connection);
            command.Prepare();
            reader = command.ExecuteReader();
            ListDB.Items.Clear();
            while (reader.Read())
            {
                if (reader.GetValue(0).ToString() == db)
                {
                    ListDB.Items.Add(reader.GetValue(0).ToString());
                    ListDB.SelectedItem = (reader.GetValue(0).ToString());
                }
                else
                {
                    ListDB.Items.Add(reader.GetValue(0));
                }

            }
            reader.Close();
        }
        // fonction de creation d'une nouvelle base de données
        public void newDB(string nameDB)
        {
            try
            {
                string connectionstring = "server=localhost;user id=root;password=; SslMode=none";
                var connection = new MySqlConnection(connectionstring);
                connection.Open();

                string db = "CREATE DATABASE '" + nameDB + "'";
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@namedb", nameDB);
                command = new MySqlCommand(db, connection);
                foreach (KeyValuePair<string, object> parametre in para)
                {
                    command.Parameters.Add(new MySqlParameter(parametre.Key, parametre.Value));
                }
                MessageBox.Show(command.ToString());
                command.Prepare();
                if (command.ExecuteNonQuery() >= 1)
                {
                    string messag = "Base de données crée avec succès";
                    string titre = "Database";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
           catch(Exception ex)
            {
                string messag = "Erreur lors de la création de la base de données";
                string titre = "Database";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                connection.Close();
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
        // Class pour modifier les informations de connexion au serveur
        public class EditServer
        {

            public string Server { set; get; }
            public string User { set; get; }
            public string Mdp { set; get; }
            public string BD { set; get; }
            public EditServer(string server, string user, string mdp, string bd)
            {
                this.Server = server; this.User = user; this.Mdp = mdp;this.BD = bd;
            }
            // methode pour modifier les informations du fichier FileConfig.ini
            public void modif()
            {
                string cheminfichierConfig = Path.Combine(Environment.CurrentDirectory, "FileConfig/FileConfig.ini");
                GestionFileIni files = new GestionFileIni(cheminfichierConfig);
                // Suppression des sections du fichier
                files.RemoveSection("Server");
                files.RemoveSection("User");
                files.RemoveSection("Mdp");
                files.RemoveSection("DataBase");
                // Enregistrement des nouveau informations
                files.WriteIni("Server", "server", Server);
                files.WriteIni("User", "user", User);
                files.WriteIni("Mdp", "mdp", Mdp);
                files.WriteIni("DataBase", "base de donnees", BD);
                string messag = "Le serveur a été configurer avec succès";
                string titre = "Serveur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // Méthode de creation du dataset et des tables du dataset

        public configServer()
        {
            InitializeComponent();
            listdb(listfonction);
            Infoconnect con = new Infoconnect(nomSchool, textBox1, textBox2);
            con.datacinfig();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel2, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel3, 5, e.Graphics, Color.Blue, 1);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button2, 7, e.Graphics, Color.Blue, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (nomSchool.Text == "")
            {
                string messag = "Veuillez entrer l'adresse du serveur ";
                string titre = "Erreur Serveur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if(textBox1.Text == "")
            {
                string messag = "Veuillez entrer le nom d'utilisateur du serveur";
                string titre = "Erreur Serveur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string server = nomSchool.Text; string user = textBox1.Text; string mdp = textBox2.Text;
                string db = listfonction.Text;
                EditServer modifier = new EditServer(server,user,mdp,db);
                modifier.modif();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox3.Text == "")
            {
                string messag = "Veuillez entrer le nom de la nouvelle base de données";
                string titre = "Erreur Database";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Création de la base de données
                string named = textBox3.Text;
                newDB(named);

            }
           
        }

        private void listfonction_Click(object sender, EventArgs e)
        {
            listdb(listfonction);
        }
    }
}
