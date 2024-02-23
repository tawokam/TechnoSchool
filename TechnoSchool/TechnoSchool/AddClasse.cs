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

    public partial class AddClasse : Form
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
        // Donnees de connexion
        public void connexionDB()
        {
            try
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

                    string messag = "Le fichier de configuration est absent";
                    string titre = "File Config";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }
            catch(FileLoadException ex)
            {
                string messag = "Nous n'avons pas pu lire correctement le fichier de configuration";
                string titre = "File Config";
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
        // Methode d'affichage des section crée
        private void listsection(ComboBox listfonction)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            try
            {
                string requete = "Select nom_section from tabsection order by nom_section asc";
                command = new MySqlCommand(requete, connection);
                command.Prepare();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listfonction.Items.Add(reader["nom_section"]);
                }
                reader.Close();
            }
            catch(MySqlException ex)
            {
                string messag = "Nous n'avons pas pu récupérer la liste des sections";
                string titre = "Duplication";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally { connection.Close(); }
            
           
        }
        // creation de la class d'insertion d'une nouvelle classe
        public class InsertClasse
        {

            private string Nomclasse { get; set; }
            private int Section { get; set; }
            private long Montinscript { get; set; }
            private long Montscolarite { get; set; }
            public int siDonneesValide = 0;
            // Constructeur
            public InsertClasse(string nom, int section, long inscript, long scolarite)
            {
                this.Nomclasse = nom; this.Section = section; this.Montinscript = inscript;
                this.Montscolarite = scolarite;
                insertdb();
            }
            private void insertdb()
            {
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                try
                {
                    int nbreetabli = 0;
                    // verifions si un établissement ayant ce nom a déja été crée
                    string req = "Select nom_classe,id_classe,count(nom_classe) as nbreclasse from classe where nom_classe=@nom ";
                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("@nom", Nomclasse);

                    var commande = new MySqlCommand(req, connection);
                    foreach (KeyValuePair<string, object> parametres in para)
                    {
                        commande.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    commande.Prepare();

                    reader2 = commande.ExecuteReader();
                    while (reader2.Read())
                    {
                        nbreetabli = int.Parse(reader2.GetValue(2).ToString());

                    }
                    reader2.Close();
                    if (nbreetabli < 1)
                    {
                        string requete2 = "INSERT INTO classe(nom_classe,montscolarite,montinscription,id_section,date_creation,date_modification) values (@nom,@scolarite,@inscription,@section,@datecre,@datemod)";
                        string datec = DateTime.Now.ToString("yyyy-MM-dd");
                        string datem = DateTime.Now.ToString("yyyy-MM-dd");
                        Dictionary<string, object> parametre = new Dictionary<string, object>();
                        parametre.Add("@nom", Nomclasse);
                        parametre.Add("@section", Section);
                        parametre.Add("@scolarite", Montscolarite);
                        parametre.Add("@inscription", Montinscript);
                        parametre.Add("@datecre", datec);
                        parametre.Add("@datemod", datem);
                        var command2 = new MySqlCommand(requete2, connection);
                        foreach (KeyValuePair<string, object> parametres in parametre)
                        {
                            command2.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                        }
                        command2.Prepare();
                        if (command2.ExecuteNonQuery() >= 1)
                        {
                            string messag = "La classe a été enregistrer avec succès. ";
                            string titre = "Insertion ";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            siDonneesValide = 1;
                        }
                        else
                        {
                            string messag = "Nous nous sommes heurtés à un problème lors de l'enregistrement de cette classe.  ";
                            string titre = "Insertion ";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            siDonneesValide = 0;
                        }
                    }
                    else
                    {
                        string messag = "Il existe déjà une classe ayant ce nom.";
                        string titre = "Duplication";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        siDonneesValide = 0;
                    }
                    reader2.Close();
                }
                catch(MySqlException ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de l'enregistrement de cette classe.  ";
                    string titre = "Insertion ";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally { connection.Close(); }

            }
        }

        // Methode pour verifier si la chaine de caractères ne contient que des nombre
        public int VerifiSiNombre(string chaine)
        {
            /* declaration de la variable valide qui renvoi 
             * 1 si chaine valide( ne contient que des chiffre [0,9]
             * 0 si chaine invalide ( contient une lettre ou un caractère spécial
             */
            int valide = 0;
            for (int i = 0; i <= chaine.Length - 1; i++)
            {
                if (chaine[i] == '1' || chaine[i] == '2' || chaine[i] == '3' || chaine[i] == '4' || chaine[i] == '5' || chaine[i] == '6' || chaine[i] == '7' || chaine[i] == '8' || chaine[i] == '9' || chaine[i] == '0')
                {
                    valide = 1;
                }
                else
                {
                    valide = 0;
                    break;
                }
            }
            return valide;
        }
        public AddClasse()
        {
            InitializeComponent();
            connexionDB();
            listsection(comboBox2);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddClasse_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                designBordure radiusform = new designBordure();
                radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
            }
           catch(Exception ex)
            {
                string messag = "Nous avons rencontrer un problème lors de la création de l'interface utilisateur. "+ex;
                string titre = "Interface utilisateur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                designBordure arrondirpanel = new designBordure();
                arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
            }
           catch(Exception ex)
            {
                string messag = "Nous avons rencontrer un problème lors de la création de l'interface utilisateur. " + ex;
                string titre = "Interface utilisateur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                designBordure arrondirpanel = new designBordure();
                arrondirpanel.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 8, e.Graphics, borderColor, borderSize);
            }
            catch(Exception ex)
            {
                string messag = "Nous avons rencontrer un problème lors de la création de l'interface utilisateur. " + ex;
                string titre = "Interface utilisateur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Récupération et traitement des informations saisi par l'utilisateur pour la création d'un établissement scolaire
            if (nomSchool.Text == "")
            {
                string messag = "Veuillez entrez le nom de la classe. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (comboBox2.Text == "")
            {
                string messag = "Veuillez séléctionner la section . ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (localisation.Text == "")
            {
                string messag = "Veuillez entrer le montant de l'inscription. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox1.Text == "")
            {
                string messag = "Veuillez entrer le montant de la scolarité. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (VerifiSiNombre(localisation.Text) == 0)
            {
                string messag = "Montant de l'inscription incorrect ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (VerifiSiNombre(textBox1.Text) == 0)
            {
                string messag = "Montant de la scolarité incorrect ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                string nom = nomSchool.Text;
                int section = int.Parse(label3.Text);
                long inscription = long.Parse(localisation.Text);
                long scolarite = long.Parse(textBox1.Text);
                InsertClasse newclasse = new InsertClasse(nom, section, inscription, scolarite);
                int valueret = newclasse.siDonneesValide;
                if(valueret == 1)
                {
                    nomSchool.Text = ""; localisation.Text = 0.ToString(); textBox1.Text = 0.ToString();
                }

            }
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                designBordure arrondirpanel = new designBordure();
                arrondirpanel.FormRegionAndBorderbouton(button3, 8, e.Graphics, borderColor, borderSize);
            }
            catch(Exception ex)
            {
                string messag = "Nous avons rencontrer un problème lors de la création de l'interface utilisateur. " + ex;
                string titre = "Interface utilisateur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                designBordure arrondirpanel = new designBordure();
                arrondirpanel.FormRegionAndBorderbouton(button2, 8, e.Graphics, borderColor, borderSize);
            }
            catch(Exception ex)
            {
                string messag = "Nous avons rencontrer un problème lors de la création de l'interface utilisateur. " + ex;
                string titre = "Interface utilisateur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            try
            {
                string requete = "Select id_section,nom_section from tabsection where nom_section = @nom ";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@nom", comboBox2.Text);
                command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));

                }
                command.Prepare();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    label3.Text = (reader["id_section"]).ToString();
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                string messag = "Impossible d'affiché la section " + ex;
                string titre = "Interface utilisateur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { connection.Close(); }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            nomSchool.Text = "";
            localisation.Text = "";
            textBox1.Text = "";
        }

        private void localisation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                designBordure arrondirpanel = new designBordure();
                arrondirpanel.FormRegionAndBorderpanel(panel2, 5, e.Graphics, Color.Blue, 1);
            }
            catch(Exception ex)
            {
                string messag = "Nous avons rencontrer un problème lors de la création de l'interface utilisateur. " + ex;
                string titre = "Interface utilisateur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                designBordure arrondirpanel = new designBordure();
                arrondirpanel.FormRegionAndBorderpanel(panel11, 5, e.Graphics, Color.Blue, 1);
            }
            catch(Exception ex)
            {
                string messag = "Nous avons rencontrer un problème lors de la création de l'interface utilisateur. " + ex;
                string titre = "Interface utilisateur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                designBordure arrondirpanel = new designBordure();
                arrondirpanel.FormRegionAndBorderpanel(panel4, 5, e.Graphics, Color.Blue, 1);
            }
            catch(Exception ex)
            {
                string messag = "Nous avons rencontrer un problème lors de la création de l'interface utilisateur. " + ex;
                string titre = "Interface utilisateur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                designBordure arrondirpanel = new designBordure();
                arrondirpanel.FormRegionAndBorderpanel(panel3, 5, e.Graphics, Color.Blue, 1);
            }
            catch(Exception ex)
            {
                string messag = "Nous avons rencontrer un problème lors de la création de l'interface utilisateur. " + ex;
                string titre = "Interface utilisateur";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
