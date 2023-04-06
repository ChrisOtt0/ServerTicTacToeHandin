namespace ServerTicTacToeHandin.Models
{
    public class MoveRequest
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Field Move { get; set; }
    }
}
