/*
    This class handles the game logic
*/

namespace Battleship
{
    public class GameManager
    {
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
    }
}
