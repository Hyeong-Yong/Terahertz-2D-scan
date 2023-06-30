using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Paix_MotionController;
using static Paix_MotionController.NMC2;

namespace THz_2D_scan
{
    partial class PaixMotion
    {
        /*
         * SingleTon implementation
         */
        private static PaixMotion instance = null;
        private PaixMotion(){}
        public static PaixMotion GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PaixMotion();
                }
                return instance;
            }
        }



        bool m_bIsOpen;
        short m_nDev_no;

        public const short GROUP = 0;


        public void AstekMotion()
        {
            m_bIsOpen = false;
            m_nDev_no = 11;
        }

        public bool Open(short dev_no)
        {
            m_nDev_no = dev_no;

            Close();
            NMC2.nmc_SetIPAddress(dev_no, 192, 168, 0);
            // 방화벽을 확인해 주십시요.
            if (NMC2.nmc_PingCheck(dev_no, 10) != 0)
            {
                MessageBox.Show("Ping Check Error");
                return false;
            }
            if (NMC2.nmc_OpenDevice(m_nDev_no) == 0)
                m_bIsOpen = true;
            else
                m_bIsOpen = false;

            return m_bIsOpen;
        }

        public bool Close()
        {
            NMC2.nmc_CloseDevice(m_nDev_no);
            m_bIsOpen = false;

            return true;
        }

        /// <summary>
        /// 지정한 한 축의 스텝 모터 전류 출력을 설정 
        /// "서보모터에서는 사용 안함"
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <param name="nOut"> 1: 출력 On, 2: 출력 Off</param>
        /// <returns></returns>
        public bool SetCurrentOn(short nAxisNo, short nOut)
        {
            short nRet = NMC2.nmc_SetCurrentOn(m_nDev_no, nAxisNo, nOut);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }


        /// <summary>
        /// 지정한 한 축의 Servo On/Off
        /// </summary>
        /// <param name="nAxisNo">축 번호</param>
        /// <param name="nOut">1: 출력 On, 2: 출력 Off</param>
        /// <returns></returns>
        public bool SetServoOn(short nAxisNo, short nOut)
        {
            short nRet = NMC2.nmc_SetServoOn(m_nDev_no, nAxisNo, nOut);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }


        /// <summary>
        /// 구조체 "Current, Servo, DCC, AlarmReset" 읽기 
        /// </summary>
        /// <param name="MotOut">모션 출력의 구조체의 포인터</param>
        /// <returns></returns>
        public bool GetAxesMotionOut(out NMCAXESMOTIONOUT MotOut)
        {
            short nRet = nmc_GetAxesMotionOut(m_nDev_no, out MotOut);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        /// <summary>
        /// 축의 표시 값(센서 입력 상태 및 위치 정보등)을 읽기
        /// </summary>
        /// <param name="pNmcData"> 축 상태를 받을 구조체의 포인터</param>
        /// <returns></returns>
        public bool GetAxesExpress(out NMCAXESEXPR pNmcData) {
            short nRet= nmc_GetAxesExpress(m_nDev_no, out pNmcData);
            
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }


        public bool SetSpeedPPS(short nAxis, double dStart, double dAcc, double dDec, double dMax)
        {
            short nRet = NMC2.nmc_SetSpeed(m_nDev_no, nAxis, dStart, dAcc, dDec, dMax);
            switch( nRet )
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 현재위치를 기준으로 지정된 위치만큼 상대이동을 실시
        /// </summary>
        /// <param name="nAxis"></param>
        /// <param name="fDist">이동하고자 하는 상대 위치(단위: 이동단위)</param>
        /// <returns></returns>
        public bool RelMove(short nAxis, double fDist)
        {
            short nRet = NMC2.NMC_UNKOWN;

            nRet = NMC2.nmc_RelMove(m_nDev_no, nAxis, fDist);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 개별 축의 설정된 펄스만큼 절대이동(Absolute move)을 실시
        /// </summary>
        /// <param name="nAxis"></param>
        /// <param name="fDist">이동하고자 하는 절대 위치 (단위: 이동단위)</param>
        /// <returns></returns>
        public bool AbsMove(short nAxis, double fDist)
        {
            int nRet;
  
               nRet = NMC2.nmc_AbsMove(m_nDev_no, nAxis, fDist);

               switch (nRet)
               {
                   case NMC2.NMC_NOTCONNECT:
                       m_bIsOpen = false;
                       return false;
                   case 0:
                       return true;
               }

               return false;
        }

        public bool JogMove(short nAxis, short nDir)
        {
            short nRet = NMC2.nmc_JogMove(m_nDev_no, nAxis, nDir);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        public bool SlowStop(short nAxis)
        {
            short nRet = NMC2.nmc_DecStop(m_nDev_no, nAxis);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        public bool Stop(short nAxis)
        {
            short nRet = NMC2.nmc_SuddenStop(m_nDev_no, nAxis);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        public bool RelMultiTwoMove(double[] fDist)
        { 
            short[] nAxis = {0, 1};


            short nRet = NMC2.nmc_VarRelMove(m_nDev_no, 2, nAxis, fDist);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        public bool AbsMultiTwoMove(double[] fDist)
        {
            short[] nAxis = {0,1};

            short nRet = NMC2.nmc_VarAbsMove(m_nDev_no, 2, nAxis, fDist);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        public bool SyncTwoMove(double pulse1, double pulse2, short opt)
        {
            short nRet = NMC2.nmc_Interpolation2Axis(m_nDev_no, 0, pulse1, 1, pulse2, opt);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        /// <summary>
        /// 축의 지령 위치(Command Position)를 지정한 위치로 변경
        /// </summary>
        /// <param name="nAxis">축 번호</param>
        /// <param name="fValue">변경하고자 하는 위치 (단위: 이동단위)</param>
        /// <returns></returns>
        public bool SetCmd(short nAxis, double fValue)
        {
            short nRet = NMC2.nmc_SetCmdPos(m_nDev_no, nAxis, fValue);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        /// <summary>
        /// 의 엔코더 위치(Encoder Position)를 지정된 위치로 변경
        /// </summary>
        /// <param name="nAxis">축 번호</param>
        /// <param name="fValue">변경하고자 하는 위치(단위: 이동단위)</param>
        /// <returns></returns>
        public bool SetEnc(short nAxis, double fValue)
        {
            short nRet = NMC2.nmc_SetEncPos(m_nDev_no, nAxis, fValue);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        //  false - A 접점  ,   true  - B 접점
        public bool SetEmerLogic(short logic)
        {
            short nRet = NMC2.nmc_SetEmgLogic(m_nDev_no, 0, logic);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }
        //  false - A 접점  ,   true  - B 접점
        public bool SetPulseLogic(short nAxis,int logic)
        {
            short nRet = NMC2.nmc_SetPulseLogic(m_nDev_no, nAxis, (short)logic);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }


        /// <summary>
        /// 모션 구동의 완료 여부를 확인
        /// </summary>
        /// <param name="nAxis">축 번호</param>
        /// <param name="nValue">"펄스 출력상태를 읽을 포인터" 0: 이동 중(펄스출력 OFF), 1: 이동완료(펄스출력 ON) </param>
        /// <returns></returns>
        public bool GetBusy(short nAxis, out short nValue)
        {
            short nRet = NMC2.nmc_GetBusyStatus(m_nDev_no, nAxis, out nValue);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        /// <summary>
        /// 여러 축의 펄스 출력 여부를 확인
        /// </summary>
        /// <param name="BusyState">"8축의 펄스 출력 상태를 읽을 8개의 배열" 0: 이동 완료, 1: 이동 중</param>
        /// <returns></returns>
        public bool GetBusyAll(short[] BusyStatus)
        {
            short nRet = NMC2.nmc_GetBusyStatusAll(m_nDev_no, BusyStatus);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }


        //  false - A 접점  ,   true  - B 접점
        public bool SetNearLogic(short nAxis, short logic)
        {
            short nRet = NMC2.nmc_SetNearLogic(m_nDev_no, nAxis, logic);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        public bool SetMinusLimitLogic(short nAxis, short logic)
        {
            short nRet = NMC2.nmc_SetMinusLimitLogic(m_nDev_no, nAxis, logic);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        public bool SetPlusLimitLogic(short nAxis, short logic)
        {
            short nRet = NMC2.nmc_SetPlusLimitLogic(m_nDev_no, nAxis, logic);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        public bool SetAlarmLogic(short nAxis, short logic)
        {
            short nRet = NMC2.nmc_SetAlarmLogic(m_nDev_no, nAxis, logic);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        public bool SetPulseMode(short nAxis,short nClock)
        {
            short nRet = NMC2.nmc_SetPulseMode(m_nDev_no, nAxis, nClock);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }


        public bool SetEncCountMode( short nAxis,short nEncMode)
        {
            short nRet = NMC2.nmc_SetEncoderCount(m_nDev_no, nAxis, nEncMode);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }


        public bool SetEncInputMode(short nAxis,short nMode)
        {
            short nRet = NMC2.nmc_SetEncoderDir(m_nDev_no, nAxis, nMode);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        //  false - A 접점  ,   true  - B 접점
        public bool SetZLogic(short nAxis, short logic)
        {
            short nRet = NMC2.nmc_SetEncoderZLogic(m_nDev_no, nAxis, logic);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }


        /// <summary>
        /// 지정된 축을 설정된 방식으로 원점으로 이동
        /// </summary>
        /// <param name="nAxis">축 번호</param>
        /// <param name="nHomeMode">0: +Limit, 1: -Limit, 2: +Near(around -Limit), 3: -Near(around +Limit), 4: -Z, 5: +Z </param>
        /// <returns></returns>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nAxis"></param>
        /// <param name="nHomeMode">0: +Limit, 1: -Limit, 2: +Near(around -Limit), 3: -Near(around +Limit), 4: -Z, 5: +Z </param>
        /// <param name="nHomeEndMode">0: None, 3: Offset 이동 후에 Cmd/Enc 값 0으로 초기화, 12:Offset 이동 전에 Cmd/Enc 값 0으로 초기화, 15: 이동 전/후 Cmd/Enc 0으로 초기화</param>
        /// <param name="dOffset">원점이동이 종료된 후 Offset이동할 위치</param>
        /// <returns></returns>        
        public bool HomeMove(short nAxis, int nHomeMode, short nHomeEndMode, double dOffset)
        {
            short nRet = NMC2.nmc_HomeMove(m_nDev_no, nAxis, (short)nHomeMode, nHomeEndMode, dOffset, 0);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }



        /// <summary>
        /// 축의 표시 값(센서 입력 상태 및 위치 정보 등)을 읽기 : Busy, Error, Enc, Near, alarm, Emer...
        /// </summary>
        /// <param name="pNmcData">축 상태를 받을 구조체의 포인터</param>
        /// <returns></returns>
        public bool GetNmcStatus(ref NMC2.NMCAXESEXPR pNmcData)
        {
            short nRet = NMC2.nmc_GetAxesExpress(m_nDev_no, out pNmcData);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }


        public bool GetStateInfo()
        {
            NMC2.NMCSTATEINFO tState;
            NMC2.NMCAXISINFO tAxis;

            short nRet = NMC2.nmc_GetStateInfo(m_nDev_no, out tState, 0);

            NMC2.nmc_StateInfoToAxisInfo(0, ref tState, out tAxis);
            
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }


        public NMC2.NMCAXISINFO UpdateAxisInfo(short nAxis)
        {
            NMC2.NMCSTATEINFO tState;
            NMC2.NMCAXISINFO tAxis;

            NMC2.nmc_GetStateInfo(m_nDev_no, out tState, 0);
            NMC2.nmc_StateInfoToAxisInfo(nAxis, ref tState, out tAxis);


            return tAxis;
        }

        public void SetUnitPulse(short nAxis, double dRatio)
        {
            NMC2.nmc_SetUnitPerPulse(m_nDev_no, nAxis, dRatio);
        }
        public bool SaveToRom()
        {
            short nRet = NMC2.nmc_MotCfgSaveToROM(m_nDev_no, 0);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 1:
                    return false;
            }
            return true;

        }
        public bool EraseRom()
        {
            short nRet = NMC2.nmc_MotCfgSetDefaultROM(m_nDev_no, 0);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 1:
                    return false;
            }
            return true;

        }
        public bool ContTest()
        {
            short nRet;
            ulong nExeNo;

            nRet = NMC2.nmc_ContiSetNodeClear(m_nDev_no, GROUP);               // 노드 버퍼의 초기화
            if(nRet != NMC2.NMC_OK) return false;

            // 그룹0,삼각파형 방지X, 예외처리:종료, 연속 보간축 : 0축, 1축 (2축 연속 보간만 수행)
            //      최대 구동 속도 : 300,출력 포트:지정 안함,출력 pin mask:0,종료 시 출력 값:0
            nRet = NMC2.nmc_ContiSetMode(m_nDev_no, GROUP, 0, 0, 0, 1, -1, 300, 2, 0, 0);
            if (nRet != NMC2.NMC_OK) return false;

            //==================== 노드 등록 ====================
            // 1st 노드[직선] === 위치:(0,200), 시작:50, 가속:250, 감속: 0, 구동속도:100
            nRet = NMC2.nmc_ContiAddNodeLine2Axis(m_nDev_no, GROUP, 0, 200, 50, 250, 0, 100, -1);
            if (nRet != NMC2.NMC_OK) return false;

            // 2nd 노드[원호] === 중심위치:(100,200), 시작:100, 가속:750, 감속: 500, 구동속도:300, 회전각:-90
            nRet = NMC2.nmc_ContiAddNodeArc(m_nDev_no, GROUP, 100, 200, 100, 750, 500, 300, -90, -1);
            if (nRet != NMC2.NMC_OK) return false;

            // 3rd 노드[직선] === 위치:(300,300), 시작:200, 가속:250, 감속: 0, 구동속도:200
            nRet = NMC2.nmc_ContiAddNodeLine2Axis(m_nDev_no, GROUP, 300, 300, 200, 250, 0, 200, -1);
            if (nRet != NMC2.NMC_OK) return false;

            // 4th 노드[직선] === 위치:(300,100), 시작:200, 가속:750, 감속: 0, 구동속도:250
            nRet = NMC2.nmc_ContiAddNodeLine2Axis(m_nDev_no, GROUP, 300, 100, 200, 750, 0, 250, -1);
            if (nRet != NMC2.NMC_OK) return false;

            // 5th 노드[원호]=== 중심위치:(200,100), 시작:250, 가속:750, 감속: 0, 구동속도:150, 회전각:-90
            nRet = NMC2.nmc_ContiAddNodeArc(m_nDev_no, GROUP, 200, 100, 250, 750, 0, 150, -90, -1);
            if (nRet != NMC2.NMC_OK) return false;

            // 6th 노드[직선]=== 위치:(0,0), 시작:150, 가속:125, 감속: 375, 구동속도:200
            nRet = NMC2.nmc_ContiAddNodeLine2Axis(m_nDev_no, GROUP, 0, 0, 150, 125, 375, 200, -1);
            if (nRet != NMC2.NMC_OK) return false;

            // 노드 전송 종료
            nRet = NMC2.nmc_ContiSetCloseNode(m_nDev_no, GROUP);
            if (nRet != NMC2.NMC_OK) return false;

            // ==================== 연속 보간 실행 ====================
            // 연속보간 실행
            nRet = NMC2.nmc_ContiRunStop(m_nDev_no, GROUP, 1);
            if (nRet != NMC2.NMC_OK) return false;

            // =================== 보간이동 종료 대기 ==================
            NMC2.NMCCONTISTATUS NmcContiStatus;
            while (true)
            {
                /* 본 while구문의 내용은 사용하실 프로그램 내 주기적으로 실행 되는 곳에서 실행 하셔도 됩니다. 
                단, 노드 실행 속도 보다는 갱신 속도가 빨라야 연속 보간이 끊김없이 실행 됩니다. */
                nRet = NMC2.nmc_ContiGetStatus(m_nDev_no, out NmcContiStatus);

                // 그룹 0의 현재까지 실행한 노드의 수
                nExeNo = NmcContiStatus.uiContiExecutionNum[GROUP];

                if (NmcContiStatus.nContiRun[GROUP] == 0) break;     // 연속보간 이동 끝, while 구문 탈출
            }

            return true;
        }
        public bool LoadFromRom()
        {
            short nRet = NMC2.nmc_MotCfgLoadFromROM(m_nDev_no, 0);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 1:
                    return false;
            
            }
            return true;

        }

        public int GetEnumList(short[] pnIp, out NMC2.NMCEQUIPLIST pNmcList)            // 2017.01.11 han
        {
            int nRet = NMC2.nmc_GetEnumList(pnIp, out pNmcList); // 2017.5.31 khd

            return nRet;
        }                                                                               // 2017.01.11 han

        public bool GetParaLogic(short nAxis, out NMC2.NMCPARALOGIC pNmcParaLogic)     // 2017.01.16 han start
        {
            short nRet = NMC2.nmc_GetParaLogic(m_nDev_no, nAxis, out pNmcParaLogic);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;

        }

        public bool SetParaLogic(short nAxis, ref NMC2.NMCPARALOGIC pNmcParaLogic)     
        {
            short nRet = NMC2.nmc_SetParaLogic(m_nDev_no, nAxis, ref pNmcParaLogic);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }
            return false;
        }                                                                               // 2017.01.16 han end


        /// <summary>
        /// 입력에 사용할 신호와 출력사양을 설정
        /// </summary>
        /// <param name="nAxisNo">축 번호</param>
        /// <param name="nInType">"Trigger에 사용할 펄스" 0 : 지령펄스 , 1 : 엔코더 펄스</param>
        /// <param name="nOutLogic">"출력 로직을 선택"  0 : Active Low, 1: High </param>
        /// <param name="nOutDelay">"출력 시점에서 Delay되는 시간을 설정" 0 ~ 65535 us </param>
        /// <param name="nOutWidth">"Trigger출력의 펄스 폭(Width)시간을 설정" 1 ~ 65536 us </param>
        /// <returns></returns>
        public bool SetTriggerIO(short nAxisNo, short nInType, short nOutLogic, int nOutDelay, int nOutWidth)
        {
            short nRet = nmc_SetTriggerIO(m_nDev_no, nAxisNo, nInType, nOutLogic, nOutDelay, nOutWidth);

            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }
        /// <summary>
        /// 시작위치와 끝위치 범위에서 지정된 주기 Pulse마다 Trigger를 출력
        /// </summary>
        /// <param name="nAxisNo">축번호</param>
        /// <param name="dStartPos">Trigger 출력을 시작하고자 하는 위치</param>
        /// <param name="dEndPos">Trigger 출력을 종료하고자 하는 위치</param>
        /// <param name="dInterval">Trigger를 출력하고자 하는 간격 위치(양수로만 지정)</param>
        /// <param name="nDir"> 0: Counter방향에 무관계, 1: Counter Up중에만 출력, 2: Counter Down중에만 출력 </param>
        /// <returns></returns>
        public bool TriggerOutLineScan(short nAxisNo, double dStartPos, double dEndPos, double dInterval, short nDir)
        {
            short nRet = nmc_TriggerOutLineScan(m_nDev_no ,nAxisNo,dStartPos, dEndPos, dInterval, nDir);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 복수의 설정된 절대 위치에서만 Trigger를 출력
        /// </summary>
        /// <param name="nAxisNo">축 번호</param>
        /// <param name="nCount"> 설정 위치 수(최대 : 128개) </param>
        /// <param name="pdPosList">설정 위치 값 </param>
        /// <returns></returns>
        public bool TriggerOutAbsPos(short nAxisNo, short nCount, double[] pdPosList)
        {
            short nRet = nmc_TriggerOutAbsPos(m_nDev_no, nAxisNo, nCount, pdPosList);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }


        /// <summary>
        /// 지정된 복수의 구간(Range)에서만 Trigger를 출력
        /// </summary>
        /// <param name="nAxisNo">축 번호</param>
        /// <param name="nCount">구간 설정 수 (최대 : 64개)</param>
        /// <param name="pdStartPosList">각 구간의 시작 위치 값</param>
        /// <param name="pdEndPosList">각 구간의 종료 위치 값</param>
        /// <returns></returns>
        public bool TriggerOutRange(short nAxisNo, short nCount, double[] pdStartPosList, double[] pdEndPosList)
        {
            short nRet = nmc_TriggerOutRange(m_nDev_no, nAxisNo, nCount, pdStartPosList, pdEndPosList);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Trigger 출력 확인 용으로 사용되며, 사용자에 의해 지정되는 임의의 순간에 수행
        /// </summary>
        /// <param name="nAxisNo">축 번호</param>
        /// <returns></returns>
        public bool TriggerOutOneShot(short nAxisNo)
        {
            short nRet = nmc_TriggerOutOneShot(m_nDev_no, nAxisNo);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 시작된 Line Scan, Position, Range Trigger를 정지
        /// </summary>
        /// <param name="nAxisNo">축 번호</param>
        /// <returns></returns>
        public bool TriggerOutStop(short nAxisNo)
        {
            short nRet= nmc_TriggerOutStop(m_nDev_no, nAxisNo);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Trigger 출력 Pin에 Axis을 Assign
        /// </summary>
        /// <param name="nGroup">"NMC 420 모델" 무조건 0 </param>
        /// <param name="nPin0">0 : TPin 0, -1 : 사용하지 않음</param>
        /// <param name="nPin1">1 : TPin 1, -1 : 사용하지 않음</param>
        /// <param name="nPin2">2 : Tpin 2, -1 : 사용하지 않음</param>
        /// <param name="nPin3">3 : Tpin 3, -1 : 사용하지 않음</param>
        /// <returns></returns>
        public bool SetTriggerGroupAssign(short nGroup,short nPin0, short nPin1, short nPin2, short nPin3)
        {
            short nRet = nmc_SetTriggerGroupAssign(m_nDev_no, nGroup, nPin0, nPin1, nPin2, nPin3);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Trigger 출력 Pin에 할당된 Axis를 확인
        /// </summary>
        /// <param name="nGroup">"NMC 420 모델" 무조건 0</param>
        /// <param name="pPin0">short nPin0 변수 정의 및 할당 </param>
        /// <param name="pPin1"></param>
        /// <param name="pPin2"></param>
        /// <param name="pPin3"></param>
        /// <returns></returns>
        public bool GetTriggerGroupAssign(short nGroup, out short pPin0, out short pPin1, out short pPin2, out short pPin3)
        {
            short nRet = nmc_GetTriggerGroupAssign(m_nDev_no, nGroup, out pPin0, out pPin1, out pPin2, out pPin3);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }

        public bool GetTriggerStatus(out NMC2.NMCTRIGSTATUS pTriggerStatus)
        {
            short nRet = nmc_GetTriggerStatus(m_nDev_no, out pTriggerStatus);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }

            return false;
        }


        /// <summary>
        /// 축의 지령 위치를 읽음
        /// </summary>
        /// <param name="nAxis">축 번호</param>
        /// <param name="plCmdPos">축 지령 위치를 받을 포인터</param>
        /// <returns></returns>
        public bool GetCmdPos(short nAxis, out int plCmdPos)
        {
            short nRet = nmc_GetCmdPos(m_nDev_no, nAxis, out plCmdPos);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 축의 엔코더 위치를 읽음
        /// </summary>
        /// <param name="nAxis">축 번호</param>
        /// <param name="plEncPos">축 엔코더 위치를 받을 포인터</param>
        /// <returns></returns>
        public bool GetEncPos(short nAxis, out int plEncPos)
        {
            short nRet = nmc_GetEncPos(m_nDev_no, nAxis, out plEncPos);
            switch (nRet)
            {
                case NMC2.NMC_NOTCONNECT:
                    m_bIsOpen = false;
                    return false;
                case 0:
                    return true;
            }
            return false;
        }

    }
}