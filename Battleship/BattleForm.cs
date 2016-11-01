using System;
using System.Drawing;
using System.Windows.Forms;

namespace Battleship
{
    public partial class BattleForm : Form
    {
        // ShipDirection enum
        private enum ShipDirection { North, South, East, West }

        // Fields
        private Color[] shipColors = { Color.Orange, Color.Green, Color.Blue, Color.Purple, Color.Yellow };

        private GameManager manager;
        private ShipDirection direction;
        private Label[,] tileLabels;

        /*
            Constructor
        */

        public BattleForm()
        {
            InitializeComponent();
        }

        /*
            The GetCoordinate method returns the coordinate of the
            passed control
        */

        private Coordinate GetCoordinate(Control control)
        {
            string[] delim = { "trackingGridTile", "shootingGridBtn", "_" };
            string[] coordTokens = control.Name.Split(delim, StringSplitOptions.RemoveEmptyEntries);

            int asciiA = 65;
            int xCoord = int.Parse(coordTokens[0]);
            int yCoord = coordTokens[1][0] - asciiA;

            return new Coordinate { x = xCoord, y = yCoord };
        }

        /*
            The GetShipPlacementCoords method gets the coords of where the
            ship currently being setup is
        */

        private Coordinate[] GetShipPlacementCoords(Label origin)
        {
            // Coordinate of label mouse entered
            Coordinate coordinate = GetCoordinate(origin);

            // Determine number of parts and/or tiles the ship occupies
            int numParts = 0;
            switch (manager.ShipSettingUp)
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

            Coordinate[] shipCoordinates = new Coordinate[numParts];

            // Get coordinates in the direction of current ShipDirection
            for (int i = 0; i < shipCoordinates.Length; i++)
                switch (direction)
                {
                    case ShipDirection.North:
                        shipCoordinates[i] = new Coordinate { x = coordinate.x, y = coordinate.y - i };
                        break;
                    case ShipDirection.South:
                        shipCoordinates[i] = new Coordinate { x = coordinate.x, y = coordinate.y + i };
                        break;
                    case ShipDirection.East:
                        shipCoordinates[i] = new Coordinate { x = coordinate.x + i, y = coordinate.y };
                        break;
                    case ShipDirection.West:
                        shipCoordinates[i] = new Coordinate { x = coordinate.x - i, y = coordinate.y };
                        break;
                }

            return shipCoordinates;
        }
        
        /*
            The LoadGridTiles method loads the labels and buttons that
            represent tiles on the grids
        */

        private void LoadGridTiles()
        {
            // Variables
            string trackingGridName = "trackingGridTile",
                   shootingGridName = "shootingGridBtn",
                   nameSuffix = "0_A";

            char xLabel = '0',
                 yLabel = 'A';

            Point location = new Point(30, 41);
            Size size = new Size(25, 25);
            Color color = Color.CornflowerBlue;
            GroupBox trackingGrid = player1BoardGroupBox;
            GroupBox shootingGrid = shootingGridGroupBox;

            // Create labels and buttons
            const int NUM_ROWS = 10;
            const int NUM_COLUMNS = 10;
            tileLabels = new Label[NUM_ROWS, NUM_COLUMNS];
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLUMNS; col++)
                {
                    // Create label
                    Label label = new Label();
                    label.Location = location;
                    label.Size = size;
                    label.Name = trackingGridName + nameSuffix;
                    label.BackColor = color;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Click += TrackingGridLabel_Click;
                    label.MouseEnter += TrackingGridLabel_MouseEnter;
                    label.MouseLeave += TrackingGridLabel_MouseLeave;
                    trackingGrid.Controls.Add(label);
                    tileLabels[row, col] = label;

                    // Create button
                    Button button = new Button();
                    button.Location = location;
                    button.Size = size;
                    button.Name = shootingGridName + nameSuffix;
                    button.BackColor = color;
                    button.Click += ShootingGridButton_Click;
                    shootingGrid.Controls.Add(button);

                    // To next xCoord
                    xLabel++;
                    location.X += 24;

                    nameSuffix = xLabel.ToString() + "_" + yLabel.ToString();
                }

                // To next yCoord
                xLabel = '0';
                yLabel++;
                location.X = 30;
                location.Y += 22;

                nameSuffix = xLabel.ToString() + "_" + yLabel.ToString();
            }
        }

        /*
            Load event
        */

        private void BattleForm_Load(object sender, EventArgs e)
        {
            manager = new GameManager();
            LoadGridTiles();

            commentLabel.Text = "Construct your " + manager.ShipSettingUp.ToString() + "!";
            direction = ShipDirection.North;
            KeyPress += BattleFormSetup_KeyPress;
            KeyPreview = true;
        }

        /*
            Click handler for Shooting Grid button
        */

        private void ShootingGridButton_Click(object sender, EventArgs e)
        {
            if (!manager.IsSetupMode)
            {
                
            }
        }

        /*
            Click handler for Tracking Grid label
        */

        private void TrackingGridLabel_Click(object sender, EventArgs e)
        {
            // Place ship currently being setup in the selected location
            Coordinate[] shipCoordinates = GetShipPlacementCoords(((Label)sender));
            if (manager.PlayerBoard.IsShipPlacementOK(shipCoordinates))
            {
                manager.PlayerBoard.PlaceShip(manager.ShipSettingUp, shipCoordinates);

                // Prepare to setup next ship, if possible
                if (!(manager.ShipSettingUp == ShipType.PatrolBoat))
                {
                    manager.ShipSettingUp++;
                    commentLabel.Text = "Construct your " + manager.ShipSettingUp.ToString() + "!";
                }
                else
                {
                    manager.IsSetupMode = false;
                    commentLabel.Text = "";

                    // Remove appropriate handler from labels
                    foreach (Label label in tileLabels)
                    {
                        label.Click -= TrackingGridLabel_Click;
                        label.MouseEnter -= TrackingGridLabel_MouseEnter;
                        label.MouseLeave -= TrackingGridLabel_MouseLeave;
                    }
                }
            }
        }

        /*
            Handler for quitButton
        */

        private void quitButton_Click(object sender, EventArgs e)
        {
            // Exit
            DialogResult result = MessageBox.Show("Are you sure you want to quit?", "Quit", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                this.Close();
        }

        /*
            Mouse enter handler for Tracking Grid labels
        */

        private void TrackingGridLabel_MouseEnter(object sender, EventArgs e)
        {
            // Get ship coords
            Coordinate[] shipCoordinates = GetShipPlacementCoords((Label)sender);

            // Set color of labels appropriately
            if (manager.PlayerBoard.IsShipPlacementOK(shipCoordinates))
                foreach (Coordinate coord in shipCoordinates)
                    tileLabels[coord.y, coord.x].BackColor = shipColors[(int)manager.ShipSettingUp];
            else
                foreach (Coordinate coord in shipCoordinates)
                    if (coord.x >= 0 && coord.x <= 9 &&
                        coord.y >= 0 && coord.y <= 9)
                    {
                        tileLabels[coord.y, coord.x].BackColor = Color.Red;
                    }
        }

        /*
            Mouse leave handler for Tracking Grid labels
        */

        private void TrackingGridLabel_MouseLeave(object sender, EventArgs e)
        {
            // Reset color of unoccupied tiles
            Coordinate[] unoccupiedCoords = manager.PlayerBoard.GetUnoccupiedCoords();
            foreach (Coordinate coord in unoccupiedCoords)
                tileLabels[coord.y, coord.x].BackColor = Color.CornflowerBlue;

            // Reset color of occupied tiles
            for (ShipType type = ShipType.AircraftCarrier; type <= ShipType.PatrolBoat; type++)
                if (manager.PlayerBoard.IsShipExisting(type))
                {
                    Coordinate[] shipCoordinates = manager.PlayerBoard.Ships[(int)type].GetCoords();
                    foreach (Coordinate coord in shipCoordinates)
                        tileLabels[coord.y, coord.x].BackColor = shipColors[(int)type];
                }
        }

        /*
            Key press handler for 'R' key when user wants to rotate direction of
            ship being setup
        */

        private void BattleFormSetup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'R' || e.KeyChar == 'r')
                direction = (direction != ShipDirection.West) ? direction + 1 : ShipDirection.North;
        }
    }
}
