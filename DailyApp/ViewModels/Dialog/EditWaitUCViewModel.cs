using DailyApp.DTOs;
using DailyApp.Service;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.ViewModels.Dialog
{
    /// <summary>
    /// 编辑待办数据模型
    /// </summary>
    internal class EditWaitUCViewModel:IDialogHostAware
    {
        /// <summary>
        /// 对话框主机唯一标识
        /// </summary>
        private const string IDialogHostName = "RootDialog";
        public EditWaitUCViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }
        /// <summary>
        /// 确定
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }
        /// <summary>
        /// 取消
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }
        /// <summary>
        /// 执行过程,接收数据
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpening(IDialogParameters parameters)
        {
            WaitInfoDTO = parameters.GetValue<WaitInfoDTO>("OldWaitInfo");
        }
        /// <summary>
        /// 保存方法
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrEmpty(WaitInfoDTO.Title) || string.IsNullOrEmpty(WaitInfoDTO.Content))
            {
                MessageBox.Show("待办事项信息不全！");
                return;
            }

            if (DialogHost.IsDialogOpen(IDialogHostName))
            {
                DialogParameters pairs = new DialogParameters();
                pairs.Add("newWaitInfo", WaitInfoDTO);
                DialogHost.Close(IDialogHostName, new DialogResult(ButtonResult.OK, pairs));
            }
        }
        /// <summary>
        /// 取消方法
        /// </summary>
        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(IDialogHostName))
            {
                DialogHost.Close(IDialogHostName, new DialogResult(ButtonResult.No));
            }
        }
        /// <summary>
        /// 待办事项录入信息
        /// </summary>
        public WaitInfoDTO WaitInfoDTO { get; set; } = new WaitInfoDTO();
    }
}
