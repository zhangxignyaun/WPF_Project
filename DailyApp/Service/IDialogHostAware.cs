﻿using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.Service
{
    internal interface IDialogHostAware
    {
        /// <summary>
        /// 打开过程中执行
        /// </summary>
        /// <param name="parameters"></param>
        void OnDialogOpening(IDialogParameters parameters);
        
        /// <summary>
        /// 确定
        /// </summary>
        DelegateCommand SaveCommand { get; set; }
        /// <summary>
        /// 取消
        /// </summary>
        DelegateCommand CancelCommand {  get; set; }
    }
}
