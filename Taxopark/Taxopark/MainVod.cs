using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;

namespace Taxopark
{
    public partial class MainVod : Form
    {
        int check = 0;

        int rowIndex = 0;

        public MainVod()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData() //Заполнение dataGridView1
        {
            DB db = new DB();
            db.openConnection();
            string query = "SELECT DataTime_Call,Telephone_Call,Otkuda,Kuda FROM 19055_taxopark.call where Accepted is null;";
            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
            }

            

            reader.Close();

            db.closeConnection();

            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Выборка номера телефона из DataGridView
            string Telephone_Call = "";

            Telephone_Call = dataGridView1[1, rowIndex].Value.ToString();

            //label2.Text = Telephone_Call;
            DateTime Accepted_DataTime = new DateTime();
            Accepted_DataTime = DateTime.UtcNow;
            
            DB db = new DB();
            MySqlCommand command = new MySqlCommand("UPDATE `call` SET `Accepted` = 1, `Accepted_DataTime`= @Accepted_DataTime WHERE `Telephone_Call` = @Telephone_Call;", db.getConnection());
            command.Parameters.Add("@Accepted_DataTime", MySqlDbType.DateTime).Value = Accepted_DataTime; 
            command.Parameters.Add("@Telephone_Call", MySqlDbType.VarChar).Value = Telephone_Call;
            db.openConnection();
            command.ExecuteNonQuery();
            //if (command.ExecuteNonQuery() == 1)
            //{
            //    MessageBox.Show("Ништяк");
            //}
            db.closeConnection();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)//Установка галочки напротив строки
        {
            getCheckBox();
        }
        private void getCheckBox()
        {
            DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
            ch1 = (DataGridViewCheckBoxCell)dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4];
            if (ch1.Value == null)
                ch1.Value = false;

            switch (ch1.Value.ToString())
            {
                case "True":
                    ch1.Value = false;
                    check--;
                    break;
                case "False":
                    check++;
                    ch1.Value = true;
                    //MessageBox.Show("ебаная ячейка");
                    if (check >= 2)
                    {
                        MessageBox.Show("Вы выбрали больше одного заказа");
                        ch1.Value = null;
                        check--;
                    }
                    rowIndex = ch1.RowIndex;
                    break;
            }
        }
        private void MainVod_Load(object sender, EventArgs e)
        {

        }
    }
}
