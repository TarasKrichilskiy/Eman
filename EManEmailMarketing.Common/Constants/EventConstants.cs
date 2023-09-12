using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EManEmailMarketing.Common.Telemetry
{
    public static class EventConstants
    {
        // Azure Functions
        public static EventId PROCESS_EMAIL_MANUAL_START = new(1, "PROCCESS_EMAIL_MANUAL_START");
        public static EventId PROCESS_EMAIL_MANUAL_END = new(1, "PROCCESS_EMAIL_MANUAL_END");
        public static EventId PROCESS_EMAIL_MANUAL_ERROR = new(1, "PROCCESS_EMAIL_MANUAL_ERROR");
        public static EventId PROCESS_EMAIL_MANUAL = new(1, "PROCCESS_EMAIL_MANUAL");

        public static EventId PROCESS_WEBSITE_SCRAPE_START = new(2, "PROCESS_WEBSITE_SCRAPE_START");
        public static EventId PROCESS_WEBSITE_SCRAPE_END = new(2, "PROCESS_WEBSITE_SCRAPE_END");
        public static EventId PROCESS_WEBSITE_SCRAPE_ERROR = new(2, "PROCESS_WEBSITE_SCRAPE_ERROR");

        public static EventId PROCESS_GOOGLE_SCRAPE_START = new(3, "PROCESS_GOOGLE_SCRAPE_START");
        public static EventId PROCESS_GOOGLE_SCRAPE_END = new(3, "PROCESS_GOOGLE_SCRAPE_END");
        public static EventId PROCESS_GOOGLE_SCRAPE_ERROR = new(3, "PROCESS_GOOGLE_SCRAPE_ERROR");

        public static EventId PROCESS_EMAIL_DAILY_START = new(4, "PROCESS_EMAIL_DAILY_START");
        public static EventId PROCESS_EMAIL_DAILY_END = new(4, "PROCESS_EMAIL_DAILY_END");
        public static EventId PROCESS_EMAIL_DAILY_ERROR = new(4, "PROCESS_EMAIL_DAILY_ERROR");

        public static EventId PROCESS_EMAIL_SEND_REQUEST_START = new(5, "PROCESS_EMAIL_SEND_REQUEST_START");
        public static EventId PROCESS_EMAIL_SEND_REQUEST_END = new(5, "PROCESS_EMAIL_SEND_REQUEST_END");
        public static EventId PROCESS_EMAIL_SEND_REQUEST_ERROR = new(5, "PROCESS_EMAIL_SEND_REQUEST_ERROR");
    }
}
