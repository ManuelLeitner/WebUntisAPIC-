using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WebUntis {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		public static DateTime StartOfWeek {
			get {
				return DateTime.Now.AddDays(-(int) DateTime.Now.DayOfWeek+1);
			}
		}
		public static DateTime EndOfWeek {
			get {
				return StartOfWeek.AddDays(6);
			}
		}
	}
}
