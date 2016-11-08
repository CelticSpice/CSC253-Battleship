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
        private Coordinate _lastHit;
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
            The MakeGuess method generates a coordinate
            If true is passed, the coordinate generated will be
            based off of the last coordinate hit
        */

        public Coordinate MakeGuess(bool intelligentDecision = false)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            Coordinate coord;

            Coordinate[] guessable = _opponent._board.GetGuessableCoords();

            if (!intelligentDecision)
                coord = guessable[rand.Next(guessable.Length)];
            else
            {
                // Code here
                coord = new Coordinate();
            }

            return coord;
        }

        /*
            The SetOpponent method sets the player's opponent
        */

        public void SetOpponent(Player opponent)
        {
            _opponent = opponent;
        }

        /*
            The SetupBoard method randomly places ships on the player's board
        */

        public void SetupBoard()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            // Setup each of five ships
            for (int i = 0; i < _board.Ships.Length; i++)
            {
                ShipType type = (ShipType)i;
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
                    Direction direction = (Direction)rand.Next(4);

                    // Get first coordinate
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
            The TakeTurn method has the player make guesses for as long
            as its guess is a hit
        */

        public void TakeTurn()
        {
            bool keepGoing = true;
            do
            {
                // Player makes guess
                Coordinate coord = MakeGuess();

                // Check if ship is hit
                if (_opponent._board.IsHit(coord))
                {
                    // Ship takes hit
                    Ship ship = _opponent._board.GetShipHit(coord);
                    ship.NumParts--;

                    // Check if game is over
                    if (_opponent._board.GetNumShipsLiving() == 0)
                        keepGoing = false;
                }
                else
                    keepGoing = false;
            } while (!keepGoing);
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
