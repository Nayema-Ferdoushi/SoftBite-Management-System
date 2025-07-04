﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace CafeShopManagementSystem
{
    public partial class AdminAddProducts : UserControl
    {
        static string conn = ConfigurationManager.ConnectionStrings["CafeShopDBConnection"].ConnectionString;
        SqlConnection connect = new SqlConnection(conn);

        public AdminAddProducts()
        {
            InitializeComponent();
            displayData();
        }

        public void refreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }
            displayData();
        }

        public bool emptyFields()
        {
            if (string.IsNullOrWhiteSpace(adminAddProducts_id.Text) || string.IsNullOrWhiteSpace(adminAddProducts_name.Text)
                || adminAddProducts_type.SelectedIndex == -1 || string.IsNullOrWhiteSpace(adminAddProducts_stock.Text)
                || string.IsNullOrWhiteSpace(adminAddProducts_price.Text) || adminAddProducts_status.SelectedIndex == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void displayData()
        {
            try
            {
                AdminAddProductsData prodData = new AdminAddProductsData();
                List<AdminAddProductsData> listData = prodData.productsListData();
                DataGridView1.DataSource = listData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void adminAddProducts_addBtn_Click(object sender, EventArgs e)
        {
            if (emptyFields())
            {
                MessageBox.Show("All fields are required to be filled.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State == ConnectionState.Closed)
                {
                    try
                    {
                        connect.Open();

                        string selectProdID = "SELECT * FROM products WHERE prod_id = @prodID";
                        using (SqlCommand selectPID = new SqlCommand(selectProdID, connect))
                        {
                            selectPID.Parameters.AddWithValue("@prodID", adminAddProducts_id.Text.Trim());
                            SqlDataAdapter adapter = new SqlDataAdapter(selectPID);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                MessageBox.Show("Product ID: " + adminAddProducts_id.Text.Trim() + " is taken already", "Error Message",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string insertData = "INSERT INTO products (prod_id, prod_name, prod_type, " +
                                    "prod_stock, prod_price, prod_status, prod_image, date_insert) VALUES(@prodID, @prodName, " +
                                    "@prodType, @prodStock, @prodPrice, @prodStatus, @prodImage, @dateInsert)";

                                DateTime today = DateTime.Today;

                                string basePath = Path.Combine(Application.StartupPath, "Images");
                                if (!Directory.Exists(basePath))
                                {
                                    Directory.CreateDirectory(basePath);
                                }
                                string path = Path.Combine(basePath, adminAddProducts_id.Text.Trim() + ".jpg");

                                if (string.IsNullOrEmpty(adminAddProducts_imageView.ImageLocation) || !File.Exists(adminAddProducts_imageView.ImageLocation))
                                {
                                    MessageBox.Show("Please select a valid image file.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                try
                                {
                                    File.Copy(adminAddProducts_imageView.ImageLocation, path, true);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    MessageBox.Show("Insufficient permissions to copy the image. Run as Administrator or choose a writable directory.",
                                        "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                catch (IOException ex)
                                {
                                    MessageBox.Show("File access error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                using (SqlCommand cmd = new SqlCommand(insertData, connect))
                                {
                                    cmd.Parameters.AddWithValue("@prodID", adminAddProducts_id.Text.Trim());
                                    cmd.Parameters.AddWithValue("@prodName", adminAddProducts_name.Text.Trim());
                                    cmd.Parameters.AddWithValue("@prodType", adminAddProducts_type.Text.Trim());
                                    cmd.Parameters.AddWithValue("@prodStock", adminAddProducts_stock.Text.Trim());
                                    cmd.Parameters.AddWithValue("@prodPrice", adminAddProducts_price.Text.Trim());
                                    cmd.Parameters.AddWithValue("@prodStatus", adminAddProducts_status.Text.Trim());
                                    cmd.Parameters.AddWithValue("@prodImage", path);
                                    cmd.Parameters.AddWithValue("@dateInsert", today);

                                    cmd.ExecuteNonQuery();
                                    clearFields();

                                    MessageBox.Show("Added successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    displayData();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed connection: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }

        private void adminAddProducts_importBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files (*.jpg; *.png)|*.jpg;*.png";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    adminAddProducts_imageView.ImageLocation = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void clearFields()
        {
            adminAddProducts_id.Text = "";
            adminAddProducts_name.Text = "";
            adminAddProducts_type.SelectedIndex = -1;
            adminAddProducts_stock.Text = "";
            adminAddProducts_price.Text = "";
            adminAddProducts_status.SelectedIndex = -1;
            adminAddProducts_imageView.Image = null;
        }

        private void adminAddProducts_clearBtn_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = DataGridView1.Rows[e.RowIndex];
                adminAddProducts_id.Text = row.Cells[1].Value?.ToString() ?? "";
                adminAddProducts_name.Text = row.Cells[2].Value?.ToString() ?? "";
                adminAddProducts_type.Text = row.Cells[3].Value?.ToString() ?? "";
                adminAddProducts_stock.Text = row.Cells[4].Value?.ToString() ?? "";
                adminAddProducts_price.Text = row.Cells[5].Value?.ToString() ?? "";
                adminAddProducts_status.Text = row.Cells[6].Value?.ToString() ?? "";

                string imagepath = row.Cells[7].Value?.ToString() ?? "";
                try
                {
                    if (!string.IsNullOrEmpty(imagepath) && File.Exists(imagepath))
                    {
                        adminAddProducts_imageView.Image = Image.FromFile(imagepath);
                    }
                    else
                    {
                        adminAddProducts_imageView.Image = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Image: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void adminAddProducts_updateBtn_Click(object sender, EventArgs e)
        {
            if (emptyFields())
            {
                MessageBox.Show("All fields are required to be filled", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Are you sure you want to Update Product ID: " + adminAddProducts_id.Text.Trim() + "?",
                    "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        try
                        {
                            connect.Open();

                            string updateData = "UPDATE products SET prod_name = @prodName, prod_type = @prodType, " +
                                "prod_stock = @prodStock, prod_price = @prodPrice, prod_status = @prodStatus, date_update = @dateUpdate " +
                                "WHERE prod_id = @prodID";
                            DateTime today = DateTime.Today;

                            using (SqlCommand updateD = new SqlCommand(updateData, connect))
                            {
                                updateD.Parameters.AddWithValue("@prodName", adminAddProducts_name.Text.Trim());
                                updateD.Parameters.AddWithValue("@prodType", adminAddProducts_type.Text.Trim());
                                updateD.Parameters.AddWithValue("@prodStock", adminAddProducts_stock.Text.Trim());
                                updateD.Parameters.AddWithValue("@prodPrice", adminAddProducts_price.Text.Trim());
                                updateD.Parameters.AddWithValue("@prodStatus", adminAddProducts_status.Text.Trim());
                                updateD.Parameters.AddWithValue("@dateUpdate", today);
                                updateD.Parameters.AddWithValue("@prodID", adminAddProducts_id.Text.Trim());

                                updateD.ExecuteNonQuery();
                                clearFields();

                                MessageBox.Show("Updated successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                displayData();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Connection failed: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connect.Close();
                        }
                    }
                }
            }
        }

        private void adminAddProducts_deleteBtn_Click(object sender, EventArgs e)
        {
            if (emptyFields())
            {
                MessageBox.Show("All fields are required to be filled", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Are you sure you want to Delete Product ID: " + adminAddProducts_id.Text.Trim() + "?",
                    "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        try
                        {
                            connect.Open();

                            string updateData = "UPDATE products SET date_delete = @dateDelete WHERE prod_id = @prodID";
                            DateTime today = DateTime.Today;

                            using (SqlCommand updateD = new SqlCommand(updateData, connect))
                            {
                                updateD.Parameters.AddWithValue("@dateDelete", today);
                                updateD.Parameters.AddWithValue("@prodID", adminAddProducts_id.Text.Trim());

                                updateD.ExecuteNonQuery();
                                clearFields();

                                MessageBox.Show("Removed successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                displayData();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Connection failed: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connect.Close();
                        }
                    }
                }
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // No implementation needed unless specific cell content click behavior is required
        }
    }
}