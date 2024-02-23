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
    public partial class Addscolarite : Form
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
        public static MySqlDataReader readerres;
        public static MySqlDataReader readerinfo;
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

        // Liste des élèves inscript(reste == 0) pour le choix de l'élève pour le versement scolaire
        public class designDatagridview
        {
            private DataGridView tableauDonnees;
            public Label Session{set; get;}
            public int number;
            private DataGridView TableauDonnees
            {
                get { return tableauDonnees; }
                set { tableauDonnees = value; }
            }
            // constructeur
            public designDatagridview(DataGridView tableauDonnees, Label session)
            {
                this.tableauDonnees = tableauDonnees;this.Session = session;
               
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
            public void affichDonnees(DataGridView datagridview,int appel, string nom)
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
                string requete = "";
                Dictionary<string, object> paracaisse = new Dictionary<string, object>();
                
                if(appel == 0)
                {
                    requete = "Select eleves.matricule,eleves.nom_eleve,eleves.prenom_eleve,inscription.matricule,inscription.reste,inscription.statut,inscription.session from inscription inner join eleves on inscription.matricule=eleves.matricule where inscription.reste='0' AND inscription.statut='Soldé' AND inscription.session=@session order by eleves.nom_eleve asc";
                    paracaisse.Add("@session", Session.Text);
                }else if(appel == 1)
                {
                    requete = "Select eleves.matricule,eleves.nom_eleve,eleves.prenom_eleve,inscription.matricule,inscription.reste,inscription.statut,inscription.session from inscription inner join eleves on inscription.matricule=eleves.matricule where inscription.reste='0' AND inscription.statut='Soldé' AND inscription.session=@session AND (eleves.nom_eleve like @nom OR eleves.prenom_eleve like @prenom OR eleves.matricule like @matricule) order by eleves.nom_eleve asc";
                    paracaisse.Add("@session", Session.Text);
                    paracaisse.Add("@nom", "%" + nom + "%");
                    paracaisse.Add("@prenom", "%" + nom + "%");
                    paracaisse.Add("@matricule", nom + "%");
                }
                var command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parametres in paracaisse)
                {
                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
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

                if (number - 1 < 1)
                {

                }
                else
                {
                    datagridview.Rows[0].Selected = false;
                }
            }

        }
        /* Vérifions si l'élève a déja effectuer un versement 
         * si oui récupéré les informations déja stocker pour l'affiche
         * si non aller dans inscription pour récupéré l'id de la classe de l'élève
         * et a partir de cette id recuperer le montant de la scolarite dans la table classe pour l'affiché
         */
         public class Infoeleve
        {
            public string Matricule { set; get; }
            public string Session { set; get; }
            public int Idclasse { set; get; }
            public TextBox Montscolarite { set; get; }
            public TextBox Montdejav { set; get; }
            public TextBox Section { set; get; }
            public TextBox Classe { set; get; }
            public Infoeleve(string matricule, string session, TextBox montscol, TextBox dejaverse, TextBox section, TextBox classe)
            {
                this.Matricule = matricule; this.Session = session;
                this.Montscolarite = montscol;this.Montdejav = dejaverse;
                this.Section = section; this.Classe = classe;
            }

            // ------------------------------------
            public void donneesExist()
            {
                // verifions si l'élève a déja effectuer un versement pour scolarité
                int siexist = 0;
                string req = "SELECT id_scolarite,count(id_scolarite) as nbre from scolarite where matricule=@matricule AND session=@session";
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@matricule", Matricule);
                para.Add("@session", Session);
                var command = new MySqlCommand(req, connection);
                foreach(KeyValuePair<string,object> parametres in para)
                {
                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                }
                command.Prepare();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    siexist = int.Parse(reader.GetValue(1).ToString());
                }
                reader.Close();
                if(siexist < 1)
                {
                   
                    // Aucun versement effectuer, on séléctionne son inscription pour récupéré l'id de la classe
                    string idcl = "SELECT id_classe from inscription where matricule=@matricule AND session=@session";
                    var connect = new MySqlConnection(connectionstring);
                    connect.Open();
                    Dictionary<string, object> id = new Dictionary<string, object>();
                    id.Add("@matricule", Matricule);
                    id.Add("@session", Session);
                    var command2 = new MySqlCommand(idcl, connect);
                    foreach(KeyValuePair<string, object> parametres in id)
                    {
                        command2.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    command2.Prepare();
                    reader2 = command2.ExecuteReader(); 
                    while (reader2.Read())
                    {
                        Idclasse = int.Parse(reader2.GetValue(0).ToString());
                    }
                    reader2.Close();
                    // Recuperation de la classe et la section de l'eleve
                    string classe = "SELECT nom_classe, nom_section FROM classe inner join tabsection on classe.id_section=tabsection.id_section where id_classe = '" + Idclasse + "'";
                    command = new MySqlCommand(classe, connect);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Section.Text = reader.GetValue(1).ToString();
                        Classe.Text = reader.GetValue(0).ToString();
                    }
                    reader.Close();
                    string mon = "SELECT montscolarite from classe where id_classe=@idclasse";
                    var connect2 = new MySqlConnection(connectionstring);
                    connect2.Open();
                    Dictionary<string, object> classes = new Dictionary<string, object>();
                    classes.Add("@idclasse", Idclasse);
                    var command3 = new MySqlCommand(mon, connect2);
                    foreach(KeyValuePair<string,object> parametres in classes)
                    {
                        command3.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    command3.Prepare();
                    reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        Montscolarite.Text = reader3.GetValue(0).ToString();
                        Montdejav.Text = reader3.GetValue(0).ToString();
                    }

                }else
                {
                    //  Données exist
                    // on séléctionne son inscription pour récupéré l'id de la classe
                    string idcl2 = "SELECT id_classe from inscription where matricule=@matricule AND session=@session";
                    var connect = new MySqlConnection(connectionstring);
                    connect.Open();
                    Dictionary<string, object> id = new Dictionary<string, object>();
                    id.Add("@matricule", Matricule);
                    id.Add("@session", Session);
                    var command2 = new MySqlCommand(idcl2, connect);
                    foreach (KeyValuePair<string, object> parametres in id)
                    {
                        command2.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    command2.Prepare();
                    reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                    {
                        Idclasse = int.Parse(reader2.GetValue(0).ToString());
                    }
                    reader2.Close();
                    // Recuperation de la classe et la section de l'eleve
                    string classe = "SELECT nom_classe, nom_section FROM classe inner join tabsection on classe.id_section=tabsection.id_section where id_classe = '" + Idclasse + "'";
                    command = new MySqlCommand(classe, connect);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Section.Text = reader.GetValue(1).ToString();
                        Classe.Text = reader.GetValue(0).ToString();
                    }
                    reader.Close();
                    // Aucun versement effectuer, on séléctionne son inscription pour récupéré l'id de la classe
                    string idcl = "SELECT montscolarite,montverse,reste from scolarite where matricule=@matricule AND session=@session";

                    Dictionary<string, object> ids = new Dictionary<string, object>();
                    ids.Add("@matricule", Matricule);
                    ids.Add("@session", Session);
                     command = new MySqlCommand(idcl, connect);
                    foreach (KeyValuePair<string, object> parametres in ids)
                    {
                        command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    command.Prepare();
                    reader2 = command.ExecuteReader();
                    while (reader2.Read())
                    {
                        Montscolarite.Text = reader2.GetValue(0).ToString();
                        Montdejav.Text = reader2.GetValue(2).ToString();
                    }
                }
            }
        }
        public class Insertfrais
        {

            public string Matricule { set; get; }
            public long Montscolarite { set; get; }
            public long Montverse { set; get; }
            public DateTime Dateverse { set; get; }
            public string Session { set; get; }
            public string Idclasse { set; get; }
            public int siinsert = 0;
            public Insertfrais(string matricule, string session, long mont, long reste, DateTime datev, string idclass)
            {
                this.Matricule = matricule; this.Session = session; this.Montscolarite = mont;
                this.Montverse = reste; this.Dateverse = datev; this.Idclasse = idclass;
            }
            public void newinsert()
            {
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                MySqlTransaction trans;
                trans = connection.BeginTransaction();
                try
                {
                    // verifions si l'élève a déja effectuer un versement pour scolarité
                    int siexist = 0;
                    string req = "SELECT id_scolarite,count(id_scolarite) as nbre from scolarite where matricule=@matricule AND session=@session";
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
                        siexist = int.Parse(reader.GetValue(1).ToString());
                    }
                    reader.Close();
                    if (siexist < 1)
                    {
                        string ins = "INSERT INTO scolarite(matricule,id_classe,montscolarite,montverse,reste,session,statut) values(@matricule,@idclasse,@montscol,@montver,@reste,@session,@statut)";
                        Dictionary<string, object> param = new Dictionary<string, object>();
                        param.Add("@matricule", Matricule);
                        param.Add("@idclasse", Idclasse);
                        param.Add("@montscol", Montscolarite);
                        param.Add("@montver", Montverse);

                        param.Add("@session", Session);

                        long reste = (Montscolarite - Montverse);
                        param.Add("@reste", reste);
                        string statut = "";
                        if (reste > 0)
                        {
                            statut = "Non soldé";
                        }
                        else if (reste == 0)
                        {
                            statut = "Soldé";
                        }
                        param.Add("@statut", statut);
                        command = new MySqlCommand(ins, connection);
                        foreach (KeyValuePair<string, object> parametres in param)
                        {
                            command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                        }
                        command.Prepare();
                        command.Transaction = trans;
                        if (command.ExecuteNonQuery() >= 1)
                        {
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
                                if (reader.GetValue(1).ToString() == " ")
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
                            paras.Add("@typeverse", "scolarite");

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
                                
                                siinsert = 1;
                            }
                        }
                    }
                    else
                    {
                        // Si premier versement déja effectuer
                        string se = "select matricule,session,montscolarite,montverse,reste from scolarite where matricule=@matricule AND session=@session";
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
                        long montscol = 0; long montdejaverse = 0;
                        while (reader.Read())
                        {

                            montscol = long.Parse(reader.GetValue(2).ToString());
                            montdejaverse = long.Parse(reader.GetValue(3).ToString());

                        }
                        reader.Close();

                        long newmontverse = (montdejaverse + Montverse);
                        long newreste = (montscol - newmontverse);
                        string statut2 = "";
                        if (newreste == 0)
                        {
                            statut2 = "Soldé";
                        }
                        else
                        {
                            statut2 = "Non soldé";
                        }
                        // Modification de la ligne inscription
                        string up = "UPDATE scolarite SET montverse=@dejaverse, reste=@reste, statut=@statut where matricule=@matricule AND session=@session";
                        Dictionary<string, object> paras = new Dictionary<string, object>();
                        paras.Add("@dejaverse", newmontverse);
                        paras.Add("@reste", newreste);
                        paras.Add("@matricule", Matricule);
                        paras.Add("@session", Session);
                        paras.Add("@statut", statut2);
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
                            paracaisse.Add("@typeverse", "scolarite");

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
                        }
                    }
                    trans.Commit();
                    string messagok = "Versement enregistré. ";
                    string titreok = "Frais scolaire";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messagok, titreok, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception error)
                {
                    string messagok = "Nous nous sommes heurtés à un problème lors de l'enregistrement du versement. "+error;
                    string titreok = "Erreur";
                    trans.Rollback();
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messagok, titreok, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
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
        public static string session2 = "";
        public Addscolarite(string session)
        {
            InitializeComponent();
            session2 = session;
            connexionDB();
            label13.Text = session;
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            designDatagridview eleve = new designDatagridview(tableaudonnees, label13);
            eleve.affichDonnees(tableaudonnees,0,"");
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
                string messag = "Aucune élève séléctionné pour le versement des frais scolaire. ";
                string titre = "Scolarité";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                string prenom = (tableaudonnees.CurrentRow.Cells["prenom"].Value).ToString();
                string matricule = (tableaudonnees.CurrentRow.Cells["matricule"].Value).ToString();
                string session = label13.Text;
                textBox1.Text = nom + " " + prenom; textBox2.Text = matricule;
                Infoeleve elev = new Infoeleve(matricule, session, textBox4, textBox5, textBox7, textBox3);
                elev.donneesExist();
                label5.Text = (elev.Idclasse).ToString();
                
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                string messag = "Veuillez séléctionner l'élève pour le versement . ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
            else if (textBox4.Text == "")
            {
                string messag = "Le montant de la scolarité est absente.";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox5.Text == "")
            {
                string messag = "Le reste a versé par cette élève pour sa scolarité est absente";
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
            else if (VerifiSiNombre(textBox6.Text) == 0)
            {
                string messag = "Montant versé incorrect";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else if(label13.Text == "non")
            {
                string messag = "Veuillez créer ou activé une session pour le versement";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (long.Parse(textBox5.Text) < long.Parse(textBox6.Text))
            {
                string messag = "Le montant versé par l'élève est supérieur au reste a payé pour sa scolarité";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                string matricule = textBox2.Text;
                long montinscript = long.Parse(textBox4.Text); long montaverse = long.Parse(textBox5.Text);
                long montverse = long.Parse(textBox6.Text); DateTime dates = DateTime.Parse(dateTimePicker1.Text);
                string session = label13.Text;
                string idclass = label5.Text;
                Insertfrais frais = new Insertfrais(matricule, session, montinscript, montverse, dates,idclass);
                frais.newinsert();

                if (frais.siinsert == 1)
                {
                    // Ouverture du formulaire d'impression
                    RECU_SCOLARITE ser = new RECU_SCOLARITE();
                    ser.ShowDialog();
                    textBox6.Text = "0";
                }

            }
        }

        private void Addscolarite_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel3, 5, e.Graphics, borderColor, borderSize);
        }

        private void panelDegrader1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelDegrader1, 8, e.Graphics, borderColor, borderSize);
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

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button2, 8, e.Graphics, borderColor, borderSize);
        }

        private void panelDegrader2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelDegrader2, 8, e.Graphics, borderColor, borderSize);
        }

        private void nomposte_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void nomposte_KeyUp(object sender, KeyEventArgs e)
        {
            designDatagridview eleve = new designDatagridview(tableaudonnees, label13);
            string nom = nomposte.Text;
            eleve.affichDonnees(tableaudonnees, 1, nom);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button1, 8, e.Graphics, borderColor, borderSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddInscription inscrits = new AddInscription(session2);
            inscrits.Show();
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel7, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel6, 5, e.Graphics, Color.Blue, 1);
        }
    }
}
