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
        private readonly IServiceProvider serviceProvider;

        public ChessGame Game { get; set; }

        public  ChessRoom (WebSocketHandler player1Handler, WebSocketHandler? player2Handler, ChessGame game): base(player1Handler, player2Handler)
        {
            Game = game;
        }


        public async Task SendChessRoom()
        {

            await SendRoom(GameType.Chess);
            await SendBoard();
            await SendMovementsMessageAsync();
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
                await Game.Board.RandomMovement();
                await SendBoard();
                await SendMovementsMessageAsync();
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
                await SaveGame(serviceProvider, GameResult.WIN);

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

        public override async Task SaveGame(IServiceProvider serviceProvider, GameResult gameResult)
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

            PlayDetail playDetailUser1 = new PlayDetail
            {
                PlayId = play.Id,
                UserId = Game.Board.Player1Turn ? Player1Id : Player2Id,
                GameResult = gameResult
            };

            PlayDetail playDetailUser2 = new PlayDetail
            {
                PlayId = play.Id,
                UserId = Game.Board.Player1Turn ? Player2Id : Player1Id,
                GameResult = gameResult == GameResult.DRAW ? gameResult : GameResult.LOSE
            };

            unitOfWork.PlayDetailRepository.Add(playDetailUser1);
            unitOfWork.PlayDetailRepository.Add(playDetailUser2);
            await unitOfWork.SaveAsync();


            if (gameResult == GameResult.DRAW)
            {
                await SendDrawMessage();
            }
            else
            {
                await SendWinMessage();

            }

        }


        public override async Task SendWinMessage()
        {
            int winnerId = Game.Board.Player1Turn ? Player1Id : Player2Id;


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

            await SaveGame(serviceProvider, GameResult.WIN);
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
