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

        //两个个计时器数组，分别都含有四个元素
        //0对应btnOPen_Click事件开启的计时器，
        //public DispatcherTimer[] mainCycleTimer;
        //public DispatcherTimer[] unitCycleTimer;
        public DispatcherTimer mainCycleTimer = new DispatcherTimer();
        public DispatcherTimer unitCycleTimer = new DispatcherTimer();

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

            mainCycleTimer.Tick += MainCycleTimer_Tick;//delegate ['delɪgət] 委托  EventHandler<>  委托的意义是啥？
            unitCycleTimer.Tick += UnitCycleTimer_Tick;

        }

        void RunTrafficLight()
        {
            InitializePerLightRunTime(chooseLightColor.SelectedIndex);//***初始化Interval；如果没有此行，就获取不到每个交通灯对应的倒计时时间<==>Interval=0；***
            TrafficLightsTime.InitializeParamsInMainTimer();
            //主计时器要考虑副计时器的延时时间不？1000ms的出现是因为计时器的延迟（halfSecondCycleTimer每次Tick会有10ms-20ms的延迟）
            //mainCycleTimer.Interval = TimeSpan.FromMilliseconds((TrafficLightsTime.firstLightRuntime + TrafficLightsTime.secondLightRuntime + TrafficLightsTime.thirdLightRuntime) * 1000 + 1000);
            mainCycleTimer.Interval = TimeSpan.FromMilliseconds(TrafficLightsTime.trafficLightsTime * 1000 + 1000);
            mainCycleTimer.Start();

            unitCycleTimer.Interval = TimeSpan.FromMilliseconds(500);//0.5s=500ms
            unitCycleTimer.Start();
        }


        /// <summary>
        /// MainTimer.Tick事件调用的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCycleTimer_Tick(object sender, EventArgs e)
        {
            InitializePerLightRunTime(chooseLightColor.SelectedIndex);//交通灯 三个灯的倒计时时间
            TrafficLightsTime.InitializeParamsInMainTimer();
            TrafficLightsTime.TrafficLightsRuntime();//初始化MainTimer.Interval
            TrafficLightsTime.InitializeLightsUpSequenceColor(chooseLightColor.SelectedIndex);
            mainTimerTb.Text = "mainTick次数：" + (++mainTimerCount).ToString() + "；" + "mainInterval：" + TrafficLightsTime.trafficLightsTime;

        }
        /// <summary>
        /// 初始化交通灯(红、黄、绿)各自的倒计时时间（firstLightRuntime\secondLightRuntime\thirdLightRuntime）
        /// </summary>
        private void InitializePerLightRunTime(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    TrafficLightsTime.firstLightRuntime = TrafficLightsTime.FetchNum(redCount.Text);
                    TrafficLightsTime.secondLightRuntime = TrafficLightsTime.FetchNum(yellowCount.Text);
                    TrafficLightsTime.thirdLightRuntime = TrafficLightsTime.FetchNum(greenCount.Text);
                    break;
                case 1:
                    TrafficLightsTime.firstLightRuntime = TrafficLightsTime.FetchNum(yellowCount.Text);
                    TrafficLightsTime.secondLightRuntime = TrafficLightsTime.FetchNum(greenCount.Text);
                    TrafficLightsTime.thirdLightRuntime = TrafficLightsTime.FetchNum(redCount.Text);
                    break;
                case 2:
                    TrafficLightsTime.firstLightRuntime = TrafficLightsTime.FetchNum(greenCount.Text);
                    TrafficLightsTime.secondLightRuntime = TrafficLightsTime.FetchNum(redCount.Text);
                    TrafficLightsTime.thirdLightRuntime = TrafficLightsTime.FetchNum(yellowCount.Text);
                    break;
                default:
                    TrafficLightsTime.firstLightRuntime = TrafficLightsTime.FetchNum(redCount.Text);
                    TrafficLightsTime.secondLightRuntime = TrafficLightsTime.FetchNum(yellowCount.Text);
                    TrafficLightsTime.thirdLightRuntime = TrafficLightsTime.FetchNum(greenCount.Text);
                    break;
            }
        }
        /// <summary>
        /// UnitTimer.Tick事件调用的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnitCycleTimer_Tick(object sender, EventArgs e)
        {
            //为UnitTimerCycle()赋值
            TrafficLightsTime.LightsUp(chooseLightColor.SelectedIndex);
            UnitTimerCycle(TrafficLightsTime.textFontColor, TrafficLightsTime.rLColor, TrafficLightsTime.yLColor, TrafficLightsTime.gLColor);
            halfSencondTimerTb.Text = "unitTick次数：" + (++unitTimerCount) + "；";
        }
        /// <summary>
        /// 微循环（0.5s）的动作（调用控件属性，无法移动到自定义类中）
        /// </summary>
        /// <param name="selectLightIndex"></param>
        /// <param name="textFontColor"></param>
        /// <param name="rLColor"></param>
        /// <param name="yLColor"></param>
        /// <param name="gLColor"></param>
        private void UnitTimerCycle(Color textFontColor, Color rLColor, Color yLColor, Color gLColor)
        {
            countdownCurrenLightupTbk.Text = TrafficLightsTime.countdownToUI.ToString();
            countdownLightsTbk.Text = (TrafficLightsTime.trafficLightsTime2 + 1).ToString();
            //倒计时时文本的颜色
            countdownCurrenLightupTbk.Foreground = new SolidColorBrush(textFontColor);
            //倒计时时灯的颜色
            redLight.Fill = new SolidColorBrush(rLColor);
            yellowLight.Fill = new SolidColorBrush(yLColor);
            greenLight.Fill = new SolidColorBrush(gLColor);
        }

        private void RestartTimer()
        {
            StopTimer();
            ReInitializeParamThenRestart();
        }
        /// <summary>
        /// 重启两个计时器（注意Timer.Start()的顺序）
        /// </summary>
        private void ReInitializeParamThenRestart()
        {
            //循环之初要处理的数据（MainTimer_Tick）
            InitializePerLightRunTime(chooseLightColor.SelectedIndex);//交通灯 三个灯的倒计时时间
            TrafficLightsTime.InitializeParamsInMainTimer();
            TrafficLightsTime.InitializeLightsUpSequenceColor(chooseLightColor.SelectedIndex);

            //每个循环要做的事情（UnitTimer_Tick）
            TrafficLightsTime.LightsUp(chooseLightColor.SelectedIndex);
            countdownCurrenLightupTbk.Text = TrafficLightsTime.countdownToUI.ToString();
            countdownLightsTbk.Text = (TrafficLightsTime.trafficLightsTime2 + 1).ToString();
            countdownCurrenLightupTbk.Foreground = new SolidColorBrush(TrafficLightsTime.textFontColor);
            redLight.Fill = new SolidColorBrush(TrafficLightsTime.rLColor);
            yellowLight.Fill = new SolidColorBrush(TrafficLightsTime.yLColor);
            greenLight.Fill = new SolidColorBrush(TrafficLightsTime.gLColor);

            //启动计时器，第一个Tick发生在Interval时间之后
            ProceedTimer();
        }
        /// <summary>
        /// Stop Timer 如果计时器.start()之后将IsEnable赋值为False，则计时器会停止，Tick事件不会再发生
        /// </summary>
        private void StopTimer()
        {
            unitCycleTimer.IsEnabled = false;
            mainCycleTimer.IsEnabled = false;
        }
        /// <summary>
        /// Restart Timer，具体操作为给Timer.IsEnabled赋值为ture<==>Timer.Start()
        /// </summary>
        private void ProceedTimer()
        {
            mainCycleTimer.Interval = TimeSpan.FromMilliseconds(TrafficLightsTime.trafficLightsTime * 1000 + 1000);
            //unitCycleTimer.Interval = TimeSpan.FromMilliseconds(500);//0.5s=500ms
            mainCycleTimer.IsEnabled = !false;
            unitCycleTimer.IsEnabled = !false;
        }

                
        /// <summary>
        /// 判断输入的Text是否在在5-120之间（简单的判断，待加入限制：输入必须为数字且不小于5并不大于120）数字5和120待用参数来代替
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        private bool IsInputTextSuitable(string textBefore, string textAfter)
        {
            bool isSuitable = false;
            int tempTextBefore = TrafficLightsTime.FetchNum(textBefore);
            int tempTextAfter = TrafficLightsTime.FetchNum(textAfter);
            //当且仅当修改后的tempText能转换为5到120的数字时
            if ((tempTextAfter >= 5) && (tempTextAfter <= 120))
            {
                isSuitable = !false;
            }
            return isSuitable;
        }
        private bool IsTextChanged(string textBefore, string textAfter)
        {
            //得加入记录 三个lightTet是否改动的记录
            bool isTextChanged = false;
            if (IsInputTextSuitable(textBefore, textAfter))
            {
                if (textBefore != textAfter)
                {
                    isTextChanged = !false;
                }
            }
            return isTextChanged;
        }


        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            //运算符的优先级 ！和&&
            //IsEnable：当计时器.start()之后IsEnable==False，计时器.stop()后IsEnable==Ture
            if (firstClick)
            {
                //点击"开始"后，立即跳至对应的交通灯的颜色（文本&灯）
                LightsColor(chooseLightColor.SelectedIndex);
                RunTrafficLight();
                firstClick = false;
            }
            else
            {
                MessageBox.Show(mainWindow, "请勿多次单击\"'开始'\"按键");
            }
        }
        private void btnDisableTimer_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
        }
        private void btnEnableTimer_Click(object sender, RoutedEventArgs e)
        {
            ReInitializeParamThenRestart();
        }
        /// <summary>
        /// NoChanges则继续，HaveChangd则重新计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setLightTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            tempTextRedAfter = TrafficLightsTime.FetchNum(redCount.Text).ToString();
            tempTextYellowAfter = TrafficLightsTime.FetchNum(yellowCount.Text).ToString();
            tempTextGreenAfter = TrafficLightsTime.FetchNum(greenCount.Text).ToString();


            bool redCountChanged = IsTextChanged(tempTextRedBefore, tempTextRedAfter);
            bool yellowCountChangded = IsTextChanged(tempTextYellowBefore, tempTextYellowAfter);
            bool greenCountChanged = IsTextChanged(tempTextGreenBefore, tempTextGreenAfter);
            if (redCountChanged || yellowCountChangded || greenCountChanged)
            {
                StopTimer();
                ReInitializeParamThenRestart();
            }
            else
            {
                ProceedTimer();
            }
            setLightTimeBtn.Foreground = new SolidColorBrush(TrafficLightsTime.Green);
        }



        /// <summary>
        /// Occurs when the keyboard is focused on this element
        /// </summary>
        /// <param name="sender">element here means TextBox</param>
        /// <param name="e"></param>
        private void LightCount_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //string typeName = sender.GetType().Name;
            TextBox lightText = sender as TextBox;
            string typeName = lightText.Name;

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
            //string typeName = sender.GetType().Name;
            TextBox setLightCountTbx = sender as TextBox;
            string setLightCount = setLightCountTbx.Name;
            switch (setLightCount)
            {
                case "redCount":
                    tempTextRedAfter = setLightCountTbx.Text;
                    break;
                case "yellowCount":
                    tempTextYellowAfter = setLightCountTbx.Text;
                    break;
                case "greenCount":
                    tempTextGreenAfter = setLightCountTbx.Text;
                    break;
                default:
                    break;
            }
        }



        /// <summary>
        /// 根据ComboBox的SelectedIndex(序号)对应的ComboBoxItem.Content来判断当前灯的颜色,和倒计时文本的颜色
        /// </summary>
        /// <param name="ComboBoxItemColor"></param>
        private void LightsColor(int ComboBoxSelectedItem)
        {
            switch (ComboBoxSelectedItem)
            {
                //能把下面有顺序的四行代码给重构掉么!有必要考虑下
                case 0://红、灰、灰
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(TrafficLightsTime.Red);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(TrafficLightsTime.Red);
                    yellowLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    greenLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    break;
                case 1://灰、黄、灰
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(TrafficLightsTime.Yellow);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    yellowLight.Fill = new SolidColorBrush(TrafficLightsTime.Yellow);
                    greenLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    break;
                case 2://灰、灰、绿
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(TrafficLightsTime.Green);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    yellowLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    greenLight.Fill = new SolidColorBrush(TrafficLightsTime.Green);
                    break;
                default://红、黄、绿
                    break;
            }
        }
        private void LightsColor(string ComboBoxSelectedItemContent)//重载
        {
            switch (ComboBoxSelectedItemContent)
            {
                //能把下面有顺序的四行代码给重构掉么!有必要考虑下
                case "红"://红、灰、灰
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(TrafficLightsTime.Red);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(TrafficLightsTime.Red);
                    yellowLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    greenLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    break;
                case "黄"://灰、黄、灰
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(TrafficLightsTime.Yellow);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    yellowLight.Fill = new SolidColorBrush(TrafficLightsTime.Yellow);
                    greenLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    break;
                case "绿"://灰、灰、绿
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(TrafficLightsTime.Green);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    yellowLight.Fill = new SolidColorBrush(TrafficLightsTime.Gray);
                    greenLight.Fill = new SolidColorBrush(TrafficLightsTime.Green);
                    break;
                default://红、黄、绿
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(TrafficLightsTime.Red);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(TrafficLightsTime.Red);
                    yellowLight.Fill = new SolidColorBrush(TrafficLightsTime.Yellow);
                    greenLight.Fill = new SolidColorBrush(TrafficLightsTime.Green);
                    break;
            }
        }

        private void chooseLightColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StopTimer();
            ComboBoxItem cbItem = sender as ComboBoxItem;
            if (cbItem != null)
            {
                LightsColor(cbItem.Content.ToString());//设置灯和文本的颜色
            }
            if (!firstClick)
            {
                ReInitializeParamThenRestart();
            }
        }


        /// <summary>
        /// ComboBox中选择某种颜色的灯的时候开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LightSelected(object sender, RoutedEventArgs e)
        {
            StopTimer();
            ComboBoxItem cbItem = sender as ComboBoxItem;
            LightsColor(cbItem.Content.ToString());//设置灯和文本的颜色
            //2016/01/11 待加入：在选择变换的灯时，立即把对应的颜色跳转过来         
            if (!firstClick)
            {
                RestartTimer();
            }
        }

    }
}