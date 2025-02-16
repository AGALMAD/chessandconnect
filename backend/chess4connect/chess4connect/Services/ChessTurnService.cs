
namespace chess4connect.Services

{

    using System.Timers;
    public class ChessTurnService : BackgroundService
    {

        private bool isWhite = true;
        private Timer turnTimer;
        private DateTime turnStartTime;

        private TimeSpan maxTurnDuration = TimeSpan.FromMinutes(1);
        private TimeSpan maxGameDuration = TimeSpan.FromMinutes(3);

        private TimeSpan whiteTimeRemaining;
        private TimeSpan blackTimeRemaining;


        public ChessTurnService()
        {
            turnTimer = new Timer(1000);
            turnTimer.Elapsed += TurnTimerElapsed;

            whiteTimeRemaining = maxGameDuration;
            blackTimeRemaining = maxGameDuration;
        }




        private void TurnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimeSpan elapsedTurnTime = DateTime.Now - turnStartTime;

            if (isWhite) whiteTimeRemaining -= TimeSpan.FromSeconds(1);

            else blackTimeRemaining -= TimeSpan.FromSeconds(1);

            

            if(whiteTimeRemaining <= TimeSpan.Zero)
            {
                EndGame();
            }

            else if(blackTimeRemaining <= TimeSpan.FromSeconds(1))
            {
                EndGame();
            }

            else if(elapsedTurnTime >= maxTurnDuration)
            {
                EndTurn();
            }

        }

        private void StartTurn()
        {
            turnStartTime = DateTime.Now;
            turnTimer.Start();
        }

        private void EndTurn()
        {
            turnTimer.Stop();

            TimeSpan elapsedTurnTime = DateTime.Now - turnStartTime;

            isWhite = !isWhite;

            StartTurn();
        }



        private void EndGame()
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
