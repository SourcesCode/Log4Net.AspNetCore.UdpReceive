using Log4Net.AspNetCore.UdpReceive.Core.Log;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Log4Net.AspNetCore.UdpReceive.Core.LayoutParser
{
    public class PatternLayoutParser
    {
        public static LogMessage ParsePatternLayout(string message)
        {
            var logMessage = new LogMessage();
            var msgArr = message.Split(new string[] { "||" }, StringSplitOptions.None);

            logMessage.Date = GetDateTime(msgArr[0]);
            logMessage.ThreadName = msgArr[1];
            logMessage.Level = msgArr[2];
            logMessage.LoggerName = msgArr[3];
            logMessage.NDC = msgArr[4];
            logMessage.Message = msgArr[5];
            logMessage.ExceptionString = msgArr[6];
            logMessage.RunMillisecond = int.Parse(msgArr[7]);
            logMessage.AppDomain = msgArr[8];
            return logMessage;

        }

        /// <summary>
        /// 带毫秒的字符转换成时间（DateTime）格式
        /// 可处理格式：2014-10-10 10:10:10,666 或 2014-10-10 10:10:10 666 或 2014/10/10 10:10:10,666
        /// </summary>
        static DateTime GetDateTime(string dateTime)
        {
            string[] strArr = dateTime.Split(new char[] { '-', '/', ' ', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

            DateTime dt = new DateTime(
                int.Parse(strArr[0]),
                int.Parse(strArr[1]),
                int.Parse(strArr[2]),
                int.Parse(strArr[3]),
                int.Parse(strArr[4]),
                int.Parse(strArr[5]),
                int.Parse(strArr[6]));
            return dt;
        }

    }
}
