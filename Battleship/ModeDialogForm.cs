/*
    This form enables the user to choose whether he wants to play
    or watch a game of Battleship
*/

using System;
using System.Windows.Forms;

namespace Battleship
{
    public partial class ModeDialogForm : Form
    {
        /*
            No-Arg Constructor
        */

        private ModeDialogForm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        /*
            The ShowModeDialog method displays a dialog requesting
            the user to input whether he wants to play or watch
            a game of Battleship
        */

        public static DialogResult ShowModeDialog()
        {
            ModeDialogForm form = new ModeDialogForm();
            DialogResult result = form.ShowDialog();
            return result;
        }

        /*
            Handler for playButton
        */

        private void playButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            // Play game.
            Close();
        }

        /*
            Handler for watchButton
        */

        private void watchButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            // Watch game.
            Close();
        }
    }
}
