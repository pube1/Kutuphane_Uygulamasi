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
using System.Security.Cryptography.X509Certificates;

namespace kutuphaneUygulamasi
{
    public partial class ogrenciKayit : Form
    {
        public ogrenciKayit()
        {
            InitializeComponent();
        }

        public bool numaraKontrol(string numara)
        {
            bool kontrol = false;
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from ogrenciler where Numara like" + "'" + "%" + numara +
                "%" + "'",baglanti);
            OleDbDataReader oku = komut.ExecuteReader();

            while (oku.Read())
            {
                if (oku["Numara"].ToString() == numara)
                {
                    kontrol = true;
                }
            }
            oku.Close();
            if (kontrol == true)
            {
                oku = komut.ExecuteReader();
                oku.Read();

                MessageBox.Show("Bu numaraya " + oku["Adi"].ToString() + " " + oku["Soyadi"] + " " + oku["Sinifi"] + " Bilgili kullanıcı kayıtlı aslan");
                baglanti.Close();
                return true;
            }

            else
            {
                baglanti.Close();
                return false;
            }

            
        }

        public static string kontrolStr(string s, string dgr)
        {

            if (System.Text.RegularExpressions.Regex.IsMatch(s, "[^\\p{L}+$ ]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: " + dgr);
                s = "";
                return s;
            }

            else { return s; }

        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Resources\\VT_kutuphane.accdb");
        private void button1_Click(object sender, EventArgs e)
        {
            
            
            if (adiTb.Text == "" || soyadiTb.Text == "" || numaraTb.Text == "" || sinifCb.SelectedIndex == -1 || subeCb.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen tüm boşlukları doldurun.");
            }

            else
            {
                bool kntrl=numaraKontrol(numaraTb.Text);
                if (kntrl == true) return;

                baglanti.Open();
                OleDbCommand komut = new OleDbCommand();
                komut.Connection = baglanti;
                string sube=sinifCb.SelectedItem.ToString() + "/" + subeCb.SelectedItem.ToString();
                komut.CommandText = "insert into ogrenciler (Numara,Adi,Soyadi,Sinifi) values ('" + numaraTb.Text + "','" +
                    adiTb.Text + "','" + soyadiTb.Text + "','" + sube + "')";
                komut.ExecuteNonQuery();
                 MessageBox.Show(numaraTb.Text + " " + adiTb.Text + " " + soyadiTb.Text + " " + sube + " Bilgili öğrenciye kayıt yapıldı");
            }
            baglanti.Close();
            verileriGoster("select * from ogrenciler");


        }
        // Textboxlara girilen değerlerin kontrolleri
        private void adiTb_TextChanged(object sender, EventArgs e)
        {
            string kntrl=adiTb.Text;
            adiTb.Text= kontrolStr(kntrl, "İsim");
            adiTb.MaxLength = 30;
        }

        private void soyadiTb_TextChanged(object sender, EventArgs e)
        {
            string kntrl = soyadiTb.Text;
            soyadiTb.Text = kontrolStr(kntrl, "Soyad");
            soyadiTb.MaxLength= 20;
        }


        private void numaraTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(numaraTb.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Numara ");
                numaraTb.Text = "";
            }

            numaraTb.MaxLength = 5;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (silNumaraTb.Text == "")
            {
                MessageBox.Show("Numarayı boş bırakmayın.");
            }
            
            baglanti.Open();

            OleDbCommand komut = new OleDbCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from ogrenciler"; 
            OleDbDataReader oku =komut.ExecuteReader();
            while (oku.Read())
            {
                if (Convert.ToInt32(oku["Numara"]) == Convert.ToInt32(silNumaraTb.Text))
                {
                   DialogResult cvp = MessageBox.Show(oku["Numara"] + " " + oku["Adi"] + " " + oku["Soyadi"] + " " + oku["Sinifi"] + " Bilgili" +
                        " öğrenciyi silmek istiyor musunuz?" , "Kayıt siliniyor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (cvp == DialogResult.Yes)
                    {
                        oku.Close();
                        komut.CommandText = "delete from ogrenciler where Numara=" + silNumaraTb.Text;
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Kayıt Silindi");
                        break;
                    }

                    else
                    {
                    }
                }
            }
            baglanti.Close();

            verileriGoster("select * from ogrenciler");


        }

        

        private void ogrenciKayit_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.ShowDialog();

        }

        public void verileriGoster(string deger)
        {
            OleDbDataAdapter da = new OleDbDataAdapter(deger, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
            
        }

        private void ogrenciKayit_Load(object sender, EventArgs e)
        {
            verileriGoster("select * from ogrenciler");
            dataGridView1.ReadOnly = true;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                verileriGoster("select * from ogrenciler where Numara like" + "'" + "%" + ogrenciAraTb.Text + "%" + "'");
            }

            else if (radioButton2.Checked)
            {
                verileriGoster("select * from ogrenciler where Adi like" + "'" + "%" + ogrenciAraTb.Text + "%" + "'");
            }


        }

        private void ogrenciAraTb_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                
                if (System.Text.RegularExpressions.Regex.IsMatch(ogrenciAraTb.Text, "[^0-9]"))
                {
                    MessageBox.Show("Lütfen doğru değer giriniz: Numara");
                    ogrenciAraTb.Text = "";
                }   
            }

            else if (radioButton2.Checked)
            {
                
                if (System.Text.RegularExpressions.Regex.IsMatch(ogrenciAraTb.Text, "[^\\p{L}+$ ]"))
                {
                    MessageBox.Show("Lütfen doğru değer giriniz: İsim ");
                    ogrenciAraTb.Text = "";
                }
            }
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ogrenciAraTb.Text = "";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ogrenciAraTb.Text = "";
        }

        

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex==1)
            {
                MessageBox.Show("Öğrenci silmek için öğrencinin numarasını girip Sil tuşuna basın.","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            verileriGoster("select * from ogrenciler");
            ogrenciAraTb.Text = "";
            adiTb.Text = "";
            soyadiTb.Text = "";
            numaraTb.Text = "";
            ogrenciAraTb.Text = "";
            sinifCb.SelectedIndex = -1;
            subeCb.SelectedIndex= -1;
            silNumaraTb.Text = "";
            ogrenciAraSilTb.Text = "";
        }

        private void silNumaraTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(silNumaraTb.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Numara ");
                silNumaraTb.Text = "";
            }

            silNumaraTb.MaxLength = 5;
        }

        private void ogrenciAraSilTb_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(ogrenciAraSilTb.Text, "[^0-9]"))
                {
                    MessageBox.Show("Lütfen doğru değer giriniz: Numara");
                    ogrenciAraSilTb.Text = "";
                }
            }

            else if (radioButton2.Checked)
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(ogrenciAraSilTb.Text, "[^\\p{L}+$ ]"))
                {
                    MessageBox.Show("Lütfen doğru değer giriniz: İsim ");
                    ogrenciAraSilTb.Text = "";
                }
            }
        }

        private void numaraOgrenciSilRb_CheckedChanged(object sender, EventArgs e)
        {
            ogrenciAraSilTb.Text = "";

        }

        private void isimOgrenciSilRb_CheckedChanged(object sender, EventArgs e)
        {
            ogrenciAraSilTb.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (numaraOgrenciSilRb.Checked)
            {
                verileriGoster("select * from ogrenciler where Numara like" + "'" + "%" + ogrenciAraTb.Text + "%" + "'");
            }

            else if (isimOgrenciSilRb.Checked)
            {
                verileriGoster("select * from ogrenciler where Adi like" + "'" + "%" + ogrenciAraTb.Text + "%" + "'");
            }
        }
    }
}
