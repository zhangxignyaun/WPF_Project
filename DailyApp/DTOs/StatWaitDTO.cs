using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.DTOs
{
    /// <summary>
    /// 接收API待办事项统计结果数据模型
    /// </summary>
    public class StatWaitDTO
    {
        /// <summary>
        /// 代办总数
        /// </summary>
        public int TotleCount { get; set; }
        /// <summary>
        /// 已完成数量
        /// </summary>
        public int FinishCount { get; set; }
        /// <summary>
        /// 完成比例
        /// </summary>
        public string FinishPercent { get; set; }
        
    }
}
