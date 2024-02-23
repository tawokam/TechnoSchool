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
    public partial class EditInscription : Form
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
        // Class pour afficher la liste des versements d'un élève x pour inscription
        public class Listverse
        {

            public int number;
            private DataGridView TableauDonnees { set; get; }
            public string Matricule { set; get; }
            public string Session {set; get; }
            // constructeur
            public Listverse(DataGridView tableauDonnees, string matricule, string session)
            {
                this.TableauDonnees = tableauDonnees;
                this.Matricule = matricule; this.Session = session;
                listversement(TableauDonnees);
            }
            public void listversement(DataGridView datagridview)
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                //pour obligé l'utilisateur a séléctionné toute la ligne
                //datagridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                //Creation des colonnes de ma datagridview
                datagridview.ColumnCount = 4;
                datagridview.Columns[0].Name = "Matricule";
                datagridview.Columns[1].Name = "Montant";
                datagridview.Columns[2].Name = "Date";
                datagridview.Columns[3].Name = "N° reçu";
                // interdire la saisis directement dans une colonne du datagridview
                datagridview.Columns[0].ReadOnly = true;
                datagridview.Columns[1].ReadOnly = false;
                datagridview.Columns[2].ReadOnly = true;
                datagridview.Columns[3].ReadOnly = true;
                // supprimer la premier colonne vide du datagridview et la dernier ligne du datagridview
                datagridview.RowHeadersVisible = false;
                datagridview.AllowUserToAddRows = false;
                string requete = "Select matricule,monverse,dateverse,numrecu From caissescolarite where matricule=@matricule AND session=@session AND typeversement=@type";
                Dictionary<string, object> parametre = new Dictionary<string, object>();
                parametre.Add("@matricule", Matricule);
                parametre.Add("@session", Session);
                parametre.Add("@type", "inscription");
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
                     datagridview.Rows.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), string.Format("{0:dd / MM / yyyy}", reader.GetValue(2)), reader.GetValue(3));
                }
                reader.Close();
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
        // Liste des classes
        public void listclasse(ComboBox liste, Label id,string session,string matricule)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string idcl = "";
            // recupere l'id de la classe dans la table inscription
            string requete = "SELECT id_classe FROM inscription WHERE matricule='" + matricule + "' AND session='" + session + "'";
            command = new MySqlCommand(requete, connection);
            command.Prepare();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                idcl = reader.GetValue(0).ToString();
            }
            reader.Close();
            string req = "SELECT nom_classe,id_classe FROM classe order by nom_classe";
            command = new MySqlCommand(req, connection);
            command.Prepare();
            MySqlDataReader reader2 = command.ExecuteReader();
            while (reader2.Read())
            {
                if(reader2.GetValue(1).ToString() == idcl)
                {
                    liste.Items.Add(reader2.GetValue(0).ToString());
                    liste.SelectedItem = reader2.GetValue(0).ToString();
                    id.Text = reader2.GetValue(1).ToString();
                }
                else
                {
                    liste.Items.Add(reader2.GetValue(0).ToString());
                }
                
            }
            reader2.Close();
            connection.Close();
        }
        // Methode de modification de la classe d'inscription d'un élève
        public void modifClassInscrit(string matricule, string session , string idclass)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            MySqlTransaction trans;
            trans = connection.BeginTransaction();
            try
            {
                // Verifions si l'élève a déja versé les frais scolaire
                int montVerse = 0;
                string req1 = "SELECT montverse FROM scolarite WHERE matricule='" + matricule + "' AND session='" + session + "'";
                command = new MySqlCommand(req1, connection);
                command.Transaction = trans;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    montVerse = int.Parse(reader.GetValue(0).ToString());
                }
                reader.Close();
                // Recuperons le montant de l'inscription de cette classe
                int newmontinscription = 0;
                int newmontscolarite = 0;
                string req2 = "SELECT montinscription,montscolarite FROM classe where id_classe='" + idclass + "'";
                command = new MySqlCommand(req2, connection);
                command.Transaction = trans;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    newmontinscription = int.Parse(reader.GetValue(0).ToString());
                    newmontscolarite = int.Parse(reader.GetValue(1).ToString());
                }
                reader.Close();

                // Si le montant de la scolarité de la nouvelle classe est supérieur ou égal au montant versé alors 
                // effectuer la modification sinon Echec...
                if (montVerse <= newmontscolarite)
                {
                   

                    // Recuperons le montant versé pour inscription
                    int montverse = 0;
                    string req3 = "SELECT montverse FROM inscription WHERE matricule='" + matricule + "' AND session='" + session + "'";
                    command = new MySqlCommand(req3, connection);
                    command.Transaction = trans;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        montverse = int.Parse(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                    if(newmontinscription < montverse)
                    {
                        throw new Exception();
                    }else
                    {
                        // Modification de la classe, du montant de l'inscription, du reste et du statut de l'élève
                        int reste = newmontinscription - montverse;
                        string statut = "";
                        if(reste > 0) { statut = "Non soldé"; }else if(reste == 0) { statut = "Soldé"; }
                        string req4 = "UPDATE inscription SET montinscription='" + newmontinscription + "',reste='" + reste + "', statut='" + statut + "', id_classe='" + idclass + "' WHERE matricule='"+matricule+"' AND session='"+session+"'";
                        command = new MySqlCommand(req4, connection);
                        command.Transaction = trans;
                        command.ExecuteNonQuery();

                        // verification si l'élève à une bourse
                        int sombourse = 0;
                        string bou = "SELECT sum(montant) as totalreduc FROM bourse WHERE matricule='" + matricule + "' AND session='" + session + "'";
                        command = new MySqlCommand(bou, connection);
                        command.Prepare();
                        command.Transaction = trans;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if(reader.GetValue(0).ToString() == "null" || reader.GetValue(0).ToString() == "")
                            {
                                sombourse = 0;
                            }
                            else
                            {
                                sombourse = int.Parse(reader.GetValue(0).ToString());
                            }
                        }
                        reader.Close();
                        //modifier le montant de la scolarite
                        int montscolarite = (newmontscolarite - sombourse);

                        // séléction de la classe de l'élève pour récupéré le montant versé
                        int montverseScol = 0;
                        string scolMv = "SELECT montverse FROM scolarite WHERE matricule='" + matricule + "' AND session='" + session + "'";
                        command = new MySqlCommand(scolMv, connection);
                        command.Prepare();
                        command.Transaction = trans;
                        reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            montverseScol = int.Parse(reader.GetValue(0).ToString());
                        }
                        reader.Close();
                        // verifions si le montant versé est supérieur au montant de la scolarité après enlèvement de la bourse/prime si l'élève en a une pour cette session
                        if(montverseScol <= montscolarite)
                        {
                            int restescol = montscolarite - montverseScol;
                            string statutscol = ""; if(reste == 0) { statutscol = "Soldé"; }else { statutscol = "Non soldé"; }
                            string upscol = "UPDATE scolarite SET montscolarite='" + montscolarite + "', reste='"+restescol+"', statut='"+statutscol+"', id_classe='"+idclass+"'  WHERE matricule='" + matricule + "' AND session='" + session + "'";
                            command = new MySqlCommand(upscol, connection);
                            command.Transaction = trans;
                            command.ExecuteNonQuery();
                        }else { throw new Exception(); }

                    }
                }
                else
                {
                  
                    throw new Exception();
                }
                trans.Commit();
                string messag2 = "Classe modifier avec succès";
                string titre2 = "Modification ";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag2, titre2, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                string messag = @"Nous nous somme heurté à un problème lors de la modification de la classe de l'élève 
Causes probables:
le nouveau montant de l'inscription est inférieur au montant versé par l'élève pour inscription
le nouveau montant de la scolarité est inférieur au montant versé par l'élève pour les frais scolaire

                    "+ex;
                string titre = "Modification";
                trans.Rollback();
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally { connection.Close(); }


        }
        public static string matricule2; public static string session2;
        public EditInscription(string matricule, string session)
        {
            InitializeComponent();
            matricule2 = matricule; session2 = session;
            connexionDB();
            label2.Text = matricule;
            Listverse list = new Listverse(tableaudonnees, matricule, session);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            listclasse(comboBox2, label3, session, matricule);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox6_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void EditInscription_Paint(object sender, PaintEventArgs e)
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

        private void tableaudonnees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string recu = (tableaudonnees.CurrentRow.Cells["N° reçu"].Value).ToString();
            formodifInscription modif = new formodifInscription(recu);
            modif.ShowDialog();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            try
            {
                string nomcla = comboBox2.Text;
                string req = "SELECT id_classe FROM classe WHERE nom_classe='" + nomcla + "'";
                command = new MySqlCommand(req, connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    label3.Text = reader.GetValue(0).ToString();
                }
                reader.Close();
            }
           catch( Exception ex)
            {
                MessageBox.Show("Erreur", "Une erreur est survenue lors du chargement des classes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { connection.Close(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string idcl = label3.Text;
            modifClassInscrit(matricule2, session2, idcl);
        }
    }
}
