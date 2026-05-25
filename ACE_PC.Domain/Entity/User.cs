using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACE_PC.Domain.Entity
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; } 

        public Role? Role { get; set; }
    }
}
