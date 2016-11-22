/*
    This class handles the game logic
*/

namespace Battleship
{
    public class Game
    {
        // Fields
        private bool _isSetupMode, _isWatchGame;
        private Direction _directionSettingUp;
        private Player player1, player2;
        private PlayerType _activePlayer;
        private ShipType _shipSettingUp;

        /*
            Constructor
            Accepts whether the game is human vs. AI or AI vs. AI; that is,
            whether it's a watch game or not
        */

        public Game(bool watchGame = false)
        {
            _isSetupMode = (!watchGame) ? true : false;
            _isWatchGame = watchGame;
            _directionSettingUp = Direction.North;
            player1 = new Player();
            player2 = new Player(player1);
            player1.SetOpponent(player2);
            _activePlayer = PlayerType.Player1;
            _shipSettingUp = ShipType.AircraftCarrier;

            player2.SetupBoard();

            if (watchGame)
                player1.SetupBoard();
        }

        /*
            The ChangeShipDirection method changes the direction in which
            to setup a ship
        */

        public void ChangeShipDirection()
        {
            if (_directionSettingUp != Direction.West)
                _directionSettingUp++;
            else
                _directionSettingUp = Direction.North;
        }

        /*
            The GetShipCoords method returns an array of the coordinates
            that the specified ship occupies on the specified player's board
        */

        public Coordinate[] GetShipCoords(PlayerType player, ShipType type)
        {
            Coordinate[] shipCoords;
            if (player == PlayerType.Player1)
                shipCoords = player1.TellShipCoords(type);
            else
                shipCoords = player2.TellShipCoords(type);
            return shipCoords;
        }

        /*
            The GetUnoccupiedCoords method returns an array of coordinates that
            are unoccupied on player 1's board
        */

        public Coordinate[] GetUnoccupiedCoords()
        {
            return player1.TellUnoccupiedCoords();
        }

        /*
            The GetWinner method returns the winner of the game, who will be
            the one player that still has ships remaining
        */

        public PlayerType GetWinner()
        {
            PlayerType winner = PlayerType.Player1;
            if (player1.IsDefeated())
                winner = PlayerType.Player2;
            return winner;
        }

        /*
            The IsOver method returns whether a player has been defeated
        */

        public bool IsOver()
        {
            bool gameOver = false;
            if (player1.IsDefeated() || player2.IsDefeated())
                gameOver = true;
            return gameOver;
        }

        /*
            The IsSetupOK method returns whether a ship can be setup
            with the given coordinates on player 1's board
        */

        public bool IsSetupOK(Coordinate[] coords)
        {
            return player1.TellIfSetupOK(coords);
        }

        /*
            The IsValidShot method returns whether the specified
            coordinate can be shot on player 2's board
        */

        public bool IsValidShot(Coordinate coord)
        {
            return player2.TellIfShotOK(coord);
        }

        /*
            The PlaceShip method places the current ship being setup on
            player 1's board with the given coordinates
        */

        public void PlaceShip(Coordinate[] coords)
        {
            player1.PlaceShip(_shipSettingUp, coords);
            if (_shipSettingUp != ShipType.PatrolBoat)
                _shipSettingUp++;
            else
                _isSetupMode = false;
        }

        /*
            The TakeTurn method has the active player take its turn
            The method returns the shot made
        */

        public Shot TakeTurn()
        {
            Shot shot;
            if (_activePlayer == PlayerType.Player1)
            {
                shot = player1.MakeShot();
                shot.Shooter = _activePlayer;
                _activePlayer = PlayerType.Player2;
            }
            else
            {
                shot = player2.MakeShot();
                shot.Shooter = _activePlayer;
                _activePlayer = PlayerType.Player1;
            }
            return shot;
        }

        /*
            The TakeTurn method has the active player take its turn,
            shooting at the specified coordinate
            The method returns the shot made
        */

        public Shot TakeTurn(Coordinate coord)
        {
            Shot shot;
            if (_activePlayer == PlayerType.Player1)
            {
                shot = player1.MakeShot(coord);
                shot.Shooter = _activePlayer;
                _activePlayer = PlayerType.Player2;
            }
            else
            {
                shot = player2.MakeShot(coord);
                shot.Shooter = _activePlayer;
                _activePlayer = PlayerType.Player1;
            }
            return shot;
        }

        /*
            IsSetupMode Property
        */

        public bool IsSetupMode
        {
            get { return _isSetupMode; }
        }

        /*
            IsWatchGame Property
        */

        public bool IsWatchGame
        {
            get { return _isWatchGame; }
        }

        /*
            DirectionSettingUp Property
        */

        public Direction DirectionSettingUp
        {
            get { return _directionSettingUp; }
        }

        /*
            ActivePlayer Property
        */

        public PlayerType ActivePlayer
        {
            get { return _activePlayer; }
        }

        /*
            ShipSettingUp Property
        */

        public ShipType ShipSettingUp
        {
            get { return _shipSettingUp; }
        }
    }
}
