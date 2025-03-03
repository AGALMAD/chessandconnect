
namespace chess4connect.Services;


using System.Timers;
public class GameTimer
{
    private Timer gameTimer;
    public event Action? OnTimeExpired; // Evento cuando el tiempo se acaba

    public GameTimer()
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
        gameTimer.Stop();
        OnTimeExpired?.Invoke(); // Disparar el evento cuando se acaba el tiempo
    }
}

