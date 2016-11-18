/*
    This class handles the game logic
*/

namespace Battleship
{
    public class Game
    {
        // Fields
        private Player _player1, _player2;
        private bool _isSetupMode;
        private ShipType _shipSettingUp;

        /*
            No-Arg Constructor
        */

        public Game()
        {
            _player1 = new Player();
            _player2 = new Player(_player1);
            _isSetupMode = true;
            _shipSettingUp = ShipType.AircraftCarrier;
        }

        /*
            Player1 Property
        */

        public Player Player1
        {
            get { return _player1; }
        }

        /*
            Player2 Property
        */

        public Player Player2
        {
            get { return _player2; }
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
            ShipSettingUp Property
        */

        public ShipType ShipSettingUp
        {
            get { return _shipSettingUp; }
            set { _shipSettingUp = value; }
        }
    }
}
