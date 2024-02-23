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
    public partial class formodifInscription : Form
    {
        // ---------------
        private int borderRadius = 20;
        private int borderSize = 1;
        private Color borderColor = Color.RoyalBlue;

        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataReader reader2;
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
        // class afficher le montant du versement séléctionné
        public class donnees
        {
            public string Recu { set; get; }
            public TextBox Mont { set; get; }
            public Label Mat { set; get; }
            public donnees(string recu, TextBox mont, Label matr)
            {
                this.Recu = recu; this.Mont = mont; this.Mat = matr;
                montvers();
            }
            // methode pour récupéré le montant
            public void montvers()
            {
                string se = "SELECT matricule,session,monverse FROM caissescolarite where numrecu=@recu";
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@recu", Recu);
                var command = new MySqlCommand(se, connection);
                foreach (KeyValuePair<string, object> parametres in para)
                {
                    command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                }
                command.Prepare();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Mont.Text = reader.GetValue(2).ToString();
                    Mat.Text = reader.GetValue(0).ToString();
                }
                para.Clear();
                reader.Close();
            }
        }
        // class de modification d'un versement et et modification du montant versé et reste de la table inscription
        public class Modif
        {
            public string Session { set; get; }
            public string Recu { set; get; }
            public string Matricule { set; get; }
            public string Newmontant { set; get; }
         public Modif(string recu, string newmontant)
            {
                this.Recu = recu; this.Newmontant = newmontant;
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
                    // Somme de tous les versements dans la caisse pour inscription et pour le matricule et la session
                    long sommcaiss = 0;
                    string scais = "SELECT monverse from caissescolarite where matricule=@matricule AND session=@session AND typeversement=@type AND numrecu<>@recu";
                    Dictionary<string, object> para2 = new Dictionary<string, object>();
                    para2.Add("@matricule", Matricule);
                    para2.Add("@session", Session);
                    para2.Add("@recu", Recu);
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
                    sommcaiss += int.Parse(Newmontant);
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
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        long montinscription = long.Parse(reader.GetValue(0).ToString());
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
                        if (montinscription >= sommcaiss)
                        {
                            // Verifions si le nouveau montant versé est egal a zero
                            // Si c'est le cas supprimer la ligne en question dans la table inscription
                            // Si non modifions la ligne conserné
                            if(sommcaiss == 0)
                            {
                                string supp = "DELETE FROM inscription where matricule='" + Matricule + "' AND session='" + Session + "'";
                                command = new MySqlCommand(supp, connection2);
                                command.Prepare();
                                command.Transaction = trans2;
                                command.ExecuteNonQuery();
                                if(int.Parse(Newmontant) == 0)
                                {
                                    string del = "DELETE FROM caissescolarite where numrecu='" + Recu + "'";
                                    command = new MySqlCommand(del, connection2);
                                    command.Prepare();
                                    command.Transaction = trans2;
                                    command.ExecuteNonQuery();
                                }else
                                {
                                    // Modification du montant de caisse
                                    string re = "UPDATE caissescolarite SET monverse=@montv where numrecu=@recu";
                                    para.Add("@montv", Newmontant);
                                    para.Add("@recu", Recu);
                                    command = new MySqlCommand(re, connection2);
                                    foreach (KeyValuePair<string, object> paramet in para)
                                    {
                                        command.Parameters.Add(new MySqlParameter(paramet.Key, paramet.Value));
                                    }
                                    command.Prepare();
                                    command.Transaction = trans2;
                                    command.ExecuteNonQuery();
                                }
                            }else
                            {
                                if(int.Parse(Newmontant) == 0)
                                {
                                    string del = "DELETE FROM caissescolarite where numrecu='" + Recu + "'";
                                    command = new MySqlCommand(del, connection2);
                                    command.Prepare();
                                    command.Transaction = trans2;
                                    command.ExecuteNonQuery();
                                }
                                else
                                {
                                    // Modification du montant de caisse
                                    string re = "UPDATE caissescolarite SET monverse=@montv where numrecu=@recu";
                                    para.Add("@montv", Newmontant);
                                    para.Add("@recu", Recu);
                                    command = new MySqlCommand(re, connection2);
                                    foreach (KeyValuePair<string, object> paramet in para)
                                    {
                                        command.Parameters.Add(new MySqlParameter(paramet.Key, paramet.Value));
                                    }
                                    command.Prepare();
                                    command.Transaction = trans2;
                                    if (command.ExecuteNonQuery() >= 1)
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
                                        command.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        // inscription non modifier
                                        throw new Exception();
                                    }
                                }
                                
                            }
                            

                        }
                        else
                        {
                            // refus la modification
                            string messag = "Le montant versé par l'élève est supérieur au montant de l'inscription";
                            string titre = "Modification";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            throw new Exception();
                        }

                    }
                    reader.Close();
                    trans.Commit(); trans2.Commit();
                    string messagok = "Modification effectuer avec succès";
                    string titreok = "Modification";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messagok, titreok, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.Close();
                }
               catch(Exception error)
                {
                    // refus la modification
                    string messag = "Nous nous sommes heurtés à un problème lors de la modification "+error;
                    string titre = "Modification";
                    trans.Rollback(); trans2.Rollback();
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    connection.Close();
                }
            }
        }
        public formodifInscription(string recu)
        {
            InitializeComponent();
            connexionDB();
            donnees infos = new donnees(recu, textBox1, label2);
            label1.Text = recu;
            label1.Visible = false;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                string messag = "Veuillez entrer le montant versé par l'élève";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else
            {
                string mont = textBox1.Text; 
                string recu = label1.Text;
                Modif mod = new Modif(recu,mont);
            }
            
        }

        private void formodifInscription_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel4, 5, e.Graphics, Color.Blue, 1);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button2, 8, e.Graphics, borderColor, borderSize);
        }
    }
}
