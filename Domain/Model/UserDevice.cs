using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class UserDevice
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DeviceToken { get; set; }
        public bool IsActive { get; set; }
        public ApplicationUser User { get; set; }
    }
}
