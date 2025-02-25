using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Security.Claims;
using System.Text.Json;

namespace chess4connect.Models.Games.Base;

public abstract class BaseRoom
{
    //Sockets de los jugadores
    public WebSocketHandler Player1Handler { get; set; }
    public WebSocketHandler? Player2Handler { get; set; }

    //Ids de los jugadores
    public int Player1Id { get; set; } = 0;
    public int Player2Id { get; set; } = 0;

    public int DrawRequests { get; set; } = 0;
    public int RemathcRequests { get; set; } = 0;


    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


    public BaseRoom(WebSocketHandler player1Handler, WebSocketHandler? player2Handler) {
        Player1Handler = player1Handler;
        Player2Handler = player2Handler;
    }
    public abstract Task SendBoard();
    public abstract Task SaveGame(IServiceProvider serviceProvider, GameResult gameResult);
    public abstract Task SendWinMessage();
    public abstract Task MessageHandler( string message);
    public abstract Task Surrender(int userId, IServiceProvider serviceProvider);



    public async Task SendRoom(GameType gameType)
    {
        int player2Id;
        if (Player2Handler == null)
            player2Id = 0;
        else
            player2Id = Player2Handler.Id;

        var roomMessage = new SocketMessage<RoomDto>
        {
            Type = SocketCommunicationType.GAME_START,

            Data = new RoomDto
            {
                GameType = gameType,
                Player1Id = Player1Handler.Id,
                Player2Id = player2Id,
            }
        };

        string stringRoomMessage = JsonSerializer.Serialize(roomMessage);

        await SendMessage(stringRoomMessage);

    }


    public async Task SendMessage(string message)
    {
        //Envia los movimientos al jugador
        await Player1Handler.SendAsync(message);


        if (Player2Handler != null)
        {
            await Player2Handler.SendAsync(message);
        }
    }

    public async Task SendChatMessage(string message, int userId)
    {

        // Si el que envia el mensaje es el Player1 se envia el mensaje al Player2 y viceversa

        if (Player1Handler.Id == userId)
        {
            if (Player2Handler != null)
            {
                await Player2Handler.SendAsync(message);
            }
        }
        else
        {
            await Player1Handler.SendAsync(message);
        }
    }


    public async Task<bool> NewDrawRequest()
    {
        await _semaphore.WaitAsync();

        DrawRequests++;

        _semaphore.Release();

        return DrawRequests == 2;

    }

    public async Task<bool> NewRematchRequest()
    {
        await _semaphore.WaitAsync();

        RemathcRequests++;

        _semaphore.Release();

        return RemathcRequests == 2;

    }

}

