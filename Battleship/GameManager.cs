/*
    This class handles the game logic
*/
using System;

namespace Battleship
{
    public class GameManager
    {
        enum ReturnValue { Success, Failure = -1}

        // Fields
        private Board _playerBoard, _computerBoard;
        private bool _isSetupMode;
        private ShipType _shipSettingUp;


        /*
            No-Arg Constructor
        */

        public GameManager()
        {
            _playerBoard = new Board();
            _computerBoard = new Board();
            _isSetupMode = true;
            _shipSettingUp = ShipType.AircraftCarrier;
        }

        /*
            IsSetupMode Property
        */

        public bool IsSetupMode
        {
            get { return _isSetupMode; }
            set { _isSetupMode = value; }
        }

        /*
            PlayerBoard Property
        */

        public Board PlayerBoard
        {
            get { return _playerBoard; }
        }

        /*
            ShipSettingUp Property
        */

        public ShipType ShipSettingUp
        {
            get { return _shipSettingUp; }
            set { _shipSettingUp = value; }
        }

        /*
             PopulateComputerBoard
        */

        public void PopulateComputerBoard()
        {
            Random rand = new Random();
            int numParts = 0;
            ShipType type;

            for (int i = 0; i < 5; i++)
            {
                type = (ShipType) i;

                switch (i)
                {
                    case 0:
                        numParts = 5;
                        break;
                    case 1:
                        numParts = 4;
                        break;
                    case 2:
                        numParts = 3;
                        break;
                    case 3:
                        numParts = 3;
                        break;
                    case 4:
                        numParts = 2;
                        break;
                }

                Coordinate[] shipCoordinates = new Coordinate[numParts];

                ShipDirection direc = (ShipDirection)rand.Next(5);
                Coordinate coordinate = new Coordinate { x = rand.Next(10), y = rand.Next(10)};

                ReturnValue success;
                do
                {
                    for (int n = 0; n < numParts; n++)
                    {
                        switch (direc)
                        {
                            case ShipDirection.North:
                                shipCoordinates[n] = new Coordinate { x = coordinate.x, y = coordinate.y - n };
                                break;
                            case ShipDirection.South:
                                shipCoordinates[n] = new Coordinate { x = coordinate.x, y = coordinate.y + n };
                                break;
                            case ShipDirection.East:
                                shipCoordinates[n] = new Coordinate { x = coordinate.x + n, y = coordinate.y };
                                break;
                            case ShipDirection.West:
                                shipCoordinates[n] = new Coordinate { x = coordinate.x - n, y = coordinate.y };
                                break;
                        }
                    }

                    success = (ReturnValue)_computerBoard.PlaceShip(type, shipCoordinates);


                } while (!(success == 0));
            }
        }
    }
}
