﻿using MySql.Data.MySqlClient;
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
    public partial class deletenscription : Form
    {
        // ---------------
        private int borderRadius = 20;
        private int borderSize = 1;
        private Color borderColor = Color.RoyalBlue;

        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataReader reader3;
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
        // Class pour afficher la liste des versements d'un élève x pour inscription
        public class Listverse
        {

            public int number;
            private DataGridView TableauDonnees { set; get; }
            public string Matricule { set; get; }
            public string Session { set; get; }
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
                datagridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
        // class de suppression d'un versement et et modification du montant versé et reste de la table inscription
        public class Modif
        {
            public string Session { set; get; }
            public string Recu { set; get; }
            public string Matricule { set; get; }
            public Modif(string recu)
            {
                this.Recu = recu;
                infoverse();
            }

            // récupération de tout les infos du versement séléctionné
            public void infoverse()
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                MySqlTransaction trans;
                trans = connection.BeginTransaction();
                var connection2 = new MySqlConnection(connectionstring);
                connection2.Open();
                MySqlTransaction trans2;
                trans2 = connection2.BeginTransaction();
                try
                {
                    // Recuperation du matricule et la session pour la recherche
                    string matricule2 = ""; string session2 = "";
                    string ve = "SELECT matricule,session from caissescolarite where numrecu='" + Recu + "'";
                    command = new MySqlCommand(ve, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        matricule2 = reader.GetValue(0).ToString(); session2 = reader.GetValue(1).ToString();
                    }
                    reader.Close();

                    // verifi si la scolarite a deja été versé
                    int nbrescolarite = 0;
                    string ver = "SELECT count(id_scolarite) as nbre from scolarite where matricule='" + matricule2 + "' AND session='" + session2 + "'";
                    command = new MySqlCommand(ver, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        nbrescolarite = int.Parse(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                    if(nbrescolarite == 0)
                    {
                        string messag = "Souhaitez vous réellemnt supprimer ce versement ?";
                        string titre = "Suppression";
                        // Programation des bouton de la boite de message
                        DialogResult result = MessageBox.Show(messag, titre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            string se = "SELECT matricule,session FROM caissescolarite where numrecu=@recu";
                            Dictionary<string, object> para = new Dictionary<string, object>();
                            para.Add("@recu", Recu);
                            command = new MySqlCommand(se, connection);
                            foreach (KeyValuePair<string, object> parametres in para)
                            {
                                command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                            }
                            command.Prepare();
                            command.Transaction = trans;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                Matricule = reader.GetValue(0).ToString();
                                Session = reader.GetValue(1).ToString();

                            }
                            para.Clear();
                            reader.Close();
                            // Modification du montant de caisse
                            string re = "DELETE FROM caissescolarite where numrecu=@recu";
                            para.Add("@recu", Recu);
                            command = new MySqlCommand(re, connection);
                            foreach (KeyValuePair<string, object> paramet in para)
                            {
                                command.Parameters.Add(new MySqlParameter(paramet.Key, paramet.Value));
                            }
                            command.Prepare();
                            command.Transaction = trans;
                            if (command.ExecuteNonQuery() >= 1)
                            {
                                // Somme de tous les versements dans la caisse pour inscription et pour le matricule et la session
                                long sommcaiss = 0;
                                string scais = "SELECT monverse from caissescolarite where matricule=@matricule AND session=@session AND typeversement=@type";
                                Dictionary<string, object> para2 = new Dictionary<string, object>();
                                para2.Add("@matricule", Matricule);
                                para2.Add("@session", Session);
                                para2.Add("@type", "inscription");
                                command = new MySqlCommand(scais, connection);
                                foreach (KeyValuePair<string, object> parametres in para2)
                                {
                                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                                }
                                command.Prepare();
                                command.Transaction = trans;
                                reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    sommcaiss += long.Parse(reader.GetValue(0).ToString());
                                }
                                reader.Close();



                                // Séléction de la ligne inscription et modification de la ligne en question
                                string seinscript = "SELECT montinscription,montverse,reste From inscription where matricule=@matricule AND session=@session";
                                Dictionary<string, object> para3 = new Dictionary<string, object>();
                                para3.Add("@matricule", Matricule);
                                para3.Add("@session", Session);
                                command = new MySqlCommand(seinscript, connection);
                                foreach (KeyValuePair<string, object> parametres in para3)
                                {
                                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                                }
                                command.Prepare();
                                command.Transaction = trans;
                                reader3 = command.ExecuteReader();
                                while (reader3.Read())
                                {
                                    long montinscription = long.Parse(reader3.GetValue(0).ToString());
                                    long reste = (montinscription - sommcaiss);
                                    string statut = "";
                                    if (reste > 0)
                                    {
                                        statut = "Non soldé";
                                    }
                                    else if (reste == 0)
                                    {
                                        statut = "Soldé";
                                    }
                                    // Verifions si le montant versé est egal a zero
                                    // Si c'est le cas supprimer la ligne en question dans la table inscription
                                    // Si non modifions la ligne conserné
                                    if(sommcaiss == 0)
                                    {
                                        string supp = "DELETE FROM inscription where matricule='" + Matricule + "' AND session='" + Session + "'";
                                        command = new MySqlCommand(supp, connection2);
                                        command.Prepare();
                                        command.Transaction = trans2;
                                        command.ExecuteNonQuery();
                                    }else
                                    {
                                        // modification de la ligne dans la table inscription
                                        string inser = "UPDATE inscription SET montverse=@montv, reste=@reste, statut=@statut where matricule=@matricule AND session=@session";
                                        Dictionary<string, object> para4 = new Dictionary<string, object>();
                                        para4.Add("@montv", sommcaiss);
                                        para4.Add("@reste", reste);
                                        para4.Add("@statut", statut);
                                        para4.Add("@matricule", Matricule);
                                        para4.Add("@session", Session);
                                        command = new MySqlCommand(inser, connection2);
                                        foreach (KeyValuePair<string, object> parametres in para4)
                                        {
                                            command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                                        }
                                        command.Prepare();
                                        command.Transaction = trans2;
                                        if (command.ExecuteNonQuery() >= 1)
                                        {


                                        }
                                    }
                                    
                                }
                                reader3.Close();
                            }
                        }
                        else
                        {
                            // Annuler la suppression
                            string messag2 = "Suppression annulé";
                            string titre2 = "Suppression";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag2, titre2, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                      
                        trans.Commit(); trans2.Commit();
                        string messag2ok = "Versement supprimer avec succès";
                        string titre2ok = "Suppression";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag2ok, titre2ok, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string messag2 = "Impossible de supprimé ce versement. Veuillez d'abord supprimer tout les versements des frais scolaire de cette élève.";
                        string titre2 = "Suppression";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag2, titre2, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    reader3.Close();
                }
                catch (Exception error)
                {
                    string messag2 = "Nous nous sommes heurtés à un problème lors de la suppression du versement "+error;
                    string titre2 = "Erreur";
                    trans.Rollback(); trans2.Rollback();
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag2, titre2, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
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
        public deletenscription(string matricule, string session)
        {
            InitializeComponent();
            connexionDB();
            label2.Text = matricule;
            Listverse list = new Listverse(tableaudonnees, matricule, session);
            this.tableaudonnees.EnableHeadersVisualStyles = false;
            tableaudonnees.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            tableaudonnees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 8, e.Graphics, borderColor, borderSize);
        }

        private void deletenscription_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void tableaudonnees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string recu = (tableaudonnees.CurrentRow.Cells["N° reçu"].Value).ToString();
            Modif newmod = new Modif(recu);
        }
    }
}
