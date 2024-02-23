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
    public partial class NEW_FONCTION : Form
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
                // couleur des entetes de colonnes
                string requete = "Select * from fonction order by nom_fonction asc";
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
                datagridview.ColumnCount = 6;
                datagridview.Columns[0].Name = "N°";
                datagridview.Columns[1].Name = "Nom";
                datagridview.Columns[2].Name = "Inscription";
                datagridview.Columns[3].Name = "Gestion enseignant";
                datagridview.Columns[4].Name = "Discipline élèves";
                datagridview.Columns[5].Name = "Enseignement";
                // interdire la saisis directement dans une colonne du datagridview
                datagridview.Columns[0].ReadOnly = true;
                datagridview.Columns[1].ReadOnly = true;
                datagridview.Columns[2].ReadOnly = true;
                datagridview.Columns[3].ReadOnly = true;
                datagridview.Columns[4].ReadOnly = true;
                datagridview.Columns[5].ReadOnly = true;
                // supprimer la premier colonne vide du datagridview et la dernier ligne du datagridview
                datagridview.RowHeadersVisible = false;
                datagridview.AllowUserToAddRows = false;
                // couleur des entetes de colonnes
                string requete = "Select * from fonction order by nom_fonction asc limit " + nbreligneParPage + " offset " + lignedebut + "";
                command = new MySqlCommand(requete, connection);
                command.Prepare();
                reader = command.ExecuteReader();
                datagridview.Rows.Clear();
                number = lignedebut + 1;
                while (reader.Read())
                {
                    datagridview.Rows.Add(number.ToString(), "" + reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), reader.GetValue(5).ToString());
                    number++;
                }
                reader.Close();
                // verifier le nombre de ligne renvoyer
                if(number - 1 < 1)
                {

                }else
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
                    labeltete.Text = "1 / 1 page        " + nbreligne + " poste(s)  enregistré(s)";
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
                    labeltete.Text = numPageClick + " / " + newNbrePage + " page(s)        " + nbreligne + " poste(s)  enregistré(s)";
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
        // classe de recuperation de la ligne séléctionné pour récupéré l'id de la ligne et l'envoyer dans la fenetre detailSchool
        public class SearchId
        {

            // Propriétés
            private string nom;
            // Accesseurs
            private string Nom { set; get; }

            // Constructeur
            public SearchId(string nom)
            {
                this.Nom = nom;
            }
            // methode de recuperation de l'id de la ligne séléctionné
            public int idLigne(int idline)
            {
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "SELECT id_fonction, nom_fonction from fonction where nom_fonction=@nom";
                Dictionary<string, object> parametre = new Dictionary<string, object>();
                parametre.Add("@nom", Nom);
                command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parameter in parametre)
                {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));

                }
                command.Prepare();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string stringid = reader.GetValue(0).ToString();
                    idline = int.Parse(stringid);
                }
                reader.Close();
                return idline;
            }
        }
        public class deleteData
        {
            public string nom;
            public string Nom { set; get; }
            // construction
            public deleteData(string nom)
            {
                this.Nom = nom; 
                delete();
            }

            // methode de suppression
            private void delete()
            {
                // Confirmation de la suppression de l'etablissement scolaire
                string messagconf = "Souhaitez-vous réellement supprimer le poste " + Nom +" ?. ";
                string titreconf = "Confirmation";
                // Programation des bouton de la boite de message
                DialogResult result = MessageBox.Show(messagconf, titreconf, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    var connection = new MySqlConnection(connectionstring);
                    connection.Open();

                    // Verifions si un employer à été créer avec se poste
                    // Si c'est le cas refusé la suppression du poste
                    // Dans le cas contraire validé lasuppression
                    int idfonction = 0;
                    // Recuperation de l'id de la fonction a partir du nom
                    string ve = "SELECT id_fonction from fonction where nom_fonction='"+Nom+"'";
                    var command = new MySqlCommand(ve, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        idfonction = int.Parse(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                    int nbreemployer = 0;
                    string ver = "SELECT count(id_employer) as nbreEmployer from employer where id_fonction='"+idfonction+"'";
                    command = new MySqlCommand(ver, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        nbreemployer = int.Parse(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                    if(nbreemployer == 0)
                    {
                        string requete = "DELETE FROM fonction where nom_fonction=@nom";
                        Dictionary<string, object> parametre = new Dictionary<string, object>();
                        parametre.Add("@nom", Nom);

                        command = new MySqlCommand(requete, connection);
                        foreach (KeyValuePair<string, object> parametres in parametre)
                        {
                            command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                        }
                        command.Prepare();
                        if (command.ExecuteNonQuery() >= 1)
                        {
                            string messag = "Poste supprimé avec succès. ";
                            string titre = "Suppression";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            string messag = "Echec de suppression du poste. Veuillez réessayer. ";
                            string titre = "Suppression";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }else
                    {
                        string messag = @"Impossible de supprimer cette fonction.
Au moins un employer à été enregistré avec cette fonction";
                        string titre = "Suppression";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    
                }
                else if (result == DialogResult.No)
                {
                    // Confirmation de la suppression de l'etablissement scolaire
                    string messagCan = "Suppression annulée";
                    string titreCan = "Confirmation";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messagCan, titreCan, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }


            }
        }
        public NEW_FONCTION()
        {
            InitializeComponent();
            connexionDB();
            designDatagridview tabschool = new designDatagridview(tableaudonnees);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tabschool.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, 1, 15);
            // Connexion au serveur
            tabschool.Connec();

            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
        }

        private void NEW_FONCTION_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
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

        private void panel7_MouseHover(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Blue;
        }

        private void panel7_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Transparent;
        }

        private void panel6_MouseHover(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Blue;
        }

        private void panel6_MouseLeave(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Transparent;
        }

        private void panel5_MouseHover(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Blue;
        }

        private void panel5_MouseLeave(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Transparent;
        }

        private void panel4_MouseHover(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Blue;
        }

        private void panel2_MouseHover(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Blue;
        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Transparent;
        }

        private void panel4_MouseLeave(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Transparent;
        }

        private void NEW_FONCTION_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 8, e.Graphics, borderColor, borderSize);
        }

        private void panel6_ClientSizeChanged(object sender, EventArgs e)
        {

        }

        private void panel6_Click(object sender, EventArgs e)
        {
            designDatagridview tabschool = new designDatagridview(tableaudonnees);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tabschool.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, 1, 15);
            // Connexion au serveur
            tabschool.Connec();

            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            AddFonction poste = new AddFonction();
            poste.ShowDialog();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            designDatagridview tabschool = new designDatagridview(tableaudonnees);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tabschool.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, 1, 15);
            // Connexion au serveur
            tabschool.Connec();

            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
        }

        private void label13_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucun poste séléctionné pour plus de détails. ";
                string titre = "Détails";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                SearchId newde = new SearchId(nom);
                int valeur = newde.idLigne(0);
                detailsPoste details = new detailsPoste(valeur);
                details.Show();
            }
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucun poste séléctionné pour la suppression. ";
                string titre = "Suppression";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                deleteData supprimer = new deleteData(nom);
            }
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucun poste séléctionné pour la modification. ";
                string titre = "Modification";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                SearchId newde = new SearchId(nom);
                int valeur = newde.idLigne(0);
                EditPoste details = new EditPoste(valeur);
                details.ShowDialog();
            }
            
        }

        private void label1_MouseHover(object sender, EventArgs e)
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

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Blue;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            AddFonction poste = new AddFonction();
            poste.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            AddFonction poste = new AddFonction();
            poste.ShowDialog();
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {

        }

        private void label2_MouseHover(object sender, EventArgs e)
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

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Blue;
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucun poste séléctionné pour la modification. ";
                string titre = "Modification";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                SearchId newde = new SearchId(nom);
                int valeur = newde.idLigne(0);
                EditPoste details = new EditPoste(valeur);
                details.ShowDialog();
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucun poste séléctionné pour la modification. ";
                string titre = "Modification";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                SearchId newde = new SearchId(nom);
                int valeur = newde.idLigne(0);
                EditPoste details = new EditPoste(valeur);
                details.ShowDialog();
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

        private void panel5_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucun poste séléctionné pour la suppression. ";
                string titre = "Suppression";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                deleteData supprimer = new deleteData(nom);
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucun poste séléctionné pour la suppression. ";
                string titre = "Suppression";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                deleteData supprimer = new deleteData(nom);
            }

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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            designDatagridview tabschool = new designDatagridview(tableaudonnees);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tabschool.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, 1, 15);
            // Connexion au serveur
            tabschool.Connec();

            // Respecter l'ordre d'appel des differentes méthode
            int numcont = int.Parse(btnPage1.Text);
            designDatagridview bv = new designDatagridview(tableaudonnees);
            bv.Compteligne(tetedonnees, panel8, backPage1, precedentPage, suivantPage, dernierPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, numcont, 15);
            bv.affichDonneesdatagrid(tableaudonnees, numcont, 15);
        }

        private void label13_MouseHover(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Transparent;
        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Transparent;
        }

        private void label13_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Transparent;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Transparent;
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucun poste séléctionné pour plus de détails. ";
                string titre = "Détails";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                SearchId newde = new SearchId(nom);
                int valeur = newde.idLigne(0);
                detailsPoste details = new detailsPoste(valeur);
                details.Show();
            }

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucun poste séléctionné pour plus de détails. ";
                string titre = "Détails";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                SearchId newde = new SearchId(nom);
                int valeur = newde.idLigne(0);
                detailsPoste details = new detailsPoste(valeur);
                details.Show();
            }

        }
    }
}
