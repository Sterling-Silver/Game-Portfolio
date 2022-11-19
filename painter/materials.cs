using System;

namespace painter
{
    public class material //parent material template
    {
        public string name = "empty";
        public int variation = 0;
        public int colorR = 0;//primary color values
        public int colorG = 0;//primary color values
        public int colorB = 0;//primary color values
        public bool acted = false;
        public bool colidable = false;
        public bool boinkable = false;
        public bool broken = false;

        public virtual bool update(int x, int y, bool fire) { return false; }
        public material() { }
        public material(int i, int j, int k) { colorR = i; colorB = j; colorG = k; }
        public virtual material clone() { return new material(colorR,colorB,colorG); }
    }
    public class stone : material
    {
        public stone()
        {
            name = "stone";
            colorR = 127;
            colorG = 127;
            colorB = 127;
            variation = 30;
            colidable = true;
            boinkable = true;
        }
        public override material clone()
        {
            return new stone();
        }
        public override bool update(int x, int y, bool fire)
        {
            return false;
        }
    }

    public class cloud : material
    {
        public cloud()
        {
            name = "stone";
            colorR = 255;
            colorG = 255;
            colorB = 255;
            variation = 30;
            colidable = true;
            boinkable = true;
        }
        public override material clone()
        {
            return new cloud();
        }
        public override bool update(int x, int y, bool fire)
        {
            return false;
        }
    }

    public class kavecrystal : material
    {
        public kavecrystal()
        {
            name = "kavecrystal";
            colorR = 191;
            colorG = 9;
            colorB = 236;
            variation = 30;
            colidable = true;
            boinkable = true;
        }
        public override material clone()
        {
            return new kavecrystal();
        }
        public override bool update(int x, int y, bool fire)
        {
            return false;
        }
    }
    public class wood : material
    {
        public wood()
        {
            name = "wood";
            colorR = 161;
            colorG = 123;
            colorB = 86;
            variation = 30;
            colidable = true;
            boinkable = true;
        }
        public override material clone()
        {
            return new wood();
        }
        public override bool update(int x, int y, bool fire)
        {
            return false;
        }
    }

    public class grass : material
    {
        public grass()
        {
            name = "grass";
            colorR = 30;
            colorG = 150;
            colorB = 30;
            variation = 30;
            colidable = true;
            boinkable = true;
        }
        public override material clone()
        {
            return new grass();
        }
        public override bool update(int x, int y, bool fire)
        {
            return false;
        }
    }
    public class sand : material
    {
        public sand()
        {
            name = "sand";
            colorR = 160;
            colorG = 150;
            colorB = 0;
            variation = 80;
            colidable = false;
            boinkable = true;

        }
        public override material clone()
        {
            return new sand();
        }
        public override bool update(int x, int y, bool fire)
        {
            if (y < Form1.height - 1 && y > 0 && x < Form1.width - 1 && x > 0 && acted == fire)
            {
                material m = Form1.matmap[x, y];
                if (Form1.matmap[x, y + 1].name == "empty" || Form1.matmap[x, y + 1].name == "water" && acted == fire) 
                {
                    Form1.matmap[x, y] = Form1.matmap[x, y + 1]; //directly bellow
                    Form1.matmap[x, y + 1] = m;
                }
                else if (Form1.matmap[x + 1, y + 1].name == "empty" || Form1.matmap[x + 1, y + 1].name == "water" && acted == fire) 
                {
                    Form1.matmap[x, y] = Form1.matmap[x + 1, y + 1]; //bellow and to the right
                    Form1.matmap[x + 1, y + 1] = m;
                }
                else if (Form1.matmap[x - 1, y + 1].name == "empty" || Form1.matmap[x - 1, y + 1].name == "water" && acted == fire) 
                {
                    Form1.matmap[x, y] = Form1.matmap[x - 1, y + 1]; //bellow and to the left
                    Form1.matmap[x - 1, y + 1] = m;
                }
                acted = !fire;
            }
            return true;
        }
    }
    public class water : material
    {
        public water()
        {
            name = "water";
            colorR = 0;
            colorG = 150;
            colorB = 250;
            variation = 80;
            colidable = false;
            boinkable = false;
        }
        public override material clone()
        {
            return new water();
        }
        public override bool update(int x, int y, bool fire)
        {
            if (y < Form1.height - 1 && y > 0 && x < Form1.width - 1 && x > 0 && acted == fire)
            {
                material m = Form1.matmap[x, y];
                if (Form1.matmap[x, y + 1].name == "empty")
                {
                    Form1.matmap[x, y] = Form1.matmap[x, y + 1];
                    Form1.matmap[x, y + 1] = m;
                }
                else if (Form1.matmap[x - 1, y + 1].name == "empty")
                {
                    Form1.matmap[x, y] = Form1.matmap[x - 1, y + 1]; // to the left and down
                    Form1.matmap[x - 1, y + 1] = m;
                }
                else if (Form1.matmap[x + 1, y + 1].name == "empty")
                {
                    Form1.matmap[x, y] = Form1.matmap[x + 1, y + 1]; //to the right and down
                    Form1.matmap[x + 1, y + 1] = m;
                }
                else if (!(Form1.matmap[x + 1, y].name == "empty" && acted && Form1.matmap[x - 1, y].name == "empty"))
                {
                    if (Form1.matmap[x + 1, y].name == "empty")
                    {
                        Form1.matmap[x, y] = Form1.matmap[x + 1, y]; //to the right
                        Form1.matmap[x + 1, y] = m;
                    }
                    else if (Form1.matmap[x - 1, y].name == "empty")
                    {
                        Form1.matmap[x, y] = Form1.matmap[x - 1, y]; // to the left
                        Form1.matmap[x - 1, y] = m;
                    }
                }
                
                acted = !fire;
            }
            return true;
        }
    } // watter is weird
    public class fire1 : material
    {
        public fire1()
        {
            name = "fire1";
            colorR = 200;
            colorG = 28;
            colorB = 36;
            variation = 20;
            colidable = false;

        }

        public override material clone()
        {
            
            return new fire1();
        }
        public override bool update(int x, int y, bool fire)
        {
                return false;
        }
    }
    public class fire2 : material
    {
        int radius;
        public fire2()
        {
            name = "fire2";
            colorR = 242;
            colorG = 101;
            colorB = 34;
            variation = 20;
            colidable = false;
            radius = 50;

        }
        public override material clone()
        {

            return new fire2();
        }
        public override bool update(int x, int y, bool fire)
        {
            if (y < Form1.height - 1 && y > 0 && x < Form1.width - 1 && x > 0 && acted == fire)
            {
                material m = Form1.matmap[x, y];
                if (Form1.matmap[x, y + 1].name == "empty")
                {
                    Form1.matmap[x, y] = Form1.matmap[x, y + 1];
                    Form1.matmap[x, y + 1] = m;
                }
                else if (Form1.matmap[x - 1, y + 1].name == "empty")
                {
                    Form1.matmap[x, y] = Form1.matmap[x - 1, y + 1]; // to the left and down
                    Form1.matmap[x - 1, y + 1] = m;
                }
                else if (Form1.matmap[x + 1, y + 1].name == "empty")
                {
                    Form1.matmap[x, y] = Form1.matmap[x + 1, y + 1]; //to the right and down
                    Form1.matmap[x + 1, y + 1] = m;
                }
                else if (!(Form1.matmap[x + 1, y].name == "empty" && acted && Form1.matmap[x - 1, y].name == "empty"))
                {
                    if (Form1.matmap[x + 1, y].name == "empty")
                    {
                        Form1.matmap[x, y] = Form1.matmap[x + 1, y]; //to the right
                        Form1.matmap[x + 1, y] = m;
                    }
                    else if (Form1.matmap[x - 1, y].name == "empty")
                    {
                        Form1.matmap[x, y] = Form1.matmap[x - 1, y]; // to the left
                        Form1.matmap[x - 1, y] = m;
                    }
                }

                acted = !fire;
            }
            return true;
        }
    }
    public class fire3 : material
    {
        int radius;

        public fire3()
        {
            name = "fire3";
            colorR = 255;
            colorG = 0;
            colorB = 0;
            variation = 20;
            colidable = false;
            boinkable = false;

        }
        public override material clone()
        {

            return new fire3();
        }
        public override bool update(int x, int y, bool fire)
        {
            if (y < Form1.height - 1 && y > 0 && x < Form1.width - 1 && x > 0 && acted != fire && broken)
            {
                if (radius != 0)
                {
                    if (fire == true)
                    {
                        Form1.matmap[x + 1, y + 1] = new fire3();Form1.matmap[x + 1, y + 1].broken = true;
                        Form1.matmap[x - 1, y + 1] = new fire3();Form1.matmap[x - 1, y + 1].broken = true;
                        Form1.matmap[x + 1, y - 1] = new fire3();Form1.matmap[x + 1, y - 1].broken = true;
                        Form1.matmap[x - 1, y - 1] = new fire3();Form1.matmap[x - 1, y - 1].broken = true;
                    }
                    else
                    {
                        Form1.matmap[x, y + 1] = new fire3();Form1.matmap[x, y + 1].broken = true;
                        Form1.matmap[x, y + 1] = new fire3();Form1.matmap[x, y + 1].broken = true;
                        Form1.matmap[x + 1, y] = new fire3();Form1.matmap[x + 1, y].broken = true;
                        Form1.matmap[x - 1, y] = new fire3();Form1.matmap[x - 1, y].broken = true;
                    }
                }
                acted = fire;
            }
            return false;
        }
    }
    public class crystal : material
    {
        public crystal()
        {
            name = "crystal";
            colorR = 255;
            colorG = 0;
            colorB = 255;
            variation = 20;
            colidable = false;
            boinkable = false;
        }

        public override material clone()
        {

            return new crystal();
        }
        public override bool update(int x, int y, bool fire)
        {
            return false;
        }
    }
    public class sandStone : stone
    {
        public sandStone()
        {
            name = "sandStone";
            colorR = 224;
            colorG = 192;
            colorB = 0;
            variation = 30;
            colidable = true;
            boinkable = true;
        }
        public override material clone()
        {
            return new sandStone();
        }
        public override bool update(int x, int y, bool fire)
        {
            return false;
        }
    }
    public class breakable : material
    {
        public breakable()
        {
            name = "breakable";
            colorR = 64;
            colorG = 32;
            colorB = 0;
            variation = 10;
            colidable = true;
            boinkable = false;
            broken = false;
        }
        public override material clone()
        {
            return new breakable();
        }
        public override bool update(int x, int y, bool fire)
        {
            if (y < Form1.height - 1 && y > 0 && x < Form1.width - 1 && x > 0 && acted == fire && broken)
            {
                material m = Form1.matmap[x, y];
                if (Form1.matmap[x, y + 1].name == "empty" || Form1.matmap[x, y + 1].name == "water" && acted == fire)
                {
                    Form1.matmap[x, y] = Form1.matmap[x, y + 1]; //directly bellow
                    Form1.matmap[x, y + 1] = m;
                }
                else if (Form1.matmap[x + 1, y + 1].name == "empty" || Form1.matmap[x + 1, y + 1].name == "water" && acted == fire)
                {
                    Form1.matmap[x, y] = Form1.matmap[x + 1, y + 1]; //bellow and to the right
                    Form1.matmap[x + 1, y + 1] = m;
                }
                else if (Form1.matmap[x - 1, y + 1].name == "empty" || Form1.matmap[x - 1, y + 1].name == "water" && acted == fire)
                {
                    Form1.matmap[x, y] = Form1.matmap[x - 1, y + 1]; //bellow and to the left
                    Form1.matmap[x - 1, y + 1] = m;
                }
                acted = !fire;
            }
            return true;

        }
    }



    public class glass : material
    {
        public glass()
        {
            name = "glass";
            colorR = 63;
            colorG = 191;
            colorB = 187;
            variation = 4;
            colidable = true;
            boinkable = false;
            broken = false;
        }
        public override material clone()
        {
            return new glass();
        }
        public override bool update(int x, int y, bool fire)
        {
            if (y < Form1.height - 1 && y > 0 && x < Form1.width - 1 && x > 0 && acted == fire && broken)
            {
                material m = Form1.matmap[x, y];
                if (Form1.matmap[x, y + 1].name == "empty" || Form1.matmap[x, y + 1].name == "water" && acted == fire)
                {
                    Form1.matmap[x, y] = Form1.matmap[x, y + 1]; //directly bellow
                    Form1.matmap[x, y + 1] = m;
                }
                else if (Form1.matmap[x + 1, y + 1].name == "empty" || Form1.matmap[x + 1, y + 1].name == "water" && acted == fire)
                {
                    Form1.matmap[x, y] = Form1.matmap[x + 1, y + 1]; //bellow and to the right
                    Form1.matmap[x + 1, y + 1] = m;
                }
                else if (Form1.matmap[x - 1, y + 1].name == "empty" || Form1.matmap[x - 1, y + 1].name == "water" && acted == fire)
                {
                    Form1.matmap[x, y] = Form1.matmap[x - 1, y + 1]; //bellow and to the left
                    Form1.matmap[x - 1, y + 1] = m;
                }
                acted = !fire;
            }
            return false;
        }
    }
    public class moonStone : material
    {
        public moonStone()
        {
            name = "moonStone";
            colorR = 255;
            colorG = 247;
            colorB = 119;
            variation = 10;
            colidable = true;
            boinkable = true;
        }
        public override material clone()
        {
            return new moonStone();
        }
        public override bool update(int x, int y, bool fire)
        {
            return false;
        }
    }
}