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

            mainCycleTimer.Tick += mainCycleTimer_Tick;//delegate ['delɪgət] 委托  EventHandler<>  委托的意义是啥？
            unitCycleTimer.Tick += unitCycleTimer_Tick;

        }

        void RunTrafficLight()
        {
            InitializePerLightRunTime(chooseLightColor.SelectedIndex);//***初始化Interval；如果没有此行，就获取不到每个交通灯对应的倒计时时间<==>Interval=0；***
            mainCycleTimer.Start();
            //主计时器要考虑副计时器的延时时间不？1000ms的出现是因为计时器的延迟（halfSecondCycleTimer每次Tick会有10ms-20ms的延迟）
            mainCycleTimer.Interval = TimeSpan.FromMilliseconds((TrafficLightsTime.redLightCountdown + TrafficLightsTime.yellowLightCountdown + TrafficLightsTime.greenLightCountdown) * 1000 + 1000);

            unitCycleTimer.Start();
            unitCycleTimer.Interval = TimeSpan.FromMilliseconds(500);//0.5s=500ms
        }


        /// <summary>
        /// 主计时器Tick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainCycleTimer_Tick(object sender, EventArgs e)
        {

            TrafficLightsTime.mainTimerInitializeParams();
            mainTimerTb.Text = "mainTick次数：" + (++mainTimerCount).ToString() + "；" + "mainInterval：" + TrafficLightsTime.trafficLightsTime;

        }
        private void unitCycleTimer_Tick(object sender, EventArgs e)
        {
            unitTimerCycle(chooseLightColor.SelectedIndex, TrafficLightsTime.textFontColor, TrafficLightsTime.rLColor, TrafficLightsTime.yLColor, TrafficLightsTime.gLColor);
            halfSencondTimerTb.Text = "unitTick次数：" + (++unitTimerCount) + "；";
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

            countdownCurrenLightupTbk.Text = TrafficLightsTime.countdownToUI;
            countdownLightsTbk.Text = (TrafficLightsTime.trafficLightsTime2 + 1).ToString();
            //倒计时时文本的颜色
            countdownCurrenLightupTbk.Foreground = new SolidColorBrush(textFontColor);
            //倒计时时灯的颜色
            redLight.Fill = new SolidColorBrush(rLColor);
            yellowLight.Fill = new SolidColorBrush(yLColor);
            greenLight.Fill = new SolidColorBrush(gLColor);
        }


        /// <summary>
        /// 初始化交通灯(红、黄、绿)各自的倒计时时间
        /// </summary>
        private void InitializePerLightRunTime()
        {
            TrafficLightsTime.redLightCountdown = FetchNum(redCount.Text);
            TrafficLightsTime.yellowLightCountdown = FetchNum(yellowCount.Text);
            TrafficLightsTime.greenLightCountdown = FetchNum(greenCount.Text);
        }
        private void InitializePerLightRunTime(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    TrafficLightsTime.firstLightRuntime = FetchNum(redCount.Text);
                    TrafficLightsTime.secondLightRuntime = FetchNum(yellowCount.Text);
                    TrafficLightsTime.thirdLightRuntime = FetchNum(greenCount.Text);
                    break;
                case 1:
                    TrafficLightsTime.firstLightRuntime = FetchNum(yellowCount.Text);
                    TrafficLightsTime.secondLightRuntime = FetchNum(greenCount.Text);
                    TrafficLightsTime.thirdLightRuntime = FetchNum(redCount.Text);
                    break;
                case 2:
                    TrafficLightsTime.firstLightRuntime = FetchNum(greenCount.Text);
                    TrafficLightsTime.secondLightRuntime = FetchNum(redCount.Text);
                    TrafficLightsTime.thirdLightRuntime = FetchNum(yellowCount.Text);
                    break;
                default:
                    TrafficLightsTime.firstLightRuntime = FetchNum(redCount.Text);
                    TrafficLightsTime.secondLightRuntime = FetchNum(yellowCount.Text);
                    TrafficLightsTime.thirdLightRuntime = FetchNum(greenCount.Text);
                    break;
            }
        }



        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            //运算符的优先级 ！和&&
            //IsEnable：当计时器.start()之后IsEnable==False，计时器.stop()后IsEnable==Ture
            if (firstClick)
            {
                LightsColor(chooseLightColor.SelectedIndex);//点击"开始"后，把灯和文本的颜色处理了
                RunTrafficLight();
                firstClick = false;
            }
            else
            {
                MessageBox.Show(mainWindow, "请勿多次单击\"'开始'\"按键");
            }
        }

        /// <summary>
        /// 重启两个计时器（注意Timer.Start()的顺序）
        /// </summary>
        private void ReInitializeParamThenRestart()
        {
            InitializePerLightRunTime(chooseLightColor.SelectedIndex);//交通灯 三个灯的倒计时时间
            TrafficLightsTime.mainTimerInitializeParams();
            countdownCurrenLightupTbk.Text = TrafficLightsTime.countdownToUI;
            countdownLightsTbk.Text = (TrafficLightsTime.trafficLightsTime2 + 1).ToString();
            countdownCurrenLightupTbk.Foreground = new SolidColorBrush(TrafficLightsTime.textFontColor);
            redLight.Fill = new SolidColorBrush(TrafficLightsTime.rLColor);
            yellowLight.Fill = new SolidColorBrush(TrafficLightsTime.yLColor);
            greenLight.Fill = new SolidColorBrush(TrafficLightsTime.gLColor);
            ProceedTimer();
        }
        /// <summary>
        /// 停止计时器 如果计时器.start()之后将IsEnable赋值为False，则计时器会停止，Tick事件不会发生
        /// </summary>
        private void StopTimer()
        {
            unitCycleTimer.IsEnabled = false;
            mainCycleTimer.IsEnabled = false;
        }
        /// <summary>
        /// 重启Timer，具体操作为给Timer.IsEnabled赋值为ture<==>Timer.Start()
        /// </summary>
        private void ProceedTimer()
        {
            //mainCycleTimer.Start();
            //unitCycleTimer.Start();
            mainCycleTimer.IsEnabled = !false;
            unitCycleTimer.IsEnabled = !false;
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

        private void btnDisableTimer_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
        }
        private void btnEnableTimer_Click(object sender, RoutedEventArgs e)
        {
            ReInitializeParamThenRestart();
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
        /// 判断输入的Text是否在在5-120之间（简单的判断，待加入限制：输入必须为数字且不小于5并不大于120）数字5和120待用参数来代替
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        private bool IsInputTextSuitable(string textBefore, string textAfter)
        {
            bool isSuitable = false;
            int tempTextBefore = FetchNum(textBefore);
            int tempTextAfter = FetchNum(textAfter);
            //当且仅当修改后的tempText能转换为5到120的数字时
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

        private void RestartTimer()
        {
            StopTimer();
            ReInitializeParamThenRestart();
        }
        /// <summary>
        /// NoChanges则继续，HaveChangd则重新计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setLightTimeBtn_Click(object sender, RoutedEventArgs e)
        {
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

        /// <summary>
        /// 得到当前是什么颜色的灯在亮，并能间接得到其他灯的颜色状态
        /// </summary>
        /// <returns>得到当前是什么颜色的灯在亮</returns>
        public string LightUpColor()
        {
            #region 三种灯的亮灭
            if (TrafficLightsTime.rLColor == TrafficLightsTime.Red)
                TrafficLightsTime.oneLightUp = "红灯";
            if (TrafficLightsTime.yLColor == TrafficLightsTime.Yellow)
                TrafficLightsTime.oneLightUp = "黄灯";
            if (TrafficLightsTime.gLColor == TrafficLightsTime.Green)
                TrafficLightsTime.oneLightUp = "绿灯";
            //灯的颜色只有红黄绿灰四种色，灯亮分别为红、黄、绿，灯灭为黑；
            return TrafficLightsTime.oneLightUp;
            #endregion
        }

        /// <summary>
        /// 通过获取当前灯的颜色来判断否是绿灯
        /// </summary>
        /// <returns></returns>
        public bool IsGreenLight()
        {

            bool isGreenlight = false;
            //if (LightUpColor() == "绿色")
            if (TrafficLightsTime.oneLightUp == "绿色")//
            {
                isGreenlight = !false;
            }
            return isGreenlight;
        }
        /// <summary>
        /// 还没处理好
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnEvent(object sender, RoutedEventArgs e)
        {
            int addTime = 55;
            int addtime2 = addTime * 2;
            int time0 = addTime % TrafficLightsTime.trafficLightsTime2;

            //int time0 = TrafficLightsTime.trafficLightsTime2 + 1;
            int surplusTimeOfLightUp = 0;
            int surPlusTimeOfMainTimer = 0;


            switch (LightUpColor())//当前灯的颜色
            {
                case "红"://红 黄 绿  强制红，默认也为红 ygLTime grLTime ryLtime trafficLightsTime2
                    #region 红30s 黄5s 绿15s 交通灯的颜色变化顺序默认为：红 黄 绿

                    if (time0 > TrafficLightsTime.ygLTime)//红灯30s，后5s时红黄闪烁（默认值为30s）
                    {//红

                    }
                    else if (time0 <= TrafficLightsTime.ygLTime && time0 > TrafficLightsTime.greenLightTime1)//黄灯5s，后5s时黄绿闪烁（默认值为5s）
                    {//黄

                    }
                    else if (time0 <= TrafficLightsTime.greenLightTime1 && time0 >= 0)//绿灯15s，后5s时绿红闪烁（默认值为15s）
                    {//绿

                    }
                    else
                    {
                        //return "BUG";
                    }

                    #endregion
                    break;
                case "黄"://黄 绿 红 强制黄灯开始
                    #region 黄5s 绿15s 红30s 交通灯的颜色变化顺序默认为：红 黄 绿
                    if (time0 > TrafficLightsTime.grLTime)//红灯30s，后5s时红黄闪烁
                    {//黄

                    }
                    else if (time0 <= TrafficLightsTime.grLTime && time0 > TrafficLightsTime.redLightTime1)
                    {//绿

                    }
                    else if (time0 <= TrafficLightsTime.redLightTime1 && time0 > 0)
                    {//红

                    }
                    else
                    {
                        //return "BUG";
                    }
                    #endregion
                    break;
                case "绿"://绿 红 黄 强制绿灯开始
                    #region 绿15s 红30s 黄5s 交通灯的颜色变化顺序默认为：红 黄 绿
                    if (time0 > TrafficLightsTime.ryLTime)//红灯30s，后5s时红黄闪烁
                    {//绿

                    }
                    else if (time0 <= TrafficLightsTime.ryLTime && time0 > TrafficLightsTime.yellowLightTime1)
                    {//红

                    }
                    else if (time0 <= TrafficLightsTime.yellowLightTime1 && time0 > 0)
                    {//黄

                    }
                    else
                    {
                        //return "BUG";
                    }
                    #endregion
                    break;
                default://默认
                    #region 红30s 黄5s 绿15s 交通灯的颜色变化顺序默认为：红 黄 绿

                    if (time0 > TrafficLightsTime.ygLTime)//红灯30s，后5s时红黄闪烁（默认值为30s）
                    {

                    }
                    else if (time0 <= TrafficLightsTime.ygLTime && time0 > TrafficLightsTime.greenLightTime1)//黄灯5s，后5s时黄绿闪烁（默认值为5s）
                    {

                    }
                    else if (time0 <= TrafficLightsTime.greenLightTime1 && time0 > 0)//绿灯15s，后5s时绿红闪烁（默认值为15s）
                    {

                    }
                    else
                    {
                        //return "BUG";
                    }

                    #endregion
                    break;
            }

            //countdownLightsTbk.Text = (TrafficLightsTime.trafficLightsTime2 + 1).ToString();
            //return TrafficLightsTime.lightUp;//返回当前亮的灯
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


    }
}