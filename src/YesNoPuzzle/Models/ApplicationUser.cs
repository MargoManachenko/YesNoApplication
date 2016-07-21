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
        public ICollection<Game> Games { get; set; }
    }
}
