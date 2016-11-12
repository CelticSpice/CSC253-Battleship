/*
    This class represents a player
*/

using System;
using System.Linq;

namespace Battleship
{
    public class Player
    {
        // Fields
        private Board _board;
        private Player _opponent;

        /*
            Constructor
        */

        public Player()
        {
            _board = new Board();
            _opponent = null;
        }

        /*
            Constructor
            Accepts the player's opponent
        */

        public Player(Player opponent)
        {
            _board = new Board();
            _opponent = opponent;
        }

        /*
            The MakeGuess method has the player make a guess
            of a coordinate to shoot at
        */

        private Coordinate MakeGuess()
        {
            return _opponent.Board.GetGuessableCoord();
        }

        /*
            The SetupBoard method has the player setup its board with ships
        */

        public void SetupBoard()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            // Setup each of five ships
            for (int i = 0; i < _board.Ships.Length; i++)
            {
                ShipType type = (ShipType)i;

                // Get number of parts
                int numParts = 0;
                switch (type)
                {
                    case ShipType.AircraftCarrier:
                        numParts = 5;
                        break;
                    case ShipType.Battleship:
                        numParts = 4;
                        break;
                    case ShipType.Submarine:
                    case ShipType.Destroyer:
                        numParts = 3;
                        break;
                    case ShipType.PatrolBoat:
                        numParts = 2;
                        break;
                }

                // Prepare array of ship coordinates
                Coordinate[] shipCoords = new Coordinate[numParts];

                bool success;   // Whether successful in setting up ship
                do
                {
                    // Determine direction to setup ship in
                    const int NUM_DIRECTIONS = 4;
                    Direction direction = (Direction)rand.Next(NUM_DIRECTIONS);

                    // Get origin coordinate
                    Coordinate coord = new Coordinate { x = rand.Next(_board.Columns), y = rand.Next(_board.Rows) };

                    // Get coords in direction setting up ship in
                    for (int j = 0; j < numParts; j++)
                    {
                        switch (direction)
                        {
                            case Direction.North:
                                shipCoords[j] = new Coordinate { x = coord.x, y = coord.y - j };
                                break;
                            case Direction.South:
                                shipCoords[j] = new Coordinate { x = coord.x, y = coord.y + j };
                                break;
                            case Direction.East:
                                shipCoords[j] = new Coordinate { x = coord.x + j, y = coord.y };
                                break;
                            case Direction.West:
                                shipCoords[j] = new Coordinate { x = coord.x - j, y = coord.y };
                                break;
                        }
                    }

                    // Attempt to place ship
                    success = _board.PlaceShip(type, shipCoords);


                } while (!success);
            }
        }

        /*
            The TakeTurn method has the player take its turn
            It returns the coordinate guessed
        */

        public Coordinate TakeTurn()
        {
            Coordinate guess = MakeGuess();
            _opponent.Board.Tiles[guess.y, guess.x].IsFiredAt = true;
            if (_opponent.Board.IsHit(guess))
            {
                _opponent.Board.GetShipAtCoord(guess).NumParts--;

                // Lower the weights of tiles surrounding hit
                // to indicate likely ship locations
                _opponent.Board.LowerWeights(guess);
            }
            return guess;
        }

        /*
            Board Property
        */

        public Board Board
        {
            get { return _board; }
        }

        /*
            Opponent Property
        */

        public Player Opponent
        {
            get { return _opponent; }
            set { _opponent = value; }
        }
    }
}
