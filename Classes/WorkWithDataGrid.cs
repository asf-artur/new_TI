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
        private readonly DataGridView _dataGridView;

        public DataClass DataClass;

        public WorkWithDataGrid(DataGridView dataGridView, DataClass dataClass)
        {
            _dataGridView = dataGridView;
            _dataGridView.RowHeadersVisible = false;
            DataClass = dataClass;
            _dataGridView.DataSource = DataClass.DataTable;
            for (int i = 0; i < _dataGridView.ColumnCount; i++)
            {
                _dataGridView.Columns[i].HeaderText = DataClass.ColumnCaptions[i];
            }
            MakeAllResizeFalse(dataGridView);
            _dataGridView.Disposed += DataGridViewDisposed;
        }

        public void DataGridViewDisposed(Object sender, EventArgs e)
        {
            DataClass.Save();
        }

        private void MakeAllResizeFalse(DataGridView dataGridView)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.Resizable = DataGridViewTriState.False;
            }
        }
    }
}
