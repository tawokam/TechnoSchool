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

    public partial class Index : Form
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

        // Donnees de connexion
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
                using (FileStream fichier = File.Create(cheminfichierConfig))
                {
                    fichier.Close();
                    string chemin = Path.Combine(Environment.CurrentDirectory, "FileConfig/FileConfig.ini");
                    GestionFileIni ger = new GestionFileIni(chemin);
                    try
                    {
                        // Ecriture des données du serveur dans le fichier fileConfig
                        ger.WriteIni("Server", "server", "localhost");
                        ger.WriteIni("User", "user", "root");
                        ger.WriteIni("Mdp", "mdp", "");
                        ger.WriteIni("DataBase", "base de donnees", "technosoft");
                        ger.WriteIni("Langue", "langue", "français");

                        // lecture des informations dans le fichier
                        string server = ger.ReadIni("Server", "server");
                        string user = ger.ReadIni("User", "user");
                        string mdp = ger.ReadIni("Mdp", "mdp");
                        string DB = ger.ReadIni("DataBase", "base de donnees");
                        connectionstring = "server=" + server + ";user id=" + user + ";password=" + mdp + ";database=" + DB + "; SslMode=none";
                    }
                    catch (Exception ex)
                    {
                        string messag = "Nous nous somme heurté à un problème lors de la configuration du serveur" + ex;
                        string titre = "Achat licence ";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                }
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
        // récupérons le dernier etablissement creer
        public class School
        {
            public Label Nom { set; get; }

            public School(Label nom)
            {
                this.Nom = nom;
            }

            // recuperation du nom de l'établissement scolaire
            public void nomschool()
            {
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                int idmax = 0;
                // Récupération du dernier numero de recu
                string res = "select id_etablissement,max(id_etablissement) as school from etablissement";
                var command4 = new MySqlCommand(res, connection);
                command4.Prepare();
                reader = command4.ExecuteReader();
                while (reader.Read())
                { 
                    if(reader.GetValue(1).ToString() == "")
                    {

                    }else
                    {
                        idmax = int.Parse(reader.GetValue(1).ToString());
                    }
                    
                }
                reader.Close();

                // recuperation du nom de l'etablissement
                var connection1 = new MySqlConnection(connectionstring);
                connection1.Open();
                string res2 = "select nom_etabli from etablissement where id_etablissement="+idmax+"";
                var command2 = new MySqlCommand(res2, connection1);
                command2.Prepare();
                reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    Nom.Text = reader2.GetValue(0).ToString();
                }
                reader2.Close();
            }
        }

        public class designBordure
        {
            private int borderRadius = 20;
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
        // Ouverture du formulaire d'achat de la licence si aucun fichier de licence exist
        public void FileLicence()
        {
            string cheminfichierConfig = Path.Combine(Environment.CurrentDirectory, "FileConfig/Licence.ini");
            try
            {
                // Vérifiez si le fichier existe

                if (File.Exists(cheminfichierConfig))
                {
                    // Si oui, rien ne se passe
                }
                else
                {
                    // Si Non, ouvrir le formulaire de configuration de la licence
                    LicenceGestion licenc = new LicenceGestion();
                    licenc.ShowDialog();
                }

            }
            catch (Exception e)
            {
                string messag = "Problème rencontré lors de la création du fichier de configuration" + e.Message;
                string titre = "Configuration";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        // methode permettant de creer les tables dans la base de données si elles n'existe pas

            public void Tables()
            {
                var connection = new MySqlConnection(connectionstring);
                connection.Open();

               string re = @"
CREATE TABLE IF NOT EXISTS `fonction` (
`id_fonction` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`nom_fonction` VARCHAR(255) NOT NULL,
`inscription` VARCHAR(10) NOT NULL,
`gest_enseignent` VARCHAR(10) NOT NULL,
`discipline_eleves` VARCHAR(10) NOT NULL,
`enseignement` VARCHAR(10) NOT NULL,
`comptabilite` VARCHAR(10) NOT NULL,
`autres` VARCHAR(10) NOT NULL,
`date_creation` date NOT NULL,
`dernier_modification` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `etablissement`(
`id_etablissement` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`nom_etabli` VARCHAR(255) NOT NULL,
`mail` TEXT NOT NULL,
`localisation` TEXT NOT NULL,
`bp` VARCHAR(255) NOT NULL,
`phone1` INT(11) NOT NULL,
`phone2` INT(11) NOT NULL,
`NIU` VARCHAR(255) NOT NULL,
`NRC` VARCHAR(255) NOT NULL,
`logo` LONGBLOB NOT NULL,
`paieEnLigne` VARCHAR(255) NOT NULL,
`Numom` INT(11) NOT NULL,
`nummm` INT(11) NOT NULL,
`date_creation` DATE NOT NULL,
`dernier_modification` DATE NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `employer`(
`id_employer` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`nom_employer` TEXT NOT NULL,
`telephone1_employer` INT(11) NOT NULL,
`telephone2_employer` INT(11) NOT NULL,
`sexe_employer` VARCHAR(10) NOT NULL,
`adresseMail_employer` TEXT NOT NULL,
`quartier_employer` TEXT NOT NULL,
`id_fonction` INT(11) NOT NULL,
`datenaiss_employer` DATE NOT NULL,
`grandiplome` VARCHAR(255) NOT NULL,
`specialitediplome` VARCHAR(255) NOT NULL,
`cv_employer` LONGBLOB NOT NULL,
`photo_employer`LONGBLOB NOT NULL,
`numerourgence` INT(11) NOT NULL,
`mdpemployer` VARCHAR(255) NOT NULL,
`date_creation` DATE NOT NULL,
`date_modification` DATE NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `tabsession`(
`id_session` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`nom_session` VARCHAR(100) NOT NULL,
`statut` VARCHAR(50) NOT NULL,
`date_creation` DATE NOT NULL,
`date_modification` DATE NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `tabsection`(
`id_section` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`nom_section` VARCHAR(100) NOT NULL,
`date_creation` DATE NOT NULL,
`date_modification` DATE NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `classe`(
`id_classe` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`nom_classe` VARCHAR(255) NOT NULL,
`montscolarite` INT(11) NOT NULL,
`montinscription` INT(11) NOT NULL,
`id_section` INT(11) NOT NULL,
`date_creation` DATE NOT NULL,
`date_modification` DATE NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `eleves`(
`id_eleve` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`nom_eleve` VARCHAR(255) NOT NULL,
`prenom_eleve` VARCHAR(255) NOT NULL,
`matricule` VARCHAR(100) NOT NULL,
`date_naiss` DATE NOT NULL,
`lieu_naiss` VARCHAR(100) NOT NULL,
`sexe` VARCHAR(10) NOT NULL,
`nationalite` VARCHAR(100) NOT NULL,
`adresse` VARCHAR(255) NOT NULL,
`maladie` VARCHAR(255) NOT NULL,
`ApteEps` VARCHAR(10) NOT NULL,
`autreInfo` TEXT NOT NULL,
`photo` LONGBLOB NULL,
`nompere` VARCHAR(255) NOT NULL,
`phonepere` INT(11) NOT NULL,
`nommere` VARCHAR(255) NOT NULL,
`phonemere` INT(11) NOT NULL,
`nomtutteur` VARCHAR(255) NOT NULL,
`phonetutteur` INT(11) NOT NULL,
`date_creation` DATE NOT NULL,
`date_modification` DATE NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `inscription`(
`id_inscription` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`matricule` VARCHAR(100) NOT NULL,
`id_classe` INT(11) NOT NULL,
`montinscription` INT(11) NOT NULL,
`montverse` INT(11) NOT NULL,
`reste` INT(11) NOT NULL,
`session` VARCHAR(50) NOT NULL,
`statut` VARCHAR(10) NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `scolarite`(
`id_scolarite` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`matricule` VARCHAR(100) NOT NULL,
`id_classe` INT(11) NOT NULL,
`montscolarite` INT(11) NOT NULL,
`montverse` INT(11) NOT NULL,
`reste` INT(11) NOT NULL,
`session` VARCHAR(50) NOT NULL,
`statut` VARCHAR(10) NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `caissescolarite`(
`id_caisse` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`monverse` INT(11) NOT NULL,
`dateverse` DATE NOT NULL,
`heure` TIME NOT NULL,
`matricule` VARCHAR(100) NOT NULL,
`session` VARCHAR(20) NOT NULL,
`numrecu` INT(11) NOT NULL,
`typeversement` VARCHAR(30) NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `bourse`(
`id_bourse` INT(11) NOT NULL PRIMARY KEY AUTO_INCREMENT,
`matricule` VARCHAR(100) NOT NULL,
`type` VARCHAR(100) NOT NULL,
`motif` VARCHAR(5000) NOT NULL,
`montant` INT(11) NOT NULL,
`session` VARCHAR(11) NOT NULL,
`statut` VARCHAR(11) NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
";
                command = new MySqlCommand(re, connection);
                command.Prepare();
                command.ExecuteNonQuery();
            }
        public Index()
        {
            InitializeComponent();
            // Configuration du serveur
            connexionDB();
            // Configuration de la licence
            FileLicence();
            Connec();
            // creation des differentes tables s'ils n'existe pas
            Tables();
            School ecole = new School(label13);
            ecole.nomschool();
        }
      

        // Methode de connexion à la base de données
        public void Connec()
        {
            try
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
            }
            catch (MySqlException e)
            {
                string messag = "Echec de connection au serveur. " + e.Message;
                string titre = "Erreur Serveur";
                // Programation des bouton de la boite de message
               DialogResult boit = MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                if(boit == DialogResult.OK)
                {
                   // Close();
                }
            }

        }

        private void envoyerUnDocumentAuxArchivesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void accederAuxArchivesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void créerUnÉtablissementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NEW_etablissements formschool = new NEW_etablissements();
            formschool.ShowDialog();
        }

        private void créerUnUtilisateurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NEW_USERS formuser = new NEW_USERS();
            formuser.Show();
        }

        private void monCompteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connexion connect = new Connexion(this);
            connect.ShowDialog();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
           
        }

        private void fichierToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void versementParClasseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            NEW_FONCTION fonction = new NEW_FONCTION();
            fonction.ShowDialog();
        }

        private void créerUneSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Addsession session = new Addsession();
            session.ShowDialog();
        }

        private void créerUneSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New_SECTION section = new New_SECTION();
            section.ShowDialog();
        }

        private void créerUneClasseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NEW_CLASSE classe = new NEW_CLASSE();
            classe.ShowDialog();
        }

        private void enregistrerUnÉlèveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NEW_ELEVE eleve = new NEW_ELEVE();
            eleve.ShowDialog();
        }

        private void inscriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NEW_INSCRIPTION inscript = new NEW_INSCRIPTION();
            inscript.ShowDialog();
        }

        private void scolaritéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NEW_SCOLARITE scol = new NEW_SCOLARITE();
            scol.ShowDialog();
        }

        private void listeDesÉlèvesInscriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Activation des menu
            if(textBox1.Text == "Nom de l'employer" && label14.Text == "Poste")
            {

            }else
            {
                paramètreToolStripMenuItem.Enabled = true;
                traitementToolStripMenuItem.Enabled = true;
                rapportsToolStripMenuItem.Enabled = true;
            }
        }

        private void panelDegrader2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelDegrader2, 8, e.Graphics, borderColor, borderSize);
        }

        private void configurerLeServeurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            configServer server = new configServer();
            server.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            LicenceGestion licence = new LicenceGestion();
            licence.ShowDialog();
        }

        private void parSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportListEleveInscript inscriptrap = new RapportListEleveInscript();
            inscriptrap.ShowDialog();
        }

        private void parSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportinscriptSection inscriptsection = new RapportinscriptSection();
            inscriptsection.ShowDialog();
        }

        private void parClasseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportListClass listclass = new RapportListClass();
            listclass.ShowDialog();
        }

        private void renvoiPourInscriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportFicheRenvoi renvoi = new RapportFicheRenvoi();
            renvoi.ShowDialog();
        }

        private void renvoiPourScolaritéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportRenvoiScolarite renvoi = new RapportRenvoiScolarite();
            renvoi.ShowDialog();
        }

        private void fraisInscriptionParClasseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportVersementParClasseInscription versement = new RapportVersementParClasseInscription();
            versement.ShowDialog();
        }

        private void fraisScolaritéParClasseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportVersementParClaaseScolarite verse = new RapportVersementParClaaseScolarite();
            verse.ShowDialog();
        }

        private void listingDeCaisseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportListingCaisse listing = new RapportListingCaisse();
            listing.ShowDialog();
        }

        private void listeDesSectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportListSection section = new RapportListSection();
            section.ShowDialog();
        }

        private void listeDesClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportListClasse classe = new RapportListClasse();
            classe.ShowDialog();
        }

        private void listeDesÉlèvesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportListEleve eleve = new RapportListEleve();
            eleve.ShowDialog();
        }

        private void dossierDesÉlèvesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoixElevePrDossier eleve = new ChoixElevePrDossier();
            eleve.ShowDialog();
        }

        private void Index_Load(object sender, EventArgs e)
        {

        }

        private void carteScolaireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportCarteScolaire newcarte = new RapportCarteScolaire();
            newcarte.ShowDialog();
        }

        private void fraisScolaireEtInscriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RapportScolariteETInscriptionParClasse scol = new RapportScolariteETInscriptionParClasse();
            scol.ShowDialog();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            NEW_BOURSEPRIME bourse = new NEW_BOURSEPRIME();
            bourse.ShowDialog();
        }

        private void boursesEtPrimesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void toutesLesBoursesEtPrimesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string toute = "toutes";
            Rapport_BourseETPrimes rap = new Rapport_BourseETPrimes(toute);
            rap.ShowDialog();
        }

        private void boursesEtPrimesEncourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string toute = "encour";
            Rapport_BourseETPrimes rap = new Rapport_BourseETPrimes(toute);
            rap.ShowDialog();
        }

        private void boursesEtPrimesRéglésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string toute = "regle";
            Rapport_BourseETPrimes rap = new Rapport_BourseETPrimes(toute);
            rap.ShowDialog();
        }
    }





    
}
