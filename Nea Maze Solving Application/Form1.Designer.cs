namespace Nea_Maze_Solving_Application
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            generateMaze = new Button();
            Dijkstra = new Button();
            SuspendLayout();
            // 
            // generateMaze
            // 
            generateMaze.Location = new Point(1851, 177);
            generateMaze.Name = "generateMaze";
            generateMaze.Size = new Size(75, 23);
            generateMaze.TabIndex = 0;
            generateMaze.Text = "Generate";
            generateMaze.UseVisualStyleBackColor = true;
            generateMaze.Click += generateMaze_Click;
            // 
            // Dijkstra
            // 
            Dijkstra.Location = new Point(1851, 206);
            Dijkstra.Name = "Dijkstra";
            Dijkstra.Size = new Size(75, 23);
            Dijkstra.TabIndex = 1;
            Dijkstra.Text = "Dijkstras";
            Dijkstra.UseVisualStyleBackColor = true;
            Dijkstra.Click += Dijkstra_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1984, 1161);
            Controls.Add(Dijkstra);
            Controls.Add(generateMaze);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button generateMaze;
        private Button Dijkstra;
    }
}
