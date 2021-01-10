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
using System.Diagnostics;
using System.Globalization;

namespace TCG_Projects
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        public static string project_names = "";
        public static string stampClock = "";
        public string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        private void btnExit_Click(object sender, EventArgs e)
        {
            const string message = "Confirm Exit?";
            const string caption = "EXITING APPLICATION";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {

                txtUsername.Text = LoginForm.username;
                string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

                MySqlConnection con = new MySqlConnection(ConString);
                string CmdString = "SELECT COUNT(*) FROM Users WHERE User_Name = '" + LoginForm.username + "' AND User_Level = 'admin'";

                MySqlDataAdapter sda = new MySqlDataAdapter(CmdString, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows[0][0].ToString() == "1")
                {
                    con.Close();
                    txtUsername.Text = "Welcome , " + LoginForm.username + "! -- Admin";

                    //SHOW OPTIONS HERE
                    btnOptions.Show();                
                }
                else
                {
                    txtUsername.Text = "Welcome , " + LoginForm.username + "!";
                    btnOptions.Hide();
                }

                try
                {
                    string ConStrings = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

                    MySqlConnection cons = new MySqlConnection(ConStrings);
                    string CmdStrings = "SELECT Proj_Name from projects Where User_Name = '" + LoginForm.username + "'";

                    MySqlDataAdapter da = new MySqlDataAdapter(CmdStrings, cons);

                    DataSet dset = new DataSet();
                    da.Fill(dset, "Projects");
                    cmbProject.DataSource = dset.Tables[0];
                    
                    cmbProject.ValueMember = "Proj_Name";
                    cmbProject.Enabled = true;
                    cmbProject.SelectedIndex = -1;
                    cmbProject.SelectedItem = "None";
                    cmbProject.SelectedText = "None";
                    cons.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }




            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                
            }
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            AdminUpdate admu = new AdminUpdate();
            admu.ShowDialog();
            //Opens Admin Window
        }
        private DateTime _start = DateTime.Now;
        private DateTime _stop;
        private string _date = DateTime.Now.ToShortDateString();
        private string _elapsedTime;
        private void stopTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan duration = DateTime.Now - _start;
            lblClock.Text = duration.ToString(@"hh\:mm\:ss");
            _elapsedTime = duration.ToString(@"hh\:mm\:ss");
        }

        
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cmbProject.Text == "None" || cmbProject.Text == "")
            {
                MessageBox.Show("No project selected");
            }
            else
            {
                lblClock.ForeColor = Color.FromArgb(0, 255, 0);//Changes timer font to green
                _start = DateTime.Now;                
                stopTimer.Start();//Timer starts
            }
            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {

            MySqlConnection con = new MySqlConnection(ConString);
            try
            {
                lblClock.ForeColor = Color.FromArgb(255, 0, 0);//Changes timer font to red
                stopTimer.Stop();//Timer stops
                stampClock = lblClock.Text;//Creates a timestamp of the current time
                lblClientHead.Text = lblClock.Text;//Sets the client label as clock time -- DEBUG
                Notes nts = new Notes();//Define the notes form
                nts.ShowDialog();//Notes window opens as timer stops

                _stop = DateTime.Now;
                con.Open();
                string query = "INSERT INTO stopwatch (Proj_Name, User_Name, Watch_Date, Watch_Start, Watch_Stop, duration) VALUES (@W_Proj, @User, @W_Date, @Start, @Stop, @Duration)";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@User", LoginForm.username);
                    cmd.Parameters.AddWithValue("@W_proj", project_names);
                    cmd.Parameters.AddWithValue("@W_Date", _date);
                    cmd.Parameters.AddWithValue("@Start", _start);
                    cmd.Parameters.AddWithValue("@Stop", _stop);
                    cmd.Parameters.AddWithValue("@Duration", _elapsedTime);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Added");
                }
                con.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Adding Note :(");
            }
        }


        private void btnNotes_Click(object sender, EventArgs e)
        {
            stampClock = lblClock.Text;//Creates a timestamp of the current time
            lblClientHead.Text = lblClock.Text;//Sets the client label as clock time -- DEBUG
            Notes nts = new Notes();
            nts.ShowDialog();
        }


        private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ConStrings = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

                
                string CmdStrings = "SELECT Company_Name from projects Where Proj_Name = '" + cmbProject.Text + "'";
                string test = cmbProject.Text;
                if(test != "None" || test !="")
                {
                    using (MySqlConnection connection =
                   new MySqlConnection(ConStrings))
                    {
                        MySqlCommand command =
                            new MySqlCommand(CmdStrings, connection);
                        connection.Open();

                        MySqlDataReader reader = command.ExecuteReader();

                        // Call Read before accessing data.
                        while (reader.Read())
                        {
                            lblClient.Text = reader["Company_Name"].ToString();
                        }

                        // Call Close when done reading.
                        reader.Close();
                        connection.Close();
                        project_names = cmbProject.Text;
                        tasksGet();
                    }
                } else
                {
                    lblClient.Text = "No project chosen";
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void tasksGet()
        {
            string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            MySqlConnection con = new MySqlConnection(ConString);

            try
            {
                con.Open();
                string CmdString = "SELECT T_Todo, Proj_Name, User_Name FROM tasks WHERE User_Name = '" + LoginForm.username + "' AND Proj_Name = '" + cmbProject.Text + "' AND T_Status = 'true'";
                MySqlDataAdapter sqlA = new MySqlDataAdapter(CmdString, con);
                DataSet newSet = new DataSet();
                sqlA.Fill(newSet);
                dgvTaskView.DataSource = newSet.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("DEBUG: Can NOT open connection! Due to: Connection Error");
                Application.Exit();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.Show();
            this.Close();
        }
    }
}
