using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Telerik.Windows.Controls.ScheduleView;

namespace TelerikScheduleViewDatabase.Entities
{
    public class SqlExceptionOccurrence : IExceptionOccurrence
    {
        public int SqlExceptionOccurrenceId { get; set; }
        public int SqlAppointmentId { get; set; }
        public DateTime ExceptionDate { get; set; }

        public IAppointment Appointment
        {
            get { return SqlExceptionAppointment; }
            set
            {
                if (SqlExceptionAppointment != value)
                {
                    if (value == null)
                    {
                        ScheduleViewRepository.Context.SqlExceptionAppointment.Remove(SqlExceptionAppointment);
                    }
                    SqlExceptionAppointment = value as SqlExceptionAppointment;
                    OnPropertyChanged();
                }
            }
        }

        public virtual SqlAppointment SqlAppointment { get; set; }
        public virtual SqlExceptionAppointment SqlExceptionAppointment { get; set; }

        public IExceptionOccurrence Copy()
        {
            var exception = new SqlExceptionOccurrence();
            exception.CopyFrom(this);
            return exception;
        }

        public void CopyFrom(IExceptionOccurrence other)
        {
            if (this.GetType().FullName != other.GetType().FullName)
                throw new ArgumentException("Invalid type");

            this.ExceptionDate = other.ExceptionDate;
            if (other.Appointment != null)
                this.Appointment = other.Appointment.Copy();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}