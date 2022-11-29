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

        public string driver = "", avto = "", numberAvto = "";

        public string deliverFrom, deliverTo;//От-До и номер машины

        public int alert = 0, accepted = 0;
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
            
            DB db = new DB();
            MySqlCommand command = new MySqlCommand("DELETE FROM `Call` WHERE `Telephone_Call`=@phoneCall;", db.getConnection());
            command.Parameters.Add("@phoneCall", MySqlDbType.VarChar).Value = phoneUser;
            db.openConnection();
            command.ExecuteNonQuery();
            db.closeConnection();
            MessageBox.Show("Вы отменили свой заказ");
            Avtoriz avt = new Avtoriz();
            avt.Show();
            Close();
        }

        private void update_Tick(object sender, EventArgs e)
        {
            DB db = new DB();

            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT `FIO_Drivers`,`Marka_Car`,`Register_Number` FROM `Call`,`Drivers`,`Сar` " +
                "WHERE(`Drivers_Id_Drivers`=`Id_Drivers`) AND(`Сar_Id_Сar`=`Id_Сar`) AND (`Accepted`= 1) AND(`Telephone_Call`=@phoneUser); ; ", db.getConnection());
            command.Parameters.Add("@phoneUser", MySqlDbType.VarChar).Value = phoneUser;

            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                label2.Text = "";
                label2.Text += table.Rows[0][0].ToString();
                label2.Text += table.Rows[0][1].ToString();
                label2.Text += table.Rows[0][2].ToString();


            }

        }

        private void InfoYourCall_Load(object sender, EventArgs e)
        {
            label1.Text = userName;
            label4.Text = "";
            label4.Text += "Отсутствует"+"\n";
            label4.Text += "Отсутствует" + "\n";
            label4.Text += "Отсутствует" + "\n";
            label4.Text += deliverFrom + "\n";
            label4.Text += deliverTo;

            update.Enabled = true;
        }

        private void data()
        {

        }



 



    }
}
