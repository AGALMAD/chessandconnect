using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Mappers;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

namespace chess4connect.Models.Games.Connect;

public class ConnectRoom : BaseRoom
{
    private readonly IServiceProvider _serviceProvider;


    public ConnectGame Game { get; set; }

    public ConnectRoom( WebSocketHandler player1Handler, WebSocketHandler player2Handler, ConnectGame game, IServiceProvider serviceProvider) : base(player1Handler, player2Handler)
    {
        Player1Handler = player1Handler;
        Player2Handler = player2Handler;

        Game = game;
        _serviceProvider = serviceProvider;
    }


    public async Task DropConnectPiece(int column)
    {
        int response = Game.Board.DropPiece(column);

        if(response != -1)
            await SendBoard();


        if (response == 1)
        {
            int winnerId = Game.Board.Player1Turn ? Player1Id : Player2Id;
            await SaveGame(_serviceProvider, GameResult.WIN, winnerId);
            await SendWinMessage(winnerId);
            return;
        }

        if (Player2Handler == null)
        {
            await Game.Board.RandomDrop();
            await SendBoard();

        }


    }


    public override async Task SendBoard()
    {

        //Lista de piezas sin  los movimientos básicos
        var roomMessage = new SocketMessage<ConnectBoardDto>
        {
            Type = SocketCommunicationType.CONNECT_BOARD,

            Data = new ConnectBoardDto
            {
                LastPiece = Game.Board.LastPiece,
                Player1Turn = Game.Board.Player1Turn,
                Player1Time = (int)Game.Board.Player1Time.TotalSeconds,
                Player2Time = (int)Game.Board.Player2Time.TotalSeconds,

            }
        };

        string stringBoardMessage = JsonSerializer.Serialize(roomMessage);
        await SendMessage(stringBoardMessage);
    }

    public async Task SendConnectRoom()
    {
        await SendRoom(GameType.Connect4);
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

    public override async Task SendDrawMessage()
    {

        var drawMessage = new SocketMessage
        {
            Type = SocketCommunicationType.DRAW,

        };

        string stringWinnerMessage = JsonSerializer.Serialize(drawMessage);

        await SendMessage(stringWinnerMessage);
    }


    public override async Task MessageHandler(string message)
    {
        SocketMessage recived = JsonSerializer.Deserialize<SocketMessage>(message);

        switch (recived.Type)
        {
            case SocketCommunicationType.CONNECT4_MOVEMENTS:
                ConnectDropPieceRequest request = JsonSerializer.Deserialize<SocketMessage<ConnectDropPieceRequest>>(message).Data;

                await DropConnectPiece(request.Column);
                

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
            Game = GameType.Connect4,
        };

        unitOfWork.PlayRepository.Add(play);

        await unitOfWork.SaveAsync();

        if(winnerId != 0)
        {
            PlayDetail playDetailUser1 = new PlayDetail
            {
                PlayId = play.Id,
                UserId = Game.Board.Player1Turn ? Player1Id : Player2Id,
                GameResult = gameResult
            };
            unitOfWork.PlayDetailRepository.Add(playDetailUser1);

        }


        if (Player2Id != 0)
        {
            PlayDetail playDetailUser2 = new PlayDetail
            {
                PlayId = play.Id,
                UserId = Game.Board.Player1Turn ? Player2Id : Player1Id,
                GameResult = gameResult == GameResult.DRAW ? gameResult : GameResult.LOSE
            };
            unitOfWork.PlayDetailRepository.Add(playDetailUser2);

        }

        await unitOfWork.SaveAsync();


        if(gameResult == GameResult.DRAW)
        {
            await SendDrawMessage();
        }
        else
        {
            await SendWinMessage(winnerId);

        }

    }

    public override async Task Surrender(int userId, IServiceProvider serviceProvider)
    {
        bool userColor = Player1Id == userId;
        Game.Board.Player1Turn = !userColor;

        int winnerId = Player1Id == userId ? Player2Id : Player1Id;

        await SaveGame(serviceProvider, GameResult.WIN, winnerId);
    }

}
