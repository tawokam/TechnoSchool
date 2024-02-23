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
    public partial class EditBoursePrime : Form
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

                string messag = "Nous nous somme heurté à un problème lors de la connexion au serveur";
                string titre = "Achat licence ";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);

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
            //private int borderRadius = 20;
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
        // Class pour afficher les informations sur la bourse ou prime séléctionné
        public class Info
        {
            public ComboBox Type { set; get; }
            public TextBox Motif { set; get; }
            public ComboBox Session { set; get; }
            public TextBox Montant { set; get; }
            public string Idbourse { set; get; }
            // Constroctor 
            public Info(ComboBox type, TextBox motif, ComboBox session, TextBox montant, string idbourse)
            {
                this.Type = type; this.Motif = motif; this.Session = session; this.Montant = montant;
                this.Idbourse = idbourse;
            }

            // Methodes
            public void affichData()
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                try
                {
                    string req = "SELECT type,motif,montant,session FROM bourse WHERE id_bourse='" + Idbourse + "'";
                    command = new MySqlCommand(req, connection);
                    command.Prepare();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if(reader.GetValue(0).ToString() == "Bourse")
                        {
                            Type.SelectedItem = "Bourse";
                        }else if(reader.GetValue(0).ToString() == "Prime")
                        {
                            Type.SelectedItem = "Prime";
                        }
                        Montant.Text = reader.GetValue(2).ToString();
                        Motif.Text = reader.GetValue(1).ToString();
                        Session.SelectedItem = reader.GetValue(3).ToString();
                    }
                    reader.Close();
                }
                catch(Exception ex)
                {
                    string messag = "Nous nous somme heurté à un problème lors de l'affichage des données de la bourse ou prime "+ex;
                    string titre = "Modification";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally { connection.Close(); }
            }
        }

        // Création d'une classe fille de la classe info pour récupéré les informations de la classe Info
        public class InsertData : Info
        {
            public InsertData(ComboBox type, TextBox motif, ComboBox session, TextBox montant, string idbourse) : base(type, motif, session, montant, idbourse)
            {
                this.Type = type; this.Motif = motif; this.Session = session; this.Montant = montant;
                this.Idbourse = idbourse;
            }

            public void Insert()
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                try
                {
                    string req = "UPDATE bourse SET type=@type, motif=@motif, session=@session, montant=@montant WHERE id_bourse=@idbours ";
                    command = new MySqlCommand(req, connection);
                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("@type", Type.Text);
                    para.Add("@motif", Motif.Text);
                    para.Add("@session", Session.Text);
                    para.Add("@montant", Montant.Text);
                    para.Add("@idbours", Idbourse);
                    foreach(KeyValuePair<string,object> parametre in para)
                    {
                        command.Parameters.Add(new MySqlParameter(parametre.Key, parametre.Value));
                    }
                    command.Prepare();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        string messag = "Modification effectuer avec succès";
                        string titre = "Modification";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                       
                    }
                }
                catch(Exception ex)
                {
                    string messag = "Nous nous somme heurté à un problème lors de la modification de la "+Type.Text+ " " + ex;
                    string titre = "Modification";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally { connection.Close(); }
            }
        }
        // Liste des sessions
        private void listsession(ComboBox session)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            try
            {
                string requete = "Select nom_session from tabsession order by nom_session asc";
                command = new MySqlCommand(requete, connection);
                command.Prepare();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    session.Items.Add(reader["nom_session"]);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                string messag = "Nous nous somme heurté à un problème lors de l'affichage de la liste des sessions " + ex;
                string titre = "Modification";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally { connection.Close(); }
        }
        public string IdBourse = "";
        public EditBoursePrime(string idbourse)
        {
            InitializeComponent();
            connexionDB();
            listsession(comboBox1);
            IdBourse = idbourse;
            Info data = new Info(comboBox3, nationalite, comboBox1, adresse, idbourse);
            data.affichData();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditBoursePrime_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel5, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel7, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel15, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel9, 5, e.Graphics, Color.Blue, 1);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button2, 4, e.Graphics, borderColor, borderSize);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InsertData modif = new InsertData(comboBox3, nationalite, comboBox1, adresse, IdBourse);
            modif.Insert();
        }
    }
}
