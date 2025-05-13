using DVLD_BuisnessLayer;
using DVLD_Project.Licenses.Local_Licenses;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Tests
{
    public partial class FRMListTestAppointments : Form
    {
        private DataTable _dtLicneseTestAppointments;
        private int _LocalDrivingLicenseApplicationID;
        private clsTestTypes.enTestType _TestType = clsTestTypes.enTestType.VisionTest;
        public FRMListTestAppointments(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestType TestType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestType = TestType;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _LoadTestTypeImageAndTitle()
        {
            switch(_TestType)
            {
                case clsTestTypes.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointment";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.eye;
                        break;
                    }
                case clsTestTypes.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointment";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.test_write;
                        break;
                    }
                case clsTestTypes.enTestType.StreetTest:
                    {
                        lblTitle.Text = "Street Test Appointment";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.traffic_light;
                        break;
                    }
            }
        }
        private void FRMListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();

            ctrlDrivingLicneseApplication1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
            _dtLicneseTestAppointments =
                clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestType);

            dgvLicenseTestAppointments.DataSource = _dtLicneseTestAppointments;
            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();

            if(dgvLicenseTestAppointments.Rows.Count>0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 150;

                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 200;

                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 150;

                dgvLicenseTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width = 100;
            }
        }
        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestType))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTest LastTest = LocalDrivingLicenseApplication.GetLastTestPerTestType(_TestType);

            //Schedule Test Per First Time
            if (LastTest == null)
            {
                FRMScheduleTest frm1 = new FRMScheduleTest(_LocalDrivingLicenseApplicationID, _TestType);
                frm1.ShowDialog();
                FRMListTestAppointments_Load(null, null);
                return;
            }

            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FRMScheduleTest frm2 = new FRMScheduleTest(_LocalDrivingLicenseApplicationID, _TestType);
            frm2.ShowDialog();
            FRMListTestAppointments_Load(null, null);
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;
            FRMScheduleTest frm = new FRMScheduleTest(_LocalDrivingLicenseApplicationID, _TestType, TestAppointmentID);
            frm.ShowDialog();
            FRMListTestAppointments_Load(null, null);
        }
        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;
            FRMTakeTest frm = new FRMTakeTest(TestAppointmentID,_TestType);
            frm.ShowDialog();
            FRMListTestAppointments_Load(null, null);
        }
    }
}
