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
    public partial class LoginForm : Form
    {
        public string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        private void LoginForm_Load(object sender, EventArgs e)
        {
            #region Public Properties

            MySqlConnection con = new MySqlConnection(ConString);

            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can NOT open connection! Due to: Connection Error");
                Application.Exit();
            }

            #endregion
        }

        public LoginForm()
        {
            InitializeComponent();
        }
        public static string username = "";
        
        

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {
                con.Open();
                
                string CmdString = "SELECT COUNT(User_Name) FROM users WHERE User_Name = '" + txtUser.Text + "' AND User_Pass = '" + txtPass.Text + "' AND User_Act = 'true'";

                MySqlDataAdapter sda = new MySqlDataAdapter(CmdString, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows[0][0].ToString() =="1")
                {
                    
                    this.Hide();


                    username = txtUser.Text;

                    Main mn = new Main();
                    mn.ShowDialog();
                } else
                {
                    MessageBox.Show("Whoops, didn't quite recognise you...");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                {
                MySqlConnection con = new MySqlConnection(ConString);
                try
                {
                    con.Open();

                    string CmdString = "SELECT COUNT(User_Name) FROM users WHERE User_Name = '" + txtUser.Text + "' AND User_Pass = '" + txtPass.Text + "' AND User_Act = 'true'";

                    MySqlDataAdapter sda = new MySqlDataAdapter(CmdString, con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows[0][0].ToString() == "1")
                    {

                        this.Hide();


                        username = txtUser.Text;

                        Main mn = new Main();
                        mn.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Whoops, didn't quite recognise you...");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
