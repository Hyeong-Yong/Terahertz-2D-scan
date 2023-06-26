using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace THz_2D_scan
{
    public class Two_D_scan
    {
        PaixMotion PaixMotion = PaixMotion.GetInstance;

        public struct SCANRANGE { 
        double start_x;
        double start_y;
        double end_x;
        double end_y;
        double interval_x;
        double interval_y;
        } ;

        SCANRANGE scan_range;

        public void scan(double start_x, double end_x, double interval_x, double interval_y, double start_y, double end_y)
        {

            double current_position_x = start_x;
            double current_position_y = start_y;

            int state=1;
            
            short isBusy = 0; // 1: 이동완료, 2: 이동 중
            bool ret;

            // Go to start position
            if (start_x < 0 & end_x > 30 & start_y < 0 & end_y > 30)
            {
                Console.WriteLine("Beyond position limit");
                return;
            }
            PaixMotion.HomeMove(0, 2, 0xF, 0); // Go to Home x-position
            PaixMotion.HomeMove(1, 2, 0xF, 0); // Go to Home y-position

            // 2D scanning logic
            while (current_position_x != end_x | current_position_y != end_y)
            {
                // 상태머신 구현 하자
                switch (state) {
                    case 1: //"Init"
                        
                        do{ PaixMotion.GetBusy(0, out isBusy);
                        } while (isBusy == 1);

                        ret = PaixMotion.AbsMove(0, start_x);
                        ret &= PaixMotion.AbsMove(0, start_y);

                        do
                        {
                            PaixMotion.GetBusy(0, out isBusy);
                        } while (isBusy == 1);

                        // Trigger On
                        ret &= PaixMotion.SetTriggerIO(0, 1, 1, 0, 1);
                        // Trigger interval(x-axis)
                        ret &= PaixMotion.TriggerOutLineScan(0, start_x, end_x, interval_x, 0); // TODO: 끝 지점에서 트리거 되는지 확인
                        
                        if (ret == true)
                        {
                            state = 2;
                        }
                        else
                        {
                            Console.WriteLine("Error, case 1: Init error");
                            state = 0;
                        }
                        break;
                    case 2: // "x_start => x_end"
                        ret = PaixMotion.AbsMove(0, end_x);
                        do
                        {
                            ret &= PaixMotion.GetBusy(0, out isBusy);
                        } while (isBusy == 1);

                        if (ret ==true)
                        {
                            state = 3;
                        }
                        else
                        {
                            Console.WriteLine("Error, case 2: x_start => x_end");
                        }
                        
                        break;
                    case 3:// "y => y + y_interval"
                        ret = PaixMotion.AbsMove(1, current_position_y);
                        current_position_y += interval_y;
                        do
                        {
                            ret &=PaixMotion.GetBusy(0, out isBusy);
                        } while (isBusy == 1);

                        if (current_position_x == end_x)
                        {
                            state = 4;
                        }
                        else if (current_position_x == start_x){
                            state = 2;
                        }
                        else
                        {
                            Console.WriteLine("Error, case 3: y => y+y_interval");
                        }

                        break;
                    case 4: // "x_end => x_start"
                        ret = PaixMotion.AbsMove(0, start_x);
                        do
                        {
                            ret &= PaixMotion.GetBusy(0, out isBusy);
                        } while (isBusy == 1);

                        if (ret == true)
                        {
                            state = 3;
                        }
                        else
                        {
                            Console.WriteLine("Error, case 4: x_end => x_start");
                        }

                        break;
                    default: 
                        break;
                }

                //Trigger off
                PaixMotion.TriggerOutStop(0);

                //Go back to the Home 
                PaixMotion.HomeMove(0, 2, 0xF, 0); // Go to Home x-position
                PaixMotion.HomeMove(1, 2, 0xF, 0); // Go to Home y-position

            }

        }
    }
}
