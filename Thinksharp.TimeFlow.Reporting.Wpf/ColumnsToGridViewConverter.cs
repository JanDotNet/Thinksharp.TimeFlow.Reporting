// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Thinksharp.TimeFlow.Reporting.Wpf
{
    public class ColumnsToGridViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var columns = value as IEnumerable<Column>;
            if (columns == null) return Binding.DoNothing;

            var grdiView = new GridView();
            foreach (var column in columns.OrderBy(c => c.SortIndex))
            {
                var binding = new Binding(column.ID + ".Value");
                grdiView.Columns.Add(new GridViewColumn
                {
                    Header = column.Header,
                    CellTemplate = CreateTemplate(binding),
                    //DisplayMemberBinding = binding,
                });
            }
            return grdiView;
        }

        private DataTemplate CreateTemplate(Binding binding)
        {
            DataTemplate cellTemplate = new DataTemplate(); // create a datatemplate
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty, binding);
            factory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Left);
            cellTemplate.VisualTree = factory;

            return cellTemplate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
