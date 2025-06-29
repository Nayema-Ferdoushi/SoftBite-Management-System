using System;
using System.Drawing;
using System.Windows.Forms;

namespace CafeShopManagementSystem
{
    public partial class CashierMainForm : Form
    {
        private AdminDashboardForm adminDashboardForm1;
        private AdminAddProducts adminAddProducts1;
        private CashierOrderForm cashierOrderForm1;
        private CashierCustomersForm cashierCustomersForm1;

        public CashierMainForm()
        {
            InitializeComponent();
            InitializeControls();
            this.Resize += CashierMainForm_Resize; // Subscribe to resize event
            this.FormBorderStyle = FormBorderStyle.Sizable; // Enable resizable border
            this.MinimumSize = new Size(800, 600); // Set a minimum size to prevent excessive shrinking
        }

        private void InitializeControls()
        {
            // Instantiate all user controls
            adminDashboardForm1 = new AdminDashboardForm();
            adminAddProducts1 = new AdminAddProducts();
            cashierOrderForm1 = new CashierOrderForm();
            cashierCustomersForm1 = new CashierCustomersForm();

            // Add controls to panel4 and set initial properties
            panel4.Controls.Add(adminDashboardForm1);
            panel4.Controls.Add(adminAddProducts1);
            panel4.Controls.Add(cashierOrderForm1);
            panel4.Controls.Add(cashierCustomersForm1);

            adminDashboardForm1.Location = new Point(0, 0);
            adminAddProducts1.Location = new Point(0, 0);
            cashierOrderForm1.Location = new Point(0, 0);
            cashierCustomersForm1.Location = new Point(0, 0);

            adminDashboardForm1.Size = panel4.Size;
            adminAddProducts1.Size = panel4.Size;
            cashierOrderForm1.Size = panel4.Size;
            cashierCustomersForm1.Size = panel4.Size;

            // Set initial visibility
            adminDashboardForm1.Visible = true;
            adminAddProducts1.Visible = false;
            cashierOrderForm1.Visible = false;
            cashierCustomersForm1.Visible = false;
        }

        private void CashierMainForm_Resize(object sender, EventArgs e)
        {
            // Adjust panel sizes and positions based on form resize
            panel1.Size = new Size(this.ClientSize.Width, panel1.Height); // Top panel full width
            panel2.Size = new Size(panel2.Width, this.ClientSize.Height - panel1.Height); // Left panel full height
            panel3.Size = new Size(this.ClientSize.Width - panel2.Width, this.ClientSize.Height - panel1.Height); // Main panel fills remaining space
            panel3.Location = new Point(panel2.Width, panel1.Height);

            // Update user control sizes to match panel4
            if (adminDashboardForm1?.Visible == true) adminDashboardForm1.Size = panel4.Size;
            else if (adminAddProducts1?.Visible == true) adminAddProducts1.Size = panel4.Size;
            else if (cashierOrderForm1?.Visible == true) cashierOrderForm1.Size = panel4.Size;
            else if (cashierCustomersForm1?.Visible == true) cashierCustomersForm1.Size = panel4.Size;
        }

        private void close_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Are you sure you want to exit?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Are you sure you want to sign out?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Hide();
            }
        }

        private void addProducts_btn_Click(object sender, EventArgs e)
        {
            adminDashboardForm1.Visible = false;
            adminAddProducts1.Visible = true;
            cashierOrderForm1.Visible = false;
            cashierCustomersForm1.Visible = false;

            if (adminAddProducts1 != null)
            {
                adminAddProducts1.refreshData();
            }
        }

        private void dashboard_btn_Click(object sender, EventArgs e)
        {
            adminDashboardForm1.Visible = true;
            adminAddProducts1.Visible = false;
            cashierOrderForm1.Visible = false;
            cashierCustomersForm1.Visible = false;

            if (adminDashboardForm1 != null)
            {
                adminDashboardForm1.Refresh();
            }
        }

        private void order_btn_Click(object sender, EventArgs e)
        {
            adminDashboardForm1.Visible = false;
            adminAddProducts1.Visible = false;
            cashierOrderForm1.Visible = true;
            cashierCustomersForm1.Visible = false;

            if (cashierOrderForm1 != null)
            {
                cashierOrderForm1.refreshData();
            }
        }

        private void customer_btn_Click(object sender, EventArgs e)
        {
            adminDashboardForm1.Visible = false;
            adminAddProducts1.Visible = false;
            cashierOrderForm1.Visible = false;
            cashierCustomersForm1.Visible = true;

            if (cashierCustomersForm1 != null)
            {
                cashierCustomersForm1.refreshData();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}