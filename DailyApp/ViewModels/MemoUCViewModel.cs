using DailyApp.DTOs;
using DailyApp.HttpClients;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.ViewModels
{
    public class MemoUCViewModel : BindableBase
    {

        private readonly HttpResClient _httpClient;
        private readonly ILogger<MemoUCViewModel> _logger;
        public MemoUCViewModel(HttpResClient httpClient, ILogger<MemoUCViewModel> logger)
        {
            _httpClient = httpClient;
            QueryMemoList();
            ShowRightMemoCommand = new DelegateCommand(ShowrightMemo);
            QueryMemoListCommand = new DelegateCommand(QueryMemoList);
            AddMemoCommand = new DelegateCommand(AddMemo);
            DeleteMemoCommand = new DelegateCommand<MemoInfoDTO>(DeleteMemo);

            _logger = logger;
        }
        private List<MemoInfoDTO> _MemoInfoList;

        public List<MemoInfoDTO> MemoInfoList
        {
            get { return _MemoInfoList; }
            set
            {
                _MemoInfoList = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 创建备忘录事项数据
        /// </summary>
        //public void CreateMemoInfoList()
        //{
        //    MemoInfoList = new List<MemoInfoDTO>();

        //}

        #region 显示右侧添加代办

        public DelegateCommand ShowRightMemoCommand { get; set; }
        private bool _IsShowRightMemo;

        public bool IsShowRightMemo
        {
            get { return _IsShowRightMemo; }
            set
            {
                _IsShowRightMemo = value;
                RaisePropertyChanged();
            }
        }
        public void ShowrightMemo()
        {
            IsShowRightMemo = true;
        }
        #endregion

        #region 添加备忘录
        public MemoInfoDTO MemoInfoDTO { get; set; } = new MemoInfoDTO();

        public DelegateCommand AddMemoCommand { get; set; }

        public void AddMemo()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.POST;
            apiRequest.Parameters = MemoInfoDTO;
            apiRequest.Route = $"Memo/AddMemo";

            ApiResponse response = _httpClient.Excute(apiRequest);
            if (response.ResultCode == 1)
            {
                QueryMemoList();//刷新列表

                _logger.LogInformation("备忘录添加成功");
                IsShowRightMemo = false;//隐藏窗口
            }
            else
            {
                MessageBox.Show(response.Msg);
            }
        }
        #endregion

        #region 查询备忘录数据
        public string searchTitle { get; set; }
        public DelegateCommand QueryMemoListCommand { get; set; }
        public void QueryMemoList()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = $"Memo/QueryMemo?title={searchTitle}";

            ApiResponse response = _httpClient.Excute(apiRequest);
            if (response.ResultCode == 1)
            {
                MemoInfoList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(response.ResultData.ToString());
                VisibilityMemo = (MemoInfoList.Count > 0) ? Visibility.Hidden : Visibility.Visible;
            }
            else
            {
                MemoInfoList = new List<MemoInfoDTO>();
            }
        }

        private Visibility _VisibilityMemo;
        public Visibility VisibilityMemo
        {
            get { return _VisibilityMemo; }
            set
            {
                _VisibilityMemo = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region 删除备忘录

        public DelegateCommand<MemoInfoDTO> DeleteMemoCommand { get; set; }

        public void DeleteMemo(MemoInfoDTO memoInfoDTO)
        {
            var selResult = MessageBox.Show("确定删除吗？", "温馨提示", MessageBoxButton.OKCancel);
            if (selResult ==MessageBoxResult.OK)
            {
                ApiRequest apiRequest = new ApiRequest();
                apiRequest.Method = RestSharp.Method.DELETE;
                apiRequest.Route = $"Memo/DeleteMemo?memoId={memoInfoDTO.MemoId}";

                ApiResponse response = _httpClient.Excute(apiRequest);
                if (response.ResultCode == 1)
                {
                    MessageBox.Show(response.Msg);
                    QueryMemoList();
                }
                else
                {
                    MessageBox.Show(response.Msg);

                }
            }
        }
        #endregion
    }
}
