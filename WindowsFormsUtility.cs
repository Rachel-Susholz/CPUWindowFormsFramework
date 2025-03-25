using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CPUWindowFormsFramework
{
    public static class WindowsFormsUtility
    {
        public static void BindHeaderControls(BindingSource bs, params Control[] controls)
        {
            foreach (Control ctrl in controls)
            {
                SetControlBinding(ctrl, bs);
            }
        }

        public static void BindOutputDateControl(Control ctrl, BindingSource bs, string columnName)
        {
            ctrl.DataBindings.Clear();
            Binding b = new Binding("Text", bs, columnName, true, DataSourceUpdateMode.OnPropertyChanged);
            b.Format += (s, e) =>
            {
                if (e.Value is DateTime dt)
                    e.Value = dt.ToShortDateString();
            };
            ctrl.DataBindings.Add(b);
        }

        public static void AcceptBaselineChanges(params DataTable[] tables)
        {
            foreach (DataTable dt in tables)
                dt.AcceptChanges();
        }

        // Uses naming convention: e.g. "txtRecipeName" binds to "RecipeName"
        public static void SetControlBinding(Control ctrl, BindingSource bs)
        {
            string propertyName = "";
            string columnName = "";
            string controlName = ctrl.Name;
            if (controlName.StartsWith("txt") || controlName.StartsWith("lbl"))
            {
                propertyName = "Text";
                columnName = controlName.Substring(3);
            }
            if (!string.IsNullOrEmpty(propertyName) && !string.IsNullOrEmpty(columnName))
            {
                ctrl.DataBindings.Clear();
                ctrl.DataBindings.Add(propertyName, bs, columnName, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public static void FormatDataGrid(DataGridView grid)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        }

        public static void FormatGridForEdit(DataGridView grid, string tablename)
        {
            grid.EditMode = DataGridViewEditMode.EditOnEnter;
            DoFormatGrid(grid, tablename);
        }

        private static void DoFormatGrid(DataGridView grid, string tablename)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.RowHeadersVisible = false;
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (col.Name.EndsWith("Id"))
                    col.Visible = false;
            }
            string pkname = tablename + "Id";
            if (grid.Columns.Contains(pkname))
                grid.Columns[pkname].Visible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        }

        public static void AddDeleteButtonToGrid(DataGridView grid, string deleteColumnName)
        {
            grid.Columns.Add(new DataGridViewButtonColumn()
            {
                Text = "X",
                HeaderText = "Delete",
                Name = deleteColumnName,
                UseColumnTextForButtonValue = true
            });
        }

        public static void SetListBinding(ComboBox lst, DataTable sourcedt, DataTable targetdt, string tablename)
        {
            lst.DataSource = sourcedt;
            lst.ValueMember = tablename + "Id";
            lst.DisplayMember = lst.Name.Substring(3);
            lst.DataBindings.Add("SelectedValue", targetdt, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public static int GetIdFromGrid(DataGridView grid, int rowIndex, string columnname)
        {
            int id = 0;
            if (rowIndex < grid.Rows.Count && grid.Columns.Contains(columnname) &&
                grid.Rows[rowIndex].Cells[columnname].Value != DBNull.Value)
            {
                if (grid.Rows[rowIndex].Cells[columnname].Value is int)
                    id = (int)grid.Rows[rowIndex].Cells[columnname].Value;
            }
            return id;
        }

        public static void AddComboBoxToGrid(DataGridView grid, DataTable datasource, string displayMember, string tableName)
        {
            string colName = tableName + "Combo";
            if (grid.Columns.Contains(colName))
                grid.Columns.Remove(colName);
            DataGridViewComboBoxColumn c = new DataGridViewComboBoxColumn();
            c.Name = colName;
            c.DataSource = datasource;
            c.DisplayMember = displayMember;
            c.ValueMember = tableName + "Id";
            c.DataPropertyName = c.ValueMember;
            c.HeaderText = tableName;
            c.DefaultCellStyle.NullValue = "Add a recipe here";
            grid.Columns.Insert(0, c);
        }

        public static void SetUpNav(ToolStrip ts)
        {
            ts.Items.Clear();
            foreach (Form f in Application.OpenForms)
            {
                if (!f.IsMdiContainer)
                {
                    ToolStripButton btn = new ToolStripButton();
                    btn.Text = f.Text;
                    btn.Tag = f;
                    btn.Click += (s, e) =>
                    {
                        ((Form)((ToolStripButton)s).Tag).Activate();
                    };
                    ts.Items.Add(btn);
                    ts.Items.Add(new ToolStripSeparator());
                }
            }
        }

        public static bool IsFormOpen(Type formType, int pkvalue = 0)
        {
            bool exists = false;
            foreach (Form frm in Application.OpenForms)
            {
                int frmpkvalue = 0;
                if (frm.Tag != null && frm.Tag is int)
                    frmpkvalue = (int)frm.Tag;
                if (frm.GetType() == formType && frmpkvalue == pkvalue)
                {
                    frm.Activate();
                    exists = true;
                    break;
                }
            }
            return exists;
        }
    }
}
