using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ACE_PC.Domain.Entity
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }


        [Required]
        public string Name { get; set; } = string.Empty;

        public List<User>? Users { get; set; }
    }
}
