﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NARAOURCEISG.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Contacts = new List<Contact>();
        }

        public int Id { get; set; }

        [StringLength(30, ErrorMessage = "El nombre no debe tener mas de 30 caracteres")]
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Primer Nombre")]
        public string FirstName { get; set; } = null!;

        [StringLength(30, ErrorMessage = "El Apellido no debe tener mas de 30 caracteres")]
        [Required(ErrorMessage = "El Apellido es requerido")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Correo  es requerido")]
        [StringLength(50, ErrorMessage = "El Correo  no debe tener mas de 50 caracteres")]
        [EmailAddress(ErrorMessage = "El correo no es valido, por favor ingrese un correo válido.")]
        [Display(Name = "Correo")]
        [DataType (DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El Telefono es obligatorio.")]
        [StringLength(8, ErrorMessage = "El teléfono debe tener exactamente 8 dígitos", MinimumLength = 8)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El campo Telefono solo debe contener números.")]
        [Display(Name = "Telefono")]
        public string? Phone { get; set; }

         [NotMapped]
        public int Take { get; set; }

        public virtual IList<Contact> Contacts { get; set; }
    }
}

