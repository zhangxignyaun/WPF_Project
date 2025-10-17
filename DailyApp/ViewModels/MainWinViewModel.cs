using DailyApp.Extension;
using DailyApp.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DailyApp.ViewModels
{
    /// <summary>
    /// 主界面视图模型
    /// </summary>
    public class MainWinViewModel : BindableBase
    {
        private readonly NavigationService _NavigationService;
        public MainWinViewModel(IRegionManager _RegionManager)
        {
            LeftMenuList = new ObservableCollection<LeftMenuInfoModel>();
            //创建菜单数据
            CreateMenu();
            //区域
            this.RegionManager = _RegionManager;

            NavigateCommand = new DelegateCommand<LeftMenuInfoModel>(Navigate);
            //前进
            GoForwardCommand = new DelegateCommand(GoForward);
            //后退
            GobackCommand = new DelegateCommand(Goback);

            //_NavigationService = NavigationService;
            //_NavigationService.Navigated += OnNavigated;
        }

        #region 左侧菜单
        private ObservableCollection<LeftMenuInfoModel> _LeftMenuList;
        /// <summary>
        /// 菜单集合
        /// </summary>
        public ObservableCollection<LeftMenuInfoModel> LeftMenuList
        {
            get { return _LeftMenuList; }
            set
            {
                _LeftMenuList = value;
                RaisePropertyChanged();
            }
        }

        #endregion
        /// <summary>
        ///创建菜单数据
        /// </summary>
        public void CreateMenu()
        {
            LeftMenuList.Add(new LeftMenuInfoModel() { Icon = "Home", MenuName = "首页", ViewName = "HomeUC" });
            LeftMenuList.Add(new LeftMenuInfoModel() { Icon = "NoteBookOutLine", MenuName = "待办事项", ViewName = "WaitUC" });
            LeftMenuList.Add(new LeftMenuInfoModel() { Icon = "NoteBookPlus", MenuName = "备忘录", ViewName = "MemoUC" });
            LeftMenuList.Add(new LeftMenuInfoModel() { Icon = "Cog", MenuName = "设置", ViewName = "SettingUC" });
        }
        #region 区域导航
        public readonly IRegionManager RegionManager;

        public DelegateCommand<LeftMenuInfoModel> NavigateCommand { get; set; }

        public void Navigate(LeftMenuInfoModel menu)
        {
            if (menu == null || string.IsNullOrEmpty(menu.ViewName))
            {
                return;
            }
            RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(menu.ViewName, callback =>
            {
                journal = callback.Context.NavigationService.Journal;
            });
        }
        #endregion

        #region 前进后退
        private IRegionNavigationJournal journal;//导航日志

        public DelegateCommand GobackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }

        public void Goback()
        {
            if (journal != null && journal.CanGoBack)
            {
                journal.GoBack();
            }
        }
        public void GoForward()
        {
            if (journal != null && journal.CanGoForward)
            {
                journal.GoForward();
            }
        }
        #endregion

        #region 设置默认首页
        public void SetDefaultHome(string loginName)
        {
            NavigationParameters pairs = new NavigationParameters();
            pairs.Add("LoginName", loginName);
            RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("HomeUC", callback =>
            {

                journal = callback.Context.NavigationService.Journal;
            }, pairs);
        }
        #endregion

        //private string _CurrentView;

        //public string CurrentView
        //{
        //    get { return _CurrentView; }
        //    set
        //    {
        //        _CurrentView = value; 
        //        RaisePropertyChanged();
        //    }
        //}

        //private void OnNavigated(object sender, NavigationEventArgs e)
        //{
        //    // 从导航Uri中提取视图标识（如“HomeView”）
        //    // 注：导航时需使用一致的Uri（如_navigationService.NavigateAsync("HomeView")）
        //    string currentView = e.Uri.ToString();
        //    // 更新选中状态变量（与TabItem的Tag匹配）
        //    CurrentView = currentView;
        //}
        //public void Dispose()
        //{
        //    if (_NavigationService != null)
        //    {
        //        _NavigationService.Navigated -= OnNavigated;
        //    }
        //}

    }
}
