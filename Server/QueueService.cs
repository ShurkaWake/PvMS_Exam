namespace Server
{
    public class QueueService
    {
        private Queue<(int, int)> _xs; 
        private Queue<(int, int)> _os;

        public QueueService()
        {
            _xs= new Queue<(int, int)> ();
            _os= new Queue<(int, int)> ();
        }

        public void Enqueue(int x, int y, TicTacToe type)
        {
            if (_xs.Count + _os.Count > 50)
            {
                throw new Exception("Queue out of memory. Please try again later");
            }

            if (type == TicTacToe.X)
            {
                _xs.Enqueue((x, y));
            }
            else
            {
                _os.Enqueue((x, y));
            }
        }

        public (int, int) Dequeue(TicTacToe type)
        {

            return type == TicTacToe.X ? _xs.Dequeue() : _os.Dequeue();
        }
    }
}
