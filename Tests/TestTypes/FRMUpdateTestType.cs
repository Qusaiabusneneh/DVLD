using DVLD_BuisnessLayer;
using DVLD_Project.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.TestTypes
{
    public partial class FRMUpdateTestType : Form
    {
        private clsTestTypes.enTestType _TestTypeID =clsTestTypes.enTestType.VisionTest;
        private clsTestTypes _TestType;
        public FRMUpdateTestType(clsTestTypes.enTestType TestType)
        {
            InitializeComponent();
            _TestTypeID = TestType;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FRMUpdateTestType_Load(object sender, EventArgs e)
        {
            _TestType = clsTestTypes.Find(_TestTypeID);
            if (_TestType != null)
            {
                lblTestTypeID.Text=((int) _TestTypeID).ToString();
                txtTitle.Text = _TestType.TestTypeTitle;
                txtDescription.Text = _TestType.TestTypeDescription;
                txtFees.Text = _TestType.TestTypeFees.ToString();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _TestType.TestTypeTitle = txtTitle.Text.Trim();
            _TestType.TestTypeDescription=txtDescription.Text.Trim();
            _TestType.TestTypeFees=Convert.ToDecimal(txtFees.Text.Trim());
            
            if(_TestType.Save())
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title Can't be Empty");
            }
            else
                errorProvider1.SetError(txtTitle, null);
        }
        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDescription, "Description Can't be Empty");
            }
            else
                errorProvider1.SetError(txtDescription, null);
        }
        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees Can't be Empty");
            }
            else
                errorProvider1.SetError(txtFees, null);

            if (!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number");
            }
            else
                errorProvider1.SetError(txtFees, null);
        }
    }
}
