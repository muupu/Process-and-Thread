using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace StartStopProcess
{
    public partial class Form1 : Form
    {
        int fileIndex;
        string fileName = "Notepad.exe";
        Process process1 = new Process();

        public Form1()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.Columns.Add("进程ID", 70, HorizontalAlignment.Left);
            listView1.Columns.Add("进程名称", 70, HorizontalAlignment.Left);
            listView1.Columns.Add("占用内存", 70, HorizontalAlignment.Left);
            listView1.Columns.Add("启动时间", 70, HorizontalAlignment.Left);
            listView1.Columns.Add("文件名", 280, HorizontalAlignment.Left);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            string argument = Application.StartupPath + "\\myfile" + fileIndex + ".txt";
            if (File.Exists(argument) == false)
            {
                File.CreateText(argument);
            }

            ProcessStartInfo ps = new ProcessStartInfo(fileName, argument);
            ps.WindowStyle = ProcessWindowStyle.Normal;
            fileIndex++;
            Process p = new Process();
            p.StartInfo = ps;
            p.Start();
            p.WaitForInputIdle();
            RefreshListView();    
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Process[] myprocesses;
            myprocesses = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(fileName));
            foreach (Process p in myprocesses)
            {
                p.CloseMainWindow();
                Thread.Sleep(1000);
                p.Close();
            }
            fileIndex = 0;
            RefreshListView();
            this.Cursor = Cursors.Default;
        }

        private void RefreshListView()
        {
            listView1.Items.Clear();
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(fileName));
            foreach (Process p in processes)
            {
                ListViewItem item = new ListViewItem(
                    new string[] {
                        p.Id.ToString(),
                        p.ProcessName,
                        string.Format("{0} KB", p.PrivateMemorySize64/1024.0f),
                        string.Format("{0}",p.StartTime),
                        p.MainModule.FileName
                    });
                listView1.Items.Add(item);
            }
        }

        
    }
}
