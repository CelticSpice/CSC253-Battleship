/*
    This class represents a ship
*/

namespace Battleship
{
    public class Ship
    {
        // Fields
        private int _numParts;
        private Tile[] _tilesOccupied;
        private ShipType _type;

        /*
            Constructor
            Accepts the Ship's type and the tiles it occupies
        */

        public Ship(ShipType type, Tile[] tiles)
        {
            switch (type)
            {
                case ShipType.AircraftCarrier:
                    _numParts = 5;
                    _tilesOccupied = new Tile[_numParts];
                    _type = type;
                    break;
                case ShipType.Battleship:
                    _numParts = 4;
                    _tilesOccupied = new Tile[_numParts];
                    _type = type;
                    break;
                case ShipType.Submarine:
                case ShipType.Destroyer:
                    _numParts = 3;
                    _tilesOccupied = new Tile[_numParts];
                    _type = type;
                    break;
                case ShipType.PatrolBoat:
                    _numParts = 2;
                    _tilesOccupied = new Tile[_numParts];
                    _type = type;
                    break;
            }

            for (int i = 0; i < tiles.Length; i++)
                _tilesOccupied[i] = new Tile(tiles[i]);
        }

        /*
            The GetCoords method returns the coordinates of the ship
        */

        public Coordinate[] GetCoords()
        {
            Coordinate[] coords = new Coordinate[_tilesOccupied.Length];
            for (int i = 0; i < coords.Length; i++)
                coords[i] = _tilesOccupied[i].Coordinate;
            return coords;
        }

        /*
            NumParts property
        */

        public int NumParts
        {
            get { return _numParts; }
        }

        /*
            TilesOccupied property
        */

        public Tile[] TilesOccupied
        {
            get { return _tilesOccupied; }
        }

        /*
            Type property
        */

        public ShipType Type
        {
            get { return _type; }
        }
    }
}