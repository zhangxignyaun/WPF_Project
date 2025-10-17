using System.ComponentModel.DataAnnotations;

namespace DailyAPI.DTOs
{
    /// <summary>
    /// 待办事项DTO(接收主页待办事项添加数据)
    /// </summary>
    public class WaitDTO
    {
        /// <summary>
        /// 待办事项id
        /// </summary>
        public int WaitId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
