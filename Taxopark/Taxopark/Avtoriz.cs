using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Taxopark
{
    public partial class Avtoriz : Form
    {
        public string localUserName;
        public Avtoriz()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string phoneUser = textBox1.Text;
            string passwordUser = textBox2.Text;
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `Сlient` WHERE `Telephone_Сlient`=@uL AND `Password_Сlient`=@uP", db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = phoneUser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passwordUser;
            adapter.SelectCommand = command;
            adapter.Fill(table);



            if (table.Rows.Count > 0)
            {
                Main main = new Main();
                main.userName = table.Rows[0][1].ToString();
                main.phoneUser = phoneUser;
                main.Show();
                Hide();
            }
            else
                MessageBox.Show("Такого пользователя не существует");
           
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Regisr reg = new Regisr();
            reg.Show();
            Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Regisr reg = new Regisr();
            reg.Show();
            Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            AvtorizVod avtVod = new AvtorizVod();
            avtVod.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainVod mainVod = new MainVod();
            mainVod.Show();
            Hide();
        }
    }
}
