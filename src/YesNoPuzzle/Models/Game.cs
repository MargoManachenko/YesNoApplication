using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YesNoPuzzle.Models
{
    public class Game
    {
        public int Id { get; set; }
        
        public string GameName { get; set; }
        
        public virtual ApplicationUser User { get; set; }
        
        public string GameCondition { get; set; }

        public bool GameState { get; set; }

        public virtual List<Question> Questions { get; set; }
    }
}
