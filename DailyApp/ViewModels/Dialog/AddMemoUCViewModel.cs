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
    internal class AddMemoUCViewModel : IDialogHostAware
    { /// <summary>
      /// 对话框主机唯一标识
      /// </summary>
        private const string IDialogHostName = "RootDialog";
        public AddMemoUCViewModel()
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
        /// 执行过程
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpening(IDialogParameters parameters)
        {

        }
        /// <summary>
        /// 保存方法
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrEmpty(MemoInfoDTO.Title) || string.IsNullOrEmpty(MemoInfoDTO.Content))
            {
                MessageBox.Show("备忘录信息不全！");
                return;
            }

            if (DialogHost.IsDialogOpen(IDialogHostName))
            {
                DialogParameters pairs = new DialogParameters();
                pairs.Add("AddMemoInfo", MemoInfoDTO);
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
        public MemoInfoDTO MemoInfoDTO { get; set; } = new MemoInfoDTO();
    }
}
