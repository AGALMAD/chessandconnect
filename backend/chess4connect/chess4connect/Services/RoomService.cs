using chess4connect.DTOs;
using chess4connect.Enums;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Connect;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;
using chess4connect.DTOs.Games;
using chess4connect.Models.Games.Chess;
using chess4connect.Models.Games;

namespace chess4connect.Services
{
    public class RoomService
    {
        private readonly IServiceProvider _serviceProvider;


        private List<ChessRoom> chessRooms = new List<ChessRoom>();
        private List<ConnectRoom> connectRooms = new List<ConnectRoom>();

        public RoomService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public async Task CreateRoomAsync(GameType gamemode, WebSocketHandler player1Hadler, WebSocketHandler player2Handler = null)
        {

            if (gamemode == GameType.Chess)
            {
                var room = new ChessRoom(player1Hadler, player2Handler,
                    new ChessGame(DateTime.Now,
                    new ChessBoard()
                    {
                        StartTurnDateTime = DateTime.Now,
                    }));

                room.Player1Id = player1Hadler.Id;

                if (player2Handler != null)
                    room.Player2Id = player2Handler.Id;

                chessRooms.Add(room);

                await room.SendChessRoom();

            }
            else if (gamemode == GameType.Connect4)
            {
                var room = new ConnectRoom(player1Hadler, player2Handler,
                   new ConnectGame(DateTime.Now,
                   new ConnectBoard()
                   {
                       StartTurnDateTime = DateTime.Now
                   }));


                room.Player1Id = player1Hadler.Id;

                if (player2Handler != null)
                    room.Player2Id = player2Handler.Id;

                connectRooms.Add(room);

                await room.SendConnectRoom();

            }

        }

        public async Task MessageHandler(int userId, string message)
        {
            SocketMessage recived = JsonSerializer.Deserialize<SocketMessage>(message);


            switch (recived.Type)
            {
                case SocketCommunicationType.CHAT:
                    ChessRoom chessRoom = GetChessRoomByUserId(userId);

                    if(chessRoom != null)
                    {
                        await chessRoom.SendChatMessage(message, userId);
                    }
                    else 
                    {
                        await GetConnectRoomByUserId(userId).SendChatMessage(message, userId);
                    }
                    break;

                case SocketCommunicationType.CHESS_MOVEMENTS:
                    await GetChessRoomByUserId(userId).MessageHandler(message);
                    break;

                case SocketCommunicationType.CONNECT4_MOVEMENTS:
                    await GetConnectRoomByUserId(userId).MessageHandler(message);
                    break;

                //Si el compañero se desconecta, elimina la sala y envía mensaje de victoria al usuario
                case SocketCommunicationType.CONNECTION:
                    ConnectionSocketMessage<ConnectionModel> connectioMessage = JsonSerializer.Deserialize<ConnectionSocketMessage<ConnectionModel>>(message);

                    if (connectioMessage.Data.Type == ConnectionType.Disconnected)
                    {
                        ChessRoom chessRoom1 = GetChessRoomByUserId(userId);

                        if (chessRoom1 != null)
                        {
                            await chessRoom1.SaveGame(_serviceProvider, GameResult.WIN);
                            chessRooms.Remove(chessRoom1);
                        }

                        else
                        {
                            ConnectRoom connectRoom = GetConnectRoomByUserId(userId);

                            if (connectRoom != null)
                            {
                                await connectRoom.SaveGame(_serviceProvider, GameResult.WIN);
                                connectRooms.Remove(connectRoom);
                            }

                        }
                    }

                    break;
                case SocketCommunicationType.DRAW_REQUEST:

                    ChessRoom room = GetChessRoomByUserId(userId);

                   

                    if (room != null)
                    {
                        if (room.NewDrawRequest(userId))
                        {
                            await room.SaveGame(_serviceProvider,GameResult.DRAW);
                            
                        }
                        else
                        {
                            WebSocketHandler opponentSocket = userId == room.Player1Id ? room.Player2Handler : room.Player1Handler;
                            if(opponentSocket != null)
                            {
                                var drawRequestMessage = new SocketMessage
                                {
                                    Type = SocketCommunicationType.DRAW_REQUEST,
                                };


                                await opponentSocket.SendAsync(JsonSerializer.Serialize(drawRequestMessage));
                            }
                            
                        }
                    }

                    else
                    {
                        ConnectRoom connectRoom = GetConnectRoomByUserId(userId);

                        if (connectRoom != null)
                        {
                            if (connectRoom.NewDrawRequest(userId))
                            {
                                await connectRoom.SaveGame(_serviceProvider, GameResult.DRAW);
                            }
                            else
                            {
                                WebSocketHandler opponentSocket = userId == connectRoom.Player1Id ? connectRoom.Player2Handler : connectRoom.Player1Handler;
                                if (opponentSocket != null)
                                {
                                    var drawRequestMessage = new SocketMessage
                                    {
                                        Type = SocketCommunicationType.DRAW_REQUEST,
                                    };


                                    await opponentSocket.SendAsync(JsonSerializer.Serialize(drawRequestMessage));
                                }
                            }
                        }

                    }


                    break;
                case SocketCommunicationType.REMATCH_REQUEST:

                    ChessRoom rematchRoom = GetChessRoomByUserId(userId);

                    if (rematchRoom != null)
                    {
                        if (rematchRoom.NewRematchRequest(userId))
                        {
                            rematchRoom.Game = new ChessGame(DateTime.Now,
                                                new ChessBoard()
                                                {
                                                    StartTurnDateTime = DateTime.Now,
                                                });


                            await rematchRoom.SendChessRoom();

                        }
                        else
                        {
                            WebSocketHandler opponentSocket = userId == rematchRoom.Player1Id ? rematchRoom.Player2Handler : rematchRoom.Player1Handler;
                            if (opponentSocket != null)
                            {
                                var rematchRequestMessage = new SocketMessage
                                {
                                    Type = SocketCommunicationType.REMATCH_REQUEST,
                                };


                                await opponentSocket.SendAsync(JsonSerializer.Serialize(rematchRequestMessage));
                            }
                        }
                    }

                    else
                    {
                        ConnectRoom connectRoom = GetConnectRoomByUserId(userId);

                        if (connectRoom != null)
                        {
                            if (connectRoom.NewDrawRequest(userId))
                            {
                                connectRoom.Game = new ConnectGame(DateTime.Now,
                                   new ConnectBoard()
                                   {
                                       StartTurnDateTime = DateTime.Now
                                   });

                                await connectRoom.SendConnectRoom();

                            }
                            else
                            {
                                WebSocketHandler opponentSocket = userId == connectRoom.Player1Id ? connectRoom.Player2Handler : connectRoom.Player1Handler;
                                if (opponentSocket != null)
                                {
                                    var rematchRequestMessage = new SocketMessage
                                    {
                                        Type = SocketCommunicationType.REMATCH_REQUEST,
                                    };


                                    await opponentSocket.SendAsync(JsonSerializer.Serialize(rematchRequestMessage));
                                }
                            }



                        }

                    }


                    break;

                case SocketCommunicationType.SURRENDER:

                    ChessRoom leaveRoom = GetChessRoomByUserId(userId);

                    if (leaveRoom != null)
                    {
                        await leaveRoom.Surrender(userId, _serviceProvider);
                    }

                    else
                    {
                        ConnectRoom connectRoom = GetConnectRoomByUserId(userId);
                        await connectRoom.Surrender(userId, _serviceProvider);

                    }


                    break;

                case SocketCommunicationType.REMATCH_DECLINED:

                    ChessRoom rematchDeclinedRoom = GetChessRoomByUserId(userId);

                    var rematchDeclinedMessage = new SocketMessage
                    {
                        Type = SocketCommunicationType.REMATCH_DECLINED,
                    };

                    if (rematchDeclinedRoom != null)
                    {
                        WebSocketHandler opponentSocket = userId == rematchDeclinedRoom.Player1Id ? rematchDeclinedRoom.Player2Handler : rematchDeclinedRoom.Player1Handler;

                        if (opponentSocket != null)
                            await opponentSocket.SendAsync(JsonSerializer.Serialize(rematchDeclinedMessage));

                        chessRooms.Remove(rematchDeclinedRoom);
                    }

                    else
                    {
                        ConnectRoom connectRoom = GetConnectRoomByUserId(userId);
                        WebSocketHandler opponentSocket = userId == connectRoom.Player1Id ? connectRoom.Player2Handler : connectRoom.Player1Handler;
                        
                        if(opponentSocket != null)
                            await opponentSocket.SendAsync(JsonSerializer.Serialize(rematchDeclinedMessage));

                        connectRooms.Remove(connectRoom);

                    }


                    break;
            }


        }



        public ChessRoom GetChessRoomByUserId(int userId)
        {
            return chessRooms.FirstOrDefault(r => r.Player1Handler.Id == userId || r.Player2Handler.Id == userId);
        }
        public ConnectRoom GetConnectRoomByUserId(int userId)
        {
            return connectRooms.FirstOrDefault(r => r.Player1Handler.Id == userId || r.Player2Handler.Id == userId);
        }





    }
}
