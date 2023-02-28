using System.Data;
using System.Data.SqlClient;

namespace Disconnected
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Server=(LocalDb)\\MSSQLLocalDB;Database=Northwind;Integrated Security=True");
        public void FormAcilis()
        {
            SqlDataAdapter adb = new SqlDataAdapter("select * from Products", baglanti);
            DataTable tablo = new DataTable();
            adb.Fill(tablo);
            dataGridView1.DataSource = tablo;

            SqlDataAdapter cadp = new SqlDataAdapter("select * from Categories", baglanti);
            DataTable cats = new DataTable();
            cadp.Fill(cats);
            listBox1.DataSource = cats;
            listBox1.DisplayMember = "CategoryName";
            listBox1.ValueMember = "CategoryId";

            SqlDataAdapter ktadp = new SqlDataAdapter("select * from Categories select * from Suppliers", baglanti);
            DataSet ds = new DataSet();
            ktadp.Fill(ds);
            cmbKategori.DataSource = ds.Tables[0];
            cmbKategori.DisplayMember = "CategoryName";
            cmbKategori.ValueMember = "CategoryId";
            cmbTedarikci.DataSource = ds.Tables[1];
            cmbTedarikci.DisplayMember = "CompanyName";
            cmbTedarikci.ValueMember = "SupplierId";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FormAcilis();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            int catid = (int)listBox1.SelectedValue;
            SqlDataAdapter fadb = new SqlDataAdapter("select * from Products where CategoryId=@c", baglanti);
            fadb.SelectCommand.Parameters.AddWithValue("@c", catid);
            DataTable dt = new DataTable();
            fadb.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand eklekomut = new SqlCommand("insert Products (ProductName,CategoryId,SupplierId,UnitPrice,UnitsInStock) values(@pn,@c,@s,@p,@us)", baglanti);
            eklekomut.Parameters.AddWithValue("@pn", txtUrunAdi.Text);
            eklekomut.Parameters.AddWithValue("@c", cmbKategori.SelectedValue);
            eklekomut.Parameters.AddWithValue("@s", cmbTedarikci.SelectedValue);
            eklekomut.Parameters.AddWithValue("@p", numFiyat.Value);
            eklekomut.Parameters.AddWithValue("@us", numStok.Value);
            baglanti.Open();
            if (eklekomut.ExecuteNonQuery() == 0) 
            {
                MessageBox.Show("Ekleme Baþarýsýz");
            }
            baglanti.Close();
            FormAcilis();
        }
    }
}