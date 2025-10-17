namespace DailyAPI.DTOs
{
    /// <summary>
    /// 账号DTO,用来接收注册信息
    /// </summary>
    public class AccountInfoDTO
    {
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
