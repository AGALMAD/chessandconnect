using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Mappers;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

namespace chess4connect.Models.Games.Chess.Chess
{
    public class ChessRoom : BaseRoom
    {
        private readonly IServiceProvider _serviceProvider;

        public ChessGame Game { get; set; }

        public  ChessRoom (WebSocketHandler player1Handler, WebSocketHandler? player2Handler, ChessGame game, IServiceProvider serviceProvider) : base(player1Handler, player2Handler)
        {
            Game = game;
            _serviceProvider = serviceProvider;
            SubscribeToGameEvents(Game.Board);
        }

        public void SubscribeToGameEvents(ChessBoard chessGame)
        {
            chessGame.OnTimeExpired += async () => await HandleTimeExpired();
        }

        public async Task SendChessRoom()
        {

            await SendRoom(GameType.Chess);
            await SendBoard();
            await SendMovementsMessageAsync();
        }



        private async Task HandleTimeExpired()
        {
            Console.WriteLine($"El juego ha terminado. {(Game.Board.Player1Turn ? "Jugador 1" : "Jugador 2")} ha perdido por tiempo.");

            await SaveGame(_serviceProvider, GameResult.WIN, Game.Board.Player1Turn ? Player2Id : Player1Id);
        }


        public override async Task SendBoard()
        {
            //Lista de piezas original
            List<ChessBasePiece> pieces = Game.Board.convertBoardToList();

            //Lista de piezas sin  los movimientos básicos
            var roomMessage = new SocketMessage<ChessBoardDto>
            {
                Type = SocketCommunicationType.CHESS_BOARD,

                Data = new ChessBoardDto
                {
                    Pieces = ChessPieceMapper.ToDto(pieces),
                    Player1Turn = Game.Board.Player1Turn,
                    Player1Time = (int)Game.Board.Player1Time.TotalSeconds,
                    Player2Time = (int)Game.Board.Player2Time.TotalSeconds,

                }
            };

            string stringBoardMessage = JsonSerializer.Serialize(roomMessage);
            await SendMessage(stringBoardMessage);
        }

        public async Task SendMovementsMessageAsync()
        {
            Game.Board.GetAllPieceMovements();

            //Lista de piezas sin  los movimientos básicos
            var roomMessage = new SocketMessage<List<ChessPieceMovementDto>>
            {
                Type = SocketCommunicationType.CHESS_MOVEMENTS,

                Data = ChessPieceMovementsMappper.ToDto(Game.Board.ChessPiecesMovements)
            };

            string stringBoardMessage = JsonSerializer.Serialize(roomMessage);

            WebSocketHandler playerSocket = Game.Board.Player1Turn ? Player1Handler : Player2Handler;

            //Envia los movimientos al jugador
            if (playerSocket != null)
            {
                await playerSocket.SendAsync(stringBoardMessage);
            }
            else { 
                //Si el bot gana, termina la partida
                if(await Game.Board.RandomMovement())
                {
                    await SaveGame(_serviceProvider,GameResult.WIN, 0);
                }
                else
                {
                    await SendMovementsMessageAsync();
                }

                await SendBoard();
            }

        }

        
        public async Task MoveChessPiece(ChessMoveRequest moveRequest)
        {

            int response = Game.Board.MovePiece(moveRequest);

            if (response == 0)
            {
                await SendBoard();

                await SendMovementsMessageAsync();

            }


            if (response == 1)
            {
                await SendBoard();
                int winnerId = Game.Board.Player1Turn ? Player1Id : Player2Id;
                await SaveGame(_serviceProvider, GameResult.WIN, winnerId);

            }


        }

        public override async Task MessageHandler(string message)
        {
            SocketMessage recived = JsonSerializer.Deserialize<SocketMessage>(message);

            switch(recived.Type)
            {
                case SocketCommunicationType.CHESS_MOVEMENTS:
                    ChessMoveRequest request = JsonSerializer.Deserialize<SocketMessage<ChessMoveRequest>>(message).Data;

                    await MoveChessPiece(request);

                    break;


            }


        }

        public override async Task SaveGame(IServiceProvider serviceProvider, GameResult gameResult, int winnerId)
        {
            using var scope = serviceProvider.CreateAsyncScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            Play play = new Play
            {
                StartDate = Game.StartDate,
                EndDate = DateTime.Now,
                Game = GameType.Chess,
            };

            unitOfWork.PlayRepository.Add(play);
            await unitOfWork.SaveAsync();

            if (winnerId != 0)
            {
                PlayDetail playDetailUser1 = new PlayDetail
                {
                    PlayId = play.Id,
                    UserId = winnerId,
                    GameResult = gameResult
                };
                unitOfWork.PlayDetailRepository.Add(playDetailUser1);

            }

            int looserId = Player1Id == winnerId ? Player2Id : Player1Id;

            if (looserId != 0)
            {
                PlayDetail playDetailLooser = new PlayDetail
                {
                    PlayId = play.Id,
                    UserId = looserId,
                    GameResult = gameResult == GameResult.DRAW ? gameResult : GameResult.LOSE
                };

                unitOfWork.PlayDetailRepository.Add(playDetailLooser);

            }

            await unitOfWork.SaveAsync();



            if (gameResult == GameResult.DRAW)
            {
                await SendDrawMessage();
            }
            else
            {
                await SendWinMessage(winnerId);

            }

        }


        public override async Task SendWinMessage(int winnerId)
        {

            //Mensaje con el id del ganador
            var winnerMessage = new SocketMessage<int>
            {
                Type = SocketCommunicationType.END_GAME,

                Data = winnerId,
            };

            string stringWinnerMessage = JsonSerializer.Serialize(winnerMessage);

            await SendMessage(stringWinnerMessage);

        }

        public override async Task Surrender(int userId, IServiceProvider serviceProvider)
        {
            bool userColor = Player1Id == userId;

            Game.Board.Player1Turn = !userColor;

            int winnerId = Player1Id == userId ? Player2Id : Player1Id;
            await SaveGame(serviceProvider, GameResult.WIN, winnerId);
        }

        public override async Task SendDrawMessage()
        {
            var drawMessage = new SocketMessage
            {
                Type = SocketCommunicationType.DRAW,

            };

            string stringWinnerMessage = JsonSerializer.Serialize(drawMessage);

            await SendMessage(stringWinnerMessage);
        }
    }
}
