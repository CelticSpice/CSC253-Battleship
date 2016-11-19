/*
    This form provides the main interface between
    the user and the Battleship game
*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Battleship
{
    public partial class MainForm : Form
    {
        // Fields
        private Color[] shipColors = {
            Color.Orange,
            Color.Green,
            Color.Blue,
            Color.Purple,
            Color.Yellow
        };

        private Label[,] lblSet1, lblSet2;
        private Game game;

        /*
            Constructor
        */

        public MainForm()
        {
            InitializeComponent();

            // Get whether we will play or watch a game
            DialogResult res = ModeDialogForm.ShowModeDialog();
            if (res == DialogResult.Yes)
            {
                game = new Game();
                SetupGrids();
            }
            else
            {
                game = new Game(true);
                SetupGrids(true);
            }

            CenterToScreen();
        }

        /*
            The BeginAIGame method begins a game between
            the AI and runs for as long as the game
            is not over
        */

        private void BeginAIGame()
        {
            AIGameWorker.RunWorkerAsync();
        }

        /*
            The GetCoordinate method returns the coordinate of the
            specified label representing a tile
        */

        private Coordinate GetCoordinate(Label control)
        {
            int numRows = lblSet1.GetLength(0);
            int numCols = lblSet1.GetLength(1);
            bool found = false;
            Coordinate coord = new Coordinate();
            for (int row = 0; row < numRows && !found; row++)
                for (int col = 0; col < numCols && !found; col++)
                    if (lblSet1[row, col] == control ||
                        lblSet2[row, col] == control)
                    {
                        found = true;
                        coord.x = col;
                        coord.y = row;
                    }
            return coord;
        }

        /*
            The GetShipPlacementCoords method gets the coordinates of where the
            ship currently being setup is
        */

        private Coordinate[] GetShipPlacementCoords(Label origin)
        {
            // Coordinate of origin
            Coordinate originCoord = GetCoordinate(origin);

            // Determine number of parts the ship has
            int numParts = 0;
            switch (game.ShipSettingUp)
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

            // Prepare array of ship's coordinates
            Coordinate[] shipCoords = new Coordinate[numParts];

            // Get coordinates in the current direction
            for (int i = 0; i < shipCoords.Length; i++)
                switch (game.DirectionSettingUp)
                {
                    case Direction.North:
                        shipCoords[i] = new Coordinate {
                            x = originCoord.x,
                            y = originCoord.y - i
                        };
                        break;
                    case Direction.South:
                        shipCoords[i] = new Coordinate {
                            x = originCoord.x,
                            y = originCoord.y + i
                        };
                        break;
                    case Direction.East:
                        shipCoords[i] = new Coordinate {
                            x = originCoord.x + i,
                            y = originCoord.y
                        };
                        break;
                    case Direction.West:
                        shipCoords[i] = new Coordinate {
                            x = originCoord.x - i,
                            y = originCoord.y
                        };
                        break;
                }

            return shipCoords;
        }

        /*
            The IsCoordInRange method checks that a coordinate
            points to a valid index in the 2D arrays of labels
        */

        private bool IsCoordInRange(Coordinate coord)
        {
            bool isInRange = false;
            if (coord.x >= 0 && coord.x < lblSet1.GetLength(0) &&
                coord.y >= 0 && coord.y < lblSet1.GetLength(1))
            {
                isInRange = true;
            }
            return isInRange;
        }

        /*
            The SetLabelColors method sets the colors of both
            sets of labels to reflect complete placement of ships

            This method should not be called to update colors
            of labels when manually placing ships - it is
            only to display ship locations in an AI game
        */

        private void SetLabelColors()
        {
            for (ShipType type = ShipType.AircraftCarrier;
                 type <= ShipType.PatrolBoat; type++)
            {
                foreach (Coordinate coord in game.GetShipCoords(
                    PlayerType.Player1, type))
                {
                    lblSet1[coord.y, coord.x].BackColor = shipColors[(int)type];
                }

                foreach (Coordinate coord in game.GetShipCoords(
                    PlayerType.Player2, type))
                {
                    lblSet2[coord.y, coord.x].BackColor = shipColors[(int)type];
                }
            }
        }

        /*
            The SetupGrids method sets up the grids
            representing each player's board
            It accepts whether the game is being
            played or being watched
        */

        private void SetupGrids(bool watch = false)
        {
            // Variables
            string lblSet1Name, lblSet2Name;
            if (!watch)
            {
                lblSet1Name = "trackingGridLbl";
                lblSet2Name = "shootingGridLbl";
            }
            else
            {
                lblSet1GroupBox.Text = "Player 1";
                lblSet2GroupBox.Text = "Player 2";
                lblSet1Name = "player1Lbl";
                lblSet2Name = "player2Lbl";
            }

            string nameSuffix = "0_A";

            char xLbl = '0',
                 yLbl = 'A';

            Point location = new Point(30, 41);
            Size size = new Size(25, 25);
            Color color = Color.CornflowerBlue;

            // Create labels
            const int NUM_ROWS = 10;
            const int NUM_COLUMNS = 10;
            lblSet1 = new Label[NUM_ROWS, NUM_COLUMNS];
            lblSet2 = new Label[NUM_ROWS, NUM_COLUMNS];
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLUMNS; col++)
                {
                    // Create labels
                    Label label = new Label();
                    label.Location = location;
                    label.Size = size;
                    label.Name = lblSet1Name + nameSuffix;
                    label.BackColor = color;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    lblSet1GroupBox.Controls.Add(label);
                    lblSet1[row, col] = label;


                    label = new Label();
                    label.Location = location;
                    label.Size = size;
                    label.Name = lblSet2Name + nameSuffix;
                    label.BackColor = color;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    lblSet2GroupBox.Controls.Add(label);
                    lblSet2[row, col] = label;

                    // To next xCoord
                    xLbl++;
                    location.X += 24;

                    nameSuffix = xLbl.ToString() + "_" + yLbl.ToString();
                }

                // To next yCoord
                xLbl = '0';
                yLbl++;
                location.X = 30;
                location.Y += 22;

                nameSuffix = xLbl.ToString() + "_" + yLbl.ToString();
            }
        }

        /*
            Click handler for Label Set 2 labels
        */

        private void LabelSet2_Click(object sender, EventArgs e)
        {
            // Get the coordinate of clicked label
            Coordinate guess = GetCoordinate((Label)sender);

            // Check that the guess can be made
            if (game.IsValidGuess(guess))
            {
                // Submit guess and get result
                GuessResult result = game.SubmitGuess(
                    PlayerType.Player1, guess);

                if (result == GuessResult.Miss)
                {
                    // Set color of label
                    lblSet2[guess.y, guess.x].BackColor = Color.FloralWhite;

                    // Display message
                    commentLbl.Text = "You missed!";
                }
                else
                {
                    // Set color of label
                    lblSet2[guess.y, guess.x].BackColor = Color.Red;

                    // Get the ship that was hit or sunk
                    ShipType t = game.GetShipHit(
                        PlayerType.Player2, guess);

                    // Display appropriate message
                    if (result == GuessResult.Hit)
                        commentLbl.Text = "You hit my " + t.ToString() + "!";
                    else
                        commentLbl.Text = "You sunk my " + t.ToString() + "!";
                }

                // Check if the game is over
                if (!game.IsOver())
                {
                    // Player 2 takes turn
                    guess = game.MakeGuess(PlayerType.Player2);

                    // Submit guess and get result
                    result = game.SubmitGuess(PlayerType.Player2, guess);

                    if (result == GuessResult.Miss)
                    {
                        // Set color of label
                        lblSet1[guess.y, guess.x].BackColor = Color.FloralWhite;

                        // Display message
                        commentLbl.Text = "Your opponent missed!";
                    }
                    else
                    {
                        // Set color of label
                        lblSet1[guess.y, guess.x].BackColor = Color.Red;

                        // Get the ship that was hit or sunk
                        ShipType t = game.GetShipHit(
                            PlayerType.Player1, guess);

                        // Display appropriate message
                        if (result == GuessResult.Hit)
                            commentLbl.Text = "Your opponent hit your " +
                                t.ToString() + "!";
                        else
                            commentLbl.Text = "Your opponent sunk your " +
                                t.ToString() + "!";
                    }
                }

                // Check if the game is over
                if (game.IsOver())
                {
                    // Remove handler from labels
                    foreach (Label label in lblSet2)
                        label.Click -= LabelSet2_Click;

                    // Declare winner
                    if (game.GetWinner() == PlayerType.Player1)
                        commentLbl.Text = "You won the game!";
                    else
                        commentLbl.Text = "The AI won! You lost!";
                }
            }
        }

        /*
            Click handler for Label Set 1 labels
        */

        private void LabelSet1_Click(object sender, EventArgs e)
        {
            // Place ship currently being setup in the selected location
            Coordinate[] shipCoords = GetShipPlacementCoords(((Label)sender));
            if (game.IsSetupOK(shipCoords))
            {
                game.PlaceShip(shipCoords);

                // Prepare to setup next ship, if possible
                if (game.IsSetupMode)
                    commentLbl.Text = "Construct your " +
                        game.ShipSettingUp.ToString();
                else
                {
                    // Remove handlers from Label Set 1 labels
                    foreach (Label label in lblSet1)
                    {
                        label.Click -= LabelSet1_Click;
                        label.MouseEnter -= LabelSet1_MouseEnter;
                        label.MouseLeave -= LabelSet1_MouseLeave;
                    }

                    // Add handler to Label Set 2 labels
                    foreach (Label label in lblSet2)
                        label.Click += LabelSet2_Click;

                    // Inform ready
                    commentLbl.Text = "Select a tile on your shooting grid";
                }
            }
        }

        /*
            Handler for quitButton
        */

        private void quitButton_Click(object sender, EventArgs e)
        {
            // Exit
            DialogResult result = MessageBox.Show(
                    "Are you sure you want to quit?", "Quit",
                    MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                Close();
        }

        /*
            Mouse enter handler for Label Set 1 labels
        */

        private void LabelSet1_MouseEnter(object sender, EventArgs e)
        {
            // Get ship coords
            Coordinate[] shipCoords = GetShipPlacementCoords((Label)sender);

            // Set color of labels appropriately
            if (game.IsSetupOK(shipCoords))
                foreach (Coordinate coord in shipCoords)
                    lblSet1[coord.y, coord.x].BackColor =
                        shipColors[(int)game.ShipSettingUp];
            else
                foreach (Coordinate coord in shipCoords)
                    if (IsCoordInRange(coord))
                        lblSet1[coord.y, coord.x].BackColor =
                            Color.Red;
        }

        /*
            Mouse leave handler for Label Set 1 labels
        */

        private void LabelSet1_MouseLeave(object sender, EventArgs e)
        {
            // Reset color of unoccupied tiles
            foreach (Coordinate coord in game.GetUnoccupiedCoords())
                lblSet1[coord.y, coord.x].BackColor =
                    Color.CornflowerBlue;

            // Reset color of occupied tiles
            for (ShipType type = ShipType.AircraftCarrier;
                 type < game.ShipSettingUp; type++)
                foreach (Coordinate coord in game.GetShipCoords(
                    PlayerType.Player1, type))
                {
                    lblSet1[coord.y, coord.x].BackColor = shipColors[(int)type];
                }
        }

        /*
            Load event handler for MainForm
        */

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!game.IsWatchGame)
            {
                foreach (Label label in lblSet1)
                {
                    label.Click += LabelSet1_Click;
                    label.MouseEnter += LabelSet1_MouseEnter;
                    label.MouseLeave += LabelSet1_MouseLeave;
                }

                commentLbl.Text = "Construct your " +
                    game.ShipSettingUp.ToString();
                KeyPress += MainFormSetup_KeyPress;
                KeyPreview = true;
            }
            else
            {
                SetLabelColors();
                BeginAIGame();
            }
        }

        /*
            Key press handler for 'R' key when user wants to rotate direction of
            ship being setup
        */

        private void MainFormSetup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'R' || e.KeyChar == 'r')
                game.ChangeShipDirection();
        }

        /*
            DoWork handler for updateGuiWorker
        */

        private void AIGameWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                // Player 1 takes turn
                AIGameWorker.ReportProgress(0, "Player 1 is thinking...");
                Thread.Sleep(900);

                Coordinate guess = game.MakeGuess(PlayerType.Player1);
                GuessResult res = game.SubmitGuess(PlayerType.Player1,
                    guess);
                GuessInfo info = new GuessInfo();
                info.coord = guess;
                info.result = res;
                info.player = PlayerType.Player1;
                if (res != GuessResult.Miss)
                    info.type = game.GetShipHit(PlayerType.Player2, guess);
                AIGameWorker.ReportProgress(0, info);
                Thread.Sleep(900);

                // Check if game is over
                if (!game.IsOver())
                {
                    // Player 2 takes turn
                    AIGameWorker.ReportProgress(0, "Player 2 is thinking...");
                    Thread.Sleep(900);

                    guess = game.MakeGuess(PlayerType.Player2);
                    res = game.SubmitGuess(PlayerType.Player2, guess);
                    info.coord = guess;
                    info.result = res;
                    info.player = PlayerType.Player2;
                    if (res != GuessResult.Miss)
                        info.type = game.GetShipHit(PlayerType.Player1, guess);
                    AIGameWorker.ReportProgress(0, info);
                    Thread.Sleep(900);
                }

                // Check if game is over
                if (game.IsOver())
                {
                    // Declare winner
                    if (game.GetWinner() == PlayerType.Player1)
                        AIGameWorker.ReportProgress(0, "Player 1 has won!");
                    else
                        AIGameWorker.ReportProgress(0, "Player 2 has won!");
                }
                else
                    Thread.Sleep(900);
            } while (!game.IsOver());
        }

        /*
            ProgressChanged handler for updateGuiWorker
        */

        private void AIGameWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is GuessInfo)
            {
                GuessInfo info = ((GuessInfo)e.UserState);

                // Check if hit
                if (info.result == GuessResult.Miss)
                {
                    if (info.player == PlayerType.Player1)
                    {
                        lblSet2[info.coord.y, info.coord.x].BackColor = Color.FloralWhite;
                        commentLbl.Text = "Player 1 missed!";
                    }
                    else
                    {
                        lblSet1[info.coord.y, info.coord.x].BackColor = Color.FloralWhite;
                        commentLbl.Text = "Player 2 missed!";
                    }
                }
                else
                {
                    if (info.player == PlayerType.Player1)
                    {
                        lblSet2[info.coord.y, info.coord.x].BackColor = Color.Red;

                        // Get if hit or sunk
                        if (info.result == GuessResult.Hit)
                            commentLbl.Text = "Player 1 hit Player 2's " +
                                info.type.ToString() + "!";
                        else
                            commentLbl.Text = "Player 1 sunk Player 2's " +
                               info.type.ToString() + "!";
                    }
                    else
                    {
                        lblSet1[info.coord.y, info.coord.x].BackColor = Color.Red;

                        // Get if hit or sunk
                        if (info.result == GuessResult.Hit)
                            commentLbl.Text = "Player 2 hit Player 1's " +
                                info.type.ToString() + "!";
                        else
                            commentLbl.Text = "Player 2 sunk Player 1's " +
                               info.type.ToString() + "!";
                    }
                }
            }
            else
                commentLbl.Text = ((string)e.UserState);
        }
    }
}
