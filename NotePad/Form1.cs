using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NotePad {
    public partial class Form1 : Form {
        string title = "";
        string str;
        string fileName="";
        bool autoWrap = true;
        bool saved = true;

        public Form1() {
            InitializeComponent();
            init();
        }
        protected override void OnClosing(CancelEventArgs e) {
            if (!saved) {
                var ret = MessageBox.Show("当前文档未保存，是否保存？",title, MessageBoxButtons.YesNoCancel);
                switch (ret) {
                    case DialogResult.Yes:
                        var saveReturnValue = save();
                        if (saveReturnValue != DialogResult.OK) e.Cancel = true;
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void init() {
            textBox1.ScrollBars = ScrollBars.Vertical;
            //开辟控制台进行调试
            //ConsoleEx.AllocConsole();
            title = this.Text;
            textBox1.KeyUp += new KeyEventHandler(textBox1_KeyUp);
            textBox1.CursorChanged += new EventHandler(textBox1_CursorChanged);
        }
        byte[] byData = new byte[100];
        char[] charData = new char[1000];
        public void Read(String filePath) {
            try {
                string sb=File.ReadAllText(filePath);
                //设置文字
                textBox1.Text = sb.ToString();
                //设置光标到最后
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
        private void Write(String fileName) {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            Console.WriteLine( textBox1.Text);
            byte[] data = System.Text.Encoding.Default.GetBytes(textBox1.Text);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }
        private void textBox1_TextChanged(object sender, EventArgs e) {
            changeStatusBarInfo();
            saved = false;
        }
        private void textBox1_CursorChanged(object sender, EventArgs e) {
            changeStatusBarInfo();
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e) {
            //Ctrl+S
            if (e.Control && e.KeyValue == 'S') {
                保存ToolStripMenuItem_Click(sender,e);
            }
        }
        private void changeStatusBarInfo() {
            int index = textBox1.GetFirstCharIndexOfCurrentLine();//得到当前行第一个字符的索引
            int line = textBox1.GetLineFromCharIndex(index) + 1;//得到当前行的行号,从0开始，习惯是从1开始，所以+1.
            int col = textBox1.SelectionStart - index + 1;//.SelectionStart得到光标所在位置的索
            str = String.Format("第 {0} 行，第 {1} 列", line, col);
            statusBarInfo.Text = str;
        }

        private void 自动换行ToolStripMenuItem_Click(object sender, EventArgs e) {
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
            autoWrap = !autoWrap;
            changeStatusBarInfo();
            textBox1.Cursor.HotSpot.ToString();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Environment.Exit(0);
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e) {

        }
        private void setTitleName() {
            this.Text = fileName + " - 记事本";
        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog of = new OpenFileDialog();
            of.ValidateNames = true;
            of.CheckPathExists = true;
            of.CheckFileExists = true;
            of.Filter = "All Files|*.*";
            if (of.ShowDialog() == DialogResult.OK) {
                this.fileName = of.FileName;
                Console.WriteLine(fileName);
                Read(fileName);
                setTitleName();
            }
            
        }
        private DialogResult save() {
            DialogResult ret = DialogResult.Cancel;
            try {
                if (fileName.Equals("")) {
                    SaveFileDialog sf = new SaveFileDialog();
                    sf.Filter = "Text Document(*.txt)|*.txt|All Files|*.*";
                    if (sf.ShowDialog() == DialogResult.OK) {
                        fileName = sf.FileName;
                        Write(fileName);
                    }
                    setTitleName();
                }
                else
                    Write(fileName);
                ret = DialogResult.OK;
            }
            catch (Exception ex) {
                ret = DialogResult.Cancel;
            }
            return ret;
        }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e) {
            save();
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                    SaveFileDialog sf = new SaveFileDialog();
                    sf.Filter = "Text Document(*.txt)|*.txt|All Files|*.*";
                    if (sf.ShowDialog() == DialogResult.OK) {
                        fileName = sf.FileName;
                        Write(fileName);
                    }
                Console.WriteLine(fileName);
            }
            catch (Exception ex) {
                ex.print();
            }
        }

        private void 查看帮助ToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("http://go.microsoft.com/fwlink/?LinkId=517009");
        }

        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e) {
            FontDialog fd = new FontDialog();
            fd.Font = textBox1.Font;
            if (fd.ShowDialog() == DialogResult.OK) {
                textBox1.Font = fd.Font;
            }
        }

        private void 日期时间ToolStripMenuItem_Click(object sender, EventArgs e) {
            var timeString=DateTime.Now.ToString();
            var sb = new StringBuilder(textBox1.Text);
            var i = textBox1.SelectionStart;
            sb.Insert(i, timeString);
            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = i + timeString.Length;
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e) {
            textBox1.Undo();
        }
    }
    static class Utils {
        public static void print(this Object a) {
            Console.WriteLine(a.ToString());
        }
    }
}
