using DVLD_Buisness;
using DVLD_Project.People;
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
    public partial class FRMManageUser : Form
    {
        private static DataTable _dtAllUsers = clsUser.GetAllUsers();
        public FRMManageUser()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            FRMAddUpdateUser frm = new FRMAddUpdateUser();
            frm.ShowDialog();
            FRMManageUser_Load(null, null);

        }
        private void _FillComboBoxFilter()
        {
            cmbFilter.Items.Clear();
            DataTable dt = _dtAllUsers;
            foreach (DataColumn column in dt.Columns)
                cmbFilter.Items.Add(column.ToString());
        }
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "Is Active") 
            {
                txtFilter.Visible = false;
                cmbIsActive.Visible = true;
                cmbIsActive.Focus();
                cmbIsActive.SelectedIndex = 0;
            }
            else
            {
                txtFilter.Visible = (cmbFilter.Text !="None");
                cmbIsActive.Visible = false;
                 
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            clsFilterDGV.Filter(txtFilter.Text, cmbFilter.Text, DGVUser, _dtAllUsers);

            string FilterColumn = "";

            switch (cmbFilter.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;

                case "UserName":
                    FilterColumn = "UserName";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecord.Text = DGVUser.Rows.Count.ToString();
                return;
            }

            if (FilterColumn != "FullName" && FilterColumn != "UserName")
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());

            lblRecord.Text = _dtAllUsers.Rows.Count.ToString();
        }
        private void FRMManageUser_Load(object sender, EventArgs e)
        {
            _FillComboBoxFilter();
            _dtAllUsers = clsUser.GetAllUsers();
            DGVUser.DataSource = _dtAllUsers;
            //cmbFilter.SelectedIndex = 0;
            lblRecord.Text = DGVUser.Rows.Count.ToString();

            if (DGVUser.Rows.Count > 0)
            {
                DGVUser.Columns[0].HeaderText = "User ID";
                DGVUser.Columns[0].Width = 110;

                DGVUser.Columns[1].HeaderText = "Person ID";
                DGVUser.Columns[1].Width = 120;

                DGVUser.Columns[2].HeaderText = "Full Name";
                DGVUser.Columns[2].Width = 250;

                DGVUser.Columns[3].HeaderText = "UserName";
                DGVUser.Columns[3].Width = 120;

                DGVUser.Columns[4].HeaderText = "Is Active";
                DGVUser.Columns[4].Width = 120;
            }
        }
        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilter.Text == "UserID" || cmbFilter.Text == "Person ID") 
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)DGVUser.CurrentRow.Cells[0].Value;
            FRMUserInfo frm = new FRMUserInfo(UserID);
            frm.Show();
        }
        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)DGVUser.CurrentRow.Cells[0].Value;
            FRMAddUpdateUser frm = new FRMAddUpdateUser(UserID);
            frm.ShowDialog();
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)DGVUser.CurrentRow.Cells[0].Value;

            if(clsUser.DeleteUser(UserID))
            {
                MessageBox.Show("User has been deleted successfully", "Deleted",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                FRMManageUser_Load(null, null);
            }

            else
                MessageBox.Show("User is not delted due to data connected to it.", "Faild",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void cmbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cmbIsActive.Text;

            switch(FilterValue)
            {
                case "All":
                    break;

                case "Yes":
                    FilterValue = "1";
                    break;

                case "No":
                    FilterValue = "0";
                    break;
            }

            if (FilterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecord.Text=_dtAllUsers.Rows.Count.ToString();
        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)DGVUser.CurrentRow.Cells[0].Value;
            FRMChangePassword frm = new FRMChangePassword(UserID);
            frm.ShowDialog();
        }
        private void DGVUser_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int UserID =(int)DGVUser.CurrentRow.Cells[0].Value;
            FRMUserInfo frm = new FRMUserInfo(UserID);
            frm.ShowDialog();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)DGVUser.CurrentRow.Cells[0].Value;
            FRMAddUpdateUser frm = new FRMAddUpdateUser(UserID);
            frm.ShowDialog();
            FRMManageUser_Load(null, null);
        }
    }
}