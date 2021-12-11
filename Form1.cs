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
        private Size rozmiar_mapy = new Size(900, 900);
        private Image<Bgr, byte> mapa_org;
        private double[] tabela_promieni;
        int kat_pocz = 0;
        int liczba_promieni = 1600;
        bool moveW, moveS, moveA, moveD;
        int speed = 3;
        Point p,mouse_last;
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        byte[] tab_color = new byte[1600];

        public Form1()
        {
            InitializeComponent();
            wplayer.URL = "src/audio/theme.wav";
            p = new Point(15, 15);
            rozmiar_klatki = screen.Size;
            
            rozmiar_mini_mapy = mini_mapa.Size;
            get_map("src/maps/test5.png", ref mapa_org);
            //analiza_mapy();
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
            
            Mat tmp1 = CvInvoke.Imread("src/img/textures/test.png");
            Mat tmp2 = CvInvoke.Imread("src/img/textures/beton.png");
            Mat tmp3 = CvInvoke.Imread("src/img/textures/cegla.png");
            
            List<byte[,,]> textures=new List<byte[,,]>();
            textures.Add(tmp1.ToImage<Bgr, byte>().Data);
            textures.Add(tmp2.ToImage<Bgr, byte>().Data);
            textures.Add(tmp3.ToImage<Bgr, byte>().Data);
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
                    Console.WriteLine(tab_color[i]);
                    int k1= textures[tab_color[i]][(int)(450 + (wspk * y)), (int)((wspk * i) % 900), 0] - (int)cien;
                    if (k1 < 0)
                        k1 = 0;
                    int k2 = textures[tab_color[i]][(int)(450 - (wspk * y)), (int)((wspk * i) % 900), 1] - (int)cien;
                    if (k2 < 0)
                        k2 = 0;
                    int k3 = textures[tab_color[i]][(int)(450 - (wspk * y)), (int)((wspk * i) % 900), 2] - (int)cien;
                    if (k3 < 0)
                        k3 = 0;
                    int k4 = textures[tab_color[i]][(int)(450 + (wspk * y)), (int)((wspk * i) % 900), 0] - (int)cien;
                    if (k4 < 0)
                        k4 = 0;
                    int k5 = textures[tab_color[i]][(int)(450 + (wspk * y)), (int)((wspk * i) % 900), 1] - (int)cien;
                    if (k5 < 0)
                        k5 = 0;
                    int k6 = textures[tab_color[i]][(int)(450 + (wspk * y)), (int)((wspk * i) % 900), 2] - (int)cien;
                    if (k6 < 0)
                        k6 = 0;
                    temp1[450 - y, i, 0] = (byte)k1;

                    temp1[450 - y, i, 1] = (byte)k2;

                    temp1[450-y, i, 2] = (byte)k3;
                    temp1[450 + y, i, 0] = (byte)k4;

                    temp1[450 + y, i, 1] = (byte)k5;

                    temp1[450 + y, i, 2] = (byte)k6;






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
            {    if(e.Y==0&&e.Y<mouse_last.Y)
                {
                    Cursor.Position = new Point(e.X, 1080);

                }
                if (e.Y == 1080 && e.Y > mouse_last.Y)
                {
                    Cursor.Position = new Point(e.X, 0);

                }
                if (e.X == 1920&& e.X > mouse_last.X)
                {
                    Cursor.Position = new Point(0, e.Y);
                }
                if (e.X == 0&& e.X < mouse_last.X)
                {
                    Cursor.Position = new Point(1920, e.Y);
                }


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

                
                mouse_last = new Point(e.X,e.Y);
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
                        if (temp1[cp.Y, cp.X, 0] == 0xFF&& temp1[cp.Y, cp.X, 1] == 0xFF&& temp1[cp.Y, cp.X, 2] == 0xFF)
                        {
                            //CvInvoke.Line(tmp_m, start, cp, kolor_promienia, 1);
                            promienie[p] = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                            //Debug.WriteLine(promienie[p]);
                            
                        }else if(temp1[cp.Y, cp.X, 0] != 0x00)
                        {
                            tab_color[p] = Convert.ToByte(temp1[cp.Y, cp.X, 0] /10);
                           
                            break;
                        }else
                        {
                            tab_color[p] = 0;

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
