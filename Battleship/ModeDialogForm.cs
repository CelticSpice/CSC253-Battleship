/*
    This form enables the user to choose game
    and shooting modes
*/

using System;
using System.Windows.Forms;

namespace Battleship
{
    public partial class ModeDialogForm : Form
    {
        // Fields
        private ModeSelection _selection;

        /*
            No-Arg Constructor
        */

        public ModeDialogForm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        /*
            Handler for playButton
        */

        private void playButton_Click(object sender, EventArgs e)
        {
            ShotMode mode = ShotMode.Normal;
            if (salvoShootRadBtn.Checked)
                mode = ShotMode.Salvo;
            _selection.watchGame = false;
            _selection.shotMode = mode;
            Close();
        }

        /*
            Handler for watchButton
        */

        private void watchButton_Click(object sender, EventArgs e)
        {
            ShotMode mode = ShotMode.Normal;
            if (salvoShootRadBtn.Checked)
                mode = ShotMode.Salvo;
            _selection.watchGame = true;
            _selection.shotMode = mode;
            Close();
        }

        /*
            _selection Property
        */

        public ModeSelection Selection
        {
            get { return _selection; }
        }
    }
}
