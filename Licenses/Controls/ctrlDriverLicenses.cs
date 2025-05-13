using DVLD_BuisnessLayer;
using DVLD_Project.Licenses.International_License;
using DVLD_Project.Licenses.Local_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID;
        private clsDriver _Driver;
        private DataTable _dtDriverLocalLicenseHistory;
        private DataTable _dtDriverInternationlLicenseHistory;
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }
        private void _LoadLocalLicenseInfo()
        {
            _dtDriverLocalLicenseHistory = clsDriver.GetLocalLicense(_DriverID);
            dgvLocalLicensesHistory.DataSource = _dtDriverLocalLicenseHistory;
            lblLocalLicensesRecords.Text=dgvLocalLicensesHistory.Rows.Count.ToString();

            if (dgvLocalLicensesHistory.Rows.Count > 0) 
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 110;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 110;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 270;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Data";
                dgvLocalLicensesHistory.Columns[3].Width = 170;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 170;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 110;
            }
        }
        private void _LoadInternationalLicenseInfo()
        {
            _dtDriverInternationlLicenseHistory = clsDriver.GetInternationalLicense(_DriverID);
            dgvInternationalLicensesHistory.DataSource = _dtDriverInternationlLicenseHistory;
            lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();

            if(dgvInternationalLicensesHistory.Rows.Count>0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 160;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 130;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 180;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 180;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 180;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 120;
            }
        }
        public void LoadInfo(int DriverID)
        {
            _DriverID = DriverID;
            _Driver=clsDriver.FindByDriverID(DriverID);

            if (_Driver == null)
            {
                MessageBox.Show("There is no Driver with id = " + _DriverID, "Error", MessageBoxButtons.OK);
                return;
            }

            _LoadInternationalLicenseInfo();
            _LoadLocalLicenseInfo();
        }
        public void LoadInfoByPersonID(int PersonID)
        {
            _Driver = clsDriver.FindByPersonID(PersonID);

            if (_Driver == null)
            {
                MessageBox.Show("There is no Driver Linked with Person ID = " + PersonID, "Error", MessageBoxButtons.OK);
                return;
            }

            _DriverID = _Driver.DriverID;
            _LoadInternationalLicenseInfo();
            _LoadLocalLicenseInfo();
        }
        public void Clear()
        {
            _dtDriverLocalLicenseHistory.Clear();
            _dtDriverInternationlLicenseHistory.Clear();
        }
        private void showLocalLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalLicenseID = (int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value;
            FRMShowLicenseInfo frm=new FRMShowLicenseInfo(LocalLicenseID);
            frm.ShowDialog();
        }
        private void showInternationalLicenseInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dgvInternationalLicensesHistory.CurrentRow.Cells[0].Value;
            FRMShowInternationalLicenseInfo frm = new FRMShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }
    }
}
