using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kutuphaneUygulamasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ogrenciKayit ogrenciKayit = new ogrenciKayit();
            this.Hide();
            ogrenciKayit.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            kitapKayit kp = new kitapKayit();
            this.Hide();
            kp.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            kitapVer kitapver= new kitapVer();
            this.Hide();
            kitapver.ShowDialog();
        }
    }
}
