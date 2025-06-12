using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace muzik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source = DESKTOP-L2IO4EG\\SQLEXPRESS; Initial Catalog = muzik; Integrated Security = True;");
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            maskedTextBox1.Mask = "00:00:00";
            listele();
        }
        public void listele()
        {
            if (baglanti.State != ConnectionState.Open)
            {
                baglanti.Open();
            }
            SqlCommand sqlCommand = new SqlCommand("select * from muzikislem", baglanti);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;

            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            DateTime saat = DateTime.Now;
            label3.Text = saat.ToString("HH:mm:ss");




        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 Dosyaları (*.mp3)|*.mp3";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;
                MessageBox.Show("Seçilen dosya: " + filePath);
                label4.Text = filePath;


            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }
                using (SqlCommand kaydet = new SqlCommand("insert into muzikislem(calmaSaati,dosyayolu) values (@Saat,@dosyayolu)", baglanti))
                {
                    kaydet.Parameters.AddWithValue("@Saat", maskedTextBox1.Text);
                    kaydet.Parameters.AddWithValue("@dosyayolu", label4.Text);


                    kaydet.ExecuteNonQuery();
                    listele();
                    axWindowsMediaPlayer1.Refresh();
                }
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
                button1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata : " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }
                using (SqlCommand sil = new SqlCommand("delete from muzikislem where id=@id", baglanti))
                {
                    sil.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells["id"].Value.ToString());
                    sil.ExecuteNonQuery();
                    listele();
                    axWindowsMediaPlayer1.Refresh();

                }
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata : " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {


                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }
                using (SqlCommand guncelle = new SqlCommand("update muzikislem set calmaSaati=@saat,dosyayolu=@yol where id=@id", baglanti))
                {
                    guncelle.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells["id"].Value.ToString());
                    guncelle.Parameters.AddWithValue("@saat", maskedTextBox1.Text);
                    guncelle.Parameters.AddWithValue("@yol", label4.Text);
                    guncelle.ExecuteNonQuery();
                    axWindowsMediaPlayer1.Refresh();

                }
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata : " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {


        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            maskedTextBox1.Text = dataGridView1.CurrentRow.Cells["calmaSaati"].Value.ToString();
            label4.Text = dataGridView1.CurrentRow.Cells["dosyayolu"].Value.ToString();

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (baglanti.State != ConnectionState.Open)
            {
                baglanti.Open();
            }
            DateTime saat = DateTime.Now;
            label3.Text = saat.ToString("HH:mm:ss");
            using (SqlCommand listele = new SqlCommand("select * from muzikislem", baglanti))
            {
                using (SqlDataReader listeyioku = listele.ExecuteReader())
                {
                    while (listeyioku.Read())
                    {
                        string saat1 = listeyioku["calmaSaati"].ToString();
                        string yol = listeyioku["dosyayolu"].ToString();

                        if (DateTime.TryParse(saat1, out DateTime planlısaat) &&
                            DateTime.TryParse(label3.Text, out DateTime suankisaat)
                            )
                        {
                            if (planlısaat == suankisaat)
                            {
                                axWindowsMediaPlayer1.URL = yol.ToString();
                                axWindowsMediaPlayer1.Ctlcontrols.play();

                                break;
                            }


                        }

                    }
                    listeyioku.Close();
                }

            }

            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
        }
    }
}

