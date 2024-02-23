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
    public partial class NEW_BOURSEPRIME : Form
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

        // gestion de datagridview (affichage du tableau)
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
                affichDonnees(tableauDonnees);
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
            public void affichDonnees(DataGridView datagridview)
            {
                Connec();
                //pour obligé l'utilisateur a séléctionné toute la ligne
                datagridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //Creation des colonnes de ma datagridview
                datagridview.ColumnCount = 8;
                datagridview.Columns[0].Name = "ID";
                datagridview.Columns[1].Name = "N°";
                datagridview.Columns[2].Name = "Matricule";
                datagridview.Columns[3].Name = "Nom";
                datagridview.Columns[4].Name = "Montant de la bourse";
                datagridview.Columns[5].Name = "Type";
                datagridview.Columns[6].Name = "Session";
                datagridview.Columns[7].Name = "Statut";
                // interdire la saisis directement dans une colonne du datagridview
                datagridview.Columns[0].ReadOnly = true;
                datagridview.Columns[1].ReadOnly = true;
                datagridview.Columns[2].ReadOnly = true;
                datagridview.Columns[3].ReadOnly = true;
                datagridview.Columns[4].ReadOnly = true;
                datagridview.Columns[5].ReadOnly = true;
                datagridview.Columns[6].ReadOnly = true;
                datagridview.Columns[7].ReadOnly = true;
                // supprimer la premier colonne vide du datagridview et la dernier ligne du datagridview
                datagridview.RowHeadersVisible = false;
                datagridview.AllowUserToAddRows = false;

                // cacher ma colonne id du datagridview
                datagridview.Columns["ID"].Visible = false;
                // couleur des entetes de colonnes
                string requete = "SELECT bourse.matricule,eleves.nom_eleve,bourse.montant,bourse.type,bourse.session,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule order by session";
                command = new MySqlCommand(requete, connection);
                command.Prepare();
                reader = command.ExecuteReader();
                datagridview.Rows.Clear();
                number = 1;
                while (reader.Read())
                {
                    // datagridview.Rows.Add(number.ToString(), ""+reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetValue(4).ToString());
                    number++;
                }
                reader.Close();
            }
            // methode d'affichage des données dans ma datagridview
            public void affichDonneesdatagrid(DataGridView datagridview, int numbtn, int nbreligneParPage)
            {
                Connec();
                //pour obligé l'utilisateur a séléctionné toute la ligne
                datagridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                int lignefin = numbtn * nbreligneParPage;
                int lignedebut = lignefin - nbreligneParPage;
                //Creation des colonnes de ma datagridview
                datagridview.ColumnCount = 8;
                datagridview.Columns[0].Name = "ID";
                datagridview.Columns[1].Name = "N°";
                datagridview.Columns[2].Name = "Matricule";
                datagridview.Columns[3].Name = "Nom";
                datagridview.Columns[4].Name = "Montant de la bourse";
                datagridview.Columns[5].Name = "Type";
                datagridview.Columns[6].Name = "Session";
                datagridview.Columns[7].Name = "Statut";
                // interdire la saisis directement dans une colonne du datagridview
                datagridview.Columns[0].ReadOnly = true;
                datagridview.Columns[1].ReadOnly = true;
                datagridview.Columns[2].ReadOnly = true;
                datagridview.Columns[3].ReadOnly = true;
                datagridview.Columns[4].ReadOnly = true;
                datagridview.Columns[5].ReadOnly = true;
                datagridview.Columns[6].ReadOnly = true;
                datagridview.Columns[7].ReadOnly = true;
                // supprimer la premier colonne vide du datagridview et la dernier ligne du datagridview

                // cacher ma colonne id du datagridview
                datagridview.Columns["ID"].Visible = false;

                datagridview.RowHeadersVisible = false;
                datagridview.AllowUserToAddRows = false;
                // couleur des entetes de colonnes
                string requete = "SELECT bourse.matricule,eleves.nom_eleve,bourse.montant,bourse.type,bourse.session,bourse.statut,bourse.id_bourse from bourse inner join eleves on bourse.matricule=eleves.matricule order by session limit " + nbreligneParPage + " offset " + lignedebut + "";
                command = new MySqlCommand(requete, connection);
                command.Prepare();
                reader = command.ExecuteReader();
                datagridview.Rows.Clear();
                number = lignedebut + 1;
                while (reader.Read())
                {
                    datagridview.Rows.Add(reader.GetValue(6).ToString(), number.ToString(), "" + reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), reader.GetValue(5).ToString());
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
            /*  
             * Méthode pour compté le nombre de ligne renvoyer par la requete
             * Calcule du nombre de page avec 30 lignes par page
             * Affichage des numéros en dessous du datagridview
             */
            // Initialisation d'un tableau dynamique qui va contenir les numéros de page s'il ya plusieurs pages
            public List<int> numPage = new List<int>();
            public int Compteligne(Label labeltete, Panel numpclob, Label backPage1, Label precedentPage, Label suivantPage, Label dernierPage, Label btnPage1, Label btnPage2, Label btnPage3, Label btnPage4, Label btnPage5, int numPageClick, int nbreLignePage)
            {
                float nbreligne = number - 1;
                float nbrePage = nbreligne / nbreLignePage;
                int newNbrePage = 0;
                if (nbrePage <= 1)
                {
                    // Une seul page trouvé
                    labeltete.Text = "1 / 1 page        " + nbreligne + " bourse(s) et prime(s)  enregistré(s)";
                    numpclob.Visible = false;
                    btnPage1.Visible = false;
                    btnPage2.Visible = false;
                    btnPage3.Visible = false;
                    btnPage4.Visible = false;
                    btnPage5.Visible = false;
                    backPage1.Visible = false;
                    precedentPage.Visible = false;
                    suivantPage.Visible = false;
                    dernierPage.Visible = false;
                }
                else
                {
                    /* Si le nombre de page renvoyé contient une virgule, alors 
                     * on recupère le nombre qui vient après la virgule, on l'a converti en entier
                     * et on ajoute un pour avoir le nombre de page sans virgule
                     */
                    List<char> mastring = new List<char>();
                    string PartieEntier = "";
                    newNbrePage = 0;
                    string test = nbrePage.ToString();
                    for (int val = 0; val < test.Length; val++)
                    {
                        mastring.Add(test[val]);
                    }
                    // verifions si la virgule se trouve dans ma liste
                    if (mastring.Contains(',') == true)
                    {
                        for (int pa = 0; pa < mastring.Count; pa++)
                        {
                            if (mastring[pa] == ',')
                            {
                                break;
                            }
                            else
                            {
                                PartieEntier += mastring[pa];
                            }
                        }
                        newNbrePage = int.Parse(PartieEntier) + 1;
                    }
                    else
                    {
                        newNbrePage = (int)nbrePage;
                    }

                    // Plusieurs pages trouvé
                    numpclob.Visible = true;
                    labeltete.Text = numPageClick + " / " + newNbrePage + " page(s)        " + nbreligne + " bourse(s) et prime(s)  enregistré(s)";
                    // remplissage de mon dictionnaire
                    for (int b = 1; b <= newNbrePage; b++)
                    {
                        numPage.Add(b);
                    }
                    /*
                     * Si le nombre de page est inferieur ou égal à 5, on affiche 5 label
                     * Si le nombre de page est supérieur à 5, on affiche les 5 prémier label
                     * Les autres label seront affiché par le clique de l'utilisateur
                     */
                    if (newNbrePage <= 5)
                    {
                        // cache les bouton de gestion des page
                        backPage1.Visible = false;
                        precedentPage.Visible = false;
                        suivantPage.Visible = false;
                        dernierPage.Visible = false;
                        // Affichage des boutons de page en fonction du nombre de page
                        if (newNbrePage == 2)
                        {
                            btnPage1.Visible = true; btnPage2.Visible = true; btnPage3.Visible = false; btnPage4.Visible = false; btnPage5.Visible = false;
                        }
                        else if (newNbrePage == 3)
                        {
                            btnPage1.Visible = true; btnPage2.Visible = true; btnPage3.Visible = true; btnPage4.Visible = false; btnPage5.Visible = false;
                        }
                        else if (newNbrePage == 4)
                        {
                            btnPage1.Visible = true; btnPage2.Visible = true; btnPage3.Visible = true; btnPage4.Visible = true; btnPage5.Visible = false;
                        }
                        else if (newNbrePage == 5)
                        {
                            btnPage1.Visible = true; btnPage2.Visible = true; btnPage3.Visible = true; btnPage4.Visible = true; btnPage5.Visible = true;
                        }
                    }
                    else
                    {
                        // Apparaitre les boutons de gestion des page
                        btnPage1.Visible = true; btnPage2.Visible = true; btnPage3.Visible = true; btnPage4.Visible = true; btnPage5.Visible = true;
                        if (btnPage1.Text == "1")
                        {
                            backPage1.Visible = false;
                            precedentPage.Visible = false;
                            suivantPage.Visible = true;
                            dernierPage.Visible = true;
                        }
                        else if (btnPage5.Text == newNbrePage.ToString())
                        {
                            backPage1.Visible = true;
                            precedentPage.Visible = true;
                            suivantPage.Visible = false;
                            dernierPage.Visible = false;
                        }
                        else
                        {
                            backPage1.Visible = true;
                            precedentPage.Visible = true;
                            suivantPage.Visible = true;
                            dernierPage.Visible = true;
                        }

                    }
                }
                return newNbrePage;
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

        // gestion de datagridview avec critères de recherche
        public class designDatagridviewSearch
        {
            private DataGridView tableauDonnees;
            private int idsection;
            public int number;
            private string nomsect;
            private DataGridView TableauDonnees
            {
                get { return tableauDonnees; }
                set { tableauDonnees = value; }
            }
       
            public string Nom { private set; get; }
            // constructeur
            public designDatagridviewSearch(DataGridView tableauDonnees, string nom)
            {
                this.TableauDonnees = tableauDonnees;
                 this.Nom = nom;
                affichDonnees(tableauDonnees);
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
            public void affichDonnees(DataGridView datagridview)
            {
                Connec();
                //pour obligé l'utilisateur a séléctionné toute la ligne
                datagridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //Creation des colonnes de ma datagridview
                datagridview.ColumnCount = 8;
                datagridview.Columns[0].Name = "ID";
                datagridview.Columns[1].Name = "N°";
                datagridview.Columns[2].Name = "Matricule";
                datagridview.Columns[3].Name = "Nom";
                datagridview.Columns[4].Name = "Montant de la bourse";
                datagridview.Columns[5].Name = "Type";
                datagridview.Columns[6].Name = "Session";
                datagridview.Columns[7].Name = "Statut";
                // interdire la saisis directement dans une colonne du datagridview
                datagridview.Columns[0].ReadOnly = true;
                datagridview.Columns[1].ReadOnly = true;
                datagridview.Columns[2].ReadOnly = true;
                datagridview.Columns[3].ReadOnly = true;
                datagridview.Columns[4].ReadOnly = true;
                datagridview.Columns[5].ReadOnly = true;
                datagridview.Columns[6].ReadOnly = true;
                datagridview.Columns[7].ReadOnly = true;
                // supprimer la premier colonne vide du datagridview et la dernier ligne du datagridview

                // cacher ma colonne id du datagridview
                datagridview.Columns["ID"].Visible = false;

                datagridview.RowHeadersVisible = false;
                datagridview.AllowUserToAddRows = false;
                // couleur des entetes de colonnes
                string requete = "";
                command = new MySqlCommand(requete, connection);
                Dictionary<string, object> parametre = new Dictionary<string, object>();
        
                    requete = "SELECT bourse.matricule,eleves.nom_eleve,bourse.montant,bourse.type,bourse.session,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule WHERE nom_eleve like @nom OR bourse.matricule like @matricule order by session";
                    parametre.Add("@nom", "%" + Nom + "%");
                    parametre.Add("@matricule", Nom + "%");
                command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parameter in parametre)
                {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));

                }
                command.Prepare();
                reader = command.ExecuteReader();
                datagridview.Rows.Clear();
                number = 1;
                while (reader.Read())
                {
                    // datagridview.Rows.Add(number.ToString(), ""+reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetValue(4).ToString());
                    number++;
                }
                reader.Close();
            }
            // methode d'affichage des données dans ma datagridview
            public void affichDonneesdatagrid(DataGridView datagridview, int numbtn, int nbreligneParPage)
            {
                Connec();
                //pour obligé l'utilisateur a séléctionné toute la ligne
                datagridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                int lignefin = numbtn * nbreligneParPage;
                int lignedebut = lignefin - nbreligneParPage;
                //Creation des colonnes de ma datagridview
                datagridview.ColumnCount = 8;
                datagridview.Columns[0].Name = "ID";
                datagridview.Columns[1].Name = "N°";
                datagridview.Columns[2].Name = "Matricule";
                datagridview.Columns[3].Name = "Nom";
                datagridview.Columns[4].Name = "Montant de la bourse";
                datagridview.Columns[5].Name = "Type";
                datagridview.Columns[6].Name = "Session";
                datagridview.Columns[7].Name = "Statut";
                // interdire la saisis directement dans une colonne du datagridview
                datagridview.Columns[0].ReadOnly = true;
                datagridview.Columns[1].ReadOnly = true;
                datagridview.Columns[2].ReadOnly = true;
                datagridview.Columns[3].ReadOnly = true;
                datagridview.Columns[4].ReadOnly = true;
                datagridview.Columns[5].ReadOnly = true;
                datagridview.Columns[6].ReadOnly = true;
                datagridview.Columns[7].ReadOnly = true;
                // supprimer la premier colonne vide du datagridview et la dernier ligne du datagridview
                datagridview.RowHeadersVisible = false;
                datagridview.AllowUserToAddRows = false;
                // couleur des entetes de colonnes
                string requete = "";
                command = new MySqlCommand(requete, connection);
                Dictionary<string, object> parametre = new Dictionary<string, object>();

                    requete = "SELECT bourse.matricule,eleves.nom_eleve,bourse.montant,bourse.type,bourse.session,bourse.statut,bourse.id_bourse from bourse inner join eleves on bourse.matricule=eleves.matricule WHERE nom_eleve like @nom OR bourse.matricule like @matricule order by session limit " + nbreligneParPage + " offset " + lignedebut + "";
                    parametre.Add("@nom", "%" + Nom + "%");
                    parametre.Add("@matricule", Nom + "%");
                command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parameter in parametre)
                {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));

                }
                command.Prepare();
                reader = command.ExecuteReader();
                datagridview.Rows.Clear();
                number = lignedebut + 1;
                while (reader.Read())
                {
                    datagridview.Rows.Add(reader.GetValue(6).ToString(),number.ToString(), "" + reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), reader.GetValue(5).ToString());
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
            /*  
             * Méthode pour compté le nombre de ligne renvoyer par la requete
             * Calcule du nombre de page avec 30 lignes par page
             * Affichage des numéros en dessous du datagridview
             */
            // Initialisation d'un tableau dynamique qui va contenir les numéros de page s'il ya plusieurs pages
            public List<int> numPage = new List<int>();
            public int Compteligne(Label labeltete, Panel numpclob, Label backPage1, Label precedentPage, Label suivantPage, Label dernierPage, Label btnPage1, Label btnPage2, Label btnPage3, Label btnPage4, Label btnPage5, int numPageClick, int nbreLignePage)
            {
                float nbreligne = number - 1;
                float nbrePage = nbreligne / nbreLignePage;
                int newNbrePage = 0;
                if (nbrePage <= 1)
                {
                    // Une seul page trouvé
                    labeltete.Text = "1 / 1 page        " + nbreligne + " bourse(s) et prime(s)  enregistré(s) ";
                    numpclob.Visible = false;
                    btnPage1.Visible = false;
                    btnPage2.Visible = false;
                    btnPage3.Visible = false;
                    btnPage4.Visible = false;
                    btnPage5.Visible = false;
                    backPage1.Visible = false;
                    precedentPage.Visible = false;
                    suivantPage.Visible = false;
                    dernierPage.Visible = false;
                }
                else
                {
                    /* Si le nombre de page renvoyé contient une virgule, alors 
                     * on recupère le nombre qui vient après la virgule, on l'a converti en entier
                     * et on ajoute un pour avoir le nombre de page sans virgule
                     */
                    List<char> mastring = new List<char>();
                    string PartieEntier = "";
                    newNbrePage = 0;
                    string test = nbrePage.ToString();
                    for (int val = 0; val < test.Length; val++)
                    {
                        mastring.Add(test[val]);
                    }
                    // verifions si la virgule se trouve dans ma liste
                    if (mastring.Contains(',') == true)
                    {
                        for (int pa = 0; pa < mastring.Count; pa++)
                        {
                            if (mastring[pa] == ',')
                            {
                                break;
                            }
                            else
                            {
                                PartieEntier += mastring[pa];
                            }
                        }
                        newNbrePage = int.Parse(PartieEntier) + 1;
                    }
                    else
                    {
                        newNbrePage = (int)nbrePage;
                    }

                    // Plusieurs pages trouvé
                    numpclob.Visible = true;
                    labeltete.Text = numPageClick + " / " + newNbrePage + " page(s)        " + nbreligne + "  bourse(s) et prime(s)  enregistré(s) ";
                    // remplissage de mon dictionnaire
                    for (int b = 1; b <= newNbrePage; b++)
                    {
                        numPage.Add(b);
                    }
                    /*
                     * Si le nombre de page est inferieur ou égal à 5, on affiche 5 label
                     * Si le nombre de page est supérieur à 5, on affiche les 5 prémier label
                     * Les autres label seront affiché par le clique de l'utilisateur
                     */
                    if (newNbrePage <= 5)
                    {
                        // cache les bouton de gestion des page
                        backPage1.Visible = false;
                        precedentPage.Visible = false;
                        suivantPage.Visible = false;
                        dernierPage.Visible = false;
                        // Affichage des boutons de page en fonction du nombre de page
                        if (newNbrePage == 2)
                        {
                            btnPage1.Visible = true; btnPage2.Visible = true; btnPage3.Visible = false; btnPage4.Visible = false; btnPage5.Visible = false;
                        }
                        else if (newNbrePage == 3)
                        {
                            btnPage1.Visible = true; btnPage2.Visible = true; btnPage3.Visible = true; btnPage4.Visible = false; btnPage5.Visible = false;
                        }
                        else if (newNbrePage == 4)
                        {
                            btnPage1.Visible = true; btnPage2.Visible = true; btnPage3.Visible = true; btnPage4.Visible = true; btnPage5.Visible = false;
                        }
                        else if (newNbrePage == 5)
                        {
                            btnPage1.Visible = true; btnPage2.Visible = true; btnPage3.Visible = true; btnPage4.Visible = true; btnPage5.Visible = true;
                        }
                    }
                    else
                    {
                        backPage1.Visible = true;
                        precedentPage.Visible = true;
                        suivantPage.Visible = true;
                        dernierPage.Visible = true;
                        // Apparaitre les boutons de gestion des page
                        btnPage1.Visible = true; btnPage2.Visible = true; btnPage3.Visible = true; btnPage4.Visible = true; btnPage5.Visible = true;
                        if (btnPage1.Text == "1")
                        {
                            backPage1.Visible = false;
                            precedentPage.Visible = false;
                            suivantPage.Visible = true;
                            dernierPage.Visible = true;
                        }
                        else if (btnPage5.Text == newNbrePage.ToString())
                        {
                            backPage1.Visible = true;
                            precedentPage.Visible = true;
                            suivantPage.Visible = false;
                            dernierPage.Visible = false;
                        }
                        else
                        {
                            backPage1.Visible = true;
                            precedentPage.Visible = true;
                            suivantPage.Visible = true;
                            dernierPage.Visible = true;
                        }

                    }
                }
                return newNbrePage;
            }
        }
        // Methode d'affichage des session activer 
        private void listsession(Label session)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string requete = "Select nom_session from tabsession where statut='activer'";
            command = new MySqlCommand(requete, connection);
            command.Prepare();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
               session.Text =reader.GetValue(0).ToString();
            }
            reader.Close();
        }
        public NEW_BOURSEPRIME()
        {
            InitializeComponent();
            connexionDB();
            designDatagridview tabschool = new designDatagridview(tableaudonnees);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tabschool.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, 1, 15);
            // Connexion au serveur
            tabschool.Connec();

            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);

            // session activer
            listsession(label11);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel2, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            BourseETPrime nouveau = new BourseETPrime();
            nouveau.ShowDialog();
        }

        private void NEW_BOURSEPRIME_Paint(object sender, PaintEventArgs e)
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

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel4, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel5, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel6, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel7, 5, e.Graphics, borderColor, borderSize);
        }

        private void btnPage1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderlabel(btnPage1, 5, e.Graphics, borderColor, borderSize);
        }

        private void btnPage2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderlabel(btnPage2, 5, e.Graphics, borderColor, borderSize);
        }

        private void btnPage3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderlabel(btnPage3, 5, e.Graphics, borderColor, borderSize);
        }

        private void btnPage4_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderlabel(btnPage4, 5, e.Graphics, borderColor, borderSize);
        }

        private void btnPage5_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderlabel(btnPage5, 5, e.Graphics, borderColor, borderSize);
        }

        private void backPage1_Click(object sender, EventArgs e)
        {
            //Retiur à la prémière page
            btnPage5.Text = (5).ToString();
            btnPage4.Text = (4).ToString();
            btnPage3.Text = (3).ToString();
            btnPage2.Text = (2).ToString();
            btnPage1.Text = (1).ToString();
            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            string session = label11.Text;
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
            btnPage1.BackColor = Color.FromArgb(0, 89, 191);
            btnPage2.BackColor = Color.FromArgb(1, 31, 68);
            btnPage3.BackColor = Color.FromArgb(1, 31, 68);
            btnPage4.BackColor = Color.FromArgb(1, 31, 68);
            btnPage5.BackColor = Color.FromArgb(1, 31, 68);
        }

        private void precedentPage_Click(object sender, EventArgs e)
        {
            //Changement dans mes label de numéro de page
            int precedent = int.Parse(btnPage1.Text) - 1;
            btnPage5.Text = (precedent + 4).ToString();
            btnPage4.Text = (precedent + 3).ToString();
            btnPage3.Text = (precedent + 2).ToString();
            btnPage2.Text = (precedent + 1).ToString();
            btnPage1.Text = (precedent).ToString();
            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage5.Text);
            string session = label11.Text;
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
            btnPage1.BackColor = Color.FromArgb(1, 31, 68);
            btnPage2.BackColor = Color.FromArgb(1, 31, 68);
            btnPage3.BackColor = Color.FromArgb(1, 31, 68);
            btnPage4.BackColor = Color.FromArgb(1, 31, 68);
            btnPage5.BackColor = Color.FromArgb(0, 89, 191);
        }

        private void btnPage1_Click(object sender, EventArgs e)
        {
            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            string session = label11.Text;
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
            //Affectation des couleurs aux differents bouton après un clique sur une page précis
            btnPage1.BackColor = Color.FromArgb(0, 89, 191);
            btnPage2.BackColor = Color.FromArgb(1, 31, 68);
            btnPage3.BackColor = Color.FromArgb(1, 31, 68);
            btnPage4.BackColor = Color.FromArgb(1, 31, 68);
            btnPage5.BackColor = Color.FromArgb(1, 31, 68);
        }

        private void btnPage2_Click(object sender, EventArgs e)
        {
            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage2.Text);
            string session = label11.Text;
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
            //Affectation des couleurs aux differents bouton après un clique sur une page précis
            btnPage2.BackColor = Color.FromArgb(0, 89, 191);
            btnPage1.BackColor = Color.FromArgb(1, 31, 68);
            btnPage3.BackColor = Color.FromArgb(1, 31, 68);
            btnPage4.BackColor = Color.FromArgb(1, 31, 68);
            btnPage5.BackColor = Color.FromArgb(1, 31, 68);
        }

        private void btnPage3_Click(object sender, EventArgs e)
        {
            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage3.Text);
            string session = label11.Text;
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
            //Affectation des couleurs aux differents bouton après un clique sur une page précis
            btnPage3.BackColor = Color.FromArgb(0, 89, 191);
            btnPage1.BackColor = Color.FromArgb(1, 31, 68);
            btnPage2.BackColor = Color.FromArgb(1, 31, 68);
            btnPage4.BackColor = Color.FromArgb(1, 31, 68);
            btnPage5.BackColor = Color.FromArgb(1, 31, 68);
        }

        private void btnPage4_Click(object sender, EventArgs e)
        {
            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage4.Text);
            string session = label11.Text;
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
            //Affectation des couleurs aux differents bouton après un clique sur une page précis
            btnPage4.BackColor = Color.FromArgb(0, 89, 191);
            btnPage1.BackColor = Color.FromArgb(1, 31, 68);
            btnPage3.BackColor = Color.FromArgb(1, 31, 68);
            btnPage2.BackColor = Color.FromArgb(1, 31, 68);
            btnPage5.BackColor = Color.FromArgb(1, 31, 68);
        }

        private void btnPage5_Click(object sender, EventArgs e)
        {
            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage5.Text);
            string session = label11.Text;
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
            //Affectation des couleurs aux differents bouton après un clique sur une page précis
            btnPage5.BackColor = Color.FromArgb(0, 89, 191);
            btnPage1.BackColor = Color.FromArgb(1, 31, 68);
            btnPage3.BackColor = Color.FromArgb(1, 31, 68);
            btnPage4.BackColor = Color.FromArgb(1, 31, 68);
            btnPage2.BackColor = Color.FromArgb(1, 31, 68);
        }

        private void suivantPage1_Click(object sender, EventArgs e)
        {
            //Changement dans mes label de numéro de page
            int suivant = int.Parse(btnPage5.Text) + 1;
            btnPage5.Text = suivant.ToString();
            btnPage4.Text = (suivant - 1).ToString();
            btnPage3.Text = (suivant - 2).ToString();
            btnPage2.Text = (suivant - 3).ToString();
            btnPage1.Text = (suivant - 4).ToString();
            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage5.Text);
            string session = label11.Text;
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
            btnPage1.BackColor = Color.FromArgb(1, 31, 68);
            btnPage2.BackColor = Color.FromArgb(1, 31, 68);
            btnPage3.BackColor = Color.FromArgb(1, 31, 68);
            btnPage4.BackColor = Color.FromArgb(1, 31, 68);
            btnPage5.BackColor = Color.FromArgb(0, 89, 191);
        }

        private void dernierPage_Click(object sender, EventArgs e)
        {
            int numcont = int.Parse(btnPage5.Text);
            //infoTableData.Visible = true;
            string session = label11.Text;
            designDatagridview bv = new designDatagridview(tableaudonnees);
            int numpage = bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);

            //Aller à la dernière page
            btnPage5.Text = (numpage).ToString();
            btnPage4.Text = (numpage - 1).ToString();
            btnPage3.Text = (numpage - 2).ToString();
            btnPage2.Text = (numpage - 3).ToString();
            btnPage1.Text = (numpage - 4).ToString();
            numcont = int.Parse(btnPage5.Text);
            // Respecter l'ordre d'appel des differentes méthode
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
            btnPage1.BackColor = Color.FromArgb(1, 31, 68);
            btnPage2.BackColor = Color.FromArgb(1, 31, 68);
            btnPage3.BackColor = Color.FromArgb(1, 31, 68);
            btnPage4.BackColor = Color.FromArgb(1, 31, 68);
            btnPage5.BackColor = Color.FromArgb(0, 89, 191);
        }

        private void panel2_MouseHover(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Blue;
        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Transparent;
        }

        private void panel4_MouseHover(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Blue;
        }

        private void panel4_MouseLeave(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Transparent;
        }

        private void panel5_MouseHover(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Blue;
        }

        private void panel5_MouseLeave(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Transparent;
        }

        private void panel6_MouseHover(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Blue;
        }

        private void panel6_MouseLeave(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Transparent;
        }

        private void panel7_MouseHover(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Blue;
        }

        private void panel7_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Transparent;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            // recuperation de l'index de la section pour le filtrage

            designDatagridviewSearch tabschool = new designDatagridviewSearch(tableaudonnees, textBox1.Text);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tabschool.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, 1, 15);
            // Connexion au serveur
            tabschool.Connec();

            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            designDatagridviewSearch bv = new designDatagridviewSearch(tableaudonnees, textBox1.Text);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel11, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune bourse ou prime séléctionné pour la modification . ";
                string titre = "Modification";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string idbourse = (tableaudonnees.CurrentRow.Cells["ID"].Value).ToString();
                /* verifions le statut de la bourse/prime
                 * si statut egal a Encour alors modification possible
                 * si statut egal réglé alors impossible de modifier
                 */
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                try
                {
                    Boolean valStatut = false;
                    string req = "SELECT statut FROM bourse WHERE id_bourse='" + idbourse + "'";
                    command = new MySqlCommand(req, connection);
                    command.Prepare();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if(reader.GetValue(0).ToString() == "Encour")
                        {
                            valStatut = true;
                        }else
                        {
                            valStatut = false;
                        }
                    }
                    reader.Close();
                    if(valStatut.ToString() == "True")
                    {
                        EditBoursePrime edit = new EditBoursePrime(idbourse);
                        edit.ShowDialog();
                    }else
                    {
                        string messag = "Impossible de modifier une bourse ou une prime déjà utiliser";
                        string titre = "Modification";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                } 
                catch(Exception ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de l'affichage du formulaire de modification . "+ ex;
                    string titre = "Modification";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally { connection.Close(); }

            }
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune bourse ou prime séléctionné pour la suppression . ";
                string titre = "Suppression";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string idbourse = (tableaudonnees.CurrentRow.Cells["ID"].Value).ToString();
                /* verifions le statut de la bourse/prime
                 * si statut egal a Encour alors modification possible
                 * si statut egal réglé alors impossible de modifier
                 */
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                try
                {
                    string messa = "Souhaitez vous réellement supprimer cette bourse/prime ?";
                    string titr = "Suppression";
                    DialogResult result = MessageBox.Show(messa, titr, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        /* Verifions la valeur du statut de la bourse/prime séléctionné
                         * Si le statut égal a réglé alors réfusé la suppression
                         * Si le statut égal a Encour alors validé la suppression si toutes les données sont correct
                         */
                        Boolean siStatut = false;
                        string se = "SELECT statut FROM bourse WHERE id_bourse='" + idbourse + "'";
                        command = new MySqlCommand(se, connection);
                        command.Prepare();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if(reader.GetValue(0).ToString() == "Réglé")
                            {
                                siStatut = false;
                            }else if(reader.GetValue(0).ToString() == "Encour")
                            {
                                siStatut = true;
                            }
                        }
                        reader.Close();
                        
                        if(siStatut.ToString() == "True")
                        {
                            string del = "DELETE FROM bourse WHERE id_bourse='" + idbourse + "'";
                            command = new MySqlCommand(del, connection);
                            command.Prepare();
                            if (command.ExecuteNonQuery() == 1)
                            {
                                string messag = "La bourse / prime a été supprimer avec succès . ";
                                string titre = "Suppression";
                                // Programation des bouton de la boite de message
                                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if(siStatut.ToString() == "False")
                        {
                            string messag = "Impossible de supprimer une bourse/prime ayant un statut Réglé ";
                            string titre = "Suppression";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        
                    }else if(result == DialogResult.No)
                    {
                        string messag = "Suppression annulé. ";
                        string titre = "Suppression";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                   
                }
                catch (Exception ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de la suppression de la bourse / prime . " + ex;
                    string titre = "Suppression";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally { connection.Close(); }

            }
        
    }

        private void panel6_Click(object sender, EventArgs e)
        {
            designDatagridview tabschool = new designDatagridview(tableaudonnees);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tabschool.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, 1, 15);
            // Connexion au serveur
            tabschool.Connec();

            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune bourse ou prime séléctionné pour les détails . ";
                string titre = "Suppression";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string idbourse = (tableaudonnees.CurrentRow.Cells["ID"].Value).ToString();
                detailBourse detail = new detailBourse(idbourse);
                detail.ShowDialog();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            BourseETPrime nouveau = new BourseETPrime();
            nouveau.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            BourseETPrime nouveau = new BourseETPrime();
            nouveau.ShowDialog();
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Blue;
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Blue;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Transparent;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Transparent;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune bourse ou prime séléctionné pour la modification . ";
                string titre = "Modification";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string idbourse = (tableaudonnees.CurrentRow.Cells["ID"].Value).ToString();
                /* verifions le statut de la bourse/prime
                 * si statut egal a Encour alors modification possible
                 * si statut egal réglé alors impossible de modifier
                 */
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                try
                {
                    Boolean valStatut = false;
                    string req = "SELECT statut FROM bourse WHERE id_bourse='" + idbourse + "'";
                    command = new MySqlCommand(req, connection);
                    command.Prepare();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetValue(0).ToString() == "Encour")
                        {
                            valStatut = true;
                        }
                        else
                        {
                            valStatut = false;
                        }
                    }
                    reader.Close();
                    if (valStatut.ToString() == "True")
                    {
                        EditBoursePrime edit = new EditBoursePrime(idbourse);
                        edit.ShowDialog();
                    }
                    else
                    {
                        string messag = "Impossible de modifier une bourse ou une prime déjà utiliser";
                        string titre = "Modification";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (Exception ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de l'affichage du formulaire de modification . " + ex;
                    string titre = "Modification";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally { connection.Close(); }

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune bourse ou prime séléctionné pour la modification . ";
                string titre = "Modification";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string idbourse = (tableaudonnees.CurrentRow.Cells["ID"].Value).ToString();
                /* verifions le statut de la bourse/prime
                 * si statut egal a Encour alors modification possible
                 * si statut egal réglé alors impossible de modifier
                 */
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                try
                {
                    Boolean valStatut = false;
                    string req = "SELECT statut FROM bourse WHERE id_bourse='" + idbourse + "'";
                    command = new MySqlCommand(req, connection);
                    command.Prepare();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetValue(0).ToString() == "Encour")
                        {
                            valStatut = true;
                        }
                        else
                        {
                            valStatut = false;
                        }
                    }
                    reader.Close();
                    if (valStatut.ToString() == "True")
                    {
                        EditBoursePrime edit = new EditBoursePrime(idbourse);
                        edit.ShowDialog();
                    }
                    else
                    {
                        string messag = "Impossible de modifier une bourse ou une prime déjà utiliser";
                        string titre = "Modification";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (Exception ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de l'affichage du formulaire de modification . " + ex;
                    string titre = "Modification";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally { connection.Close(); }

            }
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Blue;
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Blue;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Transparent;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Transparent;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune bourse ou prime séléctionné pour la suppression . ";
                string titre = "Suppression";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string idbourse = (tableaudonnees.CurrentRow.Cells["ID"].Value).ToString();
                /* verifions le statut de la bourse/prime
                 * si statut egal a Encour alors modification possible
                 * si statut egal réglé alors impossible de modifier
                 */
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                try
                {
                    string messa = "Souhaitez vous réellement supprimer cette bourse/prime ?";
                    string titr = "Suppression";
                    DialogResult result = MessageBox.Show(messa, titr, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        /* Verifions la valeur du statut de la bourse/prime séléctionné
                         * Si le statut égal a réglé alors réfusé la suppression
                         * Si le statut égal a Encour alors validé la suppression si toutes les données sont correct
                         */
                        Boolean siStatut = false;
                        string se = "SELECT statut FROM bourse WHERE id_bourse='" + idbourse + "'";
                        command = new MySqlCommand(se, connection);
                        command.Prepare();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.GetValue(0).ToString() == "Réglé")
                            {
                                siStatut = false;
                            }
                            else if (reader.GetValue(0).ToString() == "Encour")
                            {
                                siStatut = true;
                            }
                        }
                        reader.Close();

                        if (siStatut.ToString() == "True")
                        {
                            string del = "DELETE FROM bourse WHERE id_bourse='" + idbourse + "'";
                            command = new MySqlCommand(del, connection);
                            command.Prepare();
                            if (command.ExecuteNonQuery() == 1)
                            {
                                string messag = "La bourse / prime a été supprimer avec succès . ";
                                string titre = "Suppression";
                                // Programation des bouton de la boite de message
                                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if (siStatut.ToString() == "False")
                        {
                            string messag = "Impossible de supprimer une bourse/prime ayant un statut Réglé ";
                            string titre = "Suppression";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }

                    }
                    else if (result == DialogResult.No)
                    {
                        string messag = "Suppression annulé. ";
                        string titre = "Suppression";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de la suppression de la bourse / prime . " + ex;
                    string titre = "Suppression";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally { connection.Close(); }

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune bourse ou prime séléctionné pour la suppression . ";
                string titre = "Suppression";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string idbourse = (tableaudonnees.CurrentRow.Cells["ID"].Value).ToString();
                /* verifions le statut de la bourse/prime
                 * si statut egal a Encour alors modification possible
                 * si statut egal réglé alors impossible de modifier
                 */
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                try
                {
                    string messa = "Souhaitez vous réellement supprimer cette bourse/prime ?";
                    string titr = "Suppression";
                    DialogResult result = MessageBox.Show(messa, titr, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        /* Verifions la valeur du statut de la bourse/prime séléctionné
                         * Si le statut égal a réglé alors réfusé la suppression
                         * Si le statut égal a Encour alors validé la suppression si toutes les données sont correct
                         */
                        Boolean siStatut = false;
                        string se = "SELECT statut FROM bourse WHERE id_bourse='" + idbourse + "'";
                        command = new MySqlCommand(se, connection);
                        command.Prepare();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.GetValue(0).ToString() == "Réglé")
                            {
                                siStatut = false;
                            }
                            else if (reader.GetValue(0).ToString() == "Encour")
                            {
                                siStatut = true;
                            }
                        }
                        reader.Close();

                        if (siStatut.ToString() == "True")
                        {
                            string del = "DELETE FROM bourse WHERE id_bourse='" + idbourse + "'";
                            command = new MySqlCommand(del, connection);
                            command.Prepare();
                            if (command.ExecuteNonQuery() == 1)
                            {
                                string messag = "La bourse / prime a été supprimer avec succès . ";
                                string titre = "Suppression";
                                // Programation des bouton de la boite de message
                                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if (siStatut.ToString() == "False")
                        {
                            string messag = "Impossible de supprimer une bourse/prime ayant un statut Réglé ";
                            string titre = "Suppression";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }

                    }
                    else if (result == DialogResult.No)
                    {
                        string messag = "Suppression annulé. ";
                        string titre = "Suppression";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de la suppression de la bourse / prime . " + ex;
                    string titre = "Suppression";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally { connection.Close(); }

            }
        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Blue;
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Blue;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Transparent;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Transparent;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            designDatagridview tabschool = new designDatagridview(tableaudonnees);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tabschool.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, 1, 15);
            // Connexion au serveur
            tabschool.Connec();

            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            designDatagridview tabschool = new designDatagridview(tableaudonnees);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tabschool.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, 1, 15);
            // Connexion au serveur
            tabschool.Connec();

            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage1, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
        }

        private void label4_MouseHover(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Blue;
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Blue;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Transparent;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Transparent;
        }

        private void label13_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune bourse ou prime séléctionné pour les détails . ";
                string titre = "Suppression";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string idbourse = (tableaudonnees.CurrentRow.Cells["ID"].Value).ToString();
                detailBourse detail = new detailBourse(idbourse);
                detail.ShowDialog();
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune bourse ou prime séléctionné pour les détails . ";
                string titre = "Suppression";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string idbourse = (tableaudonnees.CurrentRow.Cells["ID"].Value).ToString();
                detailBourse detail = new detailBourse(idbourse);
                detail.ShowDialog();
            }
        }

        private void label13_MouseHover(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Blue;
        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Blue;
        }

        private void label13_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Transparent;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Transparent;
        }
    }
}
