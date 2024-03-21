using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NARAOURCEISG.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string ContactType { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
    }
}
