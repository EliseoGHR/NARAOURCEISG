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

        [StringLength(30, ErrorMessage = "El nombre no debe tener mas de 30 caracteres")]
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display (Name = "Primer Nombre")]
        public string FirstName { get; set; } = null!;

        [StringLength(30, ErrorMessage = "El Apellido no debe tener mas de 30 caracteres")]
        [Required(ErrorMessage = "El Apellido es requerido")]
        [Display (Name = "Apellido")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Correo  es requerido")]
        [StringLength(50, ErrorMessage = "El Correo  no debe tener mas de 50 caracteres")]
        [EmailAddress(ErrorMessage = "El correo no es valido, por favor ingrese un correo válido.")]
        [Display (Name = "Correo")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El Telefono es obligatorio.")]
        [StringLength(8, ErrorMessage = "El teléfono debe tener exactamente 8 dígitos", MinimumLength = 8)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El campo Telefono solo debe contener números.")]
        [Display (Name = "Telefono")]
        public string Phone { get; set; } = null!;

        [StringLength(30, ErrorMessage = "El Tipo de Contacto no debe tener mas de 30 caracteres")]
        [Required(ErrorMessage = "El Tipo de Contacto es requerido")]
        [Display (Name = "Tipo de Contacto")]
        public string ContactType { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
    }
}
