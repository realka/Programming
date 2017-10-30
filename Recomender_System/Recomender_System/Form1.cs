using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Recomender_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection xlsxbaglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=restoran_oneri.xls; Extended Properties='Excel 12.0 Xml;HDR=YES'");
        DataTable tablo = new DataTable(); //Verileri direkt datagrid'e çekmek için DataTable kodunu tanımlıyoruz.

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            try { 
            xlsxbaglanti.Open(); //Excel dosyamızığın bağlantısını açıyoruz.
            tablo.Clear(); //En üstte tanımladığımız Datatable değişkenini temizliyoruz.
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [rating_final$]", xlsxbaglanti);
            da.Fill(tablo); //Gelen sonuçları datatable'a gönderiyoruz.
            dataGridView1.DataSource = tablo; //datatable'da ki verileri datagrid'de listeliyoruz.
            xlsxbaglanti.Close(); //Bağlantıyı kapatıyoruz.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        double  ortalamak = 0, ortalama = 0, toplamk = 0, toplam = 0, kovtop = 0, stsapmak = 0, stsapma = 0,karea = 0,kareb = 0;
        double cos = 0;
        

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            bool kontrol = true;
            int[] kullanici = new int[7];
            try {
                kullanici[0] = Convert.ToInt32(textBox1.Text);
                kullanici[1] = Convert.ToInt32(textBox2.Text);
                kullanici[2] = Convert.ToInt32(textBox3.Text);
                kullanici[3] = Convert.ToInt32(textBox4.Text);
                kullanici[4] = Convert.ToInt32(textBox5.Text);
                kullanici[5] = Convert.ToInt32(textBox6.Text);
                kullanici[6] = Convert.ToInt32(textBox7.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            for (int i = 0; i < 7; i++)
            {
                if (kullanici[i] < 1 || kullanici[i] > 10)
                {
                    MessageBox.Show("1-10 Arası seçim yapınız..");
                    kontrol = false;
                }
            }
            if(kontrol == true)
            { 
            for(int i = 0; i < 130; i++)
            {
                ortalamak = 0; ortalama = 0; toplamk = 0; toplam = 0; kovtop = 0; stsapmak = 0; stsapma = 0; karea = 0; kareb=0;
                kovtop = 0; cos = 0;
                //Ortalama için toplam bulma ve cos kısmı
                for (int j=0; j<7; j++)
                {
                    toplamk += kullanici[j];
                    toplam += Convert.ToInt32(dataGridView1.Rows[i].Cells[j + 1].Value);
                    cos += kullanici[j]* Convert.ToInt32(dataGridView1.Rows[i].Cells[j + 1].Value);
                }
                ortalama = toplam / 7;
                ortalamak = toplamk / 7;
                //stdsapma kısımları,Pearson Kovaryans burası.
                for (int j = 0; j < 7; j++)
                {
                    kovtop += (kullanici[j] - ortalamak) * (Convert.ToInt32(dataGridView1.Rows[i].Cells[j + 1].Value) - ortalama);
                    stsapmak += Math.Pow(kullanici[j] - ortalamak, 2); 
                    stsapma += Math.Pow(Convert.ToInt32(dataGridView1.Rows[i].Cells[j + 1].Value) - ortalama, 2);
                    karea += Math.Pow(kullanici[j],2);
                    kareb += Math.Pow(Convert.ToInt32(dataGridView1.Rows[i].Cells[j + 1].Value),2);
                    
                }

                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                if ((Math.Sqrt(stsapmak) * Math.Sqrt(stsapma)) != 0)
                {
                    
                    row.Cells[1].Value = i;
                    row.Cells[0].Value = kovtop / (Math.Sqrt(stsapmak) * Math.Sqrt(stsapma));
                    dataGridView2.Rows.Add(row);
                }
                else
                {
                    row.Cells[1].Value = i;
                    row.Cells[0].Value = "Sifira Bölme Hatasi..!";
                    dataGridView2.Rows.Add(row);
                }
                if ((Math.Sqrt(karea) * Math.Sqrt(kareb)) != 0)
                {
                        dataGridView2.Rows[i].Cells[2].Value = cos / (Math.Sqrt(karea) * Math.Sqrt(kareb));
                    //row.Cells[2].Value = cos / (Math.Sqrt(karea) * Math.Sqrt(kareb));
                    //dataGridView2.Rows.Add(row);
                    }
                else
                {
                        dataGridView2.Rows[i].Cells[2].Value = "Sifira Bölme Hatasi..!";
                        //row.Cells[2].Value = "Sifira Bölme Hatasi..!";
                        //dataGridView2.Rows.Add(row);
                }

                        //richTextBox1.Text += "\n";
            }
            dataGridView2.Sort(dataGridView2.Columns["Pearson"], ListSortDirection.Descending);
            for (int i = 0; i < Convert.ToInt32(textBox8.Text); i++)
            {
                DataGridViewRow row2 = (DataGridViewRow)dataGridView4.Rows[0].Clone();
                row2.Cells[0].Value = dataGridView2.Rows[i].Cells[0].Value;
                row2.Cells[1].Value = dataGridView1.Rows[Convert.ToInt32(dataGridView2.Rows[i].Cells[1].Value)].Cells[0].Value;
                dataGridView4.Rows.Add(row2);
            }
                //dataGridView4.Sort(dataGridView4.Columns["Restoran"], ListSortDirection.Ascending);

           dataGridView2.Sort(dataGridView2.Columns["Kosinüs"], ListSortDirection.Descending);
           for (int i = 0; i < Convert.ToInt32(textBox8.Text); i++)
           {
                DataGridViewRow row3 = (DataGridViewRow)dataGridView3.Rows[0].Clone();
                row3.Cells[0].Value = dataGridView2.Rows[i].Cells[2].Value;
                row3.Cells[1].Value = dataGridView1.Rows[Convert.ToInt32(dataGridView2.Rows[i].Cells[1].Value)].Cells[0].Value;
                dataGridView3.Rows.Add(row3);
          }



         }//if bitiş
        }
    }
}
