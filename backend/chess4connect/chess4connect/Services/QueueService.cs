using chess4connect.Enums;
using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;
using System.Net.Sockets;
using System.Net.WebSockets;

namespace chess4connect.Services
{
    public class QueueService
    {

        private readonly WebSocketNetwork _network;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly RoomService _roomService;

        private List<WebSocketHandler> _queueChess = new List<WebSocketHandler> { };
        private List<WebSocketHandler> _queueConnect = new List<WebSocketHandler> { };

        public QueueService(WebSocketNetwork webSocketNetwork, RoomService roomService) 
        {
            _network = webSocketNetwork;
            _roomService = roomService;
        }


        //Añadir a la cola el jugador
        public async Task AddToQueueAsync(int userId, Game gamemode)
        {

            //Abrimos el semáforo
            await _semaphore.WaitAsync();

            // Sección crítica


            switch (gamemode)
            {
                case Game.Chess://Ajedrez

                    _queueChess.Add(_network.GetSocketByUserId(userId));//añadir a la cola

                    //Si hay más de un jugador en la cola entrar en una sala
                    if (_queueChess.Count > 1)
                    {
                        await AddToRoom(gamemode);
                    }
                    break;

                case Game.Connect4://Conecta 4

                    _queueConnect.Add(_network.GetSocketByUserId(userId));//añadir a la cola

                    //Si hay más de un jugador en la cola entrar en una sala
                    if (_queueChess.Count > 1)
                    {
                        await AddToRoom(gamemode);
                    }
                    break;
            }


            // Liberamos el semáforo
            _semaphore.Release();
        }




        //Añadir a una sala la pareja de jugadores 


        private async Task AddToRoom(Game gamemode)

        {

            //Abrimos el semáforo
            await _semaphore.WaitAsync();

            // Sección crítica

            switch (gamemode)
            {
                case Game.Chess://Ajedrez

                    WebSocketHandler chess1 = _queueChess[0];//Encuentra primer jugador

                    WebSocketHandler chess2 = _queueChess[1];//Encuentra segundo jugador

                    _queueChess.RemoveRange(0,2);//Sacar dos primeros jugadores

                    await _roomService.AddToRoom(gamemode, chess1, chess2);

                    break;
                   


                case Game.Connect4://Conecta 4
                    WebSocketHandler connect1 = _queueConnect[0];//Encuentra primer jugador

                    WebSocketHandler connect2 = _queueConnect[1];//Encuentra segundo jugador

                    _queueConnect.RemoveRange(0, 2);//Sacar dos primeros jugadores

                    await _roomService.AddToRoom(gamemode, connect1, connect2);

                    break;

            }
            // Liberamos el semáforo
            _semaphore.Release();


        }

        public async Task cancelGame(int userId, Game game)
        {
            WebSocketHandler socket = _network.GetSocketByUserId(userId);

            //Abrimos el semáforo
            await _semaphore.WaitAsync();

            switch (game)
            {
                case Game.Chess:
                    _queueChess.Remove(socket);

                    break;

                case Game.Connect4:
                    _queueConnect.Remove(socket);
                    break;
            }

            // Liberamos el semáforo
            _semaphore.Release();
        }

        public async Task goIntoIAGame(int userId, Game gamemode)
        {
            WebSocketHandler socket = _network.GetSocketByUserId(userId);

            await _roomService.AddToRoom(gamemode, socket);

        }
    }
}
