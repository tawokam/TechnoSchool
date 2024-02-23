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
    public partial class EditSchool : Form
    {
        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataReader reader2;
        public static string connectionstring = "";
        // Donnees de connexion
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
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch(Exception ex)
            {
                string messag = "Nous nous sommes heurtés à un problème lors de la lecture du fichier de configuration du logiciel TechnoSchool. ";
                string titre = "Configuration";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            private TextBox nom, email, localisation, bp, niu, nrc;
            private Label phone1, phone2, om, momo, paieOui, paieNon, addDate, editDate;
            private PictureBox logo;

            private TextBox Nom { set; get; }
            private TextBox Email { set; get; }
            private TextBox Localisation { set; get; }
            private TextBox Bp { set; get; }
            private TextBox Niu { set; get; }
            private TextBox Nrc { set; get; }
            private TextBox Phone1 { set; get; }
            private TextBox Phone2 { set; get; }
            private TextBox Om { set; get; }
            private TextBox Momo { set; get; }
            private RadioButton PaieOui { set; get; }
            private RadioButton PaieNon { set; get; }
            private PictureBox Logo { set; get; }
            // Constructeur
            public Modification(TextBox nom, TextBox email, TextBox localisation, TextBox bp, TextBox niu, TextBox nrc, TextBox phone1, TextBox phone2, TextBox om, TextBox momo, RadioButton paieoui, RadioButton paienon, PictureBox logo)
            {
                this.Nom = nom; this.Email = email; this.Localisation = localisation; this.Bp = bp; this.Niu = niu;
                this.Nrc = nrc; this.Phone1 = phone1; this.Phone2 = phone2; this.Om = om; this.Momo = momo; this.PaieOui = paieoui;
                this.PaieNon = paienon; this.Logo = logo;
            }

            // Récuperation des informations dans la base de données
            public void modifinfo(int idline, Label stockline)
            {
                try
                {
                    stockline.Text = idline.ToString();
                    stockline.Visible = false;
                    //var connectionstring = "server=localhost;user id=root;database=technosoft; SslMode=none";
                    var connection = new MySqlConnection(connectionstring);
                    connection.Open();
                    string requete = "SELECT * FROM etablissement where id_etablissement = @id";
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
                        Email.Text = reader.GetValue(2).ToString();
                        Localisation.Text = reader.GetValue(3).ToString();
                        Bp.Text = reader.GetValue(4).ToString();
                        Phone1.Text = reader.GetValue(5).ToString();
                        Phone2.Text = reader.GetValue(6).ToString();
                        Niu.Text = reader.GetValue(7).ToString();
                        Nrc.Text = reader.GetValue(8).ToString();
                        if (reader.GetValue(10).ToString() == "Oui" || reader.GetValue(10).ToString() == "oui")
                        {
                            PaieOui.Checked = true;
                            PaieNon.Checked = false;
                            Om.Visible = true; Momo.Visible = true;
                        }
                        else if (reader.GetValue(10).ToString() == "Non" || reader.GetValue(10).ToString() == "non")
                        {
                            PaieOui.Checked = false;
                            PaieNon.Checked = true;
                            Om.Visible = false; Momo.Visible = false;
                        }

                        Om.Text = reader.GetValue(11).ToString();
                        Momo.Text = reader.GetValue(12).ToString();

                        // Affichage de mon image converti par la methode convertByteArrayToImage
                        Logo.Image = CovertByteArrayToImage((byte[])(reader.GetValue(9)));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de l'ouverture du formulaire de modification de l'établissement scolaire. ";
                    string titre = "Modification ";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                catch(Exception ex)
                {
                    return null;
                    string messag = "Nous nous sommes heurtés à un problème lors de l'affichage du logo de l'établissement scolaire. ";
                    string titre = "Modification ";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }
        // class Insertion des modifications apporté aux données
        public class InsertModifEtablissement
        {


            private string nomEtabli, mailEtabli, localisationEtabli, bpEtabli, niuEtabli, nrcEtabli, paieEnLigne;
            private long phone1Etabli, phone2Etabli, omEtabli, momoEtabli, labelStockId;
            private byte[] logoEtabli;
            private string NomEtabli { get; set; }
            private string MailEtabli { get; set; }
            private string LocalisationEtabli { get; set; }
            private string BpEtabli { get; set; }
            private string nIUEtabli { get; set; }
            private string nRCEtabli { get; set; }
            private byte[]  LogoEtabli { get; set; }
            private string PaieEnLigne { get; set; }
            private long Phone1Etabli { get; set; }
            private long Phone2Etabli { get; set; }
            private long oMEtabli { get; set; }
            private long mOMOEtabli { get; set; }
            private long LabelStockId { get; set; }
            // Constructeur
            public InsertModifEtablissement(string nom, string localisation, string mail, string bp, long phone1, long phone2, string NIU, string NRC, long OM, long MOMO, byte[] logo, long stockid)
            {
                this.NomEtabli = nom; this.MailEtabli = mail; this.LocalisationEtabli = localisation; this.BpEtabli = bp;
                this.nIUEtabli = NIU; this.nRCEtabli = NRC; this.LogoEtabli = logo; this.Phone1Etabli = phone1;
                this.Phone2Etabli = phone2; this.oMEtabli = OM; this.mOMOEtabli = MOMO;
                this.LabelStockId = stockid;
                if (oMEtabli == 0 && mOMOEtabli == 0)
                {
                    this.PaieEnLigne = "Non";
                }
                else
                {
                    this.PaieEnLigne = "Oui";
                }

                insertdb();
            }
            private void insertdb()
            {
                try
                {
                    int nbreetabli = 0;
                    // verifions si un établissement ayant ce nom a déja été crée
                    string req = "Select id_etablissement,nom_etabli,count(nom_etabli) as nbreschool from etablissement where id_etablissement <> @idligne AND (nom_etabli=@nometabli OR mail=@mail OR phone1=@phone1)";
                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("@nometabli", NomEtabli);
                    para.Add("@mail", MailEtabli);
                    para.Add("@phone1", Phone1Etabli);
                    para.Add("@idligne", LabelStockId);
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
                        string requete = "UPDATE etablissement SET nom_etabli = @nom, mail = @mail, localisation = @localisation, bp = @bp, phone1 = @phone1, phone2 = @phone2, NIU = @niu, NRC = @nrc, logo = @logo, paieEnLigne = @paie, Numom = @numom, nummm = @nummm,dernier_modification = @modification WHERE id_etablissement= @identLigne";
                        string datem = DateTime.Now.ToString("yyyy-MM-dd");
                        Dictionary<string, object> parametre = new Dictionary<string, object>();
                        parametre.Add("@nom", NomEtabli);
                        parametre.Add("@mail", MailEtabli);
                        parametre.Add("@localisation", LocalisationEtabli);
                        parametre.Add("@bp", BpEtabli);
                        parametre.Add("@phone1", Phone1Etabli);
                        parametre.Add("@phone2", Phone2Etabli);
                        parametre.Add("@niu", nIUEtabli);
                        parametre.Add("@nrc", nRCEtabli);
                        parametre.Add("@logo", LogoEtabli);
                        parametre.Add("@paie", PaieEnLigne);
                        parametre.Add("@numom", oMEtabli);
                        parametre.Add("@nummm", mOMOEtabli);
                        parametre.Add("@modification", datem);
                        parametre.Add("@identLigne", LabelStockId);
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
                            string messag = "Votre établissement scolaire a été modifié avec succès. ";
                            string titre = "Modification ";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            string messag = "Nous nous sommes heurtés à un problème lors de la modification de votre établissement scolaire. ";
                            string titre = "Modification ";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        string messag = "Il existe déjà une école ayant ce nom, cette adresse éléctronique ou le numéro de téléphone 1.";
                        string titre = "Duplication";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch(Exception ex)
                {
                    string messag = "Nous nous sommes heurtés à un problème lors de la modification de votre établissement scolaire. ";
                    string titre = "Modification ";
                    // Programation des bouton de la boite de message
                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public EditSchool(int idline)
        {
            InitializeComponent();
            connexionDB();
            Modification modifinfos = new Modification(nomSchool, mail, localisation, BP, niu, nrc, phone1, phone2, MOMOnewetablissement, OMnewetablissement, radioButtonOui, radioButtonNon, imageSchool);
            modifinfos.modifinfo(idline, labelIdLigne);
        }
        // methode de conversion d'image en tableau de byte pour inserer dans la base de données
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
            catch(Exception ex)
            {
                return null;
                string messag = "Nous nous sommes heurtés à un problème lors de la conversion du logo. ";
                string titre = "Modification ";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void radioButtonOui_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void radioButtonNon_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonNon_Click(object sender, EventArgs e)
        {
            radioButtonOui.Checked = false;
            this.radioButtonNon.Checked = true;
            MOMOnewetablissement.Visible = false; MOMOnewetablissement.Text = "";
            nomMM.Visible = false;
            OMnewetablissement.Visible = false; OMnewetablissement.Text = "";
            nomOM.Visible = false;
        }

        private void radioButtonOui_Click(object sender, EventArgs e)
        {
            this.radioButtonOui.Checked = true; radioButtonNon.Checked = false;
            MOMOnewetablissement.Visible = true;
            nomMM.Visible = true;
            OMnewetablissement.Visible = true;
            nomOM.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialogaModifLogoSchool.Filter = "Extension autorisee|*.jpg;*.jpeg;*.png;*.bmp;*.ico";
            if (openFileDialogaModifLogoSchool.ShowDialog() == DialogResult.OK)
            {
                imageSchool.Image = Image.FromFile(openFileDialogaModifLogoSchool.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Récupération et traitement des informations saisi par l'utilisateur pour la création d'un établissement scolaire
            if (nomSchool.Text == "")
            {
                string messag = "Veuillez entrez le nom de l'établissement scolaire. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (mail.Text == "")
            {
                string messag = "Veuillez entrez l'adresse mail de l'établissement scolaire. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (localisation.Text == "")
            {
                string messag = "Veuillez renseigner le lieu où se trouve l'établissement scolaire. ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (phone1.Text == "")
            {
                string messag = "Veuillez entrer au moins un numéro de téléphone. Le numéro de téléphone 1 est obligatoire ";
                string titre = "Données manquantes";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                long telephone2, momo, om;
                if (phone2.Text == "") { telephone2 = 0; } else { telephone2 = long.Parse(phone2.Text); }
                if (MOMOnewetablissement.Text == "") { momo = 0; } else { momo = long.Parse(MOMOnewetablissement.Text); }
                if (OMnewetablissement.Text == "") { om = 0; } else { om = long.Parse(OMnewetablissement.Text); }

                string nom = nomSchool.Text; string email = mail.Text; string localisations = localisation.Text;
                string boitep = BP.Text; long telephone1 = long.Parse(phone1.Text);
                string nius = niu.Text; string nrcs = nrc.Text; byte[] logo = convertImageToBytes(imageSchool.Image);
                long idLigne = int.Parse(labelIdLigne.Text);
                InsertModifEtablissement newschool = new InsertModifEtablissement(nom, localisations, email, boitep, telephone1, telephone2, nius, nrcs, om, momo, logo, idLigne);
            }
        }

        private void phone1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Code pour empeché la saisi des lettres dans le champ
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void phone2_TextChanged(object sender, EventArgs e)
        {

        }

        private void MOMOnewetablissement_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Code pour empeché la saisi des lettres dans le champ
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void phone2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Code pour empeché la saisi des lettres dans le champ
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void OMnewetablissement_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Code pour empeché la saisi des lettres dans le champ
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
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

        private void panelModePaiement_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(panelModePaiement, 10, e.Graphics, Color.Blue, 1);
        }

        private void EditSchool_Paint(object sender, PaintEventArgs e)
        {
            designBordure radiusform = new designBordure();
            radiusform.FormRegionAndBorder(this, 10, e.Graphics, Color.Blue, 1);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirbouton = new designBordure();
            arrondirbouton.FormRegionAndBorderbouton(button2, 7, e.Graphics, Color.Blue, 1);
        }

        private void infoDatagridviewNewEtablissement_Paint(object sender, PaintEventArgs e)
        {
            designBordure arrondirpanel = new designBordure();
            arrondirpanel.FormRegionAndBorderpanel(infoDatagridviewNewEtablissement, 8, e.Graphics, Color.Blue, 1);
        }
    }


  
  
}

