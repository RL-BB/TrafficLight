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
        public DispatcherTimer totalCycle = new DispatcherTimer();
        public DispatcherTimer halfSecondCycle = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            LightWaitTime();
        }

        #region 灯、计时器、颜色变化
        void LightWaitTime()
        {
            totalCycle.Start();
            totalCycle.Tick += totalCycle_Tick;
            //红灯30s+黄灯5s+绿灯15s=50，参数里直接写数字不合适，应该用有内涵的param代替
            totalCycle.Interval = TimeSpan.FromSeconds(50);

            halfSecondCycle.Start();
            halfSecondCycle.Tick += halfSecondCycle_Tick;
            halfSecondCycle.Interval = TimeSpan.FromMilliseconds(500);//0.5s=500ms
        }
        private void totalCycle_Tick(object sender, EventArgs e)
        {
            WaitTime.SetNumForCount();
        }
        private void halfSecondCycle_Tick(object sender, EventArgs e)
        {
            //TextBlock中倒计时显示内容
            tBk0.Text = WaitTime.InitialParam();
            tBkCount.Text = WaitTime.iCount.ToString("000");

            //倒计时时文本的颜色
            tBk0.Foreground = new SolidColorBrush(Color.FromRgb(WaitTime.textFontColorR, WaitTime.textFontColorG, WaitTime.textFontColorB));

            //倒计时时灯的颜色
            redLight.Fill = new SolidColorBrush(Color.FromRgb(WaitTime.redLightColorR, WaitTime.redLightColorG, WaitTime.redLightColorB));
            yellowLight.Fill = new SolidColorBrush(Color.FromRgb(WaitTime.yellowLightColorR, WaitTime.yellowLightColorG, WaitTime.yellowLightColorB));
            greenLight.Fill = new SolidColorBrush(Color.FromRgb(WaitTime.greenLightColorR, WaitTime.greenLightColorG, WaitTime.greenLightColorB));

            //redLight.Opacity = WaitTime.redLightColor;
            //yellowLight.Opacity = WaitTime.yellowLightColor;
            //greenLight.Opacity = WaitTime.greenLightColor;
        }
        #endregion

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            WaitTime.SetNumForCount();
            LightWaitTime();
        }
    }
}
