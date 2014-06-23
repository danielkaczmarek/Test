using System;
using System.Data.Entity;
using System.Linq;
using TelerikScheduleViewDatabase;

namespace CreateDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ContextMaster>());

            using(ContextMaster context = new ContextMaster ())
            {
                var results = context.SqlAppointment.FirstOrDefault();
            }

            Console.WriteLine("Finished...");
            Console.ReadLine();
        }
    }
}
