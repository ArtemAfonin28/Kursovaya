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
    public partial class Main : Form
    {
        public string userName;
        public string phoneUser;
        public Main()
        {
            InitializeComponent();
        }
        private void Button4_Click(object sender, EventArgs e)
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
                MySqlCommand command = new MySqlCommand("insert into `call` (`DataTime_Call`,`Telephone_Call`,`Otkuda`,`Kuda`) " +
                    "values (@dateTime,@phoneCall,@otkuda,@kuda)", db.getConnection());
                command.Parameters.Add("@dateTime", MySqlDbType.DateTime).Value = dateTime;
                command.Parameters.Add("@phoneCall", MySqlDbType.VarChar).Value = phoneCall;
                command.Parameters.Add("@Otkuda", MySqlDbType.VarChar).Value = otkuda;
                command.Parameters.Add("@Kuda", MySqlDbType.VarChar).Value = kuda;

                db.openConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Успех");
                }
                else MessageBox.Show("Уже есть");
                    
                db.closeConnection();
                
                 
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            label1.Text = userName;
        }
    }
}