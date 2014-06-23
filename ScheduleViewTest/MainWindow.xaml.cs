using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;
using TelerikScheduleViewDatabase;
using TelerikScheduleViewDatabase.Entities;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity;

namespace ScheduleViewTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Appointments = new ObservableCollection<SqlAppointment>(ScheduleViewRepository.Context.SqlAppointment.ToList() ?? new List<SqlAppointment>());

            Loaded += MainWindow_Loaded;
            Appointments.CollectionChanged += OnAppointmentCollectionChanged;
        }

        public ObservableCollection<SqlAppointment> Appointments { get; private set; }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            myScheduleView.ResourceTypesSource = new ObservableCollection<SqlResourceType>();
            myScheduleView.AppointmentsSource = Appointments;
            myScheduleView.TimeMarkersSource = ScheduleViewRepository.Context.TimeMarker.ToList();
            myScheduleView.CategoriesSource = ScheduleViewRepository.Context.Category.ToList();
        }

        private void OnAppointmentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var app = e.NewItems == null ? null : e.NewItems[0] as SqlAppointment;
                if (app != null)
                {
                    if (ScheduleViewRepository.Context.Entry(app).State != EntityState.Unchanged)
                    {
                        ScheduleViewRepository.Context.SqlAppointment.Add(app);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var app = e.OldItems == null ? null : e.OldItems[0] as SqlAppointment;
                if (app != null && ScheduleViewRepository.Context.SqlAppointment.Any(p => p.SqlAppointmentId == app.SqlAppointmentId))
                {
                    if (app.RecurrenceRule != null)
                    {
                        var tempList = app.RecurrenceRule.Exceptions.ToList();

                        foreach (SqlExceptionOccurrence item in tempList)
                        {
                            ScheduleViewRepository.Context.SqlExceptionOccurrence.Remove(item);
                        }
                    }
                    ScheduleViewRepository.Context.SqlAppointment.Remove(app);
                }
            }
        }

        private void myScheduleView_AppointmentSaving(object sender, AppointmentSavingEventArgs e)
        {
            ScheduleViewRepository.Context.SaveChanges();
        }

        private void myScheduleView_AppointmentDeleted(object sender, AppointmentDeletedEventArgs e)
        {
            ScheduleViewRepository.Context.SaveChanges();
        }
    }
}