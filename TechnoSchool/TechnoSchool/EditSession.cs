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
    public partial class EditSession : Form
    {
        // ---------------
        private int borderRadius = 20;
        private int borderSize = 1;
        private Color borderColor = Color.RoyalBlue;

        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataReader reader2;
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
        public class Modification
        {
            private TextBox nom;
            private ComboBox statut;
            private Label idligneform;
            private TextBox Nom { get; set; }
            private ComboBox Statut { get; set; }
            private Label Idligneform { get; set; }
            // Constructeur
            public Modification(TextBox nom, ComboBox statut, Label idligneform)
            {
                this.Nom = nom; this.Statut = statut; this.Idligneform = idligneform;
            }

            // Récuperation des informations dans la base de données
            public void modifinfo(int idline, Label stockline)
            {
                stockline.Text = idline.ToString();
                stockline.Visible = false;

                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "Select id_session,nom_session,statut from tabsession where id_session = @id";
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
                    if (reader.GetValue(2).ToString() == "activer")
                    {
                        Statut.SelectedIndex = 0;
                    }
                    else
                    {
                        Statut.SelectedIndex = 1;
                    }

                }
                reader.Close();
            }
        }
        // class Insertion des modifications apporté aux données
        public class InsertModifSession
        {


            private string nom;
            private string statut;
            private long idligneform;
            private string Nom { get; set; }
            private string Statut { get; set; }
            private long Idligneform { get; set; }
            // Constructeur
            public InsertModifSession(string nom, string statut, long idligneform)
            {
                this.Nom = nom; this.Statut = statut; this.Idligneform = idligneform;
                insertdb();
            }
            private void insertdb()
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                MySqlTransaction trans;
                trans = connection.BeginTransaction();

                try
                {
                    // Recuperons l'ancien non de la session
                    string anciennom = "";
                    string ancnom = "SELECT nom_session FROM tabsession where id_session='" + Idligneform + "'";
                    command = new MySqlCommand(ancnom, connection);
                    command.Prepare();
                    command.Transaction = trans;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        anciennom = reader.GetValue(0).ToString();
                    }
                    reader.Close();
                    int nbreetabli = 0;
                    // verifions si un établissement ayant ce nom a déja été crée
                    string req = "Select nom_session,count(nom_session) as nbresession,id_session from tabsession where id_session <> @idligne AND nom_session=@nom";
                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("@idligne", Idligneform);
                    para.Add("@nom", Nom);
                    var connect = new MySqlConnection(connectionstring);
                    connect.Open();
                    var commande = new MySqlCommand(req, connect);
                    foreach (KeyValuePair<string, object> parametres in para)
                    {
                        commande.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    commande.Prepare();
                    commande.Transaction = trans;
                    reader2 = commande.ExecuteReader();
                    while (reader2.Read())
                    {
                        nbreetabli = int.Parse(reader2.GetValue(1).ToString());

                    }
                    if (nbreetabli < 1)
                    {
                        if (Statut == "desactiver")
                        {
                            string requete = "UPDATE tabsession SET nom_session = @nom, statut = @statut, date_modification = @datemodif WHERE id_session = @identLigne";
                            string datem = DateTime.Now.ToString("yyyy-MM-dd");
                            Dictionary<string, object> parametre = new Dictionary<string, object>();
                            parametre.Add("@nom", Nom);
                            parametre.Add("@statut", Statut);
                            parametre.Add("@datemodif", datem);
                            parametre.Add("@identLigne", Idligneform);
                            var command = new MySqlCommand(requete, connection);
                            foreach (KeyValuePair<string, object> parametres in parametre)
                            {
                                command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                            }
                            command.Prepare();
                            command.Transaction = trans;
                            if (command.ExecuteNonQuery() >= 1)
                            {
                                // modifier la table inscription
                                string modinsc = "UPDATE inscription SET session='" + Nom + "' WHERE session='" + anciennom + "'";
                                command = new MySqlCommand(modinsc, connection);
                                command.Transaction = trans;
                                command.ExecuteNonQuery();
                                // modifier la table scolarite
                                string modscol = "UPDATE scolarite SET session='" + Nom + "' WHERE session='" + anciennom + "'";
                                command = new MySqlCommand(modscol, connection);
                                command.Transaction = trans;
                                command.ExecuteNonQuery();
                                // modifier la table caissescolarite
                                string modcaissescol = "UPDATE caissescolarite SET session='" + Nom + "' WHERE session='" + anciennom + "'";
                                command = new MySqlCommand(modcaissescol, connection);
                                command.Transaction = trans;
                                command.ExecuteNonQuery();
                                
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        else if (Statut == "activer")
                        {
                            string requete1 = "UPDATE tabsession SET statut = @statut";
                            Dictionary<string, object> parametret = new Dictionary<string, object>();
                            parametret.Add("@statut", "desactiver");
                            var connection1 = new MySqlConnection(connectionstring);
                            connection1.Open();
                            var command1 = new MySqlCommand(requete1, connection1);
                            foreach (KeyValuePair<string, object> parametres in parametret)
                            {
                                command1.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                            }
                            command1.Prepare();
                            if (command1.ExecuteNonQuery() >= 1)
                            {
                                string requete = "UPDATE tabsession SET nom_session = @nom, statut = @statut, date_modification = @datemodif WHERE id_session = @identLigne";
                                string datem = DateTime.Now.ToString("yyyy-MM-dd");
                                Dictionary<string, object> parametre = new Dictionary<string, object>();
                                parametre.Add("@nom", Nom);
                                parametre.Add("@statut", Statut);
                                parametre.Add("@datemodif", datem);
                                parametre.Add("@identLigne", Idligneform);
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
                                    // modifier la table inscription
                                    string modinsc = "UPDATE inscription SET session='" + Nom + "' WHERE session='" + anciennom + "'";
                                    command = new MySqlCommand(modinsc, connection);
                                    command.Transaction = trans;
                                    command.ExecuteNonQuery();
                                    // modifier la table scolarite
                                    string modscol = "UPDATE scolarite SET session='" + Nom + "' WHERE session='" + anciennom + "'";
                                    command = new MySqlCommand(modscol, connection);
                                    command.Transaction = trans;
                                    command.ExecuteNonQuery();
                                    // modifier la table caissescolarite
                                    string modcaissescol = "UPDATE caissescolarite SET session='" + Nom + "' WHERE session='" + anciennom + "'";
                                    command = new MySqlCommand(modcaissescol, connection);
                                    command.Transaction = trans;
                                    command.ExecuteNonQuery();
                                   
                                }
                                else
                                {
                                   
                                }
                            }
                        }

                    }
                    else
                    {
                        string messag2 = "Il existe déjà une session ayant ce nom.";
                        string titre2 = "Duplication";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag2, titre2, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    reader2.Close();
                    trans.Commit();
                    string messag = "Les informations sur cet session ont été modifié avec succès. ";
                    string titre = "Modification ";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de la modification de cette session. ";
                    string titre = "Insertion ";
                    trans.Rollback();
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }



            }
        }
        public EditSession(int idligne)
        {
            InitializeComponent();
            connexionDB();
            Modification data = new Modification(nomsession, comboBox2, label1);
            data.modifinfo(idligne, label1);
         
        }

        private void fermFormAddSchool_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditSession_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 8, e.Graphics, borderColor, borderSize);

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel2, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel11, 5, e.Graphics, Color.Blue, 1);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button2, 7, e.Graphics, Color.Blue, 1);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Récupération et traitement des informations saisi par l'utilisateur pour la création d'un établissement scolaire
            if (nomsession.Text == "")
            {
                string messag = "Veuillez entrez le nom de la session. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string nom = nomsession.Text;
                string statut = comboBox2.Text;
                long idligne = long.Parse(label1.Text);
                InsertModifSession newschool = new InsertModifSession(nom, statut, idligne);
            }
        }
    }
}
