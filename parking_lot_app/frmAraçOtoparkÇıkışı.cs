using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otopark_otomasyonu
{
    public partial class frmAraçOtoparkÇıkışı : Form
    {
        public frmAraçOtoparkÇıkışı()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=EMRE-SAMUK;Initial Catalog=araç_otopark;Integrated Security=True");
        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
        
        private void frmAraçOtoparkÇıkışı_Load(object sender, EventArgs e)
        {
            DoluYerler();
            Plaka();
            timer1.Enabled = true;
        }

        private void Plaka()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from araç_otopark_kaydı", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                comboPlaka.Items.Add(reader["plaka"].ToString());
            }
            baglanti.Close();
        }

        private void DoluYerler()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from araç_durumu where durum='DOLU'", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                comboParkYeri.Items.Add(reader["parkyeri"].ToString());
            }
            baglanti.Close();
        }

        private void comboPlaka_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from araç_otopark_kaydı where plaka='"+comboPlaka.SelectedItem+"'", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                txtParkYeri.Text = reader["parkyeri"].ToString();
            }
            baglanti.Close();
        }

        private void comboParkYeri_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from araç_otopark_kaydı where parkyeri='" + comboParkYeri.SelectedItem + "'", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                txtParkYeri2.Text = reader["parkyeri"].ToString();
                txtTc.Text = reader["tc"].ToString();
                txtAd.Text = reader["ad"].ToString();
                txtSoyad.Text = reader["soyad"].ToString();
                txtMarka.Text = reader["marka"].ToString();
                txtSeri.Text = reader["seri"].ToString();
                txtPlaka.Text = reader["plaka"].ToString();
                lblGelişTarih.Text = reader["tarih"].ToString();
            }
            baglanti.Close();
            DateTime geliş, çıkış;
            geliş = DateTime.Parse (lblGelişTarih.Text);
            çıkış =DateTime.Parse (lblÇıkışTarih.Text);
            TimeSpan fark;
            fark = çıkış - geliş;
            lblGeçenZaman.Text = fark.TotalHours.ToString("0.00");
            lblÜcret.Text = (double.Parse(lblGeçenZaman.Text) * (2)).ToString("0.00");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblÇıkışTarih.Text =DateTime.Now.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from araç_otopark_kaydı where plaka ='"+txtPlaka.Text+"' ",baglanti);
            komut.ExecuteNonQuery();
            SqlCommand komut2 = new SqlCommand("update araç_durumu set durum = 'BOŞ' where parkyeri ='" + txtParkYeri2.Text + "' ", baglanti);
            komut2.ExecuteNonQuery();
            SqlCommand komut3 = new SqlCommand("insert into Satış_Bilgileri(parkyeri,plaka,geliş_tarihi,çıkış_tarihi,geçen_zaman,ücret) values(@parkyeri,@plaka,@geliş_tarihi,@çıkış_tarihi,@geçen_zaman,@ücret)", baglanti);
            komut3.Parameters.AddWithValue("@parkyeri", txtParkYeri2.Text);
            komut3.Parameters.AddWithValue("@plaka", txtPlaka.Text);
            komut3.Parameters.AddWithValue("@geliş_tarihi", lblGelişTarih.Text);
            komut3.Parameters.AddWithValue("@çıkış_tarihi", lblÇıkışTarih.Text);
            komut3.Parameters.AddWithValue("@geçen_zaman", double.Parse(lblGeçenZaman.Text));
            komut3.Parameters.AddWithValue("@ücret", double.Parse(lblÜcret.Text));
            komut3.ExecuteNonQuery();

            baglanti.Close() ;
            MessageBox.Show("Araç çıkışı tamamlandı!");
            foreach(Control item in groupBox2.Controls) 
            {
                if(item is TextBox) 
                {
                    item.Text = "";
                    txtParkYeri.Text = "";
                    comboParkYeri.Text = "";
                    comboPlaka.Text = "";
                }
            }
            comboPlaka.Items.Clear();
            comboParkYeri.Items.Clear();
            DoluYerler();
            Plaka();
        }
    }
}
