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
    public partial class RapportListEleve : Form
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
        // Convertir une chaine en byte
        static byte[] transform(string machaine)
        {
            byte[] bytes = new byte[machaine.Length * sizeof(char)];
            System.Buffer.BlockCopy(machaine.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        // chargement du crystal report 
        public void appelraport()
        {
            // Alimentation de mes table dataset
            connection = new MySqlConnection(connectionstring);
            connection.Open();


            Cursor = Cursors.WaitCursor;
            ListEleve cr = new ListEleve();
            string requete = "select matricule,nom_eleve,prenom_eleve,sexe,date_naiss, nom_etabli,phone1,phone2,mail,logo,localisation,bp from eleves,etablissement order by nom_eleve asc";
            command = new MySqlCommand(requete, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            adapter.SelectCommand.CommandType = CommandType.Text;
            DataSetData DB = new DataSetData();
            //Datatab
            adapter.Fill(DB, "ListEleve");

            // DB.Tables["Listinscript"].Rows.Add(rowVals);

            //adapter.Update(DB, "Listinscript");
            cr.SetDataSource(DB);
            //this.dataGridView1.DataSource = DB;
            crystalReportViewer1.ReportSource = cr;
            //  cr.Refresh();
            // connection.Close();
            Cursor = Cursors.Default;
            connection.Close();

        }
        public RapportListEleve()
        {
            InitializeComponent();
            connexionDB();
            appelraport();
        }
    }
}
