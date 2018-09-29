using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Net.AspNetCore.UdpReceive.Core.Log
{
    public class LogMessage
    {
        /// <summary>
        /// Time Stamp.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Thread Name.
        /// </summary>
        public string ThreadName { get; set; }
        /// <summary>
        /// Log Level.
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// Logger Name.
        /// </summary>
        public string LoggerName { get; set; }
        /// <summary>
        /// NDC.
        /// </summary>
        public string NDC { get; set; }
        /// <summary>
        /// Log Message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// An exception message to associate to this message.
        /// </summary>
        public string ExceptionString { get; set; }
        /// <summary>
        /// Run Millisecond
        /// </summary>
        public int RunMillisecond { get; set; }
        /// <summary>
        /// AppDomain
        /// </summary>
        public string AppDomain { get; set; }
        /// <summary>
        /// Properties collection.
        /// </summary>
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

    }
}
