using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YesNoPuzzle.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string GameName { get; set; }
        
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Display(Name = "Condition")]
        public string GameCondition { get; set; }

        public bool GameState { get; set; }

        public virtual List<Question> Questions { get; set; }
    }
}
