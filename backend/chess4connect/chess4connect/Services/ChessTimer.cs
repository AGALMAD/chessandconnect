
namespace chess4connect.Services

{
    using Microsoft.EntityFrameworkCore.Query;
    using System.Timers;
    public class ChessTimer
    {
        private Timer gameTimer;
        public event Action<bool> OnTimeExpired;

        public void remainingTime(TimeSpan timeLeft, bool isPlayer1turn)
        {
            gameTimer?.Stop();

            gameTimer = new Timer(timeLeft);
            gameTimer.Elapsed += (sender, e) => TurnTimerElapsed(isPlayer1turn);
            gameTimer.AutoReset = false;
            gameTimer.Start();
        }


        private void TurnTimerElapsed(bool isPlayer1turn)
        {
            Console.WriteLine("El tiempo ha expirado");
            OnTimeExpired?.Invoke(isPlayer1turn);
        }

        

    }
}
