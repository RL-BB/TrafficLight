using System.Windows.Media;
using System;
using System.Configuration;


namespace TrafficLights
{

    /// <summary>
    /// 红绿灯的等待默认时间：红灯30s，黄灯5s，绿灯15s
    /// </summary>
    internal class TrafficLightsTime
    {
        //四种颜色分别为：红、黄、绿、灰
        public static Color Red = Color.FromRgb(255, 0, 0);
        public static Color Yellow = Color.FromRgb(255, 255, 0);
        public static Color Green = Color.FromRgb(0, 255, 0);
        public static Color Gray = Color.FromRgb(86, 86, 86);


        //first second third是在ComboBox.SelectedIndx对应的颜色为first，
        //按红绿灯变化的顺序其他两种颜色的灯分别对应为second third
        public static int firstLightRuntime;
        public static int secondLightRuntime;
        public static int thirdLightRuntime;
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

        public static int s_tLightRuntime;
        //public static int t_fLightRuntime;
        //public static int f_sLightRuntime;
        /// <summary>
        /// 交通灯运行周期时间：firstLightRuntime+secondLightRuntime+gTime
        /// </summary>
        public static int trafficLightsTime;
        /// <summary>
        /// trafficLightsTime*2 默认为（30+5+15）*2==100
        /// </summary>
        public static int trafficLightsTime2;

        //public static int firstLightRuntime1;
        /// <summary>
        /// redLightTime*2 默认为30s*2=60
        /// </summary>
        public static int firstLightRuntime2;
        /// <summary>
        /// 黄灯计时时间，默认为5s
        /// </summary>

        //下两行代码不要删除为了理解第三行而存在
        //public static int secondLightRuntime1;
        public static int secondLightRuntime2;
        public static int thirdLightRuntime1;
        /// <summary>
        /// greenLightTime*2 默认为15s*2=30
        /// </summary>
        public static int thirdLightRuntime2;
        private string oneLightUp;

        /// <summary>
        /// flicker ['flɪkɚ] 闪烁，闪光 倒计时文本闪烁倒计时，每0.5s一次
        /// </summary>
        //public static int lightFlicker;

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
        public static void InitializeLightsUpSequenceColor(int selectedIndex)
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
        /// 字体颜色
        /// </summary>
        /// <param name="tempTime">当前状态灯的运行时间，后5s为动画</param>
        /// <param name="frontColor">当前状态灯的颜色</param>
        /// <param name="lateColor">下一灯的颜色</param>
        public static void TextFontColor(int tempTime, Color frontColor, Color lateColor)
        {
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
            //使用此函数时，需要提前把将用到的参数准备好，
            //包括：firstLightUpColor、secondLightUpColor、thirdLightUpColor
            //灯的颜色、字体颜色（包括后5s动画）、倒计时、为下次循环准备的数据
            if (trafficLightsTime2 > s_tLightRuntime)
            {
                FirstLightUpColor(selectedIndex);//灯的颜色
                TextFontColor(firstLightRuntime2, firstLightUpColor, secondLightUpColor);//字体颜色（包括后5s的动画）
                countdownToUI = DblCountdownToUI(firstLightRuntime2);//倒计时
                trafficLightsTime2--;//自减，为进下次循环处理数据
                firstLightRuntime2--;
            }
            else if ((trafficLightsTime2 <= s_tLightRuntime) && (trafficLightsTime2 > thirdLightRuntime1))
            {//灯的颜色、字体颜色（包括后5s动画）、倒计时、为下次循环准备的数据
                SecondLightUpColor(selectedIndex);
                TextFontColor(secondLightRuntime2, secondLightUpColor, thirdLightUpColor);
                countdownToUI = DblCountdownToUI(secondLightRuntime2);
                trafficLightsTime2--;
                secondLightRuntime2--;
            }
            else if (trafficLightsTime2 <= thirdLightRuntime1)
            {
                ThirdLightUpColor(selectedIndex);
                TextFontColor(thirdLightRuntime2, thirdLightUpColor, firstLightUpColor);
                countdownToUI = DblCountdownToUI(thirdLightRuntime2);
                trafficLightsTime2--;
                thirdLightRuntime2--;
            }
        }
        private static int DblCountdownToUI(int countdownTime)
        {
            return (countdownTime + 1) / 2;
        }
        public static int TrafficLightsRuntime()
        {
            trafficLightsTime = firstLightRuntime + secondLightRuntime + thirdLightRuntime;
            return trafficLightsTime;
        }

        /// <summary>
        /// 设置mainTimer.Tick所用的数据:
        /// firstLightRuntime/secondLightRuntime/thirdLightRuntime的几种组合数据
        /// </summary>
        public static void InitializeParamsInMainTimer()
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
            trafficLightsTime2 = trafficLightsTime * 2;

            //强制转换至某一颜色的灯后，会用到的变量
            s_tLightRuntime = (secondLightRuntime + thirdLightRuntime) * 2;
            //t_fLightRuntime = (thirdLightRuntime + firstLightRuntime) * 2;
            //f_sLightRuntime = (firstLightRuntime + secondLightRuntime) * 2;
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
        private static void AllLightUpColor()
        {
            rLColor = Red;
            yLColor = Yellow;
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


        //读config文件，赋值给RedCount、YellowCount、GreenCount
        public static int RdLightCd(string lightCount)
        {//
            var strCountdown = ConfigurationManager.AppSettings[lightCount];
            return Convert.ToInt32(strCountdown);
        }
        public static void WrtLightCd(string rCount,string yCount,string gCount)
        {
            //string sectionName = "appSettings";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (rCount!=null)
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



        public static void InitializePerLightRunTime(int selectedIndex,string rName,string yName,string gName)
        {
           
            switch (selectedIndex)
            {
                case 0:
                    RdAllLightsCd(rName, yName, gName);
                    break;
                case 1:
                    RdAllLightsCd(yName, gName, rName);
                    break;
                case 2:
                    RdAllLightsCd(gName, rName, yName);
                    break;
                default:
                    RdAllLightsCd(rName, yName, gName);
                    break;
            }
        }

        public static void RdAllLightsCd(string fName, string sName, string tName)
        {
            firstLightRuntime = RdLightCd(fName);
            secondLightRuntime = RdLightCd(sName);
            thirdLightRuntime = RdLightCd(tName);
        }






        /// <summary>
        /// 通过获取当前灯的颜色来判断否是绿灯
        /// </summary>
        /// <returns></returns>
        public bool IsGreenLight()
        {

            bool isGreenlight = false;
            ////if (LightUpColor() == "绿色")
            //if (oneLightUp == "绿色")//
            //{
            //    isGreenlight = !false;
            //}
            return isGreenlight;
        }
    }
}
