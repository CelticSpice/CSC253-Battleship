﻿/*
    This class represents a tile on the board
    12/8/2016
    CSC 253 0001 - M6PROJ
    Author: James Alves, Shane McCann, Timothy Burns
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
        private bool _isOccupied, _isShot;
        private int _weight;
        private Tile[] neighbors;

        /*
            Constructor
            Accepts the tile's coordinate as an argument
        */

        public Tile(Coordinate coord)
        {
            _coordinate = coord;
            _isOccupied = false;
            _isShot = false;
            _weight = 0;
            neighbors = new Tile[NUM_NEIGHBORS];
        }

        /*
            The AlterNeighborWeights method alters the weights of the tile's
            neighbors based on a specified direction. The direction and its
            cardinal opposite specify neighbors to lower the weight of; the
            opposite is true of neighbors in the other directions
        */

        public void AlterNeighborWeights(Direction direction)
        {
            const int LOWER_ALTER = -89;
            const int HIGHER_ALTER = 30;

            for (Direction d = Direction.North; d <= Direction.West; d++)
            {
                if (neighbors[(int)d] != null && !neighbors[(int)d]._isShot)
                {
                    switch (direction)
                    {
                        case Direction.North:
                        case Direction.South:
                            if (d == Direction.North || d == Direction.South)
                                neighbors[(int)d]._weight += LOWER_ALTER;
                            else
                                neighbors[(int)d]._weight += HIGHER_ALTER;
                            break;
                        case Direction.East:
                        case Direction.West:
                            if (d == Direction.East || d == Direction.West)
                                neighbors[(int)d]._weight += LOWER_ALTER;
                            else
                                neighbors[(int)d]._weight += HIGHER_ALTER;
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
                if (neighbors[(int)dir] != null &&
                    neighbors[(int)dir].Equals(neighbor))
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
            foreach (Tile n in neighbors)
                if (n != null && n.IsShotAndOccupied())
                    hitNeighbors.Add(n);
            return hitNeighbors.ToArray();
        }

        /*
            The GetNeighbors method returns the tile's
            neighbors
        */

        public Tile[] GetNeighbors()
        {
            return neighbors;
        }

        /*
            The IsLPivot method returns whether the tile is the pivot
            of an 'L' shape of hit tiles
        */

        public bool IsLPivot()
        {
            bool isPivot = false;
            for (Direction d = Direction.North; d <= Direction.West && !isPivot; d++)
            {
                if (neighbors[(int)d] != null && neighbors[(int)d].IsShotAndOccupied())
                {
                    switch (d)
                    {
                        case Direction.North:
                        case Direction.South:
                            Tile eastN = neighbors[(int)Direction.East];
                            Tile westN = neighbors[(int)Direction.West];

                            if (eastN != null && eastN.IsShotAndOccupied() ||
                                westN != null && westN.IsShotAndOccupied())
                            {
                                isPivot = true;
                            }
                            break;
                        case Direction.East:
                        case Direction.West:
                            Tile northN = neighbors[(int)Direction.North];
                            Tile southN = neighbors[(int)Direction.South];

                            if (northN != null && northN.IsShotAndOccupied() ||
                                southN != null && southN.IsShotAndOccupied())
                            {
                                isPivot = true;
                            }
                            break;
                    }
                }
            }
            return isPivot;
        }

        /*
            IsShotAndOccupied - Returns whether the tile is shot and occupied
        */

        private bool IsShotAndOccupied()
        {
            return _isShot && _isOccupied;
        }

        /*
            The LowerNeighborWeights method lowers the weights of the tile's
            neighbors
        */

        public void LowerNeighborWeights()
        {
            const int MIN = 90;
            const int MAX = 100;
            Random rand = new Random((int)DateTime.Now.Ticks);

            for (Direction dir = Direction.North; dir <= Direction.West; dir++)
                if (neighbors[(int)dir] != null)
                    neighbors[(int)dir]._weight -= rand.Next(MIN, MAX);
        }

        /*
            Coordinate Property
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
            IsShot Property
        */

        public bool IsShot
        {
            get { return _isShot; }
            set { _isShot = value; }
        }

        /*
            Weight Property
        */

        public int Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
    }
}
