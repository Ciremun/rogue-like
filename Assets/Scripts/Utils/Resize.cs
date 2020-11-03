using UnityEngine;

namespace Utils
{
    public class Resize
    {
        public static void fit(ref int imgwidth, ref int imgheight, int screenwidth, int screenheight)
        {
            float ri = imgwidth / imgheight;
            float rs = screenwidth / screenheight;
            if (rs > ri)
            {
                imgwidth = imgwidth * screenheight / imgheight;
                imgheight = screenheight;
            }
            else if (rs < ri)
            {
                imgwidth = screenwidth;
                imgheight = imgheight * screenwidth / imgwidth;
            }
            else
            {
                int imagescale = screenwidth / imgwidth;
                imgwidth *= imagescale;
                imgheight *= imagescale;
            }
        }
    }
}
