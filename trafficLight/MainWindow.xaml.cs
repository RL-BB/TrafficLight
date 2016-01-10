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

        #region Here have params:红绿灯的倒计时时间
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
        #endregion

        //窗口初始化时 bool变量默认值为false
        private bool firstClick = !false;

        #region Here have params: 用来判断红绿灯的倒计时时间是否改变 
        private string tempTextRedBefore;
        private string tempTextYellowBefore;
        private string tempTextGreenBefore;
        private string tempTextRedAfter;
        private string tempTextYellowAfter;
        private string tempTextGreenAfter;
        #endregion

        #region Here have params: 计时器Tick事件运行的次数
        /// <summary>
        /// mainTimer.Tick运行的次数
        /// </summary>
        protected int mainTimerCount;
        /// <summary>
        /// unitTimer.Tick事件运行的次数
        /// </summary>
        protected int unitTimerCount;
        #endregion

        public MainWindow()
        {

            InitializeComponent();
            //setLightTimeBtn.Click += setPerLightTimeBtn_Click;
            //mainTimerTb.Text = "110";
            //halfSencondTimerTb.Text = "114"; 
            //mainTimerTb.Text = "主T：" + (totalCycleTimer.IsEnabled == false ? "Closed" : "Open").ToString();
            //halfSencondTimerTb.Text = "副T：" + (halfSecondCycleTimer.IsEnabled == false ? "Closed" : "Open").ToString();
            //mainTimerTb.ToolTip = mainTimerTb.Text.ToString();
            //halfSencondTimerTb.ToolTip = halfSencondTimerTb.Text.ToString();
            ////LightWaitTime();
            mainCycleTimer.Tick += mainCycleTimer_Tick;//delegate ['delɪgət] 委托  EventHandler<>  委托的意义是啥？
            unitCycleTimer.Tick += unitCycleTimer_Tick;

        }

        void RunTrafficLight()
        {
            InitializePerLightRunTime();//***初始化Interval；如果没有此行Interval=0；***
            mainCycleTimer.Start();
            //红灯30s+黄灯5s+绿灯15s=50，参数里直接写数字不合适，应该用有内涵的param代替
            //主计时器要考虑副计时器的延时时间不？1000ms的出现是因为计时器的延迟（halfSecondCycleTimer每次Tick会有10ms-20ms的延迟）
            mainCycleTimer.Interval = TimeSpan.FromMilliseconds((rLNumTbx + yLNumTbx + gLNumTbx) * 1000 + 1000);


            unitCycleTimer.Start();
            unitCycleTimer.Interval = TimeSpan.FromMilliseconds(500);//0.5s=500ms

            //mainCycleTimer.Tick += mainCycleTimer_Tick;//delegate ['delɪgət] 委托  EventHandler<>  委托的意义是啥？
            //unitCycleTimer.Tick += unitCycleTimer_Tick;
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
            //mainTimerCount++;
            //System.Threading.Thread.Sleep(3000);//for Test

            mainTimerTb.Text = "mainTick次数：" + (++mainTimerCount).ToString() + "；" + "mainInterval：" + WaitTime.trafficLightsTime;

        }
        private void unitCycleTimer_Tick(object sender, EventArgs e)
        {
            //mainTimerTb.Text = "主T：" + (totalCycleTimer.Dispatcher.HasShutdownStarted == false ? "Closed" : "Open").ToString();
            //halfSencondTimerTb.Text = "副T：" + (halfSecondCycleTimer.Dispatcher.HasShutdownStarted == false ? "Closed" : "Open").ToString();
            unitTimerCycle(chooseLight.SelectedIndex, WaitTime.textFontColor, WaitTime.rLColor, WaitTime.yLColor, WaitTime.gLColor);
            //if (mainTimerCount >= 2)
            //{
            //    MessageBox.Show("报警：110");
            //    System.Threading.Thread.Sleep(5000);
            //    unitTimerCount = 0;
            //}
            //else
            //{
                halfSencondTimerTb.Text = "unitTick次数：" + (++unitTimerCount) + "；";
                //mainTimerTb.Text = "mainTimerCount次数：" + (++mainTimerCount) + "；";
            //}
            //halfSencondTimerTb.Text = "unitTick次数：" + (++unitTimerCount) + "；";
        }

        /// <summary>
        /// 微循环（0.5s）的动作
        /// </summary>
        /// <param name="selectLightIndex"></param>
        /// <param name="textFontColor"></param>
        /// <param name="rLColor"></param>
        /// <param name="yLColor"></param>
        /// <param name="gLColor"></param>
        private void unitTimerCycle(int selectLightIndex, Color textFontColor, Color rLColor, Color yLColor, Color gLColor)
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

        /// <summary>
        /// 初始化交通灯各倒计时时间
        /// </summary>
        private void InitializePerLightRunTime()
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

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            //运算符的优先级 ！和&&
            //IsEnable：当计时器.start()之后IsEnable==False，计时器.stop()后IsEnable==Ture
            //if ((totalCycleTimer.IsEnabled) && (halfSecondCycleTimer.IsEnabled))
            //{
            if (firstClick)
            {
                RunTrafficLight();
                firstClick = false;
            }
            else
            {
                MessageBox.Show(mainWindow, "请勿多次单击\"'开始'\"按键");
            }
        }

        /// <summary>
        /// 重启两个计时器（注意Timer.Start()的顺序，并确认之前的计时器已经关闭）
        /// </summary>
        private void ReInitializeParamThenRestart()
        {
            //RunTrafficLight();
            InitializePerLightRunTime();
            WaitTime.SetNumForCycleCount(rLNumTbx, yLNumTbx, gLNumTbx);
            //mainCycleTimer.Start();

            //unitCycleTimer.Start();
            ProceedTimer();
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
        /// 重启Timer，具体操作为给Timer.IsEnabled赋值为ture
        /// </summary>
        private void ProceedTimer()
        {
            //mainCycleTimer.Start();
            //unitCycleTimer.Start();
            unitCycleTimer.IsEnabled = !false;
            mainCycleTimer.IsEnabled = !false;
        }

        /// <summary>
        /// ComboBox中选择某种颜色的灯的时候开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LightSelected(object sender, RoutedEventArgs e)
        {
            if (!firstClick)
            {
                RestartTimer();
            }
        }

        private void btnDisableTimer_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            //unitCycleTimer.Stop();
            //mainCycleTimer.Stop();
            ////halfSecondCycleTimer.IsEnabled = false;
            ////totalCycleTimer.IsEnabled = false;
            //mainTimerTb.Text = "主T：" + (mainCycleTimer.IsEnabled == false ? "Dis" : "En").ToString();
            //halfSencondTimerTb.Text = "副T：" + (unitCycleTimer.IsEnabled == false ? "Dis" : "En").ToString();

            //unitCycleTimer.Stop();
            //mainCycleTimer.Stop();
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
            //unitTimerCount = 0;
            //mainTimerCount = 0;
            


        }

        /// <summary>
        /// 获得数字：把字符串转换为数字，不可转化为数字时返回0
        /// </summary>
        /// <param name="tbText"></param>
        /// <returns></returns>
        private int FetchNum(string tbText)
        {
            //待加入判断，输入的tbText没法转成数字咋办？用try()Catch{}处理？
            if (tbText == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(tbText);
            }

        }
        /// <summary>
        /// 判断输入的Text是否在在5-120之间（简单的判断，待加入限制：输入必须为数字且不小于5并不大于120）
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        private bool IsInputTextSuitable(string textBefore, string textAfter)
        {
            bool isSuitable = false;
            int tempTextBefore = FetchNum(textBefore);
            int tempTextAfter = FetchNum(textAfter);
            //if (tempTextBefore == tempTextAfter)//点击按钮设置后，数字没有改变
            //{
            //    MessageBox.Show("报警：110。没有修改 搞毛啊");
            //    return isSuitable;
            //}
            //else if ((tempTextAfter >= 5) && (tempTextAfter <= 120))
            //{
            //    isSuitable = !false;
            //    return isSuitable;
            //}
            //else
            //    return isSuitable;
            if ((tempTextAfter >= 5) && (tempTextAfter <= 120))
            {
                isSuitable = !false;
            }
            return isSuitable;
        }

        /// <summary>
        /// Occurs when the keyboard is focused on this element
        /// </summary>
        /// <param name="sender">element here means TextBox</param>
        /// <param name="e"></param>
        private void LightCount_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            string typeName = sender.GetType().Name;
            TextBox lightText = sender as TextBox;

            switch (typeName)
            {
                case "redCount":
                    tempTextRedBefore = lightText.Text;
                    break;
                case "yellowCount":
                    tempTextYellowBefore = lightText.Text;
                    break;
                case "greenCount":
                    tempTextGreenBefore = lightText.Text;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Occurs when the keyboard is no longer focused on this element
        /// </summary>
        /// <param name="sender">element here means TextBox</param>
        /// <param name="e">？？？？</param>
        private void LightCount_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            string typeName = sender.GetType().Name;
            TextBox lightText = sender as TextBox;
            switch (typeName)
            {
                case "redCount":
                    tempTextRedAfter = lightText.Text;
                    break;
                case "yellowCount":
                    tempTextYellowAfter = lightText.Text;
                    break;
                case "greenCount":
                    tempTextGreenAfter = lightText.Text;
                    break;
                default:
                    break;
            }
        }

        private bool IsTextChanged(string textBefore, string textAfter)
        {
            //得加入记录 三个lightTet是否改动的记录
            bool isChanged = false;
            if (IsInputTextSuitable(textBefore,textAfter))
            {
                if (textBefore != textAfter)
                {
                    isChanged = !false;
                }
            }
            else
            {
                MessageBox.Show("报警：110！");
            }
            return isChanged;
        }

        private void RestartTimer()
        {
            StopTimer();
            ReInitializeParamThenRestart();
        }

        private void setLightTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            bool redCountChanged = IsTextChanged(tempTextRedBefore, tempTextRedAfter);
            bool yellowCountChangded = IsTextChanged(tempTextYellowBefore, tempTextYellowAfter);
            bool greenCountChanged = IsTextChanged(tempTextGreenBefore, tempTextGreenAfter);
            if (redCountChanged || yellowCountChangded || greenCountChanged)
            {
                //没必要使用→→RestartTimer()来RestartTimer，使用ReInitializeParamThenRestart()就够了
                //因为StopTimer()在在按钮进入按钮事件之后就使用了；
                ReInitializeParamThenRestart();
                //RestartTimer();
            }
            else
            {
                ProceedTimer();
            }
            setLightTimeBtn.Foreground = new SolidColorBrush(WaitTime.Green);
        }
    }
}