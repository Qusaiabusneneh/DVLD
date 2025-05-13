using DVLD_BuisnessLayer;
using DVLD_Project.Global_Classes;
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
    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { enAddNew=0,enUpdate=1};
        private enMode _Mode = enMode.enAddNew;

        public enum enCreationMode { enFirstTimeSchedule=0,enRetakeTestSchedule=1};
        private enCreationMode _CreationMode=enCreationMode.enFirstTimeSchedule;

        private clsTestTypes.enTestType _TestTypeID = clsTestTypes.enTestType.VisionTest;

        private clsLocalDrivingLicenseApplication _LocaDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;

        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;

        public clsTestTypes.enTestType TestTypeID
        {
            get { return _TestTypeID; }

            set
            {
                _TestTypeID = value;

                switch(_TestTypeID)
                {
                    case clsTestTypes.enTestType.VisionTest:
                        {
                            gbTestType.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.eye;
                            break;
                        }
                    case clsTestTypes.enTestType.WrittenTest:
                        {
                            gbTestType.Text = "Written Test";
                            pbTestTypeImage.Image = Resources.test_write;
                            break;
                        }
                    case clsTestTypes.enTestType.StreetTest:
                        {
                            gbTestType.Text = "Street Test";
                            pbTestTypeImage.Image = Resources.traffic_light;
                            break;
                        }
                }
            }
        }
        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text=_TestAppointment.PaidFees.ToString();

            //we compare the current date with the appointment date to set the min date.
            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtpTestDate.MinDate = DateTime.Now;
            else
                dtpTestDate.MinDate = _TestAppointment.AppointmentDate;

            dtpTestDate.Value = _TestAppointment.AppointmentDate;

            if (_TestAppointment.RetakeTestApplicationID == -1) 
            {
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            else
            {
                lblRetakeAppFees.Text = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestAppID.Text=_TestAppointment.RetakeTestApplicationID.ToString();
            }
            return true;
        }
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.enAddNew &&
                clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID, _TestTypeID)) 
            {
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }
            return true;
        }
        private bool _HandleAppointmentLockedConstraint()
        {
            //if appointment is locked that means the person already sat for this test
            //we cannot update locked appointment

            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for the test, appointment loacked.";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
                lblUserMessage.Visible = false;

            return true;
        }
        private bool _HandlePreviousTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannno apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch(TestTypeID)
            {
                case clsTestTypes.enTestType.VisionTest:
                    lblUserMessage.Visible = false;
                    return true;

                case clsTestTypes.enTestType.WrittenTest:

                    if(!_LocaDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Vision Test should be Passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled=false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled=true;
                    }

                    return true;

                case clsTestTypes.enTestType.StreetTest:

                    if(!_LocaDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Written Test Should be Passed First";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled=false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible=false;
                        btnSave.Enabled=true;
                        dtpTestDate.Enabled=true;
                    }
                    return true;
            }
            return true;
        }
        private bool _HandleRetakeApplication()
        {
            if (_Mode == enMode.enAddNew && _CreationMode == enCreationMode.enRetakeTestSchedule) 
            {
                clsApplication Applications=new clsApplication();
                Applications.ApplicantPersonID = _LocaDrivingLicenseApplication.ApplicantPersonID;
                Applications.ApplicationDate = DateTime.Now;
                Applications.ApplicationTypeID = (int)clsApplication.enApplicationType.enRetakeTest;
                Applications.ApplicationStatus = clsApplication.enApplicationStatus.enCompleted;
                Applications.LastStatusDate= DateTime.Now;
                Applications.PaidFees = clsApplicationTypes.Find((int)clsApplication.enApplicationType.enRetakeTest).ApplicationTypeFees;
                Applications.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if(!Applications.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                _TestAppointment.RetakeTestApplicationID = Applications.ApplicationID;
            }
            return true;
        }
        public void LoadInfo(int LocalDrivingLicenseApplicationID,int AppointmentID=-1)
        {
            if (AppointmentID == -1)
                _Mode = enMode.enAddNew;
            else
                _Mode = enMode.enUpdate;

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = AppointmentID;
            _LocaDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if (_LocaDrivingLicenseApplication == null) 
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            //check The TestAppointment is Retake or First Time
            if (_LocaDrivingLicenseApplication.DoesAttendTestType(_TestTypeID)) 
                _CreationMode = enCreationMode.enRetakeTestSchedule;
            else
                _CreationMode = enCreationMode.enFirstTimeSchedule;

            if (_CreationMode == enCreationMode.enRetakeTestSchedule) 
            {
                lblRetakeAppFees.Text=
                    clsApplicationTypes.Find((int)clsApplication.enApplicationType.enRetakeTest).ApplicationTypeFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestAppID.Text = "0";
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }

            lblLocalDrivingLicenseAppID.Text = _LocaDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocaDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocaDrivingLicenseApplication.PersonFullName;

            //this will show the trials for this test before  
            //Number of Trial Test
            lblTrial.Text = _LocaDrivingLicenseApplication.TotalTrialsPerTest(_TestTypeID).ToString();

            if (_Mode == enMode.enAddNew)
            {
                lblFees.Text=clsTestTypes.Find(_TestTypeID).TestTypeFees.ToString();
                dtpTestDate.MinDate=DateTime.Now;
                lblRetakeTestAppID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }

            lblTotalFees.Text = (Convert.ToDecimal(lblFees.Text) + Convert.ToDecimal(lblRetakeAppFees.Text)).ToString();

            if (!_HandleActiveTestAppointmentConstraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (_HandlePreviousTestConstraint())
                return;
        }
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocaDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToDecimal(lblFees.Text);
            _TestAppointment.CreatedByUserID=clsGlobal.CurrentUser.UserID;

            if(_TestAppointment.Save())
            {
                _Mode = enMode.enUpdate;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
