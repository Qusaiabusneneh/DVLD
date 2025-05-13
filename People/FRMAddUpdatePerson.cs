using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.People
{
    public partial class FRMAddUpdatePerson : Form
    {
        public enum enMode { enAddNew=0,enUpdate=1};
        public enum enGendor { enMale=0,enFemale=1};
        private enMode _Mode;
        private int _PersonID=-1;
        clsPerson _Person;

        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;
        public FRMAddUpdatePerson()
        {
            InitializeComponent();
            _Mode=enMode.enAddNew;
        }
         public FRMAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _Mode = enMode.enUpdate;
            _PersonID= PersonID;
        }
        private void _FillCountriesInComboBox()
        {
            DataTable dtCountry = clsCountry.GetAllCountries();
            foreach (DataRow row in dtCountry.Rows)
                cmbCountry.Items.Add(row["CountryName"]);
        }
        private void _ResetDefualtValues()
        {
            _FillCountriesInComboBox();
            if(_Mode==enMode.enAddNew)
            {
                lblMode.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lblMode.Text = "Update Person";
            }

            if (radMale.Checked)
                pbPersonImage.Image = Resources.person_man__1_1;
            else
                pbPersonImage.Image = Resources.student_girl;

            lblRemoveImage.Visible = (pbPersonImage.ImageLocation != null);

            //we set the max date to 18 years from today
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            //sShould not allow adding age more than 100 years
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            //this we will set default country to jordan
            cmbCountry.SelectedIndex = cmbCountry.FindString("Jordna");

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            radMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
        }
        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);

            if(_Person==null)
            {
                MessageBox.Show("No Person with ID =" + _PersonID , "Person Not Found ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text=_Person.ThirdName;    
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text= _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (_Person.Gendor == 0)
                radMale.Checked = true;
            else
                radFemale.Checked = true;

            txtPhone.Text = _Person.Phone;
            txtAddress.Text= _Person.Address;
            txtPhone.Text= _Person.Phone;
            txtEmail.Text= _Person.Email;
            cmbCountry.SelectedIndex = cmbCountry.FindString(_Person.CountryInfo.CountryName);

            if(_Person.ImagePath!="")
                pbPersonImage.ImageLocation = _Person.ImagePath;

            lblRemoveImage.Visible = (_Person.ImagePath != "");
        }
        private void FRMAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
            if (_Mode == enMode.enUpdate)
                _LoadData();
        }
        private bool _HandlePersonImage()
        {
            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.

            if (_Person.ImagePath != pbPersonImage.ImageLocation) 
            {
                if (_Person.ImagePath != "") 
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch(IOException)
                    {

                    }
                }

                if(pbPersonImage.ImageLocation!=null)
                {
                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();
                    if(clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!,put the mouse over the red icon(s) to see the error", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage())
                return;

            int NationalityCountryID = clsCountry.Find(cmbCountry.Text).ID;
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName= txtSecondName.Text.Trim();
            _Person.ThirdName= txtThirdName.Text.Trim();   
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.NationalityCountryID = NationalityCountryID;

            if (radMale.Checked)
                _Person.Gendor = (short)enGendor.enMale;
            else
                _Person.Gendor = (short)enGendor.enFemale;

            if (pbPersonImage.ImageLocation != null)
                _Person.ImagePath = pbPersonImage.ImageLocation;

            else
                _Person.ImagePath = "";

            if(_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                _Mode = enMode.enUpdate;
                this.Text = "Update Person";
                MessageBox.Show("Data Saved Successfully...", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
                MessageBox.Show("Error: Data is not Saved Successfully...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void lblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                string SelectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(SelectedFilePath);
                lblRemoveImage.Visible = true;
            }
        }
        private void lblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if (radMale.Checked)
                pbPersonImage.Image = Resources.person_man__1_;
            else
                pbPersonImage.Image = Resources.student_girl;

            lblRemoveImage.Visible = false;
        }
        private void radMale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null) 
                pbPersonImage.Image=Resources.person_man__1_;
        }
        private void radFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.student_girl;
        }
        private void  ValidateEmptyTextBox(object sender,CancelEventArgs e)
        {
            TextBox Temp = ((TextBox)sender);
            if(string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel= true;
                errorProvider1.SetError(Temp, "This filed is required!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
                return;
            if (!clsValidation.ValidationEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format");
            }
            else
                errorProvider1.SetError(txtEmail, null);
        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This Filed is required !");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.isPersonExist(txtNationalNo.Text.Trim())) 
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }
    }
}
