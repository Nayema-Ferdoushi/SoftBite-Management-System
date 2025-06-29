using System;
using System.Drawing;
using System.Windows.Forms;

namespace CafeShopManagementSystem
{
    public partial class AdminMainForm : Form
    {
        private AdminDashboardForm adminDashboardForm1;
        private AdminAddUsers adminAddUsers1;
        private AdminAddProducts adminAddProducts1;
        private CashierCustomersForm cashierCustomersForm1;

        public AdminMainForm()
        {
            InitializeComponent();
            InitializeControls();
            this.Resize += AdminMainForm_Resize; // Subscribe to resize event
            this.FormBorderStyle = FormBorderStyle.Sizable; // Enable resizable border
            this.MinimumSize = new Size(800, 600); // Set a minimum size to prevent excessive shrinking
        }

        private void InitializeControls()
        {
            // Instantiate all user controls
            adminDashboardForm1 = new AdminDashboardForm();
            adminAddUsers1 = new AdminAddUsers();
            adminAddProducts1 = new AdminAddProducts();
            cashierCustomersForm1 = new CashierCustomersForm();

            // Add controls to panel3 and set initial properties
            panel3.Controls.Add(adminDashboardForm1);
            panel3.Controls.Add(adminAddUsers1);
            panel3.Controls.Add(adminAddProducts1);
            panel3.Controls.Add(cashierCustomersForm1);

            adminDashboardForm1.Location = new Point(0, 0);
            adminAddUsers1.Location = new Point(0, 0);
            adminAddProducts1.Location = new Point(0, 0);
            cashierCustomersForm1.Location = new Point(0, 0);

            adminDashboardForm1.Size = panel3.Size;
            adminAddUsers1.Size = panel3.Size;
            adminAddProducts1.Size = panel3.Size;
            cashierCustomersForm1.Size = panel3.Size;

            // Set initial visibility
            adminDashboardForm1.Visible = true;
            adminAddUsers1.Visible = false;
            adminAddProducts1.Visible = false;
            cashierCustomersForm1.Visible = false;
        }

        private void AdminMainForm_Resize(object sender, EventArgs e)
        {
            // Adjust panel sizes and positions based on form resize
            panel1.Size = new Size(this.ClientSize.Width, panel1.Height); // Top panel full width
            panel2.Size = new Size(panel2.Width, this.ClientSize.Height - panel1.Height); // Left panel full height
            panel3.Size = new Size(this.ClientSize.Width - panel2.Width, this.ClientSize.Height - panel1.Height); // Main panel fills remaining space
            panel3.Location = new Point(panel2.Width, panel1.Height);

            // Update user control sizes to match panel3
            if (adminDashboardForm1?.Visible == true) adminDashboardForm1.Size = panel3.Size;
            else if (adminAddUsers1?.Visible == true) adminAddUsers1.Size = panel3.Size;
            else if (adminAddProducts1?.Visible == true) adminAddProducts1.Size = panel3.Size;
            else if (cashierCustomersForm1?.Visible == true) cashierCustomersForm1.Size = panel3.Size;
        }

        private void close_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Are you sure you want to Sign out?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminDashboardForm1.Visible = true;
            adminAddUsers1.Visible = false;
            adminAddProducts1.Visible = false;
            cashierCustomersForm1.Visible = false;

            if (adminDashboardForm1 != null)
            {
                adminDashboardForm1.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            adminDashboardForm1.Visible = false;
            adminAddUsers1.Visible = true;
            adminAddProducts1.Visible = false;
            cashierCustomersForm1.Visible = false;

            if (adminAddUsers1 != null)
            {
                adminAddUsers1.refreshData();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            adminDashboardForm1.Visible = false;
            adminAddUsers1.Visible = false;
            adminAddProducts1.Visible = true;
            cashierCustomersForm1.Visible = false;

            if (adminAddProducts1 != null)
            {
                adminAddProducts1.refreshData();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            adminDashboardForm1.Visible = false;
            adminAddUsers1.Visible = false;
            adminAddProducts1.Visible = false;
            cashierCustomersForm1.Visible = true;

            if (cashierCustomersForm1 != null)
            {
                cashierCustomersForm1.refreshData();
            }
        }
    }
}