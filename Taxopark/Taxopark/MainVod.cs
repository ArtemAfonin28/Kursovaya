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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Taxopark
{
    public partial class MainVod : Form
    {
        bool driverNoCalls = true;
        string Telephone_Call = "";
        int check = 0;
        int rowIndex = 0;

        public MainVod()
        {
            InitializeComponent();
            updateDataInDataGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rowIndex != -1)
            {
                if (driverNoCalls == true)
                {
                    //Выборка номера телефона из DataGridView

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
                    MessageBox.Show("Вы приняли заказ");
                    db.closeConnection();

                    label5.Visible = false;
                    label2.Visible = true;
                    label3.Visible = true;

                    for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
                    {
                        label2.Text += dataGridView1[i, rowIndex].Value.ToString();
                        label2.Text += "\n";
                    }
                    driverNoCalls = false;
                    updateDataInDataGrid();
                }
                else
                {
                    MessageBox.Show("Вы уже приняли вызов");
                }
            }
            else MessageBox.Show("Вы не выбрали заказ");



        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)//Установка галочки напротив строки
        {
            getCheckBox();
        }
        private void getCheckBox()
        {
            DataGridViewCheckBoxCell ch3 = new DataGridViewCheckBoxCell();
            ch3 = (DataGridViewCheckBoxCell)dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4];
            if (ch3.Value == null)
                ch3.Value = false;

            switch (ch3.Value.ToString())
            {
                case "True":
                    ch3.Value = false;
                    check--;
                    break;
                case "False":
                    check++;
                    ch3.Value = true;
                    if (check >= 2)
                    {
                        MessageBox.Show("Вы выбрали больше одного заказа");
                        ch3.Value = null;
                        check--;
                    }
                    rowIndex = ch3.RowIndex;
                    break;
            }
        }
        private void MainVod_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Telephone_Call == "")
            {
                MessageBox.Show("У вас нет принятого вызова");
            }
            else
            {
                DB db = new DB();
                MySqlCommand command = new MySqlCommand("UPDATE `call` SET `Accepted` = null, `Accepted_DataTime`= null WHERE `Telephone_Call` = @Telephone_Call;", db.getConnection());
                command.Parameters.Add("@Telephone_Call", MySqlDbType.VarChar).Value = Telephone_Call;
                db.openConnection();
                command.ExecuteNonQuery();
                MessageBox.Show("Вы отказались от вызова");
                db.closeConnection();
                Telephone_Call = "";
                label5.Visible = true;
                label2.Visible = false;
                label3.Visible = false;
                label2.Text = "";
                driverNoCalls = true;
                updateDataInDataGrid();
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            updateDataInDataGrid();
        }

        private void updateDataInDataGrid() //Заполнение dataGridView1
        {
            if (check != 0)
            {
                check--;
            }
            dataGridView1.Rows.Clear();//очистка

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
    }
}
