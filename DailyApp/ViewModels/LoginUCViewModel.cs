using DailyApp.DTOs;
using DailyApp.HttpClients;
using DailyApp.MsgEvents;
using DailyApp.Tools;
using log4net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.ViewModels
{
    /// <summary>
    /// 登陆试图模型
    /// </summary>
    public class LoginUCViewModel :BindableBase, IDialogAware
    {
        public readonly HttpResClient httpResClient;
        public readonly IEventAggregator aggregator;
        
        public LoginUCViewModel(HttpResClient _httpResClient, IEventAggregator _aggregator)
        {
            LoginCommand = new DelegateCommand(Login);
            //切换登陆注册命令
            ShowRegOrLogCommand = new DelegateCommand<string>(ShowRegOrLog);
            //注册
            RegCommand = new DelegateCommand(Reg);
            //实例化注册信息
            AccountInfoDTO = new AccountInfoDTO();
            //
            this.httpResClient = _httpResClient;
            //
            aggregator = _aggregator;
           
            //_logger.LogInformation("LoginUCViewModel初始化完成"); // 记录信息级日志
        }
        public string Title { get; set; }="登录";
        
        /// <summary>
        /// 登陆命令
        /// </summary>
        public DelegateCommand LoginCommand { get; set; }

        /// <summary>
        /// 登录方法
        /// </summary>
        private void Login()
        {
            //数据验证
            if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Pwd))
            {
                //发布消息
                aggregator.GetEvent<MsgEvent>().Publish("登录信息不全");

                return;
            }
            
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            Pwd = Md5Hepler.GetMd5(Pwd);
            apiRequest.Route = $"Account/Login?account={Account}&password={Pwd}";
            
            ApiResponse response= httpResClient.Excute(apiRequest);
            if (response != null)
            {
                if (response.ResultCode==1)
                {
                    
                    AccountInfoDTO accountInfoDTO = JsonConvert.DeserializeObject<AccountInfoDTO>(response.ResultData.ToString());
                    DialogParameters pairs = new DialogParameters();
                    pairs.Add("LoginName",accountInfoDTO.Name);
                    RequestClose(new DialogResult(ButtonResult.OK,pairs));
                    

                }
                else
                {
                    //发布消息
                    aggregator.GetEvent<MsgEvent>().Publish(response.Msg);
                }
            }
            else
            {
                //发布消息
                aggregator.GetEvent<MsgEvent>().Publish("登陆返回信息为空");
            }
            
            
        }

        #region 注册
        public DelegateCommand RegCommand { get; set; }

        private void Reg()
        {
            //数据验证
            if (string.IsNullOrEmpty(AccountInfoDTO.Name) || string.IsNullOrEmpty(AccountInfoDTO.Account) || string.IsNullOrEmpty(AccountInfoDTO.Password) 
                || string.IsNullOrEmpty(AccountInfoDTO.ConfirmPwd))
            {
                //发布消息
                aggregator.GetEvent<MsgEvent>().Publish("注册信息不全");
                //MessageBox.Show("注册信息不全！");
                return;
            }
            if (AccountInfoDTO.Password != AccountInfoDTO.ConfirmPwd)
            {
                //发布消息
                aggregator.GetEvent<MsgEvent>().Publish("两次输入的密码不一致，请重新输入");
                //MessageBox.Show("两次输入的密码不一致，请重新输入");
                return;
            }
            //调用Api
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.POST;
            apiRequest.Route = "Account/Reg";

            //密码加密
            AccountInfoDTO.Password = Md5Hepler.GetMd5(AccountInfoDTO.Password);
            AccountInfoDTO.ConfirmPwd = Md5Hepler.GetMd5(AccountInfoDTO.ConfirmPwd);

            apiRequest.Parameters = AccountInfoDTO;

            ApiResponse apiResponse = httpResClient.Excute(apiRequest);
            if (apiResponse != null)
            {
                if (apiResponse.ResultCode ==1)
                {
                    //发布消息
                    aggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                    //MessageBox.Show(apiResponse.Msg);
                    //跳转登录
                    SelectdeIndex="0";
                }
                else
                {
                    aggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                }
            }
            else
            {
                aggregator.GetEvent<MsgEvent>().Publish("接口返回信息为空");
                return;
            }
        }
        private AccountInfoDTO _AccountInfoDTO;
        /// <summary>
        /// 注册信息
        /// </summary>
        public AccountInfoDTO AccountInfoDTO
        {
            get { return _AccountInfoDTO; }
            set
            {
                _AccountInfoDTO = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        public event Action<IDialogResult> RequestClose;
        /// <summary>
        /// 是否允许关闭对话窗
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        #region 显示内容
        private string _SelectedIndex;

        public string SelectdeIndex
        {
            get { return _SelectedIndex; }
            set 
            {
                _SelectedIndex = value;
                RaisePropertyChanged();
            }
        }
        public DelegateCommand<string> ShowRegOrLogCommand { get; set; }
        private void ShowRegOrLog(string index)
        {
            SelectdeIndex = index;
        }
        #endregion

        #region 登录信息
        private string _Pwd;
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get { return _Pwd; }
            set 
            {
                _Pwd = value; 
                
            }
        }
        private string _Account;
        /// <summary>
        /// 账号
        /// </summary>
        public string Account
        {
            get { return _Account; }
            set { _Account = value; }
        }


        #endregion

    }
}
