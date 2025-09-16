using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Report
    {
        public int Id { get; set; }
        public string ReporterId { get; set; }
        public ApplicationUser? Reporter { get; set; }
        public string? ReportedUserId { get; set; }
        public ApplicationUser? ReportedUser { get; set; }
        public Post? ReportedPost { get; set; }
    }
}
