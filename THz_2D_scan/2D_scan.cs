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
        public bool keepScan;
        double current_position_x;
        double current_position_y;
        double start_x;
        double end_x;
        double start_y;
        double end_y;
        double interval_x;
        double interval_y;

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
            /****************************** GUI에서 시작, 끝, 트리거 간격 불러오기 ************************/
             double[] value = param as double[];
             start_x = value[0];
             end_x = value[1];
             start_y = value[2];
             end_y = value[3];
             interval_x = value[4];
             interval_y = value[5];
            // 서보모터 최대 제한 범위 넘을시, 실행 안함
            if (start_x < 0 & end_x > 300 & start_y < 0 & end_y > 300)
            {
                Console.WriteLine("Beyond position limit");
                return;
            }


            /****************************** 초기화 ******************************************/
            motionState state = motionState.Initialization;
            keepScan = true;

            // 시작 시, 홈으로 이동
            PaixMotion.HomeMove(0, 2, 0xF, 0); // Go to Home x-position
            PaixMotion.HomeMove(1, 2, 0xF, 0); // Go to Home y-position
            BusyCheckAll(2);


            System.Threading.Thread.Sleep(1000); // 시작전 1초 간격 두기 (없어도 됨)
            
            /****************************** 2D scan 실행 ********************************/
            while (keepScan) // 끝점에 도달하면 종료
            {
                // 상태머신 구현 하자
                switch (state) {
                    case motionState.Initialization: //"Init"
                        PaixMotion.AbsMove(0, start_x);
                        PaixMotion.AbsMove(1, start_y);
                        BusyCheckAll(2);

                        PaixMotion.GetStateInfo();
                        if (PaixMotion.GetNmcStatus(ref NmcData) == false) { 
                            return;
                        }
                        current_position_x = updatePosition("x");
                        current_position_y = updatePosition("y");

                        // Trigger On
                        ret = PaixMotion.SetTriggerIO(0, 1, 1, 0, 1);
                        // Trigger interval(x-axis)
                        ret &= PaixMotion.TriggerOutLineScan(0, start_x, end_x, interval_x, 0); // TODO: 끝 지점에서 트리거 되는지 확인

                        // Go to next "x_start => x_end" step 
                        if (ret == true){
                            state = motionState.X_StartToEnd;
                        }else // Error
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

                        PaixMotion.GetStateInfo();
                        if (PaixMotion.GetNmcStatus(ref NmcData) == false) { 
                            return;
                        }
                        current_position_x = updatePosition("x");
                        current_position_y = updatePosition("y");

                        if (Check_LastPosition()) {
                            ScanFinished();
                        }

                        if (ret == true)
                        {
                            state = motionState.Y_StepIncrease;
                        }
                        else
                        {
                            Console.WriteLine("Error, case 2: x_start => x_end");
                        }
                        break;

                    case motionState.Y_StepIncrease:// "y => y + y_interval"
                        current_position_y += interval_y;
                        PaixMotion.AbsMove(1, current_position_y);
                        BusyCheckAxis(1);

                        PaixMotion.GetStateInfo();
                        if (PaixMotion.GetNmcStatus(ref NmcData) == false) { 
                            return;
                        }
                        current_position_x = updatePosition("x");
                        current_position_y = updatePosition("y");


                        if (Check_LastPosition())
                        {
                            ScanFinished();
                        }

                        if (current_position_x == end_x){
                            state = motionState.X_EndToStart;
                        }
                        else if (current_position_x == start_x){
                            state = motionState.X_StartToEnd;
                        }
                        else{
                            Console.WriteLine("Error, case 3: y => y+y_interval");
                        }
                        break;

                    case motionState.X_EndToStart: // "x_end => x_start"
                        PaixMotion.AbsMove(0, start_x);
                        ret = BusyCheckAxis(0);

                        if (ret == true){
                            state = motionState.Y_StepIncrease;
                        }
                        else{
                            Console.WriteLine("Error, case 4: x_end => x_start");
                        }

                        PaixMotion.GetStateInfo();
                        if (PaixMotion.GetNmcStatus(ref NmcData) == false) { 
                            return;
                        }
                        current_position_x = updatePosition("x");
                        current_position_y = updatePosition("y");

                        if (Check_LastPosition()){
                            ScanFinished();
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

        private double updatePosition(string axis)
        {
            double current_position;
            if (axis == "x") { 
                current_position = NmcData.dCmd[0];
            }
            else if (axis == "y")
            {
                current_position = NmcData.dCmd[1];
            }
            else
            {
                current_position = NmcData.dCmd[2];
            }

            return current_position;
        }

        private bool Check_LastPosition()
        {
            bool ret = (current_position_x == end_x && current_position_y == end_y);
            return ret;
        }
        private void ScanFinished()
        {
            keepScan = false;
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
