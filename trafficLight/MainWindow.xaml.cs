using System;
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
        //两个个计时器 
        public DispatcherTimer totalCycleTimer = new DispatcherTimer();
        public DispatcherTimer halfSecondCycleTimer = new DispatcherTimer();
        /// <summary>
        /// 红灯倒计时时间输入值，默认为30
        /// </summary>
        public static int rLNumTb;
        /// <summary>
        /// 黄灯倒计时时间输入值，默认为5
        /// </summary>
        public static int yLNumTb;
        /// <summary>
        /// 绿灯倒计时时间输入值，默认为15s
        /// </summary>
        public static int gLNumTb;

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

        #region 灯、计时器、颜色变化
        void LightWaitTime()
        {
            InitialPerLightTime();
            totalCycleTimer.Start();
            totalCycleTimer.Tick += totalCycleTimer_Tick;
            //红灯30s+黄灯5s+绿灯15s=50，参数里直接写数字不合适，应该用有内涵的param代替
            //主计时器要考虑副计时器的延时时间不？1000ms的出现是因为计时器的延迟（halfSecondCycleTimer每次Tick会有10ms-20ms的延迟）
            totalCycleTimer.Interval = TimeSpan.FromMilliseconds((rLNumTb + yLNumTb + gLNumTb) * 1000 + 1000);

            halfSecondCycleTimer.Start();
            halfSecondCycleTimer.Tick += halfSecondCycleTimer_Tick;
            halfSecondCycleTimer.Interval = TimeSpan.FromMilliseconds(500);//0.5s=500ms
        }
        /// <summary>
        /// 主计时器Tick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void totalCycleTimer_Tick(object sender, EventArgs e)
        {


            WaitTime.SetNumForCycleCount(rLNumTb, yLNumTb, gLNumTb);

            //WaitTime.SetNumForCycleCount(30, 5, 15);

        }
        private void halfSecondCycleTimer_Tick(object sender, EventArgs e)
        {
            //mainTimerTb.Text = "主T：" + (totalCycleTimer.Dispatcher.HasShutdownStarted == false ? "Closed" : "Open").ToString();
            //halfSencondTimerTb.Text = "副T：" + (halfSecondCycleTimer.Dispatcher.HasShutdownStarted == false ? "Closed" : "Open").ToString();
            halfSecondCycle(chooseLight.SelectedIndex, WaitTime.textFontColor, WaitTime.rLColor, WaitTime.yLColor, WaitTime.gLColor);
        }
        #endregion

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            //运算符的优先级 ！和&&
            //IsEnable：当计时器.start()之后IsEnable==False，计时器.stop()后IsEnable==Ture
            //if ((totalCycleTimer.IsEnabled) && (halfSecondCycleTimer.IsEnabled))
            //{
            int i = 0;
            if (!firstClick)
            {
                LightWaitTime();
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
            if (rLNumTb < 5)
            {
                MessageBox.Show("不得小于5");
            }
            else if (rLNumTb > 120)
            {
                MessageBox.Show("不得超过2min，应不超过120");
            }
            else
            {
                ConfirmTimerIsEnableThenRestart();
            }

            if (yLNumTb < 5)
            {
                MessageBox.Show("不得小于5");
            }
            else if (yLNumTb > 120)
            {
                MessageBox.Show("不得超过2min，应不超过120");
            }
            else
            {
                ConfirmTimerIsEnableThenRestart();
            }

            if (gLNumTb < 5)
            {
                MessageBox.Show("不得小于5");
            }
            else if (gLNumTb > 120)
            {
                MessageBox.Show("不得超过2min，应不超过120");
            }
            else
            {
                ConfirmTimerIsEnableThenRestart();
            }
        }

        /// <summary>
        /// 重启两个计时器（注意Timer.Start()的顺序，并确认之前的计时器已经关闭）
        /// </summary>
        private void ConfirmTimerIsEnableThenRestart()
        {
            if (!totalCycleTimer.IsEnabled && !halfSecondCycleTimer.IsEnabled)
            {
                totalCycleTimer.Start();
                halfSecondCycleTimer.Start();
            }
            else
            {
                MessageBox.Show("Timers is not ready，wait a moment");
            }

        }

        /// <summary>
        /// 计时器.stop()---（注意Timer.stop()的顺序）
        /// </summary>
        private void StopTimer()
        {
            halfSecondCycleTimer.Stop();
            totalCycleTimer.Stop();
        }
        /// <summary>
        /// 初始化交通灯各倒计时时间
        /// </summary>
        private void InitialPerLightTime()
        {
            //待优化，输入的值： 判断为数字且为int类型
            //红灯和绿灯时间范围为15秒--2分钟，黄灯的时间范围为1秒--15秒
            rLNumTb = Convert.ToInt32(redCount.Text.ToString());
            yLNumTb = Convert.ToInt32(yellowCount.Text.ToString());
            gLNumTb = Convert.ToInt32(greenCount.Text.ToString());
        }

        /// <summary>
        /// ComboBox中选择某种颜色的灯的时候开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LightSelected(object sender, RoutedEventArgs e)
        {
            StopTimer();
            ConfirmTimerIsEnableThenRestart();
        }

        private void btnDisableTimer_Click(object sender, RoutedEventArgs e)
        {
            halfSecondCycleTimer.IsEnabled = false;
            totalCycleTimer.IsEnabled = false;
            mainTimerTb.Text = "主T：" + (totalCycleTimer.IsEnabled == false ? "Dis" : "En").ToString();
            halfSencondTimerTb.Text = "副T：" + (halfSecondCycleTimer.IsEnabled == false ? "Dis" : "En").ToString();
        }

        private void btnEnableTimer_Click(object sender, RoutedEventArgs e)
        {
            halfSecondCycleTimer.IsEnabled = !false;
            totalCycleTimer.IsEnabled = !false;
            mainTimerTb.Text = "主T：" + (totalCycleTimer.IsEnabled == false ? "Dis" : "En").ToString();
            halfSencondTimerTb.Text = "副T：" + (halfSecondCycleTimer.IsEnabled == false ? "Dis" : "En").ToString();
        }
    }
}
