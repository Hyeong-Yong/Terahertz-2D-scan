using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THz_2D_scan
{
    public class Two_D_scan
    {
        PaixMotion PaixMotion = PaixMotion.getInstance;

        public struct SCANRANGE { 
        double start_x;
        double start_y;
        double end_x;
        double end_y;
        double interval_x;
        double interval_y;
        };

        public void scan(double start_x, double end_x, double interval_x, double start_y, double end_y)
        {
            // Home go
            PaixMotion.HomeMove(0, 2, 0xF, 0);
            PaixMotion.HomeMove(1, 2, 0xF, 0);

            // Trigger On
            PaixMotion.SetTriggerIO(0, 1, 1, 0, 1);

            // Trigger interval(x-axis)
            // x_distance  
            // y_distance_interval++
            // until y_distance_end 
            // 
                PaixMotion.AbsMove(0, x_dist);
            

            // Trigger Off
            //
        }
    }
}
