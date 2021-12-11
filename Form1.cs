using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Media;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading;
using WMPLib;

namespace EGZORCYSTA_3D
{  
    public partial class Form1 : Form
    {
        
        private Size rozmiar_klatki,rozmiar_mini_mapy;
        private Size rozmiar_mapy = new Size(900, 450);
        private Image<Bgr, byte> mapa_org;
        private double[] tabela_promieni;
        int kat_pocz = 0;
        int liczba_promieni = 1600;
        bool moveW, moveS, moveA, moveD;
        int speed = 3;
        Point p,mouse_last;
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

        
        public Form1()
        {
            InitializeComponent();
            wplayer.URL = "src/audio/theme.wav";
            p = new Point(75, 75);
            rozmiar_klatki = screen.Size;
            
            rozmiar_mini_mapy = mini_mapa.Size;
            get_map("src/maps/test1.png", ref mapa_org);
            analiza_mapy();
            set_mini_map();
            //ma = new Image<Bgr, byte>(rozmiar_mapy);
            moveW = moveS = moveA = moveD = false;
            
            wplayer.controls.play();
            

            mouse_last = new Point( 960,0);
            
        }
        private void analiza_mapy()
        {
            byte[,,] tmp = mapa_org.Data;

            for (int x=0;x<mapa_org.Width;x++)
            {
                for(int y=0;y<mapa_org.Height;y++)
                {
                    if(tmp[y,x,0]==0&& tmp[y, x, 1] == 255 && tmp[y, x, 2] == 0 )
                    {
                        p = new Point(x, y);
                        tmp[y, x, 0] = 255;
                        tmp[y, x, 1] = 255;
                        tmp[y, x, 2] = 255;
                    }
                }
            }
            //Tutaj analizuje mape
        }


            private void set_mini_map()
        {
            Image<Bgr, byte> tmp = new Image<Bgr, byte>(rozmiar_mini_mapy);
            tmp.SetZero();
            Image<Bgr, byte> tmp2 = new Image<Bgr, byte>(rozmiar_mapy);
            tmp2= mapa_org.Clone();
            CvInvoke.Rectangle(tmp2, new Rectangle(p.X - 5, p.Y - 5, 11, 11), new MCvScalar(0, 255, 100), -1);
           
            
            
            int dx = (int)(100 * Math.Cos(kat_pocz * (Math.PI / 180)));
            int dy = (int)(100 * Math.Sin(kat_pocz * (Math.PI / 180)));
            CvInvoke.Line(tmp2,p, new Point(p.X+dx,p.Y+dy), new MCvScalar(0, 255, 100), 3);
            CvInvoke.Resize(tmp2, tmp, rozmiar_mini_mapy);
            mini_mapa.Image = tmp.Bitmap;
        }
       
        private void get_map(string path, ref Image<Bgr, byte> Data)
        {
            Data = new Image<Bgr, byte>(rozmiar_mapy);
            Mat temp = CvInvoke.Imread(path);
            
            
            Data = temp.ToImage<Bgr, byte>();
            
           
        }

     

        private void rys_tla()
        {
            MCvScalar kolor_promienia = new MCvScalar(255,0,0);
            double krok_katowy, aktualny_kat,kat;
            double w0 = 90;
            double[] tabela_wys = new double[liczba_promieni]; ;
            aktualny_kat = (kat_pocz - 30) * (Math.PI / 180);
            krok_katowy = (Math.PI / (3 * liczba_promieni));
            Image<Bgr, byte> tmp_k;
            tmp_k = new Image<Bgr, byte>(rozmiar_klatki);
            tmp_k.SetZero();
            CvInvoke.Rectangle(tmp_k, new Rectangle(0, 0, 1600, 450), new MCvScalar(255, 255, 255), -1);
            for(int x=0;x<150;x++)
            {
                CvInvoke.Rectangle(tmp_k, new Rectangle(0, 0 + (x * 3), 1600, 3), new MCvScalar(255 - x, 255 - x, 255 - x), -1);
                CvInvoke.Rectangle(tmp_k, new Rectangle(0, 450+(x*3), 1600, 3), new MCvScalar(0+x, 0+x, 0+x), -1);
            }

            Image<Bgr, byte> wall = new Image<Bgr, byte>(900,900);
            
            Mat temp = CvInvoke.Imread("src/img/textures/test.png");
            wall = temp.ToImage<Bgr, byte>();
            double max_p = tabela_promieni.Max();
            double wsp = max_p / 200;
            byte[,,] temp1 = tmp_k.Data;
            for (int i = 0; i < liczba_promieni; i++)
            {
                kat = ((kat_pocz)* (Math.PI / 180))-aktualny_kat ;
              
                if (kat < 0) kat += 2* Math.PI;
                if (kat > 2*Math.PI) kat -= 2* Math.PI;
                tabela_wys[i] = 900*(80 / tabela_promieni[i] );
                tabela_wys[i] /= Math.Cos(kat);
                double y1 =(450-(tabela_wys[i]/2));
                
                
                byte cien = 0, b=100,g=50,r=50;
                byte[,,] test = wall.Data;
                if (tabela_promieni[i] > (max_p / 2))
                  {
                      for (int x = 0; x < ((tabela_promieni[i] - (max_p / 2)) / wsp) - 1; x++)
                      {
                        // if (r > 100) r++;
                        // if (g > 100) g++;
                        // if (b >0) b++;
                        if (cien < 100) cien++;
                    }


                  }
                double wspk = 900 / tabela_wys[i];
                if (tabela_wys[i] / 2 > 450)
                    tabela_wys[i] = 900;
                
                int tmp_y = 450;
                    for (int y=0;y< (tabela_wys[i] / 2); y++)
                    {

                    // for(int tmx=0;tmx<10;tmx++)
                    //  {

                    // }
                 
                   /* temp1[450-y, i , 0] = b;
                        temp1[450-y, i  , 1] = g;
                        temp1[450-y, i  , 2] = r;
                        
                         temp1[y+450, i, 0] = b;
                         temp1[y+450, i, 1] = g;
                         temp1[y+450, i, 2] = r;*/
                    

                    temp1[450-y, i, 0] = (byte)(test[(int)(450 + (wspk * y)),(int) ((wspk * i )% 900), 0]- (int)cien);

                    temp1[450-y, i, 1] = (byte)(test[(int)(450 - (wspk * y)), (int)((wspk * i) % 900), 1]- (int)cien);

                    temp1[450-y, i, 2] = (byte)(test[(int)(450 - (wspk * y)), (int)((wspk * i) % 900), 2]- (int)cien);
                    temp1[450 + y, i, 0] = (byte)(test[(int)(450 + (wspk * y)), (int)((wspk * i) % 900), 0] - (int)cien);

                    temp1[450 + y, i, 1] = (byte)(test[(int)(450 + (wspk * y)), (int)((wspk * i) % 900), 1] - (int)cien);

                    temp1[450 + y, i, 2] = (byte)(test[(int)(450 + (wspk * y)), (int)((wspk * i) % 900), 2] - (int)cien);






                }





                aktualny_kat += krok_katowy;

            }

            Image<Bgr, byte> gun = new Image<Bgr, byte>(300,200);
            Mat tx = CvInvoke.Imread("src/img/gun.png");


            gun = tx.ToImage<Bgr, byte>();
            byte[,,] gun_b = gun.Data;
            for (int x = 0; x <300; x++)
            { 
                for (int y = 0; y < 200; y++)
                {
                    if (gun_b[y , x , 0] != 150 && gun_b[y, x , 1] != 100 && gun_b[y , x , 2] != 50)
                    {
                        temp1[y + 700, x + 650, 0] = gun_b[y, x, 0];

                        temp1[y + 700, x + 650, 1] = gun_b[y, x, 1];

                        temp1[y + 700, x + 650, 2] = gun_b[y, x, 2];
                    }
                }
            }
               

                tmp_k.Data = temp1;
            screen.Image = tmp_k.Bitmap;
        }

        private void mapa_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

            if (timer1.Enabled == true)
            {
                if (e.KeyData == Keys.W)
                {
                    moveW = false;
                }
                if (e.KeyData == Keys.D)
                {

                    moveD = false;
                }
                if (e.KeyData == Keys.S)
                {
                    moveS = false;
                }
                if (e.KeyData == Keys.A)
                {
                    moveA = false;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (timer1.Enabled == true)
            {
                if (e.KeyData == Keys.W)
                {
                    moveW = true;
                }
                if (e.KeyData == Keys.D)
                {
                    moveD = true;
                }
                if (e.KeyData == Keys.S)
                {
                    moveS = true;
                }
                if (e.KeyData == Keys.A)
                {
                    moveA = true;

                }
            }
            if (e.KeyData == Keys.Escape)
            {
                timer1.Stop();
                Cursor.Show();
            }
        }
        WMPLib.WindowsMediaPlayer wplayer2 = new WMPLib.WindowsMediaPlayer();
        private void Form1_Click(object sender, EventArgs e)
        {


            //wplayer.controls.pause();
            if (timer1.Enabled == true)
            {
                
                wplayer2.URL=("src/audio/boner-shot.mp3");
                wplayer2.controls.play();
                //wplayer.controls.play();
            }
            
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            timer1.Start();
            Cursor.Hide();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (timer1.Enabled == true)
            {
                if (e.X > mouse_last.X && kat_pocz <= 360)
                {
                    if (kat_pocz == 360)
                    {
                        kat_pocz = 0;
                    }
                    else
                        kat_pocz += speed;
                }
                else
                if (e.X < mouse_last.X && kat_pocz >= 0)
                {
                    if (kat_pocz == 0)
                    {
                        kat_pocz = 360;
                    }
                    else
                        kat_pocz -= speed;
                }

                mouse_last.X = e.X;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Debug.WriteLine(e.KeyChar);
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            move();
            tabela_promieni = sygnatura_radialna(p);
            rys_tla();
            set_mini_map();
            Cursor.Position=new Point(960,950);
            
       
        }

        private void move()
        {
            byte[,,] tmp = mapa_org.Data;
            

            int dx = (int)(speed *2* Math.Cos(kat_pocz * (Math.PI / 180)));
            int dy = (int)(speed * 2 * Math.Sin(kat_pocz * (Math.PI / 180)));

            if(tmp[p.Y + dy, p.X + dx, 0] != 255 && tmp[p.Y + dy, p.X + dx, 1] != 255 && tmp[p.Y + dy, p.X + dx, 2] != 255)
            {
                moveW = false;
            }
            if (tmp[p.Y - dy, p.X - dx, 0] != 255 && tmp[p.Y - dy, p.X - dx, 1] != 255 && tmp[p.Y - dy, p.X - dx, 2] != 255)
            {
                moveS = false;
            }
            dx = (int)(speed * 2 * Math.Cos((kat_pocz+90) * (Math.PI / 180)));
            dy = (int)(speed * 2 * Math.Sin((kat_pocz+90) * (Math.PI / 180)));

            if (tmp[p.Y + dy, p.X + dx, 0] != 255 && tmp[p.Y + dy, p.X + dx, 1] != 255 && tmp[p.Y + dy, p.X + dx, 2] != 255)
            {
                moveD = false;
            }
            if (tmp[p.Y - dy, p.X - dx, 0] != 255 && tmp[p.Y - dy, p.X - dx, 1] != 255 && tmp[p.Y - dy, p.X - dx, 2] != 255)
            {
                moveA = false;
            }
           

            if (moveW)
            {
               
                p.X += (int)(speed*Math.Cos(kat_pocz * (Math.PI / 180)));
                p.Y += (int)(speed*Math.Sin(kat_pocz*(Math.PI/180)));
            }
            if (moveD)
            {
                p.X += (int)(speed * Math.Cos((kat_pocz+90) * (Math.PI / 180)));
                p.Y += (int)(speed * Math.Sin((kat_pocz +90) * (Math.PI / 180)));
            }
            if (moveS)
            {
                p.X -= (int)(speed * Math.Cos(kat_pocz * (Math.PI / 180)));
                p.Y -= (int)(speed * Math.Sin(kat_pocz * (Math.PI / 180)));
            }
            if (moveA)
            {
                p.X += (int)(speed * Math.Cos((kat_pocz-90) * (Math.PI / 180)));
                p.Y += (int)(speed * Math.Sin((kat_pocz-90) * (Math.PI / 180)));
            }

        }
        private double[] sygnatura_radialna(Point start)
        {
            Image<Bgr, byte> tmp_m;
            tmp_m = new Image<Bgr, byte>(rozmiar_mapy);
            
            tmp_m.SetZero();
            
            MCvScalar kolor_promienia = new MCvScalar(255,255,255);
            double[,] katy_kolejnych_promieni = new double[liczba_promieni, 2];
            double[] promienie = new double[liczba_promieni];
            double krok_katowy, aktualny_kat;

            
            aktualny_kat = (kat_pocz-30) * (Math.PI / 180);

            
                krok_katowy = ( Math.PI / (3*liczba_promieni));

            for (int i = 0; i < liczba_promieni; i++)
            {
                katy_kolejnych_promieni[i, 0] = Math.Cos(aktualny_kat);
                katy_kolejnych_promieni[i, 1] = Math.Sin(aktualny_kat);
                aktualny_kat += krok_katowy;
            }

            
            byte[,,] temp1 = mapa_org.Data;
            int zakres = (int)Math.Sqrt(Math.Pow(rozmiar_mapy.Width, 2) + Math.Pow(rozmiar_mapy.Height, 2));
            for (int p = 0; p < liczba_promieni; p++)
            {
                for (int d = 0; d < zakres; d++)
                {
                    Point cp = new Point();
                    int dx, dy;
                    dx = (int)(d * katy_kolejnych_promieni[p, 0]);
                    dy = (int)(d * katy_kolejnych_promieni[p, 1]);
                    if (Math.Abs(dx) < zakres && Math.Abs(dy) < zakres)
                    {
                        cp.X = start.X + dx;
                        cp.Y = start.Y + dy;
                        if (temp1[cp.Y, cp.X, 0] == 0x00&& temp1[cp.Y, cp.X, 1] == 0x00&& temp1[cp.Y, cp.X, 2] == 0x00)
                        {
                            //CvInvoke.Line(tmp_m, start, cp, kolor_promienia, 1);
                            promienie[p] = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                            //Debug.WriteLine(promienie[p]);
                            break;
                        }
                    }
                }
            }
            
            // CvInvoke.Line(tmp_m, start, start, new MCvScalar(0, 255, 0), 1);
            //CvInvoke.Resize(tmp_m, tmp_m, rozmiar_mini_mapy);
            //mini_map_debug.Image = tmp_m.Bitmap;
           
            return promienie;
        }
    }
}
