/*
    This class represents a shot
*/

namespace Battleship
{
    public class Shot
    {
        // Fields
        private Coordinate _coord;
        private PlayerType _shooter;
        private ShotResult _result;
        private ShipType _shipHit;

        /*
            Constructor
        */

        public Shot()
        {
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

        public PlayerType Shooter
        {
            get { return _shooter; }
            set { _shooter = value; }
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
