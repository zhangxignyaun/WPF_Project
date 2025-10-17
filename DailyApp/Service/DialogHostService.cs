﻿using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.Service
{
    public class DialogHostService:DialogService
    {
        private IContainerExtension _containerExtension;
        public DialogHostService(IContainerExtension containerExtension):base(containerExtension)
        {
            this._containerExtension = containerExtension;
        }
        public async Task<IDialogResult> ShowDialog(string name,IDialogParameters parameters,string dialogHostName= "RootDialog")
        {
            if(parameters == null)
                parameters = new DialogParameters();
            //从容器当中去除弹出窗口的实例
            var content = _containerExtension.Resolve<object>(name);
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("");

            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view,true);
            if (!(dialogContent.DataContext is IDialogHostAware viewModel))
                throw new NullReferenceException("");
            DialogOpenedEventHandler eventHandler = (sender,eventArgs) => 
            {
                if (viewModel is IDialogHostAware aware)
                {
                    aware.OnDialogOpening(parameters);
                }
                eventArgs.Session.UpdateContent(content);
            };
            return (IDialogResult)await DialogHost.Show(dialogContent,dialogHostName,eventHandler);
        }
    }
}
