using DevExpress.XtraSplashScreen;
using DVLD_Buisness;
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

namespace DVLD_Project.User
{
    public partial class FRMAddUpdateUser : Form
    {
        public enum enMode {enAddNew=0, enUpdate=1}
         private enMode _Mode;
        private int _UserID = -1;
        private clsUser _User;
        public FRMAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.enAddNew;
        }
        public FRMAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _Mode=enMode.enUpdate;
            _UserID = UserID;
        }
        private void _RefreshDefaultValues()
        {
            if(_Mode==enMode.enAddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();

                tpLoginInfo.Enabled=false;

                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;
        }
        private void _LoadData()
        {
            _User = clsUser.FindByUserID(_UserID);
            ctrlPersonCardWithFilter1.FilterEnable = false;

            if (_User == null) 
            {
                MessageBox.Show("No User With ID = " + _User, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text= _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
        }
        private void FRMAddUpdateUser_Load(object sender, EventArgs e)
        {
            _RefreshDefaultValues();

            if (_Mode == enMode.enUpdate)
                _LoadData();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fileds are not Valide !, put the mouse over the red icon(s) to see the error "
                    , "Validation Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();
            _User.Password=  txtPassword.Text.Trim();
            _User.IsActive=  chkIsActive.Checked;

            if(_User.Save())
            {
                lblUserID.Text= _User.UserID.ToString();
                _Mode = enMode.enUpdate;
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                MessageBox.Show("Data Saved Successfully...", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error : Data Is not Saved Successfully...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtConfirmPassword.Text.Trim()!=txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation Does not match Password!");
            }
            else
                errorProvider1.SetError(txtConfirmPassword, null);
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
            }
            else
                errorProvider1.SetError(txtPassword, null);
        }
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "UserName cannot be balnk");
                return;
            }
            else
                errorProvider1.SetError(txtUserName, null);

            if(_Mode==enMode.enAddNew)
            {
                if (clsUser.isUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "UserName is used by another User...");
                    return;
                }
                else
                    errorProvider1.SetError(txtUserName, null);
            }
            else
            {
                if(_User.UserName!=txtUserName.Text.Trim())
                {
                    if (clsUser.isUserExist(txtUserName.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "UserName is used by another User...");
                        return;
                    }
                    else
                        errorProvider1.SetError(txtUserName, null);
                }
            }
        }
        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if(_Mode==enMode.enUpdate)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID != -1) 
            {
                if(clsUser.isUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one...", "Selected another Person",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }
                else
                {
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                }
            }

            else
            {
                MessageBox.Show("Please selected a Person ", "Selected a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }

        }
        private void FRMAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
