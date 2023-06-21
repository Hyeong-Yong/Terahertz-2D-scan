using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using Paix_MotionController;

namespace test_MotionController
{
    public partial class GUI : Form
    {
        public string[] NMCDesc= {
                                "NMC2_220S"
                                ,"NMC2_420S"
                                ,"NMC2_620S"
                                ,"NMC2_820S"
                                ,"NMC2_220_DIO32"
                                ,"NMC2_220_DIO64"
                                ,"NMC2_420_DIO32"
                                ,"NMC2_420_DIO64"
                                ,"NMC2_820_DIO32"
                                ,"NMC2_820_DIO64"
                                ,"NMC2_DIO32"
                                ,"NMC2_DIO64"
                                ,"NMC2_DIO96"
                                ,"NMC2_DIO128"
                                ,"NMC2_220"
                                ,"NMC2_420"
                                ,"NMC2_620"
                                ,"NMC2_820"
                                ,"NMC2_620_DIO32"
                                ,"NMC2_620_DIO64"
                                ,null
                                };
        PaixMotion PaixMotion;
        Thread TdWatchSensor;



        Paix_MotionController.NMC2.NMCAXESEXPR NmcData = new Paix_MotionController.NMC2.NMCAXESEXPR();
        //

        public GUI()
        {
            InitializeComponent();

            PaixMotion = new PaixMotion();
            TdWatchSensor = new Thread(new ThreadStart(watchSensor));
            listIP.Columns.Add("IP", 100);
            listIP.Columns.Add("Model", 150);
        }

        private void MotionController_Load(object sender, EventArgs e)
        {
            this.Text = "Motor Controller";
            LoadConfigurationSetting();

        }

        private void LoadConfigurationSetting()
        {
            /*
             * Init : X-axis speed configuration
             */
            textbox_Start_X.Text = ConfigurationManager.AppSettings["textBoxSSXStart"];
            textbox_Acc_X.Text = ConfigurationManager.AppSettings["textBoxSSXAcc"];
            textbox_Max_X.Text = ConfigurationManager.AppSettings["textBoxSSXMax"];
            textbox_Dec_X.Text = ConfigurationManager.AppSettings["textBoxSSXDec"];

            /*
             * Init : Y-axis speed configuration
             */
            textbox_Start_Y.Text = ConfigurationManager.AppSettings["textBoxSSYStart"];
            textbox_Acc_Y.Text = ConfigurationManager.AppSettings["textBoxSSYAcc"];
            textbox_Max_Y.Text = ConfigurationManager.AppSettings["textBoxSSYMax"];
            textbox_Dec_Y.Text = ConfigurationManager.AppSettings["textBoxSSYDec"];

            /*
             * Init : X,Y distance value
             */
            textbox_Distance_X.Text = ConfigurationManager.AppSettings["textBoxMCXOut"];
            textbox_Distance_Y.Text = ConfigurationManager.AppSettings["textBoxMCYOut"];

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            short devId = Convert.ToInt16(textBoxDevNo.Text);

            if (buttonOpen.Text == "Open" && PaixMotion.Open(devId))
            {
                switch (TdWatchSensor.ThreadState)
                {
                    case ThreadState.Stopped:
                        TdWatchSensor = new Thread(new ThreadStart(watchSensor));
                        break;
                    case ThreadState.Unstarted:
                        break;
                    default:
                        TdWatchSensor.Abort();
//                        TdWatchSensor.Join();
                        while (TdWatchSensor.ThreadState != ThreadState.Stopped) { }
                        break;
                }

                TdWatchSensor.Start();
                buttonOpen.Text = "Close";

                Init_Btn_Status(); // 버튼 초기화

            }
            else if (buttonOpen.Text == "Close" && PaixMotion.Close())
            {
                TdWatchSensor.Abort();
                TdWatchSensor.Join();

                while (TdWatchSensor.ThreadState != ThreadState.Stopped) { }
                
                buttonOpen.Text = "Open";
            }
        }

        public void watchSensor()
        {
            //PaixMotion.NMC2.NMC_AXES_EXPR NmcData;
            while (true)
            {
                System.Threading.Thread.Sleep(1);
                this.Invoke(new delegateUpdateCmdEnc(updateCmdEnc));

            }
        }

        private delegate void delegateUpdateCmdEnc();

        private void updateCmdEnc()
        {
            PaixMotion.GetStateInfo();

            if (PaixMotion.GetNmcStatus(ref NmcData) == false)
                return;
                
            labelPEXCmdVal.Text = NmcData.dCmd[0].ToString();
            labelPEYCmdVal.Text = NmcData.dCmd[1].ToString();

            labelPEXEncVal.Text = NmcData.dEnc[0].ToString();
            labelPEYEncVal.Text = NmcData.dEnc[1].ToString();

            panelEmergency.BackColor = NmcData.nEmer[0] == 1 ? Color.Red : Color.LightYellow;
            panelSXBusy.BackColor = NmcData.nBusy[0] == 1? Color.Red : Color.LightYellow;
            panelSYBusy.BackColor = NmcData.nBusy[1] == 1? Color.Red : Color.LightYellow;

            panelSXNear.BackColor = NmcData.nNear[0] == 1? Color.Red : Color.LightYellow;
            panelSYNear.BackColor = NmcData.nNear[1] == 1? Color.Red : Color.LightYellow;
            panelSXMinusLimit.BackColor = NmcData.nMLimit[0] == 1 ? Color.Red : Color.LightYellow;
            panelSYMinusLimit.BackColor = NmcData.nMLimit[1] == 1 ? Color.Red : Color.LightYellow;

            panelSXPlusLimit.BackColor = NmcData.nPLimit[0] == 1? Color.Red : Color.LightYellow;
            panelSYPlusLimit.BackColor = NmcData.nPLimit[1] == 1? Color.Red : Color.LightYellow;

            panelSXAlarm.BackColor = NmcData.nAlarm[0] == 1 ? Color.Red : Color.LightYellow;
            panelSYAlarm.BackColor = NmcData.nAlarm[1] == 1 ? Color.Red : Color.LightYellow;

            panelSXEncZ.BackColor = NmcData.nEncZ[0] == 1 ? Color.Red : Color.LightYellow;
            panelSYEncZ.BackColor = NmcData.nEncZ[1] == 1? Color.Red : Color.LightYellow;
        }

        private void buttonMCXIncMinus_Click(object sender, EventArgs e)
        {
            PaixMotion.RelMove(0, -Convert.ToDouble(textbox_Distance_X.Text));
        }

        private void buttonMCXIncPlus_Click(object sender, EventArgs e)
        {
            PaixMotion.RelMove(0, Convert.ToDouble(textbox_Distance_X.Text));
        }

        private void buttonMCXAbsMinus_Click(object sender, EventArgs e)
        {
            PaixMotion.AbsMove(0, -Convert.ToDouble(textbox_Distance_X.Text));
        }

        private void buttonMCXAbsPlus_Click(object sender, EventArgs e)
        {
            PaixMotion.AbsMove(0, Convert.ToDouble(textbox_Distance_X.Text));
        }

        private void buttonMCXJogLeft_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(0, 1);
        }

        private void buttonMCXJogLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxMCJogCont.Checked)
            {
                stop(0);
            }
        }

        private void buttonMCXJogRight_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(0, 0);
        }

        private void buttonMCXJogRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxMCJogCont.Checked)
            {
                stop(0);
            }
        }

        private void buttonMCXStop_Click(object sender, EventArgs e)
        {
            stop(0);
        }

        private void stop(short nAxis)
        {

            if (checkBoxMCSlowStop.Checked)
            {
                PaixMotion.SlowStop(nAxis);
            }
            else
            {
                PaixMotion.Stop(nAxis);
            }
        }

        private void buttonMCYIncMinus_Click(object sender, EventArgs e)
        {
            PaixMotion.RelMove(1, -Convert.ToInt32(textbox_Distance_Y.Text));
        }

        private void buttonMCYIncPlus_Click(object sender, EventArgs e)
        {
            PaixMotion.RelMove(1, Convert.ToInt32(textbox_Distance_Y.Text));
        }

        private void buttonMCYAbsMinus_Click(object sender, EventArgs e)
        {
            PaixMotion.AbsMove(1, -Convert.ToInt32(textbox_Distance_Y.Text));
        }

        private void buttonMCYAbsPlus_Click(object sender, EventArgs e)
        {
            PaixMotion.AbsMove(1, Convert.ToInt32(textbox_Distance_Y.Text));
        }

        private void buttonMCYJogLeft_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(1, 1);
        }

        private void buttonMCYJogLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxMCJogCont.Checked)
            {
                stop(1);
            }
        }

        private void buttonMCYJogRight_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(1, 0);
        }

        private void buttonMCYJogRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxMCJogCont.Checked)
            {
                stop(1);
            }
        }

        private void buttonMCYStop_Click(object sender, EventArgs e)
        {
            stop(1);
        }

        private void buttonMCIncMultiMove_Click(object sender, EventArgs e)
        {
            if (checkBoxMCIncSync.Checked)
            {
                PaixMotion.SyncTwoMove(Convert.ToDouble(textbox_Distance_X.Text), Convert.ToDouble(textbox_Distance_Y.Text), 0);
            }
            else
            {
                double[] dest = { Convert.ToDouble(textbox_Distance_X.Text), Convert.ToDouble(textbox_Distance_Y.Text) };
                PaixMotion.RelMultiTwoMove(dest);
                //                NMC2.nmc_imul_move(1, 2, new short[2] { 1, 2 }, dest);
            }
        }

        private void buttonMCAbsMultiMove_Click(object sender, EventArgs e)
        {
            if (checkBoxMCAbsSync.Checked)
            {
                PaixMotion.SyncTwoMove(Convert.ToDouble(textbox_Distance_X.Text), Convert.ToDouble(textbox_Distance_Y.Text), 1);
            }
            else
            {
                double[] dest = { Convert.ToDouble(textbox_Distance_X.Text), Convert.ToDouble(textbox_Distance_Y.Text) };
                PaixMotion.AbsMultiTwoMove(dest);
            }
        }

        private void checkBoxMCIncSync_CheckedChanged(object sender, EventArgs e)
        {
            buttonMCIncMultiMove.Text = checkBoxMCIncSync.Checked ? "동시/동기 Move" : "다축 Move";
        }

        private void checkBoxMCAbsSync_CheckedChanged(object sender, EventArgs e)
        {
            buttonMCAbsMultiMove.Text = checkBoxMCAbsSync.Checked ? "동시/동기 Move" : "다축 Move";
        }

        private void buttonMCXHome_Click(object sender, EventArgs e)
        {
            PaixMotion.HomeMove(0, comboBoxLSXHomeMode.SelectedIndex);
        }

        private void buttonMCYHome_Click(object sender, EventArgs e)
        {
            PaixMotion.HomeMove(1, comboBoxLSYHomeMode.SelectedIndex);
        }

        private void buttonSSXSetup_Click(object sender, EventArgs e)
        {
            double dstart = Convert.ToDouble(textbox_Start_X.Text);
            double dacc = Convert.ToDouble(textbox_Acc_X.Text);
            double dmax = Convert.ToDouble(textbox_Max_X.Text);
            double ddec = Convert.ToDouble(textbox_Dec_X.Text);

            PaixMotion.SetSpeedPPS(0, dstart, dacc, ddec, dmax);
        }

        private void buttonSSYSetup_Click(object sender, EventArgs e)
        {
            double dstart = Convert.ToDouble(textbox_Start_Y.Text);
            double dacc = Convert.ToDouble(textbox_Acc_Y.Text);
            double dmax = Convert.ToDouble(textbox_Max_Y.Text);
            double ddec = Convert.ToDouble(textbox_Dec_Y.Text);

            PaixMotion.SetSpeedPPS(1, dstart, dacc, ddec, dmax);
        }

        private void MotionControler_FormClosed(object sender, FormClosedEventArgs e)
        {
            PaixMotion.Close();
            if (TdWatchSensor.IsAlive)
            {
                TdWatchSensor.Abort();
            }
        }

        private void labelPEXCmdVal_Click(object sender, EventArgs e)
        {
            PaixMotion.SetCmd(0, 0);

        }

        private void setXCmd(int val)
        {
        }

        private void labelPEYCmdVal_Click(object sender, EventArgs e)
        {
            PaixMotion.SetCmd(1, 0);

        }

        private void setYCmd(int val)
        {
        }

        private void comboBoxLSEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetEmerLogic(comboBoxLSEmergency.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void comboBoxLSXNear_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetNearLogic(0, comboBoxLSXNear.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void comboBoxLSYNear_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetNearLogic(1, comboBoxLSYNear.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void comboBoxLSXMLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetMinusLimitLogic(0, comboBoxLSXMLimit.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void comboBoxLSYMLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetMinusLimitLogic(1, comboBoxLSYMLimit.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void comboBoxLSXPLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetPlusLimitLogic(0, comboBoxLSXPLimit.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void comboBoxLSYPLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetPlusLimitLogic(1, comboBoxLSYPLimit.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void comboBoxLSXAlarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetAlarmLogic(0, comboBoxLSXAlarm.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void comboBoxLSYAlarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetAlarmLogic(1, comboBoxLSYAlarm.SelectedIndex == 0 ? (short)0 : (short)1);
        }


        private void comboBoxLSXEnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetEncCountMode(0, (short)comboBoxLSXEnc.SelectedIndex);
        }

        private void comboBoxLSYEnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetEncCountMode(1, (short)comboBoxLSYEnc.SelectedIndex);
        }

        private void comboBoxLSXEncZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetZLogic(0, (short)comboBoxLSXEncZ.SelectedIndex);
        }

        private void comboBoxLSYEncZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetZLogic(1, (short)comboBoxLSYEncZ.SelectedIndex);
        }

        private void comboBoxLSXEncInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetEncInputMode(0, (short)comboBoxLSXEncInput.SelectedIndex);
        }

        private void comboBoxLSYEncInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetEncInputMode(1, (short)comboBoxLSYEncInput.SelectedIndex);
        }

        private void labelPEXEncVal_Click(object sender, EventArgs e)
        {
            PaixMotion.SetEnc(0, 0);

        }

        private void labelPEYEncVal_Click(object sender, EventArgs e)
        {
            PaixMotion.SetEnc(1, 0);

        }

        private void textBoxLSXResolution_TextChanged(object sender, EventArgs e)
        {
            //
            PaixMotion.SetUnitPulse(0, Convert.ToDouble(textBoxLSXRatio.Text));
        }

        private void textBoxLSYRatio_TextChanged(object sender, EventArgs e)
        {
            PaixMotion.SetUnitPulse(1, Convert.ToDouble(textBoxLSYRatio.Text));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetPulseLogic(0, cBox_Axis0.SelectedIndex);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetPulseLogic(1, cBox_Axis1.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PaixMotion.SaveToRom();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PaixMotion.EraseRom();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            Paix_MotionController.NMC2.NMCPARALOGIC  NmcParaLogic = new Paix_MotionController.NMC2.NMCPARALOGIC();

            PaixMotion.LoadFromRom();
            
            PaixMotion.GetParaLogic(0, out NmcParaLogic);
            UpdateXParaLogic(ref NmcParaLogic);
            PaixMotion.GetParaLogic(1, out NmcParaLogic);
            UpdateYParaLogic(ref NmcParaLogic);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PaixMotion.ContTest();
        }

        private void btnGetList_Click(object sender, EventArgs e)
        {
            Paix_MotionController.NMC2.NMCEQUIPLIST NmcList;
            short[] nIp = new short[4];
            int nCount;
            string str;

            nIp[0] = 192;
            nIp[1] = 168;
            nIp[2] = 0;
            nIp[3] = 255;

            nCount = PaixMotion.GetEnumList(nIp, out NmcList);
            for (int i = 0; i < nCount; i++)
            {
                str = string.Format("{0:d}.{1:d}.{2:d}.{3:d} {4:s}", NmcList.lIp[i] & 0xff, NmcList.lIp[i] >> 8 & 0xff
			    , NmcList.lIp[i] >> 16 & 0xff, NmcList.lIp[i] >> 24 & 0xff, NMCDesc[NmcList.lModelType[i]]);
                listIP.Items.Add(str);
            }
        }

        private void btnGetLogic_Click(object sender, EventArgs e)
        {
            Paix_MotionController.NMC2.NMCPARALOGIC NmcParaLogic = new Paix_MotionController.NMC2.NMCPARALOGIC();

            PaixMotion.GetParaLogic(0, out NmcParaLogic);
            UpdateXParaLogic(ref NmcParaLogic);
            PaixMotion.GetParaLogic(1, out NmcParaLogic);
            UpdateYParaLogic(ref NmcParaLogic);
        }

        private void btnSetLogic_Click(object sender, EventArgs e)
        {
            Paix_MotionController.NMC2.NMCPARALOGIC NmcParaLogic = new Paix_MotionController.NMC2.NMCPARALOGIC();

            SetXParaLogic(ref NmcParaLogic);
            PaixMotion.SetParaLogic(0, ref NmcParaLogic);
            SetYParaLogic(ref NmcParaLogic);
            PaixMotion.SetParaLogic(1, ref NmcParaLogic);
        }

        private void UpdateXParaLogic(ref Paix_MotionController.NMC2.NMCPARALOGIC pNmcParaLogic)     // out으로 할 경우 Error 발생
        {
                                                                                                       // 아마도 구조체 필드의 초기화 문제 인듯 합니다.(out은 초기화를 하지 않고, ref는 초기화를 합니다.)  
            comboBoxLSEmergency.SelectedIndex = pNmcParaLogic.nEmg;
            comboBoxLSXEnc.SelectedIndex = pNmcParaLogic.nEncCount;
            comboBoxLSXEncZ.SelectedIndex = pNmcParaLogic.nEncZ;
            comboBoxLSXEncInput.SelectedIndex = pNmcParaLogic.nEncDir;
            comboBoxLSXNear.SelectedIndex = pNmcParaLogic.nNear;
            comboBoxLSXMLimit.SelectedIndex = pNmcParaLogic.nMLimit;
            comboBoxLSXPLimit.SelectedIndex = pNmcParaLogic.nPLimit;
            comboBoxLSXAlarm.SelectedIndex = pNmcParaLogic.nAlarm;
            cBox_Axis0.SelectedIndex = pNmcParaLogic.nPulseMode;  
        }
        private void UpdateYParaLogic(ref Paix_MotionController.NMC2.NMCPARALOGIC pNmcParaLogic)
        {
            comboBoxLSYEnc.SelectedIndex = pNmcParaLogic.nEncCount;
            comboBoxLSYEncZ.SelectedIndex = pNmcParaLogic.nEncZ;
            comboBoxLSYEncInput.SelectedIndex = pNmcParaLogic.nEncDir;
            comboBoxLSYNear.SelectedIndex = pNmcParaLogic.nNear;
            comboBoxLSYMLimit.SelectedIndex = pNmcParaLogic.nMLimit;
            comboBoxLSYPLimit.SelectedIndex = pNmcParaLogic.nPLimit;
            comboBoxLSYAlarm.SelectedIndex = pNmcParaLogic.nAlarm;
            cBox_Axis1.SelectedIndex = pNmcParaLogic.nPulseMode;
        }
        private void SetXParaLogic(ref Paix_MotionController.NMC2.NMCPARALOGIC pNmcParaLogic)
        {
            pNmcParaLogic.nEmg = (short)comboBoxLSEmergency.SelectedIndex;
            pNmcParaLogic.nEncCount = (short)comboBoxLSXEnc.SelectedIndex;
            pNmcParaLogic.nEncZ = (short)comboBoxLSXEncZ.SelectedIndex;
            pNmcParaLogic.nEncDir = (short)comboBoxLSXEncInput.SelectedIndex;
            pNmcParaLogic.nNear = (short)comboBoxLSXNear.SelectedIndex;
            pNmcParaLogic.nMLimit = (short)comboBoxLSXMLimit.SelectedIndex;
            pNmcParaLogic.nPLimit = (short)comboBoxLSXPLimit.SelectedIndex;
            pNmcParaLogic.nAlarm = (short)comboBoxLSXAlarm.SelectedIndex;
            pNmcParaLogic.nPulseMode = (short)cBox_Axis0.SelectedIndex;
        }
        private void SetYParaLogic(ref Paix_MotionController.NMC2.NMCPARALOGIC pNmcParaLogic)
        {
            pNmcParaLogic.nEncCount = (short)comboBoxLSYEnc.SelectedIndex;
            pNmcParaLogic.nEncZ = (short)comboBoxLSYEncZ.SelectedIndex;
            pNmcParaLogic.nEncDir = (short)comboBoxLSYEncInput.SelectedIndex;
            pNmcParaLogic.nNear = (short)comboBoxLSYNear.SelectedIndex;
            pNmcParaLogic.nMLimit = (short)comboBoxLSYMLimit.SelectedIndex;
            pNmcParaLogic.nPLimit = (short)comboBoxLSYPLimit.SelectedIndex;
            pNmcParaLogic.nAlarm = (short)comboBoxLSYAlarm.SelectedIndex;
            pNmcParaLogic.nPulseMode = (short)cBox_Axis1.SelectedIndex;
        }



        private void Btn_current_X_Click(object sender, EventArgs e)
        {
            short nCurrentOn = PaixMotion.updateAxisInfo(0).nCurrentOn; // CurrentOn 신호 값

            if (nCurrentOn == 0)
            {
                PaixMotion.SetCurrentOn(0, 1);
                this.btn_Current_X.BackColor = Color.Green;
            }
            else
            {
                PaixMotion.SetCurrentOn(0, 0);
                this.btn_Current_X.BackColor = Color.Red;
            }
        }

        private void Btn_servo_X_Click(object sender, EventArgs e)
        {
            short nServoOn = PaixMotion.updateAxisInfo(0).nServoOn; // ServoOn 신호 값

            if (nServoOn == 0)
            {
                PaixMotion.SetServoOn(0, 1);
                this.btn_Servo_X.BackColor = Color.Green;
            }
            else
            {
                PaixMotion.SetServoOn(0, 0);
                this.btn_Servo_X.BackColor = Color.Red;
            }

        }

        private void Init_Btn_Status()
        {
            short nCurrentOn_X = PaixMotion.updateAxisInfo(0).nCurrentOn; // CurrentOn 신호 값
            short nServoOn_X = PaixMotion.updateAxisInfo(0).nServoOn; // ServoOn 신호 값

            short nCurrentOn_Y = PaixMotion.updateAxisInfo(1).nCurrentOn; // CurrentOn 신호 값
            short nServoOn_Y = PaixMotion.updateAxisInfo(1).nServoOn; // ServoOn 신호 값

            if (nCurrentOn_X == 0){
                this.btn_Current_X.BackColor = Color.Red;
            }else{
                this.btn_Current_X.BackColor = Color.Green;
            }

            if (nCurrentOn_Y == 0){
                this.btn_Current_X.BackColor = Color.Red;
            }else{
                this.btn_Current_X.BackColor = Color.Green;
            }

            if (nServoOn_X == 0){
                this.btn_Servo_X.BackColor = Color.Red;
            }else{
                this.btn_Servo_X.BackColor = Color.Green;
            }

            if (nServoOn_Y == 0){
                this.btn_Servo_X.BackColor = Color.Red;
            }else{
                this.btn_Servo_X.BackColor = Color.Green;
            }

        }

        private void btn_save_config_Click(object sender, EventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("textBox_Start_X");
            config.AppSettings.Settings.Remove("textBox_Acc_X");
            config.AppSettings.Settings.Remove("textBox_Dec_X");
            config.AppSettings.Settings.Remove("textBox_Max_X");

            config.AppSettings.Settings.Remove("textBox_Start_Y");
            config.AppSettings.Settings.Remove("textBox_Acc_Y");
            config.AppSettings.Settings.Remove("textBox_Dec_X");
            config.AppSettings.Settings.Remove("textBox_Max_X");

            config.AppSettings.Settings.Remove("textBox_Distance_X");
            config.AppSettings.Settings.Remove("textBox_Distance_Y");

            config.AppSettings.Settings.Add("textBox_Start_X", Textbox_Start_X.Text);
            config.AppSettings.Settings.Add("textBox_Acc_X", Textbox_Acc_X.Text);
            config.AppSettings.Settings.Add("textBox_Dec_X", Textbox_Dec_X.Text);
            config.AppSettings.Settings.Add("textBox_Max_X", Textbox_Max_X.Text);

            config.AppSettings.Settings.Add("textBox_Start_Y", Textbox_Start_Y.Text);
            config.AppSettings.Settings.Add("textBox_Acc_Y", Textbox_Acc_Y.Text);
            config.AppSettings.Settings.Add("textBox_Dec_X", Textbox_Dec_X.Text);
            config.AppSettings.Settings.Add("textBox_Max_X", Textbox_Max_X.Text);


            config.AppSettings.Settings.Add("textBox_Distance_X", Textbox_Distance_X.Text);
            config.AppSettings.Settings.Add("textBox_Distance_Y", Textbox_Distance_Y.Text);


            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
