using System.Threading;

namespace Server
{
    public class QueueHostedService : BackgroundService
    {
        private QueueService _queue;
        private TictactoeService _tictactoe;

        public QueueHostedService(QueueService queue, TictactoeService tictactoe)
        {
            _queue = queue;
            _tictactoe = tictactoe;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var task = _queue.Dequeue(_tictactoe.Turn);
                    var winner = _tictactoe.GetWinner();
                    if (winner == TicTacToe.None)
                    {
                        _tictactoe.Place(task.Item1, task.Item2);
                    }   
                }
                catch (Exception ex)
                {
                }

                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
