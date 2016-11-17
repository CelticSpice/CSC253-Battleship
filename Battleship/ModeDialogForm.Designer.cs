namespace Battleship
{
    partial class ModeDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.modeLabel = new System.Windows.Forms.Label();
            this.playButton = new System.Windows.Forms.Button();
            this.watchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // modeLabel
            // 
            this.modeLabel.AutoSize = true;
            this.modeLabel.Location = new System.Drawing.Point(69, 9);
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(140, 13);
            this.modeLabel.TabIndex = 0;
            this.modeLabel.Text = "Please select a game mode:";
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(59, 40);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(75, 23);
            this.playButton.TabIndex = 1;
            this.playButton.Text = "Play Game";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // watchButton
            // 
            this.watchButton.AutoSize = true;
            this.watchButton.Location = new System.Drawing.Point(140, 40);
            this.watchButton.Name = "watchButton";
            this.watchButton.Size = new System.Drawing.Size(80, 23);
            this.watchButton.TabIndex = 2;
            this.watchButton.Text = "Watch Game";
            this.watchButton.UseVisualStyleBackColor = true;
            this.watchButton.Click += new System.EventHandler(this.watchButton_Click);
            // 
            // ModeDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 78);
            this.Controls.Add(this.watchButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.modeLabel);
            this.Name = "ModeDialogForm";
            this.Text = "ModeDialogForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label modeLabel;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button watchButton;
    }
}