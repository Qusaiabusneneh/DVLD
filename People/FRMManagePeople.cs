//using DVLD_BussinesLayer;
using DVLD_Buisness;
using DVLD_Project.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.People
{
    public partial class FRMManagePeople : Form
    {
        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();

        //only select the columns that you want to show in the grid
        //private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
        //                                               "FirstName", "SecondName", "ThirdName", "LastName",
        //                                               "GendorCaption", "DateOfBirth", "CountryName",
        //                                               "Phone", "Email");

        //private void _RefreshPeopleList()
        //{
        //    _dtAllPeople = clsPerson.GetAllPeople();
        //    _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
        //                                               "FirstName", "SecondName", "ThirdName", "LastName",
        //                                               "GendorCaption", "DateOfBirth", "CountryName",
        //                                               "Phone", "Email");

        //    DGVPeople.DataSource = _dtPeople;
        //    lblRecord.Text = DGVPeople.Rows.Count.ToString();
        //}

        private void _FillFilterComboBox()
        {
            cmbFilterPersonItem.Items.Clear();
            _dtAllPeople = clsPerson.GetAllPeople();
            foreach(DataColumn column in _dtAllPeople.Columns)
                cmbFilterPersonItem.Items.Add(column.ToString());
            cmbFilterPersonItem.SelectedItem = null;
        }
        public FRMManagePeople()
        {
            InitializeComponent();
        }
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            Form frm = new FRMAddUpdatePerson();
            frm.ShowDialog();
            //_RefreshPeopleList();
            FRMManagePeople_Load(null, null);
        }
        private void cmbFilterPersonItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cmbFilterPersonItem.Text != "None");

            if (txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {

            clsFilterDGV.Filter(txtFilter.Text, cmbFilterPersonItem.Text, DGVPeople, clsPerson.GetAllPeople());
            string FilterColumn = "";

            switch (cmbFilterPersonItem.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name": 
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gendor":
                    FilterColumn = "GendorCaption";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllPeople.DefaultView.RowFilter = "";
                lblRecord.Text = DGVPeople.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "PersonID")
                //in this case we deal with integer not string.

                _dtAllPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
            else
                _dtAllPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());

           lblRecord.Text = DGVPeople.Rows.Count.ToString();

        }
        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)DGVPeople.CurrentRow.Cells[0].Value;
            FRMAddUpdatePerson frm = new FRMAddUpdatePerson(PersonID);
            frm.ShowDialog();
            //_RefreshPeopleList();
            FRMManagePeople_Load(null, null);
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)DGVPeople.CurrentRow.Cells[0].Value;
            FRMPersonCard frm = new FRMPersonCard(PersonID);
            frm.ShowDialog();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FRMAddUpdatePerson frm = new FRMAddUpdatePerson((int)DGVPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            //_RefreshPeopleList();
            FRMManagePeople_Load(null, null);
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you wanna delete Person[" + DGVPeople.CurrentRow.Cells[0].Value + "]",
                "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsPerson.DeletePerson((int)DGVPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Delete Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //_RefreshPeopleList();
                    FRMManagePeople_Load(null, null);
                }
                else
                    MessageBox.Show("Person was not delete because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is not Implemented Yet", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is not Implemented Yet", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private void FRMManagePeople_Load(object sender, EventArgs e)
        {
            _FillFilterComboBox();
            _dtAllPeople = clsPerson.GetAllPeople();
            cmbFilterPersonItem.SelectedIndex = 1;
            DGVPeople.DataSource = _dtAllPeople;
            lblRecord.Text = DGVPeople.Rows.Count.ToString();
            if (DGVPeople.Rows.Count > 0) 
            {
                DGVPeople.Columns[0].HeaderText = "Person ID";
                DGVPeople.Columns[0].Width = 110;

                DGVPeople.Columns[1].HeaderText = "National No.";
                DGVPeople.Columns[1].Width = 120;

                DGVPeople.Columns[2].HeaderText = "First Name";
                DGVPeople.Columns[2].Width = 120;

                DGVPeople.Columns[3].HeaderText = "Second Name";
                DGVPeople.Columns[3].Width = 140;

                DGVPeople.Columns[4].HeaderText = "Third Name";
                DGVPeople.Columns[4].Width = 120;

                DGVPeople.Columns[5].HeaderText = "Last Name";
                DGVPeople.Columns[5].Width = 120;

                DGVPeople.Columns[6].HeaderText = "Gendor";
                DGVPeople.Columns[6].Width = 120;

                DGVPeople.Columns[7].HeaderText = "Date Of Birth";
                DGVPeople.Columns[7].Width = 140;

                DGVPeople.Columns[8].HeaderText = "Nationality";
                DGVPeople.Columns[8].Width = 120;

                DGVPeople.Columns[9].HeaderText = "Phone";
                DGVPeople.Columns[9].Width = 120;

                DGVPeople.Columns[10].HeaderText = "Email";
                DGVPeople.Columns[10].Width = 170;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterPersonItem.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void DGVPeople_DoubleClick(object sender, EventArgs e)
        {
            int PersonID = (int)DGVPeople.CurrentRow.Cells[0].Value;
            Form frm = new FRMPersonCard(PersonID);
            frm.ShowDialog();
        }
    }
}
