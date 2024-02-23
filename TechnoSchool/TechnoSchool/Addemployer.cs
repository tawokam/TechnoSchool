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
    public partial class Addemployer : Form
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
        private void listsection(ComboBox listfonction)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string requete = "Select nom_fonction from fonction order by nom_fonction asc";
            command = new MySqlCommand(requete, connection);
            command.Prepare();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                listfonction.Items.Add(reader["nom_fonction"]);
            }
            reader.Close();
        }
        // creation de la sous class pour gerer l'insertion des etablissement
        public class InsertEmployer
        {


            private string nomEmployer, mailEmployer, localisationEmployer, sexeEmployer, diplomeEmployer,specialite,mdpEmployer;
            private DateTime datenaissEmployer;
            private long numeroUrgenceEmployer, phone1Employer, phone2Employer,fonctionEmployer;
            private byte[] cvEmployer, photoEmployer;
            public int siDonneesValide = 0;
            private string NomEmployer { get; set; }
            private string MailEmployer { get; set; }
            private string LocalisationEmployer { get; set; }
            private string SexeEmployer { get; set; }
            private byte[] PhotoEmployer { get; set; }
            private string DiplomeEmployer { get; set; }
            private string Specialite { get; set; }
            private string MdpEmployer { get; set; }
            private DateTime DatenaissEmployer { get; set; }
            private long NumeroUrgenceEmployer { get; set; }
            private long Phone1Employer { get; set; }
            private long Phone2Employer { get; set; }
            private long FonctionEmployer { get; set; }
            // Constructeur
            public InsertEmployer(string nom, string localisation, string mail, string sexe, byte[] photo, string diplome, string specialite, string mdp, DateTime datenaiss, long numurgence, long phone1, long phone2, long fonction)
            {
                this.NomEmployer = nom; this.MailEmployer = mail; this.LocalisationEmployer = localisation; this.SexeEmployer = sexe;
                this.PhotoEmployer = photo; this.DiplomeEmployer = diplome; this.Specialite = specialite; this.MdpEmployer = mdp;
                this.DatenaissEmployer = datenaiss; this.NumeroUrgenceEmployer = numurgence;
                this.Phone1Employer = phone1; this.Phone2Employer = phone2; this.FonctionEmployer = fonction;

                insertdb();
            }
            public InsertEmployer()
            {

            }
            private void insertdb()
            {
                int nbreetabli = 0;
                // verifions si un établissement ayant ce nom a déja été crée
                string req = "Select telephone1_employer,mdpemployer,count(mdpemployer) as nbreemployer from employer where telephone1_employer=@phone1 OR mdpemployer=@mdp";
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@phone1", Phone1Employer);
                para.Add("@mdp", MdpEmployer);
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
                if (nbreetabli < 1)
                {
                    string requete = "INSERT INTO employer(nom_employer,telephone1_employer,telephone2_employer,sexe_employer,adresseMail_employer,quartier_employer,id_fonction,datenaiss_employer,grandiplome,specialitediplome,cv_employer,photo_employer,numerourgence,mdpemployer,date_creation,date_modification) values (@nom,@phone1,@phone2,@sexe,@mail,@localisation,@idfonction,@datenaiss,@diplome,@specialite,'',@photo,@numurgence,@mdp,@datecre,@datemod)";
                    string datec = DateTime.Now.ToString("yyyy-MM-dd");
                    string datem = DateTime.Now.ToString("yyyy-MM-dd");
                    Dictionary<string, object> parametre = new Dictionary<string, object>();
                    parametre.Add("@nom", NomEmployer);
                    parametre.Add("@mail", MailEmployer);
                    parametre.Add("@localisation", LocalisationEmployer);
                    parametre.Add("@phone1", Phone1Employer);
                    parametre.Add("@phone2", Phone2Employer);
                    parametre.Add("@sexe", SexeEmployer);
                    parametre.Add("@idfonction", FonctionEmployer);
                    parametre.Add("@datenaiss", DatenaissEmployer);
                    parametre.Add("@diplome", DiplomeEmployer);
                    parametre.Add("@specialite", Specialite);
                    parametre.Add("@photo", PhotoEmployer);
                    parametre.Add("@numurgence", NumeroUrgenceEmployer);
                    parametre.Add("@mdp", MdpEmployer);
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
                        string messag = "L'employé a été enregistrer avec succès. ";
                        string titre = "Insertion ";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        siDonneesValide = 1;
                    }
                    else
                    {
                        string messag = "Nous nous sommes heurtés à un problème lors de l'enregistrement de l'employer. ";
                        string titre = "Insertion";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        siDonneesValide = 0;
                    }
                }else
                {
                    string messag = "Il existe déjà un employer ayant ce numéro de téléphone et / ou ce mot de passe.";
                    string titre = "Duplication";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    siDonneesValide = 0;
                }
                reader2.Close();
               

            }
        }
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
        public Addemployer()
        {
            InitializeComponent();
            connexionDB();
            listsection(listfonction);
        }

        private void fermFormAddSchool_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Addemployer_Paint(object sender, PaintEventArgs e)
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

        private void panelDegrader1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelDegrader1, 8, e.Graphics, borderColor, borderSize);
        }

        private void listfonction_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listfonction_SelectedValueChanged(object sender, EventArgs e)
        {

            string requete = "Select id_fonction,nom_fonction from fonction where nom_fonction = @nom ";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@nom", listfonction.Text);
            command = new MySqlCommand(requete, connection);
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));

            }
            command.Prepare();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                label16.Text = (reader["id_fonction"]).ToString();
            }
            reader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialogPhotoEmployer.Filter = "Extension autorisee|*.jpg;*.jpeg;*.png;*.bmp;*.ico";
            if (openFileDialogPhotoEmployer.ShowDialog() == DialogResult.OK)
            {
                imageSchool.Image = Image.FromFile(openFileDialogPhotoEmployer.FileName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialogCvEmployer.Filter =  "Extension autorisee|*.jpg;*.jpeg;*.png;*.docx;*.pdf";
            if (openFileDialogCvEmployer.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text =openFileDialogCvEmployer.FileName;
            }
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

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel5, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel6, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel7, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel8, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel9, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel10, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel11, 5, e.Graphics, Color.Blue, 1);
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

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button2, 7, e.Graphics, Color.Blue, 1);
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button3, 7, e.Graphics, Color.Blue, 1);
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
        // methode de conversion de fichier(pdf et word) en tableau de byte pour inserer dans la base de données
        public byte[] convertFileToBytes(string chemin)
        {
            byte[] fichier = null;
            FileStream fs = new FileStream(chemin, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(chemin).Length;
            fichier = br.ReadBytes((int) numBytes);
            fs.Close();
            return fichier;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            /*
            // Importation de la photo de profil de l'employer
            if (openFileDialogPhotoEmployer.FileName != "")
            {
                string path = Path.Combine(Environment.CurrentDirectory, "img");
                string fileName = Path.GetFileName(openFileDialogPhotoEmployer.FileName);
                File.Copy(openFileDialogPhotoEmployer.FileName, Path.Combine(path, fileName));
            }
            // Importation de la photo de profil de l'employer
            if (openFileDialogCvEmployer.FileName != "")
            {
                string path = Path.Combine(Environment.CurrentDirectory, "img");
                string fileName = Path.GetFileName(openFileDialogCvEmployer.FileName);
                File.Copy(openFileDialogCvEmployer.FileName, Path.Combine(path, fileName));
            }
            */
            // Récupération et traitement des informations saisi par l'utilisateur pour la création d'un établissement scolaire
            if (nomSchool.Text == "")
            {
                string messag = "Veuillez entrez le nom de l'employé. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (phone1.Text == "")
            {
                string messag = "Veuillez entrez le premier numéro de l'employé. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (listfonction.Text == "")
            {
                string messag = "Veuillez séléctionnez la fonction de l'employé. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (mdpProvisaoire.Text == "")
            {
                string messag = "Veuillez entrer le mot de passe provisoire de l'employé ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
            else
            {
                try
                {
                    long telephone2, phoneurgence;
                    if (phone2.Text == "") { telephone2 = 0; } else { telephone2 = long.Parse(phone2.Text); }
                    if (numurgence.Text == "") { phoneurgence = 0; } else { phoneurgence = long.Parse(numurgence.Text); }

                    string nom = nomSchool.Text; string email = mail.Text; string localisations = localisation.Text;
                    long telephone1 = long.Parse(phone1.Text);
                    string sexe = comboBox3.Text; DateTime datenaiss = dateTimePicker1.Value;
                    long fonction = long.Parse(label16.Text); string diplome = comboBox2.Text;

                    byte[] photo = convertImageToBytes(imageSchool.Image);
                   // byte[] cv = convertFileToBytes(openFileDialogCvEmployer.FileName);
                    string specialite = textBox3.Text; string mdp = mdpProvisaoire.Text;
                    InsertEmployer newemployer = new InsertEmployer(nom, localisations, email, sexe, photo, diplome, specialite, mdp, datenaiss, phoneurgence, telephone1, telephone2, fonction);
                    int valuereturn = newemployer.siDonneesValide;
                    // vider les champs après insertion valider
                    if (valuereturn == 1)
                    {
                        nomSchool.Text = ""; mail.Text = ""; localisation.Text = ""; comboBox3.Text = "";
                        phone1.Text = 0.ToString(); phone2.Text = 0.ToString(); numurgence.Text = 0.ToString();
                        listfonction.Text = ""; comboBox2.Text = ""; textBox3.Text = "";
                        mdpProvisaoire.Text = "";
                    }
                }
                catch(Exception ex)
                {
                    string messag = "Nous nous sommes heurté à un problème lors de l'enregistrement de l'employé. ";
                    string titre = "Erreur d'insertion";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
            }
        }

        private void phone1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void phone2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void numurgence_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            nomSchool.Text = ""; mail.Text = ""; localisation.Text = ""; phone1.Text = "0";
            phone2.Text = "0"; numurgence.Text = ""; textBox3.Text = ""; mdpProvisaoire.Text = "";
        }
    }
}
