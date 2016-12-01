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
        private Tile[,] tiles;
        private Ship[] ships;

        /*
            No-Arg constructor
        */

        public Board()
        {
            const int MAX = 100;
            Random rand = new Random((int)DateTime.Now.Ticks);
            tiles = new Tile[_NUM_ROWS, _NUM_COLUMNS];
            for (int row = 0; row < _NUM_ROWS; row++)
                for (int col = 0; col < _NUM_COLUMNS; col++)
                {
                    Tile tile = new Tile(new Coordinate { x = col, y = row });
                    tile.Weight = rand.Next(MAX);
                    tiles[row, col] = tile;
                }
            ships = new Ship[_NUM_SHIPS];

            SetNeighbors();
        }

        /*
            The AlterWeights method alters the weights of tiles
            around a given tile, taking into
            consideration neighboring tiles that may have been hit
        */

        private void AlterWeights(Tile tile)
        {
            Tile[] neighborsHit = tile.GetHitNeighbors();

            // Check if tile and/or its neighbors are L-pivots     
            if (neighborsHit.Length > 0)
            {
                bool altered = false;
                if (tile.IsLPivot())
                {
                    altered = true;
                    tile.LowerNeighborWeights();
                }

                foreach (Tile neighbor in neighborsHit)
                    if (neighbor.IsLPivot())
                    {
                        altered = true;
                        neighbor.LowerNeighborWeights();
                        tile.LowerNeighborWeights();
                    }

                // If weights have not already been altered,
                // alter weights based on direction of neighbors
                if (!altered)
                {
                    foreach (Tile neighbor in neighborsHit)
                    {
                        // Alter the tile's neighbors
                        Direction dir = tile.GetDirectionOfNeighbor(neighbor);
                        tile.AlterNeighborWeights(dir);

                        // Alter neighbor's neighbors
                        dir = neighbor.GetDirectionOfNeighbor(tile);
                        neighbor.AlterNeighborWeights(dir);
                    }
                }
            }
            else
                // Weights of neighbors will be lowered
                tile.LowerNeighborWeights();
        }

        /*
            AreShipsLiving - Returns whether there are ships
            on the board that have at least 1 part
        */

        public bool AreShipsLiving()
        {
            bool living = false;
            for (int i = 0; i < ships.Length && !living; i++)
                if (ships[i].NumParts > 0)
                    living = true;
            return living;
        }

        /*
         GetNumShipsLiving - Returns the number of ships that have at
         least 1 part remaining
        */

        public int GetNumShipsLiving()
        {
            int numLiving = 0;
            foreach (Ship ship in ships)
                if (ship.NumParts > 0)
                    numLiving++;
            return numLiving;
        }

        /*
            GetOccupiedCoords - Returns occupied coordinates on
            the board
        */

        private Coordinate[] GetOccupiedCoords()
        {
            List<Coordinate> coords = new List<Coordinate>();
            foreach (Ship ship in ships)
                if (ship != null)
                    coords.AddRange(ship.GetCoords());
            return coords.ToArray();
        }

        /*
           GetShipCoords - Returns coordinates that a type of ship
           occupies on the board
        */

        public Coordinate[] GetShipCoords(ShipType shipType)
        {
            return ships[(int)shipType].GetCoords();
        }

        /*
            The GetShipAtCoord method returns the ship that exists
            at a specified coordinate
        */

        private Ship GetShipAtCoord(Coordinate coord)
        {
            return ships.First(ship => ship.GetCoords().Contains(coord));
        }

        /*
           GetUnoccupiedCoords - Returns unoccupied coordinates
           on the board
        */

        public Coordinate[] GetUnoccupiedCoords()
        {
            List<Coordinate> coords = new List<Coordinate>();
            foreach (Tile tile in tiles)
                if (!tile.IsOccupied)
                    coords.Add(tile.Coordinate);
            return coords.ToArray();
        }

        /*
            IsCoordInRange - Returns whether a coordinate
            exists on the board
        */

        private bool IsCoordInRange(Coordinate coord)
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
            IsShotOK - Returns whether a coordinate can
            be shot at on the board
        */

        public bool IsShotOK(Coordinate coord)
        {
            return !tiles[coord.y, coord.x].IsShot;
        }

        /*
            IsSetupOK - Returns whether a ship can placed on
            the board with the given coordinates
        */

        public bool IsSetupOK(Coordinate[] coords)
        {
            bool ok = true;

            // Check for coords out of range
            foreach (Coordinate coord in coords)
                if (!IsCoordInRange(coord))
                    ok = false;

            // Check for placement on occupied coordinates
            if (ok)
                foreach (Coordinate coord in GetOccupiedCoords())
                    if (coords.Contains(coord))
                        ok = false;

            return ok;
        }

        /*
            The MarkShot method marks a tile at the coordinate
            the specified shot targets as having been guessed
            If a ship exists at the tile shot, the ship loses
            parts and weights are altered
            The method modifies the shot to include results
        */

        public void MarkShot(Shot shot)
        {
            tiles[shot.Coord.y, shot.Coord.x].IsShot = true;
            if (tiles[shot.Coord.y, shot.Coord.x].IsOccupied)
            {
                Ship ship = GetShipAtCoord(shot.Coord);
                if (--ship.NumParts > 0)
                    shot.Result = ShotResult.Hit;
                else
                    shot.Result = ShotResult.Sink;
                shot.ShipHit = ship.Type;
                AlterWeights(tiles[shot.Coord.y, shot.Coord.x]);
            }
            else
                shot.Result = ShotResult.Miss;
        }

        /*
            The NextGuessableCoord method returns the coordinate
            of the next guessable and lowest weight tile
        */

        public Coordinate NextGuessableCoord()
        {
            Tile lowestTile = tiles[0, 0];
            foreach (Tile currentTile in tiles)
                if (lowestTile.IsShot || (!currentTile.IsShot &&
                    currentTile.Weight < lowestTile.Weight))
                {
                    lowestTile = currentTile;
                }
            return lowestTile.Coordinate;
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
            if (IsSetupOK(coords))
            {
                ships[(int)type] = new Ship(type, coords);
                foreach (Coordinate coord in coords)
                    tiles[coord.y, coord.x].IsOccupied = true;
                success = true;
            }
            return success;
        }

        /*
            The SetNeighbors method sets the neighbors of the board's tiles
        */

        private void SetNeighbors()
        {
            foreach (Tile tile in tiles)
            {
                // Get tile's coordinate
                Coordinate coord = tile.Coordinate;

                // Set neighbors
                // North
                Direction dir = Direction.North;
                if (coord.y > 0)
                    tile.GetNeighbors()[(int)dir] = tiles[coord.y - 1, coord.x];
                // South
                dir++;
                if (coord.y < _NUM_ROWS - 1)
                    tile.GetNeighbors()[(int)dir] = tiles[coord.y + 1, coord.x];
                // East
                dir++;
                if (coord.x < _NUM_COLUMNS - 1)
                    tile.GetNeighbors()[(int)dir] = tiles[coord.y, coord.x + 1];
                // West
                dir++;
                if (coord.x > 0)
                    tile.GetNeighbors()[(int)dir] = tiles[coord.y, coord.x - 1];
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
    }
}
