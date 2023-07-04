using System;
using static Paix_MotionController.NMC2;

namespace THz_2D_scan
{
    public class Scan
    {
        private enum motionState
        {
            Initialization,
            X_StartToEnd,
            Y_StepIncrease,
            X_EndToStart,
        }

        PaixMotion PaixMotion = PaixMotion.GetInstance;
        NMCAXESEXPR NmcData;
        bool ret;


        private bool stop_flag = true;
        public bool Stop_flag { get => stop_flag; set => stop_flag = value; }


        /// <summary>
        /// 2D Scanning
        /// </summary>
        /// <param name="start_x">X-시작지점</param>
        /// <param name="end_x">X-종료지점</param>
        /// <param name="interval_x">트리거 간격</param>
        /// <param name="interval_y">Y축 이동간격</param>
        /// <param name="start_y">Y축-시작지점</param>
        /// <param name="end_y">Y축-종료지점</param>
        public void Run(object param)
        {
            double[] value = param as double[];
             double start_x = value[0];
             double end_x = value[1];
             double start_y = value[2];
             double end_y = value[3];
             double interval_x = value[4];
             double interval_y = value[5];

            // Initialization
            motionState state = motionState.Initialization;
            stop_flag = true;

            // 예외처리
            if (start_x < 0 & end_x > 300 & start_y < 0 & end_y > 300)
            {
                Console.WriteLine("Beyond position limit");
                return;
            }

            // 시작 시, 홈으로 이동
            PaixMotion.HomeMove(0, 2, 0xF, 0); // Go to Home x-position
            PaixMotion.HomeMove(1, 2, 0xF, 0); // Go to Home y-position
            BusyCheckAll(2);


            double current_position_x = start_x;
            double current_position_y = start_y;

            System.Threading.Thread.Sleep(2000); // 시작전 1초 간격 두기 (없어도 됨)


            // 2D scanning logic
            while (current_position_x != end_x || current_position_y != end_y && stop_flag) // 끝점에 도달하면 종료
            {
                // 상태머신 구현 하자
                switch (state) {
                    case motionState.Initialization: //"Init"
                        
                        PaixMotion.AbsMove(0, start_x);
                        PaixMotion.AbsMove(1, start_y);

                        BusyCheckAll(2);

                        PaixMotion.GetStateInfo();
                        if (PaixMotion.GetNmcStatus(ref NmcData) == false)
                            return;
                        current_position_x = NmcData.dCmd[0];
                        current_position_y = NmcData.dCmd[1];


                        // Trigger On
                        ret = PaixMotion.SetTriggerIO(0, 1, 1, 0, 1);
                        // Trigger interval(x-axis)
                        ret &= PaixMotion.TriggerOutLineScan(0, start_x, end_x, interval_x, 0); // TODO: 끝 지점에서 트리거 되는지 확인
                        
                        if (ret == true)
                        {
                            state = motionState.X_StartToEnd;
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
                    case motionState.X_StartToEnd: // "x_start => x_end"
                        PaixMotion.AbsMove(0, end_x);
                        ret = BusyCheckAxis(0);

                        if (ret ==true)
                        {
                            state = motionState.Y_StepIncrease;
                        }
                        else
                        {
                            Console.WriteLine("Error, case 2: x_start => x_end");
                        }

                        PaixMotion.GetStateInfo();
                        if (PaixMotion.GetNmcStatus(ref NmcData) == false)
                            return;
                        current_position_x = NmcData.dCmd[0];
                        current_position_y = NmcData.dCmd[1];

                        if (current_position_x == end_x &&  current_position_y == end_y) {
                            stop_flag= false;
                        }

                        break;
                    case motionState.Y_StepIncrease:// "y => y + y_interval"
                        
                        PaixMotion.GetStateInfo();
                        if (PaixMotion.GetNmcStatus(ref NmcData) == false)
                            return;
                        current_position_x = NmcData.dCmd[0];
                        current_position_y = NmcData.dCmd[1];


                        current_position_y += interval_y;
                        PaixMotion.AbsMove(1, current_position_y);
                        BusyCheckAxis(1);
                        
                        if (current_position_x == end_x)
                        {
                            state = motionState.X_EndToStart;
                        }
                        else if (current_position_x == start_x){
                            state = motionState.X_StartToEnd;
                        }
                        else
                        {
                            Console.WriteLine("Error, case 3: y => y+y_interval");
                        }

                        break;
                    case motionState.X_EndToStart: // "x_end => x_start"
                        PaixMotion.AbsMove(0, start_x);
                        ret = BusyCheckAxis(0);


                        if (ret == true)
                        {
                            state = motionState.Y_StepIncrease;
                        }
                        else
                        {
                            Console.WriteLine("Error, case 4: x_end => x_start");
                        }

                        PaixMotion.GetStateInfo();
                        if (PaixMotion.GetNmcStatus(ref NmcData) == false)
                            return;
                        current_position_x = NmcData.dCmd[0];
                        current_position_y = NmcData.dCmd[1];

                        if (current_position_x == end_x && current_position_y == end_y)
                        {
                            stop_flag = false;
                        }

                        break;
                    default: 
                        break;
                }

            }


            //Trigger off
            PaixMotion.TriggerOutStop(0);
            System.Threading.Thread.Sleep(100);

            //Go back to the Home 
            PaixMotion.HomeMove(0, 2, 0xF, 0); // Go to Home x-position
            PaixMotion.HomeMove(1, 2, 0xF, 0); // Go to Home y-position
            BusyCheckAll(2);


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
