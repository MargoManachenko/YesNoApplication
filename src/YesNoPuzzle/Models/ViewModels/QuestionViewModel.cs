using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YesNoPuzzle.Models.ViewModels
{
    public class QuestionViewModel
    {
        public int GameId { get; set; }

        [Required]
        public string Text { get; set; }

        public ICollection<Game> Question { get; set; }
    }
}
