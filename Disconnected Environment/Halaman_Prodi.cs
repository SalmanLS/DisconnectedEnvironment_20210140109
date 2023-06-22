﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disconnected_Environment
{
    public partial class Form2 : Form
    {
        private string stringConnection = "data source=LUTHFI\\MCH35;"+
            "database=Prodi_TI;User ID=sa; Password=12345";
        private SqlConnection koneksi;

        private void refreshform()
        {
            nmp.Text = "";
            nmp.Enabled = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;

        }


        
        public Form2()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            refreshform();
        }

        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            nmp.Enabled=true;
            btnSave.Enabled=true;
            btnClear.Enabled=true;
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nmProdi = nmp.Text;
            if(nmProdi == "")
            {
                MessageBox.Show("Masukkan Nama Prodi", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                koneksi.Open();
                string str = "insert into dbo.prodi (nama_prodi)" + "values(@id)";
                SqlCommand cmd = new SqlCommand(str, koneksi);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("id",nmProdi));
                cmd.ExecuteNonQuery();

                koneksi.Close();
                MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView();
                refreshform();
            }
        }
        
        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView();
            btnOpen.Enabled = false;

        }

        private void dataGridView()
        {
            koneksi.Open();
            string str = "select nama_prodi from dbo.prodi";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            koneksi.Close();
        }

        

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 hu = new Form1();
            hu.Show();
            this.Hide();
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            refreshform();
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

    }
}
