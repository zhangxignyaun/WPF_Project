using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyAPI.DataModel
{
    /// <summary>
    /// 备忘录数据模型
    /// </summary>
    [Table("MemoInfoDB")]
    public class MemoInfo
    {
        /// <summary>
        /// 备忘事项id
        /// </summary>
        [Key]
        public int MemoId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
  
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
