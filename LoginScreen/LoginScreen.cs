using DVLD_Project.Global_Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BuisnessLayer;

namespace DVLD_Project
{
    public partial class LoginScreen : Form
    {

        public LoginScreen()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User = clsUser.FindByUsernameAndPassword(txtUsername.Text.Trim(), clsHashing.ComputeHash(txtPassword.Text.Trim()));

            if (User != null)
            {
                if (chkRemeber.Checked)
                {
                    //store username and password
                    clsGlobal.RememberUsernameAndPassword(txtUsername.Text.Trim(), txtPassword.Text.Trim());
                }
                else
                {
                    //store empty username and password
                    clsGlobal.RememberUsernameAndPassword("", "");
                }

                if (!User.IsActive)
                {
                    txtUsername.Focus();
                    MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = User;
                this.Hide();
                FRMDVLD frm = new FRMDVLD(this);
                frm.ShowDialog();
            }
            else
            {
                txtUsername.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoginScreen_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";

            if(clsGlobal.GetSortedCredential(ref Username,ref Password))
            {
                txtPassword.Text = Password;
                txtUsername.Text= Username;
                chkRemeber.Checked = true;
            }
            else
                chkRemeber.Checked= false;
        }
    }
}
