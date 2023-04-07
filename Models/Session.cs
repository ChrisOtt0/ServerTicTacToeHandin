using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ServerTicTacToeHandin.Models
{
    public class Session
    {
        // Public Props
        #region Public Properties
        public int Id { get; set; }
        public Player[] Players { get; set; }
        public List<List<Field>> Board { get; set; } = new List<List<Field>>();
        public Status Status { get; set; } = Status.On;
        public DateTime? Ended { get; set; } = null;
        public Field Winner { get; set; } = Field.Empty;
        #endregion

        // Private fields
        #region Private Fields
        private Random rand = new Random();
        #endregion

        // Constructor
        #region Constructor
        public Session(int id, string ownerName)
        {
            Id = id;
            Players = new Player[2];
            Field ownerAssignment = GenerateRandomField();
            Players[0] = new Player { Id = 0, Name = ownerName, AssignedField = ownerAssignment };
            Players[1] = new Player { Id = 1, AssignedField = GetOpposedField(ownerAssignment) };
            ResetBoard();
        }
        #endregion

        // Public methods
        #region Public Methods
        public Session Reset()
        {
            Players[0].AssignedField = GetOpposedField(Players[0].AssignedField);
            Players[1].AssignedField = GetOpposedField(Players[1].AssignedField);
            ResetBoard();
            Status = Status.On;
            Ended = null;
            return this;
        }

        public void HandleMoveRequest(MoveRequest moveRequest)
        {
            Board[moveRequest.Row][moveRequest.Col] = moveRequest.Move;
            CheckStatus();
            SwitchTurn();
        }

        public void CalcIsTurn()
        {
            foreach (var player in Players)
            {
                if (player.AssignedField == Field.X) player.IsTurn = true;
            }
        }
        #endregion

        // Private methods
        #region Private Methods
        private Field GenerateRandomField()
        {
            int res = rand.Next(2);
            if (res == 0)
                return Field.X;
            else
                return Field.O;
        }

        private Field GetOpposedField(Field originalField)
        {
            if (originalField == Field.X) return Field.O;
            else return Field.X;
        }

        private void ResetBoard()
        {
            Board = new List<List<Field>>();

            for (int row = 0; row < 3; row++)
            {
                Board.Add(new List<Field>());

                for (int col = 0; col < 3; col++)
                {
                    Board[row].Add(Field.Empty);
                }
            }
        }

        private void CheckStatus()
        {
            if (EnoughToWin() && Status == Status.On)
            {
                CheckRows();
                CheckCols();
                CheckDiags();
                CheckFull();
            }
        }

        private bool EnoughToWin()
        {
            int x = 0;
            int o = 0;

            foreach (var i in Board)
            {
                foreach (var f in i)
                {
                    if (f == Field.X) x++;
                    if (f == Field.O) o++;
                }
            }

            if (x < 3 && o < 3) return false;
            return true;
        }

        private bool CheckRows()
        {
            bool fullRow;

            for (int row = 0; row < Board.Count; row++)
            {
                fullRow = true;
                Field checking = Board[row][0];
                if (checking == Field.Empty) continue;

                for (int col = 0; col < Board.Count; col++)
                {
                    if (Board[row][col] != checking)
                    {
                        fullRow = false;
                        continue;
                    }
                }

                if (fullRow)
                {
                    Winner = checking;
                    Status = Status.Over;
                    Ended = DateTime.Now;
                    return true;
                }
            }

            return false;
        }

        private bool CheckCols()
        {
            bool fullCol;

            for (int col = 0; col < Board.Count; col++)
            {
                fullCol = true;
                Field checking = Board[0][col];
                if (checking == Field.Empty) continue;

                for (int row = 0; row < Board.Count; row++)
                {
                    if (Board[row][col] != checking)
                    {
                        fullCol = false;
                        continue;
                    }
                }

                if (fullCol)
                {
                    Winner = checking;
                    Status = Status.Over;
                    Ended = DateTime.Now;
                    return true;
                }
            }

            return false;
        }

        private bool CheckDiags()
        {
            bool fullDiags = true;
            Field checking = Board[0][0];
            if (checking == Field.Empty) goto next;

            for (int i = 0; i < Board.Count; i++)
            {
                if (Board[i][i] != checking)
                {
                    fullDiags = false;
                    continue;
                }
            }

            if (fullDiags)
            {
                Winner = checking;
                Status = Status.Over;
                Ended = DateTime.Now;
                return true;
            }

        next:
            fullDiags = true;
            checking = Board[0][2];
            if (checking == Field.Empty) goto end;

            for (int row = 1; row < Board.Count; row++)
            {
                for (int col = 0; col >= 0; col--)
                {
                    if (row == 1 && col == 0) continue;
                    if (row == 2 && col == 1) continue;

                    if (Board[row][col] != checking)
                    {
                        fullDiags = false;
                        continue;
                    }
                }
            }

            if (fullDiags)
            {
                Winner = checking;
                Status = Status.Over;
                Ended = DateTime.Now;
                return true;
            }

        end:
            return false;
        }

        private bool CheckFull()
        {
            for (int row = 0; row < Board.Count; row++)
            {
                for (int col = 0; col < Board.Count; col++)
                {
                    if (Board[row][col] == Field.Empty) return false;
                }
            }

            Winner = Field.Empty;
            Status = Status.Over;
            Ended = DateTime.Now;
            return true;
        }

        private void SwitchTurn()
        {
            foreach (var player in Players)
            {
                player.IsTurn = !player.IsTurn;
            }
        }
        #endregion
    }
}
