using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelerikScheduleViewDatabase;
using TelerikScheduleViewDatabase.Entities;

namespace TestApp
{
    public partial class MainWindow : Window
    {
		private Telerik.Windows.Controls.ScheduleView.IExceptionOccurrence[] currentExceptions;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void RadScheduleView_AppointmentEditing(object sender, Telerik.Windows.Controls.AppointmentEditingEventArgs e)
		{
			if (e.Appointment.RecurrenceRule != null)
			{
				this.currentExceptions = e.Appointment.RecurrenceRule.Exceptions.ToArray();
			}
		}

		private void RadScheduleView_AppointmentSaving(object sender, Telerik.Windows.Controls.AppointmentSavingEventArgs e)
		{
			if (this.currentExceptions != null && !e.Appointment.RecurrenceRule.Exceptions.Any())
			{
				foreach (var item in this.currentExceptions)
				{
					e.Appointment.RecurrenceRule.Exceptions.Add(item);
                    ScheduleViewRepository.Context.SqlExceptionOccurrence.Add(item as SqlExceptionOccurrence);
				}
			}
		}

    }
}
