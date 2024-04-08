namespace Server
{
    public class TictactoeService
    {
        public const int FIELD_SIZE = 100;
        private TicTacToe[,] _field;
        private TicTacToe _turn;

        public TictactoeService()
        {
            _field= new TicTacToe[FIELD_SIZE, FIELD_SIZE];
            _turn = TicTacToe.X;
        }

        public TicTacToe Turn => _turn;

        public void Place(int x, int y)
        {
            if (x < 0 || y < 0 || x >= FIELD_SIZE || y >= FIELD_SIZE) 
            {
                throw new ArgumentException($"X and Y must be in range [0; {FIELD_SIZE})");
            }

            _field[x, y] = _turn;
            if (_turn == TicTacToe.X)
            {
                _turn = TicTacToe.O;
            }
            else
            {
                _turn = TicTacToe.X;
            }
        }

        public TicTacToe GetWinner()
        {
            for (int i = 0; i < FIELD_SIZE; i++) 
            {
                for (int y = 0; y < FIELD_SIZE; y++)
                {
                    if (_field[i, y] != TicTacToe.None)
                    {
                        if (IsWinner(i, y))
                        {
                            return _field[i, y];
                        }
                    }
                }
            }
            return TicTacToe.None;
        }

        public bool IsWinner(int x, int y)
        {
            var type = _field[x, y];
            bool result = false;
            for (int delta_x = -1; delta_x <= 1; delta_x++)
            {
                for (int delta_y = -1; delta_y <= 1; delta_y++) 
                {
                    if (delta_x == 0 && delta_y == 0)
                    {
                        continue;
                    }

                    var current_x = x;
                    var current_y = y;

                    for (int k = 0; k < 4; k++)
                    {
                        current_x += delta_x;
                        current_y += delta_y;

                        if (current_x < 0 || current_y < 0 
                            || current_x >= FIELD_SIZE || current_y >= FIELD_SIZE)
                        {
                            break;
                        }

                        if (_field[current_x, current_y] != type)
                        {
                            break;
                        }

                        if (k == 3)
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        public TictactoeMoveModel[] GetField()
        {
            List<TictactoeMoveModel> list = new List<TictactoeMoveModel>();
            for (int i = 0; i < FIELD_SIZE; i++)
            {
                for (int j = 0; j < FIELD_SIZE; j++)
                {
                    if (_field[i, j] != TicTacToe.None)
                    {
                        list.Add(new TictactoeMoveModel
                        {
                            Type = _field[i, j],
                            x = i,
                            y = j,
                        });
                    }
                }
            }

            return list.ToArray();
        }
    }
}
