using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Net.AspNetCore.UdpReceive.Core.Log
{
    [Serializable]
    public enum LogLevel
    {
        None = -1,
        /// <summary>
        /// 跟踪信息
        /// </summary>
        Trace,
        /// <summary>
        /// 调试信息
        /// </summary>
        Debug,
        /// <summary>
        /// 一般信息
        /// </summary>
        Info,
        /// <summary>
        /// 警告信息
        /// </summary>
        Warn,
        /// <summary>
        /// 错误信息
        /// </summary>
        Error,
        /// <summary>
        /// 重大错误信息
        /// </summary>
        Fatal
    }

}
