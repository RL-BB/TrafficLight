using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;

namespace trafficLight
{

    /// <summary>
    /// 红绿灯的等待时间：红灯30s，黄灯5s，绿灯15s
    /// </summary>
    class WaitTime
    {
        //四种颜色分别为：红、黄、绿、灰
        public static Color Red = Color.FromRgb(255, 0, 0);
        public static Color Yellow = Color.FromRgb(255, 255, 0);
        public static Color Green = Color.FromRgb(0, 255, 0);
        public static Color Gray = Color.FromRgb(86, 86, 86);

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

        #region 红绿灯周期时间        
        /// <summary>
        /// 交通灯运行周期时间：rTime+yTime+gTime
        /// </summary>
        public static int trafficLightsTime;
        /// <summary>
        /// trafficLightsTime*2 默认为（30+5+15）*2==100
        /// </summary>
        public static int trafficLightsTime2;//后面的数字2意味着是trafficLightsTime的2倍。"2"和0.5s的计时器有关
        /// <summary>
        /// 红灯计时时间，默认为30s
        /// </summary>
        public static int redLightTime;
        public static int redLightTime1;
        /// <summary>
        /// redLightTime*2 默认为30s*2=60
        /// </summary>
        public static int redLightTime2;
        /// <summary>
        /// 黄灯计时时间，默认为5s
        /// </summary>
        public static int yellowLightTime;
        public static int yellowLightTime1;
        /// <summary>
        /// yellowLightTime*2 默认为5s*2=10
        /// </summary>
        public static int yellowLightTime2;
        /// <summary>
        /// 绿灯计时时间，默认为15s
        /// </summary>
        public static int greenLightTime;
        public static int greenLightTime1;
        /// <summary>
        /// greenLightTime*2 默认为15s*2=30
        /// </summary>
        public static int greenLightTime2;


        public static int ygLTime;
        public static int grLTime;
        public static int ryLTime;
        /// <summary>
        /// flicker ['flɪkɚ] 闪烁，闪光 倒计时文本闪烁倒计时，每0.5s一次
        /// </summary>
        public static int lightFlicker;
        #endregion

        /// <summary>
        /// 初始化 计时器num
        /// </summary>
        public static void SetNumForCycleCount(int rTime, int yTime, int gTime)
        {
            #region 定义每个周期内红绿灯分别运行的时间
            redLightTime = rTime;
            yellowLightTime = yTime;
            greenLightTime = gTime;
            trafficLightsTime = rTime + yTime + gTime;

            //作为“常量使用”
            redLightTime1 = rTime * 2;
            yellowLightTime1 = yTime * 2;
            greenLightTime1 = gTime * 2;

            redLightTime2 = rTime * 2;
            yellowLightTime2 = yTime * 2;
            greenLightTime2 = gTime * 2;
            trafficLightsTime2 = (rTime + yTime + gTime) * 2;

            //强制转换至某一颜色的灯后，会用到的变量
            ygLTime = (yTime + gTime) * 2;
            grLTime = (gTime + rTime) * 2;
            ryLTime = (rTime + yTime) * 2;

            //字体颜色初始化，红绿灯的颜色初始化
            textFontColor = Red;
            //形参加入 selectIndex 可使选择的
            rLColor = Red;
            yLColor = Gray;
            gLColor = Gray;


            #endregion
        }

        /// <summary>
        /// 每个0.5s的循环要处理的数据
        /// </summary>
        public static void SetNumForHalfSecond(int trafficLightColor)
        {
            switch (trafficLightColor)
            {
                case 0://强制红
                    trafficLightsTime2--;
                    if (redLightTime2 > 0)
                    {//红
                        redLightTime2--;//最终为0
                    }
                    if ((redLightTime2 == 0) && (yellowLightTime2 > 0))
                    {//黄
                        yellowLightTime2--;//最终为0
                    }
                    if ((redLightTime2 == 0) && (yellowLightTime2 == 0) && (greenLightTime2 > 0))
                    {//绿
                        greenLightTime2--;//最终为0
                    }

                    break;
                case 1://强制黄

                    trafficLightsTime2--;
                    yellowLightTime2--;
                    //}
                    break;
                case 2://强制绿
                    trafficLightsTime2--;
                    greenLightTime2--;
                    //}
                    break;
                default://默认为红灯
                    trafficLightsTime2--;
                    redLightTime2--;
                    break;
            }
        }

        /// <summary>
        /// 计时器数据显示
        /// 并处理TextBlock中字体颜色（Rgb），交通灯颜色（Rgb）
        /// </summary>
        /// <returns>计时器数字，红30，黄000，绿15</returns>
        public static string InitialParam(int selectLightIndex)
        {
            string contentCount;
            // 待补充注释
            switch (selectLightIndex)
            {
                case 0://红 黄 绿  强制红，默认也为红 ygLTime grLTime ryLtime trafficLightsTime2
                    #region 红30s 黄5s 绿15s 交通灯的颜色变化顺序默认为：红 黄 绿

                    if (trafficLightsTime2 > ygLTime)//红灯30s，后5s时红黄闪烁（默认值为30s）
                    {//红
                        contentCount = RedLightTimer();
                        //SetNumForHalfSecond(0);
                        trafficLightsTime2--;
                        redLightTime2--;
                    }
                    else if (trafficLightsTime2 <= ygLTime && trafficLightsTime2 > greenLightTime1)//黄灯5s，后5s时黄绿闪烁（默认值为5s）
                    {//黄
                        contentCount = YellowLightTimer();
                        //SetNumForHalfSecond(0);
                        trafficLightsTime2--;
                        yellowLightTime2--;
                    }
                    else if (trafficLightsTime2 <= greenLightTime1 && trafficLightsTime2 >= 0)//绿灯15s，后5s时绿红闪烁（默认值为15s）
                    {//绿
                        contentCount = GreenLightTimer();
                        //SetNumForHalfSecond(0);
                        trafficLightsTime2--;
                        greenLightTime2--;
                    }
                    else
                    {
                        return "BUG";
                    }

                    #endregion
                    //下面一行的代码不会被执行到，但不可删除，否则函数报错，下面的三个return同此；
                    return contentCount;
                case 1://黄 绿 红 强制黄灯开始
                    #region 黄5s 绿15s 红30s 交通灯的颜色变化顺序默认为：红 黄 绿
                    if (trafficLightsTime2 > grLTime)//红灯30s，后5s时红黄闪烁
                    {//黄
                        contentCount = YellowLightTimer();
                        //SetNumForHalfSecond(1);
                        trafficLightsTime2--;
                        yellowLightTime2--;
                    }
                    else if (trafficLightsTime2 <= grLTime && trafficLightsTime2 > redLightTime1)
                    {//绿
                        contentCount = GreenLightTimer();
                        //SetNumForHalfSecond(1);
                        trafficLightsTime2--;
                        greenLightTime2--;
                    }
                    else if (trafficLightsTime2 <= redLightTime1 && trafficLightsTime2 > 0)
                    {//红
                        contentCount = RedLightTimer();
                        //SetNumForHalfSecond(1);
                        trafficLightsTime2--;
                        redLightTime2--;
                    }
                    else
                    {
                        return "BUG";
                    }
                    #endregion
                    return contentCount;
                case 2://绿 红 黄 强制绿灯开始
                    #region 绿15s 红30s 黄5s 交通灯的颜色变化顺序默认为：红 黄 绿
                    if (trafficLightsTime2 > ryLTime)//红灯30s，后5s时红黄闪烁
                    {//绿
                        contentCount = GreenLightTimer();
                        //SetNumForHalfSecond(2);
                        trafficLightsTime2--;
                        greenLightTime2--;
                    }
                    else if (trafficLightsTime2 <= ryLTime && trafficLightsTime2 > yellowLightTime1)
                    {//红
                        contentCount = RedLightTimer();
                        //SetNumForHalfSecond(2);
                        trafficLightsTime2--;
                        redLightTime2--;
                    }
                    else if (trafficLightsTime2 <= yellowLightTime1 && trafficLightsTime2 > 0)
                    {//黄
                        contentCount = YellowLightTimer();
                        //SetNumForHalfSecond(2);
                        trafficLightsTime2--;
                        yellowLightTime2--;
                    }
                    else
                    {
                        return "BUG";
                    }
                    #endregion
                    return contentCount;
                default://默认
                    #region 红30s 黄5s 绿15s 交通灯的颜色变化顺序默认为：红 黄 绿

                    if (trafficLightsTime2 > ygLTime)//红灯30s，后5s时红黄闪烁（默认值为30s）
                    {
                        contentCount = RedLightTimer();
                        //SetNumForHalfSecond(0);
                        trafficLightsTime2--;
                        redLightTime2--;
                    }
                    else if (trafficLightsTime2 <= ygLTime && trafficLightsTime2 > greenLightTime1)//黄灯5s，后5s时黄绿闪烁（默认值为5s）
                    {
                        contentCount = YellowLightTimer();
                        //SetNumForHalfSecond(0);
                        trafficLightsTime2--;
                        yellowLightTime2--;
                    }
                    else if (trafficLightsTime2 <= greenLightTime1 && trafficLightsTime2 > 0)//绿灯15s，后5s时绿红闪烁（默认值为15s）
                    {
                        contentCount = GreenLightTimer();
                        //SetNumForHalfSecond(0);
                        trafficLightsTime2--;
                        greenLightTime2--;
                    }
                    else
                    {
                        return "BUG";
                    }

                    #endregion
                    return contentCount;
            }

        }

        /// <summary>
        /// ①灯序：红灰灰；②红灯倒计时；③倒数5s开始闪烁
        /// </summary>
        /// <returns>初始化参数，返回TextBlock中的倒计时数字文本</returns>
        public static string RedLightTimer()
        {
            //前提：每秒运行两次,设计初衷是为了在0.5s的计时器Tick事件中使用
            #region 灯的颜色顺序： 红灰灰
            rLColor = Red;
            yLColor = Gray;
            gLColor = Gray;
            #endregion

            #region 计时器文本数字倒计时，函数返回值（return） 数字10可以用变量代替：值=倒计时*2
            if (redLightTime2 > 10)
            {
                textFontColor = Red;
            }
            else
            {
                //iCount2==50时，即进入红灯的最后5s，字体颜色开始变化，奇数秒时为红，偶数时为黄
                if (redLightTime2 % 2 == 0)
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
            return ((redLightTime2 + 1) / 2).ToString("000");
            #endregion
        }

        /// <summary>
        /// ①灯序：灰黄灰；②黄灯倒计时；③倒数5s开始闪烁
        /// </summary>
        /// <returns>倒计时框的时间显示</returns>
        public static string YellowLightTimer()
        {
            #region 灯的颜色顺序：灰黄灰
            rLColor = Gray;
            yLColor = Yellow;
            gLColor = Gray;
            #endregion

            #region 黄灯时文本颜色  最后5s，字体颜色开始红黄变换
            if (yellowLightTime2 > 10)
            {
                textFontColor = Yellow;
            }
            else
            {
                #region 黄灯时5s文本开始黄绿变换

                if (yellowLightTime2 % 2 == 0)
                {
                    textFontColor = Green;
                }
                else
                {
                    textFontColor = Yellow;
                }

                #endregion
            }

            #endregion
            return ((yellowLightTime2 + 1) / 2).ToString("000");
        }

        /// <summary>
        /// ①灯序：灰灰绿；②绿灯倒计时；③倒数5s开始闪烁
        /// </summary>
        /// <returns>倒计时框的时间显示</returns>
        public static string GreenLightTimer()
        {
            #region  灯的颜色顺序：灰灰绿
            rLColor = Gray;
            yLColor = Gray;
            gLColor = Green;

            #endregion 

            #region greenLightTime2<=10时，即进入绿灯的最后5s，字体颜色开始红绿变换
            if (greenLightTime2 > 10)
            {
                textFontColor = Green;
            }
            else
            {
                //iCount2==50时，即进入红灯的最后5s，字体颜色开始变化，奇数秒时为红，偶数时为黄
                if (greenLightTime2 % 2 == 0)
                {
                    textFontColor = Red;
                }
                else
                {
                    textFontColor = Green;
                }
            }
            return ((greenLightTime2 + 1) / 2).ToString("000");
            #endregion

        }


    }
}
