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
            ChangeStart = new Button();
            ChangeEnd = new Button();
            ReloadFromFile = new Button();
            ExportToFile = new Button();
            RecentMazeHistory = new Button();
            label1 = new Label();
            label2 = new Label();
            Slow = new Button();
            Medium = new Button();
            Fast = new Button();
            SolveMaze = new Button();
            SuspendLayout();
            // 
            // generateMaze
            // 
            generateMaze.Location = new Point(1720, 138);
            generateMaze.Name = "generateMaze";
            generateMaze.Size = new Size(206, 23);
            generateMaze.TabIndex = 0;
            generateMaze.Text = "Generate";
            generateMaze.UseVisualStyleBackColor = true;
            generateMaze.Click += generateMaze_Click;
            // 
            // Dijkstra
            // 
            Dijkstra.Location = new Point(1720, 226);
            Dijkstra.Name = "Dijkstra";
            Dijkstra.Size = new Size(102, 23);
            Dijkstra.TabIndex = 1;
            Dijkstra.Text = "Dijkstras";
            Dijkstra.UseVisualStyleBackColor = true;
            Dijkstra.Click += Dijkstra_Click;
            // 
            // AStar
            // 
            AStar.Location = new Point(1828, 226);
            AStar.Name = "AStar";
            AStar.Size = new Size(102, 23);
            AStar.TabIndex = 2;
            AStar.Text = "A Star";
            AStar.UseVisualStyleBackColor = true;
            AStar.Click += AStar_Click;
            // 
            // BreadthFirst
            // 
            BreadthFirst.Location = new Point(1720, 255);
            BreadthFirst.Name = "BreadthFirst";
            BreadthFirst.Size = new Size(102, 23);
            BreadthFirst.TabIndex = 3;
            BreadthFirst.Text = "Breadth First";
            BreadthFirst.UseVisualStyleBackColor = true;
            BreadthFirst.Click += BreadthFirst_Click;
            // 
            // ReloadMaze
            // 
            ReloadMaze.Location = new Point(1720, 461);
            ReloadMaze.Name = "ReloadMaze";
            ReloadMaze.Size = new Size(102, 23);
            ReloadMaze.TabIndex = 4;
            ReloadMaze.Text = "Undo Action";
            ReloadMaze.UseVisualStyleBackColor = true;
            ReloadMaze.Click += ReloadMaze_Click;
            // 
            // Clear
            // 
            Clear.Location = new Point(1828, 461);
            Clear.Name = "Clear";
            Clear.Size = new Size(98, 23);
            Clear.TabIndex = 5;
            Clear.Text = "Clear Maze";
            Clear.UseVisualStyleBackColor = true;
            Clear.Click += Clear_Click;
            // 
            // ChangeStart
            // 
            ChangeStart.Location = new Point(1720, 167);
            ChangeStart.Name = "ChangeStart";
            ChangeStart.Size = new Size(102, 23);
            ChangeStart.TabIndex = 6;
            ChangeStart.Text = "Change Start";
            ChangeStart.UseVisualStyleBackColor = true;
            ChangeStart.Click += ChangeStart_Click;
            // 
            // ChangeEnd
            // 
            ChangeEnd.Location = new Point(1828, 167);
            ChangeEnd.Name = "ChangeEnd";
            ChangeEnd.Size = new Size(102, 23);
            ChangeEnd.TabIndex = 7;
            ChangeEnd.Text = "Change End";
            ChangeEnd.UseVisualStyleBackColor = true;
            ChangeEnd.Click += ChangeEnd_Click;
            // 
            // ReloadFromFile
            // 
            ReloadFromFile.Location = new Point(1720, 560);
            ReloadFromFile.Name = "ReloadFromFile";
            ReloadFromFile.Size = new Size(206, 23);
            ReloadFromFile.TabIndex = 8;
            ReloadFromFile.Text = "Reload Maze From File";
            ReloadFromFile.UseVisualStyleBackColor = true;
            ReloadFromFile.Click += ReloadFromFile_Click;
            // 
            // ExportToFile
            // 
            ExportToFile.Location = new Point(1720, 531);
            ExportToFile.Name = "ExportToFile";
            ExportToFile.Size = new Size(206, 23);
            ExportToFile.TabIndex = 9;
            ExportToFile.Text = "Export Maze To File";
            ExportToFile.UseVisualStyleBackColor = true;
            ExportToFile.Click += ExportToFile_Click;
            // 
            // RecentMazeHistory
            // 
            RecentMazeHistory.Location = new Point(1720, 490);
            RecentMazeHistory.Name = "RecentMazeHistory";
            RecentMazeHistory.Size = new Size(206, 23);
            RecentMazeHistory.TabIndex = 10;
            RecentMazeHistory.Text = "Recent Maze History";
            RecentMazeHistory.UseVisualStyleBackColor = true;
            RecentMazeHistory.Click += RecentMazeHistory_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(1720, 202);
            label1.Name = "label1";
            label1.Size = new Size(158, 21);
            label1.TabIndex = 11;
            label1.Text = "Choose an algorithm.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.FlatStyle = FlatStyle.Flat;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(1720, 291);
            label2.Name = "label2";
            label2.Size = new Size(122, 21);
            label2.TabIndex = 12;
            label2.Text = "Choose a speed.";
            // 
            // Slow
            // 
            Slow.Location = new Point(1720, 315);
            Slow.Name = "Slow";
            Slow.Size = new Size(102, 23);
            Slow.TabIndex = 13;
            Slow.Text = "Slow";
            Slow.UseVisualStyleBackColor = true;
            // 
            // Medium
            // 
            Medium.Location = new Point(1828, 315);
            Medium.Name = "Medium";
            Medium.Size = new Size(102, 23);
            Medium.TabIndex = 14;
            Medium.Text = "Medium";
            Medium.UseVisualStyleBackColor = true;
            // 
            // Fast
            // 
            Fast.Location = new Point(1720, 344);
            Fast.Name = "Fast";
            Fast.Size = new Size(102, 23);
            Fast.TabIndex = 15;
            Fast.Text = "Fast";
            Fast.UseVisualStyleBackColor = true;
            // 
            // SolveMaze
            // 
            SolveMaze.Location = new Point(1720, 386);
            SolveMaze.Name = "SolveMaze";
            SolveMaze.Size = new Size(206, 32);
            SolveMaze.TabIndex = 16;
            SolveMaze.Text = "Solve Maze";
            SolveMaze.UseVisualStyleBackColor = true;
            SolveMaze.Click += SolveMaze_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SeaGreen;
            ClientSize = new Size(1984, 1161);
            Controls.Add(SolveMaze);
            Controls.Add(Fast);
            Controls.Add(Medium);
            Controls.Add(Slow);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(RecentMazeHistory);
            Controls.Add(ExportToFile);
            Controls.Add(ReloadFromFile);
            Controls.Add(ChangeEnd);
            Controls.Add(ChangeStart);
            Controls.Add(Clear);
            Controls.Add(ReloadMaze);
            Controls.Add(BreadthFirst);
            Controls.Add(AStar);
            Controls.Add(Dijkstra);
            Controls.Add(generateMaze);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            MouseClick += Form1_MouseClick;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button generateMaze;
        private Button Dijkstra;
        private Button AStar;
        private Button BreadthFirst;
        private Button ReloadMaze;
        private Button Clear;
        private Button ChangeStart;
        private Button ChangeEnd;
        private Button ReloadFromFile;
        private Button ExportToFile;
        private Button RecentMazeHistory;
        private Label label1;
        private Label label2;
        private Button Slow;
        private Button Medium;
        private Button Fast;
        private Button SolveMaze;
    }
}
