using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnoSchool
{
    public partial class AddInscription : Form
    {
        // ---------------
        private int borderRadius = 20;
        private int borderSize = 1;
        private Color borderColor = Color.RoyalBlue;

        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataReader reader2;
        public static MySqlDataReader reader2v;
        public static MySqlDataReader reader3;
        public static MySqlDataReader readerinfo;
        public static MySqlDataReader readerres;
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
        // Liste des élèves pour le choix de l'élève a inscrire
        public class designDatagridview
        {
            private DataGridView tableauDonnees;

            public int number;
            private DataGridView TableauDonnees
            {
                get { return tableauDonnees; }
                set { tableauDonnees = value; }
            }
            // constructeur
            public designDatagridview(DataGridView tableauDonnees)
            {
                this.tableauDonnees = tableauDonnees;
               
            }
            // Connexion au serveur
            public void Connec()
            {
                try
                {
                    connection = new MySqlConnection(connectionstring);
                    connection.Open();
                }
                catch (MySqlException e)
                {
                    string messag = "Echec de connexion au serveur. " + e.Message;
                    string titre = "Erreur Serveur";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
            // methode d'affichage de récuperation des données dans la bd pour le compte 
            public void affichDonnees(DataGridView datagridview, int appel,string nom)
            {
                Connec();
                //pour obligé l'utilisateur a séléctionné toute la ligne
                datagridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //Creation des colonnes de ma datagridview
                datagridview.ColumnCount = 3;
                datagridview.Columns[0].Name = "Matricule";
                datagridview.Columns[1].Name = "Nom";
                datagridview.Columns[2].Name = "Prenom";
                // interdire la saisis directement dans une colonne du datagridview
                datagridview.Columns[0].ReadOnly = true;
                datagridview.Columns[1].ReadOnly = true;
                datagridview.Columns[2].ReadOnly = true;
                // supprimer la premier colonne vide du datagridview et la dernier ligne du datagridview
                datagridview.RowHeadersVisible = false;
                datagridview.AllowUserToAddRows = false;
                // couleur des entetes de colonnes
                if(appel == 0)
                {
                    string requete = "Select matricule,nom_eleve,prenom_eleve from eleves order by nom_eleve asc";
                    command = new MySqlCommand(requete, connection);
                    command.Prepare();
                    reader = command.ExecuteReader();
                    datagridview.Rows.Clear();
                    int number = 1;
                    while (reader.Read())
                    {
                        datagridview.Rows.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
                        number++;
                    }
                    reader.Close();
                }
                else if(appel == 1)
                {
                    string requete = "Select matricule,nom_eleve,prenom_eleve from eleves where nom_eleve like @nom OR prenom_eleve like @prenom OR matricule like @matricule order by nom_eleve asc";
                    command = new MySqlCommand(requete, connection);
                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("@nom", "%" + nom + "%");
                    para.Add("@prenom", "%" + nom + "%");
                    para.Add("@matricule", nom + "%");
                    foreach (KeyValuePair<string,object> parametre in para)
                    {
                        command.Parameters.Add(new MySqlParameter(parametre.Key, parametre.Value));
                    }
                    command.Prepare();
                    reader = command.ExecuteReader();
                    datagridview.Rows.Clear();
                    int number = 1;
                    while (reader.Read())
                    {
                        datagridview.Rows.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
                        number++;
                    }
                    reader.Close();
                }
                
         
                if (number - 1 < 1)
                {

                }
                else
                {
                    datagridview.Rows[0].Selected = false;
                }
            }
          
        }

        // Methode pour afficher les sections
        public void listsection(ComboBox section)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string requete = "Select id_section,nom_section from tabsection order by nom_section asc";
            command = new MySqlCommand(requete, connection);
            command.Prepare();
            section.Items.Clear();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                section.Items.Add(reader["nom_section"]);
            }
            reader.Close();
            
        }

        // Methode pour recuperer l'id de la section séléctionné
        public void idsection(ComboBox section, Label stock)
        {
            string id = section.Text;
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string requete = "Select id_section,nom_section from tabsection where nom_section=@nom ";
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("@nom", id);
            command = new MySqlCommand(requete, connection);
            foreach (KeyValuePair<string, object> parametres in para)
            {
                command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
            }
            command.Prepare();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                stock.Text = (reader["id_section"].ToString());
            }
            reader.Close();

        }
        // Methode pour afficher les classe en fonction des sections
        public void listclasse(ComboBox classe, Label stocksection, TextBox matricule, Label session)
        {
            int nbreetabli = 0;
            // verifions si un établissement ayant ce nom a déja été crée
            string req = "Select id_inscription,count(id_inscription) as nbre from inscription where matricule=@matricule AND session=@session ";
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("@matricule", matricule.Text);
            para.Add("@session", session.Text);
            var connectv = new MySqlConnection(connectionstring);
            connectv.Open();
            var commandev = new MySqlCommand(req, connectv);
            foreach (KeyValuePair<string, object> parametres in para)
            {
                commandev.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
            }
            commandev.Prepare();

            reader2v = commandev.ExecuteReader();
            while (reader2v.Read())
            {
                nbreetabli = int.Parse(reader2v.GetValue(1).ToString());
            }

            if (nbreetabli >= 1)
            {

            }else
            {
                string id = stocksection.Text;
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "";
                if (id == "0")
                {
                    requete = "Select id_section,nom_classe,id_classe from classe";
                }
                else
                {
                    requete = "Select id_section,nom_classe,id_classe from classe where id_section=@nom ";
                }

                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("@nom", id);
                command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parametres in paras)
                {
                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                }
                command.Prepare();
                reader = command.ExecuteReader();
                classe.Items.Clear();
                while (reader.Read())
                {
                    classe.Items.Add(reader["nom_classe"]);
                }
                reader.Close();
            }
            

        }
        // Methode pour recuperer l'id de la classe séléctionné
        public void idclass(ComboBox classe, Label stock)
        {
            string id = classe.Text;
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string requete = "Select id_classe,nom_classe from classe where nom_classe=@nom ";
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("@nom", id);
            command = new MySqlCommand(requete, connection);
            foreach (KeyValuePair<string, object> parametres in para)
            {
                command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
            }
            command.Prepare();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                stock.Text = (reader["id_classe"].ToString());
            }
            reader.Close();

        }
        // Methode pour recuperer le montant pour inscription dans la classe
        public void montinscription(ComboBox classe, TextBox montinscript, TextBox restea, TextBox matricule, Label session)
        {
            int nbreetabli = 0;
            // ------------------
            string req = "Select id_inscription,count(id_inscription) as nbre from inscription where matricule=@matricule AND session=@session ";
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("@matricule", matricule.Text);
            para.Add("@session", session.Text);
            var connectv = new MySqlConnection(connectionstring);
            connectv.Open();
            var commandev = new MySqlCommand(req, connectv);
            foreach (KeyValuePair<string, object> parametres in para)
            {
                commandev.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
            }
            commandev.Prepare();

            reader2v = commandev.ExecuteReader();
            while (reader2v.Read())
            {
                nbreetabli = int.Parse(reader2v.GetValue(1).ToString());

            }

            if (nbreetabli >= 1)
            {

            }else
            {
                string id = classe.Text;
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "Select id_classe,nom_classe,montinscription from classe where nom_classe=@nom ";
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("@nom", id);
                command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parametres in paras)
                {
                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                }
                command.Prepare();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    montinscript.Text = (reader["montinscription"].ToString());
                    restea.Text = (reader["montinscription"].ToString());
                }
                reader.Close();
            }
            

        }

        /* Méthode pour recupere et afficher tout les informations 
         * de l'élève s'il a déja effectuer un versement pour
         * inscription a la session activer
        */
        public void donneeExist(Label session, TextBox matricule, ComboBox classe, TextBox montinscript, TextBox montverse, ComboBox section, Label stocksection, Label stock)
        {

            int nbreetabli = 0;
            string idclasse = "";
            // verifions si un établissement ayant ce nom a déja été crée
            string req = "Select id_inscription,count(id_inscription) as nbre from inscription where matricule=@matricule AND session=@session ";
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("@matricule", matricule.Text);
            para.Add("@session", session.Text);
            var connectv = new MySqlConnection(connectionstring);
            connectv.Open();
            var commandev = new MySqlCommand(req, connectv);
            foreach (KeyValuePair<string, object> parametres in para)
            {
                commandev.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
            }
            commandev.Prepare();

            reader2v = commandev.ExecuteReader();
            while (reader2v.Read())
            {
                nbreetabli = int.Parse(reader2v.GetValue(1).ToString());

            }
           
            if (nbreetabli >= 1)
            {
                string se = "select inscription.matricule,inscription.id_classe,inscription.montinscription,inscription.montverse,inscription.reste,inscription.session,classe.id_classe,classe.nom_classe from inscription inner join classe on inscription.id_classe=classe.id_classe where inscription.matricule=@matricule AND session=@session";
                Dictionary<string, object> arg = new Dictionary<string, object>();
                arg.Add("@matricule", matricule.Text);
                arg.Add("@session", session.Text);
                var connec = new MySqlConnection(connectionstring);
                connec.Open();
                var comman = new MySqlCommand(se, connec);
                foreach (KeyValuePair<string, object> parametres in arg)
                {
                    comman.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                }
                comman.Prepare();

                reader2 = comman.ExecuteReader();
                while (reader2.Read())
                {
                   montinscript.Text = reader2.GetValue(2).ToString();
                    montverse.Text = reader2.GetValue(4).ToString();
                    idclasse = reader2.GetValue(1).ToString();
                }
                reader2.Close();
                // selectionnont la liste des classe avec la classe déja choisi
                string se2 = "select classe.id_classe,classe.nom_classe,classe.id_section,tabsection.id_section,tabsection.nom_section from classe inner join tabsection on classe.id_section=tabsection.id_section where classe.id_classe=@idclass";
                Dictionary<string, object> arg2 = new Dictionary<string, object>();
                arg2.Add("@idclass", idclasse);
                var connec2 = new MySqlConnection(connectionstring);
                connec2.Open();
                var comman2 = new MySqlCommand(se2, connec2);
                foreach (KeyValuePair<string, object> parametres in arg2)
                {
                    comman2.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                }
                comman2.Prepare();

                reader3 = comman2.ExecuteReader();
                classe.Items.Clear(); section.Items.Clear();
                while (reader3.Read())
                {
                    classe.Items.Add(reader3.GetValue(1).ToString());
                    classe.SelectedItem = (reader3.GetValue(1).ToString());
                    section.Items.Add(reader3.GetValue(4).ToString());
                    section.SelectedItem = (reader3.GetValue(4).ToString());
                }
                reader3.Close();
            }
            else
            {
               // classe.Items.Clear();
                montinscript.Text = "";
                montverse.Text = "";
               // section.Items.Clear();
                stocksection.Text = "0";
                stock.Text = "0";
                listclasse(classe, stocksection, matricule, session);
                listsection(section);
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
        // creation de la class d'insertion d'une nouvelle inscription
        public class Insertinscription
        {

            private string Matricule { get; set; }
            private int Idclasse { get; set; }
            private long Montinscript { get; set; }
            private long Montaverse { get; set; }
            private long Montverse { get; set; }
            private DateTime Dateverse { get; set; }
            private string Session { get; set; }
            private int Montscolarite { get; set; }
            public int siinsert = 0;
            public int siDonneesValide = 0;
            // Constructeur
            public Insertinscription(string matricule, int idclasse, long montinscript, long montaverse, long montreste, DateTime date, string session, int montscolarite)
            {
                this.Matricule = matricule; this.Idclasse = idclasse; this.Montinscript = montinscript;
                this.Montaverse = montaverse; this.Montverse = montreste; this.Dateverse = date;
                this.Session = session; this.Montscolarite = montscolarite;
                insertdb();
            }
            private void insertdb()
            {
                // connection à la base de données et lancement de la transaction 
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                MySqlTransaction trans;
                trans = connection.BeginTransaction();
                try
                {
                    int nbreetabli = 0;
                    // verifions si un l'élève a déja effectuer un versement pour l'inscription dans cette session
                    string req = "Select id_inscription,count(id_inscription) as nbre from inscription where matricule=@matricule AND session=@session ";
                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("@matricule", Matricule);
                    para.Add("@session", Session);
                    command = new MySqlCommand(req, connection);
                    foreach (KeyValuePair<string, object> parametres in para)
                    {
                        command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    command.Prepare();
                    command.Transaction = trans;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        nbreetabli = int.Parse(reader.GetValue(1).ToString());

                    }
                    reader.Close();
                    if (nbreetabli < 1)
                    {
                        // Si l'élève n'est pas encore inscrit
                        string requete2 = "INSERT INTO inscription(matricule,id_classe,montinscription,montverse,reste,session,statut) values (@matricule,@classe,@inscription,@verse,@reste,@session,@statut)";
                        Dictionary<string, object> parametre = new Dictionary<string, object>();
                        parametre.Add("@matricule", Matricule);
                        parametre.Add("@classe", Idclasse);
                        parametre.Add("@verse", Montverse);
                        parametre.Add("@inscription", Montinscript);

                        parametre.Add("@session", Session);
                        long reste = Montinscript - Montverse;
                        string statut = "";
                        if (reste <= 0)
                        {
                            statut = "Soldé";
                        }
                        else
                        {
                            statut = "Non soldé";
                        }

                        parametre.Add("@reste", reste);
                        parametre.Add("@statut", statut);
                        command = new MySqlCommand(requete2, connection);
                        foreach (KeyValuePair<string, object> parametres in parametre)
                        {
                            command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                        }
                        command.Prepare();
                        command.Transaction = trans;
                        if (command.ExecuteNonQuery() >= 1)
                        {

                            siDonneesValide = 1;
                            // séléction pour récupéré le dernier numéro de recu
                            int numrecu = 0;
                            // Récupération du dernier numero de recu
                            string res = "select numrecu,max(numrecu) as recu from caissescolarite";
                            command = new MySqlCommand(res, connection);
                            command.Prepare();
                            command.Transaction = trans;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader.GetValue(1).ToString() == "")
                                {
                                    numrecu = 0;
                                }
                                else
                                {
                                    numrecu = int.Parse(reader.GetValue(1).ToString());
                                }

                            }
                            reader.Close();
                            int newnumrecu = numrecu + 1;
                            // insertion dans la caisse
                            string req2 = "INSERT INTO caissescolarite(monverse,dateverse,heure,matricule,session,numrecu,typeversement) values (@mont,@date,@heure,@matricule,@session,@numrecu,@typeverse)";
                            string datem = DateTime.Now.ToString("h:mm:ss");
                            Dictionary<string, object> paras = new Dictionary<string, object>();
                            paras.Add("@mont", Montverse);
                            paras.Add("@date", Dateverse);
                            paras.Add("@matricule", Matricule);
                            paras.Add("@session", Session);
                            paras.Add("@typeverse", "inscription");

                            paras.Add("@numrecu", newnumrecu);
                            paras.Add("@heure", datem);

                            command = new MySqlCommand(req2, connection);
                            foreach (KeyValuePair<string, object> parametres in paras)
                            {
                                command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                            }
                            command.Prepare();
                            command.Transaction = trans;
                            if (command.ExecuteNonQuery() >= 1)
                            {
                                // Creation de la ligne scolarité avec un montant versé de 0 franc cfa
                                string scol = "INSERT INTO scolarite(matricule,id_classe,montscolarite,montverse,reste,session,statut) values (@matricule,@classe,@scolarite,@verse,@reste,@session,@statut)";
                                Dictionary<string, object> arg = new Dictionary<string, object>();
                                arg.Add("@matricule", Matricule);
                                arg.Add("@classe", Idclasse);
                                arg.Add("@verse", 0);
                                arg.Add("@scolarite", Montscolarite);
                                arg.Add("@session", Session);
                                arg.Add("@reste", Montscolarite);
                                arg.Add("@statut","Non soldé");
                                command = new MySqlCommand(scol, connection);
                                foreach(KeyValuePair<string, object> parametres in arg)
                                {
                                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                                }
                                command.Prepare();
                                command.Transaction = trans;
                                command.ExecuteNonQuery();
                                siinsert = 1;
                            }else
                            {
                                // nous n'avons pas pu enregistrer en caisse
                                throw new Exception();
                            }
                        }
                        else
                        {
                            // Si nous n'avons pas pu enregistrer la nouvelle inscription
                            throw new Exception();
                            siDonneesValide = 0;
                            siinsert = 0;
                        }
                    }
                    else
                    {
                        //si élève déja inscript,
                        string se = "select matricule,session,montinscription,montverse,reste from inscription where matricule=@matricule AND session=@session";
                        Dictionary<string, object> info = new Dictionary<string, object>();
                        info.Add("@matricule", Matricule);
                        info.Add("@session", Session);

                        command = new MySqlCommand(se, connection);
                        foreach (KeyValuePair<string, object> parametres in info)
                        {
                            command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                        }
                        command.Prepare();
                        command.Transaction = trans;
                        reader = command.ExecuteReader();
                        long montinscript = 0; long montdejaverse = 0;
                        while (reader.Read())
                        {

                            montinscript = long.Parse(reader.GetValue(2).ToString());
                            montdejaverse = long.Parse(reader.GetValue(3).ToString());

                        }
                        reader.Close();

                        long newmontverse = (montdejaverse + Montverse);
                        long newreste = (montinscript - newmontverse);
                        string statut = "";
                        if (newreste == 0)
                        {
                            statut = "Soldé";
                        }
                        else
                        {
                            statut = "Non soldé";
                        }
                        // Modification de la ligne inscription
                        string up = "UPDATE inscription SET montverse=@dejaverse, reste=@reste, statut=@statut where matricule=@matricule AND session=@session";
                        Dictionary<string, object> paras = new Dictionary<string, object>();
                        paras.Add("@dejaverse", newmontverse);
                        paras.Add("@reste", newreste);
                        paras.Add("@matricule", Matricule);
                        paras.Add("@session", Session);
                        paras.Add("@statut", statut);

                        command = new MySqlCommand(up, connection);
                        foreach (KeyValuePair<string, object> parametres in paras)
                        {
                            command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                        }
                        command.Prepare();
                        command.Transaction = trans;
                        if (command.ExecuteNonQuery() >= 1)
                        {

                            int numrecu = 0;
                            // Récupération du dernier numero de recu
                            string res = "select numrecu,max(numrecu) as recu from caissescolarite";
                            command = new MySqlCommand(res, connection);
                            command.Prepare();
                            command.Transaction = trans;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader.GetValue(1).ToString() == "")
                                {
                                    numrecu = 0;
                                }
                                else
                                {
                                    numrecu = int.Parse(reader.GetValue(1).ToString());
                                }

                            }
                            reader.Close();
                            int newnumrecu = numrecu + 1;

                            // Insertion de l'argent en caisse
                            string req2 = "INSERT INTO caissescolarite(monverse,dateverse,heure,matricule,session,numrecu,typeversement) values (@mont,@date,@heure,@matricule,@session,@numrecu,@typeverse)";
                            string datem = DateTime.Now.ToString("h:mm:ss");
                            Dictionary<string, object> paracaisse = new Dictionary<string, object>();
                            paracaisse.Add("@mont", Montverse);
                            paracaisse.Add("@date", Dateverse);
                            paracaisse.Add("@matricule", Matricule);
                            paracaisse.Add("@session", Session);
                            paracaisse.Add("@typeverse", "inscription");

                            paracaisse.Add("@numrecu", newnumrecu);
                            paracaisse.Add("@heure", datem);

                            command = new MySqlCommand(req2, connection);
                            foreach (KeyValuePair<string, object> parametres in paracaisse)
                            {
                                command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                            }
                            command.Prepare();
                            command.Transaction = trans;
                            if (command.ExecuteNonQuery() >= 1)
                            {
                               
                                siinsert = 1;
                            }
                            else
                            {
                                // nous n'avons pas pu mouvementé la caisse
                                throw new Exception();
                            }
                        }else
                        {
                            // Nous n'avons pas pu modifier l'inscription de l'élève
                            throw new Exception();
                        }

                    }
                    trans.Commit();
                    string messagok = "l'élève a été inscript avec succès. ";
                    string titreok = "Inscription";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messagok, titreok, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception error)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de l'inscription . " + error + "";
                    string titre = "Erreur";
                    trans.Rollback();
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                
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
        public AddInscription(string session)
        {
            InitializeComponent();
            connexionDB();
            label13.Text = session;
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            designDatagridview eleve = new designDatagridview(tableaudonnees);
            string chain = nomposte.Text;
            eleve.affichDonnees(tableaudonnees,0,chain);
            //Affiche section
            listsection(comboBox3);
            //Affiche classe
            listclasse(comboBox1, label11, textBox2, label13);
            // Affichege de la date d'aujourd'hui
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void fermFormAddSchool_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tableaudonnees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune élève séléctionné pour l'inscription. ";
                string titre = "Inscription";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                string prenom = (tableaudonnees.CurrentRow.Cells["prenom"].Value).ToString();
                string matricule = (tableaudonnees.CurrentRow.Cells["matricule"].Value).ToString();
                textBox1.Text = nom + " " + prenom; textBox2.Text = matricule;
                donneeExist(label13, textBox2, comboBox1, textBox4, textBox5, comboBox3, label11, label12);
            }
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
           // textBox4.Text = ""; textBox5.Text = ""; label12.Text = "";
            idsection(comboBox3, label11);
            
            //Affiche classe
            listclasse(comboBox1, label11, textBox2, label13);
            


        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            idclass(comboBox1, label12);
            // affiche le montant de l'inscription de la classe séléctionné
            montinscription(comboBox1, textBox4, textBox5, textBox2, label13);
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (textBox2.Text == "")
            {
                string messag = "Veuillez séléctionner l'élève que vous souhaitez inscrire sur la liste . ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBox1.Text == "")
            {
                string messag = "Veuillez séléctionner la classe ou vous souhaitez inscrire l'élève. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox4.Text == "")
            {
                string messag = "Le montant de l'inscription dans cette classe est absente.";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox5.Text == "")
            {
                string messag = "Le reste a versé par cette élève pour son inscription est absente";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox6.Text == "" || textBox6.Text == "0")
            {
                string messag = "Veuillez entrer le montant versé par l'élève";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(VerifiSiNombre(textBox6.Text) == 0)
            {
                string messag = "Montant versé incorrect";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else if(label13.Text == "non")
            {
                string messag = "Veuillez créer ou activé une session pour l'inscription";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (long.Parse(textBox5.Text) < long.Parse(textBox6.Text))
            {
                string messag = "Le montant versé par l'élève est supérieur au reste a payé pour son inscription";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
               
                string matricule = textBox2.Text; int classe = int.Parse(label12.Text);
                long montinscript = long.Parse(textBox4.Text); long montaverse = long.Parse(textBox5.Text);
                long montverse = long.Parse(textBox6.Text); DateTime dates = DateTime.Parse(dateTimePicker1.Text);
                string session = label13.Text; int montscolarite = 0;
                /* Recuperation du montant de la scolarité de la classe
                 * si l'élève a une bourse ou une prime encour
                 * alors soustraire le montant de la bourse ou prime 
                 * sur le montant de la scolarite
                 */ 
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                var connection2 = new MySqlConnection(connectionstring);
                connection2.Open();
                try
                {
                    int montscolariteDefault = 0;
                    string req = "SELECT montscolarite FROM classe where id_classe='" + classe + "'";
                    command = new MySqlCommand(req, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        montscolariteDefault = int.Parse(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                  
                    // verifions si l'élève a une bourse et/ou une prime
                    string req2 = "SELECT sum(montant) mont from bourse where matricule='" + matricule + "' AND session='" + session + "'";
                    command = new MySqlCommand(req2, connection);
                    command.Prepare();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if(reader.GetValue(0).ToString() == "null" || reader.GetValue(0).ToString() == "")
                        {
                            montscolarite = montscolariteDefault;
                        }
                        else
                        {
                            montscolarite = montscolariteDefault - int.Parse(reader.GetValue(0).ToString());
                            string req3 = "UPDATE bourse SET statut='Réglé' WHERE matricule='" + matricule + "' AND session='" + session + "'";
                            command = new MySqlCommand(req3, connection2);
                            command.Prepare();
                            command.ExecuteNonQuery();

                        }
                        
                    }
                    reader.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Erreur serveur", "Impossible de récupéré les informations du serveur "+ex, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally { connection.Close(); connection2.Close(); }
                Insertinscription inscript = new Insertinscription(matricule, classe, montinscript, montaverse, montverse, dates, session,montscolarite);

                if(inscript.siinsert == 1)
                {
                    // Ouverture du formulaire d'impression
                    IMPRESSION_inscription ser = new IMPRESSION_inscription();
                    ser.ShowDialog();
                    textBox6.Text = "0";
                }
                
            }
        }

        private void AddInscription_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel3, 5, e.Graphics, borderColor, borderSize);
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

        private void panelDegrader2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelDegrader2, 8, e.Graphics, borderColor, borderSize);
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

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel10, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel11, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel2, 5, e.Graphics, Color.Blue, 1);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button2, 8, e.Graphics, borderColor, borderSize);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
           
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void nomposte_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void nomposte_KeyUp(object sender, KeyEventArgs e)
        {
            designDatagridview eleve = new designDatagridview(tableaudonnees);
            string chain = nomposte.Text;
            eleve.affichDonnees(tableaudonnees, 1, chain);
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button1, 8, e.Graphics, borderColor, borderSize);
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button3, 8, e.Graphics, borderColor, borderSize);
        }

        private void button4_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button4, 8, e.Graphics, borderColor, borderSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddSection section = new AddSection();
            section.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddClasse classe = new AddClasse();
            classe.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddEleve eleve = new AddEleve();
            eleve.Show();
        }

        private void comboBox3_Click(object sender, EventArgs e)
        {
            //Affiche section
            listsection(comboBox3);
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            //Affiche classe
            listclasse(comboBox1, label11, textBox2, label13);
        }
    }
}
