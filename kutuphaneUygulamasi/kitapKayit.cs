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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;

namespace kutuphaneUygulamasi
{
    public partial class kitapKayit : Form
    {
        public kitapKayit()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Resources\\VT_kutuphane.accdb");

        public void verileriGoster(string veriler)
        {
            OleDbDataAdapter da = new OleDbDataAdapter(veriler,baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from kitaplar";
            OleDbDataReader oku= komut.ExecuteReader();
            if (barkodTb.Text == "" || kitapAdiTb.Text == "" || yazarTb.Text == "" || sayfaTb.Text == "")
            {
                MessageBox.Show("Lütfen tüm boşlukları doldurun.");
            }
            else
            {
                int kontrol = 0;
                while (oku.Read())
                {
                    if (oku["Barkod"].ToString() == barkodTb.Text)
                    {
                        kontrol++;
                        oku.Close();
                        break;
                    }

                }

                if (kontrol != 0)
                {
                    komut.CommandText = "select * from kitaplar where barkod like" + "'" + "%" + barkodTb.Text + "%" + "'";
                    OleDbDataReader sonuc = komut.ExecuteReader();
                    sonuc.Read();
                    MessageBox.Show("Bu barkoda halihazırda" + " " + sonuc["KitapAdi"].ToString() + " " + sonuc["SayfaSayi"].ToString() + " " +
                            sonuc["Yazari"].ToString() + " Bilgili kitap kayıtlı");

                    oku.Close();
                }

                
                else
                {
                   oku.Close();
                   komut.CommandText = "insert into kitaplar (Barkod,KitapAdi,SayfaSayi,Yazari) values ('" + barkodTb.Text + "','" +
                                    kitapAdiTb.Text + "','" + sayfaTb.Text + "','" + yazarTb.Text + "')";
                    komut.ExecuteNonQuery();
                    MessageBox.Show(barkodTb.Text + " " + kitapAdiTb.Text + " " + sayfaTb.Text + " " + yazarTb.Text + " Bilgili kitap kaydı yapıldı");
                        
                }

                verileriGoster("select * from kitaplar");

            }

            

            

            baglanti.Close();
        }

        private void barkodTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(barkodTb.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Numara ");
                barkodTb.Text = "";
            }
            barkodTb.MaxLength = 4;
        }

        private void kitapAdiTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(kitapAdiTb.Text, "[^\\p{L}+$ ]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Kitap Adı");
                kitapAdiTb.Text = "";
            }
            kitapAdiTb.MaxLength = 20;
        }

        private void yazarTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(yazarTb.Text, "[^\\p{L}+$ ]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Yazar");
                yazarTb.Text = "";
            }

            yazarTb.MaxLength = 45;
        }

        

        private void sayfaTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(sayfaTb.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Sayfa sayı ");
                sayfaTb.Text = "";
            }

            sayfaTb.MaxLength = 4;
        }

        private void kitapKayit_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                MessageBox.Show("Kitap silmek için kitabın barkod numarasını girip sil tuşuna basın","Kitap silme işlemi", MessageBoxButtons.OK,MessageBoxIcon.Information);
                button2.Enabled = true;
                kitapAdiTb.Enabled = false;
                yazarTb.Enabled=false;
                sayfaTb.Enabled = false;
            }

            else
            {
                button2.Enabled=false;
                kitapAdiTb.Enabled = true;
                yazarTb.Enabled = true;
                sayfaTb.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int deger = 0;
            if (silBarkodTb.Text == "")
            {
                MessageBox.Show("Barkodu boş bırakmayın.");
            }

            baglanti.Open();

            OleDbCommand komut = new OleDbCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from kitaplar";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                if (Convert.ToInt32(oku["Barkod"]) == Convert.ToInt32(silBarkodTb.Text))
                {
                    deger++;
                    DialogResult cvp = MessageBox.Show(oku["Barkod"] + " " + oku["KitapAdi"] + " " + oku["SayfaSayi"] + " " + oku["Yazari"] + " Bilgili" +
                         " kitabı silmek istiyormusunuz?", "Kayıt siliniyor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (cvp == DialogResult.Yes)
                    {
                        oku.Close();
                        komut.CommandText = "delete from kitaplar where Barkod=" + silBarkodTb.Text;
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Kayıt Silindi");
                        break;
                    }

                    else
                    {
                        break;
                    }

                }

                
            }
            if (deger == 0)
            {
                MessageBox.Show("Böyle bir kitap yok ");
            }
            verileriGoster("select * from kitaplar");
            baglanti.Close();


        }

        private void kitapKayit_Load(object sender, EventArgs e)
        {
            verileriGoster("select * from kitaplar");
            dataGridView1.ReadOnly = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 anaMenu = new Form1();
            this.Hide();
            anaMenu.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            
            if (radioButton1.Checked)
            {
                verileriGoster("select * from kitaplar where Barkod like" + "'" + "%" + kitapAraTb.Text + "%" + "'");
            }

            else if (radioButton2.Checked)
            {
                verileriGoster("select * from kitaplar where KitapAdi like" + "'" + "%" + kitapAraTb.Text + "%" + "'");
            }

            else
            {
                MessageBox.Show("Bir seçim yapın.");
            } 

        }

        

        

        private void kitapAraTb_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(kitapAraTb.Text, "[^0-9]"))
                {
                    MessageBox.Show("Lütfen doğru değer giriniz: Numara");
                    kitapAraTb.Text = "";
                }
            }

            else if (radioButton2.Checked)
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(kitapAraTb.Text, "[^\\p{L}+$ ]"))
                {
                    MessageBox.Show("Lütfen doğru değer giriniz: İsim ");
                    kitapAraTb.Text = "";
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            kitapAraTb.Text = "";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            kitapAraTb.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (silBarkodRb.Checked)
            {
                verileriGoster("select * from kitaplar where Barkod like" + "'" + "%" + silKitapAraTb.Text + "%" + "'");
            }

            else if (silİsimRb.Checked)
            {
                verileriGoster("select * from kitaplar where KitapAdi like" + "'" + "%" + silKitapAraTb.Text + "%" + "'");
            }

            else
            {
                MessageBox.Show("Bir seçim yapın.");
            }
        }

        

        private void silBarkodTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(silBarkodTb.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Numara ");
                silBarkodTb.Text = "";
            }
            silBarkodTb.MaxLength = 4;


        }

        private void silKitapAraTb_TextChanged(object sender, EventArgs e)
        {
            if (silBarkodRb.Checked)
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(kitapAraTb.Text, "[^0-9]"))
                {
                    MessageBox.Show("Lütfen doğru değer giriniz: Barkod");
                    silKitapAraTb.Text = "";
                }
            }

            else if (silİsimRb.Checked)
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(kitapAraTb.Text, "[^\\p{L}+$ ]"))
                {
                    MessageBox.Show("Lütfen doğru değer giriniz: Kitap İsim ");
                    silKitapAraTb.Text = "";
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                MessageBox.Show("Kitap silme işlemi için barkod no girip sil tuşuna basın", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            barkodTb.Text = "";
            kitapAdiTb.Text = "";
            yazarTb.Text = "";
            sayfaTb.Text = "";
            kitapAraTb.Text = "";
            silBarkodTb.Text = "";
            silKitapAraTb.Text = "";
        }
    }
}
