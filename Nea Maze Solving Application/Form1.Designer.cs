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
            AStar = new Button();
            BreadthFirst = new Button();
            ReloadMaze = new Button();
            Clear = new Button();
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
            // AStar
            // 
            AStar.Location = new Point(1851, 235);
            AStar.Name = "AStar";
            AStar.Size = new Size(75, 23);
            AStar.TabIndex = 2;
            AStar.Text = "AStar";
            AStar.UseVisualStyleBackColor = true;
            AStar.Click += AStar_Click;
            // 
            // BreadthFirst
            // 
            BreadthFirst.Location = new Point(1851, 264);
            BreadthFirst.Name = "BreadthFirst";
            BreadthFirst.Size = new Size(75, 23);
            BreadthFirst.TabIndex = 3;
            BreadthFirst.Text = "BreadthFirst";
            BreadthFirst.UseVisualStyleBackColor = true;
            BreadthFirst.Click += BreadthFirst_Click;
            // 
            // ReloadMaze
            // 
            ReloadMaze.Location = new Point(1851, 293);
            ReloadMaze.Name = "ReloadMaze";
            ReloadMaze.Size = new Size(75, 23);
            ReloadMaze.TabIndex = 4;
            ReloadMaze.Text = "Reload";
            ReloadMaze.UseVisualStyleBackColor = true;
            ReloadMaze.Click += ReloadMaze_Click;
            // 
            // Clear
            // 
            Clear.Location = new Point(1851, 322);
            Clear.Name = "Clear";
            Clear.Size = new Size(75, 23);
            Clear.TabIndex = 5;
            Clear.Text = "Clear";
            Clear.UseVisualStyleBackColor = true;
            Clear.Click += Clear_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1984, 1161);
            Controls.Add(Clear);
            Controls.Add(ReloadMaze);
            Controls.Add(BreadthFirst);
            Controls.Add(AStar);
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
        private Button AStar;
        private Button BreadthFirst;
        private Button ReloadMaze;
        private Button Clear;
    }
}
