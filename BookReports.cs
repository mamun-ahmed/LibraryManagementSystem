﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using DGVPrinterHelper;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Reflection;


namespace $safeprojectname$
{
    public partial class BookReports : Form
    {
        public BookReports()
        {
            InitializeComponent();
        }

        public void ViewGrid()
        {
            Connection con = new Connection();
            MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM `lms`.`book_master`", con.ActiveCon());
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = (n + 1).ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["book_id"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["book_name"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["book_auth"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["book_edition"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["book_pages"].ToString();
                dataGridView1.Rows[n].Cells[6].Value = item["book_publish"].ToString();
                dataGridView1.Rows[n].Cells[7].Value = item["sr_name"].ToString();
                dataGridView1.Rows[n].Cells[8].Value = item["rack_name"].ToString();
                dataGridView1.Rows[n].Cells[9].Value = item["book_staus"].ToString();
                dataGridView1.Rows[n].Cells[10].Value = item["Price"].ToString();
            }

            rawsCountlb.Text = dt.Rows.Count.ToString();


        }

        private void BookReports_Load(object sender, EventArgs e)
        {
            ViewGrid();
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            ViewGrid();


            try
            {
                //Creating iTextSharp Table from the DataTable data
                PdfPTable pdfTable = new PdfPTable(dataGridView1.ColumnCount);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.DefaultCell.BorderWidth = 1;

                //Adding Header row
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                    pdfTable.AddCell(cell);
                }

                //Adding DataRow
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        pdfTable.AddCell(cell.Value.ToString());
                    }
                }


                //Exporting to PDF
                string folderPath = "C:\\PDFs\\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using (FileStream stream = new FileStream(folderPath + "BKReports.pdf", FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();

                }
                MessageBox.Show("The file saved PDF Format!!!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
