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

namespace Taxopark
{
    public partial class Regisr : Form
    {
        public Regisr()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Avtoriz avt = new Avtoriz();
            avt.Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {

                string fioUser = textBox1.Text;
                string phoneUser = textBox2.Text;
                string passwordUser = textBox3.Text;
                string passwordUser2 = textBox4.Text;

                if (passwordUser == passwordUser2)
                {
                    if (isUserExist())
                        return;

                    DB db = new DB();
                    MySqlCommand command = new MySqlCommand("insert into `сlient` (`FIO_Сlient`,`Telephone_Сlient`,`Password_Сlient`) values (@fio,@phone,@password)", db.getConnection());
                    command.Parameters.Add("@fio", MySqlDbType.VarChar).Value = fioUser;
                    command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phoneUser;
                    command.Parameters.Add("@password", MySqlDbType.VarChar).Value = passwordUser;

                    db.openConnection();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        Avtoriz avt = new Avtoriz();
                        avt.Show();
                        Close();
                    }
                    else MessageBox.Show("Аккаунт не был создан");

                    db.closeConnection();
                }
                else MessageBox.Show("Не совпадают пароли");
            }

        }
        public Boolean isUserExist()
        {
            string phoneUser = textBox2.Text;


            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `сlient` WHERE `Telephone_Сlient`=@phone", db.getConnection());
            command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phoneUser;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Такой пользоваель уже есть");
                return true;
            }
            else
                return false;
        }

    }
}
