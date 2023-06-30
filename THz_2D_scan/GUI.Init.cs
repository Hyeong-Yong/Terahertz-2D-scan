using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using static Paix_MotionController.NMC2;

namespace THz_2D_scan
{
    partial class GUI
    {

        private void MotionController_Load(object sender, EventArgs e)
        {
            this.Text = "Motor Controller";
            LoadConfigurationSetting();

        }

        private void LoadConfigurationSetting()
        {


            /*
             * Load : X-axis speed configuration
             */
            textbox_Start_X.Text = ConfigurationManager.AppSettings["textbox_Start_X"];
            textbox_Acc_X.Text = ConfigurationManager.AppSettings["textbox_Acc_X"];
            textbox_Max_X.Text = ConfigurationManager.AppSettings["textbox_Max_X"];
            textbox_Dec_X.Text = ConfigurationManager.AppSettings["textbox_Dec_X"];

            /*
             * Load : Y-axis speed configuration
             */
            textbox_Start_Y.Text = ConfigurationManager.AppSettings["textbox_Start_Y"];
            textbox_Acc_Y.Text = ConfigurationManager.AppSettings["textbox_Acc_Y"];
            textbox_Max_Y.Text = ConfigurationManager.AppSettings["textbox_Max_X"];
            textbox_Dec_Y.Text = ConfigurationManager.AppSettings["textbox_Dec_X"];

            /*
             * Load : X,Y distance value
             */
            textbox_Distance_X.Text = ConfigurationManager.AppSettings["textbox_Distance_X"];
            textbox_Distance_Y.Text = ConfigurationManager.AppSettings["textbox_Distance_Y"];


            /*
             * Load : UnitPerPulse
             */
            textbox_UnitPerPulse_X.Text = ConfigurationManager.AppSettings["textbox_UnitPerPulse_X"];
            textbox_UnitPerPulse_Y.Text = ConfigurationManager.AppSettings["textbox_UnitPerPulse_Y"];


            /*
             * Load : Scan region
             */

            Textbox_Scan_Start_X.Text = ConfigurationManager.AppSettings["Textbox_Scan_Start_X"];
            Textbox_Scan_Start_Y.Text = ConfigurationManager.AppSettings["Textbox_Scan_Start_Y"];

            Textbox_Scan_Interval_X.Text= ConfigurationManager.AppSettings["Textbox_Scan_Interval_X"];
            Textbox_Scan_Interval_Y.Text= ConfigurationManager.AppSettings["Textbox_Scan_Interval_Y"];

            Textbox_Scan_End_X.Text = ConfigurationManager.AppSettings["Textbox_Scan_End_X"];
            Textbox_Scan_End_Y.Text = ConfigurationManager.AppSettings["Textbox_Scan_End_Y"];


        }

    }
}
