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
using TheArtOfDevHtmlRenderer.Adapters;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Taxopark
{
    public partial class Main : Form
    {
        public string userName;
        public string phoneUser;
        string deliverFrom;//От
        string deliverTo;//До
        public Main()
        {
            InitializeComponent();
        }
        private void Button4_Click(object sender, EventArgs e)// Заказ такси, занесение данных в бд
        {
            if (haveCall() == true)
            {
                MessageBox.Show("Вы уже закали такси");
                openInfoYourCall();
            }
            else
            {
                DateTime dateTime = new DateTime();
                dateTime = DateTime.UtcNow;

                if (TextBox1.Text == "" || TextBox2.Text == "")
                {
                    MessageBox.Show("Заполните все поля");
                }
                else
                {
                    string phoneCall = phoneUser;
                    string otkuda = TextBox1.Text;
                    string kuda = TextBox2.Text;

                    DB db = new DB();
                    MySqlCommand command = new MySqlCommand("insert into `Call` (`DataTime_Call`,`Telephone_Call`,`Otkuda`,`Kuda`) " +
                        "values (@dateTime,@phoneCall,@otkuda,@kuda)", db.getConnection());
                    command.Parameters.Add("@dateTime", MySqlDbType.DateTime).Value = dateTime;
                    command.Parameters.Add("@phoneCall", MySqlDbType.VarChar).Value = phoneCall;
                    command.Parameters.Add("@Otkuda", MySqlDbType.VarChar).Value = otkuda;
                    command.Parameters.Add("@Kuda", MySqlDbType.VarChar).Value = kuda;

                    db.openConnection();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы успешно заказали такси");
                        openInfoYourCall();
                    }
                    else MessageBox.Show("Уже есть");
                    db.closeConnection();
                }
            }


        }

        private void Main_Load(object sender, EventArgs e)
        {
            label1.Text = userName;
            if (haveCall() == true)
            {
                MessageBox.Show("Вы уже закали такси");
                openInfoYourCall();
            }
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Avtoriz avt = new Avtoriz();
            avt.Show();
            Close();
        }
        private bool haveCall() //проверка на наличие заказа у клиента
        {
            bool bollean;
            string phoneCall = phoneUser;

            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `Call` WHERE `Telephone_Call`=@uL", db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = phoneCall;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                bollean = true;

                //Даныне откуда и куда
                deliverFrom = table.Rows[0][3].ToString();
                deliverTo = table.Rows[0][4].ToString();
                //инфа о вызове
            }
            else
            {
                bollean = false;
            }
            return bollean;
            
            

        }


        private void openInfoYourCall()
        {
            InfoYourCall info = new InfoYourCall();

            info.userName = userName;
            info.phoneUser = phoneUser;

            info.deliverFrom = deliverFrom;
            info.deliverTo = deliverTo;

            info.Show();
            Close();
        }
    }
}