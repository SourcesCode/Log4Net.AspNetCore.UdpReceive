using Log4Net.AspNetCore.UdpReceive.Core.Log;
using System;

namespace Log4Net.AspNetCore.UdpReceive.Core.Receive
{
    [Serializable]
    public abstract class BaseReceiver : MarshalByRefObject, IReceiver
    {
        [NonSerialized]
        protected ILogMessageNotifiable Notifiable;

        #region IReceiver Members

        public abstract void Initialize();
        public abstract void Start();
        public abstract void Terminate();
        public virtual void Attach(ILogMessageNotifiable notifiable)
        {
            Notifiable = notifiable;
        }

        public virtual void Detach()
        {
            Notifiable = null;
        }

        #endregion
    }
}
