/*
    This class represents a tile on the board
*/

using System;
using System.Collections.Generic;

namespace Battleship
{
    public class Tile
    {
        // Fields
        private const int NUM_NEIGHBORS = 4;

        private Coordinate _coordinate;
        private bool _isOccupied;
        private bool _isFiredAt;
        private int _weight;
        private Tile[] _neighbors;

        /*
            Constructor
            Accepts the tile's coordinate as an argument
        */

        public Tile(Coordinate coord)
        {
            _coordinate = coord;
            _isOccupied = false;
            _isFiredAt = false;
            _weight = 0;
            _neighbors = new Tile[NUM_NEIGHBORS];
        }

        /*
            Copy Constructor
        */

        public Tile(Tile toCopy)
        {
            _coordinate = toCopy._coordinate;
            _isOccupied = toCopy._isOccupied;
            _isFiredAt = toCopy._isFiredAt;
            _weight = toCopy._weight;
            _neighbors = toCopy._neighbors;
        }

        /*
            The AlterNeighborWeights method alters the weights of the tile's
            neighbors based on a specified direction. The direction and its
            cardinal opposite specify neighbors to lower the weight of; the
            opposite is true of neighbors in the other directions
        */

        public void AlterNeighborWeights(Direction direction)
        {
            const int LOWER_ALTER = -65;
            const int HIGHER_ALTER = 30;

            for (Direction d = Direction.North; d < Direction.Null; d++)
            {
                if (_neighbors[(int)d] != null)
                {
                    switch (direction)
                    {
                        case Direction.North:
                        case Direction.South:
                            if (d == Direction.North || d == Direction.South)
                                _neighbors[(int)d]._weight += LOWER_ALTER;
                            else
                                _neighbors[(int)d]._weight += HIGHER_ALTER;
                            break;
                        case Direction.East:
                        case Direction.West:
                            if (d == Direction.East || d == Direction.West)
                                _neighbors[(int)d]._weight += LOWER_ALTER;
                            else
                                _neighbors[(int)d]._weight += HIGHER_ALTER;
                            break;
                    }
                }
            }

        }

        /*
            The GetDirectionOfNeighbor method returns the direction
            that a specified neighbor is in relation to this tile
            If the tile is not a neighbor, null direction is returned
        */

        public Direction GetDirectionOfNeighbor(Tile neighbor)
        {
            Direction direction = Direction.Null;
            for (Direction dir = Direction.North; dir < Direction.Null; dir++)
                if (_neighbors[(int)dir] != null &&
                    _neighbors[(int)dir].Equals(neighbor))
                {
                    direction = dir;
                }
            return direction;
        }

        /*
            The GetHitNeighbors method returns an array containing neighbors
            that have been hit
        */

        public Tile[] GetHitNeighbors()
        {
            List<Tile> hitNeighbors = new List<Tile>();
            foreach (Tile neighbor in _neighbors)
                if (neighbor != null && neighbor._isFiredAt)
                    hitNeighbors.Add(neighbor);
            return hitNeighbors.ToArray();
        }

        /*
            The LowerNeighborWeights method lowers the weights of the tile's
            neighbors
        */

        public void LowerNeighborWeights()
        {
            const int MIN = 80;
            const int MAX = 100;
            Random rand = new Random((int)DateTime.Now.Ticks);

            for (Direction dir = Direction.North; dir < Direction.Null; dir++)
                if (_neighbors[(int)dir] != null)
                    _neighbors[(int)dir]._weight -= rand.Next(MIN, MAX);
        }

        /*
            Coordinate property
        */

        public Coordinate Coordinate
        {
            get { return _coordinate; }
        }

        /*
            IsOccupied Property
        */

        public bool IsOccupied
        {
            get { return _isOccupied; }
            set { _isOccupied = value; }
        }

        /*
            IsFiredAt property
        */

        public bool IsFiredAt
        {
            get { return _isFiredAt; }
            set { _isFiredAt = value; }
        }

        /*
            Weight Property
        */

        public int Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        /*
            Neighbors Property
        */

        public Tile[] Neighbors
        {
            get { return _neighbors; }
        }
    }
}
