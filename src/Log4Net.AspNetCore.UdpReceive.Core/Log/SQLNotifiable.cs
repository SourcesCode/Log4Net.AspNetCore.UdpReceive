using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Log4Net.AspNetCore.UdpReceive.Core.Utils;

namespace Log4Net.AspNetCore.UdpReceive.Core.Log
{
    public class SQLNotifiable : ILogMessageNotifiable
    {
        #region ILogMessageNotifiable Members

        public void Notify(LogMessage[] logMsgs)
        {
            foreach (LogMessage logMsg in logMsgs)
            {
                Notify(logMsg);
            }
        }

        public void Notify(LogMessage logMsg)
        {
            SaveToDB(logMsg);
        }

        #endregion

        private void SaveToDB(LogMessage message)
        {
            try
            {
                string sql = "insert into Log4Net values(@Date,@appname,@Thread,@Level,@Logger,@Message,@Exception,@UserId,@ClientIp)";
                SqlParameter[] paramList = new SqlParameter[9];
                SqlParameter p1 = new SqlParameter("@Date", System.Data.SqlDbType.DateTime, 8);
                p1.Value = message.Date;
                SqlParameter p2 = new SqlParameter("@Thread", System.Data.SqlDbType.VarChar, 255);
                p2.Value = message.ThreadName;
                SqlParameter p3 = new SqlParameter("@Level", System.Data.SqlDbType.VarChar, 50);
                p3.Value = message.Level;
                SqlParameter p4 = new SqlParameter("@Logger", System.Data.SqlDbType.VarChar, 255);
                p4.Value = message.LoggerName;
                SqlParameter p5 = new SqlParameter("@Message", System.Data.SqlDbType.VarChar, 4000);
                p5.Value = message.Message;
                SqlParameter p6 = new SqlParameter("@Exception", System.Data.SqlDbType.VarChar, 40000);
                p6.Value = message.ExceptionString ?? "";
                SqlParameter p7 = new SqlParameter("@UserId", System.Data.SqlDbType.VarChar, 50);
                SqlParameter p8 = new SqlParameter("@ClientIp", System.Data.SqlDbType.VarChar, 20);
                SqlParameter p9 = new SqlParameter("@appname", System.Data.SqlDbType.VarChar, 50);
                if (message.Properties == null || message.Properties.Count == 0)
                {
                    p7.Value = string.Empty;
                    p8.Value = string.Empty;
                    p9.Value = string.Empty;
                }
                else
                {
                    p7.Value = message.Properties["log4net:UserName"];
                    p8.Value = message.Properties["log4net:HostName"];
                    p9.Value = message.Properties["log4japp"];
                }
                paramList[0] = p1;
                paramList[1] = p2;
                paramList[2] = p3;
                paramList[3] = p4;
                paramList[4] = p5;
                paramList[5] = p6;
                paramList[6] = p7;
                paramList[7] = p8;
                paramList[8] = p9;

                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionDBString, System.Data.CommandType.Text, sql, paramList);
            }
            catch
            {
            }
        }
    }
}
