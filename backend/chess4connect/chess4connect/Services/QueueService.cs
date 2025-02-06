using chess4connect.Enums;
using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;

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
        public async Task addToQueueAsync(int userId, Game gamemode)
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
                        await addToRoom(gamemode);
                    }
                    break;

                case Game.Connect4://Conecta 4

                    _queueConnect.Enqueue(_network.GetSocketByUserId(userId));//añadir a la cola

                    //Si hay más de un jugador en la cola entrar en una sala
                    if (_queueChess.Count > 1)
                    {
                        await addToRoom(gamemode);
                    }
                    break;

            }


            // Liberamos el semáforo
            _semaphore.Release();
        }




        //Añadir a una sala la pareja de jugadores 

        private async Task addToRoom(Game gamemode)
        {
            //Abrimos el semáforo
            await _semaphore.WaitAsync();

            // Sección crítica

            switch (gamemode)
            {
                case Game.Chess://Ajedrez
                    WebSocketHandler chess1 = _queueChess.Dequeue();//Sacar primer jugador
                    WebSocketHandler chess2 = _queueChess.Dequeue();//Sacar segundo jugador


                    _roomService.addToChessRoom(chess1, chess2);//añadir a sala
                    break;

                case Game.Connect4://Conecta 4
                    WebSocketHandler connect1 = _queueConnect.Dequeue();//Sacar primer jugador
                    WebSocketHandler connect2 = _queueConnect.Dequeue();//Sacar segundo jugador


                    _roomService.addToConnnectRoom(connect1, connect2);//añadir a sala
                    break;

            }
            // Liberamos el semáforo
            _semaphore.Release();

        }

    }
}
