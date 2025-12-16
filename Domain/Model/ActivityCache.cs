using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ActivityCache
    {
        public string Description { get; set; }
        public string ActivityCategory { get; set; }
        public string CacheKey { get; set; }
        public string UserId { get; set; }
        public int Range { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
