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

        public string driver, avto;

        public string deliverFrom, deliverTo;//От-До

        public string alert, accepted;
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

        private void InfoYourCall_Load(object sender, EventArgs e)
        {
            label1.Text = userName;
            fillTable();
            infoLabel4();
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
            label4.Text += "\n";
            label4.Text += "\n";
            label4.Text += deliverFrom + "\n";
            label4.Text += deliverTo;
        }
    }
}
