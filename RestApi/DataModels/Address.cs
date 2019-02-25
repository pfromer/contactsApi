using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.DataModels
{
    public class Address
    {
        //things to discuss:
        //is it reasonable to have a separate class for Address?
        //if so: is it reasoneable to store addresses in a separate table?
        //regarding the ZipCode, State and City fields: should we create separate entities for them (as I did for country)? 

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        //if we only cosidered one country then it would be easy to have a separate table for States with "Id and Name"
        //considering contacts from differente countries the State field becomes trickier
        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public Country Country { get; set; }

    }
}
