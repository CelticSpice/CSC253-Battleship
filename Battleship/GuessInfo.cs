/*
    This structure represents information about a guess made
*/

namespace Battleship
{
   public struct GuessInfo
    {
        public Coordinate coord;
        public GuessResult result;
        public PlayerType player;
        public ShipType type;
    }
}
