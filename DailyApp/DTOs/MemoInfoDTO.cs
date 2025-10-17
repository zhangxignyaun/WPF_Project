using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.DTOs
{
    /// <summary>
    /// 备忘录DTO
    /// </summary>
    public class MemoInfoDTO
    {
        /// <summary>
        /// 备忘录id
        /// </summary>
        public int MemoId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        
    }
}
