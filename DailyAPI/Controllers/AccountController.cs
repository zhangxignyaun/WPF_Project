using AutoMapper;
using DailyAPI.ApiResponses;
using DailyAPI.DataModel;
using DailyAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyAPI.Controllers
{
    /// <summary>
    /// 账户接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly ILogger<AccountController> _logger;
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly DailyDBContext db;
        /// <summary>
        /// automapper
        /// </summary>
        private readonly IMapper mapper;
        public AccountController(DailyDBContext _db,IMapper _mapper, ILogger<AccountController> logger)
        {
            db = _db;
            mapper = _mapper;
            _logger = logger;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="accountInfoDTO">注册信息</param>
        /// <returns>1:注册成功,-1:账号已存在,-99:未知错误</returns>
        [HttpPost]
        public IActionResult Reg(AccountInfoDTO accountInfoDTO)
        {
            ApiResponse result = new ApiResponse();
            //业务处理
            try
            {
                //1.账号是否存在
                var dbAccount = db.accountInfos.Where(m =>m.Account == accountInfoDTO.Account).FirstOrDefault();
                if (dbAccount != null)
                {
                    result.ResultCode = -1;//账号已存在
                    result.Msg = "该账号已被注册";
                    return Ok(result);
                }
                //2.不存在则添加账号
                AccountInfo accountInfo = mapper.Map<AccountInfo>(accountInfoDTO);
                db.accountInfos.Add(accountInfo);
                int resultRow = db.SaveChanges();
                if (resultRow == 1)
                {
                    result.ResultCode = 1;//注册成功
                    result.Msg = "注册成功";
                }
                else
                {
                    result.ResultCode = -99;//服务器忙
                    result.Msg = "服务器忙，请稍后重试";
                }
            }
            catch (Exception)
            {

                result.ResultCode = -99;//服务器忙
                result.Msg = "服务器忙，请稍后重试";
            }
            return Ok(result);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string account, string password)
        {
            ApiResponse result = new ApiResponse();
            try
            {
                _logger.LogInformation($"用户{account}登录....");
                var dbAccountInfo = db.accountInfos.Where(m => m.Account == account).FirstOrDefault();
                if (dbAccountInfo == null)
                {
                    result.ResultCode= -1;
                    result.Msg = "账号错误";
                    return Ok(result);
                }
               if(dbAccountInfo.Password != password)
                {
                    result.ResultCode = -2;
                    result.Msg = "密码错误";
                    return Ok(result);
                }

                result.ResultCode = 1;
                result.Msg = "登陆成功";
                result.ResultData = dbAccountInfo;
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.ResultCode = -22;
                result.Msg = "服务器异常";
                _logger.LogError(ex,"发生了未处理的异常");
            }
            return Ok(result);
        }
    }
}
