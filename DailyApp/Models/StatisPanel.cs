using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.Models
{
    /// <summary>
    /// 首页统计面板
    /// </summary>
    public class StatisPanel : BindableBase
    {
        /// <summary>
        /// 统计图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 统计项名称
        /// </summary>
        public string ItemName { get; set; }

        public string _Result;
        /// <summary>
        /// 统计结果
        /// </summary>
        public string Result
        {
            get { return _Result; }
            set
            {
                _Result = value;
                RaisePropertyChanged();
            }
        }


        /// 背景颜色
        /// </summary>
        public string BackColor { get; set; }
        /// <summary>
        /// 点击过后跳转到的页面名称
        /// </summary>
        public string ViewName { get; set; }

        public string Hand
        {
            get
            {
                if (ViewName != null)
                {
                    return "Hand";

                }
                else
                {
                    return "";
                }
            }
        }
    }
}
