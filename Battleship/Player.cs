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
            The GetChoices method asks the player for valid guesses that can be
            made on its board
        */

        public Coordinate[] GetChoices()
        {
            return _board.GetGuessableCoords();
        }

        /*
            The GetIfHit method asks the player if a tile on the player's board
            specified by a coordinate is a hit
        */

        public bool GetIfHit(Coordinate coord)
        {
            return _board.IsHit(coord);
        }

        /*
            The GetIfShipSunk method asks the player if a ship of the specified
            type is sunk
        */

        public bool GetIfShipSunk(ShipType type)
        {
            return _board.IsSunk(type);
        }

        /*
            The GetShipTypeAt method returns the type of ship that exists
            at the specified coordinate
        */

        public ShipType GetShipTypeAt(Coordinate coord)
        {
            return _board.GetShipAtCoord(coord).Type;
        }

        /*
            The InformOfGuess method informs the player of a guess made
            of a shot to take on its board
        */

        public void InformOfGuess(Coordinate guess)
        {
            _board.MarkGuess(guess);
        }

        /*
            The MakeGuess method has the player make a guess
            of a coordinate to shoot at
        */

        public Coordinate MakeGuess()
        {
            const int LOWEST_WEIGHT_CHOICE = 0;
            return _opponent.GetChoices()[LOWEST_WEIGHT_CHOICE];
        }

        /*
            The SetupBoard method has the player setup its board with ships
        */

        public void SetupBoard()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            // Setup each of five ships
            for (ShipType type = ShipType.AircraftCarrier;
                 type <= ShipType.PatrolBoat; type++)
            {
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

                    // Get coordinates in direction setting up ship in
                    for (int c = 0; c < numParts; c++)
                    {
                        switch (dir)
                        {
                            case Direction.North:
                                shipCoords[c] = new Coordinate {
                                     x = coord.x,
                                     y = coord.y - c
                                };
                                break;
                            case Direction.South:
                                shipCoords[c] = new Coordinate {
                                     x = coord.x,
                                     y = coord.y + c
                                };
                                break;
                            case Direction.East:
                                shipCoords[c] = new Coordinate {
                                     x = coord.x + c,
                                     y = coord.y
                                };
                                break;
                            case Direction.West:
                                shipCoords[c] = new Coordinate {
                                     x = coord.x - c,
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
