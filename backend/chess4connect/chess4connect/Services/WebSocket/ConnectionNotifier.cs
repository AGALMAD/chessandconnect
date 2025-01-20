using chess4connect.Models.Database.Entities;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace chess4connect.Services.WebSocket;

public class ConnectionNotifier
{

    private UnitOfWork _unitOfWork;

    public ConnectionNotifier(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;   
    }


    public Task NotifyAllFriends(int userId)
    {


        //Notifica a todos los amigos conectados



    }





}
