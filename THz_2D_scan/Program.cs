using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace THz_2D_scan
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new Form1());
            Application.Run(new GUI());
        }
        /* TODO : 
         *   1) 사용자 컨트롤 (클래스 분할)
         *   2) 차트 표현 
         *   3) Lock in amp.와 연동
         */
    }
}
