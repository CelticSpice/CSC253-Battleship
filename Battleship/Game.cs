/*
    This class handles the game logic
*/

namespace Battleship
{
    public class Game
    {
        // Fields
        private Player _player1, _player2;
        private ShipType _shipSettingUp;

        /*
            Constructor
            Accepts whether the game is human vs. AI or AI vs. AI; that is,
            whether it's a watch game or not
        */

        public Game(bool watchGame = false)
        {
            _player1 = new Player();
            _player2 = new Player(_player1);
            _player1.Opponent = _player2;
            _shipSettingUp = ShipType.AircraftCarrier;

            _player2.SetupBoard();

            if (watchGame)
                _player1.SetupBoard();
        }

        /*
            The IsSetupOK method returns whether a ship can be setup
            with the given coordinates
        */

        public bool IsSetupOK(Coordinate[] coords)
        {
            return _player1.GetIfSetupOK(coords);
        }

        /*
            The PlaceShip method places the current ship being setup on
            player 1's board with the given coordinates
        */

        public void PlaceShip(Coordinate[] coords)
        {
            _player1.PlaceShip(_shipSettingUp, coords);
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
            ShipSettingUp Property
        */

        public ShipType ShipSettingUp
        {
            get { return _shipSettingUp; }
            set { _shipSettingUp = value; }
        }
    }
}
