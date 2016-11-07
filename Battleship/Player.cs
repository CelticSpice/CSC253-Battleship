/*
    This class represents a player
*/

using System;

namespace Battleship
{
    public class Player
    {
        // Fields
        private bool _isAI;
        private Board _board;
        private Player _opponent;

        /*
            Constructor
            Accepts the player's opponent whether the player is an AI
        */

        public Player(Player opponent, bool ai = false)
        {
            _isAI = ai;
            _board = new Board();
            _opponent = opponent;
        }

        /*
            The MakeGuess method generates a coordinate
        */

        public Coordinate MakeGuess()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return new Coordinate { x = rand.Next(_board.Columns), y = rand.Next(_board.Rows) };
        }

        /*
            The SetupBoard method randomly places ships on the player's board
        */

        public void SetupBoard()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            // Setup each of five ships
            int numParts = 0;
            for (int i = 0; i < 5; i++)
            {
                ShipType type = (ShipType)i;

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
                    ShipDirection direction = (ShipDirection)rand.Next(4);

                    // Get first coordinate
                    Coordinate coord = new Coordinate { x = rand.Next(_board.Columns), y = rand.Next(_board.Rows) };

                    // Get coords in direction setting up ship in
                    for (int j = 0; j < numParts; j++)
                    {
                        switch (direction)
                        {
                            case ShipDirection.North:
                                shipCoords[j] = new Coordinate { x = coord.x, y = coord.y - j };
                                break;
                            case ShipDirection.South:
                                shipCoords[j] = new Coordinate { x = coord.x, y = coord.y + j };
                                break;
                            case ShipDirection.East:
                                shipCoords[j] = new Coordinate { x = coord.x + j, y = coord.y };
                                break;
                            case ShipDirection.West:
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
            IsAI Property
        */

        public bool IsAI
        {
            get { return _isAI; }
        }

        /*
            Board Property
        */

        public Board Board
        {
            get { return _board; }
        }
    }
}
