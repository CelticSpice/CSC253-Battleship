/*
    This class represents a shot
    12/8/2016
    CSC 253 0001 - M6PROJ
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Battleship
{
    public class Shot
    {
        // Fields
        private Coordinate _coord;
        private Player _shooter;
        private ShotResult _result;
        private ShipType _shipHit;

        /*
            Constructor - Accepts the shooter
        */

        public Shot(Player player)
        {
            _shooter = player;
        }

        /*
            Coord Property
        */

        public Coordinate Coord
        {
            get { return _coord; }
            set { _coord = value; }
        }

        /*
            Shooter Property
        */

        public Player Shooter
        {
            get { return _shooter; }
        }

        /*
            Result Property
        */

        public ShotResult Result
        {
            get { return _result; }
            set { _result = value; }
        }

        /*
            ShipHit Property
        */

        public ShipType ShipHit
        {
            get { return _shipHit; }
            set { _shipHit = value; }
        }
    }
}
