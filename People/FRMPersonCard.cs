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
    public partial class FRMPersonCard : Form
    {
        public FRMPersonCard(int PersonID)
        {
            InitializeComponent();
            ctrlPersonCard2.LoadPersonInfo(PersonID);

        }
        public FRMPersonCard(string NationalNo)
        {
            InitializeComponent();
            ctrlPersonCard2.LoadPersonInfo(NationalNo);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
