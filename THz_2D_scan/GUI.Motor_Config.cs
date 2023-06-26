using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using static Paix_MotionController.NMC2;

namespace THz_2D_scan
{
    public partial class GUI
    {

        private void Combobox_Emergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetEmerLogic(combobox_Emergency.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void Combobox_Near_X_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetNearLogic(0, combobox_Near_X.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void Combobox_Near_Y_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetNearLogic(1, combobox_Near_Y.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void Combobox_Limit_Minus_X_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetMinusLimitLogic(0, combobox_Limit_Minus_X.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void Combobox_Limit_Minus_Y_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetMinusLimitLogic(1, combobox_Limit_Minus_Y.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void Combobox_Limit_Plus_X_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetPlusLimitLogic(0, combobox_Limit_Plus_X.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void Combobox_Limit_Plus_Y_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetPlusLimitLogic(1, combobox_Limit_Plus_Y.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void Combobox_Alarm_X_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetAlarmLogic(0, combobox_Alarm_X.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void Combobox_Alarm_Y_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetAlarmLogic(1, combobox_Alarm_Y.SelectedIndex == 0 ? (short)0 : (short)1);
        }

        private void Combobox_Enc_Mode_X_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetEncCountMode(0, (short)combobox_Enc_Mode_X.SelectedIndex);
        }

        private void Combobox_Enc_Mode_Y_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetEncCountMode(1, (short)combobox_Enc_Mode_Y.SelectedIndex);
        }

        private void Combobox_EncZ_X_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetZLogic(0, (short)combobox_EncZ_X.SelectedIndex);
        }

        private void Combobox_EncZ_Y_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetZLogic(1, (short)combobox_EncZ_Y.SelectedIndex);
        }

        private void Combobox_Enc_Dir_X_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetEncInputMode(0, (short)combobox_Enc_Dir_X.SelectedIndex);
        }

        private void Combobox_Enc_Dir_Y_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetEncInputMode(1, (short)combobox_Enc_Dir_Y.SelectedIndex);
        }






        private void Textbox_UnitPerPulse_X_TextChanged(object sender, EventArgs e)
        {
            PaixMotion.SetUnitPulse(0, Convert.ToDouble(textbox_UnitPerPulse_X.Text));
        }

        private void TextBox_UnitPerPulse_Y_TextChanged(object sender, EventArgs e)
        {
            PaixMotion.SetUnitPulse(1, Convert.ToDouble(textBox_UnitPerPulse_Y.Text));
        }

        private void Combobox_Pulse_Mode_X_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetPulseLogic(0, combobox_Pulse_Mode_X.SelectedIndex);
        }

        private void Combobox_Pulse_Mode_Y_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaixMotion.SetPulseLogic(1, combobox_Pulse_Mode_Y.SelectedIndex);
        }




        private void Button1_Click(object sender, EventArgs e)
        {
            PaixMotion.SaveToRom();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            PaixMotion.EraseRom();
        }

        private void Button3_Click(object sender, EventArgs e)
        {

            Paix_MotionController.NMC2.NMCPARALOGIC NmcParaLogic = new Paix_MotionController.NMC2.NMCPARALOGIC();

            PaixMotion.LoadFromRom();

            PaixMotion.GetParaLogic(0, out NmcParaLogic);
            UpdateXParaLogic(ref NmcParaLogic);
            PaixMotion.GetParaLogic(1, out NmcParaLogic);
            UpdateYParaLogic(ref NmcParaLogic);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            PaixMotion.ContTest();
        }

        private void BtnGetList_Click(object sender, EventArgs e)
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

        private void BtnGetLogic_Click(object sender, EventArgs e)
        {
            Paix_MotionController.NMC2.NMCPARALOGIC NmcParaLogic = new Paix_MotionController.NMC2.NMCPARALOGIC();

            PaixMotion.GetParaLogic(0, out NmcParaLogic);
            UpdateXParaLogic(ref NmcParaLogic);
            PaixMotion.GetParaLogic(1, out NmcParaLogic);
            UpdateYParaLogic(ref NmcParaLogic);
        }

        private void BtnSetLogic_Click(object sender, EventArgs e)
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
            combobox_Emergency.SelectedIndex = pNmcParaLogic.nEmg;
            combobox_Enc_Mode_X.SelectedIndex = pNmcParaLogic.nEncCount;
            combobox_EncZ_X.SelectedIndex = pNmcParaLogic.nEncZ;
            combobox_Enc_Dir_X.SelectedIndex = pNmcParaLogic.nEncDir;
            combobox_Near_X.SelectedIndex = pNmcParaLogic.nNear;
            combobox_Limit_Minus_X.SelectedIndex = pNmcParaLogic.nMLimit;
            combobox_Limit_Plus_X.SelectedIndex = pNmcParaLogic.nPLimit;
            combobox_Alarm_X.SelectedIndex = pNmcParaLogic.nAlarm;
            combobox_Pulse_Mode_X.SelectedIndex = pNmcParaLogic.nPulseMode;
        }
        private void UpdateYParaLogic(ref Paix_MotionController.NMC2.NMCPARALOGIC pNmcParaLogic)
        {
            combobox_Enc_Mode_Y.SelectedIndex = pNmcParaLogic.nEncCount;
            combobox_EncZ_Y.SelectedIndex = pNmcParaLogic.nEncZ;
            combobox_Enc_Dir_Y.SelectedIndex = pNmcParaLogic.nEncDir;
            combobox_Near_Y.SelectedIndex = pNmcParaLogic.nNear;
            combobox_Limit_Minus_Y.SelectedIndex = pNmcParaLogic.nMLimit;
            combobox_Limit_Plus_Y.SelectedIndex = pNmcParaLogic.nPLimit;
            combobox_Alarm_Y.SelectedIndex = pNmcParaLogic.nAlarm;
            combobox_Pulse_Mode_Y.SelectedIndex = pNmcParaLogic.nPulseMode;
        }
        private void SetXParaLogic(ref Paix_MotionController.NMC2.NMCPARALOGIC pNmcParaLogic)
        {
            pNmcParaLogic.nEmg = (short)combobox_Emergency.SelectedIndex;
            pNmcParaLogic.nEncCount = (short)combobox_Enc_Mode_X.SelectedIndex;
            pNmcParaLogic.nEncZ = (short)combobox_EncZ_X.SelectedIndex;
            pNmcParaLogic.nEncDir = (short)combobox_Enc_Dir_X.SelectedIndex;
            pNmcParaLogic.nNear = (short)combobox_Near_X.SelectedIndex;
            pNmcParaLogic.nMLimit = (short)combobox_Limit_Minus_X.SelectedIndex;
            pNmcParaLogic.nPLimit = (short)combobox_Limit_Plus_X.SelectedIndex;
            pNmcParaLogic.nAlarm = (short)combobox_Alarm_X.SelectedIndex;
            pNmcParaLogic.nPulseMode = (short)combobox_Pulse_Mode_X.SelectedIndex;
        }
        private void SetYParaLogic(ref Paix_MotionController.NMC2.NMCPARALOGIC pNmcParaLogic)
        {
            pNmcParaLogic.nEncCount = (short)combobox_Enc_Mode_Y.SelectedIndex;
            pNmcParaLogic.nEncZ = (short)combobox_EncZ_Y.SelectedIndex;
            pNmcParaLogic.nEncDir = (short)combobox_Enc_Dir_Y.SelectedIndex;
            pNmcParaLogic.nNear = (short)combobox_Near_Y.SelectedIndex;
            pNmcParaLogic.nMLimit = (short)combobox_Limit_Minus_Y.SelectedIndex;
            pNmcParaLogic.nPLimit = (short)combobox_Limit_Plus_Y.SelectedIndex;
            pNmcParaLogic.nAlarm = (short)combobox_Alarm_Y.SelectedIndex;
            pNmcParaLogic.nPulseMode = (short)combobox_Pulse_Mode_Y.SelectedIndex;
        }

        private void Btn_current_X_Click(object sender, EventArgs e)
        {
            short nCurrentOn = PaixMotion.UpdateAxisInfo(0).nCurrentOn; // CurrentOn 신호 값

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
            short nCurrentOn = PaixMotion.UpdateAxisInfo(0).nCurrentOn; // CurrentOn 신호 값

            if (nCurrentOn == 0)
            {
                PaixMotion.SetCurrentOn(1, 1);
                this.btn_Current_X.BackColor = Color.Green;
            }
            else
            {
                PaixMotion.SetCurrentOn(1, 0);
                this.btn_Current_X.BackColor = Color.Red;
            }

        }

        private void Btn_Current_Y_Click(object sender, EventArgs e)
        {
            short nServoOn = PaixMotion.UpdateAxisInfo(1).nServoOn; // ServoOn 신호 값

            if (nServoOn == 0)
            {
                PaixMotion.SetServoOn(1, 1);
                this.btn_Servo_X.BackColor = Color.Green;
            }
            else
            {
                PaixMotion.SetServoOn(1, 0);
                this.btn_Servo_X.BackColor = Color.Red;
            }
        }

        private void Init_Btn_Status()
        {
            PaixMotion.GetAxesMotionOut(out MotOut);
                        
            if (MotOut.nCurrentOn[0] == 0)
            {
                this.btn_Current_X.BackColor = Color.Red;
            }
            else
            {
                this.btn_Current_X.BackColor = Color.Green;
            }

            if (MotOut.nCurrentOn[1] == 0)
            {
                this.btn_Current_Y.BackColor = Color.Red;
            }
            else
            {
                this.btn_Current_Y.BackColor = Color.Green;
            }

            if (MotOut.nServoOn[0] == 0)
            {
                this.btn_Servo_X.BackColor = Color.Red;
            }
            else
            {
                this.btn_Servo_X.BackColor = Color.Green;
            }

            if (MotOut.nServoOn[1] == 0)
            {
                this.btn_Servo_Y.BackColor = Color.Red;
            }
            else
            {
                this.btn_Servo_Y.BackColor = Color.Green;
            }
        }

        private void Btn_save_config_Click(object sender, EventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("texbtox_Start_X");
            config.AppSettings.Settings.Remove("textbox_Acc_X");
            config.AppSettings.Settings.Remove("textbox_Dec_X");
            config.AppSettings.Settings.Remove("textbox_Max_X");

            config.AppSettings.Settings.Remove("textbox_Start_Y");
            config.AppSettings.Settings.Remove("textbox_Acc_Y");
            config.AppSettings.Settings.Remove("textbox_Dec_X");
            config.AppSettings.Settings.Remove("textbox_Max_X");

            config.AppSettings.Settings.Remove("textbox_Distance_X");
            config.AppSettings.Settings.Remove("textbox_Distance_Y");

            config.AppSettings.Settings.Add("textbox_Start_X", Textbox_Start_X.Text);
            config.AppSettings.Settings.Add("textbox_Acc_X", Textbox_Acc_X.Text);
            config.AppSettings.Settings.Add("textbox_Dec_X", Textbox_Dec_X.Text);
            config.AppSettings.Settings.Add("textbox_Max_X", Textbox_Max_X.Text);

            config.AppSettings.Settings.Add("textbox_Start_Y", Textbox_Start_Y.Text);
            config.AppSettings.Settings.Add("textbox_Acc_Y", Textbox_Acc_Y.Text);
            config.AppSettings.Settings.Add("textbox_Dec_X", Textbox_Dec_X.Text);
            config.AppSettings.Settings.Add("textbox_Max_X", Textbox_Max_X.Text);


            config.AppSettings.Settings.Add("textbox_Distance_X", Textbox_Distance_X.Text);
            config.AppSettings.Settings.Add("textbox_Distance_Y", Textbox_Distance_Y.Text);


            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

    }
}
