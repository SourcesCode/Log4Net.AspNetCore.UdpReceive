using Log4Net.AspNetCore.UdpReceive.Core.Log;

namespace Log4Net.AspNetCore.UdpReceive.Core.Receive
{
    public interface IReceiver
    {
        void Initialize();
        void Start();
        void Terminate();
        void Attach(ILogMessageNotifiable notifiable);
        void Detach();
    }
}
