using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YesNoPuzzle.Models.ViewModels
{
    public class GameViewModel
    {
        public string GameName { get; set; }
        public string GameCondition { get; set; }

        public List<Question> Questions { get; set; }
    }
}
