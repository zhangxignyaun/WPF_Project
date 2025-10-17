using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.DTOs
{
    /// <summary>
    /// 待办事项DTO
    /// </summary>
    public class WaitInfoDTO
    {
        /// <summary>
        /// 待办事项id
        /// </summary>
        public int WaitId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        public string BackColor {
            get
            {
                return Status == 0 ? "Green" : "#00BFFF";
            }
        }
    }
}
