using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarsDatabase
{
    public partial class frmSearch : Form
    {
        public frmSearch()
        {
            InitializeComponent();
            this.Text = $"Task A Search Nathan {Convert.ToString(DateTime.Today).Remove(10)}";
        }

        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        private void frmSearch_Load(object sender, EventArgs e)
        {
            cboField.Items.Add("VehicleRegNo");
            cboField.Items.Add("Make"); 
            cboField.Items.Add("EngineSize");
            cboField.Items.Add("RentalPerDay ");
            cboField.Items.Add("Available");

            cboOperator.Items.Add("=");
            cboOperator.Items.Add("<");
            cboOperator.Items.Add(">");
            cboOperator.Items.Add("<=");
            cboOperator.Items.Add(">=");
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string field, sOperator, value;

            field = cboField.Text;           
            sOperator = cboOperator.Text;
            value = txtBoxValue.Text;

            //Changing the available value field to accept Yes and No values
            if (field == "Available" && value == "Yes")
            {
                value = true.ToString();
            }
            else if (field == "Available" && value == "No")
            {
                value = false.ToString();
            }
            
            SqlConnection sqlCon = new SqlConnection(myconnstring);
            DataTable dataTable = new DataTable();

            try
            {
                string sqlQuery = "SELECT * FROM tblCar WHERE " + field + sOperator + "@Value";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlCon);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                sqlCon.Open();
                cmd.Parameters.AddWithValue("@Value", value);

                adapter.Fill(dataTable);

                //if satement for preventing unnecessary result errors from displaying
                if (field == "VehicleRegNo" && sOperator != "=" || field == "Make" && sOperator != "=" || field == "Available" && sOperator != "=")
                {
                    MessageBox.Show($"You can't use the {field} field with the {sOperator} operator","Invalid Input",MessageBoxButtons.OK);
                    dataTable.Clear();
                }

                dgvCars.DataSource = dataTable;

                //Changing the display format of the date registered and rental per day displayed
                dgvCars.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvCars.Columns[4].DefaultCellStyle.Format = "C";
                dgvCars.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.Message);

            }
            finally
            {
                sqlCon.Close();
            }          
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            frmCars cars = new frmCars();
            cars.Show();
            this.Hide();
        }
    }
}
