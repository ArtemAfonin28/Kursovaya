using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;

namespace Taxopark
{
    public partial class AvtorizVod : Form
    {
        public AvtorizVod()
        {
            InitializeComponent();
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string phoneDriver = textBox1.Text;
            string passwordDriver = textBox2.Text;

            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `Drivers` WHERE `Telephone_Drivers`=@uL AND `Password_Drivers`=md5(@uP)", db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = phoneDriver;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passwordDriver;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)                      
            {
                MainVod mainVod = new MainVod();
                mainVod.idDriver = Convert.ToInt32(table.Rows[0][0]);
                mainVod.Show();
                Close();
            }
            else 
            MessageBox.Show("Такого пользователя не существует");

        }

        private void label10_Click(object sender, EventArgs e)
        {
            Avtoriz avt = new Avtoriz();
            avt.Show();
            Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Avtoriz avt = new Avtoriz();
            avt.Show();
            Close();
        }

        public string GetHashMD5(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AvtorizVod_Load(object sender, EventArgs e)
        {

        }
    }
}

