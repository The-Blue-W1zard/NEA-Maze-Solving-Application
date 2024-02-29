namespace Nea_Maze_Solving_Application
{
    partial class FinishedForm
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
            label1 = new Label();
            label2 = new Label();
            Ignore = new Button();
            RevertMaze = new Button();
            ClearMaze = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(12, 46);
            label1.Name = "label1";
            label1.Size = new Size(207, 28);
            label1.TabIndex = 0;
            label1.Text = "Finished Solving Maze";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15F);
            label2.Location = new Point(12, 74);
            label2.Name = "label2";
            label2.Size = new Size(277, 28);
            label2.TabIndex = 1;
            label2.Text = "What do you want to do now?";
            // 
            // Ignore
            // 
            Ignore.Location = new Point(12, 130);
            Ignore.Name = "Ignore";
            Ignore.Size = new Size(75, 23);
            Ignore.TabIndex = 2;
            Ignore.Text = "Ignore";
            Ignore.UseVisualStyleBackColor = true;
            Ignore.Click += Ignore_Click;
            // 
            // RevertMaze
            // 
            RevertMaze.Location = new Point(93, 130);
            RevertMaze.Name = "RevertMaze";
            RevertMaze.Size = new Size(113, 23);
            RevertMaze.TabIndex = 3;
            RevertMaze.Text = "Revert to unsolved";
            RevertMaze.UseVisualStyleBackColor = true;
            RevertMaze.Click += RevertMaze_Click;
            // 
            // ClearMaze
            // 
            ClearMaze.Location = new Point(214, 130);
            ClearMaze.Name = "ClearMaze";
            ClearMaze.Size = new Size(75, 23);
            ClearMaze.TabIndex = 4;
            ClearMaze.Text = "Clear Maze";
            ClearMaze.UseVisualStyleBackColor = true;
            ClearMaze.Click += ClearMaze_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(318, 179);
            Controls.Add(ClearMaze);
            Controls.Add(RevertMaze);
            Controls.Add(Ignore);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form2";
            Text = "Finished Solving";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Button Ignore;
        private Button RevertMaze;
        private Button ClearMaze;
    }
}