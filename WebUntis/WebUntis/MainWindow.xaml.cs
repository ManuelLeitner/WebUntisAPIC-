using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WebUntis.Connector;
using WebUntis.Connector.Requests;

namespace WebUntis {
    public partial class MainWindow : Window {

        public User User { get; } = new User();
        public WebUntisConnector Connector { get; set; }

        public TimeTable ClassTimeTable { get; set; }

        public MainWindow() {
            InitializeComponent();
            Authenticate(null, null);
        }

        private void Authenticate(object sender, RoutedEventArgs e) {
            Connector = new WebUntisConnector(User);
            try {
                Connector.Authenticate();
                Credentials.Content = Connector.Credentials;
                Load();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public void Load() {
            try {
                ClassChooser.ItemsSource = Connector.Classes.Values;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            try {
                Subjects.ItemsSource = Connector.Subjects.Values;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            try {
                Rooms.ItemsSource = Connector.Rooms.Values;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            try {
                Holidays.ItemsSource = Connector.Holidays.Values;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            try {
                Connector.RefreshTeachers();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void PasswordChanged(object sender, RoutedEventArgs e) {
            User.SecurePassword = ((PasswordBox)sender).SecurePassword;
        }
        private void ClassChanged(object sender, SelectionChangedEventArgs e) {
            try {
                TimeTable timeTable = Connector.GetClassTimeTable(((ComboBox)sender).SelectedItem as Class, StartDate.SelectedDate, EndDate.SelectedDate);
                ClassTimeTable = timeTable;

                ClassGrid.ColumnCount = timeTable.Days;

                int columnCount = timeTable.Days;
                int rowCount = timeTable.LongestDayMinutes / 5 + 1;

                /*	ColumnDefinitionCollection columns = ClassGrid.ColumnDefinitions;
                    for(int i = 0; i < columnCount; i++) {
                        columns.Add(new ColumnDefinition());
                    }  */

                RowDefinitionCollection rows = ClassGrid.RowDefinitions;
                for (int i = 0; i < rowCount; i++) {
                    rows.Add(new RowDefinition());
                }
                Console.WriteLine(rowCount);


                foreach (TimeTableField field in timeTable.Fields) {
                    Label lbl = new Label();
                    if (field.Subjects.Length > 0)
                        lbl.Content = field.Subjects[0].Id;
                    lbl.Background = Brushes.Red;
                    int column = (field.StartTime - timeTable.MinDate).Days;
                    int row = (TimeTable.MinutesOfDay(field.StartTime) - timeTable.EarliestLessonMinute) / 5;
                    int rowSpan = (TimeTable.MinutesOfDay(field.EndTime) - TimeTable.MinutesOfDay(field.StartTime)) / 5;
                    Grid.SetColumn(lbl, column);
                    Grid.SetRow(lbl, row);
                    Grid.SetRowSpan(lbl, rowSpan);
                    ClassGrid.Children.Add(lbl);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClosingWindow(object sender, System.ComponentModel.CancelEventArgs e) {
            Connector?.Logout();
        }
    }

    public class ColorToBrush : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return new SolidColorBrush((Color?)value ?? Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return Binding.DoNothing;
        }
    }

    [ValueConversion(typeof(DateTime), typeof(String))]
    public class DateConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            DateTime date = (DateTime)value;

            return date.ToString(culture.DateTimeFormat);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DateTime.Parse((string)value);
        }

    }
}
