using DailyApp.Extension;
using DailyApp.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.ViewModels
{
    /// <summary>
    /// 设置页视图模型
    /// </summary>
    public class SettingUCViewModel : BindableBase
    {
        public SettingUCViewModel(IRegionManager _RegionManager)
        {
            CreateSettingList();
            this.RegionManager = _RegionManager;
            NavigateCommand = new DelegateCommand<LeftMenuInfoModel>(Navigate);
        }
        #region 菜单
        private List<LeftMenuInfoModel> _LeftMenuList;

        public List<LeftMenuInfoModel> LeftMenuList
        {
            get { return _LeftMenuList; }
            set
            {
                _LeftMenuList = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 创建菜单
        /// </summary>
        void CreateSettingList()
        {
            LeftMenuList = new List<LeftMenuInfoModel>();
            LeftMenuList.Add(new LeftMenuInfoModel() { Icon="Palatte",MenuName="个性化",ViewName="PersonalUC"});
            LeftMenuList.Add(new LeftMenuInfoModel() { Icon="Cog",MenuName="系统设置",ViewName="SystemUC"});
            LeftMenuList.Add(new LeftMenuInfoModel() { Icon="Information",MenuName="关于更多",ViewName="AboutUsUC"});
        }
        #endregion

        #region 区域导航
        public readonly IRegionManager RegionManager;

        public DelegateCommand<LeftMenuInfoModel> NavigateCommand { get; set; }

        public void Navigate(LeftMenuInfoModel menu)
        {
            if (menu == null || string.IsNullOrEmpty(menu.ViewName))
            {
                return;
            }
            RegionManager.Regions[PrismManager.SettingRegionName].RequestNavigate(menu.ViewName);
        }
        #endregion
    }
}
