namespace DailyAPI.DTOs
{
    /// <summary>
    /// 统计待办事项DTO
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
        public string FinishPercent
        {
            get
            {
                if (TotleCount != 0)
                {
                    return (FinishCount * 100.00 / TotleCount).ToString("f2") + "%";
                }
                else
                {
                    return "0.00%";
                }
            }

        }


    }
}
