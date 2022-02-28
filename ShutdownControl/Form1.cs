using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ShutdownControl
{
    public partial class Form1 : Form
    {
        private static int count = 1;
        public Form1()
        {
            InitializeComponent();

        }
        //Button按下
        private void button1_Click(object sender, EventArgs e)
        {
            
            timer1.Interval = 1000;//count 1秒
            timer1.Enabled = true; //開啟timer1
            label1.Text = "The computer will power off at" + DateTime.Now.AddHours(1).ToString("HH:mm:ss");//顯示再多久時間會關機

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            count++;
            if (count == 3590)//剩10秒時最後提示
                label1.Text = "The computer will power off...";
            if (count > 3600)//1hr過後會關機
            {

                System.Diagnostics.Process.Start("C:\\WINDOWS\\system32\\shutdown.exe", "-f -s -t 0");
                timer1.Enabled = false;

            }
        }

        //Icon滑鼠雙點擊showForm
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowForm();
        }
        //按下顯示提示時showForm
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            ShowForm();
        }
        //顯示視窗程式
        private void ShowForm()
        {
            if (this.WindowState == FormWindowState.Minimized) 
            {
                //如果目前是縮小狀態 才要回復成一般大小的視窗
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }

            //Activate the form
            this.Activate();
            this.Focus();
        }
        //Menu選單:結束
        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Close();
        }

        /// <summary>
        /// 使用者按下視窗關閉，把它最小化就好
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrayIcon(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;//將視窗最小化
                notifyIcon1.Tag = string.Empty;
                this.notifyIcon1.Visible = true;            //Icon設定隱藏
                this.Hide();                                //視窗隱藏
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
