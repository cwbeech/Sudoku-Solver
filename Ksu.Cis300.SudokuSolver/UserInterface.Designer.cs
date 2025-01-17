﻿namespace Ksu.Cis300.SudokuSolver
{
    partial class UserInterface
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
            this.uxMenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.puzzleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.new4x4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.new9x9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxFlowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.uxMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxMenuStrip1
            // 
            this.uxMenuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.uxMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.uxMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.puzzleToolStripMenuItem});
            this.uxMenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.uxMenuStrip1.Name = "uxMenuStrip1";
            this.uxMenuStrip1.Size = new System.Drawing.Size(800, 33);
            this.uxMenuStrip1.TabIndex = 0;
            this.uxMenuStrip1.Text = "menuStrip1";
            // 
            // puzzleToolStripMenuItem
            // 
            this.puzzleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new4x4ToolStripMenuItem,
            this.new9x9ToolStripMenuItem,
            this.solveToolStripMenuItem});
            this.puzzleToolStripMenuItem.Name = "puzzleToolStripMenuItem";
            this.puzzleToolStripMenuItem.Size = new System.Drawing.Size(77, 30);
            this.puzzleToolStripMenuItem.Text = "Puzzle";
            // 
            // new4x4ToolStripMenuItem
            // 
            this.new4x4ToolStripMenuItem.Name = "new4x4ToolStripMenuItem";
            this.new4x4ToolStripMenuItem.Size = new System.Drawing.Size(182, 34);
            this.new4x4ToolStripMenuItem.Text = "New 4x4";
            this.new4x4ToolStripMenuItem.Click += new System.EventHandler(this.new4x4ToolStripMenuItem_Click);
            // 
            // new9x9ToolStripMenuItem
            // 
            this.new9x9ToolStripMenuItem.Name = "new9x9ToolStripMenuItem";
            this.new9x9ToolStripMenuItem.Size = new System.Drawing.Size(182, 34);
            this.new9x9ToolStripMenuItem.Text = "New 9x9";
            this.new9x9ToolStripMenuItem.Click += new System.EventHandler(this.new9x9ToolStripMenuItem_Click);
            // 
            // solveToolStripMenuItem
            // 
            this.solveToolStripMenuItem.Name = "solveToolStripMenuItem";
            this.solveToolStripMenuItem.Size = new System.Drawing.Size(182, 34);
            this.solveToolStripMenuItem.Text = "Solve";
            this.solveToolStripMenuItem.Click += new System.EventHandler(this.solveToolStripMenuItem_Click);
            // 
            // uxFlowLayoutPanel1
            // 
            this.uxFlowLayoutPanel1.AutoSize = true;
            this.uxFlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.uxFlowLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.uxFlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.uxFlowLayoutPanel1.Location = new System.Drawing.Point(13, 37);
            this.uxFlowLayoutPanel1.MinimumSize = new System.Drawing.Size(50, 50);
            this.uxFlowLayoutPanel1.Name = "uxFlowLayoutPanel1";
            this.uxFlowLayoutPanel1.Size = new System.Drawing.Size(50, 50);
            this.uxFlowLayoutPanel1.TabIndex = 1;
            this.uxFlowLayoutPanel1.WrapContents = false;
            // 
            // UserInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uxFlowLayoutPanel1);
            this.Controls.Add(this.uxMenuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.uxMenuStrip1;
            this.MaximizeBox = false;
            this.Name = "UserInterface";
            this.Text = "Sudoku Solver";
            this.uxMenuStrip1.ResumeLayout(false);
            this.uxMenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip uxMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem puzzleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem new4x4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem new9x9ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solveToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel uxFlowLayoutPanel1;
    }
}

