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
    public partial class AddFonction : Form
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
        public class InsertFonction
        {
            private string nomfonction, econome, surveillant, censeur, prof, comptable,autres;
            public int siDonneesValide = 0;
            private string Nomfonction { get; set; }
            private string Econome { get; set; }
            private string Surveillant { get; set; }
            private string Censeur { get; set; }
            private string Prof { get; set; }
            private string Comptable { get; set; }
            private string Autres { get; set; }
            // Constructeur
            public InsertFonction(string nom, string econome, string surveillant, string censeur, string prof, string comptable, string autres)
            {
                this.Nomfonction = nom; this.Econome = econome; this.Surveillant = surveillant; this.Censeur = censeur;
                this.Prof = prof; this.Comptable = comptable;this.Autres = autres;

                insertdb();
            }
            private void insertdb()
            {
                int nbreetabli = 0;
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string req = "SELECT nom_fonction,count(nom_fonction) as nbreligne from fonction where nom_fonction=@nom";
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@nom", Nomfonction);
                var command2 = new MySqlCommand(req, connection);
                foreach(KeyValuePair<string, object> parametres in para)
                {
                    command2.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                }
                command2.Prepare();
                reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    nbreetabli = int.Parse(reader2.GetValue(1).ToString());

                }
                if (nbreetabli < 1)
                {
                    string requete = "INSERT INTO fonction(nom_fonction,inscription,gest_enseignent,discipline_eleves,enseignement,comptabilite,autres,date_creation,dernier_modification) values(@nom,@inscription,@gestprof,@disciplineleve,@prof,@compta,@autre,@creation,@modification)";
                    string datec = DateTime.Now.ToString("yyyy-MM-dd");
                    string datem = DateTime.Now.ToString("yyyy-MM-dd");
                    Dictionary<string, object> parametre = new Dictionary<string, object>();
                    parametre.Add("@nom", Nomfonction);
                    parametre.Add("@inscription", Econome);
                    parametre.Add("@gestprof", Censeur);
                    parametre.Add("@disciplineleve", Surveillant);
                    parametre.Add("@prof", Prof);
                    parametre.Add("@compta", Comptable);
                    parametre.Add("@autre", Autres);
                    parametre.Add("@creation", datec);
                    parametre.Add("@modification", datem);

                    var connection2 = new MySqlConnection(connectionstring);
                    connection2.Open();
                    var command = new MySqlCommand(requete, connection2);
                    foreach (KeyValuePair<string, object> parametres in parametre)
                    {
                        command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    command.Prepare();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        string messag = "Nouveau poste enregister avec succès. ";
                        string titre = "Insertion ";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        siDonneesValide = 1;
                    }
                    else
                    {
                        string messag = "Nous nous sommes heurtés à un problème lors de l'enregistrement de ce poste. ";
                        string titre = "Insertion ";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        siDonneesValide = 0;
                    }
                }else
                {
                    string messag = "Le poste "+ Nomfonction + " existe déjà";
                    string titre = "Duplication";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    siDonneesValide = 0;
                }

                reader2.Close();
               

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
        }
        public AddFonction()
        {
            InitializeComponent();
            connexionDB();
        }

        private void fermFormAddSchool_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddFonction_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel2, 5, e.Graphics, Color.Blue, 1);
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button1, 7, e.Graphics, Color.Blue, 1);
        }

        private void button4_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button4, 7, e.Graphics, Color.Blue, 1);
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button5, 7, e.Graphics, Color.Blue, 1);
        }

        private void button6_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button6, 7, e.Graphics, Color.Blue, 1);
        }

        private void button7_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button7, 5, e.Graphics, Color.Blue, 1);
        }

        private void button4_Paint_1(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button4, 5, e.Graphics, Color.Blue, 1);
        }

        private void button5_Paint_1(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button5, 5, e.Graphics, Color.Blue, 1);
        }

        private void button6_Paint_1(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button6, 5, e.Graphics, Color.Blue, 1);
        }

        private void button7_Paint_1(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button7, 5, e.Graphics, Color.Blue, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string poste = nomposte.Text;
            string econome, surveillant, censeur, enseignant, comptable, autres;
            if(checkBox1.Checked == true) { econome = "OUI"; }else { econome = "NON"; }
            if(checkBox3.Checked == true) { surveillant = "OUI"; } else { surveillant = "NON"; }
            if(checkBox2.Checked == true) { censeur = "OUI"; } else { censeur = "NON"; }
            if(checkBox4.Checked == true) { enseignant = "OUI"; } else { enseignant = "NON"; }
            if(checkBox5.Checked == true) { comptable = "OUI"; } else { comptable = "NON"; }
            if(checkBox6.Checked == true) { autres = "OUI"; } else { autres = "NON"; }

            if (nomposte.Text == "")
            {
                string messag = "Veuillez entrer le nom du poste (ex : comptable, econome, enseignant,...)";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else if(checkBox1.Checked == false && checkBox3.Checked == false && checkBox2.Checked == false && checkBox4.Checked == false && checkBox5.Checked == false && checkBox6.Checked == false)
            {
                string messag = "S'il n'y a pas de responsabilités mentionnées ci-dessus qui correspondent à la responsabilité du poste que vous souhaitez créer, cochez autres pour créer le poste.";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else
            {
            InsertFonction newschool = new InsertFonction(poste, econome, surveillant, censeur, enseignant, comptable, autres);
                int valuerenvoi = newschool.siDonneesValide;
                if(valuerenvoi == 1)
                {
                    nomposte.Text = ""; checkBox1.Checked = false; checkBox3.Checked = false;
                    checkBox2.Checked = false; checkBox4.Checked = false;
                    checkBox5.Checked = false; checkBox6.Checked = false;
                }
            }
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button2, 5, e.Graphics, Color.Blue, 1);
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button3, 5, e.Graphics, Color.Blue, 1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox5.Text = "L'agent comptable est chargé de la comptabilité générale dans les conditions définies par le plan comptable applicable à l'établissement. Il est chargé, entre autres, du paiement des dépenses.";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox5.Text = @"  Le surveillant est chargé de la surveillance des élèves, en collège ou au lycée, qu'ils 
soient publics ou privés, de même que dans les permanences en cas d'absence de l'enseignant. De plus,
le surveillant doit également s'assurer du respect du règlement interne de l'établissement par
l'ensemble des élèves. Celui-ci fait aussi partie intégrante de l'équipe éducative, et de ce fait,
aide également l'équipe enseignante et administrative de l'établissement. Le surveillant fait office de
lien entre les élèves et l'équipe administrative ou enseignante. En outre, sa mission consiste aussi à intégrer
les élèves handicapés, au sein du collège ou du lycée dans lequel il exerce ses fonctions. Enfin, le surveillant
peut être assigné à l'entrée ou la sortie de l'établissement et aura pour tâche de filtrer les entrées et sorties
des élèves. Ce dernier peut par ailleurs effectuer des rondes dans les espaces extérieurs ou dans les couloirs
afin de s'assurer qu'il n'y a aucun problème.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox5.Text = @"  Sous l’autorité du directeur ou du proviseur, l’économe s’occupe de la gestion des biens de l’établissement.

    Il perçoit les frais scolaires, réceptionne des dons et legs, exécute les dépenses.Il participe à l’élaboration et à la validation du projet de budget de l’établissement à la veille de chaque rentrée scolaire.

    Il s’occupe des infrastructures mobilières et immobilières, en s’assurant de la qualité et de l’intégrité des salles de classes, des tables bancs et de tout le mobilier destiné au personnel enseignant et administratif.

    L’économe est également chargé de payer les primes diverses, de payer les salaires des enseignants volontaires, de faire les versements du FSE à l’inspection et les fonds de l’établissement à la banque ou dans une institution de microfinance.

Il dirige l’organisation des fêtes dans l’établissement.";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox5.Text = @" le travail de l'ACE(Adjoint au Chef de l'Etablissement) se situe à trois niveaux de responsabilité. En effet, il a des responsabilités au niveau administratif, au niveau pédagogique et au niveau de l'encadrement des élèves.

1- Au niveau administratif, l’Adjoint, élabore les projets de circulaires, les notes de services relatives aux réunions avec tous les personnels. Il établit les procès verbaux et comptes rendus des réunions qu’il soumet à l’approbation du Chef d’Établissement.

2- Au niveau pédagogique, l’Adjoint sous la supervision du Chef d’Établissement, confectionne les emplois du temps. Contrôle les documents pédagogiques. Il veille à l’application du règlement intérieur par les élèves et préside souvent seul des conseils de classes. L’Adjoint prend part aux visites de classes programmées ou voulues par le Chef d’Établissement.

3- Au niveau de l'encadrement et du contrôle, l’Adjoint s’occupe de la présence des élèves aux cours, établit les billets d’annulation de zéro et participe de façon active aux activités de vie scolaire. En outre, l’Adjoint supervise, sous l’autorité du Chef d’Établissement, l’organisation des opérations liées aux examens et compositions qui se déroulent dans l’établissement.";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox5.Text = @"Dans un système éducatif, les enseignants sont le facteur le plus déterminant de l’apprentissage des élèves. Bien plus que de simples transmetteurs de savoirs, ils apportent aux enfants les outils nécessaires pour analyser des problèmes, les résoudre et utiliser efficacement l’information.";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox5.Text = "S'il n'y a pas de responsabilités mentionnées ci-dessus qui correspondent à la responsabilité du poste que vous souhaitez créer, cochez autres pour créer le poste.";
        }

        private void button8_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button8, 5, e.Graphics, Color.Blue, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            nomposte.Text = ""; checkBox1.Checked = false; checkBox3.Checked = false;
            checkBox2.Checked = false; checkBox4.Checked = false;
            checkBox5.Checked = false; checkBox6.Checked = false;
        }
    }
}
