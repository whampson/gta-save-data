using System.Linq;
using System.Windows.Controls;

namespace TestApp
{
    public class RadioMenuItem : MenuItem
    {
        public string GroupName { get; set; }
        private new bool IsCheckable { get; }   // Hide this property as it messes up the state of the checkbox

        protected override void OnClick()
        {
            if (Parent is ItemsControl ic)
            {
                RadioMenuItem menuItem = ic.Items.OfType<RadioMenuItem>()
                    .FirstOrDefault(i => i.GroupName == GroupName && i.IsChecked);
                if (menuItem != null)
                {
                    menuItem.IsChecked = false;
                }
                IsChecked = true;
            }
            base.OnClick();
        }
    }
}
