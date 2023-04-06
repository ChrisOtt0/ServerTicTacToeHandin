namespace ServerTicTacToeHandin.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Field AssignedField { get; set; } = Field.Empty;
        public bool IsTurn { get; set; } = false;
    }
}
