using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnoSchool
{
    public partial class IMPRESSION_inscription : Form
    {
        // ---------------
        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataReader reader2;
        public static MySqlDataReader reader42;
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
        // methode permettant de convertir un nombre en lettre
            public static string converti(int chiffre)
            {
                int centaine, dizaine, unite, reste, y;
                bool dix = false;
                string lettre = "";
                //strcpy(lettre, "");  
                reste = chiffre / 1;
                for (int i=1000000000; i>=1; i/=1000)
                {
                    y = reste/i;
                    if (y!=0)
                    {
                        centaine = y/100;
                        dizaine = (y - centaine*100)/10;
                        unite = y-(centaine*100)-(dizaine*10);
                        switch (centaine)
                        {
                            case 0: break;
                            case 1: lettre += "cent "; break;
                            case 2:
                                if ((dizaine == 0)&&(unite == 0)) lettre +="deux cents ";
                                else lettre +="deux cent "; break;
                            case 3:
                                if ((dizaine == 0)&&(unite == 0)) lettre += "trois cents ";
                                else lettre += "trois cent "; break;
                            case 4:
                                if ((dizaine == 0)&&(unite == 0)) lettre+="quatre cents ";
                                else lettre +="quatre cent "; break;
                            case 5:
                                if ((dizaine == 0)&&(unite == 0)) lettre+="cinq cents ";
                                else lettre+="cinq cent "; break;
                            case 6:
                                if ((dizaine == 0)&&(unite == 0)) lettre+="six cents ";
                                else lettre += "six cent "; break;
                            case 7:
                                if ((dizaine == 0)&&(unite == 0)) lettre+="sept cents ";
                                else lettre+="sept cent "; break;
                            case 8:
                                if ((dizaine == 0)&&(unite == 0)) lettre+="huit cents "; else lettre+="huit cent ";	break;
                            case 9:
                                if ((dizaine == 0)&&(unite == 0)) lettre+="neuf cents "; else lettre+="neuf cent "; break;
                        }
                        // endSwitch(centaine)   	
                        switch (dizaine)
                        {
                            case 0: break;
                            case 1: dix = true; break;
                            case 2: lettre+="vingt "; break;
                            case 3: lettre+="trente "; break;
                            case 4: lettre+="quarante "; break;
                            case 5: lettre+="cinquante "; break;
                            case 6: lettre+="soixante "; break;
                            case 7: dix = true; lettre+="soixante "; break;
                            case 8: lettre+="quatre-vingt "; break;
                            case 9: dix = true; lettre+="quatre-vingt "; break;
                        } // endSwitch(dizaine)
                        switch (unite)
                        {
                            case 0: if(dix) lettre+="dix "; break;
                            case 1: if(dix) lettre+="onze ";  else lettre+="un "; break;
                            case 2: if(dix) lettre+="douze "; else lettre+="deux "; break;
                            case 3: if(dix) lettre+="treize "; else lettre+="trois "; break;
                            case 4: if(dix) lettre+="quatorze "; else lettre+="quatre "; break;
                            case 5: if(dix) lettre+="quinze "; else lettre+="cinq "; break;
                            case 6: if(dix) lettre+="seize "; else lettre+="six "; break;
                            case 7: if(dix) lettre+="dix-sept "; else lettre+="sept "; break;
                            case 8: if(dix) lettre+="dix-huit "; else lettre+="huit "; break;
                            case 9: if(dix) lettre+="dix-neuf "; else lettre+="neuf "; break;
                        }
                        // endSwitch(unite)   		
                        switch (i)
                        {
                            case 1000000000: if(y>1) lettre+="milliards "; else lettre+="milliard "; break;
                            case 1000000: if(y>1) lettre+="millions "; else lettre+="million "; break;
                            case 1000: if(lettre == "un ") lettre = "mille "; else lettre += "mille "; break;
                    }
                    }
                    // end
                    if (y!=0) reste -= y*i; dix = false; 		 }
                // end for 	
                if (lettre.Length ==0) lettre+="zero";
                return lettre;
            }
     


        // Récupération du dernier versement en caisse
            public class Maxcaisse
        {
            public Label Session { set; get; }
            public Label Numrecu { set; get; }
            public Label Nom { set; get; }
            public Label Dates { set; get; }
            public Label Heure { set; get; }
            public Label Classe { set; get; }
            public Label Motif { set; get; }
            public Label Montant { set; get; }
            public Label Montlettre { set; get; }
            public Label Matricule { set; get; }
            public Label Reste { set; get; }

            public Maxcaisse(Label session,Label numrecu,Label nom,Label dates,Label heure,Label classe,Label motif,Label montant, Label montlettre, Label matricule,Label reste)
            {
                this.Session = session; this.Numrecu = numrecu; this.Nom = nom; this.Dates = dates; this.Heure = heure;
                this.Classe = classe; this.Motif = motif; this.Montant = montant; this.Montlettre = montlettre;
                this.Matricule = matricule; this.Reste = reste;
            }

            // récupéré le max recu dans la caisse;
            public void maxcaisse()
            {
                // séléction pour récupéré le dernier numéro de recu
                var connection4 = new MySqlConnection(connectionstring);
                connection4.Open();
                string numrecu = "";
                string matricule = "";
                string session = "";
                // Récupération du dernier numero de recu
                string res = "select monverse,dateverse,heure,matricule,session,numrecu,max(numrecu) as recu from caissescolarite";
                var command4 = new MySqlCommand(res, connection4);
                command4.Prepare();
                reader = command4.ExecuteReader();
                while (reader.Read())
                {
                    Montant.Text = reader.GetValue(0).ToString();
                    
                    
                   // Matricule.Text = reader.GetValue(3).ToString();
                    Session.Text = reader.GetValue(4).ToString();
                    Numrecu.Text = reader.GetValue(6).ToString();
                    numrecu = reader.GetValue(6).ToString();
                   
                }
                reader.Close();
                // Récupération du montant versé
                var connection42 = new MySqlConnection(connectionstring);
                connection42.Open();
                string res2 = "select monverse,matricule,session,dateverse,heure from caissescolarite where numrecu="+numrecu+"";
                var command42 = new MySqlCommand(res2, connection42);
                command42.Prepare();
                reader42 = command42.ExecuteReader();
                while (reader42.Read())
                {
                    Montant.Text = reader42.GetValue(0).ToString();
                    matricule = reader42.GetValue(1).ToString();
                    Matricule.Text = reader42.GetValue(1).ToString();
                    session = reader42.GetValue(2).ToString();
                    Dates.Text = string.Format("{0:dd / MM / yyyy}", reader42.GetValue(3));
                    Heure.Text = (reader42.GetValue(4).ToString());
                    Session.Text = reader42.GetValue(2).ToString();
                    Montlettre.Text = converti(int.Parse(reader42.GetValue(0).ToString())) + " franc cfa";
                }
                reader42.Close();
                string se = "select inscription.session,inscription.reste,inscription.id_classe,inscription.matricule,eleves.matricule,eleves.nom_eleve,eleves.prenom_eleve,classe.id_classe,classe.nom_classe from inscription inner join eleves on inscription.matricule=eleves.matricule inner join classe on inscription.id_classe=classe.id_classe where inscription.matricule=@matricule AND inscription.session=@session";
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@matricule", matricule);
                para.Add("@session", session);
                var connect = new MySqlConnection(connectionstring);
                connect.Open();
                var commande = new MySqlCommand(se, connect);
                foreach (KeyValuePair<string, object> parametres in para)
                {
                    commande.Parameters.Add(new MySqlParameter(parametres.Key, parametres.Value));
                }
                commande.Prepare();

                reader2 = commande.ExecuteReader();
                while (reader2.Read())
                {
                    Nom.Text = reader2.GetValue(5).ToString()+" " + reader2.GetValue(6).ToString();
                    Classe.Text = reader2.GetValue(8).ToString();
                    Reste.Text = reader2.GetValue(1).ToString();
                   if(int.Parse(reader2.GetValue(1).ToString()) > 0)
                    {
                        Motif.Text = "INSCRIPTION";
                    }else if(int.Parse(reader2.GetValue(1).ToString()) == 0)
                    {
                        Motif.Text = "SOLDE INSCRIPTION";
                    }
                }
                reader2.Close();
            }
        }

        // Récupération des informations de l'établissement 
        public class Infoschool
        {
            public Label Nom { set; get; }
            public Label Nom2 { set; get; }
            public Label Phone1 { set; get; }
            public Label Mail { set; get; }
            public Label Localisation { set; get; }
            public Label Bp { set; get; }
            public PictureBox Logo { set; get; }
            public Infoschool(Label nom, Label phone1, Label mail, Label localise, Label bp, PictureBox logo, Label nom2)
            {
                this.Nom = nom; this.Phone1 = phone1; this.Mail = mail;
                this.Localisation = localise; this.Bp = bp; this.Logo = logo; this.Nom2 = nom2;
            }
            public void Infos()
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
                    idmax = int.Parse(reader.GetValue(1).ToString());
                }
                reader.Close();

                // recuperation du nom de l'etablissement
                var connection1 = new MySqlConnection(connectionstring);
                connection1.Open();
                string res2 = "select nom_etabli,mail,localisation,bp,phone1,phone2,logo from etablissement where id_etablissement=" + idmax + "";
                var command2 = new MySqlCommand(res2, connection1);
                command2.Prepare();
                reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    Nom.Text = reader2.GetValue(0).ToString();
                    Nom2.Text = reader2.GetValue(0).ToString();
                    Mail.Text = "EMAIL : " + reader2.GetValue(1).ToString();
                    Phone1.Text = "TEL : " + reader2.GetValue(4).ToString() + " -"+ reader2.GetValue(5).ToString();
                    Localisation.Text = reader2.GetValue(2).ToString();
                    Bp.Text = "B.P : " + reader2.GetValue(3).ToString();
                    Logo.Image = CovertByteArrayToImage((byte[])(reader2.GetValue(6)));
                }
                reader2.Close();
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
        public IMPRESSION_inscription()
        {
            InitializeComponent();
            connexionDB();
            Maxcaisse recu = new Maxcaisse(label27, label30, label31, label32, label33, label34, label35, label36, label37, label38, label39);
            recu.maxcaisse();
            Infoschool ecol = new Infoschool(label5, label6, label7, label9, label8, pictureBox1, label10);
            ecol.Infos();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += myPrintPage;
            pd.Document = doc;
            if(pd.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }
        }
        private void myPrintPage(Object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(panel1.Width, panel1.Height);
            panel1.DrawToBitmap(bm, new Rectangle(0, 0, panel1.Width, panel1.Height));
            e.Graphics.DrawImage(bm, 0, 0);
            bm.Dispose();
        }
    }
}
