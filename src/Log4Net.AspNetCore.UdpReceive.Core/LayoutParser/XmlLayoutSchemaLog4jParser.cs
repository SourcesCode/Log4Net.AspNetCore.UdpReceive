using Log4Net.AspNetCore.UdpReceive.Core.Log;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Log4Net.AspNetCore.UdpReceive.Core.LayoutParser
{
    public static class XmlLayoutSchemaLog4jParser
    {
        static readonly DateTime s1970 = new DateTime(1970, 1, 1);

        /// <summary>
        /// We can share settings to improve performance
        /// </summary>
        static readonly XmlReaderSettings XmlSettings = CreateSettings();

        static XmlReaderSettings CreateSettings()
        {
            return new XmlReaderSettings { CloseInput = false, ValidationType = ValidationType.None };
        }

        /// <summary>
        /// We can share parser context to improve performance
        /// </summary>
        static readonly XmlParserContext XmlContext = CreateContext();

        static XmlParserContext CreateContext()
        {
            var nt = new NameTable();
            var nsmanager = new XmlNamespaceManager(nt);
            nsmanager.AddNamespace("log4j", "http://jakarta.apache.org/log4j/");
            return new XmlParserContext(nt, nsmanager, "elem", XmlSpace.None, Encoding.UTF8);
        }

        /// <summary>
        /// Parse LOG4JXml from string
        /// </summary>
        public static LogMessage ParseLog4JXmlLogEvent(string logEvent, string defaultLogger)
        {
            try
            {
                using (var reader = new XmlTextReader(logEvent, XmlNodeType.Element, XmlContext))
                    return ParseLog4JXmlLogEvent(reader, defaultLogger);
            }
            catch (Exception e)
            {
                return new LogMessage
                {
                    // Create a simple log message with some default values
                    LoggerName = defaultLogger,
                    ThreadName = "NA",
                    Message = logEvent,
                    Date = DateTime.Now,
                    Level = LogLevel.Info.ToString().ToUpper(),
                    ExceptionString = e.Message
                };
            }
        }

        /// <summary>
        /// Parse LOG4JXml from xml stream
        /// </summary>
        public static LogMessage ParseLog4JXmlLogEvent(Stream logStream, string defaultLogger)
        {
            using (var reader = XmlReader.Create(logStream, XmlSettings, XmlContext))
                return ParseLog4JXmlLogEvent(reader, defaultLogger);
        }

        /// <summary>
        /// Here we expect the log event to use the log4j schema.
        /// Sample:
        ///     <log4j:event logger="IoTSHMonitorService" timestamp="1533712597749" level="INFO" thread="1">
        ///         <log4j:message>info</log4j:message>
        ///         <log4j:properties>
        ///             <log4j:data name="log4japp" value="IoTSHMonitorService.exe" />
        ///             <log4j:data name="log4net:UserName" value="Jason-PC\Jason" />
        ///             <log4j:data name="log4jmachinename" value="Jason-PC" />
        ///             <log4j:data name="log4net:HostName" value="Jason-PC" />
        ///         </log4j:properties>
        ///     </log4j:event>
        ///     
        /// </summary>
        public static LogMessage ParseLog4JXmlLogEvent(XmlReader reader, string defaultLogger)
        {
            var logMsg = new LogMessage();

            reader.Read();
            if ((reader.MoveToContent() != XmlNodeType.Element) || (reader.Name != "log4j:event"))
                throw new Exception("The Log Event is not a valid log4j Xml block.");

            logMsg.LoggerName = reader.GetAttribute("logger");

            //获取日志等级
            logMsg.Level = reader.GetAttribute("level");
            logMsg.ThreadName = reader.GetAttribute("thread");

            long timeStamp;
            if (long.TryParse(reader.GetAttribute("timestamp"), out timeStamp))
                logMsg.Date = s1970.AddMilliseconds(timeStamp).ToLocalTime();

            int eventDepth = reader.Depth;
            reader.Read();
            while (reader.Depth > eventDepth)
            {
                if (reader.MoveToContent() == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "log4j:message":
                            logMsg.Message = reader.ReadString();
                            break;

                        case "log4j:throwable":
                            logMsg.ExceptionString = reader.ReadString();
                            break;

                        case "log4j:locationInfo":
                            break;

                        case "log4j:properties":
                            reader.Read();
                            while (reader.MoveToContent() == XmlNodeType.Element
                                   && reader.Name == "log4j:data")
                            {
                                string name = reader.GetAttribute("name");
                                string value = reader.GetAttribute("value");
                                logMsg.Properties[name] = value;
                                reader.Read();
                            }
                            break;
                    }
                }
                reader.Read();
            }

            return logMsg;
        }
    }
}
