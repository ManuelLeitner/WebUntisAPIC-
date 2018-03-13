using System.Windows;
using System.Windows.Controls;

namespace WebUntis {
    public class TimeTableGrid : Grid {
        public static readonly DependencyProperty ColumnCountProperty =
DependencyProperty.Register("ColumnCount", typeof(string), typeof(TimeTableGrid), new UIPropertyMetadata(null));

        public int ColumnCount {
            set {
                ColumnDefinitions.Clear();
                for (int i = 0; i < value; i++) {
                    ColumnDefinitions.Add(new ColumnDefinition());
                }
            }
            get {
                return ColumnDefinitions.Count;
            }
        }
    }
}
