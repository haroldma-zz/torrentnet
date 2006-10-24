using System;

namespace TrackerProbe
{
    partial class TrackerProbe
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
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.trackerInfo = new System.Windows.Forms.GroupBox();
            this.trackerStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.hash = new System.Windows.Forms.Label();
            this.pieceLength = new System.Windows.Forms.Label();
            this.numberOfPieces = new System.Windows.Forms.Label();
            this.trackerUrl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.peerList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.swarmSize = new System.Windows.Forms.Label();
            this.mainMenu.SuspendLayout();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.trackerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(680, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // splitter
            // 
            this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitter.Location = new System.Drawing.Point(0, 24);
            this.splitter.Name = "splitter";
            this.splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitter.Panel1
            // 
            this.splitter.Panel1.Controls.Add(this.trackerInfo);
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.Controls.Add(this.peerList);
            this.splitter.Size = new System.Drawing.Size(680, 420);
            this.splitter.SplitterDistance = 160;
            this.splitter.TabIndex = 1;
            this.splitter.TabStop = false;
            // 
            // trackerInfo
            // 
            this.trackerInfo.Controls.Add(this.swarmSize);
            this.trackerInfo.Controls.Add(this.trackerStatus);
            this.trackerInfo.Controls.Add(this.label5);
            this.trackerInfo.Controls.Add(this.hash);
            this.trackerInfo.Controls.Add(this.pieceLength);
            this.trackerInfo.Controls.Add(this.numberOfPieces);
            this.trackerInfo.Controls.Add(this.trackerUrl);
            this.trackerInfo.Controls.Add(this.label4);
            this.trackerInfo.Controls.Add(this.label3);
            this.trackerInfo.Controls.Add(this.label2);
            this.trackerInfo.Controls.Add(this.label1);
            this.trackerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackerInfo.Location = new System.Drawing.Point(0, 0);
            this.trackerInfo.Name = "trackerInfo";
            this.trackerInfo.Size = new System.Drawing.Size(680, 160);
            this.trackerInfo.TabIndex = 0;
            this.trackerInfo.TabStop = false;
            this.trackerInfo.Text = "Torrent Information";
            // 
            // trackerStatus
            // 
            this.trackerStatus.AutoSize = true;
            this.trackerStatus.Location = new System.Drawing.Point(83, 111);
            this.trackerStatus.Name = "trackerStatus";
            this.trackerStatus.Size = new System.Drawing.Size(0, 13);
            this.trackerStatus.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Status:";
            // 
            // hash
            // 
            this.hash.AutoSize = true;
            this.hash.Location = new System.Drawing.Point(83, 86);
            this.hash.Name = "hash";
            this.hash.Size = new System.Drawing.Size(0, 13);
            this.hash.TabIndex = 7;
            // 
            // pieceLength
            // 
            this.pieceLength.AutoSize = true;
            this.pieceLength.Location = new System.Drawing.Point(83, 64);
            this.pieceLength.Name = "pieceLength";
            this.pieceLength.Size = new System.Drawing.Size(0, 13);
            this.pieceLength.TabIndex = 6;
            // 
            // numberOfPieces
            // 
            this.numberOfPieces.AutoSize = true;
            this.numberOfPieces.Location = new System.Drawing.Point(83, 42);
            this.numberOfPieces.Name = "numberOfPieces";
            this.numberOfPieces.Size = new System.Drawing.Size(0, 13);
            this.numberOfPieces.TabIndex = 5;
            // 
            // trackerUrl
            // 
            this.trackerUrl.AutoSize = true;
            this.trackerUrl.Location = new System.Drawing.Point(83, 20);
            this.trackerUrl.Name = "trackerUrl";
            this.trackerUrl.Size = new System.Drawing.Size(0, 13);
            this.trackerUrl.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Hash:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Piece Length:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "# of Pieces:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tracker URL:";
            // 
            // peerList
            // 
            this.peerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.peerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.peerList.Location = new System.Drawing.Point(0, 0);
            this.peerList.Name = "peerList";
            this.peerList.Size = new System.Drawing.Size(680, 256);
            this.peerList.TabIndex = 0;
            this.peerList.UseCompatibleStateImageBehavior = false;
            this.peerList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "IP";
            this.columnHeader1.Width = 178;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Port";
            // 
            // swarmSize
            // 
            this.swarmSize.AutoSize = true;
            this.swarmSize.Location = new System.Drawing.Point(13, 133);
            this.swarmSize.Name = "swarmSize";
            this.swarmSize.Size = new System.Drawing.Size(0, 13);
            this.swarmSize.TabIndex = 10;
            // 
            // TrackerProbe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 444);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.mainMenu);
            this.Name = "TrackerProbe";
            this.Text = "TrackerProbe";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            this.splitter.ResumeLayout(false);
            this.trackerInfo.ResumeLayout(false);
            this.trackerInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitter;
        private System.Windows.Forms.GroupBox trackerInfo;
        private System.Windows.Forms.ListView peerList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label hash;
        private System.Windows.Forms.Label pieceLength;
        private System.Windows.Forms.Label numberOfPieces;
        private System.Windows.Forms.Label trackerUrl;
        private System.Windows.Forms.Label trackerStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label swarmSize;
    }
}

