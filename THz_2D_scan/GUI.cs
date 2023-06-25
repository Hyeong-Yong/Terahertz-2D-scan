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
using static Paix_MotionController.NMC2;
using Paix_MotionController;

namespace THz_2D_scan
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

        PaixMotion PaixMotion = PaixMotion.getInstance;
        Thread TdWatchSensor;

        NMCAXESMOTIONOUT MotOut;
        NMCAXESEXPR NmcData;


        public GUI()
        {
            InitializeComponent();

            TdWatchSensor = new Thread(new ThreadStart(WatchSensor));
            listIP.Columns.Add("IP", 100);
            listIP.Columns.Add("Model", 150);
        }


        private void Btn_Open_Click(object sender, EventArgs e)
        {
            short devId = Convert.ToInt16(textBoxDevNo.Text);

            if (btn_Open.Text == "Open" && PaixMotion.Open(devId))
            {
                switch (TdWatchSensor.ThreadState)
                {
                    case ThreadState.Stopped:
                        TdWatchSensor = new Thread(new ThreadStart(WatchSensor));
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
                btn_Open.Text = "Close";

                Init_Btn_Status(); // 버튼 초기화

            }
            else if (btn_Open.Text == "Close" && PaixMotion.Close())
            {
                TdWatchSensor.Abort();
                TdWatchSensor.Join();

                while (TdWatchSensor.ThreadState != ThreadState.Stopped) { }
                
                btn_Open.Text = "Open";
            }
        }

        private void MotionController_FormClosed(object sender, FormClosedEventArgs e)
        {
            PaixMotion.Close();
            if (TdWatchSensor.IsAlive)
            {
                TdWatchSensor.Abort();
            }
        }

        public void WatchSensor()
        {
            //PaixMotion.NMC2.NMC_AXES_EXPR NmcData;
            while (true)
            {
                System.Threading.Thread.Sleep(1);
                this.Invoke(new delegateUpdateCmdEnc(UpdateCmdEnc));

            }
        }

        private delegate void delegateUpdateCmdEnc();

        private void UpdateCmdEnc()
        {
            PaixMotion.GetStateInfo();

            if (PaixMotion.GetNmcStatus(ref NmcData) == false)
                return;
                
            label_Cmd_X.Text = NmcData.dCmd[0].ToString();
            label_Cmd_Y.Text = NmcData.dCmd[1].ToString();

            label_Enc_X.Text = NmcData.dEnc[0].ToString();
            label_Enc_Y.Text = NmcData.dEnc[1].ToString();

            panel_Emergency.BackColor = NmcData.nEmer[0] == 1 ? Color.Red : Color.LightYellow;
            panel_Busy_X.BackColor = NmcData.nBusy[0] == 1? Color.Red : Color.LightYellow;
            panel_Busy_Y.BackColor = NmcData.nBusy[1] == 1? Color.Red : Color.LightYellow;

            panel_Near_X.BackColor = NmcData.nNear[0] == 1? Color.Red : Color.LightYellow;
            panel_Near_Y.BackColor = NmcData.nNear[1] == 1? Color.Red : Color.LightYellow;
            panel_Limit_Minus_X.BackColor = NmcData.nMLimit[0] == 1 ? Color.Red : Color.LightYellow;
            panel_Limit_Minus_Y.BackColor = NmcData.nMLimit[1] == 1 ? Color.Red : Color.LightYellow;

            panel_Limit_Plus_X.BackColor = NmcData.nPLimit[0] == 1? Color.Red : Color.LightYellow;
            panel_Limit_Plus_Y.BackColor = NmcData.nPLimit[1] == 1? Color.Red : Color.LightYellow;

            panel_Alarm_X.BackColor = NmcData.nAlarm[0] == 1 ? Color.Red : Color.LightYellow;
            panel_Alarm_Y.BackColor = NmcData.nAlarm[1] == 1 ? Color.Red : Color.LightYellow;

            panel_EncZ_X.BackColor = NmcData.nEncZ[0] == 1 ? Color.Red : Color.LightYellow;
            panel_EncZ_Y.BackColor = NmcData.nEncZ[1] == 1? Color.Red : Color.LightYellow;
        }


        private void Btn_Setup_X_Click(object sender, EventArgs e)
        {
            double dstart = Convert.ToDouble(textbox_Start_X.Text);
            double dacc = Convert.ToDouble(textbox_Acc_X.Text);
            double dmax = Convert.ToDouble(textbox_Max_X.Text);
            double ddec = Convert.ToDouble(textbox_Dec_X.Text);

            PaixMotion.SetSpeedPPS(0, dstart, dacc, ddec, dmax);
        }

        private void Btn_Setup_Y_Click(object sender, EventArgs e)
        {
            double dstart = Convert.ToDouble(textbox_Start_Y.Text);
            double dacc = Convert.ToDouble(textbox_Acc_Y.Text);
            double dmax = Convert.ToDouble(textbox_Max_Y.Text);
            double ddec = Convert.ToDouble(textbox_Dec_Y.Text);
            
            PaixMotion.SetSpeedPPS(1, dstart, dacc, ddec, dmax);
        }

        private void Textbox_scan_start_x_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
