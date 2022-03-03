using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProfileCRUD
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Authenticte1;Integrated Security=True;");

        public void DisplayData()
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from [ProfileDetails]";
            cmd.ExecuteNonQuery();
            DataTable dta = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dta);
            dataGrid_Display.DataSource = dta;
            conn.Close();
        }

        public Form1()
        {
            InitializeComponent();
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'authenticte1DataSet.ProfileDetails' table. You can move, or remove it, as needed.
            //this.profileDetailsTableAdapter.Fill(this.authenticte1DataSet.ProfileDetails);

        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            conn.Open();

            SqlCommand sqcmd = new SqlCommand("Select * from [ProfileDetails] where RegID='" + txt_reg.Text + "'", conn);

            SqlDataAdapter da = new SqlDataAdapter(sqcmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if(dt.Rows.Count > 0)
            {
                conn.Close();
                MessageBox.Show("Reg ID already exists");
            }
            else
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Insert into [ProfileDetails] (RegID, FirstName, LastName, Address) values ('" + txt_reg.Text + "','" + txt_FirstName.Text + "','" + txt_LastName.Text + "','" + txt_Address.Text + " ')";
                cmd.ExecuteNonQuery();
                conn.Close();

                txt_reg.Text = "";
                txt_FirstName.Text = "";
                txt_LastName.Text = "";
                txt_Address.Text = "";

                DisplayData();
                MessageBox.Show("Data successfully inserted.");
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update [ProfileDetails] set FirstName='" + txt_FirstName.Text + "', LastName='" + txt_LastName.Text + "', Address='" + txt_Address.Text + "' where RegID='" + txt_reg.Text + "'";

            cmd.ExecuteNonQuery();
            conn.Close();
            DisplayData();

            txt_reg.Text = "";
            txt_FirstName.Text = "";
            txt_LastName.Text = "";
            txt_Address.Text = "";

            btn_Insert.Enabled = true;
            btn_Update.Enabled = false;
            btn_Delete.Enabled = false;

            MessageBox.Show("Data updated");
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Delete from [ProfileDetails] where RegID='" + txt_reg.Text + "'";

            cmd.ExecuteNonQuery();
            conn.Close();
            DisplayData();

            txt_reg.Text = "";
            txt_FirstName.Text = "";
            txt_LastName.Text = "";
            txt_Address.Text = "";

            btn_Insert.Enabled = true;
            btn_Update.Enabled = false;
            btn_Delete.Enabled = false;

            txt_reg.Enabled = true;

            MessageBox.Show("Data deleted.");
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            conn.Open();
            string search = txt_Search.Text;
            SqlCommand sqcmd = new SqlCommand("Select * from [ProfileDetails] where FirstName Like '%" + search + "%' OR LastName Like '%" + search + "%' OR Address Like '%" + search + "%'", conn);

            SqlDataAdapter da = new SqlDataAdapter(sqcmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if(dt.Rows.Count > 0)
            {
                dataGrid_Display.DataSource = dt;
                conn.Close();
            }
            else
            {
                MessageBox.Show("No record found.");
                conn.Close();
            }
        }

        private void btn_DisplayAll_Click(object sender, EventArgs e)
        {
            DisplayData();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            conn.Close();
            DisplayData();

            txt_reg.Text = "";
            txt_FirstName.Text = "";
            txt_LastName.Text = "";
            txt_Address.Text = "";

            btn_Insert.Enabled = true;
            btn_Update.Enabled = false;
            btn_Delete.Enabled = false;

            txt_reg.Enabled = true;
        }

        private void dataGrid_Display_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            DataGridViewRow row = dataGrid_Display.Rows[i];

            string reg = row.Cells[0].Value.ToString();
            string value1 = row.Cells[1].Value.ToString();
            string value2 = row.Cells[2].Value.ToString();
            string value3 = row.Cells[3].Value.ToString();

            txt_reg.Text = reg;
            txt_reg.Enabled = false;

            txt_FirstName.Text = value1;
            txt_LastName.Text = value2;
            txt_Address.Text = value3;

            btn_Insert.Enabled = false;
            btn_Update.Enabled = true;
            btn_Delete.Enabled = true;
        }
    }
}
