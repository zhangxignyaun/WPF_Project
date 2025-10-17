using DailyApp.DTOs;
using DailyApp.HttpClients;
using log4net.Core;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.ViewModels
{
    /// <summary>
    /// 待办事项视图模型
    /// </summary>
    public class WaitUCViewModel : BindableBase, INavigationAware
    {
        private readonly HttpResClient HttpClient;
        
        public WaitUCViewModel()
        {
            HttpClient = new HttpResClient();

          
            ShowRightWaitCommand = new DelegateCommand(ShowrightWait);
            QueryWaitCommand = new DelegateCommand(QueryeWaitInfoList);


        }
        private List<WaitInfoDTO> _WaitInfoList;

        public List<WaitInfoDTO> WaitInfoList
        {
            get { return _WaitInfoList; }
            set
            {
                _WaitInfoList = value;
                RaisePropertyChanged();
            }
        }
        #region 查询待办数据
        public string SearchWaitText { get; set; }

        private int _SearchWaitStatus;

        public int SearchWaitStatus
        {
            get { return _SearchWaitStatus; }
            set
            {
                _SearchWaitStatus = value;
                RaisePropertyChanged();
            }
        }



        public DelegateCommand QueryWaitCommand { get; set; }//查询代办数据命令
        /// <summary>
        /// 查询待办事项数据
        /// </summary>
        public void QueryeWaitInfoList()
        {

            int? status = null;
            if (SearchWaitStatus == 1)
            {
                status = 0;
            }
            if (SearchWaitStatus == 2)
            {
                status = 1;
            }
            //调用API取出数据
            ApiRequest apiRequest = new ApiRequest();

            apiRequest.Method = RestSharp.Method.GET;

            apiRequest.Route = $"Wait/QueryWait?title={SearchWaitText}&status={status}";
            ApiResponse response = HttpClient.Excute(apiRequest);

            if (response.ResultCode == 1)
            {
                WaitInfoList = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(response.ResultData.ToString());

            }
            else
            {
                WaitInfoList = new List<WaitInfoDTO>();
            }
        }
        #endregion

        #region 显示右侧添加代办

        public DelegateCommand ShowRightWaitCommand { get; set; }
        private string _IsShowRightWait;

        public string IsShowRightWait
        {
            get { return _IsShowRightWait; }
            set
            {
                _IsShowRightWait = value;
                RaisePropertyChanged();
            }
        }
        public void ShowrightWait()
        {
            IsShowRightWait = "True";
        }

        #endregion

        #region 导航

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("SelectedIndex"))
            {
                SearchWaitStatus = navigationContext.Parameters.GetValue<int>("SelectedIndex");
            }
            else
            {
                SearchWaitStatus = 0;
            }
            QueryeWaitInfoList();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion
    }
}
