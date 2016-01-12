using System.Windows.Media;

namespace trafficLight
{

    /// <summary>
    /// 红绿灯的等待默认时间：红灯30s，黄灯5s，绿灯15s
    /// </summary>
    class TrafficLightsTime
    {
        //当前在亮的那盏灯
        public static string oneLightUp;
        public static string firstLightUpCountdown;
        public static string secondLightUpCountdown;
        public static string thirdLightUpCountdown;


        public static int firstLightRuntime;
        public static int secondLightRuntime;
        public static int thirdLightRuntime;
        //当前灯状态倒计时
        public static int countdownToUI;
        #region Here have params:定义四种颜色
        //四种颜色分别为：红、黄、绿、灰
        public static Color Red = Color.FromRgb(255, 0, 0);
        public static Color Yellow = Color.FromRgb(255, 255, 0);
        public static Color Green = Color.FromRgb(0, 255, 0);
        public static Color Gray = Color.FromRgb(86, 86, 86);
        #endregion 
         
        #region Here have params:红绿灯的倒计时时间(从界面中获取)
        public static int surplusTime;
        //下一次红、黄、绿分别需要的时间；
        public static int rLNextTime;
        public static int yLNextTime;
        public static int gLNextTime;
        #endregion

        #region Here have params: 交通各颜色变化
        public static Color firstLightUpColor;
        public static Color secondLightUpColor;
        public static Color thirdLightUpColor;
        public static void FirstLightUpColor(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    RedLightUpColor();
                    firstLightUpColor = Red;
                    secondLightUpColor = Yellow;
                    thirdLightUpColor = Green;
                    break;
                case 1:
                    YellowLightUpColor();
                    firstLightUpColor = Yellow;
                    break;
                case 2:
                    GreenLightUpColor();
                    firstLightUpColor = Green;
                    break;
                case 3:
                    break;
            }
        }
        public static void SecondLightUpColor(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    YellowLightUpColor();
                    secondLightUpColor = Yellow;
                    break;
                case 1:
                    GreenLightUpColor();
                    secondLightUpColor = Green;
                    break;
                case 2:
                    RedLightUpColor();
                    secondLightUpColor = Red;
                    break;
                case 3:
                    break;
            }
        }
        public static void ThirdLightUpColor(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    GreenLightUpColor();
                    thirdLightUpColor = Green;
                    break;
                case 1:
                    RedLightUpColor();
                    thirdLightUpColor = Red;
                    break;
                case 2:
                    YellowLightUpColor();
                    thirdLightUpColor = Yellow;
                    break;
                case 3:
                    break;
            }
        }
        public static void AllLightsUpColor(int selectedIndex)
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
                case 3:
                    break;
            }
        }
        public static void TextFontColor(int tempTime,Color frontColor, Color lateColor)
        {
            if (tempTime>10)
            {
                textFontColor = frontColor;
            }
            else
            {
                //动画，偶数的时候为lateColor,奇数的时候为frontColor
                if (tempTime%2==0)//
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
        #endregion

        #region Here have params:红绿灯周期时间
        public static int s_tLightRuntime;
        public static int t_fLightRuntime;
        public static int f_sLightRuntime;
        /// <summary>
        /// 交通灯运行周期时间：firstLightRuntime+secondLightRuntime+gTime
        /// </summary>
        public static int trafficLightsTime;
        /// <summary>
        /// trafficLightsTime*2 默认为（30+5+15）*2==100
        /// </summary>
        public static int trafficLightsTime2;
        /// <summary>
        /// 红灯计时时间，默认为30s
        /// </summary>
        public static int redLightCountdown;
        public static int redLightCountdownText;
        public static int redLightTime1;
        /// <summary>
        /// redLightTime*2 默认为30s*2=60
        /// </summary>
        public static int firstLightRuntime2;
        /// <summary>
        /// 黄灯计时时间，默认为5s
        /// </summary>
        public static int yellowLightCountdown;
        public static int yellowLightCountdowntText;
        public static int yellowLightTime1;
        /// <summary>
        /// yellowLightTime*2 默认为5s*2=10
        /// </summary>
        public static int secondLightRuntime2;
        /// <summary>
        /// 绿灯计时时间，默认为15s
        /// </summary>
        public static int greenLightCountdown;
        public static int greenLightCountdownText;
        public static int greenLightTime1;
        /// <summary>
        /// greenLightTime*2 默认为15s*2=30
        /// </summary>
        public static int thirdLightRuntime2;


        public static int ygLTime;
        public static int grLTime;
        public static int ryLTime;
        /// <summary>
        /// flicker ['flɪkɚ] 闪烁，闪光 倒计时文本闪烁倒计时，每0.5s一次
        /// </summary>
        public static int lightFlicker;
        #endregion

        /// <summary>
        /// 设置mainTimer.Tick所用的数据（mainCycleTimer_Tick里使用的方法）
        /// </summary>
        public static void mainTimerInitializeParams( )
        {
            trafficLightsTime = firstLightRuntime + secondLightRuntime + secondLightRuntime;

            //作为“常量使用”
            redLightTime1 = firstLightRuntime * 2;
            yellowLightTime1 = secondLightRuntime * 2;
            greenLightTime1 = secondLightRuntime * 2;
            //返回倒计时数字(文本)
            firstLightRuntime2 = firstLightRuntime * 2;
            secondLightRuntime2 = secondLightRuntime * 2;
            thirdLightRuntime2 = secondLightRuntime * 2;
            trafficLightsTime2 = (firstLightRuntime + secondLightRuntime + secondLightRuntime) * 2;

            //强制转换至某一颜色的灯后，会用到的变量
            s_tLightRuntime = (secondLightRuntime + thirdLightRuntime) * 2;
            t_fLightRuntime = (thirdLightRuntime + firstLightRuntime) * 2;
            f_sLightRuntime = (firstLightRuntime + secondLightRuntime) * 2;
         }


        /// <summary>
        /// 计时器数据显示
        /// 并处理TextBlock中字体颜色（Rgb），交通灯颜色（Rgb）
        /// </summary>
        /// <returns>return倒计时时间（单位为秒）</returns>
        public static void unitTimerInitialzeParams(int selectLightIndex)
        {
            //string countdown;//倒计时剩余秒数，即当前颜色的灯的颜色
            //// 待补充注释
            //switch (selectLightIndex)
            //{
            //    case 0://红 黄 绿  强制红，默认也为红 ygLTime grLTime ryLtime trafficLightsTime2
            //        #region 红30s 黄5s 绿15s 交通灯的颜色变化顺序默认为：红 黄 绿

            //        if (trafficLightsTime2 > ygLTime)//红灯30s，后5s时红黄闪烁（默认值为30s）
            //        {//红
            //            countdown = RedLightUp();
            //        }
            //        else if (trafficLightsTime2 <= ygLTime && trafficLightsTime2 > greenLightTime1)//黄灯5s，后5s时黄绿闪烁（默认值为5s）
            //        {//黄
            //            countdown = YellowLightUp();
            //        }
            //        else if (trafficLightsTime2 <= greenLightTime1 && trafficLightsTime2 >= 0)//绿灯15s，后5s时绿红闪烁（默认值为15s）
            //        {//绿
            //            countdown = GreenLightUp();
            //        }
            //        else
            //        {
            //            return "BUG";
            //        }
            //        #endregion
            //        //TimeAutoDecrement();
            //        return countdown;
            //    case 1://黄 绿 红 强制黄灯开始
            //        #region 黄5s 绿15s 红30s 交通灯的颜色变化顺序默认为：红 黄 绿
            //        if (trafficLightsTime2 > grLTime)//红灯30s，后5s时红黄闪烁
            //        {//黄
            //            countdown = YellowLightUp();
            //        }
            //        else if (trafficLightsTime2 <= grLTime && trafficLightsTime2 > redLightTime1)
            //        {//绿
            //            countdown = GreenLightUp();
            //        }
            //        else if (trafficLightsTime2 <= redLightTime1 && trafficLightsTime2 > 0)
            //        {//红
            //            countdown = RedLightUp();
            //        }
            //        else
            //        {
            //            return "BUG";
            //        }
            //        #endregion
            //        //TimeAutoDecrement();
            //        return countdown;
            //    case 2://绿 红 黄 强制绿灯开始
            //        #region 绿15s 红30s 黄5s 交通灯的颜色变化顺序默认为：红 黄 绿
            //        if (trafficLightsTime2 > ryLTime)//红灯30s，后5s时红黄闪烁
            //        {//绿
            //            countdown = GreenLightUp();
            //        }
            //        else if (trafficLightsTime2 <= ryLTime && trafficLightsTime2 > yellowLightTime1)
            //        {//红
            //            countdown = RedLightUp();
            //        }
            //        else if (trafficLightsTime2 <= yellowLightTime1 && trafficLightsTime2 > 0)
            //        {//黄
            //            countdown = YellowLightUp();
            //        }
            //        else
            //        {
            //            return "BUG";
            //        }
            //        #endregion
            //        //TimeAutoDecrement();
            //        return countdown;
            //    default://默认
            //        #region 红30s 黄5s 绿15s 交通灯的颜色变化顺序默认为：红 黄 绿

            //        if (trafficLightsTime2 > ygLTime)//红灯30s，后5s时红黄闪烁（默认值为30s）
            //        {
            //            countdown = RedLightUp();
            //        }
            //        else if (trafficLightsTime2 <= ygLTime && trafficLightsTime2 > greenLightTime1)//黄灯5s，后5s时黄绿闪烁（默认值为5s）
            //        {
            //            countdown = YellowLightUp();
            //        }
            //        else if (trafficLightsTime2 <= greenLightTime1 && trafficLightsTime2 > 0)//绿灯15s，后5s时绿红闪烁（默认值为15s）
            //        {
            //            countdown = GreenLightUp();
            //        }
            //        else
            //        {
            //            return "BUG";
            //        }
            //        #endregion
            //        //TimeAutoDecrement();
            //return countdown;
            //}
        }
        /// <summary>
        /// 为unitTimer.Tick事件提供所用的函数
        /// 用此函数前，需用到：mainTimerInitializeParams();AllLightsUpColor(selectedIndex);初始化其所用变量
        /// </summary>
        /// <param name="selectedIndex"></param>
        private static void LightsUp(int selectedIndex)
        {
            //使用此函数时，需要提前把将用到的参数准备好，
            //包括：firstLightUpColor、secondLightUpColor、thirdLightUpColor
            //灯的颜色、字体颜色（包括后5s动画）、倒计时、为下次循环准备的数据
            if (trafficLightsTime2 > s_tLightRuntime )
            {
                FirstLightUpColor(selectedIndex);//灯的颜色
                TextFontColor(firstLightRuntime2, firstLightUpColor, secondLightUpColor);//字体颜色（包括后5s的动画）
                countdownToUI = (firstLightRuntime2+1)/2;//倒计时
                trafficLightsTime2--;//自减，为进下次循环处理数据
                firstLightRuntime2--;
            }
            else if ((trafficLightsTime2 <= s_tLightRuntime ) && (trafficLightsTime2 > thirdLightRuntime2 ))
            {//灯的颜色、字体颜色（包括后5s动画）、倒计时、为下次循环准备的数据
                SecondLightUpColor(selectedIndex);
                TextFontColor(firstLightRuntime2, secondLightUpColor, thirdLightUpColor);
                countdownToUI = (secondLightRuntime2 + 1) / 2;
                trafficLightsTime2--;
                secondLightRuntime2--;
            }
            else if (trafficLightsTime2 <= thirdLightRuntime2)
            {
                ThirdLightUpColor(selectedIndex);
                TextFontColor(thirdLightRuntime2, thirdLightUpColor, firstLightUpColor);
                countdownToUI = (thirdLightRuntime2 + 1) / 2;
                trafficLightsTime2--;
                thirdLightRuntime2--;
            }
        }
        private static string PerLightTimeRunTime(int selectedIndex, int allLightTimeSum, int twoLightTimeSum, int oneLightTime)
        {
            string countdownTime = "";
            //if (trafficLightsTime2 > twoLightTimeSum)//红灯30s，后5s时红黄闪烁（默认值为30s）
            //{
            //    countdownTime = RedLightUp();
            //}
            //else if (trafficLightsTime2 <= twoLightTimeSum && trafficLightsTime2 > oneLightTime)//黄灯5s，后5s时黄绿闪烁（默认值为5s）
            //{
            //    countdownTime = YellowLightUp();
            //}
            //else if (trafficLightsTime2 <= oneLightTime && trafficLightsTime2 > 0)//绿灯15s，后5s时绿红闪烁（默认值为15s）
            //{
            //    countdownTime = GreenLightUp();
            //}
            //else
            //{
            //    return "BUG";
            //}
            return countdownTime;
        }
        private static void LightsOrder(int selectedIndex)
        {
            //switch (selectedIndex)
            //{
            //    case 0:
            //        firstLightUpCountdown = RedLightUp(0,firstLightRuntime2);
            //        secondLightUpCountdown = YellowLightUp();
            //        thirdLightUpCountdown = GreenLightUp();
            //        break;
            //    case 1:
            //        firstLightUpCountdown = YellowLightUp();
            //        secondLightUpCountdown = GreenLightUp();
            //        thirdLightUpCountdown = RedLightUp(1,thirdLightRuntime2);
            //        break;
            //    case 2:
            //        firstLightUpCountdown = GreenLightUp();
            //        secondLightUpCountdown = RedLightUp(2,secondLightRuntime2);
            //        thirdLightUpCountdown = YellowLightUp();
            //        break;
            //    default:
            //        break;

            //}
        }

        /// <summary>
        /// 用于倒计数文本的自减
        /// </summary>
        private static void TimeAutoDecrement(int selectedIndex)
        {
            trafficLightsTime2--;
            if (trafficLightsTime2 > s_tLightRuntime * 2)
            {
                firstLightRuntime2--;
            }
            else if ((trafficLightsTime2 <= s_tLightRuntime * 2) && (trafficLightsTime2 > thirdLightRuntime * 2))
            {
                secondLightRuntime2--;
            }
            else if (trafficLightsTime2 <= thirdLightRuntime * 2)
            {
                thirdLightRuntime2--;
            }
        }

        /// <summary>
        /// ①灯序：红灰灰；②红灯倒计时；③倒数5s开始闪烁
        /// 例：当redLightTime=60,59,58,57……4,3,2,1时，return的值分别为：30,30,29,29……2,2,1,1
        /// </summary>
        /// <returns>交通灯倒计时时间显示</returns>
        public static string TextFontColor(int selectedIndex,int tempTime)
        {//每种颜色的灯 倒计时的文本
            Color originalColor;
            Color theOtherColor;
            switch (selectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
            #region 计时器文本数字倒计时，函数返回值（return） 数字10可以用static变量代替：值=倒计时*2
            if (tempTime > 10)
            {
                textFontColor = Red;
            }
            else
            {
                //iCount2==50时，即进入红灯的最后5s，字体颜色开始变化，奇数秒时为红，偶数时为黄
                if (tempTime % 2 == 0)
                {
                    textFontColor = Yellow;
                }
                else
                {
                    textFontColor = Red;
                }
            }
            //前提：所在方法每0.5运行一次，1s钟输出两次
            //例：当redLightTime=60,59,58,57……4,3,2,1时，return的值分别为：30,30,29,29……2,2,1,1
            return ((tempTime + 1) / 2).ToString("000");
            #endregion
        }
        /// <summary>
        /// ①灯序：灰黄灰；②黄灯倒计时；③倒数5s开始闪烁
        /// </summary>
        /// <returns>交通灯倒计时时间显示</returns>
        public static string YellowLightUp()
        {
            YellowLightUpColor();

            if (secondLightRuntime2 > 10)
            {
                textFontColor = Yellow;
            }
            else
            {
                #region 黄灯时5s文本开始黄绿变换

                if (secondLightRuntime2 % 2 == 0)
                {
                    textFontColor = Green;
                }
                else
                {
                    textFontColor = Yellow;
                }

                #endregion
            }

            return ((secondLightRuntime2 + 1) / 2).ToString("000");
        }

        /// <summary>
        /// ①灯序：灰灰绿；②绿灯倒计时；③倒数5s开始闪烁
        /// </summary>
        /// <returns>交通灯倒计时时间显示</returns>
        public static string GreenLightUp()
        {
            GreenLightUpColor();

            if (thirdLightRuntime2 > 10)
            {
                textFontColor = Green;
            }
            else
            {
                //iCount2==50时，即进入红灯的最后5s，字体颜色开始变化，奇数秒时为红，偶数时为黄
                if (thirdLightRuntime2 % 2 == 0)
                {
                    textFontColor = Red;
                }
                else
                {
                    textFontColor = Green;
                }
            }
            return ((thirdLightRuntime2 + 1) / 2).ToString("000");
        }



        public static void LightUpColor(string lightColor)
        {
            switch (lightColor)
            {
                case "红":
                    RedLightUpColor();
                    break;
                case "黄":
                    YellowLightUpColor();
                    break;
                case "绿":
                    GreenLightUpColor();
                    break;
                default:
                    AllLightUpColor();
                    break;
            }
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
            rLColor = Red;
            yLColor = Gray;
            gLColor = Gray;
        }
        private static void AllLightUpColor()
        {
            rLColor = Red;
            yLColor = Yellow;
            gLColor = Green;
        }



        private static string LightsUp0(int selectdeIndex, int firstLightRuntime, int secondLightRuntime, int thirdLightRuntime)
        {
            string countdownTime = "";

            s_tLightRuntime = secondLightRuntime + thirdLightRuntime;
            t_fLightRuntime = thirdLightRuntime + firstLightRuntime;
            f_sLightRuntime = firstLightRuntime + secondLightRuntime;

            if (trafficLightsTime2 > s_tLightRuntime * 2)
            {
                countdownTime = FirstLightUp(selectdeIndex);
            }
            else if ((trafficLightsTime2 <= s_tLightRuntime * 2) && (trafficLightsTime2 > thirdLightRuntime * 2))
            {
                countdownTime = SecondLightUp(selectdeIndex);
            }
            else if (trafficLightsTime2 <= thirdLightRuntime * 2)
            {
                countdownTime = ThirdLightUp(selectdeIndex);
            }
            return countdownTime;
        }
        public static void mainTimerInitializeParams0(int rTime, int yTime, int gTime)
        {
            #region 定义每个周期内红绿灯分别运行的时间
            trafficLightsTime = rTime + yTime + gTime;

            //作为“常量使用”
            redLightTime1 = rTime * 2;
            yellowLightTime1 = yTime * 2;
            greenLightTime1 = gTime * 2;

            firstLightRuntime2 = rTime * 2;
            secondLightRuntime2 = yTime * 2;
            thirdLightRuntime2 = gTime * 2;
            trafficLightsTime2 = (rTime + yTime + gTime) * 2;

            //强制转换至某一颜色的灯后，会用到的变量
            ygLTime = (yTime + gTime) * 2;
            grLTime = (gTime + rTime) * 2;
            ryLTime = (rTime + yTime) * 2;
            #endregion
        }
        private static string FirstLightUp(int selectedIndex)
        {
            string countdownTime = "";
            switch (selectedIndex)
            {
                case 0:
                    countdownTime = RedLightUp();
                    break;
                case 1:
                    countdownTime = YellowLightUp();
                    break;
                case 2:
                    countdownTime = GreenLightUp();
                    break;
                default:
                    break;
            }
            return countdownTime;
        }
        private static string SecondLightUp(int selectedIndex)
        {
            string countdownTime = "";
            switch (selectedIndex)
            {
                case 0:
                    countdownTime = YellowLightUp();
                    break;
                case 1:
                    countdownTime = GreenLightUp();
                    break;
                case 2:
                    countdownTime = RedLightUp();
                    break;
                default:
                    break;
            }
            return countdownTime;
        }
        private static string ThirdLightUp(int selectedIndex)
        {
            string countdownTime = "";
            switch (selectedIndex)
            {
                case 0:
                    countdownTime = GreenLightUp();
                    break;
                case 1:
                    countdownTime = RedLightUp();
                    break;
                case 2:
                    countdownTime = YellowLightUp();
                    break;
                default:
                    break;
            }
            return countdownTime;
        }
    }
}
