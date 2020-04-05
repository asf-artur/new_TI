using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FuzzyLogic.Forms;

namespace FuzzyLogic
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form1 = new Form1 {Visible = false};
            var fromForParametersSet = new FromForParametersSet(form1);
            Application.Run(fromForParametersSet);
        }
    }
}
