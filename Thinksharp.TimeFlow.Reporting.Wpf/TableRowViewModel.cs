// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Thinksharp.TimeFlow.Reporting.Iterator;

namespace Thinksharp.TimeFlow.Reporting.Wpf
{
  public class RowViewModel : DynamicObject, INotifyPropertyChanged
  {
    private readonly Dictionary<string, object> keyValues;

    public RowViewModel(Row row, Dictionary<string, object> keyValues)
    {
      Row = row;
      this.keyValues = keyValues;
    }

    #region INotifyPropertyChanged support

    protected virtual void OnPropertyChanged(string propertyName)
    {
      var handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected Boolean SetValue<T>(ref T valueToAssign, T newValue, [CallerMemberName] string propertyName = null)
    {
      if (valueToAssign == null && newValue == null)
        return false;

      if (valueToAssign != null && valueToAssign.Equals(newValue))
        return false;

      valueToAssign = newValue;

      OnPropertyChanged(propertyName);

      return true;
    }

    #endregion

    public override IEnumerable<string> GetDynamicMemberNames()
    {
      return keyValues.Keys;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      return keyValues.TryGetValue(binder.Name, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
      throw new NotSupportedException("Settings row values is not supported.");
    }

    private Color foreground;

    public Color Foreground
    {
      get { return foreground; }
      set { SetValue(ref foreground, value); }
    }

    private Color background = Colors.Beige;

    public Color Background
    {
      get { return background; }
      set { SetValue(ref background, value); }
    }

    public Row Row { get; }
  }
}
