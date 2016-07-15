using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YesNoPuzzle.Models
{
    public class Question
    {
        public int Id { get; set; }

        public int ApplicationUserId { get; set; }

        [Required]
        public string Text { get; set; }

        public int State { get; set; }//0 - no answer; 1 - yes; 2 - no; 3 - no matter;
    }
}
