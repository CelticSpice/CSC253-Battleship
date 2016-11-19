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
        private ShipType _shipSettingUp;

        /*
            Constructor
            Accepts whether the game is human vs. AI or AI vs. AI; that is,
            whether it's a watch game or not
        */

        public Game(bool watchGame = false)
        {
            _isSetupMode = true;
            _isWatchGame = watchGame;
            _directionSettingUp = Direction.North;
            player1 = new Player();
            player2 = new Player(player1);
            player1.SetOpponent(player2);
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
            The GetShipHit method returns the ship that was hit at the specified
            coordinate on the specified player's board
        */

        public ShipType GetShipHit(PlayerType player, Coordinate coord)
        {
            ShipType type = ShipType.AircraftCarrier;
            if (player == PlayerType.Player1)
                type = player1.TellShipHit(coord);
            else
                type = player2.TellShipHit(coord);
            return type;
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
            if (player2.TellNumShipsLiving() != 0)
                winner = PlayerType.Player2;
            return winner;
        }

        /*
            The IsOver method returns whether a player's ships have all
            been sunk, and thus the game is over
        */

        public bool IsOver()
        {
            bool gameOver = false;
            if (player1.TellNumShipsLiving() == 0 ||
                player2.TellNumShipsLiving() == 0)
            {
                gameOver = true;
            }
            return gameOver;
        }

        /*
            The IsSetupOK method returns whether a ship can be setup
            with the given coordinates on player 1's board
        */

        public bool IsSetupOK(Coordinate[] coords)
        {
            return player1.GetIfSetupOK(coords);
        }

        /*
            The IsValidGuess method returns whether the specified coordinate
            can be guessed on player 2's board
        */

        public bool IsValidGuess(Coordinate guess)
        {
            return player2.TellIfGuessOK(guess);
        }

        /*
            The MakeGuess method has the specified player make a guess
            of a coordinate to shoot at
        */

        public Coordinate MakeGuess(PlayerType player)
        {
            Coordinate guess = new Coordinate { x = -1, y = -1 };
            if (player == PlayerType.Player1)
                guess = player1.MakeGuess();
            else
                guess = player2.MakeGuess();
            return guess;
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
            The SubmitGuess method has the specified player submit a
            guess to its opponent
            The method returns the results of the guess
        */

        public GuessResult SubmitGuess(PlayerType player, Coordinate guess)
        {
            GuessResult result = GuessResult.Miss;
            if (player == PlayerType.Player1)
                result = player2.InformOfGuess(guess);
            else
                result = player1.InformOfGuess(guess);
            return result;
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
            ShipSettingUp Property
        */

        public ShipType ShipSettingUp
        {
            get { return _shipSettingUp; }
        }
    }
}
