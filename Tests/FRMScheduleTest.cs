using DVLD_BuisnessLayer;
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
    public partial class FRMScheduleTest : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestTypes.enTestType _TestTypeID = clsTestTypes.enTestType.VisionTest;
        private int _AppointmentID = -1;
        public FRMScheduleTest(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestType TestTypeID,int AppointmentID=-1)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;
            _AppointmentID = AppointmentID;
            _LocalDrivingLicenseApplicationID= LocalDrivingLicenseApplicationID;
        }
        private void FRMScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _TestTypeID;
            ctrlScheduleTest1.LoadInfo(_LocalDrivingLicenseApplicationID, _AppointmentID);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
