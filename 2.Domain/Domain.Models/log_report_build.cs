using System;
using Domain.Core;

namespace Domain.Models
{
    public class log_report_build : EntityBase<log_report_build, int>
    {
        public long report_event_no { get; set; }
        public DateTime report_build_date { get; set; }
        public int is_finish { get; set; }
        public DateTime? report_build_finish_date { get; set; }
    }
}