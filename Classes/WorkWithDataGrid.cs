using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuzzyLogic.Classes
{
    public class WorkWithDataGrid
    {
        private DataGridView DataGridView;

        public DataClass DataClass;

        public WorkWithDataGrid(DataGridView dataGridView, DataClass dataClass)
        {
            DataGridView = dataGridView;
            DataGridView.RowHeadersVisible = false;
            DataClass = dataClass;
            DataGridView.DataSource = DataClass.DataTable;
            for (int i = 0; i < DataGridView.ColumnCount; i++)
            {
                DataGridView.Columns[i].HeaderText = DataClass.ColumnCaptions[i];
            }
            DataGridView.Disposed += DataGridViewDisposed;
        }

        public void DataGridViewDisposed(Object sender, EventArgs e)
        {
            DataClass.Save();
        }
    }
}
