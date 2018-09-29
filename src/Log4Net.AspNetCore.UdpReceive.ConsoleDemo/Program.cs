using Log4Net.AspNetCore.UdpReceive.Core.Log;
using Log4Net.AspNetCore.UdpReceive.Core.Receive;
using Log4Net.AspNetCore.UdpReceive.Core.Utils;
using System;

namespace Log4Net.AspNetCore.UdpReceive.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigHelper.Init();

            UdpReceiver udp = new UdpReceiver();
            udp.Initialize();
            ILogMessageNotifiable logNotifiable = new SQLNotifiable();
            udp.Attach(logNotifiable);

            udp.Start();

            Console.WriteLine("输入‘E’/‘e’退出：");
            while (!Console.ReadLine().Equals("e", StringComparison.CurrentCultureIgnoreCase))
            {
            }

            try
            {
                udp.Terminate();
            }
            catch (Exception ex)
            {
            }

        }
    }
}
