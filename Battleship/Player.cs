/*
    This class represents a player
*/

using System;

namespace Battleship
{
    public class Player
    {
        // Fields
        private Board board;
        private Player opponent;

        /*
            Constructor
        */

        public Player()
        {
            board = new Board();
            opponent = null;
        }

        /*
            Constructor
            Accepts the player's opponent
        */

        public Player(Player oppo)
        {
            board = new Board();
            opponent = oppo;
        }

        /*
            The GetIfSetupOK method returns whether a ship
            with the given coordinates can be placed on the player's board
        */

        public bool GetIfSetupOK(Coordinate[] coords)
        {
            return board.IsShipPlacementOK(coords);
        }

        /*
            The InformOfGuess method tells the player that a guess has been
            made at the specified coordinate
            The method returns the result of the guess
        */

        public GuessResult InformOfGuess(Coordinate guess)
        {
            GuessResult result = GuessResult.Miss;
            board.MarkGuess(guess);
            if (board.IsHit(guess))
            {
                if (board.IsSunk(board.GetShipTypeAtCoord(guess)))
                    result = GuessResult.Sink;
                else
                    result = GuessResult.Hit;
            }
            return result;
        }

        /*
            The MakeGuess method has the player make a guess
            of a coordinate to shoot at
        */

        public Coordinate MakeGuess()
        {
            return opponent.board.GetGuessableCoord();
        }

        /*
            The PlaceShip method has the player place the specified
            ship type on its board with the given coordinates
        */

        public void PlaceShip(ShipType type, Coordinate[] coords)
        {
            board.PlaceShip(type, coords);
        }

        /*
            The SetOpponent method sets the player's opponent
        */

        public void SetOpponent(Player oppo)
        {
            opponent = oppo;
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
                        x = rand.Next(board.Columns),
                        y = rand.Next(board.Rows)
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
                    success = board.PlaceShip(type, shipCoords);


                } while (!success);
            }
        }

        /*
            The TellIfGuessOK method returns whether a guess can be made
            at the specified coordinate on the player's board
        */

        public bool TellIfGuessOK(Coordinate guess)
        {
            return board.IsGuessOK(guess);
        }

        /*
            The TellNumShipsLiving method returns the number of ships that are
            currently living (have 1 or more parts that have not been hit)
            on the player's board
        */

        public int TellNumShipsLiving()
        {
            return board.GetNumShipsLiving();
        }

        /*
            The TellShipCoords method returns an array containing coordinates
            that the specified ship occupies
        */

        public Coordinate[] TellShipCoords(ShipType type)
        {
            return board.GetShipCoords(type);
        }

        /*
            The TellShipHit method returns the ship type that was hit on the
            player's board at the specified coordinate
        */

        public ShipType TellShipHit(Coordinate coord)
        {
            return board.GetShipTypeAtCoord(coord);
        }

        /*
            The TellUnoccupiedCoords method returns an array of coordinates
            on the player's board that are unoccupied
        */

        public Coordinate[] TellUnoccupiedCoords()
        {
            return board.GetUnoccupiedCoords();
        }
    }
}
