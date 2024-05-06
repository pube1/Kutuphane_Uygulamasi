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
    public partial class kitapVer : Form
    {
        public kitapVer()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Resources\\VT_kutuphane.accdb");


        public void verileriGoster(string veriler,int deger)
        {
            OleDbDataAdapter da = new OleDbDataAdapter(veriler, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);



            if (deger == 0)
            {
                verilenKitapDgv.DataSource = ds.Tables[0];
            }

            else if (deger == 1)
            {
                kitaplarDgv.DataSource = ds.Tables[0];
            }

            else if (deger == 2)
            {
                kullanicilarDgv.DataSource = ds.Tables[0];
            }
        }

        


        public bool kisiKontrol(string kisi)
        { 
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from ogrenciler where Numara like" + "'" + "%" + kisi + "%" + "'" ,baglanti);
            OleDbDataReader oku = komut.ExecuteReader();
            bool kontrol = false;
            while (oku.Read())
            {
                if (oku["Numara"].ToString() == kisi)
                {
                    kontrol = true;
                    oku.Close();
                    break;
                }
            }

            if (kontrol == true)
            {
                baglanti.Close();
                return true;
            }

            else
            {
                MessageBox.Show("Böyle bir kullanıcı yok");
                baglanti.Close();
                return false;
            }

            
        }

        public bool kitapKisiKontrol(string kitap)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from verilenKitaplar where verilenKitapBarkod like" + "'" + "%" +
                kitap + "%" + "'", baglanti);
            OleDbDataReader oku = komut.ExecuteReader();
            bool kontrolKitap = false;
            while (oku.Read())
            {
                if (oku["verilenKitapBarkod"].ToString() == kitap)
                {
                    kontrolKitap = true;
                    break;
                }
            }

            if (kontrolKitap == true)
            {
                
                OleDbCommand bulunan = new OleDbCommand("select * from ogrenciler where Numara like" + "'" + "%" + oku["verilenKisiId"] + "%" + "'", baglanti);
                oku.Close();

                oku = bulunan.ExecuteReader();

                oku.Read();
                MessageBox.Show("Bu kitap halihazırda" + " " + oku["Numara"].ToString() + " " + oku["Adi"].ToString() + " " +
                        oku["Soyadi"].ToString() + " " + oku["Sinifi"] + " Bilgili kullanıcıya kayıtlı");
                oku.Close();
                baglanti.Close();
                return true;
            }
            
            else
            {
                baglanti.Close();
                return false;
            }
        }

        public bool kitapKontrol(string kitap)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from kitaplar where Barkod like" + "'" + "%" + kitap + "%" + "'", baglanti);
            OleDbDataReader oku = komut.ExecuteReader();
            bool kontrol = false;
            while (oku.Read())
            {
                if (oku["Barkod"].ToString() == kitap)
                {
                    kontrol = true;
                    oku.Close();
                    break;
                }
            }

            if (kontrol == true)
            {
                baglanti.Close();
                return true;
            }

            else
            {
                MessageBox.Show("Böyle bir kitap yok");
                baglanti.Close();
                return false;
            }
        }

        public bool kisiKitapKontrol(string kisi)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from verilenKitaplar where verilenKisiId like" + "'" + "%" + kisi + "%" + "'", baglanti);
            OleDbDataReader oku = komut.ExecuteReader();
            bool kontrolKisi = false;
            while (oku.Read())
            {
                if (oku["verilenKisiId"].ToString() == kisiTb.Text)
                {
                    kontrolKisi = true;
                    break;
                }
            }

            if (kontrolKisi == true)
            {

                OleDbCommand bulunan = new OleDbCommand("select * from kitaplar where Barkod like" + "'" + "%" + oku["verilenKitapBarkod"] + "%" + "'", baglanti);
                oku.Close();

                oku = bulunan.ExecuteReader();

                oku.Read();
                MessageBox.Show("Bu kişiye halihazırda" + " " + oku["Barkod"].ToString() + " " + oku["KitapAdi"].ToString() + " " +
                        oku["SayfaSayi"].ToString() + oku["Yazari"] + " Bilgili kitap kayıtlı");
                oku.Close();
                baglanti.Close();
                return true;
            }

            else
            {
                baglanti.Close();
                return false;
            }
                
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (kayitTb.Text == "" || kisiTb.Text == "" || kitapBarkodTb.Text == "")
            {
                MessageBox.Show("Lütfen eksikleri doldurun", "Hata", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

            else
            {
                // kullanıcının var olup olmadığının kontrolü
                bool kntrl = kisiKontrol(kisiTb.Text);
                if (kntrl == false) return;

                // Kitabın var olup olmadığının kontrolü
                kntrl = kitapKontrol(kitapBarkodTb.Text);
                if (kntrl == false) return;

                // Kitabın birinde olup olmadığının kontrolü
                kntrl = kitapKisiKontrol(kitapBarkodTb.Text);
                if (kntrl == true) return;

                // Bir kişide zaten bir kitap olup olmadığının kontrolü
                kntrl = kisiKitapKontrol(kisiTb.Text);
                if (kntrl == true) return;

                baglanti.Open();

                

                OleDbCommand komut = new OleDbCommand("insert into verilenKitaplar (id,verilenKisiId,verilenKitapBarkod,Tarih) values ('" + kayitTb.Text + "','" +
                                    kisiTb.Text + "','" + kitapBarkodTb.Text + "','" + DateTime.Today + "')");


                komut.Connection = baglanti;
                try
                {
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Kayıt edildi");
                }

                catch
                {
                    MessageBox.Show("Böyle bir ID bulunuyor");
                }

            }





            verileriGoster("select * from verilenKitaplar",0);
            baglanti.Close();
        }

        private void kitapVer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (silKayitTb.Text == "")
            {
                MessageBox.Show("Lütfen kayıt ID giriniz");
            }

            else
            {
                bool kontrol = false;
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("select * from verilenKitaplar where id like" + "'" + "%" + silKayitTb.Text.ToString() + "%" + "'",baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    if (oku["id"].ToString() == silKayitTb.Text)
                    {
                        kontrol = true; break;
                    }

                }
                

                if (kontrol == false)
                {
                    MessageBox.Show("Böyle bir kayıt bulunmuyor.");
                }

                else
                {
                    komut.CommandText = "select * from ogrenciler where Numara like" + "'" + "%" + oku["verilenKisiId"] + "%" + "'";
                    oku.Close();
                    oku=komut.ExecuteReader();
                    oku.Read();
                    DialogResult cvp = MessageBox.Show(oku["Numara"] + " " + oku["Adi"] + " "  +  oku["Soyadi"] + " " + oku["Sinifi"] + " bilgili öğrencinin kayıtlı kitabını silmek istiyor musunuz?", "Kayıt siliniyor.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (cvp == DialogResult.Yes)
                    {
                        MessageBox.Show("Kayıt silindi");
                        oku.Close();
                        komut.CommandText = "delete from verilenKitaplar where id=" + silKayitTb.Text;
                        komut.ExecuteNonQuery();
                    }

                    else MessageBox.Show("İşlem iptal edildi");
                }
            }
            
            verileriGoster("select * from verilenKitaplar",0);
            baglanti.Close();
        }


        private void kitapVer_Load(object sender, EventArgs e)
        {
            verileriGoster("select * from verilenKitaplar",0);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                verileriGoster("select * from verilenKitaplar",0);
            }

            else if (tabControl1.SelectedIndex == 1)
            {
                verileriGoster("select * from ogrenciler",2);
            }

            else if (tabControl1.SelectedIndex==2)
            {
                verileriGoster("select * from kitaplar",1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 anamenu = new Form1();
            this.Hide();
            anamenu.ShowDialog();
        }

        private void kayitTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(kayitTb.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Kayıt ID ");
                kayitTb.Text = "";
            }
            kayitTb.MaxLength = 4;
        }

        private void kitapBarkodTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(kitapBarkodTb.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Kitap Barkod ");
                kitapBarkodTb.Text = "";
            }
            kitapBarkodTb.MaxLength = 4;
        }

        private void kisiTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(kisiTb.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Kişi Numara ");
                kisiTb.Text = "";
            }
            kisiTb.MaxLength = 4;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (alAraOgrRb.Checked)
            {
                verileriGoster("select * from verilenKitaplar where verilenKisiId like" + "'" + "%" + silKitapAraTb.Text + "%" + "'", 0);
            }

            else if (alAraBarkodRb.Checked)
            {
                verileriGoster("select * from verilenKitaplar where verilenKitapBarkod like" + "'" + "%" + silKitapAraTb.Text + "%" + "'", 0);
            }

            else if (alAraKayitRb.Checked)
            {
                verileriGoster("select * from verilenKitaplar where id like" + "'" + "%" + silKitapAraTb.Text + "%" + "'", 0);
            }

            else
            {
                MessageBox.Show("Bir seçim yapın.");
            }
        }

        private void silKayitTb_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(silKayitTb.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen doğru değer giriniz: Kayıt ID ");
                silKayitTb.Text = "";
            }
            silKayitTb.MaxLength = 4;
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl2.SelectedIndex == 1)
            {
                MessageBox.Show("Kayıt ID girerek kitap geri alma işlemini gerçekleştirebilirsiniz","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
    }

    
}
