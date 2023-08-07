using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace THz_2D_scan
{
    public partial class GUI
    {
        private void Btn_AbsInc_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name) { 
                case "btn_Inc_Minus_X":
                    PaixMotion.RelMove(0, -Convert.ToDouble(textbox_Distance_X.Text));
                    break;
                case "btn_Inc_Plus_X":
                    PaixMotion.RelMove(0, Convert.ToDouble(textbox_Distance_X.Text));
                    break;
                case "btn_Abs_Minus_X":
                    PaixMotion.AbsMove(0, -Convert.ToDouble(textbox_Distance_X.Text));
                    break;
                case "btn_Abs_Plus_X":
                    PaixMotion.AbsMove(0, Convert.ToDouble(textbox_Distance_X.Text));
                    break;

                case "btn_Inc_Minus_Y":
                    PaixMotion.AbsMove(0, -Convert.ToDouble(textbox_Distance_X.Text));
                    break;
                case "btn_Inc_Plus_Y":
                    PaixMotion.RelMove(1, Convert.ToInt32(textbox_Distance_Y.Text));

                    break;
                case "btn_Abs_Minus_Y":
                    PaixMotion.AbsMove(1, -Convert.ToInt32(textbox_Distance_Y.Text));
                    break;
                case "btn_Abs_Plus_Y":
                    PaixMotion.AbsMove(1, Convert.ToInt32(textbox_Distance_Y.Text));
                    break;
                default:
                break;
            }
        }
        private void Btn_Jog_MouseDown(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "btn_Jog_Left_X":
                    PaixMotion.JogMove(0, 1);
                    break;
                case "btn_Jog_Right_X":
                    PaixMotion.JogMove(0, 0);

                    break;
                case "btn_Jog_Left_Y":
                    PaixMotion.JogMove(1, 1);
                    break;
                case "btn_Jog_Right_Y":
                    PaixMotion.JogMove(1, 0);
                    break;
                default:
                    break;
            }
        }
        private void Btn_Jog_MouseUp(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Name == "btn_Jog_Left_X" ||  button.Name == "btn_Jog_Right_X")
            {
                if (!checkBoxMCJogCont.Checked)
                {
                    Stop(0);
                }
            }
            else if (button.Name == "btn_Jog_Left_Y" || button.Name == "btn_Jog_Right_Y")
            {
                if (!checkBoxMCJogCont.Checked)
                {
                    Stop(1);
                }
            }

        }
        private void Btn_Home_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "btn_Home_X":
                    PaixMotion.HomeMove(0, 2, 0xF, 0); // Go to Home x-position        }
                                                       //            PaixMotion.HomeMove(0, combobox_Home_Mode_X.SelectedIndex, 2, 0);
                    break;
                case "btn_Home_Y":
                    PaixMotion.HomeMove(1, 2, 0xF, 0); // Go to Home y-position        }
                                                       //            PaixMotion.HomeMove(1, combobox_Home_Mode_Y.SelectedIndex, 2, 0);
                    break;

            }
        }
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch(button.Name)
            {
                case "btn_Stop_X":
                    Stop(0);
                    break;
                case "btn_Stop_Y":
                    Stop(1);
                    break;
            }
        }
        private void Label_Cmd_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch ( button.Name)
            {
                case "label_Cmd_X":
                    PaixMotion.SetCmd(0, 0);
                    break;
                case "label_Cmd_Y":
                    PaixMotion.SetCmd(1, 0);
                    break;
            }
        }
        private void Label_Enc_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "label_Enc_X":
                    PaixMotion.SetEnc(0, 0);
                    break;
                case "label_Enc_Y":
                    PaixMotion.SetEnc(1, 0);
                    break;
            }
        }
        private void Stop(short nAxis)
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

        //private void Btn_Inc_Minus_X_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.RelMove(0, -Convert.ToDouble(textbox_Distance_X.Text));
        //}
        //private void Btn_Inc_Plus_X_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.RelMove(0, Convert.ToDouble(textbox_Distance_X.Text));
        //}
        //private void Btn_Abs_Minus_X_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.AbsMove(0, -Convert.ToDouble(textbox_Distance_X.Text));
        //}
        //private void Btn_Abs_Plus_X_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.AbsMove(0, Convert.ToDouble(textbox_Distance_X.Text));
        //}
        //private void Btn_Inc_Minus_Y_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.RelMove(1, -Convert.ToInt32(textbox_Distance_Y.Text));
        //}
        //private void Btn_Inc_Plus_Y_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.RelMove(1, Convert.ToInt32(textbox_Distance_Y.Text));
        //}
        //private void Btn_Abs_Minus_Y_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.AbsMove(1, -Convert.ToInt32(textbox_Distance_Y.Text));
        //}
        //private void Btn_Abs_Plus_Y_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.AbsMove(1, Convert.ToInt32(textbox_Distance_Y.Text));
        //}

        //private void Btn_Jog_Left_X_MouseDown(object sender, MouseEventArgs e)
        //{
        //    PaixMotion.JogMove(0, 1);
        //}

        //private void Btn_Jog_Left_X_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (!checkBoxMCJogCont.Checked)
        //    {
        //        Stop(0);
        //    }
        //}

        //private void Btn_Jog_Right_X_MouseDown(object sender, MouseEventArgs e)
        //{
        //    PaixMotion.JogMove(0, 0);
        //}

        //private void Btn_Jog_Right_X_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (!checkBoxMCJogCont.Checked)
        //    {
        //        Stop(0);
        //    }
        //}

        //private void Btn_Jog_Left_Y_MouseDown(object sender, MouseEventArgs e)
        //{
        //    PaixMotion.JogMove(1, 1);
        //}

        //private void Btn_Jog_Left_Y_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (!checkBoxMCJogCont.Checked)
        //    {
        //        Stop(1);
        //    }
        //}

        //private void Btn_Jog_Right_Y_MouseDown(object sender, MouseEventArgs e)
        //{
        //    PaixMotion.JogMove(1, 0);
        //}

        //private void Btn_Jog_Right_Y_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (!checkBoxMCJogCont.Checked)
        //    {
        //        Stop(1);
        //    }
        //}

        //private void Btn_Home_X_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.HomeMove(0, 2, 0xF, 0); // Go to Home x-position        }
        //                                       //            PaixMotion.HomeMove(0, combobox_Home_Mode_X.SelectedIndex, 2, 0);
        //}

        //private void Btn_Home_Y_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.HomeMove(1, 2, 0xF, 0); // Go to Home y-position        }
        //                                       //            PaixMotion.HomeMove(1, combobox_Home_Mode_Y.SelectedIndex, 2, 0);
        //}

        //private void Btn_Stop_Y_Click(object sender, EventArgs e)
        //{
        //    Stop(1);
        //}
        //private void Btn_Stop_X_Click(object sender, EventArgs e)
        //{
        //    Stop(0);
        //}

        //private void Label_Cmd_X_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.SetCmd(0, 0);
        //}

        //private void Label_Cmd_Y_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.SetCmd(1, 0);
        //}

        //private void Label_Enc_X_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.SetEnc(0, 0);
        //}

        //private void Label_Enc_Y_Click(object sender, EventArgs e)
        //{
        //    PaixMotion.SetEnc(1, 0);
        //}

    }
}
