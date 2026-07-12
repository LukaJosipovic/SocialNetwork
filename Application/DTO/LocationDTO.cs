using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class LocationDTO : UserBriefDetailsDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool GhostMode { get; set; }
        public bool DoNotDisturb { get; set; }
    }
}
