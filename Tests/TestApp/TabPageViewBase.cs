using System.Windows.Controls;

namespace TestApp
{
    /// <summary>
    /// Base class for Page views.
    /// A Page view is a generic view that is shown in the main TabControl;
    /// each tab shows a separate "Page".
    /// </summary>
    public abstract class TabPageViewBase : UserControl
    {
        protected TabPageViewBase()
            : base()
        { }
    }
}
