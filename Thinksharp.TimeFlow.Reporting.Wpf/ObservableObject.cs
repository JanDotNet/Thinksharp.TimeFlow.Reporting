using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Thinksharp.TimeFlow.Reporting.Wpf
{
  public class ObservableObject : INotifyPropertyChanged
  {
    protected virtual void OnPropertyChanged(string propertyName)
    {
      var handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

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

    public event PropertyChangedEventHandler PropertyChanged;
  }

  public static class NotifyPropertyChangedExtension
  {

  }
}
