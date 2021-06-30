using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Project1DbContext
{
    public partial class Customer
    {
        public Customer()
        {
            Invoices = new HashSet<Invoice>();
            PreferredStores = new HashSet<PreferredStore>();
        }

        [Display(Name = "Customer ID")]
        public int Id { get; set; }


        [Required(ErrorMessage="Please enter your first name")]
        [MaxLength(20)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [MaxLength(20)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter a username")]
        [MaxLength(20)]        
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password")]
        [MaxLength(20)]
        public string Password { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<PreferredStore> PreferredStores { get; set; }
    }
}
