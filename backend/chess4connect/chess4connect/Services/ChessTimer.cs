
namespace chess4connect.Services

{
    using Microsoft.EntityFrameworkCore.Query;
    using System.Timers;
    public class ChessTimer
    {
        private Timer gameTimer;
     
        public void remainingTime(TimeSpan timeLeft)
        {
            gameTimer?.Stop();

            gameTimer = new Timer(timeLeft.TotalMicroseconds);
            gameTimer.Elapsed += TurnTimerElapsed;
            gameTimer.AutoReset = false;
            gameTimer.Start();
        }


        private void TurnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("El tiempo ha expirado");
        }

        

    }
}
