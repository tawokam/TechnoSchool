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
    public partial class EditEleve : Form
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
            private TextBox nom, prenom, matricule, lieunaiss, nationalite, adresse, maladie,infos,nomp,nomm,nomt,phonep,phonem,phonet;
            private ComboBox sexe, eps;
            private DateTimePicker datenaiss;
            private PictureBox photo;
            private Label idligneform;
            private TextBox Nom { get; set; }
            private TextBox Prenom { get; set; }
            private TextBox Matricule { get; set; }
            private TextBox Lieunaiss { get; set; }
            private TextBox Nationalite { get; set; }
            private TextBox Adresse { get; set; }
            private TextBox Maladie { get; set; }
            private TextBox Infos { get; set; }
            private DateTimePicker Datenaiss { get; set; }
            private TextBox Nomp { get; set; }
            private TextBox Nomm { get; set; }
            private TextBox Nomt { get; set; }
            private TextBox Phonep { get; set; }
            private TextBox Phonem { get; set; }
            private TextBox Phonet { get; set; }
            private ComboBox Sexe { get; set; }
            private ComboBox Eps { get; set; }
            private PictureBox Photo { get; set; }
            private Label Idligneform { get; set; }
            public static string idfonction;
            // Constructeur
            public Modification(TextBox nom, TextBox prenom, TextBox matricule, TextBox lieu, TextBox nationalite, TextBox adresse, TextBox maladie, TextBox infos, DateTimePicker datenaiss, TextBox nomp, TextBox nomm, TextBox nomt, TextBox phonep, TextBox phonem, TextBox phonet,ComboBox sexe,ComboBox eps, PictureBox photo, Label idligneform)
            {
                this.Nom = nom; this.Prenom = prenom; this.Matricule = matricule; this.Lieunaiss = lieu;
                this.Nationalite = nationalite; this.Adresse = adresse; this.Maladie = maladie;
                this.Infos = infos; this.Datenaiss = datenaiss; this.Nomp = nomp;
                this.Nomm = nomm; this.Nomt = nomt; this.Phonep = phonep; this.Phonem = phonem; this.Phonet = phonet;
                this.Sexe = sexe; this.Eps = eps; this.Photo = photo;
                this.Idligneform = idligneform;
            }

            // Récuperation des informations dans la base de données
            public void modifinfo(int idline, Label stockline)
            {
                stockline.Text = idline.ToString();
                stockline.Visible = false;

                var connection = new MySqlConnection(connectionstring);
                connection.Open();
                string requete = "Select * from eleves where id_eleve = @id";
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
                    Prenom.Text = reader.GetValue(2).ToString();
                    Matricule.Text = reader.GetValue(3).ToString();
                    if (reader.GetValue(6).ToString() == "Maxulin")
                    {
                        Sexe.SelectedIndex = 0;
                    }
                    else
                    {
                        Sexe.SelectedIndex = 1;
                    }
                    //-----------
                    if (reader.GetValue(10).ToString() == "OUI")
                    {
                        Eps.SelectedIndex = 0;
                    }
                    else
                    {
                        Eps.SelectedIndex = 1;
                    }

                    
                    Lieunaiss.Text = reader.GetValue(5).ToString();
                    Datenaiss.Text = reader.GetValue(4).ToString();
                    Nationalite.Text = reader.GetValue(7).ToString();
                    Adresse.Text = reader.GetValue(8).ToString();
                    Maladie.Text = reader.GetValue(9).ToString();
                    Infos.Text = reader.GetValue(11).ToString();
                    Nomp.Text = reader.GetValue(13).ToString();
                    Nomm.Text = reader.GetValue(15).ToString();
                    Nomt.Text = reader.GetValue(17).ToString();
                    Phonep.Text = reader.GetValue(14).ToString();
                    Phonem.Text = reader.GetValue(16).ToString();
                    Phonet.Text = reader.GetValue(18).ToString();
                    Idligneform.Text = idline.ToString();
                    // Affichage de mon image converti par la methode convertByteArrayToImage
                    if(reader.GetValue(12).ToString() == "")
                    {

                    }else
                    {
                        Photo.Image = CovertByteArrayToImage((byte[])(reader.GetValue(12)));
                    }
                    
   
                }
                reader.Close();
            }
            //Methode de Convertion du byte Array provenant de la base de données en image à affiché dans ma picturebox
            private Image CovertByteArrayToImage(byte[] data)
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    return Image.FromStream(ms);
                }
            }
        }
        // --------------
        // class Insertion des modifications apporté aux données
        public class InsertModifEleve
        {

            private string nom, prenom, matricule, lieunaiss, nationalite, adresse, maladie, infos, nomp, nomm, nomt, sexe, eps;
            private long phonep, phonem, phonet, idligneform;
            private byte[] photo;
            private DateTime datenaiss;
            private string Nom { get; set; }
            private string Prenom { get; set; }
            private string Matricule { get; set; }
            private string Lieunaiss { get; set; }
            private string Nationalite { get; set; }
            private string Adresse { get; set; }
            private string Maladie { get; set; }
            private string Infos { get; set; }
            private DateTime Datenaiss { get; set; }
            private string Nomp { get; set; }
            private string Nomm { get; set; }
            private string Nomt { get; set; }
            private long Phonep { get; set; }
            private long Phonem { get; set; }
            private long Phonet { get; set; }
            private string Sexe { get; set; }
            private string Eps { get; set; }
            private byte[] Photo { get; set; }
            private long Idligneform { get; set; }
            // Constructeur
            public InsertModifEleve(string nom, string prenom, string matricule, string lieu, string nationalite, string adresse, string maladie, string infos, DateTime datenaiss, string nomp, string nomm, string nomt, long phonep, long phonem, long phonet, string sexe, string eps, byte[] photo, long idligneform)
            {
                this.Nom = nom; this.Prenom = prenom; this.Matricule = matricule; this.Lieunaiss = lieu;
                this.Nationalite = nationalite; this.Adresse = adresse; this.Maladie = maladie;
                this.Infos = infos; this.Datenaiss = datenaiss; this.Nomp = nomp;
                this.Nomm = nomm; this.Nomt = nomt; this.Phonep = phonep; this.Phonem = phonem; this.Phonet = phonet;
                this.Sexe = sexe; this.Eps = eps; this.Photo = photo;
                this.Idligneform = idligneform;
                insertdb();
            }
            private void insertdb()
            {
                int nbreetabli = 0;
                // verifions si un établissement ayant ce nom a déja été crée
                string req = "Select id_eleve,nom_eleve,count(nom_eleve) as nbreschool from eleves where id_eleve <> @idligne AND matricule=@matricule";
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@matricule", Matricule);
                para.Add("@idligne", Idligneform);
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
                    string requete = "UPDATE eleves SET nom_eleve = @nom, prenom_eleve = @prenom, matricule = @matricule, lieu_naiss = @lieu, nationalite = @nationalite, adresse = @adresse, maladie = @maladie, autreinfo = @info, date_naiss = @datenaiss, nompere = @nomp, nommere = @nomm,nomtutteur = @nomt, phonepere = @phonep,phonemere = @phonem,phonetutteur = @phonet,sexe = @sexe,ApteEps=@eps,photo = @photo,date_modification = @datemodif WHERE id_eleve = @identLigne";
                    string datem = DateTime.Now.ToString("yyyy-MM-dd");
                    Dictionary<string, object> parametre = new Dictionary<string, object>();
                    parametre.Add("@nom", Nom.ToUpper());
                    parametre.Add("@prenom", Prenom.ToUpper());
                    parametre.Add("@matricule", Matricule.ToUpper());
                    parametre.Add("@lieu", Lieunaiss);
                    parametre.Add("@nationalite", Nationalite);
                    parametre.Add("@adresse", Adresse);
                    parametre.Add("@maladie", Maladie);
                    parametre.Add("@info", Infos);
                    parametre.Add("@datenaiss", Datenaiss);
                    parametre.Add("@nomp", Nomp);
                    parametre.Add("@nomm", Nomm);
                    parametre.Add("@nomt", Nomt);
                    parametre.Add("@phonep", Phonep);
                    parametre.Add("@phonem", Phonem);
                    parametre.Add("@phonet", Phonet);
                    parametre.Add("@sexe", Sexe);
                    parametre.Add("@eps", Eps);
                    parametre.Add("@photo", Photo);
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
                        string messag = "Les informations sur cet élève a été modifié avec succès. ";
                        string titre = "Modification ";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string messag = "Erreur de modification ";
                        string titre = "Modification ";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    string messag = "Il existe déjà un élève ayant ce matricule.";
                    string titre = "Duplication";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
        // Classe contenant les fonction du formulaire et les arrondi de bordure
        public class designBordure
        {
          //  private int borderRadius = 20;
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
            //Methode pour arrondir les textbox
            public void FormRegionAndBordertextbox(TextBox textbox, float radius, Graphics graph, Color borderColor, float bordersize)
            {
                using (GraphicsPath roundPath = GetRoundedPath(textbox.ClientRectangle, radius))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (Matrix transform = new Matrix())
                {
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    textbox.Region = new Region(roundPath);
                    if (borderSize > 1)
                    {
                        Rectangle rect = textbox.ClientRectangle;
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
        public EditEleve(int idligne)
        {
            InitializeComponent();
            connexionDB();
            Modification modifinfos = new Modification(nomSchool, prenom, matricule, lieu, nationalite, adresse, maladie, infoplus, dateTimePickera1, nompere, nommere, nomtutteur, phonepere, phonemere, phonetutteur, comboBox3, comboBox1, imageSchool, label20);
            modifinfos.modifinfo(idligne, label20);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Extension autorisee|*.jpg;*.jpeg;*.png;*.bmp;*.ico";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageSchool.Image = Image.FromFile(openFileDialog1.FileName);
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
            }
            else if (matricule.Text == "")
            {
                string messag = "Veuillez entrez le matricule. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBox3.Text == "")
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
                }
                else { phonep = long.Parse(phonepere.Text); }
                if (phonemere.Text == "")
                {
                    phonem = 0;
                }
                else { phonem = long.Parse(phonemere.Text); }
                if (phonetutteur.Text == "")
                {
                    phonet = 0;
                }
                else { phonet = long.Parse(phonetutteur.Text); }

                string nom = nomSchool.Text; string prenoms = prenom.Text; string matricules = matricule.Text;
                string lieus = lieu.Text; string sexes = comboBox3.Text; string nationalites = nationalite.Text;
                string adresses = adresse.Text; string maladies = maladie.Text; string epss = comboBox1.Text;
                string infos = infoplus.Text; string nomp = nompere.Text; string nomm = nommere.Text;
                long idligneselect = long.Parse(label20.Text);
                string nomt = nomtutteur.Text;
                byte[] photo;
                if (imageSchool.Image == null) { photo = null; } else { photo = convertImageToBytes(imageSchool.Image); }
                DateTime datenaiss = DateTime.Parse(dateTimePickera1.Text);
                InsertModifEleve eleve = new InsertModifEleve(nom, prenoms, matricules, lieus, nationalites, adresses, maladies, infos, datenaiss, nomp, nomm, nomt, phonep, phonem, phonet, sexes, epss, photo, idligneselect);
            }
        }
        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text == "Maxulin")
            {
                nationalite.Text = "Camerounais";
            }
            else if (comboBox3.Text == "Feminin")
            {
                nationalite.Text = "Camerounaise";
            }
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

        private void fermFormAddSchool_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditEleve_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, 10, e.Graphics, Color.Blue, 1);
        }

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 8, e.Graphics, Color.Blue, 1);
        }

        private void panelDegrader1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelDegrader1, 8, e.Graphics, Color.Blue, 1);
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
            arrondirpanel.FormRegionAndBorderpanel(panel14, 5, e.Graphics, Color.Blue, 1);
        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel15, 5, e.Graphics, Color.Blue, 1);
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
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button2, 7, e.Graphics, Color.Blue, 1);
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button1, 7, e.Graphics, Color.Blue, 1);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panel1, 5, e.Graphics, borderColor, borderSize);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageSchool.Image = null;
        }
    }
}
