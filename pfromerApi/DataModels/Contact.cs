﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pfromerApi.DataModels
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public Company Company { get; set; }
        
        [Required]
        [EmailAddress]
        public String Email { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        public String WorkPhoneNumber { get; set; }

        [Required]
        public String PersonalPhoneNumber { get; set; }

        [Required]
        public Address Address { get; set; }

        public bool Equals(Contact anotherContact)
        {
            return anotherContact.Name == this.Name &&
                anotherContact.Company.Id == this.Company.Id &&
                anotherContact.Email == this.Email &&
                anotherContact.Birthdate == this.Birthdate &&
                anotherContact.WorkPhoneNumber == this.WorkPhoneNumber &&
                anotherContact.PersonalPhoneNumber == this.PersonalPhoneNumber;

            //TODO: also compare addresses are equal/ implement Equals method in Address class
        }

    }
}
