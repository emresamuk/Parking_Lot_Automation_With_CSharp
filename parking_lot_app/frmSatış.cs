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
    public partial class frmSatış : Form
    {
        public frmSatış()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=EMRE-SAMUK;Initial Catalog=araç_otopark;Integrated Security=True");
        DataSet ds = new DataSet();
        private void frmSatış_Load(object sender, EventArgs e)
        {
            SatışListe();
            Hesaplama();
        }

        private void Hesaplama()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select sum(ücret) from Satış_Bilgileri", baglanti);
            label1.Text = "Toplam tutar=" + komut.ExecuteScalar() + "TL";
            baglanti.Close();
        }

        private void SatışListe()
        {
            baglanti.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select *from Satış_Bilgileri", baglanti);
            adapter.Fill(ds, "Satış_Bilgileri");
            dataGridView1.DataSource = ds.Tables["Satış_Bilgileri"];
            baglanti.Close();
        }
    }
}
