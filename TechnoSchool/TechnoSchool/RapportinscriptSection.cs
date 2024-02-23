﻿using MySql.Data.MySqlClient;
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
    public partial class RapportinscriptSection : Form
    {
        public static MySqlConnection connection;
        public static MySqlCommand command;

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

        // chargement du crystal report 
        public void appelraport(string section, string session)
        {
            // Alimentation de mes table dataset
            connection = new MySqlConnection(connectionstring);
            connection.Open();


            Cursor = Cursors.WaitCursor;
            RapportInscriptSection cr = new RapportInscriptSection();
            
            string requete = "select nom_eleve,prenom_eleve,sexe,date_naiss,nom_classe,nom_section,session,inscription.matricule,nom_etabli,phone1,phone2,mail,logo,localisation,bp from inscription inner join eleves on inscription.matricule=eleves.matricule inner join classe on inscription.id_classe=classe.id_classe inner join tabsection on classe.id_section=tabsection.id_section, etablissement WHERE nom_section='"+section+"' AND session='"+session+"' order by nom_eleve asc";
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
            crystalReportViewer1.ReportSource = cr;
              cr.Refresh();
            // connection.Close();
            Cursor = Cursors.Default;
            connection.Close();

        }
        // liste des section
        public void listesection()
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string req = "SELECT nom_section FROM tabsection";
            command = new MySqlCommand(req, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetValue(0).ToString());
                comboBox1.SelectedIndex = 0;
            }
            reader.Close();
            connection.Close();
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
        public RapportinscriptSection()
        {
            InitializeComponent();
            connexionDB();
            listesection();
            sessionactive(label3);
            string section = comboBox1.Text;
            string session = label3.Text;
            appelraport(section,session);
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            string section = comboBox1.Text;
            string session = label3.Text;
            appelraport(section,session);
        }
    }
}
