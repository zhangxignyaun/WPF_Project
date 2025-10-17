using DailyApp.Extension;
using DailyApp.Models;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DailyApp.ViewModels
{
    public class PersonalUCViewModel:BindableBase
    {
        public PersonalUCViewModel()
        {
            ChangeHueCommand = new DelegateCommand<object>(ChangeHue);
        }
        private bool _isDarkTheme;

        #region 主题背景色
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                
                if (SetProperty(ref _isDarkTheme,value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? BaseTheme.Dark : BaseTheme.Light));
                }
            }
        }

        public static void ModifyTheme(Action<Theme> modifyaction)
        {
            var paletthelper = new PaletteHelper();
            Theme theme = paletthelper.GetTheme();
            modifyaction?.Invoke(theme);
            paletthelper.SetTheme(theme);
        }
        #endregion

        #region 顶部背景色
        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;
        private PaletteHelper paletteHelper = new PaletteHelper();
        public DelegateCommand<object> ChangeHueCommand { get; }

        private void ChangeHue(object? obj)
        {
            Theme theme = paletteHelper.GetTheme();
            var color =(Color)obj;
            theme.PrimaryLight = new ColorPair(color.Lighten());
            theme.PrimaryMid = new ColorPair(color);
            theme.PrimaryDark = new ColorPair(color.Darken());

            paletteHelper.SetTheme(theme);
        }
        #endregion
    }
}
