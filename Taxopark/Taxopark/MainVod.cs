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
using Microsoft.Office.Interop.Excel;
using static Guna.UI2.Native.WinApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Taxopark
{
    public partial class MainVod : Form
    {
        public int idDriver;

        bool driverNoCalls = true;
        string Telephone_Call = "";
        int check = 0;
        int rowIndex = 0;

        public MainVod()
        {
            InitializeComponent();
            updateDataInDataGrid();
        }

        private void button1_Click(object sender, EventArgs e)//принятие заказа
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

                    int idDriverCall = idDriver;

                    DB db = new DB();
                    MySqlCommand command = new MySqlCommand("UPDATE `Call` SET `Accepted` = 1, `Accepted_DataTime`= @Accepted_DataTime,Drivers_Id_Drivers=@idDriverCall WHERE `Telephone_Call` = @Telephone_Call;", db.getConnection());
                    command.Parameters.Add("@Accepted_DataTime", MySqlDbType.DateTime).Value = Accepted_DataTime;
                    command.Parameters.Add("@Telephone_Call", MySqlDbType.VarChar).Value = Telephone_Call;
                    command.Parameters.Add("@idDriverCall", MySqlDbType.Int32).Value = idDriverCall;
                    db.openConnection();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Вы приняли заказ");
                    db.closeConnection();

                    label5.Visible = false;
                    label2.Visible = true;
                    label3.Visible = true;

                    driverNoCalls = false;
                    fillLabel4();
                    updateDataInDataGrid();
                    OutputToExcel();
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
            ch3 = (DataGridViewCheckBoxCell)dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[5];
            if (ch3.Value == null)
                ch3.Value = false;

            switch (ch3.Value.ToString())
            {
                case "True":
                    ch3.Value = false;
                    check--;
                    rowIndex = -1;
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
                    else rowIndex = ch3.RowIndex;
                    break;
            }
        }
        private void MainVod_Load(object sender, EventArgs e)
        {
            checkDriverNoCalls();
            if (driverNoCalls == false)
            {
                label3.Visible = true;
                label2.Visible = true;
                label5.Visible = false;
                fillLabel4();
            }
        }

        private void button3_Click(object sender, EventArgs e)//Отказаться от заказа
        {
            if (driverNoCalls == true)
            {
                MessageBox.Show("У вас нет принятого вызова");
            }
            else
            {
                DB db = new DB();
                MySqlCommand command = new MySqlCommand("UPDATE `Call` SET `Accepted` = null, `Accepted_DataTime`= null, `Alerts`=null WHERE `Telephone_Call` = @Telephone_Call;", db.getConnection());
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
            string query = "SELECT `DataTime_Call`,`Telephone_Call`,`Otkuda`,`Kuda`,`Name_Services` FROM `Call`,`Add_Services` WHERE (`Add_Services_Id_Services` = `Id_Services`) and (`Accepted` is null);";
            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[5]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
            }



            reader.Close();

            db.closeConnection();

            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Avtoriz avt = new Avtoriz();
            avt.Show();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)//Оповещение
        {
            if (driverNoCalls == true)
            {
                MessageBox.Show("У вас нет принятого вызова");
            }
            else
            {
                DB db = new DB();
                MySqlCommand command = new MySqlCommand("UPDATE `Call` SET `Alerts` = 1 WHERE `Drivers_Id_Drivers`=@idDriver;", db.getConnection());
                command.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = idDriver;
                db.openConnection();
                command.ExecuteNonQuery();
                MessageBox.Show("Оповещение придет клиенту как только он войдет в приложение");
                button4.Enabled = true;
                db.openConnection();
            }
        }

        private void button4_Click(object sender, EventArgs e)//завершить поездку
        {
            if (driverNoCalls == true)
            {
                MessageBox.Show("У вас нет принятого вызова");
            }
            else
            {
                DB db = new DB();
                MySqlCommand command = new MySqlCommand("UPDATE `Call` SET `Finished` = 1 WHERE  `Drivers_Id_Drivers`=@idDriver;", db.getConnection());
                command.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = idDriver;
                db.openConnection();
                command.ExecuteNonQuery();
                MessageBox.Show("Вы завершили вызов");
                db.closeConnection();
                driverNoCalls = true;
                label2.Text = "";
                label5.Visible = true;
                label2.Visible = false;
                label3.Visible = false;

                button4.Enabled = false;

                deleteFinishedCall();
            }
        }

        private void deleteFinishedCall()//удаление завершенного заказа
        {
            DB db = new DB();
            MySqlCommand command = new MySqlCommand("DELETE FROM `Call` WHERE `Finished`=1;", db.getConnection());
            db.openConnection();
            command.ExecuteNonQuery();
            db.closeConnection();
        }

        private void fillLabel4()//заполнение поля с информацие о текущем заказе
        {
            if (driverNoCalls == false)
            {
                DB db = new DB();
                System.Data.DataTable table = new System.Data.DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand("SELECT DataTime_Call,Telephone_Call,Otkuda,Kuda FROM 19055_Taxopark.Call where Drivers_Id_Drivers=@idDriver;", db.getConnection());
                command.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = idDriver;
                adapter.SelectCommand = command;
                adapter.Fill(table);

                for (int i = 0; i < 4; i++)
                {
                    label2.Text += table.Rows[0][i].ToString() + "\n";
                }

                db.closeConnection();
            }

        }
        private void checkDriverNoCalls()//проверка есть ли у водителя заказ
        {
            DB db = new DB();
            System.Data.DataTable table = new System.Data.DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT DataTime_Call,Telephone_Call,Otkuda,Kuda FROM 19055_Taxopark.Call where Drivers_Id_Drivers=@idDriver;", db.getConnection());
            command.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = idDriver;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            db.closeConnection();

            if (table.Rows.Count > 0)
            {
                driverNoCalls= false;
            }
            else driverNoCalls = true;
        }



        private void OutputToExcel()
        {
            String FileName = "C:\\Users\\Savan\\Desktop\\git\\Kursovaya\\Taxopark\\Excel1.xlsx";


            DB db = new DB();
            System.Data.DataTable table = new System.Data.DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT DataTime_Call,Telephone_Call,Otkuda,Kuda,Add_Services_Id_Services FROM 19055_Taxopark.Call where Drivers_Id_Drivers=@idDriver;", db.getConnection());
            command.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = idDriver;
            adapter.SelectCommand = command;
            adapter.Fill(table);


            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWb = xlApp.Workbooks.Open(FileName); //открываем Excel файл
            Microsoft.Office.Interop.Excel.Worksheet xlSht = xlWb.Sheets[1]; //первый лист в файле
            int iLastRow = xlSht.Cells[xlSht.Rows.Count, "A"].End[Microsoft.Office.Interop.Excel.XlDirection.xlUp].Row + 1;  //последняя заполненная строка в столбце А


            for (int j = 0; j < table.Columns.Count; j++)
            {
                xlApp.Cells[iLastRow, j + 1] = table.Rows[0][j];
            }





            xlWb.Close(true); //закрыть и сохранить книгу
            xlApp.Quit();
            MessageBox.Show("Файл успешно сохранён!");
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
