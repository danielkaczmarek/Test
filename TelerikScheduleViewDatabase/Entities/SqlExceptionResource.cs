namespace TelerikScheduleViewDatabase.Entities
{
    public class SqlExceptionResource
    {
        public int SqlExceptionResourceId { get; set; }
        public int SqlExceptionAppointmentId { get; set; }
        public int SqlResourceId { get; set; }

        public virtual SqlExceptionAppointment SqlExceptionAppointment { get; set; }
        public virtual SqlResource SqlResource { get; set; }
    }
}