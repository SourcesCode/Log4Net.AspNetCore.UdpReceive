using Log4Net.AspNetCore.UdpReceive.Core.Log;
using Log4Net.AspNetCore.UdpReceive.Core.Utils;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Log4Net.AspNetCore.UdpReceive.Core.Receive
{
    public class UdpReceiver : BaseReceiver
    {
        [NonSerialized]
        private Thread _workerThread;
        [NonSerialized]
        private UdpClient _udpClient;
        [NonSerialized]
        private IPEndPoint _remoteEndPoint;

        [Category("Configuration")]
        [DisplayName("UDP Port Number")]
        [DefaultValue(7071)]
        public int ListeningPort { get; set; }
        [Category("Configuration")]
        [DisplayName("Use IPv6 Addresses")]
        [DefaultValue(false)]
        public bool IsUseIpV6 { get; set; }

        [Category("Configuration")]
        [DisplayName("Multicast Group Address (Optional)")]
        public string MulticastGroupAddress { get; set; }
        public string LevelFilterStr { get; set; }
        public LogLayoutTypeEnum LogLayoutType { get; set; }
        private bool IsContinueToListenToUDP
        {
            get
            {
                return _udpClient != null && _remoteEndPoint != null;
            }
        }

        #region IReceiver Members

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            //获取配置中接受的日志等级
            LevelFilterStr = ConfigHelper.Configuration["Udp:Level_Filter"];

            var multicastGroupAddress = ConfigHelper.Configuration["Udp:MulticastGroupAddress"];
            var isUseIpV6 = ConfigHelper.Configuration["Udp:IsUseIpV6"];
            var listeningPort = ConfigHelper.Configuration["Udp:ListeningPort"];
            var logLayoutType = ConfigHelper.Configuration["Udp:LogLayoutType"];
            MulticastGroupAddress = multicastGroupAddress;
            IsUseIpV6 = Convert.ToBoolean(isUseIpV6);
            ListeningPort = Convert.ToInt32(listeningPort);
            if ("XmlLayoutSchemaLog4j".Equals(logLayoutType))
            {
                LogLayoutType = LogLayoutTypeEnum.XmlLayoutSchemaLog4j;
            }
            else
            {
                LogLayoutType = LogLayoutTypeEnum.PatternLayout;
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public override void Start()
        {
            if (_workerThread != null && _workerThread.IsAlive)
                return;

            // Init connexion here, before starting the thread, to know the status now
            _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _udpClient = IsUseIpV6 ? new UdpClient(ListeningPort, AddressFamily.InterNetworkV6) : new UdpClient(ListeningPort);

            if (!String.IsNullOrEmpty(MulticastGroupAddress))
                _udpClient.JoinMulticastGroup(IPAddress.Parse(MulticastGroupAddress));

            // We need a working thread
            _workerThread = new Thread(ReceiveMessage);
            _workerThread.IsBackground = true;
            _workerThread.Start();
        }

        /// <summary>
        /// 终止
        /// </summary>
        public override void Terminate()
        {
            if (_udpClient != null)
            {
                _udpClient.Close();
                _udpClient = null;
                _remoteEndPoint = null;
            }

            if (_workerThread != null && _workerThread.IsAlive)
                _workerThread.Abort();
            _workerThread = null;
        }

        #endregion

        /// <summary>
        /// 接受消息
        /// </summary>
        private void ReceiveMessage()
        {
            while (IsContinueToListenToUDP)
            {
                try
                {
                    byte[] buffer = _udpClient.Receive(ref _remoteEndPoint);

                    if (Notifiable == null)
                        continue;

                    //获取LogMessage
                    LogMessage logMsg = ParserHelper.ParseLayout(buffer, LogLayoutType);

                    //检查等级过滤
                    if (!string.IsNullOrWhiteSpace(LevelFilterStr) &&
                        !LevelFilterStr.Contains(logMsg.Level))
                    {
                        //需要过滤，且配置中不包含该等级名称
                        continue;
                    }

                    //保存日志到数据库
                    Notifiable.Notify(logMsg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

    }
}


