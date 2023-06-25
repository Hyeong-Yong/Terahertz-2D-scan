using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THz_2D_scan
{
    public class Two_D_scan
    {
        PaixMotion PaixMotion = PaixMotion.getInstance;


        public void scan()
            {
            bool Ret = PaixMotion.AbsMove(0, Convert.ToDouble(11) );

            }
    }
}
