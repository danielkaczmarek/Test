namespace TelerikScheduleViewDatabase.Entities
{
    public class SqlAppointmentResource
    {
        public int SqlAppointmentResourceId { get; set; }
        public int SqlAppointmentId { get; set; }
        public int SqlResourceId { get; set; }

        public virtual SqlAppointment SqlAppointment { get; set; }
        public virtual SqlResource SqlResource { get; set; }
    }
}