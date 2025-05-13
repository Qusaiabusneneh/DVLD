using DVLD_BuisnessLayer;
using DVLD_Project.Applications.Local_Licenses;
using DVLD_Project.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.Local_Licenses
{
    public partial class FRMManageLocalDrivingLicenseApplication : Form
    {
        private DataTable _dtAllLocalDrivingLicenseApplications=clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicneseApplications();
        public FRMManageLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }
        private void _FillComboBoxFilter()
        {
            DataTable dt = _dtAllLocalDrivingLicenseApplications;
            foreach(DataColumn column in dt.Columns)
                cmbFilter.Items.Add(column.ToString());
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FRMLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _FillComboBoxFilter();
             _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicneseApplications();
            cmbFilter.SelectedIndex = 1;
            DGVLocalLicnese.DataSource = _dtAllLocalDrivingLicenseApplications;
            lblRecord.Text=DGVLocalLicnese.Rows.Count.ToString();

            if (DGVLocalLicnese.Rows.Count > 0)
            {
                DGVLocalLicnese.Columns[0].HeaderText = "L.D.L AppID";
                DGVLocalLicnese.Columns[0].Width = 120;

                DGVLocalLicnese.Columns[1].HeaderText = "Driving Class";
                DGVLocalLicnese.Columns[1].Width = 300;

                DGVLocalLicnese.Columns[2].HeaderText = "National No.";
                DGVLocalLicnese.Columns[2].Width = 120;

                DGVLocalLicnese.Columns[3].HeaderText = "Full Name";
                DGVLocalLicnese.Columns[3].Width = 300;

                DGVLocalLicnese.Columns[4].HeaderText = "Application Date";
                DGVLocalLicnese.Columns[4].Width = 120;

                DGVLocalLicnese.Columns[5].HeaderText = "Passed Test";
                DGVLocalLicnese.Columns[5].Width = 120;
            }
        }
        private void btnAddLocalLicense_Click(object sender, EventArgs e)
        {
            FRMAddUpdateLocalDrivingLicenseApplication frm = new FRMAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
            FRMLocalDrivingLicenseApplication_Load(null, null);
        }
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cmbFilter.Text != "None");
            if(txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }
            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lblRecord.Text=DGVLocalLicnese.Rows.Count.ToString();
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            clsFilterDGV.Filter(txtFilter.Text, cmbFilter.Text, DGVLocalLicnese, _dtAllLocalDrivingLicenseApplications);

            string FilterColumn = "";
            switch(cmbFilter.Text)
            {
                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if(txtFilter.Text.Trim()==""||FilterColumn=="None")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecord.Text=_dtAllLocalDrivingLicenseApplications.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "LocalDrivingLicenseApplicationID")
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            
            lblRecord.Text=DGVLocalLicnese.Rows.Count.ToString();
        }
        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilter.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            FRMLocalDrivingLicenseApplicationInfo frm =
                new FRMLocalDrivingLicenseApplicationInfo(ApplicationID);
            frm.ShowDialog();

            FRMLocalDrivingLicenseApplication_Load(null, null);
        }
        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            FRMAddUpdateLocalDrivingLicenseApplication frm = new FRMAddUpdateLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
            FRMLocalDrivingLicenseApplication_Load(null, null);
        }
        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to cancel this application?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication != null)
            {
                if(LocalDrivingLicenseApplication.Cancel())
                {
                    MessageBox.Show("Application Cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FRMLocalDrivingLicenseApplication_Load(null, null);
                }
                else
                    MessageBox.Show("Could not cancel applicatoin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to delete this application?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            if(LocalDrivingLicenseApplication!=null)
            {
                if(LocalDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FRMLocalDrivingLicenseApplication_Load(null, null);
                }
                else
                    MessageBox.Show("Could not delete applicatoin, other data depends on it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void vistionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            clsTestAppointment Appointment =
                clsTestAppointment.GetLastTestAppointment(LocalDrivingLicenseApplicationID, clsTestTypes.enTestType.VisionTest);

            if (Appointment != null)
            {
                MessageBox.Show("No Vision Test Appointment Found!", "Set Appointment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FRMTakeTest frm = new FRMTakeTest(Appointment.TestAppointmentID, clsTestTypes.enTestType.VisionTest);
            frm.ShowDialog();
        }
        private void sechduleEyesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestType.VisionTest);
        }
        private void writtenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicneseApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            if (!clsLocalDrivingLicenseApplication.DoesPassTestType(LocalDrivingLicneseApplicationID, clsTestTypes.enTestType.VisionTest))
            {
                MessageBox.Show("Person Should Pass the Vision Test First!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTestAppointment Appointment =
                clsTestAppointment.GetLastTestAppointment(LocalDrivingLicneseApplicationID, clsTestTypes.enTestType.WrittenTest);

            if (Appointment == null)
            {
                MessageBox.Show("No Written Test Appointment Found!", "Set Appointment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FRMTakeTest frm = new FRMTakeTest(Appointment.TestAppointmentID, clsTestTypes.enTestType.WrittenTest);
            frm.ShowDialog();
        }
        private void sechduleWrittineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestType.WrittenTest);
        }
        private void streetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenceApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            if (!clsLocalDrivingLicenseApplication.DoesPassTestType(LocalDrivingLicenceApplicationID, clsTestTypes.enTestType.WrittenTest))
            {
                MessageBox.Show("Person Should Pass the Written Test First!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTestAppointment Appointment =
                clsTestAppointment.GetLastTestAppointment(LocalDrivingLicenceApplicationID, clsTestTypes.enTestType.StreetTest);

            if (Appointment == null)
            {
                MessageBox.Show("No Street Test Appointment Found!", "Set Appointment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FRMTakeTest frm = new FRMTakeTest(Appointment.TestAppointmentID, clsTestTypes.enTestType.StreetTest);
            frm.ShowDialog();
        }
        private void sechduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestType.StreetTest);
        }
        private void _ScheduleTest(clsTestTypes.enTestType TestType)
        {
            int LocalDrivingLicenseApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            FRMListTestAppointments frm= new FRMListTestAppointments(LocalDrivingLicenseApplicationID,TestType);
            frm.ShowDialog();
            FRMLocalDrivingLicenseApplication_Load(null, null);
        }
        private void issueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            FRMIssueDriverLicenseFirstTime frm = new FRMIssueDriverLicenseFirstTime(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
            FRMLocalDrivingLicenseApplication_Load(null, null);
        }
        private void CMSLocalDrivingLicnese_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            int TotalPassedTest = (int)DGVLocalLicnese.CurrentRow.Cells[5].Value;
            bool LicenseExists = LocalDrivingLicenseApplication.IsLicenseIssued();

            issueToolStripMenuItem.Enabled = (TotalPassedTest == 3) && !LicenseExists;

            showLicenseToolStripMenuItem.Enabled = LicenseExists;

            editApplicationToolStripMenuItem.Enabled =
                !LicenseExists && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.enNew);

            ScheduleTestsMenue.Enabled = !LicenseExists;

            cancelApplicationToolStripMenuItem.Enabled =
                (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.enNew);

            deleteApplicationToolStripMenuItem.Enabled =
                (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.enNew);

            bool PassedVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.VisionTest);
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.StreetTest);

            ScheduleTestsMenue.Enabled =
                (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest)
                && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.enNew);

            if(ScheduleTestsMenue.Enabled)
            {
                sechduleEyesToolStripMenuItem.Enabled = !PassedVisionTest;
                sechduleWrittineToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;
                sechduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }
        }
        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            int LicenseID=
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                FRMShowLicenseInfo frm = new FRMShowLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void showPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)DGVLocalLicnese.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            FRMShowPersonLicenseHistory frm = new FRMShowPersonLicenseHistory(localDrivingLicenseApplication.ApplicantPersonID);
            frm.ShowDialog();

        }
    }
}
