
namespace chess4connect.Services

{
    using Microsoft.EntityFrameworkCore.Query;
    using System.Timers;
    public class ChessTimer
    {
        private Timer gameTimer;
        public event Action? OnTimeExpired; // Evento sin parámetros

        public ChessTimer()
        {
            gameTimer = new Timer();
            gameTimer.Elapsed += TurnTimerElapsed;
            gameTimer.AutoReset = false;
        }

        public void StartTimer(TimeSpan timeLeft)
        {
            gameTimer.Stop();
            gameTimer.Interval = timeLeft.TotalMilliseconds; // Convertir TimeSpan a milisegundos
            gameTimer.Start();
        }

        private void TurnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            Console.WriteLine("El tiempo ha expirado.");
             // Llamar al evento cuando el tiempo se acabe
             gameTimer.Stop();
        }
    }
}

