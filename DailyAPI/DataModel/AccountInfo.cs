using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyAPI.DataModel
{
    /// <summary>
    /// 登陆账号数据模型
    /// </summary>
    [Table("Accounttb")]
    public class AccountInfo
    {
        /// <summary>
        /// 账号ID
        /// </summary>
        [Key]//主键，自增
        public int AccountId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
