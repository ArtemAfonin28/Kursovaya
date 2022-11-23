using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
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
        string alertsCall; //приехал ли водитель
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
                alertsCall = table.Rows[0][7].ToString();
                bollean = true;
                timer1.Enabled = true;

                //Даныне откуда и куда
                deliverFrom = table.Rows[0][3].ToString();
                deliverTo = table.Rows[0][4].ToString();
                //инфа о вызове
            }
            else
            {
                alertsCall = "";
                bollean = false;
            }
            return bollean;
            
            

        }

        private void timer1_Tick(object sender, EventArgs e)//таймер отслеживающий приехал ли водитель
        {
            if (alertsCall == "1")
            {
                MessageBox.Show("Водитель ожидает вас на месте");
                timer1.Enabled = false;
            }
        }
        private void infoYourCall()
        {
            //label4.Visible = true;
            //label4.Text = "";
            ////label4.Location = new Point(12, 245);
            //label4.Size = new Size(585, 149);
            //label4.Left = 12;
            //label4.Top = 245;
            //label4.Text +=  "Информация о вашем вызове:" + "\n";
            //label4.Text += deliverFrom + " ----------> "+ deliverTo+"\n";
            //if (alertsCall == "1")
            //{
            //    label4.Text += "Водитель уже ожидает вас";
            //}
        }
    }
}