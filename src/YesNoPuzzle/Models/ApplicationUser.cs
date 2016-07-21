using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace YesNoPuzzle.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string GameName { get; set; }

        [Required]
        public string GameCondition { get; set; }

        public bool GameState { get; set; }
    }
}
