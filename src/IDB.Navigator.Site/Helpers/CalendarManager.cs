using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Exchange.WebServices.Data;
using System.Configuration;
using System.Net;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Helpers.Outlook
{
    public class CalendarManager
    {

        public static List<Marker> GetAppointments(List<Marker> marker)
        {
            try
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                //  service.Url = new Uri(ConfigurationManager.AppSettings["Exchange_service"]);
                service.Credentials = new NetworkCredential(
                              "rcerrato",
                             "",
                               "IDB");
                service.AutodiscoverUrl("rcerrato@iadb.org");
                service.Timeout = 10000;
                List<AttendeeInfo> attendees = new List<AttendeeInfo>();

                foreach (Marker office in marker)
                {
                    string user = office.OfficeNumber.Substring(0, 2) + "-" + office.OfficeNumber.Substring(2);
                    attendees.Add(new AttendeeInfo()
                    {
                        SmtpAddress = user.ToUpper() + "@Contractual.iadb.org",
                        AttendeeType = MeetingAttendeeType.Optional
                    });
                }

                // Specify options to request free/busy information and suggested meeting times.
                AvailabilityOptions availabilityOptions = new AvailabilityOptions();
                // availabilityOptions.MergedFreeBusyInterval =;
                availabilityOptions.RequestedFreeBusyView = FreeBusyViewType.Detailed;

                GetUserAvailabilityResults results = service.GetUserAvailability(attendees,
                                                                        new TimeWindow(DateTime.Now, DateTime.Now.AddHours(18)),
                                                                        AvailabilityData.FreeBusy,
                                                                      availabilityOptions);
                int i = 0;
                foreach (Marker office in marker)
                {
                    if (office.Type.Code == "EXECUTIVE_CONF")
                    {
                        office.Status = "BUSY";
                        office.Detail = "Busy";
                    }
                    else
                    {
                        office.Status = "FREE";
                        office.Detail = "Free";
                        foreach (CalendarEvent _nextEvent in results.AttendeesAvailability[i].CalendarEvents)
                        {
                            if (_nextEvent.StartTime.Date == DateTime.Today && _nextEvent.StartTime < DateTime.Now && _nextEvent.EndTime > DateTime.Now)
                            {
                                office.Status = "BUSY";
                                office.Detail = "Busy(" + _nextEvent.Details.Subject.Trim() + "). Free at " + _nextEvent.EndTime.TimeOfDay.ToString();
                            }
                        }
                        i++;
                    }
                }

            }
            catch (Exception ex)
            {
                foreach (Marker office in marker)
                {
                    if (office.Type.Code == "EXECUTIVE_CONF")
                    {
                        office.Status = "BUSY";
                        office.Detail = "Busy";
                    }
                    else
                    {
                        office.Status = "UNKNOWN";
                        office.Detail = "Unknown";
                    }
                }

            }

           
            
            return marker;
        }
    }
}