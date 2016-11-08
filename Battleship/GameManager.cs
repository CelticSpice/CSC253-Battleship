/*
    This class handles the game logic
*/

namespace Battleship
{
    public class GameManager
    {
        // Fields
        private Player _player1, _player2;
        private bool _isSetupMode;
        private ShipType _shipSettingUp;


        /*
            No-Arg Constructor
        */

        public GameManager()
        {
            _player1 = new Player();
            _player2 = new Player(_player1, true);
            _player1.SetOpponent(_player2);
            _isSetupMode = true;
            _shipSettingUp = ShipType.AircraftCarrier;
        }

        /*
            The PrepareBattlePhase method prepares the battle phase
            of the game
        */

        public void PrepareBattlePhase()
        {
            _isSetupMode = false;
            _player2.SetupBoard();
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
