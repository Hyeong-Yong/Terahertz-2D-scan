using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace THz_2D_scan
{
    public partial class GUI
    {

        private void Btn_Inc_Minus_X_Click(object sender, EventArgs e)
        {
            PaixMotion.RelMove(0, -Convert.ToDouble(textbox_Distance_X.Text));
        }

        private void Btn_Inc_Plus_X_Click(object sender, EventArgs e)
        {
            PaixMotion.RelMove(0, Convert.ToDouble(textbox_Distance_X.Text));
        }

        private void Btn_Abs_Minus_X_Click(object sender, EventArgs e)
        {
            PaixMotion.AbsMove(0, -Convert.ToDouble(textbox_Distance_X.Text));
        }

        private void Btn_Abs_Plus_X_Click(object sender, EventArgs e)
        {
            PaixMotion.AbsMove(0, Convert.ToDouble(textbox_Distance_X.Text));
        }

        private void Btn_Jog_Left_X_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(0, 1);
        }

        private void Btn_Jog_Left_X_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxMCJogCont.Checked)
            {
                stop(0);
            }
        }

        private void Btn_Jog_Right_X_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(0, 0);
        }

        private void Btn_Jog_Right_X_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxMCJogCont.Checked)
            {
                stop(0);
            }
        }

        private void Btn_Inc_Minus_Y_Click(object sender, EventArgs e)
        {
            PaixMotion.RelMove(1, -Convert.ToInt32(textbox_Distance_Y.Text));
        }

        private void Btn_Inc_Plus_Y_Click(object sender, EventArgs e)
        {
            PaixMotion.RelMove(1, Convert.ToInt32(textbox_Distance_Y.Text));
        }

        private void Btn_Abs_Minus_Y_Click(object sender, EventArgs e)
        {
            PaixMotion.AbsMove(1, -Convert.ToInt32(textbox_Distance_Y.Text));
        }

        private void Btn_Abs_Plus_Y_Click(object sender, EventArgs e)
        {
            PaixMotion.AbsMove(1, Convert.ToInt32(textbox_Distance_Y.Text));
        }

        private void Btn_Jog_Left_Y_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(1, 1);
        }

        private void Btn_Jog_Left_Y_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxMCJogCont.Checked)
            {
                stop(1);
            }
        }

        private void Btn_Jog_Right_Y_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(1, 0);
        }

        private void Btn_Jog_Right_Y_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxMCJogCont.Checked)
            {
                stop(1);
            }
        }

        private void Btn_Stop_Y_Click(object sender, EventArgs e)
        {
            stop(1);
        }



        private void Btn_Home_X_Click(object sender, EventArgs e)
        {
            PaixMotion.HomeMove(0, combobox_Home_Mode_X.SelectedIndex, 3, 0);
        }

        private void Btn_Home_Y_Click(object sender, EventArgs e)
        {
            PaixMotion.HomeMove(1, combobox_Home_Mode_Y.SelectedIndex, 3, 0);
        }

        private void Label_Cmd_X_Click(object sender, EventArgs e)
        {
            PaixMotion.SetCmd(0, 0);

        }

        private void Label_Cmd_Y_Click(object sender, EventArgs e)
        {
            PaixMotion.SetCmd(1, 0);

        }
        private void Btn_Stop_X_Click(object sender, EventArgs e)
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

        private void Label_Enc_X_Click(object sender, EventArgs e)
        {
            PaixMotion.SetEnc(0, 0);

        }

        private void Label_Enc_Y_Click(object sender, EventArgs e)
        {
            PaixMotion.SetEnc(1, 0);

        }

    }
}
