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

namespace DVLD_Project.Application_Types
{
    public partial class FRMManageApplicationTypes : Form
    {
        private static DataTable _dtAllApplicationTypes;
        public FRMManageApplicationTypes()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FRMManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _dtAllApplicationTypes = clsApplicationTypes.GetAllApplicationTypes();
            DGVApplicationTypes.DataSource = _dtAllApplicationTypes;
            lblRecordsCount.Text = DGVApplicationTypes.Rows.Count.ToString();

            if (DGVApplicationTypes.Rows.Count > 0)
            {
                DGVApplicationTypes.Columns[0].HeaderText = "Application Type ID";
                DGVApplicationTypes.Columns[0].Width = 110;

                DGVApplicationTypes.Columns[1].HeaderText = "Title";
                DGVApplicationTypes.Columns[1].Width = 400;

                DGVApplicationTypes.Columns[2].HeaderText = "Fees";
                DGVApplicationTypes.Columns[2].Width = 120;
            }
        }
        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationTypeID = (int)DGVApplicationTypes.CurrentRow.Cells[0].Value;
            FRMUpdateApplicationType frm = new FRMUpdateApplicationType(ApplicationTypeID);
            frm.ShowDialog();
            FRMManageApplicationTypes_Load(null, null);
        }
    }
}
