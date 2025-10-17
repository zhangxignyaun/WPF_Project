using AutoMapper;
using DailyAPI.ApiResponses;
using DailyAPI.DataModel;
using DailyAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WaitController : ControllerBase
    {
        private readonly ILogger<WaitController> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        public WaitController(DailyDBContext _db, IMapper mapper, ILogger<WaitController> logger)
        {
            this._db = _db;
            this.mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly DailyDBContext _db;
        private readonly IMapper mapper;
        /// <summary>
        /// 统计待办事项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult StatWait()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var list = _db.WaitInfo.ToList();
                var finishList = list.Where(m=>m.Status==1).ToList();

                StatWaitDTO statWaitDTO = new StatWaitDTO() { FinishCount = finishList.Count ,TotleCount = list.Count };

                response.ResultCode = 1;//统计成功
                response.Msg="待办事项统计成功";
                response.ResultData = statWaitDTO;
            }
            catch (Exception)
            {

                response.ResultCode = -99;
                response.Msg = "服务器异常，请稍后再试。。。";
            }
            return Ok(response);
        }
        /// <summary>
        /// 添加代办事项
        /// </summary>
        /// <param name="waitDTO"></param>
        /// <returns>1:添加成功; -1:添加失败;  -99:服务器异常</returns>
        [HttpPost]
        public IActionResult AddWait(WaitDTO waitDTO)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                WaitInfo waitInfo = mapper.Map<WaitInfo>(waitDTO);
                _db.WaitInfo.Add(waitInfo);
                int result = _db.SaveChanges();
                if (result ==1)
                {
                    response.ResultCode = 1;
                    response.Msg = "待办事项添加成功";
                }
                else
                {
                    response.ResultCode = -1;
                    response.Msg = "待办事项添加失败";
                }
            }
            catch (Exception)
            {

                response.ResultCode = -99;
                response.Msg = "服务器异常，请稍后再试....";
            }

            return Ok(response);
        }
        /// <summary>
        /// 获取代办状态的所有待办事项
        /// </summary>
        /// <returns>1;获取成功；-1:获取失败;-99:服务器异常</returns>
        [HttpGet]
        public IActionResult GetWaitTodo()
        {
            ApiResponse apiResponse = new ApiResponse();

            try
            {
                var list = from A in _db.WaitInfo where A.Status==0
                           select new WaitDTO
                           {
                               Status = A.Status,
                               Content = A.Content,
                               WaitId = A.WaitId,
                               Title = A.Title
                           };
                apiResponse.ResultCode = 1;
                apiResponse.Msg = "获取成功";
                apiResponse.ResultData = list;
            }
            catch (Exception)
            {

                apiResponse.ResultCode = -99;
                apiResponse.Msg = "服务器异常，请稍后再试";
            }
            return Ok(apiResponse);
        }

        /// <summary>
        /// 编辑修改待办事项
        /// </summary>
        /// <param name="waitDTO"></param>
        /// <returns>1:修改成功；-1:id不存在；-99:服务器异常</returns>
        [HttpPut]
        public IActionResult UpdateWaitStatus(WaitDTO waitDTO)
        {
            ApiResponse apiResponse = new ApiResponse();

            try
            {
                
                var dbInfo =_db.WaitInfo.Find(waitDTO.WaitId);//_db.WaitInfo.Where(t=>t.WaitId ==waitDTO.WaitId).FirstOrDefault() 
                if (dbInfo !=null)
                {
                    dbInfo.Status = waitDTO.Status;
                    dbInfo.Title = waitDTO.Title;
                    dbInfo.Content = waitDTO.Content;
                    int result = _db.SaveChanges();
                    if (result == 1)
                    {
                        apiResponse.ResultCode = 1;
                        apiResponse.Msg = "修改成功";
                    }
                    else
                    {
                        apiResponse.ResultCode = -1;
                        apiResponse.Msg = "修改失败";
                    }
                }
                else
                {
                    apiResponse.ResultCode = -1;
                    apiResponse.Msg = "id不存在";
                }
                
                
            }
            catch (Exception)
            {

                apiResponse.ResultCode = -99;
                apiResponse.Msg = "服务器异常，请稍后再试";
            }
            return Ok(apiResponse);
        }

        /// <summary>
        /// 待办查询
        /// </summary>
        /// <param name="title">标题(模糊查询)</param>
        /// <param name="status">状态(等值查询)</param>
        /// <returns>1：查询成功；-99：服务器异常</returns>
        [HttpGet]
        public IActionResult QueryWait(string? title,int? status)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var query = from A in _db.WaitInfo
                            select new WaitDTO
                            {
                                WaitId = A.WaitId,
                                Title = A.Title,
                                Status = A.Status,
                                Content = A.Content

                            };
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Title.Contains(title));
                }
                if (status != null)
                {
                    query = query.Where(m => m.Status == status);

                }

                apiResponse.ResultData = query;
                apiResponse.ResultCode = 1;
                apiResponse.Msg = "待办数据查询成功";
            }
            catch (Exception)
            {

                apiResponse.ResultCode = -99;
                apiResponse.Msg = "服务器异常";
            }
            return Ok(apiResponse);
        }
    }
}
