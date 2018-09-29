using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Log4Net.AspNetCore.UdpSend.ConsoleDemo
{
    public class TestFlashLogger
    {
        public static void Test()
        {
            //FlashLogger.Instance().Register();
            //FlashLogger.Info("Info");
            var sss = Console.ReadLine();
            var count = Convert.ToInt32(sss);
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            TestLogHelper(count);
            sw1.Stop();
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            TestFlash(count);
            sw2.Stop();
            Console.WriteLine(sw1.ElapsedTicks);
            Console.WriteLine(sw2.ElapsedTicks);

        }

        private static void TestLogHelper(int count)
        {
            for (int i = 0; i < count; i++)
            {
                LogHelper.Write("Info", LogLevel.Info);
            }
        }

        private static void TestFlash(int count)
        {
            for (int i = 0; i < count; i++)
            {
                FlashLogger.Info("Info");
            }
        }

    }
}
