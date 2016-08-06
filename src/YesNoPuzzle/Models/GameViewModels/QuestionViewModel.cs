using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YesNoPuzzle.Models.GameViewModels
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }       

        public string UserName { get; set; }

        public Game Game { get; set; }

        public Question Question { get; set; }

        [Required]
        [Display(Name = "Question")]
        public string Text { get; set; }

        public int State { get; set; }//0 - no answer; 1 - yes; 2 - no; 3 - no matter;

        public DateTime QuestionDate { get; set; }

    }
}