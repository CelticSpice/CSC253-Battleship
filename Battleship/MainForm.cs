/*
   Form providing the main UI
   12/8/2016
   CSC 253 0001 - M6PROJ
   Author: James Alves, Shane McCann, Timothy Burns
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

        private Label[,] grid1, grid2;
        private Game game;

        /*
            Constructor
        */

        public MainForm()
        {
            InitializeComponent();

            // Get game & shooting mode
            ModeSelection selection = ModeDialogForm.Display();

            // Start game
            if (!selection.cancelClicked)
            {
                game = new Game(selection.watchGame, selection.shotMode);
                CenterToScreen();
            }
        }

        /*
            BeginAIGame - Begins an AI vs. AI game
        */

        private void BeginAIGame()
        {
            aiGameWorker.RunWorkerAsync();
        }

        /*
            GetCoordinate - Returns coordinate of a label
        */

        private Coordinate GetCoordinate(Label control)
        {
            int numRows = grid1.GetLength(0);
            int numCols = grid1.GetLength(1);
            bool found = false;
            int xCoord = -1;
            int yCoord = -1;
            for (int row = 0; row < numRows && !found; row++)
                for (int col = 0; col < numCols && !found; col++)
                    if (grid1[row, col] == control ||
                        grid2[row, col] == control)
                    {
                        found = true;
                        xCoord = col;
                        yCoord = row;
                    }
            return new Coordinate { x = xCoord, y = yCoord };
        }

        /*
            GetShipPlacementCoords - Returns coordinates of ship
            being setup
        */

        private Coordinate[] GetShipPlacementCoords(Label head)
        {
            // Get number of coordinates ship being setup
            // occupies
            int numCoords = 0;
            switch (game.ShipSettingUp)
            {
                case ShipType.Aircraft_Carrier:
                    numCoords = 5;
                    break;
                case ShipType.Battleship:
                    numCoords = 4;
                    break;
                case ShipType.Submarine:
                case ShipType.Destroyer:
                    numCoords = 3;
                    break;
                case ShipType.Patrol_Boat:
                    numCoords = 2;
                    break;
            }

            // Prepare array of ship's coordinates
            Coordinate[] shipCoords = new Coordinate[numCoords];
            shipCoords[0] = GetCoordinate(head);

            // Get coordinates in direction head is facing
            switch (game.DirectionSettingUp)
            {
                case Direction.North:
                    for (int i = 1; i < shipCoords.Length; i++)
                        shipCoords[i] = new Coordinate
                        {
                            x = shipCoords[0].x,
                            y = shipCoords[0].y - i
                        };
                    break;
                case Direction.South:
                    for (int i = 1; i < shipCoords.Length; i++)
                        shipCoords[i] = new Coordinate
                        {
                            x = shipCoords[0].x,
                            y = shipCoords[0].y + i
                        };
                    break;
                case Direction.East:
                    for (int i = 1; i < shipCoords.Length; i++)
                        shipCoords[i] = new Coordinate
                        {
                            x = shipCoords[0].x + i,
                            y = shipCoords[0].y
                        };
                    break;
                case Direction.West:
                    for (int i = 1; i < shipCoords.Length; i++)
                        shipCoords[i] = new Coordinate
                        {
                            x = shipCoords[0].x - i,
                            y = shipCoords[0].y
                        };
                    break;
            }

            return shipCoords;
        }

        /*
            IsCoordInRange - Returns whether a coordinate
            exists on a grid
        */

        private bool IsCoordInRange(Coordinate coord)
        {
            return (coord.x >= 0 && coord.x < grid1.GetLength(0) &&
                    coord.y >= 0 && coord.y < grid1.GetLength(1));
        }

        /*
            ReportShotResult - Reports and displays results of a shot
        */

        private void ReportShotResult(Shot shot)
        {
            Label[,] lblSet = (shot.Shooter == game.Player1) ? grid2 :
                                                               grid1;

            // Check if hit
            if (shot.Result == ShotResult.Miss)
            {
                lblSet[shot.Coord.y, shot.Coord.x].BackColor =
                    Color.FloralWhite;

                if (shot.Shooter == game.Player1)
                {
                    if (!game.IsWatchGame)
                        commentLbl.Text = "You missed me!";
                    else
                        commentLbl.Text = "Player 1 missed!";
                }

                if (shot.Shooter == game.Player2)
                    commentLbl.Text = "Player 2 missed!";
            }
            else
            {
                lblSet[shot.Coord.y, shot.Coord.x].BackColor = Color.Red;
                string shipHit = shot.ShipHit.ToString().Replace('_', ' ');


                // Get if hit or sunk
                if (shot.Result == ShotResult.Hit)
                {
                    if (shot.Shooter == game.Player1)
                    {
                        if (!game.IsWatchGame)
                            commentLbl.Text = "You hit my " +
                                shipHit + "!";
                        else
                            commentLbl.Text = "Player 1 hit Player 2's " +
                               shipHit + "!";
                    }
                    else
                    commentLbl.Text = "Player 2 hit Player 1's " +
                        shipHit + "!";
                }
                else
                {
                    if (shot.Shooter == game.Player1)
                    {
                        if (!game.IsWatchGame)
                            commentLbl.Text = "You sunk my " +
                                shipHit + "!";
                        else
                        commentLbl.Text = "Player 1 sunk Player 2's " +
                           shipHit + "!";
                    }
                    else
                        commentLbl.Text = "Player 2 sunk Player 1's " +
                            shipHit + "!";
                }
            }
        }

        /*
            SetWatchGameLblColors - Sets colors of labels for a game
            that is being watched
        */

        private void SetWatchGameLblColors()
        {
            for (ShipType type = ShipType.Aircraft_Carrier;
                 type <= ShipType.Patrol_Boat; type++)
            {
                foreach (Coordinate coord in game.GetShipCoords(
                    game.Player1, type))
                {
                    grid1[coord.y, coord.x].BackColor = shipColors[(int)type];
                }

                foreach (Coordinate coord in game.GetShipCoords(
                    game.Player2, type))
                {
                    grid2[coord.y, coord.x].BackColor = shipColors[(int)type];
                }
            }
        }

        /*
            SetupGrids- Builds the grids
        */

        private void SetupGrids()
        {
            // Variables
            Point location = new Point(30, 41);
            Size size = new Size(25, 25);
            Color color = Color.CornflowerBlue;

            // Change grid names if the game is being watched
            if (game.IsWatchGame)
            {
                grid1GrpBox.Text = "Player 1";
                grid2GrpBox.Text = "Player 2";
            }

            // Create labels
            int numRows = game.GetBoardSize();
            int numColumns = numRows;
            grid1 = new Label[numRows, numColumns];
            grid2 = new Label[numRows, numColumns];
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    // Grid 1
                    Label label = new Label();
                    label.Location = location;
                    label.Size = size;
                    label.BackColor = color;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    grid1GrpBox.Controls.Add(label);
                    grid1[row, col] = label;

                    // Grid 2
                    label = new Label();
                    label.Location = location;
                    label.Size = size;
                    label.BackColor = color;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    grid2GrpBox.Controls.Add(label);
                    grid2[row, col] = label;

                    // To next X coord
                    location.X += 24;
                }
                // To next Y coord
                location.X = 30;
                location.Y += 22;
            }
        }

        /*
            Click handler for Label Set 2 labels
        */

        private void Grid2Label_Click(object sender, EventArgs e)
        {
            // Get the coordinate of clicked label
            Coordinate coord = GetCoordinate((Label)sender);

            // Check that the shot can be made
            if (!humanGameWorker.IsBusy && game.IsValidShot(coord))
                // Do turn
                humanGameWorker.RunWorkerAsync(coord);
        }

        /*
            Click handler for Grid 1 labels
        */

        private void Grid1Label_Click(object sender, EventArgs e)
        {
            // Place ship currently being setup in the selected location
            Coordinate[] shipCoords = GetShipPlacementCoords(((Label)sender));
            if (game.IsSetupOK(shipCoords))
            {
                game.PlaceShip(shipCoords);

                // Prepare to setup next ship, if possible
                if (game.IsSetupMode)
                {
                    string shipSettingUp = game.ShipSettingUp.ToString().Replace('_', ' ');
                    commentLbl.Text = "Construct your " + shipSettingUp;
                }
                else
                {
                    // Hide key label
                    keyLbl.Visible = false;

                    // Remove handlers from Grid 1 labels
                    foreach (Label label in grid1)
                    {
                        label.Click -= Grid1Label_Click;
                        label.MouseEnter -= Grid1Label_MouseEnter;
                        label.MouseLeave -= Grid1Label_MouseLeave;
                    }

                    // Add handler to Grid 2 labels
                    foreach (Label label in grid2)
                        label.Click += Grid2Label_Click;

                    // Inform ready
                    commentLbl.Text = "Select a tile on your shooting grid!";
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
            Mouse enter handler for Grid 1 labels
        */

        private void Grid1Label_MouseEnter(object sender, EventArgs e)
        {
            // Get coordinates
            Coordinate[] shipCoords = GetShipPlacementCoords((Label)sender);

            // Set color of labels appropriately
            if (game.IsSetupOK(shipCoords))
                foreach (Coordinate coord in shipCoords)
                    grid1[coord.y, coord.x].BackColor =
                        shipColors[(int)game.ShipSettingUp];
            else
                foreach (Coordinate coord in shipCoords)
                    if (IsCoordInRange(coord))
                        grid1[coord.y, coord.x].BackColor =
                            Color.Red;
        }

        /*
            Mouse leave handler for Grid 1 labels
        */

        private void Grid1Label_MouseLeave(object sender, EventArgs e)
        {
            // Reset color of unoccupied tiles
            foreach (Coordinate coord in game.GetUnoccupiedCoords())
                grid1[coord.y, coord.x].BackColor =
                    Color.CornflowerBlue;

            // Reset color of occupied tiles
            for (ShipType type = ShipType.Aircraft_Carrier;
                 type < game.ShipSettingUp; type++)
            {
                foreach (Coordinate coord in game.GetShipCoords(game.Player1,
                    type))
                    {
                        grid1[coord.y, coord.x].BackColor =
                            shipColors[(int)type];
                    }
            }
        }

        /*
            Load event
        */

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (game != null)
            {
                SetupGrids();

                if (!game.IsWatchGame)
                {
                    foreach (Label label in grid1)
                    {
                        label.Click += Grid1Label_Click;
                        label.MouseEnter += Grid1Label_MouseEnter;
                        label.MouseLeave += Grid1Label_MouseLeave;
                    }

                    string shipSettingUp = game.ShipSettingUp.ToString().Replace('_', ' ');
                    commentLbl.Text = "Construct your " + shipSettingUp;
                    keyLbl.Visible = true;
                    KeyPress += MainFormSetup_KeyPress;
                    KeyPreview = true;
                }
                else
                {
                    SetWatchGameLblColors();
                    BeginAIGame();
                }
            }
            else
                Close();
        }

        /*
            Key Press handler for 'r' key
        */

        private void MainFormSetup_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Change direction setting up ship in
            if (e.KeyChar == 'R' || e.KeyChar == 'r')
                game.ChangeShipDirection();
        }

        /*
            DoWork handler for AIGameWorker
        */

        private void AIGameWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!game.IsOver())
            {
                // Player 1 takes turn
                do
                {
                    Shot shot = game.DoTurn(game.Player1);
                    aiGameWorker.ReportProgress(0, shot);
                    Thread.Sleep(1000);
                } while (game.NumShotsRemaining != 0 && !game.IsOver());

                // Check if game is over
                if (!game.IsOver())
                    // Player 2 takes turn
                    do
                    {
                        Shot shot = game.DoTurn(game.Player2);
                        aiGameWorker.ReportProgress(0, shot);
                        Thread.Sleep(1000);
                    } while (game.NumShotsRemaining != 0 && !game.IsOver());

                // Check if game is over
                if (game.IsOver())
                {
                    // Declare winner
                    if (game.GetWinner() == game.Player1)
                        aiGameWorker.ReportProgress(0, "Player 1 has won!");
                    else
                        aiGameWorker.ReportProgress(0, "Player 2 has won!");
                }
            }
        }

        /*
            ProgressChanged handler for AIGameWorker
        */

        private void AIGameWorker_ProgressChanged(object sender,
                                                  ProgressChangedEventArgs e)
        {
            if (e.UserState is Shot)
                ReportShotResult((Shot)e.UserState);
            else
                commentLbl.Text = ((string)e.UserState);
        }

        /*
            HumanGameWorker DoWork
        */

        private void HumanGameWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Player 1 takes turn
            Shot shot = game.DoTurn((Coordinate)e.Argument);
            humanGameWorker.ReportProgress(0, shot);
            Thread.Sleep(1000);

            // Check if the AI should take its turn
            if (!game.IsOver() && game.NumShotsRemaining == 0)
                do
                {
                    shot = game.DoTurn(game.Player2);
                    humanGameWorker.ReportProgress(0, shot);
                    Thread.Sleep(1000);
                } while (game.NumShotsRemaining != 0 && !game.IsOver());

            // Check if game is over
            if (game.IsOver())
            {
                // Remove handlers from labels
                foreach (Label label in grid2)
                    label.Click -= Grid2Label_Click;

                // Declare winner
                if (game.GetWinner() == game.Player1)
                    humanGameWorker.ReportProgress(0, "You won!");
                else
                    humanGameWorker.ReportProgress(0, "The AI won!");
            }
        }

        /*
           HumanGameWorker ProgressChanged
        */

        private void HumanGameWorker_ProgressChanged(object sender,
                                                ProgressChangedEventArgs e)
        {
            if (e.UserState is Shot)
                ReportShotResult(e.UserState as Shot);
            else
                commentLbl.Text = ((string)e.UserState);
        }
    }
}
