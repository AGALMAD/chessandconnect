
namespace chess4connect.Services

{
    using Microsoft.EntityFrameworkCore.Query;
    using System.Timers;
    public class ChessTimer
    {
        private Timer gameTimer;
     
        public ChessTimer(TimeSpan timeLeft)
        {
            gameTimer = new Timer(timeLeft);
            gameTimer.Elapsed += TurnTimerElapsed;             
        }


        private void TurnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("El tiempo ha expirado");
        }

        

    }
}
