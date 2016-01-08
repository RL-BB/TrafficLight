﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Timers;

namespace trafficLight
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //两个个计时器数组，分别都含有四个元素
        //0对应btnOPen_Click事件开启的计时器，
        //public DispatcherTimer[] mainCycleTimer;
        //public DispatcherTimer[] unitCycleTimer;
        public DispatcherTimer mainCycleTimer = new DispatcherTimer();
        public DispatcherTimer unitCycleTimer = new DispatcherTimer();

        /// <summary>
        /// 红灯倒计时时间输入值，默认为30
        /// </summary>
        public static int rLNumTbx;
        /// <summary>
        /// 黄灯倒计时时间输入值，默认为5
        /// </summary>
        public static int yLNumTbx;
        /// <summary>
        /// 绿灯倒计时时间输入值，默认为15s
        /// </summary>
        public static int gLNumTbx;

        //窗口初始化时 bool变量默认值为false
        private bool firstClick;

        public MainWindow()
        {
            InitializeComponent();

            mainTimerTb.Text = "110";
            halfSencondTimerTb.Text = "114";
            //mainTimerTb.Text = "主T：" + (totalCycleTimer.IsEnabled == false ? "Closed" : "Open").ToString();
            //halfSencondTimerTb.Text = "副T：" + (halfSecondCycleTimer.IsEnabled == false ? "Closed" : "Open").ToString();
            mainTimerTb.ToolTip = mainTimerTb.Text.ToString();
            halfSencondTimerTb.ToolTip = halfSencondTimerTb.Text.ToString();
            //LightWaitTime();
        }

        void RunTrafficLight()
        {
            InitialPerLightRunTime();
            mainCycleTimer.Start();
            mainCycleTimer.Tick += mainCycleTimer_Tick;//delegate ['delɪgət] 委托  EventHandler<>  委托的意义是啥？
            //红灯30s+黄灯5s+绿灯15s=50，参数里直接写数字不合适，应该用有内涵的param代替
            //主计时器要考虑副计时器的延时时间不？1000ms的出现是因为计时器的延迟（halfSecondCycleTimer每次Tick会有10ms-20ms的延迟）
            mainCycleTimer.Interval = TimeSpan.FromMilliseconds((rLNumTbx + yLNumTbx + gLNumTbx) * 1000 + 1000);


            unitCycleTimer.Start();
            unitCycleTimer.Tick += unitCycleTimer_Tick;
            unitCycleTimer.Interval = TimeSpan.FromMilliseconds(500);//0.5s=500ms
        }
        /// <summary>
        /// 主计时器Tick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainCycleTimer_Tick(object sender, EventArgs e)
        {
            WaitTime.SetNumForCycleCount(rLNumTbx, yLNumTbx, gLNumTbx);
            //WaitTime.SetNumForCycleCount(30, 5, 15);

        }
        private void unitCycleTimer_Tick(object sender, EventArgs e)
        {
            //mainTimerTb.Text = "主T：" + (totalCycleTimer.Dispatcher.HasShutdownStarted == false ? "Closed" : "Open").ToString();
            //halfSencondTimerTb.Text = "副T：" + (halfSecondCycleTimer.Dispatcher.HasShutdownStarted == false ? "Closed" : "Open").ToString();
            halfSecondCycle(chooseLight.SelectedIndex, WaitTime.textFontColor, WaitTime.rLColor, WaitTime.yLColor, WaitTime.gLColor);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            //运算符的优先级 ！和&&
            //IsEnable：当计时器.start()之后IsEnable==False，计时器.stop()后IsEnable==Ture
            //if ((totalCycleTimer.IsEnabled) && (halfSecondCycleTimer.IsEnabled))
            //{
            if (!firstClick)
            {
                RunTrafficLight();
                firstClick = !false;
            }
            else
            {
                MessageBox.Show(mainWindow, "请勿多次单击\"'开始'\"按键");
            }
        }
        /// <summary>
        /// 微循环（0.5s）的动作
        /// </summary>
        /// <param name="selectLightIndex"></param>
        /// <param name="textFontColor"></param>
        /// <param name="rLColor"></param>
        /// <param name="yLColor"></param>
        /// <param name="gLColor"></param>
        private void halfSecondCycle(int selectLightIndex, Color textFontColor, Color rLColor, Color yLColor, Color gLColor)
        {
            //TextBlock中倒计时显示内容，参数1为临时添加 无意义
            tBk0.Text = WaitTime.InitialParam(selectLightIndex);
            tBkCount.Text = (WaitTime.trafficLightsTime2 + 1).ToString();
            //倒计时时文本的颜色
            tBk0.Foreground = new SolidColorBrush(textFontColor);
            //倒计时时灯的颜色
            redLight.Fill = new SolidColorBrush(rLColor);
            yellowLight.Fill = new SolidColorBrush(yLColor);
            greenLight.Fill = new SolidColorBrush(gLColor);
        }

        private void LighCount_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            StopTimer();
            if (rLNumTbx < 5)
            {
                MessageBox.Show("不得小于5");
            }
            else if (rLNumTbx > 120)
            {
                MessageBox.Show("不得超过2min，应不超过120");
            }
            else
            {
                ReInitializeParamThenRestart();
            }

            if (yLNumTbx < 5)
            {
                MessageBox.Show("不得小于5");
            }
            else if (yLNumTbx > 120)
            {
                MessageBox.Show("不得超过2min，应不超过120");
            }
            else
            {
                ReInitializeParamThenRestart();

            }

            if (gLNumTbx < 5)
            {
                MessageBox.Show("不得小于5");
            }
            else if (gLNumTbx > 120)
            {
                MessageBox.Show("不得超过2min，应不超过120");
            }
            else
            {
                ReInitializeParamThenRestart();

            }
        }

        /// <summary>
        /// 重启两个计时器（注意Timer.Start()的顺序，并确认之前的计时器已经关闭）
        /// </summary>
        private void ReInitializeParamThenRestart()
        {
            //RunTrafficLight();
            InitialPerLightRunTime();
            WaitTime.SetNumForCycleCount(rLNumTbx, yLNumTbx, gLNumTbx);
            mainCycleTimer.Start();
            unitCycleTimer.Start();
        }
        /// <summary>
        /// 停止计时器 如果计时器.start()之后将IsEnable赋值为False，则计时器会停止，Tick事件不会发生
        /// </summary>
        private void StopTimer()
        {
            //***比较Timer.IsEnable=False和Timer.stop()***

            //unitCycleTimer.Dispatcher.Thread.Abort();
            unitCycleTimer.IsEnabled = false;
            mainCycleTimer.IsEnabled = false;
            //halfSecondCycleTimer.Stop();
            //totalCycleTimer.Stop();

        }

        /// <summary>
        /// 初始化交通灯各倒计时时间
        /// </summary>
        private void InitialPerLightRunTime()
        {
            //待优化，输入的值： 判断为数字且为int类型
            //红灯和绿灯时间范围为15秒--2分钟，黄灯的时间范围为1秒--15秒
            rLNumTbx = FetchNum(redCount.Text);
            yLNumTbx = FetchNum(yellowCount.Text);
            gLNumTbx = FetchNum(greenCount.Text);
            //rLNumTb = Convert.ToInt32(redCount.Text);
            //yLNumTb = Convert.ToInt32(yellowCount.Text);
            //gLNumTb = Convert.ToInt32(greenCount.Text);
        }

        /// <summary>
        /// ComboBox中选择某种颜色的灯的时候开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LightSelected(object sender, RoutedEventArgs e)
        {
            //if (firstClick)
            //{
                StopTimer();
                ReInitializeParamThenRestart();
            //}

        }

        private void btnDisableTimer_Click(object sender, RoutedEventArgs e)
        {
                                                

            unitCycleTimer.Stop();
            mainCycleTimer.Stop();
            //halfSecondCycleTimer.IsEnabled = false;
            //totalCycleTimer.IsEnabled = false;
            mainTimerTb.Text = "主T：" + (mainCycleTimer.IsEnabled == false ? "Dis" : "En").ToString();
            halfSencondTimerTb.Text = "副T：" + (unitCycleTimer.IsEnabled == false ? "Dis" : "En").ToString();

            unitCycleTimer.Stop();
            mainCycleTimer.Stop();
        }

        private void btnEnableTimer_Click(object sender, RoutedEventArgs e)
        {
            //InitialPerLightRunTime();
            //WaitTime.SetNumForCycleCount(rLNumTbx, yLNumTbx, gLNumTbx);
            //unitCycleTimer.IsEnabled = !false;
            //mainCycleTimer.IsEnabled = !false;
            //mainTimerTb.Text = "主T：" + (mainCycleTimer.IsEnabled == false ? "Dis" : "En").ToString();
            //halfSencondTimerTb.Text = "副T：" + (unitCycleTimer.IsEnabled == false ? "Dis" : "En").ToString();

            ReInitializeParamThenRestart();
        }

        private int FetchNum(string tbText)
        {
            if (tbText == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(tbText);
            }

        }

        private void SelectRed(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (firstClick)
            {
                StopTimer();
                ReInitializeParamThenRestart();
            }
        }
        private void SelectYellow(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (firstClick)
            {
                StopTimer();
                ReInitializeParamThenRestart();
            }
        }
        private void SelectGreen(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (firstClick)
            {
                StopTimer();
                ReInitializeParamThenRestart();
            }
        }
    }
}
