using Log4Net.AspNetCore.UdpReceive.Core.LayoutParser;
using Log4Net.AspNetCore.UdpReceive.Core.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Net.AspNetCore.UdpReceive.Core.Utils
{
    public class ParserHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="logLayoutType"></param>
        /// <returns></returns>
        public static LogMessage ParseLayout(byte[] buffer, LogLayoutTypeEnum logLayoutType)
        {
            LogMessage logMsg = null;
            string message = Encoding.UTF8.GetString(buffer);
            switch (logLayoutType)
            {
                case LogLayoutTypeEnum.PatternLayout:
                    //通过|分隔来获取数据
                    logMsg = PatternLayoutParser.ParsePatternLayout(message);
                    break;
                case LogLayoutTypeEnum.XmlLayoutSchemaLog4j:
                    //通过xml来获取数据
                    logMsg = XmlLayoutSchemaLog4jParser.ParseLog4JXmlLogEvent(message, "UdpLogger");
                    break;
            }
            return logMsg;
        }


    }
}
