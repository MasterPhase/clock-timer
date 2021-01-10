using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace TCG_Projects
{
    public partial class Notes : Form
    {
        public Notes()
        {
            InitializeComponent();
        }
        public static string username = "";
        public string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        private void Notes_Load(object sender, EventArgs e)
        {
            #region Public Properties

            MySqlConnection con = new MySqlConnection(ConString);

            try
            {
                con.Open();
                string CmdString = "SELECT N_Stamp, N_Note, Proj_Name FROM notes WHERE User_Name = '" + LoginForm.username + "' AND Proj_Name = '" + Main.project_names + "'";

                MySqlDataAdapter sda = new MySqlDataAdapter(CmdString, con);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("DEBUG: Can NOT open connection! Due to: Connection Error");
                Application.Exit();
            }

            #endregion
            label1.Text = Main.stampClock;
        }

        private void btnAddNote_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {
                
                con.Open();
                string query = "INSERT INTO notes (N_Note, User_Name, Proj_Name, N_Stamp) VALUES (@Note, @User, @Proj, @Stamp)";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Note", txtAddNote.Text);
                    cmd.Parameters.AddWithValue("@User", LoginForm.username);
                    cmd.Parameters.AddWithValue("@Proj", Main.project_names);
                    cmd.Parameters.AddWithValue("@Stamp", Main.stampClock);
                    
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Added");
                }
                con.Close();

                Notes_Load(null, null);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Adding Note :(" + ex);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Notes_Load(null, null);
        }
    }
}
