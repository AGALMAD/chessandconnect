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

        private Queue<WebSocketHandler> _queueChess = new Queue<WebSocketHandler> { };
        private Queue<WebSocketHandler> _queueConnect = new Queue<WebSocketHandler> { };

        public QueueService(WebSocketNetwork webSocketNetwork, RoomService roomService) 
        {
            _network = webSocketNetwork;
            _roomService = roomService;
        }


        //Añadir a la cola el jugador
        public async Task<Room> AddToQueueAsync(int userId, Game gamemode)
        {

            //Abrimos el semáforo
            await _semaphore.WaitAsync();

            // Sección crítica


            switch (gamemode)
            {
                case Game.Chess://Ajedrez

                    _queueChess.Enqueue(_network.GetSocketByUserId(userId));//añadir a la cola

                    //Si hay más de un jugador en la cola entrar en una sala
                    if (_queueChess.Count > 1)
                    {
                        return await AddToRoom(gamemode);
                    }
                    break;

                case Game.Connect4://Conecta 4

                    _queueConnect.Enqueue(_network.GetSocketByUserId(userId));//añadir a la cola

                    //Si hay más de un jugador en la cola entrar en una sala
                    if (_queueChess.Count > 1)
                    {
                        return await AddToRoom(gamemode);
                    }
                    break;
            }


            // Liberamos el semáforo
            _semaphore.Release();
            return null;
        }




        //Añadir a una sala la pareja de jugadores 


        private async Task addToRoom(Game gamemode, int player1, int player2)

        {
            WebSocketHandler p1 = _network.GetSocketByUserId(player1);
            WebSocketHandler p2 = _network.GetSocketByUserId(player2);

            //Abrimos el semáforo
            await _semaphore.WaitAsync();

            // Sección crítica

            switch (gamemode)
            {
                case Game.Chess://Ajedrez
                    if (p1 == null)
                    {
                        _queueChess = new Queue<WebSocketHandler>(_queueChess.Where(s => s != p1));
                    }
                    if (p2 == null)
                    {
                        _queueChess = new Queue<WebSocketHandler>(_queueChess.Where(s => s != p2));
                    }
                    if (p1 != null && p2 != null)
                    {
                        _roomService.addToChessRoom(p1, p2);//añadir a sala
                    }
                    break ;
                        
                    

                    //WebSocketHandler chess1 = _queueChess.Dequeue();//Sacar primer jugador
                    //WebSocketHandler chess2 = _queueChess.Dequeue();//Sacar segundo jugador
                   


                case Game.Connect4://Conecta 4
                    if (p1 == null)
                    {
                        _queueConnect = new Queue<WebSocketHandler>(_queueConnect.Where(s => s != p1));
                    }
                    if (p2 == null)
                    {
                        _queueConnect = new Queue<WebSocketHandler>(_queueConnect.Where(s => s != p2));
                    }
                    if (p1 != null && p2 != null)
                    {
                        _roomService.addToConnnectRoom(p1, p2);//añadir a sala
                    }

                    break;

            }
            // Liberamos el semáforo
            _semaphore.Release();

        }

        private async Task cancelGame(int userId, Game game)
        {
            WebSocketHandler socket = _network.GetSocketByUserId(userId);

            //Abrimos el semáforo
            await _semaphore.WaitAsync();

            switch (game)
            {
                case Game.Chess:
                    _queueChess = new Queue<WebSocketHandler>(_queueChess.Where(s => s != socket));

                    break;

                case Game.Connect4:
                    _queueChess = new Queue<WebSocketHandler>(_queueConnect.Where(s => s != socket));
                    break;
            }

            // Liberamos el semáforo
            _semaphore.Release();


        }
    }
}
