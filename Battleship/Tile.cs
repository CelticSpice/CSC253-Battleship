﻿/*
    This class represents a tile on the board
*/

namespace Battleship
{
    public class Tile
    {
        // Fields
        private Coordinate _coordinate;
        private bool _isOccupied;
        private bool _isFiredAt;
        private int _weight;

        /*
            Constructor
            Accepts the Tile's coordinate as an argument
        */

        public Tile(Coordinate coord)
        {
            _coordinate = coord;
            _isOccupied = false;
            _isFiredAt = false;
            _weight = 0;
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
    }
}
