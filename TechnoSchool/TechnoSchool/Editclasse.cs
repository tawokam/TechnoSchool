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
    public partial class Editclasse : Form
    {
        // ---------------
        private int borderRadius = 20;
        private int borderSize = 1;
        private Color borderColor = Color.RoyalBlue;

        public static MySqlConnection connection;
        public static MySqlConnection connection2;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataReader reader2;
        public static MySqlDataReader reader3;
        public static MySqlDataReader reader4;
        public static MySqlDataReader reader5;
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
                connectionstring = "server=" + server + ";user id=" + user + ";password=" + mdp + ";database=" + DB + "; SslMode=none;";

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

        public class Modification
        {
            private TextBox nom, inscription, scolarite;
            private ComboBox section;
            private Label idligneform;
            private Label idsectionOpen;
            private TextBox Nom { get; set; }
            private TextBox Inscription { get; set; }
            private TextBox Scolarite { get; set; }
            private ComboBox Section { get; set; }
            private Label Idligneform { get; set; }
            private Label IdsectionOpen { get; set; }
            public static string idfonction = "";
            // Constructeur
            public Modification(TextBox nom, TextBox inscription, TextBox scolarite, ComboBox section, Label idligneform, Label idsectionopen)
            {
                this.Nom = nom; this.Inscription = inscription; this.Scolarite = scolarite; this.Section = section;
                this.Idligneform = idligneform; this.IdsectionOpen = idsectionopen;
            }

            // Récuperation des informations dans la base de données
            public void modifinfo(int idline, Label stockline)
            {
                stockline.Text = idline.ToString();
                stockline.Visible = false;

                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "Select classe.id_classe,classe.nom_classe,classe.montinscription,classe.montscolarite,classe.id_section,tabsection.id_section,tabsection.nom_section from classe inner join tabsection on classe.id_section=tabsection.id_section where classe.id_classe = @id";
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
                    Inscription.Text = reader.GetValue(2).ToString();
                    Scolarite.Text = reader.GetValue(3).ToString();
                    idfonction = reader.GetValue(4).ToString();
                    IdsectionOpen.Text = reader.GetValue(4).ToString();
                    Idligneform.Text = idline.ToString();

                }
                reader.Close();
            }  // ---------
               // Affichage de la liste des fonction avec séléction automatique de la fonction de l'employer
            public void listfonction()
            {
                
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "Select id_section, nom_section from tabsection order by nom_section asc";
                command = new MySqlCommand(requete, connection);
                command.Prepare();
                reader2 = command.ExecuteReader();
                while (reader2.Read())
                {
                    if (reader2.GetValue(0).ToString() == idfonction)
                    {
                        Section.Items.Add(reader2.GetValue(1).ToString());
                        Section.SelectedItem = (reader2.GetValue(1).ToString());
                      
                    }
                    else
                    {
                        Section.Items.Add(reader2.GetValue(1).ToString());
                    }
                }
                reader2.Close();
            }
        }

        // --------------
        // class Insertion des modifications apporté aux données
        public class InsertModifClasse
        {


            private string nom;
            private long idsection, idligneform, inscription, scolarite;
            private string Nom { get; set; }
            private long Idsection { get; set; }
            private long Inscription{ get; set; }
            private long Scolarite { get; set; }
            private long Idligneform { get; set; }
            // Constructeur
            public InsertModifClasse(string nom, long idsection, long inscription, long scolarite, long idligneform)
            {
                this.Nom = nom; this.Idsection = idsection; this.Inscription = inscription; this.Scolarite = scolarite;
                this.Idligneform = idligneform;
                insertdb();
            }
            private void insertdb()
            {
                //Création d'une transaction
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                connection2 = new MySqlConnection(connectionstring);
                connection2.Open();
                MySqlTransaction trans;
                trans = connection.BeginTransaction();
                MySqlTransaction trans2;
                trans2 = connection2.BeginTransaction();
                try
                {
                    int nbreetabli = 0;
                    // verifions si un établissement ayant ce nom a déja été crée
                    string req = "Select id_classe,nom_classe,count(nom_classe) as nbreschool from classe where id_classe <> @idligne AND nom_classe = @nomclasse";
                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("@nomclasse", Nom);
                    para.Add("@idligne", Idligneform);
                    command = new MySqlCommand(req, connection);
                    foreach (KeyValuePair<string, object> parametres in para)
                    {
                        command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    command.Prepare();
                    command.Transaction = trans;
                    reader2 = command.ExecuteReader();
                    while (reader2.Read())
                    {
                        nbreetabli = int.Parse(reader2.GetValue(2).ToString());

                    }
                    reader2.Close();
                    if (nbreetabli < 1)
                    {

                        string requete = "UPDATE classe SET nom_classe = @nom, montinscription = @inscription, montscolarite = @scolarite, id_section = @idsection, date_modification = @datemodif WHERE id_classe = @identLigne";
                        string datem = DateTime.Now.ToString("yyyy-MM-dd");
                        Dictionary<string, object> parametre = new Dictionary<string, object>();
                        parametre.Add("@nom", Nom);
                        parametre.Add("@inscription", Inscription);
                        parametre.Add("@scolarite", Scolarite);
                        parametre.Add("@idsection", Idsection);
                        parametre.Add("@datemodif", datem);
                        parametre.Add("@identLigne", Idligneform);
                        command = new MySqlCommand(requete, connection);
                        foreach (KeyValuePair<string, object> parametres in parametre)
                        {
                            command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));

                        }
                        command.Prepare();
                        command.Transaction = trans;
                        command.ExecuteNonQuery();
                            // Récupération de la session active
                            string value = "";
                            int nbre;
                            string requete2 = "Select nom_session,count(nom_session) as nbre from tabsession where statut='activer'";
                            command = new MySqlCommand(requete2, connection);
                            command.Prepare();
                            command.Transaction = trans;
                            reader2 = command.ExecuteReader();
                            while (reader2.Read())
                            {
                                nbre = int.Parse(reader2.GetValue(1).ToString());
                                if (nbre == 1)
                                {
                                    value = reader2.GetValue(0).ToString();
                                }
                                else
                                {
                                    value = "non";

                                }
                            }
                            reader2.Close();
                            /* Compte le nombre d'inscription de la section active dont le montant
                             * versé est supérieur au nouveau montant de la classe
                             */

                            // Modification de l'inscription
                            int nbreinscrip = 0;
                            string verif = "select id_inscription, count(id_inscription) as nbre from inscription where montverse > @newmontins AND session=@session AND id_classe=@idclasse";
                            command = new MySqlCommand(verif, connection);
                            Dictionary<string, object> para2 = new Dictionary<string, object>();
                            para2.Add("@session", value);
                            para2.Add("@newmontins", Inscription);
                            para2.Add("@idclasse", Idligneform);
                            foreach (KeyValuePair<string, object> parametres in para2)
                            {
                                command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                            }
                            command.Prepare();
                            command.Transaction = trans;
                            reader2 = command.ExecuteReader();
                            while (reader2.Read())
                            {
                                nbreinscrip = int.Parse(reader2.GetValue(1).ToString());

                            }
                            reader2.Close();
                            // Vérifi le nombre de ligne renvoyer
                            if (nbreinscrip < 1)
                            {
                                /* Aucune ligne renvoyé dont il ya pas de versement supérieur au montant 
                                 * dans ce cas nous passons à la modification du montant de l'inscription
                                 * et le calcul du nouveau reste a versé par les differents élèves inscript
                                 * sur cette session
                                 */
                                //--------------------------------
                                // Séléction pour récupéré ligne par ligne et les modifier
                                string mod = "SELECT montinscription,montverse,reste,id_inscription from inscription where session=@session AND id_classe=@idclasse";
                                command = new MySqlCommand(mod, connection);
                                Dictionary<string, object> para3 = new Dictionary<string, object>();
                                para3.Add("@session", value);
                                para3.Add("@idclasse", Idligneform);
                                foreach (KeyValuePair<string, object> parametres in para3)
                                {
                                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                                }
                                command.Prepare();
                                command.Transaction = trans;
                                reader4 = command.ExecuteReader();
                                while (reader4.Read())
                                {
                                    string montverse = reader4.GetValue(1).ToString();
                                    string idInscript = reader4.GetValue(3).ToString();
                                    long reste = (Inscription - int.Parse(montverse));
                                    string statut = "";
                                    if (reste == 0) { statut = "Soldé"; } else if (reste > 0) { statut = "Non soldé"; }
                                    // Modification des inscriptions de chaque élève concerné
                                    string up = "UPDATE inscription SET montinscription=@mont, reste=@reste,statut=@statut where id_inscription=@idinscript";
                                    Dictionary<string, object> paramet = new Dictionary<string, object>();
                                    paramet.Add("@mont", Inscription);
                                    paramet.Add("@reste", reste);
                                    paramet.Add("@idinscript", idInscript);
                                    paramet.Add("@statut", statut);
                                    var command2 = new MySqlCommand(up, connection2);
                                    foreach (KeyValuePair<string, object> parametres in paramet)
                                    {
                                        command2.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                                    }
                                    command2.Prepare();
                                    command2.Transaction = trans2;
                                    command2.ExecuteNonQuery();
                                }
                                reader4.Close();

                            }
                            else
                            {


                                // Un versement effectuer par l'élève est supérieur au frais d'inscription de cette classe
                                string messag = @"Un ou plusieurs élève ont versé un montant supérieur au nouveau montant d'inscription pour cette classe. 
  Veuillez diminuer le montant pour inscription pour l'ajouter comme versement pour la scolarité et ensuite vous pourrez
modifier le montant de l'inscription de cette classe";
                                string titre = "Modification ";
                                // Programation des bouton de la boite de message
                                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // Lancer l'exception 
                            throw new Exception();
                        }
                            // Modification de la scolarité
                            int nbrescolarite = 0;
                            string verifscol = "select id_scolarite, count(id_scolarite) as nbre from scolarite where montverse > @newmontins AND session=@session AND id_classe=@idclasse";
                            command = new MySqlCommand(verifscol, connection);
                            Dictionary<string, object> para2s = new Dictionary<string, object>();
                            para2s.Add("@session", value);
                            para2s.Add("@newmontins", Scolarite);
                            para2s.Add("@idclasse", Idligneform);
                            foreach (KeyValuePair<string, object> parametres in para2s)
                            {
                                command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                            }
                            command.Prepare();
                            command.Transaction = trans;
                            reader3 = command.ExecuteReader();
                            while (reader3.Read())
                            {
                                nbrescolarite = int.Parse(reader3.GetValue(1).ToString());

                            }
                            reader3.Close();
                            // Vérifi le nombre de ligne renvoyer
                            if (nbrescolarite < 1)
                            {
                                /* Aucune ligne renvoyé dont il ya pas de versement supérieur au montant 
                                 * dans ce cas nous passons à la modification du montant de la scolarité
                                 * et le calcul du nouveau reste a versé par les differents élèves inscript
                                 * sur cette session
                                 */
                                //--------------------------------
                                // Séléction pour récupéré ligne par ligne et les modifier
                                string mod = "SELECT montscolarite,montverse,reste,id_scolarite,matricule,session from scolarite where session=@session AND id_classe=@idclasse";
                                command = new MySqlCommand(mod, connection);
                                Dictionary<string, object> para3 = new Dictionary<string, object>();
                                para3.Add("@session", value);
                                para3.Add("@idclasse", Idligneform);
                                foreach (KeyValuePair<string, object> parametres in para3)
                                {
                                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                                }
                                command.Prepare();
                                command.Transaction = trans;
                                reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    string montverse = reader.GetValue(1).ToString();
                                    string idScolarite = reader.GetValue(3).ToString();
                                   
                                    string matricule = reader.GetValue(4).ToString();
                                    string session = reader.GetValue(5).ToString();
                                    string statut = "";


                                      
                                // Selection de la table bourse pour voir si l'utilisateur bénéficie d'une bourse/prime
                                long sibourse = 0;
                                string bour = "SELECT sum(montant) as montbourse FROM bourse WHERE matricule='" + matricule + "' AND session='" + session + "'";
                                command = new MySqlCommand(bour, connection2);
                                command.Prepare();
                                var reader2 = command.ExecuteReader();
                                while (reader2.Read())
                                {
                                    if(reader2.GetValue(0).ToString() == "null" || reader2.GetValue(0).ToString() == "")
                                    {
                                        sibourse = 0;
                                    }else
                                    {
                                        sibourse = int.Parse(reader2.GetValue(0).ToString());
                                    }
                                    
                                }
                                reader2.Close();
                                long newScolarite = (Scolarite - sibourse);
                                long reste = (newScolarite - int.Parse(montverse));
                                if (reste == 0) { statut = "Soldé"; } else if (reste > 0) { statut = "Non soldé"; }
                                    // Modification des inscriptions de chaque élève concerné
                                    string up = "UPDATE scolarite SET montscolarite=@mont, reste=@reste,statut=@statut where id_scolarite=@idscolarite";
                                    Dictionary<string, object> paramet = new Dictionary<string, object>();
                                    paramet.Add("@mont", newScolarite);
                                    paramet.Add("@reste", reste);
                                    paramet.Add("@idscolarite", idScolarite);
                                    paramet.Add("@statut", statut);
                                    command = new MySqlCommand(up, connection2);
                                    foreach (KeyValuePair<string, object> parametres in paramet)
                                    {
                                        command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                                    }
                                    command.Prepare();
                                    command.Transaction = trans2;
                                    command.ExecuteNonQuery();
                                }
                                reader.Close();

                            }
                            else
                            {
                                // Un versement effectuer par l'élève est supérieur au frais d'inscription de cette classe
                                string messag = @"Un ou plusieurs élève ont versé un montant supérieur au nouveau montant de la scolarité pour cette classe. 
  Veuillez modifier le montant ensuite vous pourrez
modifier le montant de la scolarite de cette classe";
                                string titre = "Modification ";
                           // trans.Rollback(); trans2.Rollback();
                                // Programation des bouton de la boite de message
                                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new Exception();
                            }
                       
                    }
                    else
                    {
                        string messag = "Il existe déjà une classe ayant ce nom .";
                        string titre = "Duplication";
                        //trans.Rollback();
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    trans.Commit(); trans2.Commit();
                    string messagok = "La classe a été modifié avec succès";
                    string titreok = "Modification";
                    //trans.Rollback();
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messagok, titreok, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception error)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de la modification de cette classe. "+error+"";
                    string titre = "Erreur";
                    trans.Rollback(); trans2.Rollback();
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                



            }
        }
        public Editclasse(int idligne)
        {
            InitializeComponent();
            connexionDB();
            Modification modifinfos = new Modification(nomSchool, inscription, scolarite, comboBox2, label5, label3);
            modifinfos.modifinfo(idligne, label5);
            modifinfos.listfonction();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Editclasse_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Récupération et traitement des informations saisi par l'utilisateur pour la création d'un établissement scolaire
            if (nomSchool.Text == "")
            {
                string messag = "Veuillez entrez le nom de la classe. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBox2.Text == "")
            {
                string messag = "Veuillez séléctionné la section. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (inscription.Text == "")
            {
                string messag = "Veuillez entrer le montant de l'inscription. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (scolarite.Text == "")
            {
                string messag = "Veuillez entrer le montant de la scolarité";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string nom = nomSchool.Text; int idsection = int.Parse(label3.Text);
                long inscript = long.Parse(inscription.Text); long scolarit = long.Parse(scolarite.Text);
                int idligneform = int.Parse(label5.Text);

                InsertModifClasse newschool = new InsertModifClasse(nom, idsection, inscript, scolarit, idligneform);
            }
        }

        private void inscription_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void scolarite_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            var connection = new MySqlConnection(connectionstring);
            connection.Open();
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

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 8, e.Graphics, borderColor, borderSize);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button2, 7, e.Graphics, Color.Blue, 1);
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

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel4, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel3, 5, e.Graphics, Color.Blue, 1);
        }
    }
}
