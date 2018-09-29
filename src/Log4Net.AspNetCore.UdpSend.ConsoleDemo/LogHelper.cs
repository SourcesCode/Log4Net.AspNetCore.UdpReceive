using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;

namespace Log4Net.AspNetCore.UdpSend.ConsoleDemo
{
    public class LogHelper
    {
        private static ILog _log = null;
        static LogHelper()
        {
            ILoggerRepository repository = LogManager.CreateRepository("NetCoreLogHelperRepository");
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            FileInfo configFile = new FileInfo(filePath);
            if (!configFile.Exists)
            {
                throw new Exception("未配置log4net配置文件！");
            }
            XmlConfigurator.Configure(repository, configFile);
            //_log = LogManager.GetLogger(repository.Name, "NetCoreLogHelper");
            _log = LogManager.GetLogger(repository.Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// 记录LOG日志,同时将Exception信息也记录
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        /// <param name="level"></param>
        public static void Write(string message, LogLevel level, Exception exception = null)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _log.Debug(message, exception);
                    break;
                case LogLevel.Info:
                    _log.Info(message, exception);
                    break;
                case LogLevel.Warn:
                    _log.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    _log.Error(message, exception);
                    break;
                case LogLevel.Fatal:
                    _log.Fatal(message, exception);
                    break;
            }
        }

    }

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
