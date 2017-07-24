using NopFramework.Core;
using NopFramework.Services.Task;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services
{
    public class LogTask : ITask
    {
        private static string filePath = @"D:\TestfRAME\logs.txt";
        //private readonly IWebHelper _webHelper;
        //public LogTask(IWebHelper web)
        //{
        //    this._webHelper = web;
        //}
        public void Execute()
        {
            string content = " WriteLog：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";

            FileInfo fs = new FileInfo(filePath);
            StreamWriter sw = fs.AppendText();
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
        }
    }
}
