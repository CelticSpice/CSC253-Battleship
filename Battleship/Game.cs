/*
    This class handles the game logic
*/

using System.Threading;

namespace Battleship
{
    public class Game
    {
        // Fields
        private bool _isSetupMode, _isWatchGame;
        private Direction _directionSettingUp;
        private int _numShotsRemaining;
        private Player _player1, _player2;
        private ShipType _shipSettingUp;
        private ShotMode _shotMode;

        /*
            Constructor - Accepts whether the game is human vs. AI or AI vs. AI,
            and the shot mode
        */

        public Game(bool watchGame, ShotMode mode)
        {
            _isSetupMode = (!watchGame) ? true : false;
            _isWatchGame = watchGame;
            _directionSettingUp = Direction.North;
            _numShotsRemaining = 0;
            _player1 = new Player();
            _player2 = new Player(_player1);
            _player1.SetOpponent(_player2);
            _shipSettingUp = ShipType.AircraftCarrier;
            _shotMode = mode;

            _player2.SetupBoard();

            if (watchGame)
            {
                Thread.Sleep(1500);
                _player1.SetupBoard();
            }
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
            GetShipCoords - Returns coordinates that a type of
            ship occupies on a player's board
        */

        public Coordinate[] GetShipCoords(Player player, ShipType shipType)
        {
            Coordinate[] shipCoords;
            if (player == _player1)
                shipCoords = _player1.TellShipCoords(shipType);
            else
                shipCoords = _player2.TellShipCoords(shipType);
            return shipCoords;
        }

        /*
            GetUnoccupiedCoords - Returns unoccupied coordinates on
            player 1's board
        */

        public Coordinate[] GetUnoccupiedCoords()
        {
            return _player1.TellUnoccupiedCoords();
        }

        /*
            GetWinner - Returns the winner of the game
        */

        public Player GetWinner()
        {
            Player winner = null;
            if (_player1.IsDefeated())
                winner = _player2;
            if (_player2.IsDefeated())
                winner = _player1;
            return winner;
        }

        /*
            IsOver - Returns whether a player has been defeated
        */

        public bool IsOver()
        {
            return _player1.IsDefeated() || _player2.IsDefeated();
        }

        /*
            IsSetupOK - Returns whether a ship can be placed on
            player 1's board with the given coordinates
        */

        public bool IsSetupOK(Coordinate[] coords)
        {
            return _player1.TellIfSetupOK(coords);
        }

        /*
            IsValidShot - Returns whether a shot can be made
            at a coordinate on player 2's board
        */

        public bool IsValidShot(Coordinate coord)
        {
            return _player2.TellIfShotOK(coord);
        }

        /*
            PlaceShip - Places ship currently being setup on
            player 1's board
        */

        public void PlaceShip(Coordinate[] coords)
        {
            _player1.PlaceShip(_shipSettingUp, coords);
            if (_shipSettingUp != ShipType.PatrolBoat)
                _shipSettingUp++;
            else
                _isSetupMode = false;
        }

        /*
            DoTurn - Has player take a turn and returns
            the shot the player made
            Accepts the player to take turn
        */

        public Shot DoTurn(Player player)
        {
            if (_shotMode == ShotMode.Salvo)
                _numShotsRemaining = player.TellNumShipsLiving();

            Shot shot;
            if (player == _player1)
                shot = _player1.MakeShot();
            else
                shot = _player2.MakeShot();
            return shot;
        }

        /*
            DoTurn - Has player 1 take a turn and returns the shot
            the player made at the specified coordinate
        */

        public Shot DoTurn(Coordinate coord)
        {
            return _player1.MakeShot(coord);
        }

        /*
            DoSalvoTurn - Has player take a turn and returns
            the shots the player made
            Accepts the player to take turn
        */

        public Shot[] DoSalvoTurn(Player player)
        {
            Shot[] shots;
            if (player == _player1)
            {
                shots = new Shot[_player1.TellNumShipsLiving()];
                for (int i = 0; i < shots.Length; i++)
                    shots[i] = _player1.MakeShot();
            }
            else
            {
                shots = new Shot[_player2.TellNumShipsLiving()];
                for (int i = 0; i < shots.Length; i++)
                    shots[i] = _player2.MakeShot();
            }
            return shots;
        }

        /*
            DoSalvoTurn - Has player 1 take a turn and returns
            the shot the player made at a specified coordinate
        */

        public Shot DoSalvoTurn(Coordinate coord)
        {
            if (_numShotsRemaining == 0)
                _numShotsRemaining = _player1.TellNumShipsLiving();

            _numShotsRemaining--;
            return _player1.MakeShot(coord);
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
            NumShotsRemaining Property
        */

        public int NumShotsRemaining
        {
            get { return _numShotsRemaining; }
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
        }

        /*
            ShotMode Property
        */

        public ShotMode ShotMode
        {
            get { return _shotMode; }
        }
    }
}
