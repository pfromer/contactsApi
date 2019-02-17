using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pfromerApi.DataModels
{
    public class Country
    {
        [Required]
        public int Id { get; set; }

        public String Name { get; set; }
    }
}
