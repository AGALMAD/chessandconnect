namespace chess4connect.Models.Games.Base;

public abstract class BaseRoom
{
    public int Player1Id { get; set; }
    public int Player2Id { get; set; }

    public BaseRoom(int player1Id, int player2Id) { 
        Player1Id = player1Id;
        Player2Id = player2Id;
    }
}
