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

namespace Disconnected_Environment
{
    public partial class Form3 : Form
    {
        private string stringConnection = "data source=LUTHFI\\MCH35;"+
            "database=Prodi_TI;User ID=sa; Password=12345";
        private SqlConnection koneksi;
        private string nim, nama, alamat, jk, prodi;
        private DateTime tgl;
        BindingSource costumersBindingSource = new BindingSource();
        private void Form3_Load(object sender, EventArgs e)
        {
            

        }

        private void FormDataMahasiswa()
        {
            koneksi.Open();
            SqlDataAdapter dataAdapter1 = new SqlDataAdapter(new SqlCommand("Select m.nim, m.nama_mahasiswa, m.alamat, m.jenis_kelamin, m.tgl_lahir, p.nama_prodi\r\nFrom dbo.mahasiswa m \r\njoin dbo.prodi p \r\non m.id_prodi = p.id_prodi", koneksi));
            DataSet ds = new DataSet();
            dataAdapter1.Fill(ds);

            this.costumersBindingSource.DataSource = ds.Tables[0];
            this.txtNIM.DataBindings.Add(
                new Binding("Text", this.costumersBindingSource, "nim", true));
            this.txtNama.DataBindings.Add(
                new Binding("Text", this.costumersBindingSource, "nama_mahasiswa", true));
            this.txtAlamat.DataBindings.Add(
                new Binding("Text", this.costumersBindingSource, "alamat", true));
            this.cbxJenisKelamin.DataBindings.Add(new Binding("Text", this.costumersBindingSource, "jenis_kelamin", true));
            this.dtTanggalLahir.DataBindings.Add(new Binding("Text", this.costumersBindingSource, "tgl_lahir", true));
            this.cbxProdi.DataBindings.Add(new Binding("Text", this.costumersBindingSource, "nama_prodi", true));
            koneksi.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtNIM.Text = "";
            txtNama.Text = "";
            txtAlamat.Text = "";
            dtTanggalLahir.Value = DateTime.Today;
            txtNIM.Enabled = true;
            txtNama.Enabled = true;
            cbxJenisKelamin.Enabled = true;
            txtAlamat.Enabled = true;
            dtTanggalLahir.Enabled = true;
            cbxProdi.Enabled = true;
            Prodicbx();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnAdd.Enabled = false;  
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            nim = txtNIM.Text;
            nama = txtNama.Text;
            jk = cbxJenisKelamin.Text;
            alamat = txtAlamat.Text;
            tgl = dtTanggalLahir.Value;
            prodi = cbxProdi.Text;
            int hs = 0;
            koneksi.Open();
            string strs = "select id_prodi from dbo.prodi where nama_prodi = @dd";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@dd", prodi));
            SqlDataReader dr = cm.ExecuteReader();
            while(dr.Read())
            {
                hs = int.Parse(dr["id_prodi"].ToString());
            }
            dr.Close();
            string str = "insert into dbo.mahasiswa (nim,nama_mahasiswa, jenis_kelamin, alamat, tgl_lahir, id_prodi)"+
                          "values(@nim, @nm, @jk, @al, @tgll, @idp) ";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("nim", nim));
            cmd.Parameters.Add(new SqlParameter("nm", nama));
            cmd.Parameters.Add(new SqlParameter("jk", jk));
            cmd.Parameters.Add(new SqlParameter("al", alamat));
            cmd.Parameters.Add(new SqlParameter("tgll", tgl));
            cmd.Parameters.Add(new SqlParameter("idp", hs));
            cmd.ExecuteNonQuery();

            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

            refreshform();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void clearBinding()
        {
            this.txtNIM.DataBindings.Clear();
            this.txtNama.DataBindings.Clear();
            this.txtAlamat.DataBindings.Clear();
            this.cbxJenisKelamin.DataBindings.Clear();
            this.dtTanggalLahir.DataBindings.Clear();
            this.cbxProdi.DataBindings.Clear();

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtNIM_TextChanged(object sender, EventArgs e)
        {

        }

        private void refreshform()
        {
            txtNIM.Enabled = false;
            txtNama.Enabled = false;
            txtAlamat.Enabled = false;
            cbxJenisKelamin.Enabled = false;
            cbxProdi.Enabled = false;
            dtTanggalLahir.Enabled = false;
            btnAdd.Enabled = true;
            btnClear.Enabled = false;
            btnSave.Enabled = false;
            clearBinding();
            FormDataMahasiswa();
        }
        

        
        public Form3()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            this.bnMahasiswa.BindingSource = this.costumersBindingSource;
            refreshform();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }

        private void Prodicbx()
        {
            koneksi.Open();
            string str = "select nama_prodi from dbo.prodi";
            SqlCommand cmd = new SqlCommand(str,koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();
            cbxProdi.DisplayMember = "nama_prodi";
            cbxProdi.ValueMember = "id_prodi";
            cbxProdi.DataSource = ds.Tables[0];
        }
       
    }
}
