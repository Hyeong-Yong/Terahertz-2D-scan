using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace test_MotionControler
{
    public partial class FormInput : Form
    {
        public FormInput()
        {
            InitializeComponent();
        }

        public delegate void delegateClickButtonOK(int val);
        delegateClickButtonOK m_DCB;

        public FormInput(string label, int val, delegateClickButtonOK dcb)
        {
            InitializeComponent();

            label1.Text = label;
            textBox1.Text = val.ToString();
            m_DCB = new delegateClickButtonOK(dcb);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            int val = Convert.ToInt32(textBox1.Text);
            m_DCB(val);
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
