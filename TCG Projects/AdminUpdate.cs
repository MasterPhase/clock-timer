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
    public partial class AdminUpdate : Form
    {
        public AdminUpdate()
        {
            InitializeComponent();
        }
        public string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        private void AdminUpdate_Load(object sender, EventArgs e)
        {
            pnlClient.Hide();
            pnlProjects.Hide();
            pnlTasks.Hide();
            pnlUsers.Show();
        }

        private void btnUserAdmin_Click(object sender, EventArgs e)
        {
            //pnlUsers.BringToFront();

            pnlClient.Hide();
            pnlProjects.Hide();
            pnlTasks.Hide();
            pnlUsers.Show();
        }

        private void BtnClientsAdmin_Click(object sender, EventArgs e)
        {
            //pnlClient.BringToFront();

            pnlProjects.Hide();
            pnlTasks.Hide();
            pnlUsers.Hide();
            pnlClient.Show();
        }

        private void btnProjectsAdmin_Click(object sender, EventArgs e)
        {
            //pnlProjects.BringToFront();

            pnlClient.Hide();
            pnlTasks.Hide();
            pnlUsers.Hide();
            pnlProjects.Show();
        }

        private void btnTaskAdmin_Click(object sender, EventArgs e)
        {
            //pnlTasks.BringToFront();

            pnlClient.Hide();
            pnlProjects.Hide();
            pnlUsers.Hide();
            pnlTasks.Show();
        }

        private void btnAdminAddUser_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {

                con.Open();
                string query = "INSERT INTO users (User_Name, User_Level, User_Act, User_Pass) VALUES (@Name, @Level, @Active, @Pass)";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", txtAdminUserName.Text);
                    cmd.Parameters.AddWithValue("@Level", txtAdminUserPerm.Text);
                    cmd.Parameters.AddWithValue("@Active", cmbAdminUserStatus.Text);
                    cmd.Parameters.AddWithValue("@Pass", txtAdminUserPwd.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Added");
                }
                con.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Adding User :(");
            }
        }

        private void btnAdminAddClient_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {

                con.Open();
                string query = "INSERT INTO company (Comp_Name, Comp_Act) VALUES (@CName, @CActive)";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CName", txtAdminClientName.Text);
                    cmd.Parameters.AddWithValue("@CActive", cmbAdminClientStatus.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Added");
                }
                con.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Adding Company :(");
            }
        }

        private void button1_Click(object sender, EventArgs e)//projects add button
        {
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {

                con.Open();
                string query = "INSERT INTO projects (Proj_Name, Proj_Act, Company_Name, User_Name) VALUES (@PProject, @PActive, @PCompany, @PUser)";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@PProject", txtAdminProjName.Text);
                    cmd.Parameters.AddWithValue("@PUser", txtAdminProjLead.Text);
                    cmd.Parameters.AddWithValue("@PCompany", txtAdminProjClient.Text);
                    cmd.Parameters.AddWithValue("@PActive", cmbAdminProjStatus);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Added");
                }
                con.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Adding Project :(");
            }
        }

        private void btnAdminTaskAdd_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {

                con.Open();
                string query = "INSERT INTO tasks (T_Todo, T_Status, Proj_Name, User_Name) VALUES (@TTodo, @TStatus, @TProject, @TUser)";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TTodo", txtAdminTaskName.Text);
                    cmd.Parameters.AddWithValue("@TStatus", cmbAdminTaskStatus.Text);
                    cmd.Parameters.AddWithValue("@TProject", txtAdminProjName.Text);
                    cmd.Parameters.AddWithValue("@TUser", txtAdminTaskUser.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Added");
                }
                con.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Adding Task :(");
            }
        }

        private void btnAdminUserSearch_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {

                con.Open();

                string query = "SELECT * FROM users WHERE User_Name LIKE '%" + txtAdminUserSearch.Text + "%'";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader da = cmd.ExecuteReader();
                    while (da.Read())
                    {
                        txtAdminUserName.Text = da.GetValue(1).ToString();
                        txtAdminUserPerm.Text = da.GetValue(2).ToString();
                        txtAdminUserPwd.Text = da.GetValue(4).ToString();

                        try
                        {
                            string ConStrings = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

                            MySqlConnection cons = new MySqlConnection(ConStrings);
                            string CmdStrings = "SELECT U_ID, User_Name, User_Level, User_Act from users Where User_Name = '" + txtAdminUserName.Text + "'";

                            MySqlDataAdapter daCmb = new MySqlDataAdapter(CmdStrings, cons);

                            DataSet dset = new DataSet();
                            daCmb.Fill(dset, "users");
                            cmbAdminProjStatus.DataSource = dset.Tables[0];
                            dgvUsers.DataSource = dset.Tables[0];

                            cmbAdminProjStatus.ValueMember = "User_Act";
                            cmbAdminProjStatus.Enabled = true;
                            cmbAdminProjStatus.SelectedIndex = 0;
                            cons.Close();
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show("Combo Error");
                        }
                    }
                }


                con.Close();
                
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void btnUpdateAdmin_Click(object sender, EventArgs e)
        {

        }

        private void btnAdminProjSearch_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {

                con.Open();
                
                //string query = "SELECT * FROM projects WHERE Proj_Name = '" + txtAdminProjSearch.Text + "'";
                string query = "SELECT * FROM projects WHERE Proj_Name LIKE '%" + txtAdminProjSearch.Text + "%'";

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader da = cmd.ExecuteReader();
                while (da.Read())
                {
                    txtAdminProjName.Text = da.GetValue(1).ToString();
                    txtAdminProjClient.Text = da.GetValue(3).ToString();
                    txtAdminProjLead.Text = da.GetValue(4).ToString();

                    try
                    {
                        string ConStrings = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

                        MySqlConnection cons = new MySqlConnection(ConStrings);
                        string CmdStrings = "SELECT Proj_Name, Proj_Act, Company_Name, User_Name from projects Where Proj_Name = '" + txtAdminProjName.Text + "'";

                        MySqlDataAdapter daCmb = new MySqlDataAdapter(CmdStrings, cons);

                        DataSet dset = new DataSet();
                        daCmb.Fill(dset, "projects");
                        cmbAdminProjStatus.DataSource = dset.Tables[0];
                        dgvProject.DataSource = dset.Tables[0];

                        cmbAdminProjStatus.ValueMember = "Proj_Act";
                        cmbAdminProjStatus.Enabled = true;
                        cmbAdminProjStatus.SelectedIndex = 0;
                        cons.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Combo Error");
                    }

                }
                con.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Searching Project :(");
            }
        }

        private void btnAdminProjUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnAdminClientUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnAdminClientsUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnAdmintasksSearch_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {

                con.Open();

                //string query = "SELECT * FROM projects WHERE Proj_Name = '" + txtAdminProjSearch.Text + "'";
                string query = "SELECT * FROM tasks WHERE T_Todo LIKE '%" + txtAdminTaskSearch.Text + "%'";

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader da = cmd.ExecuteReader();
                while (da.Read())
                {
                    txtAdminTaskName.Text = da.GetValue(1).ToString();
                    txtAdminTaskProj.Text = da.GetValue(3).ToString();
                    txtAdminTaskUser.Text = da.GetValue(4).ToString();

                    try
                    {
                        string ConStrings = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

                        MySqlConnection cons = new MySqlConnection(ConStrings);
                        string CmdStrings = "SELECT User_Name, Proj_Name, T_Todo, T_Status from tasks Where T_Todo = '" + txtAdminTaskName.Text + "'";

                        MySqlDataAdapter daCmb = new MySqlDataAdapter(CmdStrings, cons);

                        DataSet dset = new DataSet();
                        daCmb.Fill(dset, "projects");
                        cmbAdminProjStatus.DataSource = dset.Tables[0];
                        dgvTasks.DataSource = dset.Tables[0];

                        cmbAdminTaskStatus.ValueMember = "T_Status";
                        cmbAdminTaskStatus.Enabled = true;
                        cmbAdminTaskStatus.SelectedIndex = 0;
                        cons.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Combo Error");
                    }
                }
                con.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Searching Task :(");
            }
        }

        private void btnAdminClientsSearch_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {

                con.Open();

                //string query = "SELECT * FROM projects WHERE Proj_Name = '" + txtAdminProjSearch.Text + "'";
                string query = "SELECT * FROM tasks WHERE T_Todo LIKE '%" + txtAdminTaskSearch.Text + "%'";

                MySqlCommand cmd = new MySqlCommand(query, con);

                MySqlDataAdapter daTbl = new MySqlDataAdapter(query, con);

                DataSet dset = new DataSet();
                daTbl.Fill(dset, "Clients");
                cmbAdminProjStatus.DataSource = dset.Tables[0];
                dgvClients.DataSource = dset.Tables[0];

                MySqlDataReader da = cmd.ExecuteReader();
                while (da.Read())
                {
                    txtAdminTaskName.Text = da.GetValue(1).ToString();
                    txtAdminTaskProj.Text = da.GetValue(3).ToString();
                    txtAdminTaskUser.Text = da.GetValue(4).ToString();
                }
                con.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Searching Task :(");
            }
        }
    }
}
