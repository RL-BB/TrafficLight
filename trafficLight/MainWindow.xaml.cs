using System;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TrafficLights
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //窗口初始化时 bool变量默认值为false
        private bool firstClick = !false;

        private string tempTextRedBefore;
        private string tempTextYellowBefore;
        private string tempTextGreenBefore;
        private string tempTextRedAfter;
        private string tempTextYellowAfter;
        private string tempTextGreenAfter;

        /// <summary>
        /// mainTimer.Tick运行的次数
        /// </summary>
        private int mainTimerCount;
        /// <summary>
        /// unitTimer.Tick事件运行的次数
        /// </summary>
        private int unitTimerCount;

        public MainWindow()
        {

            InitializeComponent();
            //TrafficLightsTime.ReadLightCd(redCount.Text);
            Lights.mainCycleTimer.Tick += MainCycleTimer_Tick;//delegate ['delɪgət] 委托  EventHandler<>  委托的意义是啥？
            Lights.unitCycleTimer.Tick += UnitCycleTimer_Tick;

        }

        void RunTrafficLight()
        {
            ConfigValueToTbx();
            IniPerLightRunTime(chooseLightColor.SelectedIndex);//***初始化Interval；如果没有此行，就获取不到每个交通灯对应的倒计时时间<==>Interval=0；***
            Lights.mainCycleTimer.Start();
            Lights.IniParamsInMainTimer();
            //TrafficLightsTime.mainCycleTimer.Tick += MainCycleTimer_Tick;
            //主计时器要考虑副计时器的延时时间不？1000ms的出现是因为计时器的延迟（halfSecondCycleTimer每次Tick会有10ms-20ms的延迟）
            //mainCycleTimer.Interval = TimeSpan.FromMilliseconds((TrafficLightsTime.firstLightRuntime + TrafficLightsTime.secondLightRuntime + TrafficLightsTime.thirdLightRuntime) * 1000 + 1000);
            Lights.mainCycleTimer.Interval = TimeSpan.FromMilliseconds(Lights.trafficLightsTime * 1000 + 1000);

            Lights.unitCycleTimer.Start();
            Lights.unitCycleTimer.Interval = TimeSpan.FromMilliseconds(500);//0.5s=500ms
        }


        /// <summary>
        /// MainTimer.Tick事件调用的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCycleTimer_Tick(object sender, EventArgs e)
        {
            IniPerLightRunTime(chooseLightColor.SelectedIndex);//交通灯 三个灯的倒计时时间
            Lights.IniParamsInMainTimer();
            Lights.TrafficLightsRuntime();//初始化MainTimer.Interval
            Lights.IniLightsUpSequenceColor(chooseLightColor.SelectedIndex);
            mainTimerTb.Text = "mainTick次数：" + (++mainTimerCount).ToString() + "；" + "mainInterval：" + Lights.trafficLightsTime;

        }
        /// <summary>
        /// 初始化交通灯(红、黄、绿)各自的倒计时时间（firstLightRuntime\secondLightRuntime\thirdLightRuntime）
        /// </summary>
        private void IniPerLightRunTime(int selectedIndex)
        {
            string rName = redCount.Name;
            string yName = yellowCount.Name;
            string gName = greenCount.Name;
            Lights.IniPerLightRunTime(selectedIndex, rName, yName, gName);
        }
        /// <summary>
        /// 给三个用来设置红绿灯运行时间的TextBox.Text赋值
        /// </summary>
        private void ConfigValueToTbx()
        {
            redCount.Text = Lights.RdLightCd(redCount.Name).ToString();
            yellowCount.Text = Lights.RdLightCd(yellowCount.Name).ToString();
            greenCount.Text = Lights.RdLightCd(greenCount.Name).ToString();
        }
        /// <summary>
        /// UnitTimer.Tick事件调用的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnitCycleTimer_Tick(object sender, EventArgs e)
        {
            //为UnitTimerCycle()赋值
            Lights.LightsUp(chooseLightColor.SelectedIndex);
            UnitTimerCycle(Lights.textFontColor, Lights.rLColor, Lights.yLColor, Lights.gLColor);
            halfSencondTimerTb.Text = "unitTick次数：" + (++unitTimerCount) + "；";
        }

        /// <summary>
        /// 微循环（0.5s）的动作（调用控件属性，无法移动到自定义类中）
        /// </summary>
        /// <param name="textFontColor"></param>
        /// <param name="rLColor"></param>
        /// <param name="yLColor"></param>
        /// <param name="gLColor"></param>
        private void UnitTimerCycle(Color textFontColor, Color rLColor, Color yLColor, Color gLColor)
        {
            countdownCurrenLightupTbk.Text = Lights.countdownToUI.ToString();
            countdownLightsTbk.Text = (Lights.trafficLightsTime2 + 1).ToString();
            //倒计时时文本的颜色
            countdownCurrenLightupTbk.Foreground = new SolidColorBrush(textFontColor);
            //倒计时时灯的颜色
            redLight.Fill = new SolidColorBrush(rLColor);
            yellowLight.Fill = new SolidColorBrush(yLColor);
            greenLight.Fill = new SolidColorBrush(gLColor);
        }

        //private void RestartTimer()
        //{
        //    Lights.StopTimer();
        //    ReInitializeParamThenRestart();
        //}
        /// <summary>
        /// 重启两个计时器（注意Timer.Start()的顺序）
        /// </summary>
        private void ReIniParamThenRestart()
        {
            //循环之初要处理的数据（MainTimer_Tick）
            IniPerLightRunTime(chooseLightColor.SelectedIndex);//交通灯 三个灯的倒计时时间
            Lights.IniParamsInMainTimer();
            Lights.IniLightsUpSequenceColor(chooseLightColor.SelectedIndex);

            //每个循环要做的事情（UnitTimer_Tick）
            Lights.LightsUp(chooseLightColor.SelectedIndex);
            countdownCurrenLightupTbk.Text = Lights.countdownToUI.ToString();
            countdownLightsTbk.Text = (Lights.trafficLightsTime2 + 1).ToString();
            countdownCurrenLightupTbk.Foreground = new SolidColorBrush(Lights.textFontColor);
            redLight.Fill = new SolidColorBrush(Lights.rLColor);
            yellowLight.Fill = new SolidColorBrush(Lights.yLColor);
            greenLight.Fill = new SolidColorBrush(Lights.gLColor);

            //启动计时器，第一个Tick发生在Interval时间之后
            Lights.ProceedTimer();
        }

        


        private void btnBegin_Click(object sender, RoutedEventArgs e)
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
            Lights.StopTimer();
        }
        private void btnEnableTimer_Click(object sender, RoutedEventArgs e)
        {
            ReIniParamThenRestart();
        }
        /// <summary>
        /// NoChanges则继续，HaveChangd则重新计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetLightTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            
            //WrtLightCd(string rCount, string yCount, string gCount)
            tempTextRedAfter = Lights.FetchNum(redCount.Text).ToString();
            tempTextYellowAfter = Lights.FetchNum(yellowCount.Text).ToString();
            tempTextGreenAfter = Lights.FetchNum(greenCount.Text).ToString();

            bool redCountChanged = Lights.IsTextChanged(tempTextRedBefore, tempTextRedAfter);
            bool yellowCountChangded = Lights.IsTextChanged(tempTextYellowBefore, tempTextYellowAfter);
            bool greenCountChanged = Lights.IsTextChanged(tempTextGreenBefore, tempTextGreenAfter);

            //把更改后的内容保存到App.config中
            string rCount = Lights.IsValueChanged(redCountChanged, tempTextRedAfter);
            string yCount = Lights.IsValueChanged(yellowCountChangded, tempTextYellowAfter);
            string gCount = Lights.IsValueChanged(greenCountChanged, tempTextGreenAfter);
            Lights.WrtLightCd(rCount, yCount, gCount);


            if (redCountChanged || yellowCountChangded || greenCountChanged)
            {
                Lights.StopTimer();
                ReIniParamThenRestart();
            }
            else
            {
                Lights.ProceedTimer();
            }
            SetLightTimeBtn.Foreground = new SolidColorBrush(Lights.Green);
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
        /// 根据ComboBox的SelectedIndex(序号)对应的ComboBoxItem.Content来判断当前灯的颜色,和倒计时文本的颜色
        /// </summary>
        /// <param name="ComboBoxItemColor"></param>
        private void LightsColor(int comboBoxSelectedItem)
        {
            switch (comboBoxSelectedItem)
            {
                //能把下面有顺序的四行代码给重构掉么!有必要考虑下
                case 0://红、灰、灰
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(Lights.Red);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(Lights.Red);
                    yellowLight.Fill = new SolidColorBrush(Lights.Gray);
                    greenLight.Fill = new SolidColorBrush(Lights.Gray);
                    break;
                case 1://灰、黄、灰
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(Lights.Yellow);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(Lights.Gray);
                    yellowLight.Fill = new SolidColorBrush(Lights.Yellow);
                    greenLight.Fill = new SolidColorBrush(Lights.Gray);
                    break;
                case 2://灰、灰、绿
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(Lights.Green);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(Lights.Gray);
                    yellowLight.Fill = new SolidColorBrush(Lights.Gray);
                    greenLight.Fill = new SolidColorBrush(Lights.Green);
                    break;
                default://红、黄、绿
                    break;
            }
        }
        private void LightsColor(string cbxSelectedItemContent)//重载
        {
            switch (cbxSelectedItemContent)
            {
                //能把下面有顺序的四行代码给重构掉么!有必要考虑下
                case "红"://红、灰、灰
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(Lights.Red);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(Lights.Red);
                    yellowLight.Fill = new SolidColorBrush(Lights.Gray);
                    greenLight.Fill = new SolidColorBrush(Lights.Gray);
                    break;
                case "黄"://灰、黄、灰
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(Lights.Yellow);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(Lights.Gray);
                    yellowLight.Fill = new SolidColorBrush(Lights.Yellow);
                    greenLight.Fill = new SolidColorBrush(Lights.Gray);
                    break;
                case "绿"://灰、灰、绿
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(Lights.Green);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(Lights.Gray);
                    yellowLight.Fill = new SolidColorBrush(Lights.Gray);
                    greenLight.Fill = new SolidColorBrush(Lights.Green);
                    break;
                default://红、黄、绿
                    countdownCurrenLightupTbk.Foreground = new SolidColorBrush(Lights.Red);
                    //倒计时时灯的颜色
                    redLight.Fill = new SolidColorBrush(Lights.Red);
                    yellowLight.Fill = new SolidColorBrush(Lights.Yellow);
                    greenLight.Fill = new SolidColorBrush(Lights.Green);
                    break;
            }
        }

        private void chooseLightColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Lights.StopTimer();
            ComboBoxItem cbItem = sender as ComboBoxItem;
            if (cbItem != null)
            {
                LightsColor(cbItem.Content.ToString());//设置灯和文本的颜色
            }
            if (!firstClick)
            {
                ReIniParamThenRestart();
            }
        }

        /// <summary>
        /// Label:"周期""afterCurStatusColor.Tostring()""beforeCurStatusColor.ToString()""xStatusColor.ToString()"
        /// </summary>
        private void SetXStatusLables()
        {
            //***beforeCurStatusTime----xCurStatusTime-----afterCurStatusTime***//
            int v = Lights.validTime;
            int x = Lights.xCurStatusTimeLeft;
            int a = Lights.afterXStatusTime;
            int b = Lights.beforeXStatusTime;
            int tL1 = Lights.trafficLightsTime1;

            LblNum4.Content = "当前状态：剩余" + Lights.calculate(x) + "秒";
            LblContent4.Content = Lights.ColorToTextString(Lights.xCurStatusColor);
            
            if (Lights.addTime >= tL1)
            {
                LblContent3.Content = "周期";
                LblNum3.Content = Lights.nCycle.ToString("000") + "个";

                //addTime = addTimeMW * 2;
                //nCycle = addTime / trafficLightsTime * 2;
                //validTime = addTime % trafficLightsTime * 2;

                LblContent0.Content = Lights.ColorToTextString(Lights.afterXStatusColor);
                LblContent1.Content = Lights.ColorToTextString(Lights.beforeXStatusColor);
                LblContent2.Content = Lights.ColorToTextString(Lights.xCurStatusColor);
               
                if ((v >= 0) && (v <= a))
                {
                    LblNum0.Content = Lights.calculate(v).ToString("000") + "秒";
                    LblNum1.Content = "000秒";
                    LblNum2.Content = "000秒";
                }
                else if ((v > a) && (v <= (a + b)))
                {
                    LblNum0.Content = Lights.calculate(a).ToString("000") + "秒";
                    LblNum1.Content = Lights.calculate(v - a).ToString("000") + "秒";
                    LblNum2.Content = "000秒";
                }
                else if ((v > (a + b)) && (v < tL1))
                {
                    LblNum0.Content = Lights.calculate(a).ToString("000") + "秒";
                    LblNum1.Content = Lights.calculate(b).ToString("000") + "秒";
                    LblNum2.Content = Lights.calculate(tL1 - a - b).ToString("000") + "秒";
                }
            }
            else
            {
                //Lbl0.Content = "周期";
                //LblNum0.Content = TrafficLightsTime.nCycle.ToString("000") + "个";
                //addTime = addTimeMW * 2;
                //nCycle = addTime / trafficLightsTime * 2;
                //validTime = addTime % trafficLightsTime * 2;

                LblContent0.Content = Lights.ColorToTextString(Lights.afterXStatusColor);
                LblContent1.Content = Lights.ColorToTextString(Lights.beforeXStatusColor);
                LblContent2.Content = Lights.ColorToTextString(Lights.xCurStatusColor);
                if ((v >= 0) && (v <= a))
                {
                    LblNum0.Content = Lights.calculate(v).ToString("000") + "秒";
                    LblNum1.Content = "000" + "秒";
                    LblNum2.Content = "000" + "秒";
                }
                else if ((v > a) && (v <= (a + b)))
                {
                    LblNum0.Content = Lights.calculate(a).ToString("000") + "秒";
                    LblNum1.Content = (v - a).ToString("000") + "秒";
                    LblNum2.Content = "000" + "秒";

                }
                else if ((v > (a + b)) && (v < tL1))
                {
                    LblNum0.Content = Lights.calculate(a).ToString("000") + "秒";
                    LblNum1.Content = Lights.calculate(b).ToString("000") + "秒";
                    LblNum2.Content = Lights.calculate(tL1 - a - b).ToString("000") + "秒";
                }
            }
        }

        private void AddTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            Lights.StopTimer();
            int addTime;
            //没有不能转换为数字、输入数值大小等问题
            if (AddTimeTbx.Text == null)
            {
                addTime = 0;
            }
            else
            {
                addTime = Convert.ToInt32(AddTimeTbx.Text);
            }
            Lights.GetAddTimeParamsValue(addTime);
            SetXStatusLables();
        }
    }
}