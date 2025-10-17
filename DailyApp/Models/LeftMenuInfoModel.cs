using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.Models
{
    /// <summary>
    /// 左侧菜单
    /// </summary>
    public class LeftMenuInfoModel
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 菜单项名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 视图名称
        /// </summary>
        public string ViewName { get; set; }
    }
}
