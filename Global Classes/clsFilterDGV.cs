using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public class clsFilterDGV
    {
            public static void Filter(string Text, string ColumnToSearch, DataGridView DGV,DataTable DefualtDataTable)
            {
                if (string.IsNullOrEmpty(ColumnToSearch))
                    return;

                DataTable dt= DefualtDataTable;
                if(string.IsNullOrEmpty(Text))
                {
                    DGV.DataSource= dt;
                    return;
                }
                DataView dataView = new DataView(dt);
                dataView.RowFilter= $"CONVERT({ColumnToSearch},'System.String') Like '{Text}%'";
                DGV.DataSource = dataView;
            }
    }
}
