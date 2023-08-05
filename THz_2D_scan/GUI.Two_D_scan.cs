using System;
using System.Configuration;
using System.Threading;

namespace THz_2D_scan
{
    public partial class GUI
    {
        Scan _Scan = new Scan();


        private void ScanThreadStart()
        {
            double[] param = { 
                Convert.ToDouble(Textbox_Scan_Start_X.Text),    //start_x
                Convert.ToDouble(Textbox_Scan_End_X.Text),      //end_x
                Convert.ToDouble(Textbox_Scan_Start_Y.Text),    //start_y
                Convert.ToDouble(Textbox_Scan_End_Y.Text),      //end_y
                Convert.ToDouble(Textbox_Scan_Interval_X.Text), //interval_x
                Convert.ToDouble(Textbox_Scan_Interval_Y.Text)  //interval_y
        };
            Thread _thread = new Thread(new ParameterizedThreadStart(_Scan.Run));
            _thread.Start(param);
        }




        private void Btn_Run_Scan_Click(object sender, EventArgs e)
        {
            ScanThreadStart();
        }


        private void Btn_Stop_Scan_Click(object sender, EventArgs e)
        {
            _Scan.keepScan = false;
        }

        private void Btn_Save_Scanrange_Click(object sender, EventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("Textbox_Scan_Start_X");
            config.AppSettings.Settings.Remove("Textbox_Scan_Start_Y");

            config.AppSettings.Settings.Remove("Textbox_Scan_Interval_X");
            config.AppSettings.Settings.Remove("Textbox_Scan_Interval_Y");

            config.AppSettings.Settings.Remove("Textbox_Scan_End_Y");
            config.AppSettings.Settings.Remove("Textbox_Scan_End_X");


            config.AppSettings.Settings.Add("Textbox_Scan_Start_X", Textbox_Scan_Start_X.Text);
            config.AppSettings.Settings.Add("Textbox_Scan_Start_Y", Textbox_Scan_Start_Y.Text);

            config.AppSettings.Settings.Add("Textbox_Scan_Interval_X", Textbox_Scan_Interval_X.Text);
            config.AppSettings.Settings.Add("Textbox_Scan_Interval_Y", Textbox_Scan_Interval_Y.Text);

            config.AppSettings.Settings.Add("Textbox_Scan_End_X", Textbox_Scan_End_X.Text);
            config.AppSettings.Settings.Add("Textbox_Scan_End_Y", Textbox_Scan_End_Y.Text);






        }
    }
}
