using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Net.AspNetCore.UdpReceive.Core.Utils
{
    public class ConfigHelper
    {
        public static IConfiguration Configuration;
        public static void Init()
        {

            var builder = new ConfigurationBuilder()
               //.SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)//增加环境配置文件，新建项目默认有
            //.AddEnvironmentVariables();
            Configuration = builder.Build();

        }
    }
}
