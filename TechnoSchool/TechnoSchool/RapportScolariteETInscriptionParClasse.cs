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
    public partial class RapportScolariteETInscriptionParClasse : Form
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

        // chargement du crystal report 
        public void appelraport(string session)
        {
            // Alimentation de mes table dataset
            connection = new MySqlConnection(connectionstring);
            connection.Open();


            Cursor = Cursors.WaitCursor;
            ScolariteETInscriptionParClasse cr = new ScolariteETInscriptionParClasse();
            string requete = "SELECT nom_section,nom_classe,count(id_inscription) as nombre_eleve,sum(scolarite.montscolarite) as montant_scolarite,sum(inscription.montinscription) as montant_inscription,sum(scolarite.reste + inscription.reste) as reste,nom_etabli,phone1,phone2,mail,logo,localisation,bp,inscription.session from scolarite inner join classe on scolarite.id_classe=classe.id_classe inner join tabsection on classe.id_section=tabsection.id_section right join inscription on scolarite.matricule=inscription.matricule, etablissement WHERE inscription.session='"+session+"' GROUP BY scolarite.id_classe ORDER BY nom_section ASC;";
            command = new MySqlCommand(requete, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            adapter.SelectCommand.CommandType = CommandType.Text;
            DataSetData DB = new DataSetData();
            //Datatab
            adapter.Fill(DB, "Scolarite&Inscription par classe");
            //adapter.Update(DB, "Listinscript");
            cr.SetDataSource(DB);
            //this.dataGridView1.DataSource = DB;
            crystalReportViewer1.ReportSource = cr;
            //  cr.Refresh();
            // connection.Close();
            Cursor = Cursors.Default;
            connection.Close();

        }
        public RapportScolariteETInscriptionParClasse()
        {
            InitializeComponent();
            connexionDB();
            sessionactive(label3);
            appelraport(label3.Text);
        }
    }
}
