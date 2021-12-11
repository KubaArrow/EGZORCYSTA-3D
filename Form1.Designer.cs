
namespace EGZORCYSTA_3D
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.screen = new System.Windows.Forms.PictureBox();
            this.mini_mapa = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_start = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.screen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mini_mapa)).BeginInit();
            this.SuspendLayout();
            // 
            // screen
            // 
            resources.ApplyResources(this.screen, "screen");
            this.screen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.screen.Name = "screen";
            this.screen.TabStop = false;
            // 
            // mini_mapa
            // 
            resources.ApplyResources(this.mini_mapa, "mini_mapa");
            this.mini_mapa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.mini_mapa.Name = "mini_mapa";
            this.mini_mapa.TabStop = false;
            this.mini_mapa.Click += new System.EventHandler(this.mapa_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 33;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_start
            // 
            resources.ApplyResources(this.button_start, "button_start");
            this.button_start.Name = "button_start";
            this.button_start.TabStop = false;
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.mini_mapa);
            this.Controls.Add(this.screen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Click += new System.EventHandler(this.Form1_Click);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.screen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mini_mapa)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox screen;
        private System.Windows.Forms.PictureBox map;
        private System.Windows.Forms.PictureBox mini_mapa;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_start;
    }
}

