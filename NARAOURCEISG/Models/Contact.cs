using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NARAOURCEISG.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        [Display(Name = "Cliente")]
        public int CustomerId { get; set; }

        [Display (Name = "Primer Nombre")]
        public string FirstName { get; set; } = null!;

        [Display (Name = "Apellido")]
        public string LastName { get; set; } = null!;

        [Display (Name = "Correo")]
        public string? Email { get; set; }

        [Display (Name = "Telefono")]
        public string Phone { get; set; } = null!;

        [Display (Name = "Tipo de Contacto")]
        public string ContactType { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
    }
}
