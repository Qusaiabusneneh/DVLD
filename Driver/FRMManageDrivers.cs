using DVLD_BuisnessLayer;
using DVLD_Project.Licenses.Local_Licenses;
using DVLD_Project.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Driver
{
    public partial class FRMManageDrivers : Form
    {
        private clsDriver _Driver;
        private int _DriverID = -1;
        private DataTable _dtDriver = clsDriver.GetAllDrivers();
        public FRMManageDrivers()
        {
            InitializeComponent();
        }
        private void _FillComboBoxFilter()
        {
            DataTable dt = _dtDriver;
            foreach (DataColumn column in dt.Columns) 
            {
                cmbFilter.Items.Add(column.ToString());
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FRMManageDrivers_Load(object sender, EventArgs e)
        {
            _FillComboBoxFilter();
            cmbFilter.SelectedIndex = 0;
            _dtDriver = clsDriver.GetAllDrivers();
            DGVDrivers.DataSource = _dtDriver;
            lblRecord.Text=DGVDrivers.Rows.Count.ToString();
            if(DGVDrivers.Rows.Count>0)
            {
                DGVDrivers.Columns[0].HeaderText = "Driver ID";
                DGVDrivers.Columns[0].Width = 120;

                DGVDrivers.Columns[1].HeaderText = "Person ID";
                DGVDrivers.Columns[1].Width = 120;

                DGVDrivers.Columns[2].HeaderText = "National No.";
                DGVDrivers.Columns[2].Width = 140;

                DGVDrivers.Columns[3].HeaderText = "Full Name";
                DGVDrivers.Columns[3].Width = 320;

                DGVDrivers.Columns[4].HeaderText = "Date";
                DGVDrivers.Columns[4].Width = 170;

                DGVDrivers.Columns[5].HeaderText = "Active Licenses";
                DGVDrivers.Columns[5].Width = 150;
            }
        }
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cmbFilter.Text != "None");

            if (cmbFilter.Text == "None")
                txtFilter.Enabled = false;
            else
                txtFilter.Enabled = true;

            txtFilter.Text = "";
            txtFilter.Focus();
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            clsFilterDGV.Filter(txtFilter.Text, cmbFilter.Text, DGVDrivers, _dtDriver);

            string FilterColumn = "";
            switch(cmbFilter.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }
            if (txtFilter.Text.Trim() == "" || FilterColumn == "None") 
            {
                _dtDriver.DefaultView.RowFilter = "";
                lblRecord.Text=DGVDrivers.Rows.Count.ToString();
                return;
            }
            if (FilterColumn != "FullName" && FilterColumn != "NationalNo")
                _dtDriver.DefaultView.RowFilter = string.Format("[{0}] = 1", FilterColumn, txtFilter.Text.Trim());
            else
                _dtDriver.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());

            lblRecord.Text = DGVDrivers.Rows.Count.ToString();
        }
        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilter.Text == "Driver ID" || cmbFilter.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)DGVDrivers.CurrentRow.Cells[1].Value;
            FRMPersonCard frm = new FRMPersonCard(PersonID);
            frm.ShowDialog();
        }
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)DGVDrivers.CurrentRow.Cells[1].Value;
            FRMShowPersonLicenseHistory frm = new FRMShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }
    }
}
