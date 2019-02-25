using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.DataModels
{
    public class Company
    {
        /// At this point I only consider Id and Name for a given company, but
        /// more fields will be added
        [Required]
        public int Id { get; set; }
        public String Name { get; set; }
    }
}
