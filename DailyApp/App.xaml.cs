using DailyApp.HttpClients;
using DailyApp.Service;
using DailyApp.ViewModels;
using DailyApp.ViewModels.Dialog;
using DailyApp.Views;
using DailyApp.Views.Dialog;
using DryIoc;
using log4net.Config;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection.Metadata;
using System.Windows;

namespace DailyApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWin>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 加载log4net配置文件（如果是App.config，可省略文件路径）
            XmlConfigurator.Configure(new FileInfo("Log4net.config"));
        }
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //登录
            containerRegistry.RegisterDialog<LoginUC,LoginUCViewModel>();
            //请求
            containerRegistry.GetContainer().Register<HttpResClient>(made:Parameters.Of.Type<string>(serviceKey:"webUrl"));
            //导航页
            containerRegistry.RegisterForNavigation<HomeUC,HomeUCViewModel>();//首页
            containerRegistry.RegisterForNavigation<MemoUC,MemoUCViewModel>();//备忘录
            containerRegistry.RegisterForNavigation<WaitUC,WaitUCViewModel>();//待办事项
            containerRegistry.RegisterForNavigation<SettingUC,SettingUCViewModel>();//设置页

            //设置页内部导航
            containerRegistry.RegisterForNavigation<PersonalUC,PersonalUCViewModel>();//个性化
            containerRegistry.RegisterForNavigation<SystemUC>();//系统设置
            containerRegistry.RegisterForNavigation<AboutUsUC>();//关于我们

            //添加待办事项
            containerRegistry.RegisterForNavigation<AddWaitUC,AddWaitUCViewModel>();
            containerRegistry.Register<DialogHostService>();
            //编辑代办事项
            containerRegistry.RegisterForNavigation<EditWaitUC,EditWaitUCViewModel>();

            //添加备忘录
            containerRegistry.RegisterForNavigation<AddMemoUC, AddMemoUCViewModel>();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInitialized()
        {

            var dilog = Container.Resolve<DialogService>();
            dilog.ShowDialog("LoginUC", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {

                    Environment.Exit(0);
                    return;
                }
                var mainVM = Current.MainWindow.DataContext as MainWinViewModel;
                if (mainVM != null)
                {
                    if (callback.Parameters != null)
                    {
                        string loginName = callback.Parameters.GetValue<string>("LoginName");
                        mainVM.SetDefaultHome(loginName);
                    }

                }
                base.OnInitialized();
            });

        }
    }

}
