using chess4connect.Enums;
using chess4connect.Models.Games.Base;

namespace chess4connect.Models.Games.Chess.Chess
{
    public class ChessRoom: BaseRoom
    {

        public ChessGame Game { get; set; }

        public ChessRoom (int player1Id, int? player2Id, ChessGame game): base(player1Id, player2Id)
        {
            Game = game;
        }
    }
}
