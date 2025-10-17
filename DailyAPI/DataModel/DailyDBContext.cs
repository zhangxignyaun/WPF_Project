using Microsoft.EntityFrameworkCore;

namespace DailyAPI.DataModel
{
    public class DailyDBContext:DbContext
    {
        public DailyDBContext(DbContextOptions<DailyDBContext> options):base(options)
        {

        }
        /// <summary>
        /// 账号信息
        /// </summary>
        public virtual DbSet<AccountInfo> accountInfos { get; set; }  
        /// <summary>
        /// 待办事项
        /// </summary>
        public virtual DbSet<WaitInfo> WaitInfo { get; set; }
        /// <summary>
        /// 备忘录
        /// </summary>
        public virtual DbSet<MemoInfo> MemoInfo { get; set; }
    }
}
