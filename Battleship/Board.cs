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

        // Fields
        private Tile[,] _tiles;
        private Ship[] _ships;

        /*
            No-Arg constructor
        */

        public Board()
        {
            const int ROWS = 10;
            const int COLUMNS = 10;
            const int SHIPS = 5;
            _tiles = new Tile[ROWS, COLUMNS];
            for (int row = 0; row < ROWS; row++)
                for (int col = 0; col < COLUMNS; col++)
                    _tiles[row, col] = new Tile(new Coordinate { x = col, y = row });
            _ships = new Ship[SHIPS];
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
            The IsShipExisting method returns whether the specified Ship exists on the Board
        */

        public bool IsShipExisting(ShipType type)
        {
            return (_ships[(int)type] != null) ? true : false;
        }

        /*
            The IsShipPlacementOK method checks if a Ship with the given coordinates
            can be placed on the Board without error
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
            The PlaceShip method places a Ship on the Board
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
