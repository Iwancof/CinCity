namespace CinCity
{
    partial class CinCity
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.DispTim = new System.Windows.Forms.Timer(this.components);
            this.tcplis = new System.Windows.Forms.Button();
            this.ServerStop = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ProcessTimer = new System.Windows.Forms.Timer(this.components);
            this.Clear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // DispTim
            // 
            this.DispTim.Interval = 1;
            this.DispTim.Tick += new System.EventHandler(this.DispTim_Tick);
            // 
            // tcplis
            // 
            this.tcplis.Location = new System.Drawing.Point(12, 198);
            this.tcplis.Name = "tcplis";
            this.tcplis.Size = new System.Drawing.Size(169, 43);
            this.tcplis.TabIndex = 1;
            this.tcplis.Text = "Listen";
            this.tcplis.UseVisualStyleBackColor = true;
            this.tcplis.Click += new System.EventHandler(this.button1_Click);
            // 
            // ServerStop
            // 
            this.ServerStop.Location = new System.Drawing.Point(12, 149);
            this.ServerStop.Name = "ServerStop";
            this.ServerStop.Size = new System.Drawing.Size(169, 43);
            this.ServerStop.TabIndex = 2;
            this.ServerStop.Text = "Stop";
            this.ServerStop.UseVisualStyleBackColor = true;
            this.ServerStop.Click += new System.EventHandler(this.ServerStop_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1002, 520);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.Resize += new System.EventHandler(this.CinCity_Resize);
            // 
            // ProcessTimer
            // 
            this.ProcessTimer.Interval = 10;
            this.ProcessTimer.Tick += new System.EventHandler(this.ProcessTimer_Tick);
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(12, 247);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(169, 43);
            this.Clear.TabIndex = 3;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // CinCity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 520);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.ServerStop);
            this.Controls.Add(this.tcplis);
            this.Controls.Add(this.pictureBox1);
            this.Name = "CinCity";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer DispTim;
        private System.Windows.Forms.Button tcplis;
        private System.Windows.Forms.Button ServerStop;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer ProcessTimer;
        private System.Windows.Forms.Button Clear;
    }
}

