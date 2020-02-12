using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace WPF_AccessDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con;
        DataTable dt;

        public MainWindow()
        {
            InitializeComponent();

            //Connect your access database
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\GameDB.mdb";
            BindGrid();
        }

        //Display records in grid
        private void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from tbGame";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lblCount.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblCount.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }

        }

        //Add records in grid
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtGameId.Text != "")
            {
                if (txtGameId.IsEnabled == true)
                {
                    if (txtGameName.Text != "Select Game")
                    {
                        cmd.CommandText = "insert into tbGame(GameId,GameName,Rating) Values(" + txtGameId.Text + ",'" + txtGameName.Text + "'," + txtRating.Text; 
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        MessageBox.Show("Game Added Successfully...");
                        ClearAll();

                    }
                    else
                    {
                        MessageBox.Show("Please Select Gender Option....");
                    }
                }
                else
                {
                    cmd.CommandText = "update tbGame set GameName='" + txtGameName.Text + "',Rating=" + txtRating.Text;
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Employee Details Updated Succesffully...");
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Please Add Employee Id.......");
            }
        }

        //Clear all records from controls
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            txtGameId.Text = "";
            txtGameName.Text = "";
           
            
            txtRating.Text = "";
            btnAdd.Content = "Add";
            txtGameId.IsEnabled = true;
        }

        //Edit records
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                txtGameId.Text = row["GameId"].ToString();
                txtGameName.Text = row["GameName"].ToString();
                
                
                txtRating.Text = row["Rating"].ToString();
                txtGameId.IsEnabled = false;
                btnAdd.Content = "Update";
            }
            else
            {
                MessageBox.Show("Please Select Any Employee From List...");
            }
        }

        //Delete records from grid
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from tbEmp where EmpId=" + row["EmpId"].ToString();
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Employee Deleted Successfully...");
                ClearAll();
            }
            else
            {
                MessageBox.Show("Please Select Any Employee From List...");
            }
        }

        //Exit
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}