using AutoMapper;
using DailyAPI.ApiResponses;
using DailyAPI.DataModel;
using DailyAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyAPI.Controllers
{
    /// <summary>
    /// 备忘录接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemoController : ControllerBase
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly DailyDBContext _db;
        private readonly IMapper _mapper;

        public MemoController(DailyDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// 备忘录统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult StatMemo()
        {
            ApiResponse apiResponses = new ApiResponse();
            try
            {
                int count = _db.MemoInfo.Count();
                apiResponses.ResultCode = 1;
                apiResponses.Msg = "备忘录总数获取成功";
                apiResponses.ResultData=count;
            }
            catch (Exception)
            {
                apiResponses.ResultCode = -99;
                apiResponses.Msg = "服务器异常";
                
            }
            return Ok(apiResponses);
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        /// <param name="memoDTO"></param>
        /// <returns>1：添加成功；-99：服务器异常</returns>
        [HttpPost]
        public IActionResult AddMemo(MemoDTO memoDTO)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                MemoInfo  memoInfo = _mapper.Map<MemoInfo>(memoDTO);
                _db.MemoInfo.Add(memoInfo);
                int result = _db.SaveChanges();
                if (result == 1)
                {
                    response.ResultCode = 1;
                    response.Msg = "备忘录添加成功";
                }
                else
                {
                    response.ResultCode = -1;
                    response.Msg = "备忘录添加失败";
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
        /// 备忘录数据查询
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult QueryMemo(string? title)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var query = from A in _db.MemoInfo
                            select new MemoDTO()
                            {
                                MemoId = A.MemoId,
                                Title = A.Title,
                                Content = A.Content
                            } ;
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Title.Contains(title));
                }
                response.ResultCode = 1;
                response.Msg = "查询成功";
                response.ResultData = query;
            }
            catch (Exception)
            {

                response.ResultCode = -99;
                response.Msg = "服务器异常";
            }
            return Ok(response);
        }

        /// <summary>
        /// 删除备忘录信息
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteMemo(int memoId)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var dbInfo = _db.MemoInfo.Find(memoId);//_db.WaitInfo.Where(t=>t.WaitId ==waitDTO.WaitId).FirstOrDefault() 
                if (dbInfo != null)
                {
                    _db.Remove(dbInfo);
                    int result = _db.SaveChanges();
                    if (result == 1)
                    {
                        apiResponse.ResultCode = 1;
                        apiResponse.Msg = $"备忘录【{dbInfo.Title}】删除成功";
                    }
                    else
                    {
                        apiResponse.ResultCode = -1;
                        apiResponse.Msg = $"备忘录【{dbInfo.Title}】删除失败";
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
    }
}
