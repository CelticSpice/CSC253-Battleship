/*
    This class represents a player
    12/8/2016
    CSC 253 0001 - M6PROJ
    Author: James Alves, Shane McCann, Timothy Burns
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
            GetBoardSize - Returns the size of the player's board
            (N = NxN)
        */

        public int GetBoardSize()
        {
            return board.Rows;
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
            IsDefeated - Returns whether the player is defeated
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
            Shot shot = new Shot(this);
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
            Shot shot = new Shot(this);
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
            for (ShipType type = ShipType.Aircraft_Carrier;
                 type <= ShipType.Patrol_Boat; type++)
            {
                // Get number of parts
                int numParts = 0;
                switch (type)
                {
                    case ShipType.Aircraft_Carrier:
                        numParts = 5;
                        break;
                    case ShipType.Battleship:
                        numParts = 4;
                        break;
                    case ShipType.Submarine:
                    case ShipType.Destroyer:
                        numParts = 3;
                        break;
                    case ShipType.Patrol_Boat:
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
            TellNumShipsLiving - Returns the number of ships living
            on the player's board
        */

        public int TellNumShipsLiving()
        {
            return board.GetNumShipsLiving();
        }

        /*
           TellShipCoords - Returns coordinates that a type of ship
           occupies on the player's board
        */

        public Coordinate[] TellShipCoords(ShipType shipType)
        {
            return board.GetShipCoords(shipType);
        }

        /*
            TellUnoccupiedCoords - Returns unoccupied coordinates
            on the player's board
        */

        public Coordinate[] TellUnoccupiedCoords()
        {
            return board.GetUnoccupiedCoords();
        }

        /*
            TellIfShotOK - Returns whether a shot can be made
            at a coordinate on the player's board
        */

        public bool TellIfShotOK(Coordinate coord)
        {
            return board.IsShotOK(coord);
        }

        /*
            TellIfSetupOK - Returns whether a ship can be placed on
            the player's board with the given coordinates
        */

        public bool TellIfSetupOK(Coordinate[] coords)
        {
            return board.IsSetupOK(coords);
        }
    }
}
