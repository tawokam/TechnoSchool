using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TechnoSchool.AddClasse;

namespace TechnoSchool
{
    public partial class RapportListEleveInscript : Form
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
            RapportListinscript cr = new RapportListinscript();
            string requete = "select nom_eleve,prenom_eleve,sexe,date_naiss,nom_classe,nom_section,session,inscription.matricule,nom_etabli,phone1,phone2,mail,logo,localisation,bp from inscription inner join eleves on inscription.matricule=eleves.matricule inner join classe on inscription.id_classe=classe.id_classe inner join tabsection on classe.id_section=tabsection.id_section, etablissement order by nom_eleve asc";
            command = new MySqlCommand(requete, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            adapter.SelectCommand.CommandType = CommandType.Text;
            DataSetData DB = new DataSetData();
            //Datatab
            adapter.Fill(DB, "Listinscript");
            object[] rowVals = new object[15];
            rowVals[0] = "azert"; rowVals[1] = "azert"; rowVals[2] = "azert"; rowVals[3] = "2022/08/15"; rowVals[4] = "WQSE";
            rowVals[5] = "WQSE"; rowVals[6] = "WQSE"; rowVals[7] = "WQSE"; rowVals[8] = "WQSE"; rowVals[9] = "1233";
            rowVals[10] = "133445"; rowVals[11] = "WQSE"; rowVals[12] = transform("azerty"); rowVals[13] = "WQSE";
            rowVals[14] = "WQSE";
           // DB.Tables["Listinscript"].Rows.Add(rowVals);
            adapter.InsertCommand = new MySqlCommand("INSERT INTO Listinscript(nom_eleve,prenom_eleve,sexe,date_naiss,nom_classe,nom_section,session,matricule,nom_etabli,phone1,phone2,mail,logo,localisation,bp)" + "VALUES(@nomel,@prenom,@sexe,@datenaiss,@nomclas,@nomsect,@session,@matricule,@nometabli,@phone1,@phone2,@mail,@logo,@localisation,@bp)", connection);
            adapter.InsertCommand.Parameters.Add("nomel", MySqlDbType.VarChar, 0, "nom_eleve");
            adapter.InsertCommand.Parameters.Add("prenom", MySqlDbType.VarChar, 15, "prenom_eleve");
            adapter.InsertCommand.Parameters.Add("sexe", MySqlDbType.VarChar, 15, "sexe");
            adapter.InsertCommand.Parameters.Add("datenaiss", MySqlDbType.VarChar, 15, "date_naiss");
            adapter.InsertCommand.Parameters.Add("nomclas", MySqlDbType.VarChar, 15, "nom_classe");
            adapter.InsertCommand.Parameters.Add("nomsect", MySqlDbType.VarChar, 15, "nom_section");
            adapter.InsertCommand.Parameters.Add("session", MySqlDbType.VarChar, 15, "session");
            adapter.InsertCommand.Parameters.Add("matricule", MySqlDbType.VarChar, 15, "matricule");
            adapter.InsertCommand.Parameters.Add("nometabli", MySqlDbType.VarChar, 15, "nom_etabli");
            adapter.InsertCommand.Parameters.Add("phone1", MySqlDbType.Int32, 15, "phone1");
            adapter.InsertCommand.Parameters.Add("phone2", MySqlDbType.Int32, 15, "phone2");
            adapter.InsertCommand.Parameters.Add("mail", MySqlDbType.VarChar, 15, "mail");
            adapter.InsertCommand.Parameters.Add("logo", MySqlDbType.LongBlob, 15, "logo");
            adapter.InsertCommand.Parameters.Add("localisation", MySqlDbType.VarChar, 15, "localisation");
            adapter.InsertCommand.Parameters.Add("bp", MySqlDbType.VarChar, 15, "bp");
            //adapter.Update(DB, "Listinscript");
            cr.SetDataSource(DB);
            //this.dataGridView1.DataSource = DB;
            crystalReportViewer3.ReportSource = cr;
            //  cr.Refresh();
            // connection.Close();
            Cursor = Cursors.Default;
            connection.Close();

        }


        // rapport avec session en critere
        public void appelraportSession(string session)
        {
            // Alimentation de mes table dataset
            connection = new MySqlConnection(connectionstring);
            connection.Open();


            Cursor = Cursors.WaitCursor;
            RapportListinscript cr = new RapportListinscript();
            string requete = "select nom_eleve,prenom_eleve,sexe,date_naiss,nom_classe,nom_section,session,inscription.matricule,nom_etabli,phone1,phone2,mail,logo,localisation,bp from inscription inner join eleves on inscription.matricule=eleves.matricule inner join classe on inscription.id_classe=classe.id_classe inner join tabsection on classe.id_section=tabsection.id_section, etablissement where inscription.session='" + session +"' order by nom_eleve asc";
            command = new MySqlCommand(requete, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            adapter.SelectCommand.CommandType = CommandType.Text;
            DataSetData DB = new DataSetData();
            //Datatab
            adapter.Fill(DB, "Listinscript");
            object[] rowVals = new object[15];
            rowVals[0] = "azert"; rowVals[1] = "azert"; rowVals[2] = "azert"; rowVals[3] = "2022/08/15"; rowVals[4] = "WQSE";
            rowVals[5] = "WQSE"; rowVals[6] = "WQSE"; rowVals[7] = "WQSE"; rowVals[8] = "WQSE"; rowVals[9] = "1233";
            rowVals[10] = "133445"; rowVals[11] = "WQSE"; rowVals[12] = transform("azerty"); rowVals[13] = "WQSE";
            rowVals[14] = "WQSE";
            // DB.Tables["Listinscript"].Rows.Add(rowVals);
            adapter.InsertCommand = new MySqlCommand("INSERT INTO Listinscript(nom_eleve,prenom_eleve,sexe,date_naiss,nom_classe,nom_section,session,matricule,nom_etabli,phone1,phone2,mail,logo,localisation,bp)" + "VALUES(@nomel,@prenom,@sexe,@datenaiss,@nomclas,@nomsect,@session,@matricule,@nometabli,@phone1,@phone2,@mail,@logo,@localisation,@bp)", connection);
            adapter.InsertCommand.Parameters.Add("nomel", MySqlDbType.VarChar, 0, "nom_eleve");
            adapter.InsertCommand.Parameters.Add("prenom", MySqlDbType.VarChar, 15, "prenom_eleve");
            adapter.InsertCommand.Parameters.Add("sexe", MySqlDbType.VarChar, 15, "sexe");
            adapter.InsertCommand.Parameters.Add("datenaiss", MySqlDbType.VarChar, 15, "date_naiss");
            adapter.InsertCommand.Parameters.Add("nomclas", MySqlDbType.VarChar, 15, "nom_classe");
            adapter.InsertCommand.Parameters.Add("nomsect", MySqlDbType.VarChar, 15, "nom_section");
            adapter.InsertCommand.Parameters.Add("session", MySqlDbType.VarChar, 15, "session");
            adapter.InsertCommand.Parameters.Add("matricule", MySqlDbType.VarChar, 15, "matricule");
            adapter.InsertCommand.Parameters.Add("nometabli", MySqlDbType.VarChar, 15, "nom_etabli");
            adapter.InsertCommand.Parameters.Add("phone1", MySqlDbType.Int32, 15, "phone1");
            adapter.InsertCommand.Parameters.Add("phone2", MySqlDbType.Int32, 15, "phone2");
            adapter.InsertCommand.Parameters.Add("mail", MySqlDbType.VarChar, 15, "mail");
            adapter.InsertCommand.Parameters.Add("logo", MySqlDbType.LongBlob, 15, "logo");
            adapter.InsertCommand.Parameters.Add("localisation", MySqlDbType.VarChar, 15, "localisation");
            adapter.InsertCommand.Parameters.Add("bp", MySqlDbType.VarChar, 15, "bp");
            //adapter.Update(DB, "Listinscript");
            cr.SetDataSource(DB);
            //this.dataGridView1.DataSource = DB;
            crystalReportViewer3.ReportSource = cr;
            //  cr.Refresh();
            // connection.Close();
            Cursor = Cursors.Default;
            connection.Close();

        }
        // liste des session 
        public void listesision()
        {
            connection.Open();
            string req = "SELECT nom_session FROM tabsession";
            command = new MySqlCommand(req, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetValue(0).ToString());
            }
            reader.Close();
            connection.Close();
        }
        public RapportListEleveInscript()
        {
            InitializeComponent();
            connexionDB();
            appelraport();
            listesision();
        }

        

        private void RapportListEleveInscript_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'technosoftDataSet.eleves' table. You can move, or remove it, as needed.
            //this.elevesTableAdapter.Fill(this.technosoftDataSet.eleves);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            appelraport();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            string session = comboBox1.Text;
            appelraportSession(session);
        }
    }
}
