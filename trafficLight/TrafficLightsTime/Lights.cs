using System.Windows.Media;
using System;
using System.Configuration;
using System.Windows.Threading;


namespace TrafficLights
{

    /// <summary>
    /// 红绿灯的等待默认时间：红灯30s，黄灯5s，绿灯15s
    /// </summary>
    internal class Lights
    {
        private Lights()
        { }
        //四种颜色分别为：红、黄、绿、灰
        public static Color Red = Color.FromRgb(255, 0, 0);
        public static Color Yellow = Color.FromRgb(255, 255, 0);
        public static Color Green = Color.FromRgb(0, 255, 0);
        public static Color Gray = Color.FromRgb(86, 86, 86);

        public static DispatcherTimer mainCycleTimer = new DispatcherTimer();
        public static DispatcherTimer unitCycleTimer = new DispatcherTimer();
        //first second third是在ComboBox.SelectedIndx对应的颜色为first，
        //按红绿灯变化的顺序其他两种颜色的灯分别对应为second third
        public static int firstLightRuntime;
        public static int secondLightRuntime;
        public static int thirdLightRuntime;
        /// <summary>
        /// flicker ['flɪkɚ] 闪烁，闪光 倒计时文本闪烁倒计时，每0.5s一次
        /// </summary>
        //public static int lightFlicker;

        public static int s_tLightRuntime;
        //public static int t_fLightRuntime;
        //public static int f_sLightRuntime;
        /// <summary>
        /// 交通灯运行周期时间：firstLightRuntime+secondLightRuntime+thirdLightTime
        /// </summary>
        public static int trafficLightsTime;

        //分别对对应灯实际运行时间的两倍，Runtime1当常量使用，Runtime2当Countdown使用
        /// <summary>
        /// 当做常量使用    默认值（30+5+15）*2==100
        /// </summary>
        public static int trafficLightsTime1;
        public static int trafficLightsTime2;

        //public static int firstLightRuntime1;
        public static int firstLightRuntime2;
        //public static int secondLightRuntime1;
        public static int secondLightRuntime2;
        public static int thirdLightRuntime1;
        public static int thirdLightRuntime2;

        //当前灯状态倒计时
        public static int countdownToUI;

        public static Color firstLightUpColor;
        public static Color secondLightUpColor;
        public static Color thirdLightUpColor;
        /// <summary>
        /// 红灯的颜色
        /// </summary>
        public static Color rLColor;
        /// <summary>
        /// 黄灯的颜色
        /// </summary>
        public static Color yLColor;
        /// <summary>
        /// 绿灯的颜色
        /// </summary>
        public static Color gLColor;
        //public static Color grayLColor;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public static Color textFontColor;
        //private string oneLightUp;

        #region  Here have params for AddTime
        //单位均为0.5s。
        public static int xCurStatusTimeLeft;//当前周期运行至的数字（从高到底）
        //public static int xCurStatusTime;
        public static int beforeXStatusTime;
        public static int afterXStatusTime;
        /// <summary>
        /// xTrafficLightsTime2=trafficLightsTime2
        /// </summary>
        //public static int xTrafficLightsTime2;

        //public string xCurStatus;
        //public string beforeCurStatus;
        //public string afterCurStatus;
        //public string addTimeResult01;
        //public string addTimeResult02;
        //public string addTimeResult03;
        //public string addTimeResult11;
        //public string addTimeResult12;
        //public string addTimeResult13;

        //public static int sumInterval;
        public static int addTime;
        /// <summary>
        /// 当addTime小于sumInterval的时候，validTime=addTime
        /// </summary>
        public static int validTime;//=addTime%sumInterval
        /// <summary>
        /// 经历过了几个周期(afterXStatus+beforeXStatus+Xstatus).Time
        /// </summary>
        public static int nCycle;//=addTime/sumInterval;

        //当前字体的主颜色，用来判断是哪个灯在亮
        public static Color xCurStatusColor;
        public static Color beforeXStatusColor;
        public static Color afterXStatusColor;
        #endregion


        /// <summary>
        /// Restart Timer，具体操作为给Timer.IsEnabled赋值为ture<==>Timer.Start()
        /// </summary>
        public static void ProceedTimer()
        {
            mainCycleTimer.Interval = TimeSpan.FromMilliseconds(trafficLightsTime * 1000 + 1000);
            //unitCycleTimer.Interval = TimeSpan.FromMilliseconds(500);//0.5s=500ms
            mainCycleTimer.IsEnabled = !false;
            unitCycleTimer.IsEnabled = !false;
        }
        /// <summary>
        /// Stop Timer 如果计时器.start()之后将IsEnable赋值为False，则计时器会停止，Tick事件不会再发生
        /// </summary>
        public static void StopTimer()
        {
            unitCycleTimer.IsEnabled = false;
            mainCycleTimer.IsEnabled = false;
        }

        /// <summary>
        /// ComboBox.SelectedIndex确定的情况下，交通灯亮起的第一盏灯为：0,red;1,yellow;2,green;
        /// </summary>
        /// <param name="selectedIndex"></param>
        public static void FirstLightUpColor(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    RedLightUpColor();
                    //firstLightUpColor = Red;
                    break;
                case 1:
                    YellowLightUpColor();
                    //firstLightUpColor = Yellow;
                    break;
                case 2:
                    GreenLightUpColor();
                    //firstLightUpColor = Green;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ComboBox.SelectedIndex确定的情况下，交通灯亮起的第二盏灯为：0,yellow;1,green;2,red;
        /// </summary>
        /// <param name="selectedIndex"></param>
        public static void SecondLightUpColor(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    YellowLightUpColor();
                    //secondLightUpColor = Yellow;
                    break;
                case 1:
                    GreenLightUpColor();
                    //secondLightUpColor = Green;
                    break;
                case 2:
                    RedLightUpColor();
                    //secondLightUpColor = Red;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ComboBox.SelectedIndex确定的情况下，交通灯亮起的第三盏灯为：0,green;1,red;2,yellow;
        /// </summary>
        /// <param name="selectedIndex"></param>
        public static void ThirdLightUpColor(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    GreenLightUpColor();
                    //thirdLightUpColor = Green;
                    break;
                case 1:
                    RedLightUpColor();
                    //thirdLightUpColor = Red;
                    break;
                case 2:
                    YellowLightUpColor();
                    //thirdLightUpColor = Yellow;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ComboBox.SelectedIndex确定的情况下，交通灯颜色变化的顺序(TextFontColor)：
        /// 0:red,yellow,green;1:yellow,green,red;2:green,red,yellow
        /// </summary>
        /// <param name="selectedIndex"></param>
        public static void IniLightsUpSequenceColor(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    firstLightUpColor = Red;
                    secondLightUpColor = Yellow;
                    thirdLightUpColor = Green;
                    break;
                case 1:
                    firstLightUpColor = Yellow;
                    secondLightUpColor = Green;
                    thirdLightUpColor = Red;
                    break;
                case 2:
                    firstLightUpColor = Green;
                    secondLightUpColor = Red;
                    thirdLightUpColor = Yellow;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 当前灯状态，后5s内的动画
        /// </summary>
        /// <param name="tempTime">当前状态灯的运行时间，后5s为动画</param>
        /// <param name="frontColor">当前状态灯的颜色</param>
        /// <param name="lateColor">下一灯的颜色</param>
        public static void TextFontColor(int tempTime, Color frontColor, Color lateColor)
        {
            xCurStatusColor = frontColor;//当前处于的状态
            if (tempTime >= 10)
            {
                textFontColor = frontColor;
            }
            else
            {
                //动画，偶数的时候为lateColor,奇数的时候为frontColor
                if (tempTime % 2 == 1)//
                {
                    textFontColor = lateColor;
                }
                else
                {
                    textFontColor = frontColor;
                }
            }
        }
        /// <summary>
        /// 每个周期内三种灯色的循环，倒计时，
        /// </summary>
        /// <param name="selectedIndex">ComboBox.SelectedIndex</param>
        public static void LightsUp(int selectedIndex)
        {
            //xTrafficLightsTime2 = trafficLightsTime2;
            //使用此函数时，需要提前把将用到的参数准备好，
            //包括：firstLightUpColor、secondLightUpColor、thirdLightUpColor
            //灯的颜色、字体颜色（包括后5s动画）、倒计时、为下次循环准备的数据
            if (trafficLightsTime2 > s_tLightRuntime)
            {
                #region 重构之前的语句：对方法LightsUp(int,curColor,afterColor)的理解。勿删
                //FirstLightUpColor(selectedIndex);//灯的颜色
                //TextFontColor(firstLightRuntime2, firstLightUpColor, secondLightUpColor);//字体颜色（包括后5s的动画）
                //countdownToUI = DblCountdownToUI(firstLightRuntime2);//倒计时
                //trafficLightsTime2--;//自减，为进下次循环处理数据
                //firstLightRuntime2--;
                #endregion
                FirstLightUpColor(selectedIndex);//灯的颜色
                LightsUp(firstLightRuntime2, firstLightUpColor, secondLightUpColor);
            }
            else if ((trafficLightsTime2 <= s_tLightRuntime) && (trafficLightsTime2 > thirdLightRuntime1))
            {//灯的颜色、字体颜色（包括后5s动画）、倒计时、为下次循环准备的数据
                SecondLightUpColor(selectedIndex);
                LightsUp(secondLightRuntime2, secondLightUpColor, thirdLightUpColor);
            }
            else if (trafficLightsTime2 <= thirdLightRuntime1)
            {
                ThirdLightUpColor(selectedIndex);
                LightsUp(thirdLightRuntime2, thirdLightUpColor, firstLightUpColor);
            }
        }
        /// <summary>
        /// 当前样色的灯的Counttime及自减&&字体动画&&
        /// trafficLightsTime2自减&&当前倒计时时间CountdownToUI&&
        /// 赋值给xCurStatusTime
        /// </summary>
        /// <param name="curLightRuntime2"></param>
        /// <param name="curLightColor"></param>
        /// <param name="afterLightColor"></param>
        private static int LightsUp(int curLightRuntime2, Color curLightColor, Color afterLightColor)
        {
            //xCurStatusTime = curLightRuntime2;
            TextFontColor(curLightRuntime2, curLightColor, afterLightColor);
            countdownToUI = DblCountdownToUI(curLightRuntime2);
            //curLightRuntime2--;//值类型，引用类型
            if (trafficLightsTime2 > s_tLightRuntime)
            {
                xCurStatusTimeLeft = firstLightRuntime2;
                trafficLightsTime2--;
                return firstLightRuntime2--;
            }
            else if ((trafficLightsTime2 <= s_tLightRuntime) && (trafficLightsTime2 > thirdLightRuntime1))
            {
                xCurStatusTimeLeft = secondLightRuntime2;
                trafficLightsTime2--;
                return secondLightRuntime2--;
            }
            else if (trafficLightsTime2 <= thirdLightRuntime1)
            {
                xCurStatusTimeLeft = thirdLightRuntime2;

                trafficLightsTime2--;
                return thirdLightRuntime2--;
            }
            else
            {
                trafficLightsTime2--;
                return 110;
            }

        }

        /// <summary>
        /// 设置mainTimer.Tick所用的数据:
        /// firstLightRuntime/secondLightRuntime/thirdLightRuntime的几种组合数据
        /// </summary>
        public static void IniParamsInMainTimer()
        {
            trafficLightsTime = TrafficLightsRuntime(); ;

            //作为“常量使用”
            //firstLightRuntime1 = firstLightRuntime * 2;
            //secondLightRuntime1 = secondLightRuntime * 2;
            thirdLightRuntime1 = thirdLightRuntime * 2;
            //返回倒计时数字(文本)
            firstLightRuntime2 = firstLightRuntime * 2;
            secondLightRuntime2 = secondLightRuntime * 2;
            thirdLightRuntime2 = thirdLightRuntime * 2;

            trafficLightsTime1 = trafficLightsTime * 2;
            trafficLightsTime2 = trafficLightsTime * 2;

            //亮起的second和third灯运行时间的和
            s_tLightRuntime = (secondLightRuntime + thirdLightRuntime) * 2;
            //t_fLightRuntime = (thirdLightRuntime + firstLightRuntime) * 2;
            //f_sLightRuntime = (firstLightRuntime + secondLightRuntime) * 2;
        }
        /// <summary>
        /// 三个cd 之和 ==Sum
        /// </summary>
        /// <returns>三个cd之和 当做常量使用</returns>
        public static int TrafficLightsRuntime()
        {
            trafficLightsTime = firstLightRuntime + secondLightRuntime + thirdLightRuntime;
            return trafficLightsTime;
        }
        private static int DblCountdownToUI(int countdownTime)
        {
            return (countdownTime + 1) / 2;
        }

        private static void RedLightUpColor()
        {
            rLColor = Red;
            yLColor = Gray;
            gLColor = Gray;
        }
        private static void YellowLightUpColor()
        {
            rLColor = Gray;
            yLColor = Yellow;
            gLColor = Gray;
        }
        private static void GreenLightUpColor()
        {
            rLColor = Gray;
            yLColor = Gray;
            gLColor = Green;
        }


        /// <summary>
        /// 获得数字：把字符串转换为数字，不可转化为数字时返回0
        /// </summary>
        /// <param name="tbText"></param>
        /// <returns></returns>
        public static int FetchNum(string tbText)
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
        /// 一个值如果改变，则返回改变后的值strValue，没有则返回null；
        /// </summary>
        /// <param name="isChanged">是否改变了</param>
        /// <param name="strValue">改变的值or ＮＵＬＬ</param>
        /// <returns></returns>
        public static string IsValueChanged(bool isChanged, string strValue)
        {
            string returnValue = null;
            if (isChanged)
            {
                returnValue = strValue;
            }
            return returnValue;
        }
        public static bool IsTextChanged(string textBefore, string textAfter)
        {
            //得加入记录 三个lightTet是否改动的记录
            bool isTextChanged = false;
            if (IsInputTextSuitable(textAfter))
            {
                if (textBefore != textAfter)
                {
                    isTextChanged = !false;
                }
            }
            return isTextChanged;
        }
        public static bool IsInputTextSuitable(string textAfter)
        {
            bool isSuitable = false;
            //int tempTextBefore = TrafficLightsTime.FetchNum(textBefore);
            int tempTextAfter = FetchNum(textAfter);
            //当且仅当修改后的tempText能转换为5到120的数字时
            if ((tempTextAfter >= 5) && (tempTextAfter <= 120))
            {
                isSuitable = !false;
            }
            return isSuitable;
        }


        /// <summary>
        /// 根据ComboBox.SelectedIndex值：0、1、2,the firstLightUp Color 分别为：红、黄、绿
        /// get everyLightup cd Time
        /// </summary>
        /// <param name="selectedIndex">ComboBox.SelectedIndex</param>
        /// <param name="rName">红灯</param>
        /// <param name="yName">黄灯</param>
        /// <param name="gName">绿灯</param>
        public static void IniPerLightRunTime(int selectedIndex, string rName, string yName, string gName)
        {
            //RdAllLightsCd(string fName, string sName, string tName)
            switch (selectedIndex)
            {
                case 0://灯亮起的顺序：红、黄、绿
                    RdAllLightsCd(rName, yName, gName);
                    break;
                case 1://灯亮起的顺序：黄、绿、红
                    RdAllLightsCd(yName, gName, rName);
                    break;
                case 2://灯亮起的顺序：绿、红、黄
                    RdAllLightsCd(gName, rName, yName);
                    break;
                default://灯亮起的顺序：红、黄、绿
                    RdAllLightsCd(rName, yName, gName);
                    break;
            }
        }
        /// <summary>
        /// 从config中配置三个灯的c，对应关系取决于cbo.SelectedIndex ==>first
        /// 按亮起的顺序依次为first、second、third
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="sName"></param>
        /// <param name="tName"></param>
        public static void RdAllLightsCd(string fName, string sName, string tName)
        {
            firstLightRuntime = RdLightCd(fName);
            secondLightRuntime = RdLightCd(sName);
            thirdLightRuntime = RdLightCd(tName);
        }
        /// <summary>
        /// 读config文件，用来给RedCount、YellowCount、GreenCount赋值
        /// </summary>
        /// <param name="lightCount">appsettings 中的Key</param>
        /// <returns></returns>
        public static int RdLightCd(string lightCount)
        {//
            var strCountdown = ConfigurationManager.AppSettings[lightCount];
            return Convert.ToInt32(strCountdown);
        }
        /// <summary>
        /// 更改App.config中红绿灯的CD
        /// </summary>
        /// <param name="rCount"></param>
        /// <param name="yCount"></param>
        /// <param name="gCount"></param>
        public static void WrtLightCd(string rCount, string yCount, string gCount)
        {
            //string sectionName = "appSettings";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (rCount != null)
            {
                config.AppSettings.Settings["redCount"].Value = rCount;
            }
            if (yCount != null)
            {
                config.AppSettings.Settings["yellowCount"].Value = yCount;
            }
            if (gCount != null)
            {
                config.AppSettings.Settings["greenCount"].Value = gCount;
            }
            //config.AppSettings.Settings["redCount"].Value = rCount;
            //config.AppSettings.Settings["yellowCount"].Value = yCount;
            //config.AppSettings.Settings["greenCount"].Value = gCount;
            config.Save(ConfigurationSaveMode.Full);
            //config.Save();
            ConfigurationManager.RefreshSection("appSettings");

        }

        public static void GetAddTimeParamsValue(int addTimeMW)
        {
            GetCurStatusColor();

            addTime = addTimeMW * 2-xCurStatusTimeLeft; 
            nCycle = addTime / trafficLightsTime1;
            validTime = addTime % trafficLightsTime1;

            if ((trafficLightsTime2 + 1) > s_tLightRuntime)
            {
                ConfirmXStatusIndex(secondLightRuntime, thirdLightRuntime);
            }
            else if (((trafficLightsTime2 + 1) <= s_tLightRuntime) && ((trafficLightsTime2 + 1) > thirdLightRuntime1))
            {
                ConfirmXStatusIndex(thirdLightRuntime, firstLightRuntime);
            }
            else if ((trafficLightsTime2 + 1) <= thirdLightRuntime1)
            {
                ConfirmXStatusIndex(firstLightRuntime, secondLightRuntime);
            }
            //xCurStatusTime = fTime;
            //afterCurStatusTime = sTime;
            //beforeCurStatusTime = tTime;

            //if (nCycle == 0)
            //{
            //    if (validTime <= afterXCurStatusTime)
            //    {
            //    }
            //    else if ((validTime > afterXCurStatusTime) && (validTime <= (afterXCurStatusTime + beforeXCurStatusTime)))
            //    {
            //    }
            //    else if ((validTime > (afterXCurStatusTime + beforeXCurStatusTime)) && (validTime < trafficLightsTime * 2))
            //    {
            //    }
            //}
            //else if (nCycle > 0)
            //{

            //}

        }
        /// <summary>
        /// 判断xCurStatus对应的序号及runtime(秒)，如：firstLightUpRuntime，secondLightUpRuntime,thirdLightUpRuntime
        /// </summary>
        /// <param name="fTime"></param>
        /// <param name="sTime"></param>
        /// <param name="tTime"></param>
        private static void ConfirmXStatusIndex(int sTime, int tTime)
        {
            //xCurStatusTime = fTime*2;
            afterXStatusTime = sTime * 2;
            beforeXStatusTime = tTime * 2;
        }
        /// <summary>
        /// 判断该当前的状态(xStatus)的颜色，推断出各Status对应的颜色
        /// </summary>
        public static void GetCurStatusColor()
        {
            if (xCurStatusColor == Red)
            {
                ConfirmXStatusColor(Green, Yellow);
            }
            else if (xCurStatusColor == Yellow)
            {
                ConfirmXStatusColor(Red, Green);
            }
            else if (xCurStatusColor == Green)
            {
                ConfirmXStatusColor(Yellow, Red);
            }
        }
        private static void ConfirmXStatusColor(Color beforeColor, Color afterColor)
        {
            beforeXStatusColor = beforeColor;
            afterXStatusColor = afterColor;
        }
        public static string ColorToTextString(Color statusColor)
        {
            //string status=null ;
            if (statusColor == Red)
            {
                return "红灯";
            }
            else if (statusColor == Yellow)
            {
                return "黄灯";
            }
            else if (statusColor == Green)
            {
                return "绿灯";
            }
            else
            {
                //处理意外情况
                return "110";
            }
        }
        /// <summary>
        /// 处理数据：(num+1)/2
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int calculate(int num)
        {
            return (num + 1) / 2;
        }







        /// <summary>
        /// 通过获取当前灯的颜色来判断否是绿灯
        /// </summary>
        /// <returns></returns>
        public static bool IsGreenLight()
        {
            bool isGreenlight = false;
            //if (LightUpColor() == "绿色")
            if (xCurStatusColor == Green)//
            {
                isGreenlight = !false;
            }
            return isGreenlight;
        }
        //private static void AllLightUpColor()
        //{
        //    rLColor = Red;
        //    yLColor = Yellow;
        //    gLColor = Green;
        //}
    }
}
