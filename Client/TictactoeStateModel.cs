namespace Client
{
    public class TictactoeStateModel
    {
        public TicTacToe Winner { get; set; }
        public IEnumerable<TictactoeMoveModel> Moves { get; set; }
    }
}
