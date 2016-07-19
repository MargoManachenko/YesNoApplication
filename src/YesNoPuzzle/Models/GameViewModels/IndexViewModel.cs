using System.Collections.Generic;

namespace YesNoPuzzle.Models.GameViewModels
{
    public class IndexViewModel
    {
        public string GameName { get; set; }

        public string GameCondition { get; set; }

        public bool IsSolved { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
