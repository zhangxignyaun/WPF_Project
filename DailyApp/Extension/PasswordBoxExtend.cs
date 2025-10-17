using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DailyApp.Extension
{
    /// <summary>
    /// PasswordBox扩展属性
    /// </summary>
    public class PasswordBoxExtend
    {


        public static string GetPwd(DependencyObject obj)
        {
            return (string)obj.GetValue(PwdProperty);
        }

        public static void SetPwd(DependencyObject obj, string value)
        {
            obj.SetValue(PwdProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PwdProperty =
            DependencyProperty.RegisterAttached("Pwd", typeof(string), typeof(PasswordBoxExtend), new PropertyMetadata("",OnPwdChanged));

        /// <summary>
        /// 自定义附加属性发生变化时，改变Password属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        public static void OnPwdChanged(DependencyObject obj ,DependencyPropertyChangedEventArgs args)
        {
            PasswordBox pwdBox = obj as PasswordBox;
            string newPwd = (string)args.NewValue;
            if (pwdBox != null && newPwd !=pwdBox.Password)
            {
                pwdBox.Password = newPwd;   
            }
        }
    }
    /// <summary>
    /// password行为，当password变化时，自定义附加属性也跟着变化
    /// </summary>
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        /// <summary>
        /// password行为，当password变化时，自定义附加属性也跟着变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = PasswordBoxExtend.GetPwd(passwordBox);//附加属性的值
            if (passwordBox != null && passwordBox.Password !=password)
            {
                PasswordBoxExtend.SetPwd(passwordBox, passwordBox.Password);
            }
        }
        /// <summary>
        /// 注入事件
        /// </summary>
        protected override void OnAttached()
        {
            
            base.OnAttached();
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }
        /// <summary>
        /// 销毁事件
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }
    }
}
