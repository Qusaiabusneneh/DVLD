using DVLD_BuisnessLayer;
using DVLD_Project.Global_Classes;
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
using static DVLD_BuisnessLayer.clsLicense;

namespace DVLD_Project.Applications.Replacement_Damaged_or_Lost_License
{
    public partial class FRMReplacementDamagedORLostLicense : Form
    {
        private int _NewLicenseID = -1;
        public FRMReplacementDamagedORLostLicense()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private int _GetApplicationTypeID()
        {
            if (radDamaged.Checked)
                return (int)clsApplication.enApplicationType.enReplaceDamagedDrivingLicense;
            else
                return (int)clsApplication.enApplicationType.enReplaceLostDrivingLicense;
        }
        private enIssueReason _GetIssueReason()
        {
            if (radDamaged.Checked)
                return enIssueReason.enDamagedReplacement;
            else
                return enIssueReason.enLostReplacement;
        }
        private void FRMReplacementDamagedORLostLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

            radDamaged.Checked = true;
        }
        private void radDamaged_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Damaged License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationTypes.Find(_GetApplicationTypeID()).ApplicationTypeFees.ToString();
        }
        private void radLost_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Lost License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationTypes.Find(_GetApplicationTypeID()).ApplicationTypeFees.ToString();
        }
        private void FRMReplacementDamagedORLostLicense_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            lblOldLicense.Text = SelectedLicenseID.ToString();
            lblShowLicenseHistory.Enabled = (SelectedLicenseID != -1);
            if (SelectedLicenseID == -1)
                return;

            int DefaultValidityLength = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.DefaultValidityLength;
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);

            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }

            btnIssue.Enabled = true;
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsLicense NewLicense =
            ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Replace(_GetIssueReason(), clsGlobal.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Faild to Replacement the License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;
            lblReplacedLicenseID.Text = _NewLicenseID.ToString();

            MessageBox.Show("Licensed Replaced Successfully with ID=" + _NewLicenseID.ToString(), "License Issued",
                 MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssue.Enabled = false;
            grpRemplacementFor.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnable = false;
            lblShowNewLicenseInfo.Enabled = true;
        }
        private void lblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FRMShowPersonLicenseHistory frm = new FRMShowPersonLicenseHistory();
            frm.ShowDialog();
        }
        private void lblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FRMShowLicenseInfo frm = new FRMShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }
    }
}
