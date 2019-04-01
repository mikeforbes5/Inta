using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp15
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
            
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
                string path = @"C:\LazyPull\FeedPull\Username.txt";
                string path2 = @"C:\LazyPull\FeedPull\Password.txt";//path to resource file location
                                                                                        // Create a file to write to.                                                                                    // Create a file to write to.                                                                
                using (StreamWriter writer = File.CreateText(path))
                {
                    string line = textBox1.Text;
                    writer.WriteLine(line);
  
                }
                using (StreamWriter writer2 = File.CreateText(path2))
                {
                    string line2 = textBox2.Text;
                    writer2.WriteLine(line2);
                    writer2.Close();
                }

                this.Close();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            TextBox tb = (TextBox)f1.Controls["textbox1"];
            TextBox tb2 = (TextBox)f1.Controls["textbox2"];
            Label lbl = (Label)f1.Controls["label1"];
            Button btn = (Button)f1.Controls["button1"];
            tb.Visible = false;
            tb2.Visible = true;
            lbl.Visible = true;
            btn.Visible = true;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            TextBox tb = (TextBox)f1.Controls["textbox1"];
            TextBox tb2 = (TextBox)f1.Controls["textbox2"];
            Label lbl = (Label)f1.Controls["label1"];
            Button btn = (Button)f1.Controls["button1"];
            tb.Visible = false;
            tb2.Visible = true;
            lbl.Visible = true;
            btn.Visible = true;
        }
    }
    
}
