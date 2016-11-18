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
        private bool _isOccupied, _isGuessed;
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
            _isGuessed = false;
            _weight = 0;
            _neighbors = new Tile[NUM_NEIGHBORS];
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

            for (Direction d = Direction.North; d <= Direction.West; d++)
            {
                if (_neighbors[(int)d] != null &&
                    !_neighbors[(int)d]._isGuessed)
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
        */

        public Direction GetDirectionOfNeighbor(Tile neighbor)
        {
            Direction direction = Direction.North;
            for (Direction dir = Direction.North; dir <= Direction.West; dir++)
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
            foreach (Tile neigh in _neighbors)
                if (neigh != null && neigh._isGuessed && neigh._isOccupied)
                    hitNeighbors.Add(neigh);
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

            for (Direction dir = Direction.North; dir <= Direction.West; dir++)
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
            IsGuessed property
        */

        public bool IsGuessed
        {
            get { return _isGuessed; }
            set { _isGuessed = value; }
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
