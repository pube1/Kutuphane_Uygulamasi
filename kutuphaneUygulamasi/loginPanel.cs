using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace kutuphaneUygulamasi
{
    public partial class loginPanel : Form
    {
        public loginPanel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Resources\\VT_kutuphane.accdb");
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from adminler", baglanti);
            OleDbDataReader oku = komut.ExecuteReader();

            while (oku.Read())
            {
                if (oku["kullaniciAdi"].ToString()==kAdiTb.Text && oku["sifre"].ToString()==sifreTb.Text)
                {
                    MessageBox.Show("Başarıyla giriş yapıldı.");
                    oku.Close();
                    baglanti.Close();
                    this.Hide();
                    Form1 form1 = new Form1();
                    form1.ShowDialog();
                    break;
                }

                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı");
                    break;
                }
            }
        }

        
    }
}
