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
    public partial class AddEleve : Form
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
            try
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
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }
            catch(FileLoadException ex)
            {
                string messag = "Nou nous somme heurté à un problème lors de la connexion au serveur "+ex;
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

        // creation de la sous class pour gerer l'insertion des élèves
        public class InsertEleve
        {


            public string nom, prenom, matricule, lieu, sexe, nationalite, adresse, maladie, eps, info, nompere, nommere, nomtutteur;
            private DateTime datenaiss;
            private long phonepere, phonemere, phonetutteur;
            private byte[] photoEleve;
            public int siDonneesValide = 0;
            private string Nom { get { return nom; } set { nom = value; } }
            private string Prenom { get; set; }
            private string Matricule { get; set; }
            private string Lieu { get; set; }
            private byte[] PhotoEleve { get; set; }
            private string Sexe { get; set; }
            private string Nationalite { get; set; }
            private string Adresse { get; set; }
            private string Maladie { get; set; }
            private string Eps { get; set; }
            private string Info { get; set; }
            private string Nompere { get; set; }
            private string Nommere { get; set; }
            private string Nomtutteur { get; set; }
            private DateTime Datenaiss { get; set; }
            private long Phonepere { get; set; }
            private long Phonemere { get; set; }
            private long Phonetutteur { get; set; }
            // Constructeur
            public InsertEleve(string nom, string prenom, string matricule, string lieu, byte[] photo, string sexe, string nationalite, string adresse, string maladie,string eps,string info,string nompere, string nommere, string nomtutteur, DateTime datenaiss, long phonepere, long phonemere, long phonetutteur)
            {
                this.Nom = nom; this.Prenom = prenom; this.Matricule = matricule; this.Lieu = lieu;
                this.PhotoEleve = photo; this.Sexe = sexe; this.Nationalite = nationalite; this.Adresse = adresse;
                this.Maladie = maladie; this.Eps = eps; this.Info = info;
                this.Nompere = nompere; this.Nommere = nommere; this.Nomtutteur = nomtutteur;
                this.Datenaiss = datenaiss; this.Phonepere = phonepere; this.Phonemere = phonemere;
                this.Phonetutteur = phonetutteur;

                insertdb();
            }
            private void insertdb()
            {
                try
                {
                    int nbreetabli = 0;
                    // verifions si un établissement ayant ce nom a déja été crée
                    string req = "Select id_eleve,matricule,count(id_eleve) as nbreemployer from eleves where matricule=@matricule ";
                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("@matricule", Matricule);
                    var connect = new MySqlConnection(connectionstring);
                    connect.Open();
                    var commande = new MySqlCommand(req, connect);
                    foreach (KeyValuePair<string, object> parametres in para)
                    {
                        commande.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                    }
                    commande.Prepare();

                    reader2 = commande.ExecuteReader();
                    while (reader2.Read())
                    {
                        nbreetabli = int.Parse(reader2.GetValue(2).ToString());

                    }
                    reader2.Close();
                    if (nbreetabli < 1)
                    {
                        string requete = "INSERT INTO eleves(nom_eleve,prenom_eleve,matricule,date_naiss,lieu_naiss,sexe,nationalite,adresse,maladie,ApteEps,autreinfo,photo,nompere,phonepere,nommere,phonemere,nomtutteur,phonetutteur,date_creation,date_modification) values (@nom,@prenom,@matricule,@datenaiss,@lieunaiss,@sexe,@nationalite,@adresse,@maladie,@eps,@info,@photo,@nompere,@phonepere,@nommere,@phonemere,@nomtutteur,@phonetutteur,@datecre,@datemod)";
                        string datec = DateTime.Now.ToString("yyyy-MM-dd");
                        string datem = DateTime.Now.ToString("yyyy-MM-dd");
                        Dictionary<string, object> parametre = new Dictionary<string, object>();

                        parametre.Add("@nom", Nom.ToUpper());
                        parametre.Add("@prenom", Prenom.ToUpper());
                        parametre.Add("@matricule", Matricule.ToUpper());
                        parametre.Add("@datenaiss", Datenaiss);
                        parametre.Add("@lieunaiss", Lieu);
                        parametre.Add("@sexe", Sexe);
                        parametre.Add("@nationalite", Nationalite);
                        parametre.Add("@adresse", Adresse);
                        parametre.Add("@maladie", Maladie);
                        parametre.Add("@eps", Eps);
                        parametre.Add("@info", Info);
                        parametre.Add("@photo", PhotoEleve);
                        parametre.Add("@nompere", Nompere);
                        parametre.Add("@phonepere", Phonepere);
                        parametre.Add("@nommere", Nommere);
                        parametre.Add("@phonemere", Phonemere);
                        parametre.Add("@nomtutteur", Nomtutteur);
                        parametre.Add("@phonetutteur", Phonetutteur);
                        parametre.Add("@datecre", datec);
                        parametre.Add("@datemod", datem);
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
                            string messag = "L'élève a été enregistrer avec succès. ";
                            string titre = "Insertion ";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            siDonneesValide = 1;
                        }
                        else
                        {
                            string messag = "Nous nous sommes heurtés à un problème lors de l'enregistrement de l'élève. ";
                            string titre = "Insertion";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            siDonneesValide = 0;
                        }
                    }
                    else
                    {
                        string messag = "Il existe déjà un élève ayant ce matricule.";
                        string titre = "Duplication";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        siDonneesValide = 0;
                    }
                }
                catch(Exception ex)
                {
                    siDonneesValide = 0;
                    string messag = "Nous nous sommes heurtés à un problème lors de l'enregistrement de l'élève. L'image séléctionnée est trop volumineux "+ex;
                    string titre = "Duplication";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
               
               


            }
        }
        public AddEleve()
        {
            InitializeComponent();
            connexionDB();
            // séléction automatique de OUI apte a l'EPS
            comboBox1.SelectedItem = "OUI";
        }

        private void AddEleve_Paint(object sender, PaintEventArgs e)
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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel3, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel4, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel8, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel6, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel5, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel7, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel9, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel4, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel5, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel16_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel16, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel12, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel13, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel11, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel10, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel18_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel18, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel17_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel17, 5, e.Graphics, Color.Blue, 1);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button2, 8, e.Graphics, borderColor, borderSize);
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button3, 8, e.Graphics, borderColor, borderSize);
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button1, 4, e.Graphics, borderColor, borderSize);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void fermFormAddSchool_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Extension autorisee|*.jpg;*.jpeg;*.png;*.bmp;*.ico";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageSchool.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            nomSchool.Text = ""; prenom.Text = ""; matricule.Text = ""; lieu.Text = "";
            nationalite.Text = ""; adresse.Text = ""; maladie.Text = ""; comboBox1.SelectedItem = "OUI"; infoplus.Text = "";
            imageSchool.Image = null; nompere.Text = ""; nommere.Text = ""; nomtutteur.Text = "";
            phonepere.Text = 0.ToString(); phonemere.Text = 0.ToString(); phonetutteur.Text = 0.ToString();
        }

        private void phonepere_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void phonemere_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void phonetutteur_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox3.Text == "Maxulin")
            {
                nationalite.Text = "Camerounais";
            }else if(comboBox3.Text == "Feminin")
            {
                nationalite.Text = "Camerounaise";
            }
        }
        // methode de conversion d'image en tableau de byte pour inserer dans la base de données
        byte[] convertImageToBytes(Image Img)
        {
                using (MemoryStream sm = new MemoryStream())
                {
                    Img.Save(sm, System.Drawing.Imaging.ImageFormat.Png);
                    return sm.ToArray();
                } 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Récupération et traitement des informations saisi par l'utilisateur pour la création d'un établissement scolaire
            if (nomSchool.Text == "")
            {
                string messag = "Veuillez entrez le nom de l'élève. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else if(matricule.Text == "")
            {
                string messag = "Veuillez entrez le matricule. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else if(comboBox3.Text == "")
            {
                string messag = "Veuillez séléctionné le sexe de l'élève. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                long phonep, phonem, phonet;
                if (phonepere.Text == "")
                {
                    phonep = 0;
                }else { phonep = long.Parse(phonepere.Text); }
                if(phonemere.Text == "")
                {
                    phonem = 0;
                } else { phonem = long.Parse(phonemere.Text); }
                if (phonetutteur.Text == "")
                {
                    phonet = 0;
                } else { phonet = long.Parse(phonetutteur.Text); }

                string nom = nomSchool.Text; string prenoms = prenom.Text; string matricules = matricule.Text;
                string lieus = lieu.Text; string sexes = comboBox3.Text; string nationalites = nationalite.Text;
                string adresses = adresse.Text; string maladies = maladie.Text; string epss = comboBox1.Text;
                string infos = infoplus.Text; string nomp = nompere.Text; string nomm = nommere.Text;
                string nomt = nomtutteur.Text;
                byte[] photo;
                if (imageSchool.Image == null) { photo = null; }else { photo = convertImageToBytes(imageSchool.Image); }
                DateTime datenaiss = DateTime.Parse(dateTimePicker1.Text);
                InsertEleve eleve = new InsertEleve(nom,prenoms,matricules,lieus,photo,sexes,nationalites,adresses,maladies,epss,infos,nomp,nomm,nomt, datenaiss,phonep,phonem,phonet);

                if( eleve.siDonneesValide == 1)
                {
                    nomSchool.Text = ""; prenom.Text = ""; matricule.Text = ""; lieu.Text = ""; comboBox3.Text = "";
                    nationalite.Text = ""; adresse.Text = ""; maladie.Text = ""; comboBox1.SelectedText = "Oui";
                    infoplus.Text = ""; nompere.Text = ""; nommere.Text = ""; nomtutteur.Text = ""; imageSchool.Image = null;
                    phonepere.Text = "0"; phonemere.Text = "0"; phonetutteur.Text = "0";
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageSchool.Image = null;
        }

        private void button4_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderbouton(button4, 4, e.Graphics, borderColor, borderSize);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string nom = nomSchool.Text;
            string prenoms = prenom.Text;
            if(nom == "")
            {
                string messag = "Le nom de l'élève est obligatoire pour généré le matricule. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string matriculeProvisoire = "";
                // Recuperation des insigne du nom
                string[] tabnom = nom.Split(' ');
                string nomAbreger = "";
                if (tabnom.Length == 1)
                {
                    try
                    {
                        // Si l'élève a un seul nom
                        string lettre1 = tabnom[0].Substring(0, 1);
                        nomAbreger = (lettre1 + "0").ToUpper();
                    }
                    catch(Exception ex)
                    {
                        string messag = "Erreur de saisi sur le nom de l'élève (le nom ne peut commencé par un espace vide et il ne peut avoir deux espace entre les noms) " + ex;
                        string titre = "Erreur";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                }else if(tabnom.Length > 1)
                {
                    try
                    {
                        // Si l'élève a plusieurs nom
                        string lettre1 = tabnom[0].Substring(0, 1);
                        string lettre2 = tabnom[1].Substring(0, 1);
                        nomAbreger = (lettre1 + lettre2).ToUpper();
                    }
                    catch(Exception ex)
                    {
                        string messag = "Erreur de saisi sur le nom de l'élève (le nom ne peut commencé par un espace vide et il ne peut avoir deux espace entre les noms) " + ex;
                        string titre = "Erreur";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                try
                {
                    // Recuperation des insigne du prenom
                    string prenomAbreger = "";
                    string[] tabprenom = prenoms.Split(' ');
                    if (prenoms == "")
                    {
                        // Si l'élève n'a pas de prenom
                        prenomAbreger = "00";
                    }
                    else if (tabprenom.Length == 1)
                    {
                        // Si l'élève a un seul prenom
                        string lettre1 = tabprenom[0].Substring(0, 1);
                        prenomAbreger = (lettre1 + "0").ToUpper();
                    }
                    else if (tabprenom.Length > 1)
                    {
                        // Si l'élève a plus d'un prenom
                        string lettre1 = tabprenom[0].Substring(0, 1);
                        string lettre2 = tabprenom[1].Substring(0, 1);
                        prenomAbreger = (lettre1 + lettre2).ToUpper();
                    }
                    matriculeProvisoire = (nomAbreger + "" + prenomAbreger);
                }
                catch (Exception ex)
                {
                    string messag = "Erreur de saisi sur le prenom de l'élève (le prenom ne peut commencé par un espace vide et il ne peut avoir deux espace entre les prenoms) " + ex;
                    string titre = "Erreur";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // selection du dernier matricule enregistre
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                try
                {
                    string matriculeBD = "";
                    int maxLine = 0;
                    string req = "SELECT max(id_eleve) as nbre FROM eleves ";
                    command = new MySqlCommand(req, connection);
                    command.Prepare();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string nbre = reader.GetValue(0).ToString();
                        if (nbre == "")
                        {
                            
                        }
                        else
                        {
                            maxLine = int.Parse(nbre);
                        }

                    }
                    reader.Close();
                    if(maxLine == 0)
                    {
                        matriculeBD = "vide";
                    }else
                    {
                        string req2 = "SELECT matricule FROM eleves WHERE id_eleve='" + maxLine + "'";
                        command = new MySqlCommand(req2, connection);
                        command.Prepare();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            matriculeBD = reader.GetValue(0).ToString();
                        }
                    }
                    reader.Close();
                    int taille = matriculeBD.Length+2;
                    string nbreMatricule = matriculeBD.Substring(4);
                    string newMatricule = matriculeProvisoire + (int.Parse(nbreMatricule) + 1);
                    matricule.Text = newMatricule;

                }
                catch(Exception ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de la génération du matricule " + ex;
                    string titre = "Erreur";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally { connection.Close(); }
            }
        }

        private void AddEleve_Load(object sender, EventArgs e)
        {

        }
    }
}
