using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Activity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ActivityCategory { get; set; }
        [NotMapped]
        public string CacheKey { get; set; }
        //Creator
        public ApplicationUser User { get; set; }
    }
}
