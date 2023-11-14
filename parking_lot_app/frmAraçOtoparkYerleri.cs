using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Otopark_otomasyonu
{
    public partial class frmAraçOtoparkYerleri : Form
    {
        public frmAraçOtoparkYerleri()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=EMRE-SAMUK;Initial Catalog=araç_otopark;Integrated Security=True");
        private void frmAraçOtoparkYerleri_Load(object sender, EventArgs e)
        {
            BoşParkYerleri();
            DoluParkYerleri();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from araç_otopark_kaydı", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                foreach (Control item in Controls)
               {
                    if (item is Button)
                   {
                        if (item.Text == reader["parkyeri"].ToString())
                        {
                            item.Text = reader["plaka"].ToString();
                        }
                    }
                }
            }
            baglanti.Close();
        }

        private void DoluParkYerleri()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from araç_durumu WHERE durum = 'DOLU'", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                foreach (Control item in Controls)
                {
                    if (item is Button)
                    {
                        if (item is Button)
                        {
                            if (item.Text == reader["parkyeri"].ToString() && reader["durum"].ToString() == "DOLU")
                            {
                                item.BackColor = Color.Red;
                            }
                        }
                    }
                }
            }
            baglanti.Close();
        }

        private void BoşParkYerleri()
        {
            int sayac = 1;
            foreach (Control item in Controls)
            {
                if (item is Button)
                {
                    item.Text = "P-" + sayac;
                    item.Name = "P-" + sayac;
                    sayac++;
                }
            }
        }
    }
}
