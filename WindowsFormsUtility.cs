using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUWindowFormsFramework
{
    internal class WindowsFormsUtility
    {
    }

    public static void FormatGridForSearchResults(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

}
