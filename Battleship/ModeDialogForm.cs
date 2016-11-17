using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public partial class ModeDialogForm : Form
    {
        private ModeDialogForm()
        {
            InitializeComponent();
        }

        public static DialogResult ShowModeDialog()
        {
            ModeDialogForm form = new ModeDialogForm();
            DialogResult result = form.ShowDialog();
            return result;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            // Play game.
            Close();
        }

        private void watchButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            // Watch game.
            Close();
        }
    }
}
