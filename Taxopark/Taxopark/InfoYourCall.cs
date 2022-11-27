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
    public partial class InfoYourCall : Form
    {
        public string userName, phoneUser;

        public string driver = "", avto = "";

        public string deliverFrom, deliverTo;//От-До

        public string alert, accepted = "";
        public InfoYourCall()
        {
            InitializeComponent();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Avtoriz avt = new Avtoriz();
            avt.Show();
            Close();
        }

        private void button3_Click(object sender, EventArgs e) //Удаление заказа
        {
            string phoneCall = phoneUser;

            DB db = new DB();
            MySqlCommand command = new MySqlCommand("DELETE FROM `call` WHERE `Telephone_Call`=@phoneCall;", db.getConnection());
            command.Parameters.Add("@phoneCall", MySqlDbType.VarChar).Value = phoneCall;
            db.openConnection();
            command.ExecuteNonQuery();
            db.closeConnection();
            MessageBox.Show("Вы отменили свой заказ");
            Avtoriz avt = new Avtoriz();
            avt.Show();
            Close();
        }

        private void InfoYourCall_Load(object sender, EventArgs e)
        {
            label1.Text = userName;
            fillTable();
            infoLabel4();
            checkAccept();
        }

        private void fillTable()
        {
            string phoneCall = phoneUser;

            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `Call` WHERE `Telephone_Call`=@uL", db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = phoneCall;

            adapter.SelectCommand = command;
            adapter.Fill(table);
            db.closeConnection();
            alert = (table.Rows[0][7]).ToString();
            accepted = (table.Rows[0][5]).ToString();
            infoLabel5();
        }
        private void infoLabel5()
        {
            if (accepted == "1")
            {
                if (alert == "1")
                {
                    label5.Text = "Ваш водитель на месте";
                }
                else label5.Text = "Ваш водитель в пути";
            }
            else label5.Text = "Ваш заказ еще никто не принял";

        }
        private void infoLabel4()
        {
            label4.Text = "";
            label4.Text += driver + "\n";
            label4.Text += avto + "\n";
            label4.Text += deliverFrom + "\n";
            label4.Text += deliverTo;
        }

        private void checkAccept()//кто принял заказ
        {
            if (accepted == "1")
            {
                string phoneCall = phoneUser;

                DB db = new DB();
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand command = new MySqlCommand("SELECT `Accepted`,`FIO_Drivers`,`Marka_Car` FROM `call`,`drivers`,`сar` WHERE " +
                    "(`Drivers_Id_Drivers`=`Id_Drivers`) and (`Сar_Id_Сar`=`Id_Сar`) and (`Telephone_Call` = @phoneCall) and (`Accepted` = 1);", db.getConnection());
                command.Parameters.Add("@phoneCall", MySqlDbType.VarChar).Value = phoneCall;

                adapter.SelectCommand = command;
                adapter.Fill(table);

                driver = (table.Rows[0][1]).ToString();
                avto = (table.Rows[0][2]).ToString();
                infoLabel4();
            }

        }
    }
}
