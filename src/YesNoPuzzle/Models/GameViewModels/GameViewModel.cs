using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YesNoPuzzle.Models.GameViewModels
{
    public class GameViewModel
    {
        public string GameName { get; set; }
        public string GameCondition { get; set; }

        public string GameSolution { get; set; }

        public bool IsSolved { get; set; }

        public ICollection<Question> Questions { get; set; }

        public string Text { get; set; }

        public Game Game { get; set; }

        public int GameId { get; set; }

    }
}


