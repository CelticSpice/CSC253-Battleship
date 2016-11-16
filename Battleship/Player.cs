/*
    This class represents a player
*/

using System;

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
            The InformOfGuess method informs the player of a guess made
            and returns whether a ship was hit
        */

        public bool InformOfGuess(Coordinate guess)
        {
            bool isHit = _board.MarkHit(guess);
            return isHit;
        }

        /*
            The MakeGuess method has the player make a guess
            of a coordinate to shoot at
        */

        public Coordinate MakeGuess()
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
                    Direction dir = (Direction)rand.Next(NUM_DIRECTIONS);

                    // Get origin coordinate
                    Coordinate coord = new Coordinate {
                        x = rand.Next(_board.Columns),
                        y = rand.Next(_board.Rows)
                    };

                    // Get coords in direction setting up ship in
                    for (int j = 0; j < numParts; j++)
                    {
                        switch (dir)
                        {
                            case Direction.North:
                                shipCoords[j] = new Coordinate {
                                     x = coord.x,
                                     y = coord.y - j
                                };
                                break;
                            case Direction.South:
                                shipCoords[j] = new Coordinate {
                                     x = coord.x,
                                     y = coord.y + j
                                };
                                break;
                            case Direction.East:
                                shipCoords[j] = new Coordinate {
                                     x = coord.x + j,
                                     y = coord.y
                                };
                                break;
                            case Direction.West:
                                shipCoords[j] = new Coordinate {
                                     x = coord.x - j,
                                     y = coord.y
                                };
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
            if (_opponent.InformOfGuess(guess))
            {
                // Alter weights of tiles appropriately
                _opponent.Board.AlterNeighborWeights(guess);
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
