using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NotePad {
    public partial class Form1 : Form {

        string str;
        bool autoWrap = true;

        public Form1() {
            InitializeComponent();
            init();
        }
        private void init() {
            textBox1.ScrollBars = ScrollBars.Vertical;
            //开辟控制台进行调试
            ConsoleEx.AllocConsole();
        }
        private void textBox1_TextChanged(object sender, EventArgs e) {
            str=((TextBox)sender).Text;
            Console.WriteLine(str);
        }

        private void 自动换行ToolStripMenuItem_Click(object sender, EventArgs e) {
            autoWrap=!autoWrap;
            自动换行ToolStripMenuItem.Checked = !自动换行ToolStripMenuItem.Checked;
            //如果处于可自动换行状态则转换为不自动换行的状态，
            //那么需要增加横向滚动轴
            if (autoWrap) {
                自动换行ToolStripMenuItem.CheckState = CheckState.Unchecked;
                textBox1.WordWrap = false;
                textBox1.ScrollBars = ScrollBars.Both;
            }
            else {
                自动换行ToolStripMenuItem.CheckState = CheckState.Checked;
                textBox1.WordWrap = true;
                textBox1.ScrollBars = ScrollBars.Vertical;
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Environment.Exit(0);
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void 查看帮助ToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("http://go.microsoft.com/fwlink/?LinkId=517009");
        }
    }
}
