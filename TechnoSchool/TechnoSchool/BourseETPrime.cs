using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TechnoSchool.AddEleve;

namespace TechnoSchool
{
    public partial class BourseETPrime : Form
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
            public void affichDonnees(DataGridView datagridview, int appel, string nom)
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
                if (appel == 0)
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
                else if (appel == 1)
                {
                    string requete = "Select matricule,nom_eleve,prenom_eleve from eleves where nom_eleve like @nom OR prenom_eleve like @prenom OR matricule like @matricule order by nom_eleve asc";
                    command = new MySqlCommand(requete, connection);
                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("@nom", "%" + nom + "%");
                    para.Add("@prenom", "%" + nom + "%");
                    para.Add("@matricule", nom + "%");
                    foreach (KeyValuePair<string, object> parametre in para)
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
        // Methode pour afficher les sessions
        public void listsession(ComboBox section)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string requete = "Select id_session,nom_session from tabsession order by nom_session asc";
            command = new MySqlCommand(requete, connection);
            command.Prepare();
            section.Items.Clear();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                section.Items.Add(reader["nom_session"]);
            }
            reader.Close();

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

        // Iinsertion des bourses
        public class Bourse
        {
            public string Matricule { get; set; }
            public string Type { get; set; }
            public string Motif { get; set; }
            public string Session { get; set; }
            public int Montant { get; set; }
            public int siDonneesValide { get; set; }
            // Constructeur
            public Bourse(string matricule, string type, string motif, string session, int montant)
            {
                this.Matricule = matricule; this.Type = type; this.Motif = motif; this.Session = session;
                this.Montant = montant;
            }

            public void insertBourse()
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                
                try
                {
                    // Vérifier l'élève a déja été inscrit a cette session
                    int siInscrit = 0;
                    string req2 = "SELECT count(matricule) as nbre FROM inscription WHERE matricule='" + Matricule + "' AND session='" + Session + "'";
                    command = new MySqlCommand(req2, connection);
                    command.Prepare();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        siInscrit = int.Parse(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                    if(siInscrit >= 1)
                    {
                        string messag = "Impossible d'attribué une "+Type + ". L'élève a déja été inscrit à la session " + Session + " ";
                        string titre = "Insertion ";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        siDonneesValide = 0;
                    }
                    else
                    {
                        int nbreligne = 0;
                        // verifi si l'élève choisi a déja bénéficier d'une bourse a cette session
                        string requete = "SELECT count(matricule) as nbre FROM bourse WHERE type='" + Type + "' AND session='" + Session + "' AND matricule='" + Matricule + "'";
                        command = new MySqlCommand(requete, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            nbreligne = int.Parse(reader.GetValue(0).ToString());
                        }
                        reader.Close();
                        if (nbreligne == 0)
                        {
                            string req = "INSERT INTO bourse(matricule,type,motif,montant,session,statut) values(@matricule,@type,@motif,@montant,@session,@statut)";
                            Dictionary<string, object> para = new Dictionary<string, object>();
                            para.Add("@matricule", Matricule);
                            para.Add("@type", Type);
                            para.Add("@motif", Motif);
                            para.Add("@montant", Montant);
                            para.Add("@session", Session);
                            para.Add("@statut", "Encour");
                            command = new MySqlCommand(req, connection);
                            foreach (KeyValuePair<string, object> parametres in para)
                            {
                                command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                            }
                            command.Prepare();
                            if (command.ExecuteNonQuery() == 1)
                            {
                                string messag = Type + " enregistré avec succès";
                                string titre = "Insertion ";
                                // Programation des bouton de la boite de message
                                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                siDonneesValide = 1;
                            }
                            else
                            {
                                string messag = "Nous nous sommes heurtés à un problème lors de l'enregistrement de cette bourse. ";
                                string titre = "Insertion ";
                                // Programation des bouton de la boite de message
                                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                siDonneesValide = 0;
                            }
                        }
                        else
                        {
                            string messag = "Cet élève à une " + Type + " encour pour cette session(" + Session + ")";
                            string titre = "Insertion ";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            siDonneesValide = 0;
                        }

                    }

                }
                catch(MySqlException ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de l'enregistrement de cette bourse. "+ex;
                    string titre = "Insertion ";
                    siDonneesValide = 0;
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public BourseETPrime()
        {
            InitializeComponent();
            connexionDB();
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            designDatagridview eleve = new designDatagridview(tableaudonnees);
            string chain = nomposte.Text;
            eleve.affichDonnees(tableaudonnees, 0, chain);
            // Liste des sessions
            listsession(comboBox1);
        }
        
        private void fermFormAddSchool_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BourseETPrime_Load(object sender, EventArgs e)
        {

        }

        private void nomposte_KeyUp(object sender, KeyEventArgs e)
        {
            designDatagridview eleve = new designDatagridview(tableaudonnees);
            string chain = nomposte.Text;
            eleve.affichDonnees(tableaudonnees, 1, chain);
        }

        private void tableaudonnees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // verifie si au moins une ligne existe pour une modification
            if (tableaudonnees.Rows.Count == 0)
            {
                string messag = "Aucune élève séléctionné pour la bourse. ";
                string titre = "Bourse et Prime";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string nom = (tableaudonnees.CurrentRow.Cells["nom"].Value).ToString();
                string prenoms = (tableaudonnees.CurrentRow.Cells["prenom"].Value).ToString();
                string matricule = (tableaudonnees.CurrentRow.Cells["matricule"].Value).ToString();
                nomSchool.Text = nom + " " + prenoms; prenom.Text = matricule;
              //  donneeExist(label13, textBox2, comboBox1, textBox4, textBox5, comboBox3, label11, label12);
            }
        }

        private void adresse_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void BourseETPrime_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel10, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void panelDegrader2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelDegrader2, 5, e.Graphics, borderColor, borderSize);
        }

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 5, e.Graphics, borderColor, borderSize);
        }

        private void panelDegrader1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelDegrader1, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel3, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel2, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel5, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel7, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel15, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel9, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel11, 5, e.Graphics, borderColor, borderSize);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button2, 8, e.Graphics, borderColor, borderSize);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(prenom.Text == "")
            {
                string messag = "Aucune élève séléctionné pour la bourse. ";
                string titre = "Bourse et Prime";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else if(comboBox3.Text == "")
            {
                string messag = "Veuillez séléctionné le type de reduction (bourse ou prime )";
                string titre = "Bourse et Prime";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(nationalite.Text == "")
            {
                string messag = "Veuillez renseigné le motif de la bourse ou prime";
                string titre = "Bourse et Prime";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(comboBox1.Text == "")
            {
                string messag = "la bourse est active pour quelle session ?";
                string titre = "Bourse et Prime";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else if(adresse.Text == "")
            {
                string messag = "Veuillez entrer le montant de la bourse ou prime";
                string titre = "Bourse et Prime";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else if(VerifiSiNombre(adresse.Text) == 0)
            {
                string messag = "Montant incorrect";
                string titre = "Bourse et Prime";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else if(adresse.Text == "0")
            {
                string messag = "Le montant de la bourse doit etre superieur à 0";
                string titre = "Bourse et Prime";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else
            {
                string matricule = prenom.Text; string type = comboBox3.Text; string motif = nationalite.Text;
                string session = comboBox1.Text; int montant = int.Parse(adresse.Text);
                Bourse prime = new Bourse(matricule, type, motif, session, montant);
                prime.insertBourse();
                if(prime.siDonneesValide == 1)
                {
                    // on vide les champs
                    nomSchool.Text = ""; prenom.Text = ""; adresse.Text = "";
                }
            }
        }
    }
}
