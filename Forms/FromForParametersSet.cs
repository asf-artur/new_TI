using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuzzyLogic.Forms
{
    public partial class FromForParametersSet : Form
    {
        private Form1 form1;
        public bool ButtonClicked;

        public void Init()
        {
            var TermNames = @"Низкий
Средний
Высокий";
            var TermValues = @"[160,165)
[165,170)
[170,175)
[175,180)
[180,185)
[185,190)
[190,195)
[195,200)";
            richTextBox1.Text = TermNames;
            richTextBox2.Text = TermValues;
            numericUpDown1.Value = 5;
            ButtonClicked = false;
        }

        public FromForParametersSet()
        {
            InitializeComponent();
            Init();
        }

        public FromForParametersSet(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
            Init();
        }

        private void FromForParametersSet_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ButtonClicked = true;
            var termNames = richTextBox1.Text.Trim().Split().ToList();
            var termValues = richTextBox2.Text.Trim().Split().ToList();
            var expertCount = Convert.ToInt32(numericUpDown1.Value);
            Visible = false;
            form1.Init(expertCount, termNames, termValues);
            form1.Show();
        }

        private void FromForParametersSet_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
