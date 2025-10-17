using System.ComponentModel.DataAnnotations;

namespace DailyAPI.DTOs
{
    public class MemoDTO
    {
        /// <summary>
        /// 备忘事项id
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
