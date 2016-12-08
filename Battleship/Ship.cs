/*
    This class represents a ship
    12/8/2016
    CSC 253 0001 - M6PROJ
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Battleship
{
    public class Ship
    {
        // Fields
        private int _numParts;
        private Coordinate[] coords;
        private ShipType _type;

        /*
            Constructor
            Accepts the ship's type and the coordinates it occupies
        */

        public Ship(ShipType type, Coordinate[] coordinates)
        {
            switch (type)
            {
                case ShipType.Aircraft_Carrier:
                    _numParts = 5;
                    coords = coordinates;
                    _type = type;
                    break;
                case ShipType.Battleship:
                    _numParts = 4;
                    coords = coordinates;
                    _type = type;
                    break;
                case ShipType.Submarine:
                case ShipType.Destroyer:
                    _numParts = 3;
                    coords = coordinates;
                    _type = type;
                    break;
                case ShipType.Patrol_Boat:
                    _numParts = 2;
                    coords = coordinates;
                    _type = type;
                    break;
            }
        }

        /*
            The GetCoords method returns the ship's coordinates
        */

        public Coordinate[] GetCoords()
        {
            return coords;
        }

        /*
            NumParts Property
        */

        public int NumParts
        {
            get { return _numParts; }
            set { _numParts = value; }
        }

        /*
            Type Property
        */

        public ShipType Type
        {
            get { return _type; }
        }
    }
}
