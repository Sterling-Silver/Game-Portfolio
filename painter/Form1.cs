using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;


namespace painter
{
    public partial class Form1 : Form
    {

        bool playerJumped = false;
        int walkspeed = 2;
        int jumpheight = 5;
        double jumpspeed = 0;
        double gravity = 0.5;

        int dash = 15;
        int dashdist = 0;
        int dashspeed = 5;
        public bool DashUp = false;
        public bool DashDown = false;
        public bool DashLeft = false;
        public bool DashRight = false;



        int playerShmoveX = 0;
        int playerShmoveY = 0;


        public bool up;
        public bool down;
        public bool left;
        public bool right;
        private void keyup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) { left = false; }
            if (e.KeyCode == Keys.D) { right = false; }
            if (e.KeyCode == Keys.W) { up = false; }
            if (e.KeyCode == Keys.S) { down = false; }
        }
        private void keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) { left = true; }
            if (e.KeyCode == Keys.D) { right = true; }
            if (e.KeyCode == Keys.W) { up = true; }
            if (e.KeyCode == Keys.S) { down = true; }
        }

        Random rand = new Random(123);
        public static int width = 0;
        public static int height = 0;
        public int pixels_per_pixel = 5;

        public bool fire = true;

        public int WorldHeight;
        public int WorldWidth;

        public int camX = 100;
        public int camY = 1800;
        public int worldX;
        public int worldY;
        public static material[,] loaded;
        public static material[,] loadedBkgrnd;

        public static material[,] matmap; //this is the material map of the screen
        public static material[,] matmapBkgrnd; //this is the material map of the screen
        public DirectBitmap bmp;//un-safe death mega var
        public DirectBitmap bmpBkgrnd;

        public static List<material> color_chart = new List<material>();

        public Form1()//general init
        {
            InitializeComponent();

            //FormBorderStyle = FormBorderStyle.None;
            //WindowState = FormWindowState.Maximized;

            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
            pictureBox2.Width = this.Width;
            pictureBox2.Height = this.Height;

            width = pictureBox1.Width / pixels_per_pixel;
            height = pictureBox1.Height / pixels_per_pixel;

            //player.BackColor = Color.Transparent;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox2.Controls.Add(pictureBox1);
            pictureBox2.Controls.Add(player);
            pictureBox1.Controls.Add(player);

            matmap = new material[width, height];
            matmapBkgrnd = new material[width, height];

            bmp = new DirectBitmap(width * pixels_per_pixel, height * pixels_per_pixel);
            bmpBkgrnd = new DirectBitmap(width * pixels_per_pixel, height * pixels_per_pixel);
            //entiies



            //materials
            color_chart.Add(new material());

            color_chart.Add(new sand());
            color_chart.Add(new stone());
            color_chart.Add(new cloud());
            color_chart.Add(new water());
            color_chart.Add(new grass());
            color_chart.Add(new wood());
            color_chart.Add(new fire1());
            color_chart.Add(new fire2());
            color_chart.Add(new fire3());

            color_chart.Add(new sandStone());
            color_chart.Add(new crystal());
            color_chart.Add(new kavecrystal());
            color_chart.Add(new breakable());
            color_chart.Add(new glass());
            color_chart.Add(new moonStone());


            initalise_loaded();
            load();
            loadBkgrnd();
            populate_matmap();
        }
        public void loadBkgrnd()
        {
            DirectBitmap Disposable = new DirectBitmap(width * pixels_per_pixel, height * pixels_per_pixel);
            Disposable.Bitmap = read("Background");
            WorldHeight = Disposable.Bitmap.Height;
            WorldWidth = Disposable.Bitmap.Width;
            loadedBkgrnd = new material[WorldWidth, WorldHeight];
            for (int i = 0; i < WorldWidth - 2; ++i)
            {
                for (int j = 0; j < WorldHeight - 2; ++j)
                {
                    material m = new material();
                    m.colorR = Disposable.Bitmap.GetPixel(i, j).R;
                    m.colorG = Disposable.Bitmap.GetPixel(i, j).G;
                    m.colorB = Disposable.Bitmap.GetPixel(i, j).B;
                    loadedBkgrnd[i, j] = m;
                }
            }
        }


        public void load()
        {
            DirectBitmap Disposable = new DirectBitmap(width * pixels_per_pixel, height * pixels_per_pixel);
            Disposable.Bitmap = read("world");
            WorldHeight = Disposable.Bitmap.Height;
            WorldWidth = Disposable.Bitmap.Width;
            loaded = new material[WorldWidth, WorldHeight];
            for (int i = 0; i < WorldWidth - 2; ++i)
            {
                for (int j = 0; j < WorldHeight - 2; ++j)
                {
                    material m = convert(Disposable.Bitmap.GetPixel(i, j));
                    loaded[i, j] = m;
                }
            }
        }
        public void initalise_loaded()
        {
            for (int i = 0; i < WorldHeight; ++i)//make loaded matmap
            {
                for (int j = 0; j < WorldWidth; ++j)
                {
                    loaded[i, j] = new material();
                    loadedBkgrnd[i, j] = new material();
                }
            }
            for (int i = 0; i < width - 1; ++i)//make schreen material maps
            {
                for (int j = 0; j < height - 1; ++j)
                {
                    matmap[i, j] = new material();
                    matmapBkgrnd[i, j] = new material();
                }
            }
        }



        public void populate_matmap()
        {
            for (int i = 0; i < width; ++i)//make loaded matmap
            {
                for (int j = 0; j < height; ++j)
                {
                    material m = loaded[i + camX, j + camY];
                    matmap[i, j] = DoVariation(m.clone());

                    m = loadedBkgrnd[i + camX, j + camY];
                    matmapBkgrnd[i, j] = DoVariation(m.clone());
                }
            }
        }

        public void shift_matmap(int x, int y)
        {
            // move entities

            playerShmoveX -= x * pixels_per_pixel;
            playerShmoveY -= y * pixels_per_pixel;
            
            // move matmap
            camX += x;
            camY += y;
            if (x > 0 || y > 0)
            {
                for (int i = 0; i < width; ++i)//move left or down
                {
                    for (int j = 0; j < height; ++j)
                    {
                        safe_load(x, y, i, j);
                    }
                }
            }
            if (x < 0 || y < 0)
            {
                for (int i = width - 1; i != -1; --i)//move up or right
                {
                    for (int j = height - 1; j != -1; --j)
                    {
                        safe_load(x, y, i, j);
                    }
                }
            }
        }
        private void safe_load(int x, int y, int i, int j)// safely ignore invalid values for loaded map aria
        {
            if (x + i <= 0 || x + i > width - 1 || y + j <= 0 || y + j > height - 1)//load pixels
            {
                if (i + camX < 0 || j + camY < 0 || i + camX > WorldWidth || j + camY > WorldHeight)
                { //is load safe
                    material m = new material();
                    matmap[i, j] = DoVariation(m.clone());
                    matmapBkgrnd[i, j] = DoVariation(m.clone());

                }
                else
                {   //ya we good
                    material m = loaded[i + camX, j + camY];
                    matmap[i, j] = DoVariation(m.clone());

                    m = loadedBkgrnd[i + camX, j + camY];
                    matmapBkgrnd[i, j] = m.clone();
                }
            }
            else //move pixels
            {
                matmap[i, j] = matmap[i + x, j + y];
                matmapBkgrnd[i, j] = matmapBkgrnd[i + x, j + y];
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();

            player.SoundLocation = "music.wav";
            player.Play();
        }

        private Bitmap read(string str)
        {

            Bitmap bmp = new Bitmap((Bitmap)Image.FromFile(str + ".bmp"));
            return bmp;
        }

        private material DoVariation(material c)
        {
            //apply variation
            c.colorR += rand.Next(0, c.variation);
            c.colorG += rand.Next(0, c.variation);
            c.colorB += rand.Next(0, c.variation);
            //limit bounds to 0-255
            if (c.colorR > 255) { c.colorR = 255; }
            if (c.colorG > 255) { c.colorG = 255; }
            if (c.colorB > 255) { c.colorB = 255; }
            if (c.colorR < 0) { c.colorR = 0; }
            if (c.colorG < 0) { c.colorG = 0; }
            if (c.colorB < 0) { c.colorB = 0; }

            return c;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            update(); //world
            update_player();
        }

        private void update_player()
        {

            // start doing collision detection for up down left and right
            int playerPixelWide = (player.Location.X + player.Width) / pixels_per_pixel;
            int playerPixelsTall = (player.Location.Y + player.Height) / pixels_per_pixel;

            int playerLocalX = player.Location.X / pixels_per_pixel;
            int playerLocalY = player.Location.Y / pixels_per_pixel;


            bool playerHitboxeLeft = false;
            bool playerHitboxeRight = false;
            bool playerHitboxeUp = false;
            bool playerHitboxeDown = false;


            bool playerDashboxeLeft = false;
            bool playerDashboxeRight = false;
            bool playerDashboxeUp = false;
            bool playerDashboxeDown = false;


            for (int x = playerLocalX; x < playerPixelWide; ++x) // get the matmap dims of X
            {
                for (int y = playerLocalY; y < playerPixelsTall; ++y) // get the matmap dims of Y
                {
                    
                    if (matmap[x, y].colidable == true && x == playerLocalX && y != playerLocalY && y != playerPixelsTall - 1) // left hitbox
                    { playerHitboxeLeft = true; if (matmap[x, y].boinkable) { playerDashboxeLeft = true; } }
                    if (matmap[x, y].colidable == true && x == playerPixelWide - 1 && y != playerLocalY && y != playerPixelsTall - 1) // right hitbox
                    { playerHitboxeRight = true; if (matmap[x, y].boinkable) { playerDashboxeRight = true; } }
                    if (matmap[x, y].colidable == true && y == playerLocalY ) // up hitbox
                    { playerHitboxeUp = true; if (matmap[x, y].boinkable) { playerDashboxeUp = true; } }
                    if (matmap[x, y].colidable == true && y == playerPixelsTall - 1 && x > playerLocalX+walkspeed && x < playerPixelWide - walkspeed) // down hitbox
                    { playerHitboxeDown = true; if (matmap[x, y].boinkable) { playerDashboxeDown = true; } }

                    if (dashdist != 0 && matmap[x, y].boinkable == false) { matmap[x, y].broken = true; matmap[x, y].colidable = false; }
                    if (matmap[x, y].name == "crystal") { 
                        if (up) { DashUp = true; dashdist = dash; }
                        if (left) { DashLeft = true; dashdist = dash; }
                        if (down) { DashDown = true; dashdist = dash; }
                        if (right) { DashRight = true; dashdist = dash; }
                    }


                }
            }


            // start player controlls
            
            if (dashdist != 0) {

                if (DashUp && !playerDashboxeUp) { shift_matmap(0, -dashspeed); playerShmoveY -= dashspeed * pixels_per_pixel; }
                if (DashDown && !playerDashboxeDown) { shift_matmap(0, dashspeed); playerShmoveY += dashspeed * pixels_per_pixel; }
                if (DashLeft && !playerDashboxeLeft) { shift_matmap(-dashspeed, 0); playerShmoveX -= dashspeed * pixels_per_pixel; }
                if (DashRight && !playerDashboxeRight) { shift_matmap(dashspeed, 0); playerShmoveX += dashspeed * pixels_per_pixel; }
                dashdist--;
                if (DashUp && playerDashboxeUp) { dashdist = 0; }
                if (DashDown && playerDashboxeDown) { dashdist = 0; }
                if (DashLeft && playerDashboxeLeft) { dashdist = 0; }
                if (DashRight && playerDashboxeRight) { dashdist = 0; }
            }
            else{
                #region player controlls

                //clr dash
                 DashUp = false; 
                 DashLeft = false; 
                 DashDown = false; 
                 DashRight = false; 


                if (left && !playerHitboxeLeft)
                {
                    shift_matmap(-walkspeed, 0);
                    playerShmoveX -= pixels_per_pixel * walkspeed;
                }
                if (right && !playerHitboxeRight)
                {
                    shift_matmap(walkspeed, 0);
                    playerShmoveX += pixels_per_pixel * walkspeed;
                }
                if (!playerHitboxeUp && up && !playerJumped)
                {
                    playerShmoveY -= pixels_per_pixel * (int)jumpspeed;
                    shift_matmap(0, -(int)jumpspeed);
                    jumpspeed = jumpheight;
                    playerJumped = true;
                }
                if (!playerHitboxeDown && jumpspeed < 0)
                {
                    shift_matmap(0, 1);
                    playerShmoveY += pixels_per_pixel;
                }
                if (!playerHitboxeUp && !playerHitboxeDown)
                {
                    playerShmoveY -= pixels_per_pixel * (int)jumpspeed;
                    shift_matmap(0, -(int)jumpspeed);
                }

                if (jumpspeed > -1) { jumpspeed -= gravity; } // decrease jump
                if (playerHitboxeDown) { playerJumped = false; } // restore jump
                if (playerHitboxeDown && (playerHitboxeLeft || playerHitboxeRight) && (left || right))
                {
                    shift_matmap(0, -1);
                    playerShmoveY -= pixels_per_pixel;
                }

                #endregion player controls
            }



            player.Location = new Point(player.Location.X + playerShmoveX, player.Location.Y + playerShmoveY);
            playerShmoveX =0;
            playerShmoveY =0;

        }

        private void update()
        {
            fire = !fire;

            for (int x = 0; x < width * pixels_per_pixel; ++x)
            {
                for (int y = 0; y < height * pixels_per_pixel; ++y)
                {
                    int mapX = (x / pixels_per_pixel);
                    int mapY = (y / pixels_per_pixel);

                    matmap[mapX, mapY].update(mapX, mapY, fire); // update the phisics for next frame if its does shit

                    Color color = Color.FromArgb( // this is sooo ineficient because it redraws all pixels even if they dont need it
                           (matmap[mapX, mapY].name == "empty") ? 0 : 255,
                           matmap[mapX, mapY].colorR, // but dont touch it or than stuff breaks
                           matmap[mapX, mapY].colorG,
                           matmap[mapX, mapY].colorB
                       );
                    Color colorbk = Color.FromArgb( // this is sooo ineficient because it redraws all pixels even if they dont need it
                           255,
                           matmapBkgrnd[mapX, mapY].colorR, // but dont touch it or than stuff breaks
                           matmapBkgrnd[mapX, mapY].colorG,
                           matmapBkgrnd[mapX, mapY].colorB
                       );


                    bmp.SetPixel(x, y, color);
                    bmpBkgrnd.SetPixel(x, y, colorbk);
                }
            }
            pictureBox1.Image = bmp.Bitmap;
            pictureBox2.Image = bmpBkgrnd.Bitmap;

        }
        private material convert(Color color)
        {
            material close_mat = new material();
            double least_color_dist = 255;
            foreach (material m in color_chart)
            {
                double colordist = Math.Sqrt(
                            (color.R - m.colorR) ^ 2 +
                            (color.G - m.colorG) ^ 2 +
                            (color.B - m.colorB) ^ 2);
                if (least_color_dist > colordist)
                {
                    close_mat = m;
                    least_color_dist = colordist;
                }

                if (color.R == m.colorR && color.B == m.colorB && color.G == m.colorG) { return m; }
            }

            return close_mat;
        }

    }

}