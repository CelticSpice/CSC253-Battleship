/*
    This class represents a game board
*/

using System.Collections.Generic;
using System.Linq;

namespace Battleship
{
    public class Board
    {
        // Consts
        private const int _NUM_ROWS = 10;
        private const int _NUM_COLUMNS = 10;
        private const int _NUM_SHIPS = 5;

        // Fields
        private Tile[,] _tiles;
        private Ship[] _ships;

        /*
            No-Arg constructor
        */

        public Board()
        {
            _tiles = new Tile[_NUM_ROWS, _NUM_COLUMNS];
            for (int row = 0; row < _NUM_ROWS; row++)
                for (int col = 0; col < _NUM_COLUMNS; col++)
                    _tiles[row, col] = new Tile(new Coordinate { x = col, y = row });
            _ships = new Ship[_NUM_SHIPS];
        }

        /*
            The GetGuessableCoords method returns an array containing coordinates that
            can be guessed
        */

        public Coordinate[] GetGuessableCoords()
        {
            List<Coordinate> coords = new List<Coordinate>();
            foreach (Tile tile in _tiles)
                if (!tile.IsFiredAt)
                    coords.Add(tile.Coordinate);
            return coords.ToArray();
        }

        /*
            The GetOccupiedCoords method returns an array of the coordinates
            that are currently occupied by ships
        */

        private Coordinate[] GetOccupiedCoords()
        {
            List<Tile> occupiedTiles = new List<Tile>();
            foreach (Tile tile in _tiles)
                if (tile.IsOccupied)
                    occupiedTiles.Add(tile);

            Coordinate[] coords = new Coordinate[occupiedTiles.Count];
            for (int i = 0; i < coords.Length; i++)
                coords[i] = occupiedTiles[i].Coordinate;

            return coords;
        }

        /*
            The GetNumShipsLiving method returns the number of ships
            on the board that have 1 or more parts that have not been
            hit
        */

        public int GetNumShipsLiving()
        {
            int numLiving = 0;
            foreach (Ship ship in _ships)
                if (ship.NumParts > 0)
                    numLiving++;
            return numLiving;
        }

        /*
            The GetUnoccupiedCoords method returns an array of the coordinates of tiles
            that are not currently occupied by any ship
        */

        public Coordinate[] GetUnoccupiedCoords()
        {
            List<Tile> unoccupiedTiles = new List<Tile>();
            foreach (Tile tile in _tiles)
                if (!tile.IsOccupied)
                    unoccupiedTiles.Add(tile);

            Coordinate[] coords = new Coordinate[unoccupiedTiles.Count];
            for (int i = 0; i < coords.Length; i++)
                coords[i] = unoccupiedTiles[i].Coordinate;

            return coords;
        }

        /*
            The IsGuessOK method returns whether the specified 
            coordinate/tile has not already been guessed
        */

        public bool IsGuessOK(Coordinate coord)
        {
            bool ok;
            if (!_tiles[coord.y, coord.x].IsFiredAt)
                ok = true;
            else
                ok = false;
            return ok;
        }

        /*
            The IsHit method returns whether the specified
            coordinate is occupied by a ship
        */

        public bool IsHit(Coordinate coord)
        {
            bool isHit = false;

            for (ShipType type = ShipType.AircraftCarrier; type <= ShipType.PatrolBoat && !isHit; type++)
            {
                // Get coordinates of ship
                Coordinate[] shipCoords = _ships[(int)type].GetCoords();

                // Check if specified coordinate matches a ship coordinate
                foreach (Coordinate shipCoord in shipCoords)
                    if (coord.Equals(shipCoord))
                        isHit = true;
            }

            return isHit;
        }

        /*
            The IsShipExisting method returns whether the specified ship exists on the board
        */

        public bool IsShipExisting(ShipType type)
        {
            return (_ships[(int)type] != null) ? true : false;
        }

        /*
            The IsShipPlacementOK method checks if a ship with the given coordinates
            can be placed on the board without error
        */

        public bool IsShipPlacementOK(Coordinate[] coords)
        {
            bool ok = true;

            // Check for coords out of range
            foreach (Coordinate coordinate in coords)
            {
                if ((coordinate.x < 0 || coordinate.x > 9) ||
                    (coordinate.y < 0 || coordinate.y > 9))
                {
                    ok = false;
                }
            }

            // Check for placement on occupied coordinates
            if (ok)
            {
                foreach (Coordinate coord in GetOccupiedCoords())
                    if (coords.Contains(coord))
                        ok = false;
            }

            return ok;
        }

        /*
            The PlaceShip method places a ship on the board
            It accepts the type of ship being placed and its coordinates as arguments
            The method returns a boolean indicating whether the operation was successful
        */

        public bool PlaceShip(ShipType type, Coordinate[] coords)
        {
            bool success;

            if (IsShipPlacementOK(coords))
            {
                // Fetch the tiles to assign to the Ship
                Tile[] tiles = new Tile[coords.Length];
                for (int i = 0; i < tiles.Length; i++)
                {
                    tiles[i] = _tiles[coords[i].y, coords[i].x];
                    tiles[i].IsOccupied = true;
                }
                _ships[(int)type] = new Ship(type, tiles);
                success = true;
            }
            else
                success = false;

            return success;
        }

        /*
            Rows Property
        */

        public int Rows
        {
            get { return _NUM_ROWS; }
        }

        /*
            Columns Property
        */

        public int Columns
        {
            get { return _NUM_COLUMNS; }
        }

        /*
            Tiles property
        */

        public Tile[,] Tiles
        {
            get { return _tiles; }
        }

        /*
            Ships property
        */

        public Ship[] Ships
        {
            get { return _ships; }
        }
    }
}
