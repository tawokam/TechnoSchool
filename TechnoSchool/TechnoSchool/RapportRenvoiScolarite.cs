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
    public partial class RapportRenvoiScolarite : Form
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

                string messag = "Nous nous somme heurté à un problème lors de la connexion au serveur";
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
        // liste des classes
        public void listeclasse(ComboBox classe)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string req = "SELECT nom_classe FROM classe";
            command = new MySqlCommand(req, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                classe.Items.Add(reader.GetValue(0).ToString());
                classe.SelectedIndex = 0;
            }
            reader.Close();
            connection.Close();
        }
        // chargement du crystal report 
        public void appelraport(string session, string classe, int mont, RadioButton inferieur, RadioButton superieur)
        {
            // Alimentation de mes table dataset
            connection = new MySqlConnection(connectionstring);
            connection.Open();


            Cursor = Cursors.WaitCursor;
            RenvoiScolarite cr = new RenvoiScolarite();
            string requete = "";
            if(inferieur.Checked == true)
            {
                requete = "select scolarite.matricule,scolarite.montscolarite,montverse,reste,scolarite.session,nom_eleve,prenom_eleve,nom_etabli,phone1,phone2,mail,logo,localisation,bp,nom_classe from scolarite inner join eleves on scolarite.matricule=eleves.matricule inner join classe on scolarite.id_classe=classe.id_classe, etablissement where scolarite.session='" + session + "' AND nom_classe='" + classe + "' AND montverse < @mont order by nom_eleve asc";
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@mont", mont);
                command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parameter in para)
                {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));

                }
                command.Prepare();
            }
            else if(superieur.Checked == true)
            {
                requete = "select scolarite.matricule,scolarite.montscolarite,montverse,reste,scolarite.session,nom_eleve,prenom_eleve,nom_etabli,phone1,phone2,mail,logo,localisation,bp,nom_classe from scolarite inner join eleves on scolarite.matricule=eleves.matricule inner join classe on scolarite.id_classe=classe.id_classe, etablissement where scolarite.session='" + session + "' AND nom_classe='" + classe + "' AND montverse > @mont order by nom_eleve asc";
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("@mont", mont);
                command = new MySqlCommand(requete, connection);
                foreach (KeyValuePair<string, object> parameter in para)
                {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));

                }
                command.Prepare();
            }else
            {
                requete = "select scolarite.matricule,scolarite.montscolarite,montverse,reste,scolarite.session,nom_eleve,prenom_eleve,nom_etabli,phone1,phone2,mail,logo,localisation,bp,nom_classe from scolarite inner join eleves on scolarite.matricule=eleves.matricule inner join classe on scolarite.id_classe=classe.id_classe, etablissement where scolarite.session='" + session + "' AND nom_classe='" + classe + "' order by nom_eleve asc";
                command = new MySqlCommand(requete, connection);
                command.Prepare();
            }
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            adapter.SelectCommand.CommandType = CommandType.Text;
            DataSetData DB = new DataSetData();
            //Datatab
            adapter.Fill(DB, "RenvoiScolarite");
            object[] rowVals = new object[15];
            rowVals[0] = "azert"; rowVals[1] = "111"; rowVals[2] = "111"; rowVals[3] = "44443"; rowVals[4] = "WQSE";
            rowVals[5] = "WQSE"; rowVals[6] = "WQSE"; rowVals[7] = "WQSE"; rowVals[8] = "111"; rowVals[9] = "1233";
            rowVals[10] = "133445"; rowVals[11] = transform("azerty"); rowVals[12] = "dfdf"; rowVals[13] = "WQSE";
            rowVals[14] = "WQSE";
            //DB.Tables["RenvoiInscription"].Rows.Add(rowVals);
            adapter.InsertCommand = new MySqlCommand("INSERT INTO RenvoiScolarite(matricule,montscolarite,montverse,reste,session,nom_eleve,prenom_eleve,nom_etabli,phone1,phone2,mail,logo,localisation,bp,nom_classe)" + "VALUES(@matricule,@montinscription,@montverse,@reste,@session,@nom_eleve,@prenom_eleve,@nometabli,@phone1,@phone2,@mail,@logo,@localisation,@bp,@nomclasse)", connection);
            adapter.InsertCommand.Parameters.Add("matricule", MySqlDbType.VarChar, 0, "matricule");
            adapter.InsertCommand.Parameters.Add("montinscription", MySqlDbType.Int32, 15, "montscolarite");
            adapter.InsertCommand.Parameters.Add("montverse", MySqlDbType.Int32, 15, "montverse");
            adapter.InsertCommand.Parameters.Add("reste", MySqlDbType.Int32, 15, "reste");
            adapter.InsertCommand.Parameters.Add("session", MySqlDbType.VarChar, 15, "session");
            adapter.InsertCommand.Parameters.Add("nom_eleve", MySqlDbType.VarChar, 15, "nom_eleve");
            adapter.InsertCommand.Parameters.Add("prenom_eleve", MySqlDbType.VarChar, 15, "prenom_eleve");
            adapter.InsertCommand.Parameters.Add("nometabli", MySqlDbType.VarChar, 15, "nom_etabli");
            adapter.InsertCommand.Parameters.Add("phone1", MySqlDbType.Int32, 15, "phone1");
            adapter.InsertCommand.Parameters.Add("phone2", MySqlDbType.Int32, 15, "phone2");
            adapter.InsertCommand.Parameters.Add("mail", MySqlDbType.VarChar, 15, "mail");
            adapter.InsertCommand.Parameters.Add("logo", MySqlDbType.LongBlob, 15, "logo");
            adapter.InsertCommand.Parameters.Add("localisation", MySqlDbType.VarChar, 15, "localisation");
            adapter.InsertCommand.Parameters.Add("bp", MySqlDbType.VarChar, 15, "bp");
            adapter.InsertCommand.Parameters.Add("nomclasse", MySqlDbType.VarChar, 15, "nom_classe");
            //adapter.Update(DB, "Listinscript");
            cr.SetDataSource(DB);
            //this.dataGridView1.DataSource = DB;
            crystalReportViewer1.ReportSource = cr;
            //  cr.Refresh();
            // connection.Close();
            Cursor = Cursors.Default;
            connection.Close();

        }
        public RapportRenvoiScolarite()
        {
            InitializeComponent();
            connexionDB();
            sessionactive(label3);
            listeclasse(comboBox1);
            appelraport("session", "classe", 0, radioButton1, radioButton2);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string session = label3.Text; string classe = comboBox1.Text; int mont = 0;
            if(textBox1.Text == "")
            {
                mont = 0;
            }else { mont = int.Parse(textBox1.Text); }
            appelraport(session,classe,mont, radioButton1, radioButton2);
        }
    }
}
