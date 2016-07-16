using System.ComponentModel.DataAnnotations;


namespace YesNoPuzzle.Models
{
    public class Question
    {
        public int Id { get; set; }        

        public virtual ApplicationUser User { get; set; }        

        public virtual Game Game { get; set; }
        
        public string Text { get; set; }

        //Щас бы что то типа data State = NoAnswer | Yes | No | NoMatter
        public int State { get; set; }//0 - no answer; 1 - yes; 2 - no; 3 - no matter;
    }
}
