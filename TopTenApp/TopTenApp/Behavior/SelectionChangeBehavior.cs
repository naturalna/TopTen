using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TopTenApp.Behavior
{
    public class SelectionChangeBehavior
    {
        public static void ExecuteSelectionChangedCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as GridView;
            if (control == null)
            {
                return;
            }
            if ((e.NewValue != null) && (e.OldValue == null))
            {
                control.SelectionChanged += (snd, args) =>
                {
                    var command = (snd as GridView).GetValue(SelectionChangeBehavior.SelectionChangedProperty) as ICommand;
                    if (command != null)
                    {
                        command.Execute(args);
                    }
                };
            }
        }

        public static ICommand GetSelectionChanged(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SelectionChangedProperty);
        }

        public static void SetSelectionChanged(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SelectionChangedProperty, value);
        }

        public static readonly DependencyProperty SelectionChangedProperty =
            DependencyProperty.RegisterAttached("SelectionChanged",
                typeof(ICommand),
                typeof(SelectionChangeBehavior),
                new PropertyMetadata(null,ExecuteSelectionChangedCommand));
    }
}
