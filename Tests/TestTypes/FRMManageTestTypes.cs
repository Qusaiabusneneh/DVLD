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

namespace DVLD_Project.TestTypes
{
    public partial class FRMManageTestTypes : Form
    {
        private static DataTable _dtAllTestTypes;
        public FRMManageTestTypes()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FRMManageTestTypes_Load(object sender, EventArgs e)
        {
            _dtAllTestTypes = clsTestTypes.GetAllTestTypes();
            DGVTestType.DataSource = _dtAllTestTypes;
            lblRecordCount.Text = DGVTestType.Rows.Count.ToString();

            if (DGVTestType.Rows.Count > 0)
            {
                DGVTestType.Columns[0].HeaderText = "TestType ID";
                DGVTestType.Columns[0].Width = 110;

                DGVTestType.Columns[1].HeaderText = "Title";
                DGVTestType.Columns[1].Width = 120;

                DGVTestType.Columns[2].HeaderText = "Description";
                DGVTestType.Columns[2].Width = 400;

                DGVTestType.Columns[3].HeaderText = "Fees";
                DGVTestType.Columns[3].Width = 110;
            }
        }
        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestTypeID = (int)DGVTestType.CurrentRow.Cells[0].Value;
            FRMUpdateTestType frm = new FRMUpdateTestType((clsTestTypes.enTestType)DGVTestType.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            FRMManageTestTypes_Load(null, null);
        }
    }
}
