using chess4connect.Enums;
using chess4connect.Models.SocketComunication.Handlers;
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
        public async Task AddToQueueAsync(int userId, GameType gamemode)
        {

            //Abrimos el semáforo
            await _semaphore.WaitAsync();

            // Sección crítica

            switch (gamemode)
            {
                case GameType.Chess://Ajedrez

                    //Si ya está en la cola no lo añade
                    WebSocketHandler chessUserSocket = _queueChess.FirstOrDefault(q => q.Id == userId);
                    if (chessUserSocket != null)
                        return;

                    _queueChess.Add(_network.GetSocketByUserId(userId));//añadir a la cola

                    //Si hay más de un jugador en la cola entrar en una sala
                    if (_queueChess.Count > 1)
                    {
                        await AddToRoom(gamemode);
                    }



                    break;

                case GameType.Connect4://Conecta 4

                    //Si ya está en la cola no lo añade
                    WebSocketHandler connectUserSocket = _queueConnect.FirstOrDefault(q => q.Id == userId);
                    if (connectUserSocket != null)
                        return;

                    _queueConnect.Add(_network.GetSocketByUserId(userId));//añadir a la cola

                    //Si hay más de un jugador en la cola entrar en una sala
                    if (_queueConnect.Count > 1)
                    {
                        await AddToRoom(gamemode);
                    }
                    break;
            }

            // Liberamos el semáforo
            _semaphore.Release();
        }




        //Añadir a una sala la pareja de jugadores 
        private async Task AddToRoom(GameType gamemode)

        {

            switch (gamemode)
            {
                case GameType.Chess://Ajedrez

                    WebSocketHandler chess1 = _queueChess[0];//Encuentra primer jugador

                    WebSocketHandler chess2 = _queueChess[1];//Encuentra segundo jugador

                    _queueChess.RemoveRange(0, 2);//Sacar dos primeros jugadores

                    await _roomService.CreateRoomAsync(gamemode, chess1, chess2);

                    break;



                case GameType.Connect4://Conecta 4
                    WebSocketHandler connect1 = _queueConnect[0];//Encuentra primer jugador

                    WebSocketHandler connect2 = _queueConnect[1];//Encuentra segundo jugador

                    _queueConnect.RemoveRange(0, 2);//Sacar dos primeros jugadores

                    await _roomService.CreateRoomAsync(gamemode, connect1, connect2);

                    break;

            }


        }

        public async Task cancelGame(int userId, GameType game)
        {
            WebSocketHandler socket = _network.GetSocketByUserId(userId);

            //Abrimos el semáforo
            await _semaphore.WaitAsync();

            switch (game)
            {
                case GameType.Chess:
                    _queueChess.Remove(socket);

                    break;

                case GameType.Connect4:
                    _queueConnect.Remove(socket);
                    break;
            }

            // Liberamos el semáforo
            _semaphore.Release();
        }

        public async Task goIntoIAGame(int userId, GameType gamemode)
        {
            WebSocketHandler socket = _network.GetSocketByUserId(userId);

            await _roomService.CreateRoomAsync(gamemode, socket);

        }

        public async Task goIntoFriendGame(int anfitrion, int friend, GameType gamemode)
        {
            WebSocketHandler player1 = _network.GetSocketByUserId(anfitrion);
            WebSocketHandler player2 = _network.GetSocketByUserId(friend);

            await _roomService.CreateRoomAsync(gamemode, player1, player2);

        }
    }
}
