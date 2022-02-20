// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.Linq;

namespace Thinksharp.TimeFlow.Reporting.Wpf
{
    public class TableGridViewModel : ViewModelBase
    {
        private readonly Table myTable;

        public TableGridViewModel(Table table)
        {
            myTable = table;
        }

        public IEnumerable<TableRowViewModel> Rows
        {
            get { return myTable.Rows.Select(r => new TableRowViewModel(myTable, r)); }
        }

        public IEnumerable<Column> Columns
        {
            get { return myTable.Columns; }
        }
    }
}
