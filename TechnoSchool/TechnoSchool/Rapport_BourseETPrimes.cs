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
    public partial class Rapport_BourseETPrimes : Form
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
        // liste des sessions
        public void listsession(ComboBox session)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();
            string req = "SELECT nom_session FROM tabsession order by nom_session";
            command = new MySqlCommand(req, connection);
            reader = command.ExecuteReader();
            session.Items.Clear();
            while (reader.Read())
            {
                session.Items.Add(reader.GetValue(0).ToString());
            }
            reader.Close();
            connection.Close();
        }

        public void bourse(string session, string type, string matricule, int numAppel, string statut)
        {
            connection = new MySqlConnection(connectionstring);
            connection.Open();


            Cursor = Cursors.WaitCursor;
            BourseETprime cr = new BourseETprime();
            // Alimentation de mon datatable CarteScolaire
            string requete = "";

            // rapport toutes les bourse et primes
            if(statut == "toutes")
            {
                if (numAppel == 0)
                {
                    requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement order by session,nom_eleve";
                }
                else
                {
                    if (session != "" && type == "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' order by session,nom_eleve";
                    }
                    else if (session != "" && type != "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND type='" + type + "' order by session,nom_eleve";
                    }
                    else if (session != "" && type != "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND type='" + type + "' AND bourse.matricule like '" + "%" + matricule + "%" + "' order by session,nom_eleve";
                    }
                    else if (session != "" && type == "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND bourse.matricule like '" + "%" + matricule + "%" + "' order by session,nom_eleve";
                    }
                    else if (session == "" && type == "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE bourse.matricule like '" + "%" + matricule + "%" + "' order by session,nom_eleve";
                    }
                    else if (session == "" && type != "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE type='" + type + "' AND bourse.matricule like '" + "%" + matricule + "%" + "' order by session,nom_eleve";
                    }
                    else if (session != "" && type != "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE type='" + type + "' AND session='" + session + "' order by session,nom_eleve";
                    }
                    else if (session == "" && type == "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement order by session,nom_eleve";
                    }
                }
            }
            else if (statut == "encour")
            {
                if (numAppel == 0)
                {
                    requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE bourse.statut='Encour' order by session,nom_eleve";
                }
                else
                {
                    if (session != "" && type == "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND bourse.statut='Encour' order by session,nom_eleve";
                    }
                    else if (session != "" && type != "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND type='" + type + "' AND bourse.statut='Encour' order by session,nom_eleve";
                    }
                    else if (session != "" && type != "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND type='" + type + "' AND bourse.matricule like '" + "%" + matricule + "%" + "' AND bourse.statut='Encour' order by session,nom_eleve";
                    }
                    else if (session != "" && type == "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND bourse.matricule like '" + "%" + matricule + "%" + "' AND bourse.statut='Encour' order by session,nom_eleve";
                    }
                    else if (session == "" && type == "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE bourse.matricule like '" + "%" + matricule + "%" + "' AND bourse.statut='Encour' order by session,nom_eleve";
                    }
                    else if (session == "" && type != "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE type='" + type + "' AND bourse.matricule like '" + "%" + matricule + "%" + "' AND bourse.statut='Encour' order by session,nom_eleve";
                    }
                    else if (session != "" && type != "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE type='" + type + "' AND session='" + session + "' AND bourse.statut='Encour' order by session,nom_eleve";
                    }
                    else if (session == "" && type == "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement where bourse.statut='Encour' order by session,nom_eleve";
                    }
                }
            }
            else if (statut == "regle")
            {
                if (numAppel == 0)
                {
                    requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement where bourse.statut='Réglé' order by session,nom_eleve";
                }
                else
                {
                    if (session != "" && type == "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND bourse.statut='Réglé' order by session,nom_eleve";
                    }
                    else if (session != "" && type != "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND type='" + type + "' AND bourse.statut='Réglé' order by session,nom_eleve";
                    }
                    else if (session != "" && type != "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND type='" + type + "' AND bourse.matricule like '" + "%" + matricule + "%" + "' AND bourse.statut='Réglé' order by session,nom_eleve";
                    }
                    else if (session != "" && type == "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE session='" + session + "' AND bourse.matricule like '" + "%" + matricule + "%" + "' AND bourse.statut='Réglé' order by session,nom_eleve";
                    }
                    else if (session == "" && type == "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE bourse.matricule like '" + "%" + matricule + "%" + "' AND bourse.statut='Réglé' order by session,nom_eleve";
                    }
                    else if (session == "" && type != "" && matricule != "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE type='" + type + "' AND bourse.matricule like '" + "%" + matricule + "%" + "' AND bourse.statut='Réglé' order by session,nom_eleve";
                    }
                    else if (session != "" && type != "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE type='" + type + "' AND session='" + session + "' AND bourse.statut='Réglé' order by session,nom_eleve";
                    }
                    else if (session == "" && type == "" && matricule == "")
                    {
                        requete = "SELECT bourse.matricule,eleves.nom_eleve,eleves.prenom_eleve,bourse.type,bourse.motif,bourse.montant,bourse.session,nom_etabli,phone1,phone2,mail,logo,localisation,bp,bourse.statut from bourse inner join eleves on bourse.matricule=eleves.matricule, etablissement WHERE bourse.statut='Réglé' order by session,nom_eleve";
                    }
                }
            }
            command = new MySqlCommand(requete, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            adapter.SelectCommand.CommandType = CommandType.Text;
            DataSetData DB = new DataSetData();
            //Datatab
            adapter.Fill(DB, "BourseEtPrime");

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
        public string Statut = "";
        public Rapport_BourseETPrimes(string toute)
        {
            InitializeComponent();
            connexionDB();
            listsession(comboBox2);
            Statut = toute;
            bourse(comboBox2.Text, comboBox1.Text, textBox1.Text, 0, Statut);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bourse(comboBox2.Text, comboBox1.Text, textBox1.Text, 1, Statut);
        }
    }
}
