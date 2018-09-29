using System;

namespace Log4Net.AspNetCore.UdpSend.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            LogHelper.Write("this is test", LogLevel.Info, new Exception("test"));
            LogHelper.Write("我的调试信息", LogLevel.Debug);
            LogHelper.Write("我的错误信息", LogLevel.Error);
            LogHelper.Write("我的错误信息", LogLevel.Warn);

            try
            {
                //TestFlashLogger.Test();
                Convert.ToDateTime("2323232");
            }
            catch (Exception e)
            {
                LogHelper.Write("重大错误", LogLevel.Fatal, e);
            }

            Console.WriteLine("Log4Net Test Over.Please enter any word to End....");
            Console.Read();
        }

    }
}
