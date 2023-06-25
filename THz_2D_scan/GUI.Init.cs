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
             * Init : X-axis speed configuration
             */
            textbox_Start_X.Text = ConfigurationManager.AppSettings["textbox_Start_X"];
            textbox_Acc_X.Text = ConfigurationManager.AppSettings["textbox_Acc_X"];
            textbox_Max_X.Text = ConfigurationManager.AppSettings["textbox_Max_X"];
            textbox_Dec_X.Text = ConfigurationManager.AppSettings["textbox_Dec_X"];

            /*
             * Init : Y-axis speed configuration
             */
            textbox_Start_Y.Text = ConfigurationManager.AppSettings["textbox_Start_Y"];
            textbox_Acc_Y.Text = ConfigurationManager.AppSettings["textbox_Acc_Y"];
            textbox_Max_Y.Text = ConfigurationManager.AppSettings["textbox_Max_X"];
            textbox_Dec_Y.Text = ConfigurationManager.AppSettings["textbox_Dec_X"];

            /*
             * Init : X,Y distance value
             */
            textbox_Distance_X.Text = ConfigurationManager.AppSettings["textbox_Distance_X"];
            textbox_Distance_Y.Text = ConfigurationManager.AppSettings["textbox_Distance_Y"];

        }

    }
}
