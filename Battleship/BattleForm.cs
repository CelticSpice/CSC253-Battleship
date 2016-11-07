using System;
using System.Drawing;
using System.Windows.Forms;

namespace Battleship
{
    public partial class BattleForm : Form
    {
        // Fields
        private Color[] shipColors = { Color.Orange, Color.Green, Color.Blue, Color.Purple, Color.Yellow };

        private Button[,] shootingGridBtns;
        private Label[,] trackingGridLbls;
        private GameManager manager;
        private ShipDirection direction;

        /*
            Constructor
        */

        public BattleForm()
        {
            InitializeComponent();
        }

        /*
            The GetCoordinate method returns the coordinate of the
            specified control representing a tile
        */

        private Coordinate GetCoordinate(Control control)
        {
            const int NUM_ROWS = 10;
            const int NUM_COLUMNS = 10;
            bool found = false;
            Coordinate coord = new Coordinate();
            for (int row = 0; row < NUM_ROWS && !found; row++)
                for (int col = 0; col < NUM_COLUMNS && !found; col++)
                    if (trackingGridLbls[row, col] == control)
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
            // Coordinate of label mouse entered
            Coordinate originCoord = GetCoordinate(origin);

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

            // Prepare array of ship's coordinates
            Coordinate[] shipCoords = new Coordinate[numParts];

            // Get coordinates in the direction of current ShipDirection
            for (int i = 0; i < shipCoords.Length; i++)
                switch (direction)
                {
                    case ShipDirection.North:
                        shipCoords[i] = new Coordinate { x = originCoord.x, y = originCoord.y - i };
                        break;
                    case ShipDirection.South:
                        shipCoords[i] = new Coordinate { x = originCoord.x, y = originCoord.y + i };
                        break;
                    case ShipDirection.East:
                        shipCoords[i] = new Coordinate { x = originCoord.x + i, y = originCoord.y };
                        break;
                    case ShipDirection.West:
                        shipCoords[i] = new Coordinate { x = originCoord.x - i, y = originCoord.y };
                        break;
                }

            return shipCoords;
        }
        
        /*
            The LoadGridTiles method loads the labels and buttons that
            represent tiles on the grids
        */

        private void LoadGridTiles()
        {
            // Variables
            string trackingGridName = "trackingGridLbl",
                   shootingGridName = "shootingGridBtn",
                   nameSuffix = "0_A";

            char xLbl = '0',
                 yLbl = 'A';

            Point location = new Point(30, 41);
            Size size = new Size(25, 25);
            Color color = Color.CornflowerBlue;
            GroupBox trackingGrid = trackingGridGroupBox;
            GroupBox shootingGrid = shootingGridGroupBox;

            // Create labels and buttons
            const int NUM_ROWS = 10;
            const int NUM_COLUMNS = 10;
            trackingGridLbls = new Label[NUM_ROWS, NUM_COLUMNS];
            shootingGridBtns = new Button[NUM_ROWS, NUM_COLUMNS];
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
                    trackingGridLbls[row, col] = label;

                    // Create button
                    Button button = new Button();
                    button.Location = location;
                    button.Size = size;
                    button.Name = shootingGridName + nameSuffix;
                    button.BackColor = color;
                    shootingGrid.Controls.Add(button);
                    shootingGridBtns[row, col] = button;

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
            Load event
        */

        private void BattleForm_Load(object sender, EventArgs e)
        {
            manager = new GameManager();
            LoadGridTiles();

            commentLbl.Text = "Construct your " + manager.ShipSettingUp.ToString() + "!";
            direction = ShipDirection.North;
            KeyPress += BattleFormSetup_KeyPress;
            KeyPreview = true;
        }

        /*
            Click handler for Shooting Grid button
        */

        private void ShootingGridButton_Click(object sender, EventArgs e)
        {
            
        }

        /*
            Click handler for Tracking Grid label
        */

        private void TrackingGridLabel_Click(object sender, EventArgs e)
        {
            // Place ship currently being setup in the selected location
            Coordinate[] shipCoords = GetShipPlacementCoords(((Label)sender));
            if (manager.Player1.Board.IsShipPlacementOK(shipCoords))
            {
                manager.Player1.Board.PlaceShip(manager.ShipSettingUp, shipCoords);

                // Prepare to setup next ship, if possible
                if (!(manager.ShipSettingUp == ShipType.PatrolBoat))
                {
                    manager.ShipSettingUp++;
                    commentLbl.Text = "Construct your " + manager.ShipSettingUp.ToString() + "!";
                }
                else
                {
                    // Remove handlers from labels
                    foreach (Label label in trackingGridLbls)
                    {
                        label.Click -= TrackingGridLabel_Click;
                        label.MouseEnter -= TrackingGridLabel_MouseEnter;
                        label.MouseLeave -= TrackingGridLabel_MouseLeave;
                    }

                    // Add handler to buttons
                    foreach (Button button in shootingGridBtns)
                        button.Click += ShootingGridButton_Click;

                    // Prepare battle phase
                    manager.PrepareBattlePhase();

                    // Inform ready
                    commentLbl.Text = "Select a tile on your shooting grid!":
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
                Close();
        }

        /*
            Mouse enter handler for Tracking Grid labels
        */

        private void TrackingGridLabel_MouseEnter(object sender, EventArgs e)
        {
            // Get ship coords
            Coordinate[] shipCoords = GetShipPlacementCoords((Label)sender);

            // Set color of labels appropriately
            if (manager.Player1.Board.IsShipPlacementOK(shipCoords))
                foreach (Coordinate coord in shipCoords)
                    trackingGridLbls[coord.y, coord.x].BackColor = shipColors[(int)manager.ShipSettingUp];
            else
                foreach (Coordinate coord in shipCoords)
                    if (coord.x >= 0 && coord.x <= 9 &&
                        coord.y >= 0 && coord.y <= 9)
                    {
                        trackingGridLbls[coord.y, coord.x].BackColor = Color.Red;
                    }
        }

        /*
            Mouse leave handler for Tracking Grid labels
        */

        private void TrackingGridLabel_MouseLeave(object sender, EventArgs e)
        {
            // Reset color of unoccupied tiles
            Coordinate[] unoccupiedCoords = manager.Player1.Board.GetUnoccupiedCoords();
            foreach (Coordinate coord in unoccupiedCoords)
                trackingGridLbls[coord.y, coord.x].BackColor = Color.CornflowerBlue;

            // Reset color of occupied tiles
            for (ShipType type = ShipType.AircraftCarrier; type <= ShipType.PatrolBoat; type++)
                if (manager.Player1.Board.IsShipExisting(type))
                {
                    Coordinate[] shipCoords = manager.Player1.Board.Ships[(int)type].GetCoords();
                    foreach (Coordinate coord in shipCoords)
                        trackingGridLbls[coord.y, coord.x].BackColor = shipColors[(int)type];
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
