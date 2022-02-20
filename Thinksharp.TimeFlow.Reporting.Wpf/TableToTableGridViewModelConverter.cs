// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Windows.Data;

namespace Thinksharp.TimeFlow.Reporting.Wpf
{
    public class TableToTableGridViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var table = value as Table;
            if (table == null) return Binding.DoNothing;
            return new TableGridViewModel(table);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
