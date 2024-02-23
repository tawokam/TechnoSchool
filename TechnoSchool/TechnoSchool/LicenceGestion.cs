using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnoSchool
{
    public partial class LicenceGestion : Form
    {
        // class de récupération et d'insertion des info sur la licence d'utilisation
        public class Newlicence
        {
            public DateTime Dateinst { set; get; }
            public RadioButton Typelice3m { set; get; }
            public RadioButton Typelice1A { set; get; }
            public TextBox Mdplic { set; get; }
            public Newlicence(DateTime date,RadioButton licence3m, RadioButton licence1A, TextBox mdp)
            {
                this.Dateinst = date; this.Typelice3m = licence3m; this.Typelice1A = licence1A; this.Mdplic = mdp;
            }
            // Enregistrement des données de licence
            public void datalic()
            {
                string cheminfichierConfig = Path.Combine(Environment.CurrentDirectory, "FileConfig/Licence.ini");
                try
                {
                    // type licence
                    int jour = 0; string typeLicence = "";
                    if (Typelice3m.Checked)
                    {
                        jour = 90; typeLicence = "test";
                    }
                    else if (Typelice1A.Checked)
                    {
                        jour = 400; typeLicence = "annuel";
                    }
                    if(typeLicence == "test" && Mdplic.Text != "160320013mois")
                    {
                        string messag = "Le mot de passe entrée est incorrect. Veuillez contacter votre fournisseur pour la configuration";
                        string titre = "Configuration";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }else if(typeLicence == "annuel" && Mdplic.Text != "16031989")
                    {
                        string messag = "Le mot de passe entrée est incorrect. Veuillez contacter votre fournisseur pour la configuration";
                        string titre = "Configuration";
                        // Programation des bouton de la boite de message
                        MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }else
                    {
                        string newdate = Dateinst.ToString("yyyy-MM-dd");
                        // Vérifiez si le fichier existe

                        if (File.Exists(cheminfichierConfig))
                        {
                            // Si oui, ouvrir le fichier et modifier le contenu
                            GestionFileIni ger = new GestionFileIni(cheminfichierConfig);
                            // suppression des section 
                            ger.RemoveSection("Date d'installation");
                            ger.RemoveSection("Version");
                            ger.RemoveSection("Jour");
                            ger.RemoveSection("Dernier connection");
                            
                            
                            // reecriture des nouvelles informations dans mon fichier ini
                            ger.WriteIni("Date d'installation","DateInstal",newdate);
                            ger.WriteIni("Version", "Version", typeLicence);
                            ger.WriteIni("Jour", "Jour", jour.ToString());
                            ger.WriteIni("Dernier connection", "Dernier connection", newdate);

                            string messag = "Licence d'utilisation mise a jour ave succès";
                            string titre = "Achat licence ";
                            // Programation des bouton de la boite de message
                            MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // si non créer le fichier ini
                            using (StreamWriter fichier = new StreamWriter(cheminfichierConfig))
                            {
                                fichier.Close();
                                string chai = Path.Combine(Environment.CurrentDirectory, "FileConfig/Licence.ini");
                                GestionFileIni ger = new GestionFileIni(chai);
                                try
                                {
                                    ger.WriteIni("Date d'installation", "DateInstal", newdate);
                                    ger.WriteIni("Version", "Version", typeLicence);
                                    ger.WriteIni("Jour", "Jour", jour.ToString());
                                    ger.WriteIni("Dernier connection", "Dernier connection", newdate);
                                    string messag = "Achat de la licence d'utilisation du logiciel TechnoSchool validé";
                                    string titre = "Achat licence ";
                                    // Programation des bouton de la boite de message
                                    DialogResult de = MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    if (de == DialogResult.OK)
                                    {
                                        LicenceGestion formu = new LicenceGestion();
                                        formu.Close();
                                    }
                                }
                                catch(Exception ex)
                                {
                                    string messag = "Nou nous somme heurté à un problème lors de la configuration du logiciel "+ex;
                                    string titre = "Achat licence ";
                                    // Programation des bouton de la boite de message
                                    MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                
                            }
                           
                        }
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
        public LicenceGestion()
        {
            InitializeComponent();
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked == false && radioButton2.Checked == false)
            {
                string messag = " Veuillez séléctionner la version que vous souhaitez installer (test ou annuel)";
                string titre = "Configuration";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }else if(mdpProvisoire.Text == "")
            {
                string messag = "Le mot de passe est obligatoire";
                string titre = "Configuration";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }else
            {
                DateTime dates = DateTime.Parse(dateTimePicker1.Text);
                Newlicence licence = new Newlicence(dates, radioButton1, radioButton2, mdpProvisoire);
                    licence.datalic();
            }
        }
    }
}
