using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NARAOURCEISG.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Contacts = new List<Contact>();
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [DataType (DataType.EmailAddress)]
        public string? Email { get; set; }
        
        [Required]
        public string? Phone { get; set; }

        public virtual IList<Contact> Contacts { get; set; }
    }
}
