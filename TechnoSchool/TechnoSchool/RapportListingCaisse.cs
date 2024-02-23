using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TechnoSchool.AddClasse;

namespace TechnoSchool
{
    public partial class RapportListingCaisse : Form
    {
        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataReader reader3;
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
                connectionstring = "server=" + server + ";user id=" + user + ";password=" + mdp + ";database=" + DB + "";

            }
            else
            {
                // si non créer le fichier ini

                string messag = "Nous nous somme heurtés à un problème lors de la connexion au serveur";
                string titre = "Fichier de configuration";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        // chargement du crystal report 
        public void appelraport(string motif, string matricule, string datedebut,string datefin,string session,CheckBox cochmotif, CheckBox cochmatricule, CheckBox cochdate)
        {
            // Alimentation de mes table dataset
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            try
            {
                //Cursor = Cursors.WaitCursor;
                ListingCaisse cr = new ListingCaisse();
                string requete = "";
                if (cochmotif.Checked == true && cochmatricule.Checked == false && cochdate.Checked == false)
                {
                    
                    requete = "SELECT caissescolarite.matricule,nom_eleve,monverse,dateverse,typeversement,session,nom_etabli,phone1,phone2,mail,logo,localisation,bp FROM caissescolarite inner join eleves on caissescolarite.matricule=eleves.matricule, etablissement where typeversement='" + motif + "' AND session ='" + session + "' ";

                }
                else if (cochmotif.Checked == true && cochmatricule.Checked == true && cochdate.Checked == false)
                {
                    requete = "SELECT caissescolarite.matricule,nom_eleve,monverse,dateverse,typeversement,session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from caissescolarite inner join eleves on caissescolarite.matricule=eleves.matricule, etablissement where caissescolarite.matricule='" + matricule + "' AND typeversement='" + motif + "' AND session ='" + session + "' order by nom_eleve asc";
                }
                else if (cochmotif.Checked == true && cochmatricule.Checked == true && cochdate.Checked == true)
                {
                    requete = "SELECT caissescolarite.matricule,nom_eleve,monverse,dateverse,typeversement,session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from caissescolarite inner join eleves on caissescolarite.matricule=eleves.matricule, etablissement where caissescolarite.matricule='" + matricule + "' AND typeversement='" + motif + "' AND dateverse between '" + datedebut + "' AND '" + datefin + "' AND session ='" + session + "' order by nom_eleve asc";
                }
                else if (cochmotif.Checked == false && cochmatricule.Checked == true && cochdate.Checked == false)
                {
                    requete = "SELECT caissescolarite.matricule,nom_eleve,monverse,dateverse,typeversement,session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from caissescolarite inner join eleves on caissescolarite.matricule=eleves.matricule, etablissement where caissescolarite.matricule='" + matricule + "' AND session ='" + session + "' order by nom_eleve asc";
                }
                else if (cochmotif.Checked == false && cochmatricule.Checked == true && cochdate.Checked == true)
                {
                    requete = "SELECT caissescolarite.matricule,nom_eleve,monverse,dateverse,typeversement,session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from caissescolarite inner join eleves on caissescolarite.matricule=eleves.matricule, etablissement where caissescolarite.matricule='" + matricule + "' AND dateverse between '" + datedebut + "' AND '" + datefin + "' AND session ='" + session + "' order by nom_eleve asc";
                }
                else if (cochmotif.Checked == false && cochmatricule.Checked == false && cochdate.Checked == true)
                {
                    requete = "SELECT caissescolarite.matricule,nom_eleve,monverse,dateverse,typeversement,session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from caissescolarite inner join eleves on caissescolarite.matricule=eleves.matricule, etablissement where dateverse between '" + datedebut + "' AND '" + datefin + "' AND session ='" + session + "' order by nom_eleve asc";
                }
                else if (cochmotif.Checked == true && cochmatricule.Checked == false && cochdate.Checked == true)
                {
                    requete = "SELECT caissescolarite.matricule,nom_eleve,monverse,dateverse,typeversement,session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from caissescolarite inner join eleves on caissescolarite.matricule=eleves.matricule, etablissement where typeversement='" + motif + "' AND dateverse between '" + datedebut + "' AND '" + datefin + "' AND session ='" + session + "' order by nom_eleve asc";
                }
                else if (cochmotif.Checked == false && cochmatricule.Checked == false && cochdate.Checked == false)
                {
                    requete = "SELECT caissescolarite.matricule as matricule,nom_eleve,monverse,dateverse,typeversement,session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from caissescolarite inner join eleves on caissescolarite.matricule=eleves.matricule, etablissement where session ='" + session + "' order by nom_eleve asc ";
                }
                command = new MySqlCommand(requete, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.SelectCommand.CommandType = CommandType.Text;
                DataSetData DB = new DataSetData();
                //Datatable
                
                adapter.Fill(DB, "ListingCaisse");
                
                //adapter.Update(DB, "Listinscript");
                cr.SetDataSource(DB);
                //this.dataGridView1.DataSource = DB;
                crystalReportViewer1.ReportSource = cr;
                cr.Refresh();
                // connection.Close();
                Cursor = Cursors.Default;
            }
            catch(Exception ex)
            {
                string messag = "Nous n'avons pas pu ouvrir votre rapport "+ ex;
                string titre = "Rapports";
                // Programation des bouton de la boite de message
                MessageBox.Show(messag, titre, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               
            }
            finally
            {
                connection.Close();
            }
           

        }
        // liste des session
        public void sessionactive(Label session)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string req = "SELECT nom_session FROM tabsession where statut='activer'";
            command = new MySqlCommand(req, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                session.Text = reader.GetValue(0).ToString();
            }
            reader.Close();
            connection.Close();
        }
        public RapportListingCaisse()
        {
            InitializeComponent();
            connexionDB();
            sessionactive(label10);

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void RapportListingCaisse_Load(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string motif = comboBox2.Text; string matricule = textBox2.Text; string datedebut = string.Format("{0:yyyy-MM-dd}", dateTimePicker3.Value);
            string datefin = string.Format("{0:yyyy-MM-dd}", dateTimePicker4.Value);
            appelraport(motif, matricule, datedebut, datefin, label10.Text, checkBox5, checkBox4, checkBox6);
        }
    }
}
