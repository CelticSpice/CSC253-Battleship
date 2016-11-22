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
            The InformOfShot method tells the player of a shot
            that has been made
            The method modifies the shot to include results
        */

        private void InformOfShot(Shot shot)
        {
            board.MarkShot(shot);
        }

        /*
            The IsDefeated method returns whether the player has been
            defeated
        */

        public bool IsDefeated()
        {
            return !board.AreShipsLiving();
        }

        /*
            The MakeShot method has the player make a shot automatically
            The method returns the shot made
        */

        public Shot MakeShot()
        {
            Shot shot = new Shot();
            shot.Coord = opponent.board.NextGuessableCoord();
            opponent.InformOfShot(shot);
            return shot;
        }

        /*
            The MakeShot method has the player make a shot at the
            specified coordinate
            The method returns the shot made
        */

        public Shot MakeShot(Coordinate coord)
        {
            Shot shot = new Shot();
            shot.Coord = coord;
            opponent.InformOfShot(shot);
            return shot;
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
            automatically
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
            The TellShipCoords method returns an array containing coordinates
            that the specified ship occupies on the player's board
        */

        public Coordinate[] TellShipCoords(ShipType type)
        {
            return board.GetShipCoords(type);
        }

        /*
            The TellUnoccupiedCoords method returns an array of coordinates
            on the player's board that are unoccupied
        */

        public Coordinate[] TellUnoccupiedCoords()
        {
            return board.GetUnoccupiedCoords();
        }

        /*
            The TellIfShotOK method returns whether a shot can be made
            on the player's board at the specified coordinate
        */

        public bool TellIfShotOK(Coordinate coord)
        {
            return board.IsShotOK(coord);
        }

        /*
            The TellIfSetupOK method returns whether a ship
            with the given coordinates can be placed on the player's board
        */

        public bool TellIfSetupOK(Coordinate[] coords)
        {
            return board.IsSetupOK(coords);
        }
    }
}
