/*
    This class represents a game board
*/

using System;
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
            const int MAX = 100;
            Random rand = new Random((int)DateTime.Now.Ticks);
            _tiles = new Tile[_NUM_ROWS, _NUM_COLUMNS];
            for (int row = 0; row < _NUM_ROWS; row++)
                for (int col = 0; col < _NUM_COLUMNS; col++)
                {
                    Tile tile = new Tile(new Coordinate { x = col, y = row });
                    tile.Weight = rand.Next(MAX);
                    _tiles[row, col] = tile;
                }
            _ships = new Ship[_NUM_SHIPS];

            SetNeighbors();
        }

        /*
            The AlterNeighborWeights method alters the weights of tiles
            around a given tile at a specified coordinate, taking into
            consideration neighboring tiles that may have been hit
        */

        public void AlterNeighborWeights(Coordinate coord)
        {
            // If there are neighbors hit, weights will
            // be altered based on that condition
            Tile[] neighborsHit = _tiles[coord.y, coord.x].
                GetHitNeighbors();
            if (neighborsHit.Length > 0)
            {
                foreach (Tile neighbor in neighborsHit)
                {
                    // Alter this tile's neighbors
                    Direction dir = _tiles[coord.y, coord.x].
                        GetDirectionOfNeighbor(neighbor);
                    _tiles[coord.y, coord.x].AlterNeighborWeights(dir);

                    // Alter neighbor's neighbors
                    dir = neighbor.GetDirectionOfNeighbor(
                        _tiles[coord.y, coord.x]);
                    neighbor.AlterNeighborWeights(dir);
                }
            }
            else
                // Weights of neighbors will be lowered
                _tiles[coord.y, coord.x].LowerNeighborWeights();
        }

        /*
            The GetGuessableCoord method returns the next lowest weight
            coordinate
        */

        public Coordinate GetGuessableCoord()
        {
            return GetGuessableTiles()[0].Coordinate;
        }

        /*
            The GetGuessableTiles method returns an array containing tiles that
            can be guessed, in ascending order of weight
        */

        private Tile[] GetGuessableTiles()
        {
            // Get list of guessable tiles
            List<Tile> tiles = new List<Tile>();
            foreach (Tile tile in _tiles)
                if (!tile.IsFiredAt)
                    tiles.Add(tile);

            // Sort list by weight in ascending order
            tiles.Sort((Tile a, Tile b) => a.Weight.CompareTo(b.Weight));

            // Return sorted list of guessable tiles as an array
            return tiles.ToArray();
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
            The GetOccupiedCoords method returns an array of the coordinates
            that are currently occupied by ships
        */

        private Coordinate[] GetOccupiedCoords()
        {
            List<Coordinate> occupiedCoords = new List<Coordinate>();
            foreach (Ship ship in _ships)
                if (ship != null)
                    occupiedCoords.AddRange(ship.GetCoords());
            return occupiedCoords.ToArray();
        }

        /*
            The GetShipAtCoord method returns the ship that exists
            at a specified coordinate
        */

        public Ship GetShipAtCoord(Coordinate coord)
        {
            Ship ship = null;
            foreach (Ship s in _ships)
                if (s.GetCoords().Contains(coord))
                    ship = s;
            return ship;
        }

        /*
            The GetUnoccupiedCoords method returns an array of the coordinates
            that are not currently occupied by any ship
        */

        public Coordinate[] GetUnoccupiedCoords()
        {
            List<Coordinate> unoccupiedCoords = new List<Coordinate>();
            foreach (Tile tile in _tiles)
                if (!tile.IsOccupied)
                    unoccupiedCoords.Add(tile.Coordinate);
            return unoccupiedCoords.ToArray();
        }

        /*
            The IsCoordInRange method checks if a coordinate
            exists on the board
        */

        public bool IsCoordInRange(Coordinate coord)
        {
            bool isInRange = false;
            if (coord.x >= 0 && coord.x < _NUM_COLUMNS &&
                coord.y >= 0 && coord.y < _NUM_ROWS)
            {
                isInRange = true;
            }
            return isInRange;
        }

        /*
            The IsGuessOK method returns whether the specified
            coordinate can be guessed
        */

        public bool IsGuessOK(Coordinate coord)
        {
            bool ok = false;
            if (IsCoordInRange(coord) && !_tiles[coord.y, coord.x].IsFiredAt)
                ok = true;
            return ok;
        }

        /*
            The IsOccupied method returns whether the specified
            coordinate is occupied by a ship
        */

        public bool IsOccupied(Coordinate coord)
        {
            bool occupied = false;
            if (_tiles[coord.y, coord.x].IsOccupied)
                occupied = true;
            return occupied;
        }

        /*
            The IsShipExisting method returns whether the specified ship exists
            or is alive on the board
        */

        public bool IsShipExisting(ShipType type)
        {
            bool exists = false;
            Ship ship = _ships[(int)type];
            if (ship != null && ship.NumParts >= 1)
                exists = true;
            return exists;
        }

        /*
            The IsShipPlacementOK method checks if a ship with the given
            coordinates can be placed on the board without error
        */

        public bool IsShipPlacementOK(Coordinate[] coords)
        {
            bool ok = true;

            // Check for coords out of range
            foreach (Coordinate coord in coords)
            {
                if (!IsCoordInRange(coord))
                    ok = false;
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
            The MarkHit method marks a tile at the specified coordinate
            as having been guessed
            If the tile is occupied by a ship, the ship loses parts
            The method returns whether a ship has been hit
        */

        public bool MarkHit(Coordinate coord)
        {
            bool shipHit = false;
            _tiles[coord.y, coord.x].IsFiredAt = true;
            if (IsOccupied(coord))
            {
                GetShipAtCoord(coord).NumParts--;
                shipHit = true;
            }
            return shipHit;
        }

        /*
            The PlaceShip method places a ship on the board
            It accepts the type of ship being placed and
            its coordinates as arguments
            The method returns a boolean indicating whether
            the operation was successful
        */

        public bool PlaceShip(ShipType type, Coordinate[] coords)
        {
            bool success = false;

            if (IsShipPlacementOK(coords))
            {
                // Fetch the tiles to assign to the ship
                Tile[] tiles = new Tile[coords.Length];
                for (int i = 0; i < tiles.Length; i++)
                {
                    tiles[i] = _tiles[coords[i].y, coords[i].x];
                    tiles[i].IsOccupied = true;
                }
                _ships[(int)type] = new Ship(type, tiles);
                success = true;
            }

            return success;
        }

        /*
            The SetNeighbors method sets the neighbors of the board's tiles
        */

        private void SetNeighbors()
        {
            foreach (Tile tile in _tiles)
            {
                // Get tile's coordinate
                Coordinate coord = tile.Coordinate;

                // Set neighbors
                // North
                Direction dir = Direction.North;
                if (coord.y > 0)
                    tile.Neighbors[(int)dir] = _tiles[coord.y - 1, coord.x];
                // South
                dir++;
                if (coord.y < _NUM_ROWS - 1)
                    tile.Neighbors[(int)dir] = _tiles[coord.y + 1, coord.x];
                // East
                dir++;
                if (coord.x < _NUM_COLUMNS - 1)
                    tile.Neighbors[(int)dir] = _tiles[coord.y, coord.x + 1];
                // West
                dir++;
                if (coord.x > 0)
                    tile.Neighbors[(int)dir] = _tiles[coord.y, coord.x - 1];
            }
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
