using DailyApp.DTOs;
using DailyApp.Extension;
using DailyApp.HttpClients;
using DailyApp.Models;
using DailyApp.MsgEvents;
using DailyApp.Service;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Common;
using Prism.DryIoc;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.ViewModels
{
    public class HomeUCViewModel : BindableBase, INavigationAware
    {
        private readonly HttpResClient httpResClient;
        private readonly DialogHostService dialogHostService;
       
        public HomeUCViewModel(HttpResClient httpResClient, DialogHostService dialogHostService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            CreateStatPanelList();//统计面板
            this.httpResClient = httpResClient;
            GetWaitInfoList();//待办事项
            GetMemoInfoList();//备忘录  
            
            NavigateCommand = new DelegateCommand<StatisPanel>(Navigate);

            this.dialogHostService = dialogHostService;
            ShowAddWaitCommand = new DelegateCommand(ShowAddWait);
            ChangeWaitStatusCommand = new DelegateCommand<WaitInfoDTO>(ChangeWaitStatus);//修改待办事项状态
            ShowEditWaitDialogCommand = new DelegateCommand<WaitInfoDTO>(ShowEditWaitDialog);//打开编辑对话窗口


            ShowAddMemoCommand = new DelegateCommand(ShowAddMemo);
            _RegionManager = regionManager;
            CallStatMemo();//统计备忘录
            //订阅消息
            //eventAggregator.GetEvent<AddWaitMsgEvent>().Subscribe(OnAddWaitInfo);
        }
        #region 统计面板数据
        private ObservableCollection<StatisPanel> _StatisPanelList;

        public ObservableCollection<StatisPanel> StatisPanelList
        {
            get { return _StatisPanelList; }
            set
            {
                _StatisPanelList = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 创建统计面板数据
        /// </summary>
        private void CreateStatPanelList()
        {
            StatisPanelList = new ObservableCollection<StatisPanel>();
            StatisPanelList.Add(new StatisPanel() { Icon = "Clockfast", ItemName = "汇总", BackColor = "#BA55D3", Result = "9", ViewName = "WaitUC" });
            StatisPanelList.Add(new StatisPanel() { Icon = "ClockCheckOutline", ItemName = "已完成", BackColor = "#FFA500", Result = "9", ViewName = "WaitUC" });
            StatisPanelList.Add(new StatisPanel() { Icon = "ChartLineVariant", ItemName = "完成比例", BackColor = "#4876FF", Result = "9" });
            StatisPanelList.Add(new StatisPanel() { Icon = "PlaylistStar", ItemName = "备忘录", BackColor = "#ADFF2F", Result = "9", ViewName = "MemoUC" });
        }

        #endregion

        #region 统计面板导航

        private readonly IRegionManager _RegionManager;
        public DelegateCommand<StatisPanel> NavigateCommand { get; set; }

        public void Navigate(StatisPanel statisPanel)
        {
            if (!string.IsNullOrEmpty(statisPanel.ViewName))
            {
                if (statisPanel.ItemName == "已完成")
                {
                    NavigationParameters pairs = new NavigationParameters();
                    pairs.Add("SelectedIndex", 2);
                    _RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(statisPanel.ViewName, pairs);
                }
                else
                {
                    _RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(statisPanel.ViewName);
                }

            }
        }

        #endregion

        #region 待办事项数据
        public List<WaitInfoDTO> _WaitInfoList;

        public List<WaitInfoDTO> WaitInfoList
        {
            get { return _WaitInfoList; }
            set
            {
                _WaitInfoList = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 查询待办事项数据
        /// </summary>
        public void GetWaitInfoList()
        {

            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = "Wait/GetWaitTodo";

            ApiResponse apiResponse = httpResClient.Excute(apiRequest);
            if (apiResponse != null)
            {
                if (apiResponse.ResultCode == 1)
                {
                    WaitInfoList = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(apiResponse.ResultData.ToString());
                    RefreshWaitStat();
                }
            }
            else
            {
                WaitInfoList = new List<WaitInfoDTO>();
            }


        }
        #endregion

      
        #region 导航首页提示信息
        private string _loginInfo;

        public string LoginInfo
        {
            get { return _loginInfo; }
            set
            {
                _loginInfo = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 接收导航并处理
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("LoginName"))
            {
                DateTime now = DateTime.Now;
                string[] week = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                string loginName = navigationContext.Parameters.GetValue<string>("LoginName");
                LoginInfo = $"您好{loginName},今天是{now.ToString("yyyy-MM-dd")} {week[(int)now.DayOfWeek]}";
            }
            CallStatWait();
            GetWaitInfoList();
            GetMemoInfoList();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion

        #region 待办事项统计


        public StatWaitDTO StatWaitDTO = new StatWaitDTO();

        public void CallStatWait()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = "Wait/StatWait";
            try
            {
                ApiResponse apiResponse = httpResClient.Excute(apiRequest);
                if (apiResponse != null)
                {
                    if (apiResponse.ResultCode == 1)
                    {
                        StatWaitDTO = JsonConvert.DeserializeObject<StatWaitDTO>(apiResponse.ResultData.ToString());
                        RefreshWaitStat();
                    }
                }
            }
            catch (Exception)
            {


            }
        }
        public void RefreshWaitStat()
        {
            StatisPanelList[0].Result = StatWaitDTO.TotleCount.ToString();
            StatisPanelList[1].Result = StatWaitDTO.FinishCount.ToString();
            StatisPanelList[2].Result = StatWaitDTO.FinishPercent;
        }
        #endregion

        #region 待办事项新增
        public DelegateCommand ShowAddWaitCommand { get; set; }
        /// <summary>
        /// 打开待办添加对话框
        /// </summary>
        public async void ShowAddWait()
        {
            var result = await dialogHostService.ShowDialog("AddWaitUC", null);
            if (result.Result == ButtonResult.OK)
            {
                //接受对话框返回参数
                if (result.Parameters.ContainsKey("AddWaitInfo"))
                {
                    var waitInfo = result.Parameters.GetValue<WaitInfoDTO>("AddWaitInfo");
                    //调用Api实现待办数据写入
                    ApiRequest apiRequest = new ApiRequest();

                    apiRequest.Method = RestSharp.Method.POST;
                    apiRequest.Parameters = waitInfo;
                    apiRequest.Route = "Wait/AddWait";
                    ApiResponse response = httpResClient.Excute(apiRequest);

                    if (response.ResultCode == 1)
                    {

                        //刷新统计面板
                        CallStatWait();
                        GetWaitInfoList();


                    }
                    else
                    {
                        MessageBox.Show(response.Msg);
                    }

                }
            }
        }
        #endregion

        #region 待办事项编辑
        public DelegateCommand<WaitInfoDTO> ShowEditWaitDialogCommand { get; set; }
        /// <summary>
        /// 打开待办添加对话框
        /// </summary>
        public async void ShowEditWaitDialog(WaitInfoDTO waitInfoDTO)
        {
            DialogParameters oldWaitInfo = new DialogParameters();
            oldWaitInfo.Add("OldWaitInfo", waitInfoDTO);
            var result = await dialogHostService.ShowDialog("EditWaitUC", oldWaitInfo);
            if (result.Result == ButtonResult.OK)
            {
                //接受对话框返回参数
                if (result.Parameters.ContainsKey("newWaitInfo"))
                {
                    var waitInfo = result.Parameters.GetValue<WaitInfoDTO>("newWaitInfo");
                    //调用Api实现待办数据写入
                    ApiRequest apiRequest = new ApiRequest();

                    apiRequest.Method = RestSharp.Method.PUT;
                    apiRequest.Parameters = waitInfo;
                    apiRequest.Route = "Wait/UpdateWaitStatus";
                    ApiResponse response = httpResClient.Excute(apiRequest);

                    if (response.ResultCode == 1)
                    {

                        //刷新统计面板
                        CallStatWait();

                        GetWaitInfoList();
                    }
                    else
                    {
                        MessageBox.Show(response.Msg);
                    }

                }
            }
        }
        #endregion

        #region 待办状态修改
        public DelegateCommand<WaitInfoDTO> ChangeWaitStatusCommand { get; set; }
        private void ChangeWaitStatus(WaitInfoDTO waitInfoDTO)
        {
            //调用Api实现待办数据写入
            ApiRequest apiRequest = new ApiRequest();

            apiRequest.Method = RestSharp.Method.PUT;
            apiRequest.Parameters = waitInfoDTO;
            apiRequest.Route = "Wait/UpdateWaitStatus";
            ApiResponse response = httpResClient.Excute(apiRequest);

            if (response.ResultCode == 1)
            {
                MessageBox.Show(response.Msg);
                //刷新统计面板
                CallStatWait();
                //刷新待办列表
                GetWaitInfoList();
            }
            else
            {
                MessageBox.Show(response.Msg);
            }
        }
        #endregion


        #region 备忘录

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
        /// 创建待办事项数据
        /// </summary>
        //public void CreateMemoInfoList()
        //{
        //    MemoInfoList = new List<MemoInfoDTO>();
        //    MemoInfoList.Add(new MemoInfoDTO() { Title = "明天开会i", Content = "不想开" });
        //    MemoInfoList.Add(new MemoInfoDTO() { Title = "后天也要开会", Content = "给各级领导汇报" });
        //}



        /// <summary>
        /// 数据总量查询
        /// </summary>
        public void CallStatMemo()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;

            apiRequest.Route = "Memo/StatMemo";
            ApiResponse response = httpResClient.Excute(apiRequest);

            if (response.ResultCode == 1)
            {

                StatisPanelList[3].Result = response.ResultData.ToString();
            }

        }

        public DelegateCommand ShowAddMemoCommand { get; set; }
        /// <summary>
        /// 添加备忘录
        /// </summary>
        public async void ShowAddMemo()
        {
            var result = await dialogHostService.ShowDialog("AddMemoUC", null);
            if (result.Result == ButtonResult.OK)
            {
                //接受对话框返回参数
                if (result.Parameters.ContainsKey("AddMemoInfo"))
                {
                    var memoInfo = result.Parameters.GetValue<MemoInfoDTO>("AddMemoInfo");
                    //调用Api实现待办数据写入
                    ApiRequest apiRequest = new ApiRequest();

                    apiRequest.Method = RestSharp.Method.POST;
                    apiRequest.Parameters = memoInfo;
                    apiRequest.Route = "Memo/AddMemo";
                    ApiResponse response = httpResClient.Excute(apiRequest);

                    if (response.ResultCode == 1)
                    {

                        //刷新统计面板
                        CallStatMemo();
                        GetMemoInfoList();


                    }
                    else
                    {
                        MessageBox.Show(response.Msg);
                    }

                }
            }
        }
        /// <summary>
        /// 备忘录数据查询
        /// </summary>
        public void GetMemoInfoList()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = $"Memo/QueryMemo";

            ApiResponse apiResponse = httpResClient.Excute(apiRequest);
            if (apiResponse != null)
            {
                if (apiResponse.ResultCode == 1)
                {
                    MemoInfoList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(apiResponse.ResultData.ToString());
                    RefreshWaitStat();
                }
            }
            else
            {
                MemoInfoList = new List<MemoInfoDTO>();
            }
        }
        #endregion

        #region 订阅消息后执行的方法
        private void OnAddWaitInfo(WaitInfoDTO waitInfoDTO)
        {
            WaitInfoList.Add(waitInfoDTO);
        }
        #endregion

    }
}
