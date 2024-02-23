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
  
    public partial class EditEmployer : Form
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
        // Récupération des informations dépuis la BD grace à l'id
        public class Modification
        {
            private TextBox nomEmployer, mailEmployer, localisationEmployer, phone1Employer, phone2Employer, numeroUrgenceEmployer, specialite;
            private DateTimePicker datenaissEmployer;
            private PictureBox photoEmployer;
            private Label idligneform;
            private TextBox NomEmployer { get; set; }
            private TextBox MailEmployer { get; set; }
            private TextBox LocalisationEmployer { get; set; }
            private ComboBox SexeEmployer { get; set; }
            private PictureBox PhotoEmployer { get; set; }
            private ComboBox DiplomeEmployer { get; set; }
            private TextBox Specialite { get; set; }
            private DateTimePicker DatenaissEmployer { get; set; }
            private TextBox NumeroUrgenceEmployer { get; set; }
            private TextBox Phone1Employer { get; set; }
            private TextBox Phone2Employer { get; set; }
            private ComboBox FonctionEmployer { get; set; }
            private Label Idligneform { get; set; }
            public static string idfonction;
            // Constructeur
            public Modification(TextBox nom, TextBox localisation, TextBox mail, ComboBox sexe, PictureBox photo, ComboBox diplome, TextBox specialite, DateTimePicker datenaiss, TextBox numurgence, TextBox phone1, TextBox phone2, ComboBox fonction, Label idligneform)
            {
                this.NomEmployer = nom; this.MailEmployer = mail; this.LocalisationEmployer = localisation; this.SexeEmployer = sexe;
                this.PhotoEmployer = photo; this.DiplomeEmployer = diplome; this.Specialite = specialite;
                this.DatenaissEmployer = datenaiss; this.NumeroUrgenceEmployer = numurgence;
                this.Phone1Employer = phone1; this.Phone2Employer = phone2; this.FonctionEmployer = fonction;
                this.Idligneform = idligneform;
            }

            // Récuperation des informations dans la base de données
            public void modifinfo(int idline, Label stockline)
            {
                stockline.Text = idline.ToString();
                stockline.Visible = false;
                
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "Select employer.id_employer,employer.nom_employer, employer.adresseMail_employer, employer.telephone1_employer, employer.telephone2_employer, employer.sexe_employer, employer.quartier_employer, employer.datenaiss_employer, employer.grandiplome, employer.specialitediplome, employer.cv_employer, employer.photo_employer, employer.numerourgence,employer.id_fonction,employer.specialitediplome,fonction.id_fonction,fonction.nom_fonction from employer inner join fonction on employer.id_fonction=fonction.id_fonction where employer.id_employer = @id";
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
                    NomEmployer.Text = reader.GetValue(1).ToString();
                    MailEmployer.Text = reader.GetValue(2).ToString();
                    LocalisationEmployer.Text = reader.GetValue(6).ToString();
                    if (reader.GetValue(5).ToString() == "Maxulin")
                    {
                        SexeEmployer.SelectedIndex = 0;
                    }
                    else if(reader.GetValue(5).ToString() == "Feminin")
                    {
                        SexeEmployer.SelectedIndex = 1;
                    }
                    //-----------
                    if (reader.GetValue(8).ToString() == "BEPC / CAP")
                    {
                        DiplomeEmployer.SelectedIndex = 0;
                    }
                    else if (reader.GetValue(8).ToString() == "PROBATOIRE")
                    {
                        DiplomeEmployer.SelectedIndex = 1;
                    }
                    else if (reader.GetValue(8).ToString() == "BACCALEAUREAT")
                    {
                        DiplomeEmployer.SelectedIndex = 2;
                    }
                    else if (reader.GetValue(8).ToString() == "BTS / DUT")
                    {
                        DiplomeEmployer.SelectedIndex = 3;
                    }
                    else if (reader.GetValue(8).ToString() == "LICENCE")
                    {
                        DiplomeEmployer.SelectedIndex = 4;
                    }
                    else if (reader.GetValue(8).ToString() == "MASTER 2")
                    {
                        DiplomeEmployer.SelectedIndex = 5;
                    }
                    else if (reader.GetValue(8).ToString() == "DOCTORAT")
                    {
                        DiplomeEmployer.SelectedIndex = 6;
                    }
                    DiplomeEmployer.Items.Add(reader.GetValue(15).ToString());
                    Specialite.Text = reader.GetValue(9).ToString();

                    DatenaissEmployer.Text = reader.GetValue(7).ToString();
                    NumeroUrgenceEmployer.Text = reader.GetValue(12).ToString();
                    Phone1Employer.Text = reader.GetValue(3).ToString();
                    Phone2Employer.Text = reader.GetValue(4).ToString();
                    Idligneform.Text = idline.ToString();

                    idfonction = reader.GetValue(13).ToString();
                    // Affichage de mon image converti par la methode convertByteArrayToImage
                    PhotoEmployer.Image = CovertByteArrayToImage((byte[])(reader.GetValue(11)));

                }
                reader.Close();
            }
            //Methode de Convertion du byte Array provenant de la base de données en image à affiché dans ma picturebox
            private Image CovertByteArrayToImage(byte[] data)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        return Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    return null;
                    string messag = "Nous nous sommes heurtés à un problème lors de l'affichage du logo de l'établissement scolaire. ";
                    string titre = "Modification ";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            // ---------
            // Affichage de la liste des fonction avec séléction automatique de la fonction de l'employer
            public void listfonction()
            {
                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "Select id_fonction, nom_fonction from fonction order by nom_fonction asc";           
                command = new MySqlCommand(requete, connection);
                command.Prepare();
                reader2 = command.ExecuteReader();
                while (reader2.Read())
                {
                    if(reader2.GetValue(0).ToString() == idfonction)
                    {
                        FonctionEmployer.Items.Add(reader2.GetValue(1).ToString());
                        FonctionEmployer.SelectedItem = (reader2.GetValue(1).ToString());
                    }else
                    {
                        FonctionEmployer.Items.Add(reader2.GetValue(1).ToString());
                    }
                }
                reader2.Close();
            }
        }

        // --------------
        // class Insertion des modifications apporté aux données
        public class InsertModifEmployer
        {


            private string nomEmployer, mailEmployer, localisationEmployer, sexeEmployer, diplomeEmployer, specialite;
            private long phone1Employer, phone2Employer, numeroUrgenceEmployer, fonctionEmployer, idligneform;
            private DateTime datenaissEmployer;

            private string NomEmployer { get; set; }
            private string MailEmployer { get; set; }
            private string LocalisationEmployer { get; set; }
            private string SexeEmployer { get; set; }
            private string DiplomeEmployer { get; set; }
            private string Specialite { get; set; }
            private DateTime DatenaissEmployer { get; set; }
            private long NumeroUrgenceEmployer { get; set; }
            private long Phone1Employer { get; set; }
            private long Phone2Employer { get; set; }
            private long FonctionEmployer { get; set; }
            private long Idligneform { get; set; }
            public byte[] PhotoEmployer { get; set; }
            // Constructeur
            public InsertModifEmployer(string nom, string localisation, string mail, string sexe, byte[] photo, string diplome, string specialite, DateTime datenaiss, long numurgence, long phone1, long phone2, long fonction, long idligneform)
            {
                this.NomEmployer = nom; this.MailEmployer = mail; this.LocalisationEmployer = localisation; this.SexeEmployer = sexe;
                this.PhotoEmployer = photo; this.DiplomeEmployer = diplome; this.Specialite = specialite;
                this.DatenaissEmployer = datenaiss; this.NumeroUrgenceEmployer = numurgence;
                this.Phone1Employer = phone1; this.Phone2Employer = phone2; this.FonctionEmployer = fonction;this.Idligneform = idligneform;
                insertdb();
            }
            private void insertdb()
            {
                string requete = "UPDATE employer SET nom_employer = @nom, adresseMail_employer = @mail, quartier_employer = @localisation, telephone1_employer = @phone1, telephone2_employer = @phone2, sexe_employer = @sexe, id_fonction = @idfonction, datenaiss_employer = @datenaiss, grandiplome = @diplome, specialitediplome = @specialite,photo_employer = @photo, numerourgence = @numurgence,date_modification = @datemodif WHERE id_employer = @identLigne";
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
                parametre.Add("@datemodif", datem);
                parametre.Add("@identLigne", Idligneform);
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
                    string messag = "Les informations sur cet employé a été modifié avec succès. ";
                    string titre = "Modification ";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }
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
        public EditEmployer(int idline)
        {
            InitializeComponent();
            connexionDB();
            Modification modifinfos = new Modification(nomSchool, localisation, mail , comboBox3, imageSchool, comboBox2, textBox3, dateTimePicker1, numurgence, phone1, phone2, listfonction, label15);
            modifinfos.modifinfo(idline, label15);
            modifinfos.listfonction();
        }

        private void fermFormAddSchool_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialogphotoprofil.Filter = "Extension autorisee|*.jpg;*.jpeg;*.png;*.bmp;*.ico";
            if (openFileDialogphotoprofil.ShowDialog() == DialogResult.OK)
            {
                imageSchool.Image = Image.FromFile(openFileDialogphotoprofil.FileName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialogcvEmployer.Filter = "Extension autorisee|*.jpg;*.jpeg;*.png;*.docx;*.pdf";
            if (openFileDialogcvEmployer.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = (openFileDialogcvEmployer.FileName);
            }
        }

        private void listfonction_SelectedValueChanged(object sender, EventArgs e)
        {
            var connection = new MySqlConnection(connectionstring);
            connection.Open();
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

        private void EditEmployer_Load(object sender, EventArgs e)
        {
            var connection = new MySqlConnection(connectionstring);
            connection.Open();
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

            // Methode de conversion de l'image en byte pour le stockage dans DB
            byte[] convertImageToBytes(Image Img)
        {
            try
            {
                using (MemoryStream sm = new MemoryStream())
                {
                    Img.Save(sm, System.Drawing.Imaging.ImageFormat.Png);
                    return sm.ToArray();
                }
            }
            catch (Exception ex)
            {
                return null;
                string messag = "Nous nous sommes heurtés à un problème lors de la conversion du logo. ";
                string titre = "Modification ";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {


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
            else
            {
                long telephone2, telephoneurgence;
                if (phone2.Text == "") { telephone2 = 0; } else { telephone2 = long.Parse(phone2.Text); }
                if (numurgence.Text == "") { telephoneurgence = 0; } else { telephoneurgence = long.Parse(numurgence.Text); }

                string nom = nomSchool.Text; string email = mail.Text; string localisations = localisation.Text;
                string sexe = comboBox3.Text; long telephone1 = long.Parse(phone1.Text);
                DateTime datenaiss = dateTimePicker1.Value; long idfonction = long.Parse(label16.Text); string diplome = comboBox2.Text;
                byte[] photo = convertImageToBytes(imageSchool.Image);// string cv = Path.GetFileName(openFileDialogcvEmployer.FileName);
                string specialite = textBox3.Text;long idform = long.Parse(label15.Text);
                InsertModifEmployer newschool = new InsertModifEmployer(nom, localisations, email, sexe, photo, diplome, specialite, datenaiss, telephoneurgence, telephone1, telephone2, idfonction, idform);
            }
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button2, 7, e.Graphics, Color.Blue, 1);
        }

        private void EditEmployer_Paint(object sender, PaintEventArgs e)
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

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button1, 7, e.Graphics, Color.Blue, 1);
        }
    }
}
