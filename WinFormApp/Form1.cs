using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;




namespace WindowsFormsApp15
{
    public partial class Form1 : Form
    {


        public Form1()
        {


            string path = @"C:\LazyPull\FeedPull\Username.txt";
            string path2 = @"C:\LazyPull\FeedPull\Password.txt";//path to resource file location
            InitializeComponent();

            if (File.Exists(path) == false && File.Exists(path2) == false)
            {

                Form Form2 = new Form2();
                textBox1.Visible = false;
                textBox2.Visible = false;
                button1.Visible = false;
                label1.Visible = false;
                Form2.Show();
            }
            
        }
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {

            string value = "";
            comboBox1.Invoke(new MethodInvoker(delegate
            {
                if (comboBox1.SelectedIndex != -1)
                {
                    value = comboBox1.SelectedItem.ToString();
                }
            }));
            if (value == "Vendor1")
            {
                label2.Visible = true;
                comboBox2.Show();
            }
            else if (value == "Vendor2")
            {
                label2.Visible = false;
                comboBox2.Hide();
            }
            else if (value == "Vendor3")
            {
                label2.Visible = false;
                comboBox2.Hide();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Show();
            backgroundWorker1.RunWorkerAsync();

        }


        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void resetCredentialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Form2 = new Form2();
            textBox1.Visible = false;
            textBox2.Visible = false;
            button1.Visible = false;
            label1.Visible = false;
            Form2.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {


                OracleConnection cnn;
                string sql = null;
                string data = null;

                int i = 0;
                int j = 0;

                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


                Excel.Workbook MyBook = null;
                Excel.Application MyApp = null;
                Excel.Worksheet MySheet = null;
                MyApp = new Excel.Application();
                //MyBook = MyApp.Workbooks.Add(misValue);
                MyBook = MyApp.Workbooks.Open(@"C:\LazyPull\FeedPull\Raw_Feed_Template_V4.xlsm");
                MySheet = (Excel.Worksheet)MyBook.Worksheets.get_Item(2);
                MySheet.Cells.ClearContents();
                MySheet.Columns.NumberFormat = "General";
                //string path = @"C:\LazyPull\FeedPull\Username.txt";
                //string path2 = @"C:\LazyPull\FeedPull\Password.txt";
                string Username = System.IO.File.ReadAllLines(@"C:\LazyPull\FeedPull\Username.txt").First();
                string Password = System.IO.File.ReadAllLines(@"C:\LazyPull\FeedPull\Password.txt").First();
                string connectionString = $"Data Source= (DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = mcadb01.markit.partners)(PORT = 1521)) (CONNECT_DATA = (SERVICE_NAME = MCASERV.markit.partners))));User Id={Username};Password={Password}";
                cnn = new OracleConnection(connectionString);
                cnn.Open();

                string caid = textBox2.Text;

                try
                {
                    string value = "";
                    comboBox1.Invoke(new MethodInvoker(delegate {
                        if (comboBox1.SelectedIndex != -1)
                        {
                            value = comboBox1.SelectedItem.ToString();
                        }
                    }));
                    if (value == "EDI")
                    {
                        textBox1.Text = @"select * from MCAtable1
";
                    }
                    else if (value == "IDC")
                    {
                        textBox1.Text = @" select * from MCAtable2";
                    }
                    else
                        throw new NotImplementedException();
                }   
                
                catch (NotImplementedException)
                {
                    MessageBox.Show("Please select vendor name");
                    
                }

                sql = textBox1.Text;

                OracleDataAdapter dscmd = new OracleDataAdapter(sql, cnn);
                DataSet ds = new DataSet();
                DataSet dss = new DataSet();
                OracleCommand cmd = new OracleCommand(sql, cnn);
                cmd.BindByName = true;
                OracleDataReader reader = cmd.ExecuteReader();
                DataTable schemaTable = reader.GetSchemaTable();
                dscmd.Fill(ds);

                int ii = 0;
                for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)

                {

                    DataRow row = schemaTable.Rows[ii];
                  // xlWorkSheet.Cells[i + 1, j + 1] = row["columnName"];
                    MySheet.Cells[i + 1, j + 1] = row["columnName"];
                    ii = ii + 1;

                }

                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {

                    for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                    {

                        data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
                       // xlWorkSheet.Cells[i + 2, j + 1] = data;
                        MySheet.Cells[i + 2, j + 1] = data;
                    }

                }
               if (MySheet.Cells[2,2].value  == null)
               {
                   xlWorkBook.Close(false, misValue, misValue);
                xlApp.Quit();
                 MyBook.Close(false, misValue, misValue);
                  MyApp.Quit();
                 releaseObject(MySheet);
                  releaseObject(MyBook);
                releaseObject(MyApp);
                 MessageBox.Show("please pick a valid VNDR_NTC_ID");
                 return;
                }
                MyApp.Visible = true;
                MyApp.Run("Dist");

               // xlWorkBook.SaveAs("Raw_Feed_Pull.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);


               // MessageBox.Show("Excel file created , you can find the file c:\\Raw_Feed_Pull.xls");

                releaseObject(MySheet);
                releaseObject(MyBook);
                releaseObject(MyApp);




            }
            catch (InvalidOperationException)
            {
                { };
            }

            catch (OracleException)
            {
                MessageBox.Show("Please Input Valid Credentials");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }

            finally
            {

                GC.Collect();
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Hide();
        }


    }

   }

    
   
 
