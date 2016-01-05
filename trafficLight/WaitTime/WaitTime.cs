using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;

namespace trafficLight
{

    /// <summary>
    /// 红绿灯的等待时间：红灯30s，黄灯5s，绿灯15s
    /// </summary>
    class WaitTime
    {
        /// <summary>
        /// 50s为红绿灯的一次循环
        /// </summary>
        public static byte iCount;
        public static byte iCount2;
        /// <summary>
        /// 30s为红灯的运行时间
        /// </summary>
        public static byte redCount;
        //public static int yellowCount;
        /// <summary>
        /// 15s为绿灯的运行时间
        /// </summary>
        public static int greenCount;

        //TextBlock中字体颜色的组成FromGrb(r,g,b)
        public static byte textFontColorR;
        public static byte textFontColorG;
        public static byte textFontColorB;

        //红灯的颜色变化
        public static byte redLightColorR;
        public static byte redLightColorG;
        public static byte redLightColorB;

        //黄灯的颜色变化
        public static byte yellowLightColorR;
        public static byte yellowLightColorG;
        public static byte yellowLightColorB;

        //绿灯的颜色变化
        public static byte greenLightColorR;
        public static byte greenLightColorG;
        public static byte greenLightColorB;

        /// <summary>
        /// 初始化 计时器num
        /// </summary>
        public static void SetNumForCount()
        {
            iCount = 50;
            iCount2 = 100;
            redCount = 30;
            //yellowCount = 5;
            greenCount = 15;
            textFontColorR = 86;
            textFontColorG = 86;
            textFontColorB = 86;

            redLightColorR = 86;
            redLightColorG = 86;
            redLightColorB = 86;

            yellowLightColorR = 86;
            yellowLightColorG = 86;
            yellowLightColorB = 86;

            greenLightColorR = 86;
            greenLightColorG = 86;
            greenLightColorB = 86;
        }

        /// <summary>
        /// 计时器数据显示
        /// 并处理TextBlock中字体颜色（Rgb），交通灯颜色（Rgb）
        /// </summary>
        /// <returns>计时器数字，红30，黄000，绿15</returns>
        public static string InitialParam()
        {            
            #region 增加使能参数
            if (iCount > 20 && iCount2 > 40)//红灯30s，后5s时红黄闪烁
            {
                #region 红灯30s
                #region 计时器字体颜色 iCount>40&&iCount2<=50时，即进入红灯的最后5s，字体颜色开始红黄变换
                if (iCount2 > 50)
                {
                    textFontColorR = 255;
                    textFontColorG = 0;
                    textFontColorB = 0;
                }
                else
                {
                    //iCount2==50时，即进入红灯的最后5s，字体颜色开始变化，奇数秒时为红，偶数时为黄
                    if (iCount2 % 2 == 0)
                    {
                        textFontColorR = 255;
                        textFontColorG = 255;
                        textFontColorB = 0;
                    }
                    else
                    {
                        textFontColorR = 255;
                        textFontColorG = 0;
                        textFontColorB = 0;
                    }
                }
                #endregion

                redLightColorR = 255;
                redLightColorG = 0;
                redLightColorB = 0;

                yellowLightColorR = 86;
                yellowLightColorG = 86;
                yellowLightColorB = 86;

                greenLightColorR = 86;
                greenLightColorG = 86;
                greenLightColorB = 86;

                if (iCount2-- % 2 == 1)//无论是否执行if的语句块，iCount2--都会执行
                {
                    iCount--;
                    return redCount--.ToString("000");
                }
                else
                {
                    return redCount.ToString("000");
                }
                #endregion
            }
            else if (iCount <= 20 && iCount > 15 && iCount2 <= 40 && iCount2 > 30)
            {
                #region 黄灯5s
                #region 计时器字体颜色 准备进入绿灯，黄绿闪烁

                if (iCount2 % 2 == 0)
                {
                    textFontColorR = 255;
                    textFontColorG = 255;
                    textFontColorB = 0;
                }
                else
                {
                    textFontColorR = 0;
                    textFontColorG = 255;
                    textFontColorB = 0;
                }

                #endregion

                redLightColorR = 86;
                redLightColorG = 86;
                redLightColorB = 86;

                yellowLightColorR = 255;
                yellowLightColorG = 255;
                yellowLightColorB = 0;

                greenLightColorR = 86;
                greenLightColorG = 86;
                greenLightColorB = 86;

                if (iCount2-- % 2 == 1)//无论是否执行if的语句块，iCount2--都会执行
                {
                    iCount--;
                }
                return "000";
                #endregion
            }
            else if (iCount <= 15 && iCount >= 1 && iCount2 <= 30 && iCount2 >= 1)
            {
                #region 绿灯20s
                #region iCount2<=10时，即进入绿灯的最后5s，字体颜色开始红绿变换
                if (iCount2 > 10)
                {
                    textFontColorR = 0;
                    textFontColorG = 255;
                    textFontColorB = 0;
                }
                else
                {
                    //iCount2==50时，即进入红灯的最后5s，字体颜色开始变化，奇数秒时为红，偶数时为黄
                    if (iCount2 % 2 == 0)
                    {
                        textFontColorR = 0;
                        textFontColorG = 255;
                        textFontColorB = 0;
                    }
                    else
                    {
                        textFontColorR = 255;
                        textFontColorG = 0;
                        textFontColorB = 0;
                    }
                }
                #endregion

                redLightColorR = 86;
                redLightColorG = 86;
                redLightColorB = 86;

                yellowLightColorR = 86;
                yellowLightColorG = 86;
                yellowLightColorB = 86;

                greenLightColorR = 0;
                greenLightColorG = 255;
                greenLightColorB = 0;

                if (iCount2-- % 2 == 0)//无论是否执行if的语句块，iCount2--都会执行
                {
                    iCount--;
                    return greenCount--.ToString("000");
                }
                else
                {
                    return greenCount.ToString("000");
                }
                #endregion
            }
            else
            {
                return "BUG";
            }
            #endregion
        }
    }
}
