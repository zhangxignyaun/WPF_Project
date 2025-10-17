using AutoMapper;
using DailyAPI.DataModel;
using DailyAPI.DTOs;

namespace DailyAPI.AutoMappers
{
    /// <summary>
    /// model之间的转换设置
    /// </summary>
    public class AutoMapperSetting:Profile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AutoMapperSetting()
        {
            //账号信息
            CreateMap<AccountInfoDTO,AccountInfo>().ReverseMap();
            //待办事项
            CreateMap<WaitDTO, WaitInfo>().ReverseMap();
            //备忘录
            CreateMap<MemoDTO, MemoInfo>().ReverseMap();
        }
    }
}
