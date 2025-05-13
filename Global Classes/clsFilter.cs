using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Global_Classes
{
    public class clsFilter
    {
        public static void Filter(string Text,string ColumnToSearchIn,DataGridView DGV,DataTable Defualt_dt)
        {
            if (string.IsNullOrEmpty(ColumnToSearchIn))
                return;

            DataTable dt = Defualt_dt;
            if(string.IsNullOrEmpty(Text))
            {
                DGV.DataSource = dt;
                return;
            }
            DataView dataView = new DataView(dt);

            dataView.RowFilter = $"CONVERT({ColumnToSearchIn}.'System.String')Like'{Text}%'";
            DGV.DataSource = dataView;
        }
    }
}
