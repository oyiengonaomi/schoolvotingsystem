using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Votingsystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        //properties of the application user
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }


    }
}
