using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.Tools
{
    /// <summary>
    /// Md5工具类
    /// </summary>
    public class Md5Hepler
    {
        /// <summary>
        /// Md5
        /// </summary>
        /// <param name="content">明文</param>
        /// <returns>Md5</returns>
        public static string  GetMd5(string content)
        {
            return string.Join("",MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(content)).Select(x =>x.ToString("x2")));
        }
    }
}
