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
    public partial class EditPoste : Form
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
        // Récupération des informations dépuis la BD grace à l'id
        public class Modification
        {
            private TextBox nom;
            private CheckBox econome, surveillant, censeur, prof, comptable, autres;
            private TextBox Nom { set; get; }
            private CheckBox Econome { set; get; }
            private CheckBox Surveillant { set; get; }
            private CheckBox Censeur { set; get; }
            private CheckBox Prof { set; get; }
            private CheckBox Comptable { set; get; }
            private CheckBox Autres { set; get; }

            // Constructeur
            public Modification(TextBox nom, CheckBox Econome, CheckBox Surveillant, CheckBox Censeur, CheckBox Prof, CheckBox Comptable, CheckBox Autres)
            {
                this.Nom = nom; this.Econome = Econome; this.Surveillant = Surveillant; this.Censeur = Censeur; this.Prof = Prof;
                this.Comptable = Comptable; this.Autres = Autres; 
            }

            // Récuperation des informations dans la base de données
            public void modifinfo(int idline, Label stockline)
            {
                stockline.Text = idline.ToString();
                stockline.Visible = false;
               
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "SELECT * FROM fonction where id_fonction = @id";
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
                    if (reader.GetValue(2).ToString() == "OUI") { Econome.Checked = true; } else { Econome.Checked = false; }
                    if (reader.GetValue(3).ToString() == "OUI") { Censeur.Checked = true; } else { Censeur.Checked = false; }
                    if (reader.GetValue(4).ToString() == "OUI") { Surveillant.Checked = true; } else { Surveillant.Checked = false; }
                    if (reader.GetValue(5).ToString() == "OUI") { Prof.Checked = true; } else { Prof.Checked = false; }
                    if (reader.GetValue(6).ToString() == "OUI") { Comptable.Checked = true; } else { Comptable.Checked = false; }
                    if (reader.GetValue(7).ToString() == "OUI") { Autres.Checked = true; } else { Autres.Checked = false; }
                   
                }
                reader.Close();
            }
        }
        // class Insertion des modifications apporté aux données
        public class InsertModifFonction
        {
            private string nomfonction, econome, surveillant, censeur, prof, comptable, autres;
            long idligne;
            private string Nomfonction { get; set; }
            private string Econome { get; set; }
            private string Surveillant { get; set; }
            private string Censeur { get; set; }
            private string Prof { get; set; }
            private string Comptable { get; set; }
            private string Autres { get; set; }
            private long Idligne { get; set; }
            // Constructeur
            public InsertModifFonction(string nom, string econome, string surveillant, string censeur, string prof, string comptable, string autres, long idligne)
            {
                this.Nomfonction = nom; this.Econome = econome; this.Surveillant = surveillant; this.Censeur = censeur;
                this.Prof = prof; this.Comptable = comptable; this.Autres = autres;this.Idligne = idligne;

                insertdb();
            }
            private void insertdb()
            {
                int nbreetabli = 0;
                var connection2 = new MySqlConnection(connectionstring);
                connection2.Open();
                string req = "SELECT nom_fonction,count(nom_fonction) as nbreligne from fonction where id_fonction <> @idfonction AND nom_fonction=@nom";
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@nom", Nomfonction);
                para.Add("@idfonction", Idligne);
                var command2 = new MySqlCommand(req, connection2);
                foreach (KeyValuePair<string, object> parametres in para)
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

                    string requete = "UPDATE fonction SET nom_fonction = @nom, inscription = @econome, gest_enseignent = @gestprof, discipline_eleves = @discipline, enseignement = @prof, comptabilite = @compta, autres = @autres, dernier_modification = @modif WHERE id_fonction= @identLigne";
                    string datem = DateTime.Now.ToString("yyyy-MM-dd");
                    Dictionary<string, object> parametre = new Dictionary<string, object>();
                    parametre.Add("@nom", Nomfonction);
                    parametre.Add("@econome", Econome);
                    parametre.Add("@gestprof", Censeur);
                    parametre.Add("@discipline", Surveillant);
                    parametre.Add("@prof", Prof);
                    parametre.Add("@compta", Comptable);
                    parametre.Add("@autres", Autres);
                    parametre.Add("@modif", datem);
                    parametre.Add("@identLigne", Idligne);
                    var connection = new MySqlConnection(connectionstring);
                    connection.Open();
                    var command = new MySqlCommand(requete, connection);
                    foreach (KeyValuePair<string, object> parametres in parametre)
                    {
                        command.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    command.Prepare();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        string messag = "Les informations du poste a été modifié avec succès. ";
                        string titre = "Modification ";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }else
                    {
                        string messag = "Nous nous sommes heurtés à un problème lors de la modification de ce poste. ";
                        string titre = "Insertion ";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }else
                {
                    string messag = "Le poste " + Nomfonction + " existe déjà";
                    string titre = "Duplication";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                reader2.Close();


            }
        }
        public EditPoste(int idline)
        {
            InitializeComponent();
            connexionDB();
            Modification modifinfos = new Modification(nomposte, checkBox1, checkBox3, checkBox2, checkBox4, checkBox5, checkBox6);
            modifinfos.modifinfo(idline, label1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox5.Text = @"  Sous l’autorité du directeur ou du proviseur, l’économe s’occupe de la gestion des biens de l’établissement.

    Il perçoit les frais scolaires, réceptionne des dons et legs, exécute les dépenses.Il participe à l’élaboration et à la validation du projet de budget de l’établissement à la veille de chaque rentrée scolaire.

    Il s’occupe des infrastructures mobilières et immobilières, en s’assurant de la qualité et de l’intégrité des salles de classes, des tables bancs et de tout le mobilier destiné au personnel enseignant et administratif.

    L’économe est également chargé de payer les primes diverses, de payer les salaires des enseignants volontaires, de faire les versements du FSE à l’inspection et les fonds de l’établissement à la banque ou dans une institution de microfinance.

Il dirige l’organisation des fêtes dans l’établissement.";
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

        private void button7_Click(object sender, EventArgs e)
        {
            textBox5.Text = "L'agent comptable est chargé de la comptabilité générale dans les conditions définies par le plan comptable applicable à l'établissement. Il est chargé, entre autres, du paiement des dépenses.";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox5.Text = "S'il n'y a pas de responsabilités mentionnées ci-dessus qui correspondent à la responsabilité du poste que vous souhaitez créer, cochez autres pour créer le poste.";
        }

        private void fermFormAddSchool_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel2, 5, e.Graphics, Color.Blue, 1);
        }

        private void EditPoste_Paint(object sender, PaintEventArgs e)
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
            arrondirbouton.FormRegionAndBorderbouton(button7, 7, e.Graphics, Color.Blue, 1);
        }

        private void button8_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button8, 7, e.Graphics, Color.Blue, 1);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button2, 7, e.Graphics, Color.Blue, 1);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string poste = nomposte.Text;
            string econome, surveillant, censeur, enseignant, comptable, autres;
            if (checkBox1.Checked == true) { econome = "OUI"; } else { econome = "NON"; }
            if (checkBox3.Checked == true) { surveillant = "OUI"; } else { surveillant = "NON"; }
            if (checkBox2.Checked == true) { censeur = "OUI"; } else { censeur = "NON"; }
            if (checkBox4.Checked == true) { enseignant = "OUI"; } else { enseignant = "NON"; }
            if (checkBox5.Checked == true) { comptable = "OUI"; } else { comptable = "NON"; }
            if (checkBox6.Checked == true) { autres = "OUI"; } else { autres = "NON"; }
            long idline = long.Parse(label1.Text.ToString());
            if (nomposte.Text == "")
            {
                string messag = "Veuillez entrer le nom du poste (ex : comptable, econome, enseignant,...)";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (checkBox1.Checked == false && checkBox3.Checked == false && checkBox2.Checked == false && checkBox4.Checked == false && checkBox5.Checked == false && checkBox6.Checked == false)
            {
                string messag = "S'il n'y a pas de responsabilités mentionnées ci-dessus qui correspondent à la responsabilité du poste que vous souhaitez créer, cochez autres pour créer le poste.";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                InsertModifFonction newschool = new InsertModifFonction(poste, econome, surveillant, censeur, enseignant, comptable, autres,idline);
            }
        }
    }
}
