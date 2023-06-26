using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace THz_2D_scan
{
    public partial class GUI
    {

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
