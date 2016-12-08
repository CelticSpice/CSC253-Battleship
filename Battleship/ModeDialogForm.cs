/*
    Dialog for enabling the user to choose
    game and shooting modes
    12/8/2016
    CSC 253 0001 - M6PROJ
    Author: James Alves, Shane McCann, Timothy Burns
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

        private ModeDialogForm()
        {
            InitializeComponent();
            _selection.cancelClicked = true;
            CenterToScreen();
        }

        /*
            Display - Shows the dialog
        */

        public static ModeSelection Display()
        {
            ModeDialogForm dialog = new ModeDialogForm();
            dialog.ShowDialog();                
            return dialog.Selection;
        }

        /*
            Handler for playButton
        */

        private void playButton_Click(object sender, EventArgs e)
        {
            _selection.cancelClicked = false;
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
            _selection.cancelClicked = false;
            ShotMode mode = ShotMode.Normal;
            if (salvoShootRadBtn.Checked)
                mode = ShotMode.Salvo;
            _selection.watchGame = true;
            _selection.shotMode = mode;
            Close();
        }

        /*
            Selection Property
        */

        public ModeSelection Selection
        {
            get { return _selection; }
        }
    }
}
