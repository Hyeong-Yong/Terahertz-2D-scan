using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Paix_MotionController.NMC2;

namespace THz_2D_scan
{
    public class Scan
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

        NMCAXESEXPR NmcData;

        public void scan(double start_x, double end_x, double interval_x, double interval_y, double start_y, double end_y)
        {

            double current_position_x = start_x;
            double current_position_y = start_y;

            int state=1;
            
            bool ret;

            // Go to start position
            if (start_x < 0 & end_x > 300 & start_y < 0 & end_y > 300)
            {
                Console.WriteLine("Beyond position limit");
                return;
            }

            PaixMotion.HomeMove(0, 2, 0xF, 0); // Go to Home x-position
            PaixMotion.HomeMove(1, 2, 0xF, 0); // Go to Home y-position

            BusyCheckAll(2);

            System.Threading.Thread.Sleep(2000);


            // 2D scanning logic
            while (current_position_x != end_x || current_position_y != end_y) // 끝점에 도달하면 종료
            {
                // 상태머신 구현 하자
                switch (state) {
                    case 1: //"Init"
                        
                        PaixMotion.AbsMove(0, start_x);
                        PaixMotion.AbsMove(1, start_y);

                        BusyCheckAll(2);


                        // Trigger On
                        ret = PaixMotion.SetTriggerIO(0, 1, 1, 0, 1);
                        // Trigger interval(x-axis)
                        ret &= PaixMotion.TriggerOutLineScan(0, start_x, end_x, interval_x, 0); // TODO: 끝 지점에서 트리거 되는지 확인
                        
                        if (ret == true)
                        {
                            state = 2;
                        }
                        else
                        {
                            Console.WriteLine("Error, case 1: Init error");

                            //Trigger off
                            PaixMotion.TriggerOutStop(0);

                            //Go back to the Home 
                            PaixMotion.HomeMove(0, 2, 0xF, 0); // Go to Home x-position
                            PaixMotion.HomeMove(1, 2, 0xF, 0); // Go to Home y-position

                            return;

                        }
                        break;
                    case 2: // "x_start => x_end"
                        PaixMotion.AbsMove(0, end_x);
                        ret = BusyCheckAxis(0);

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
                        
                        PaixMotion.GetStateInfo();
                        if (PaixMotion.GetNmcStatus(ref NmcData) == false)
                            return;
                        current_position_x = NmcData.dCmd[0];
                        current_position_y = NmcData.dCmd[1];


                        current_position_y += interval_y;
                        PaixMotion.AbsMove(1, current_position_y);
                        ret = BusyCheckAxis(1);
                        

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
                        PaixMotion.AbsMove(0, start_x);
                        ret = BusyCheckAxis(0);


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

            }


            //Trigger off
            PaixMotion.TriggerOutStop(0);

            //Go back to the Home 
            PaixMotion.HomeMove(0, 2, 0xF, 0); // Go to Home x-position
            PaixMotion.HomeMove(1, 2, 0xF, 0); // Go to Home y-position


        }

        private bool BusyCheckAll(int totalaxis)
        {
            short[] nBusyStatus= new short[totalaxis];

            foreach (int i in nBusyStatus) {
                nBusyStatus[i] = 1;
            }
            bool bAllMoving = true;
            while (bAllMoving)
            {
                PaixMotion.GetBusyAll(nBusyStatus);
                bAllMoving = false;
                foreach (int i in nBusyStatus) 
                {
                    if (nBusyStatus[i] == 1) bAllMoving = true;
                }
                System.Threading.Thread.Sleep(1);

            }

            return true;
        }

        private bool BusyCheckAxis(short axis)
        {
            short isBusy = 1;
            while (isBusy ==1)
            {
                PaixMotion.GetBusy(axis, out isBusy);
                System.Threading.Thread.Sleep(1);
            }

            return true;
        }





    }
}
