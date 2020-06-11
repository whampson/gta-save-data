using System;
using System.Collections.Generic;
using System.Text;
using WpfEssentials;

namespace TestApp
{
    public enum PageVisibility
    {
        /// <summary>
        /// Indicates that a tab page should always be visibile.
        /// </summary>
        Always,

        /// <summary>
        /// Indicates that a tab page should be visibile only when
        /// editing a file.
        /// </summary>
        WhenFileLoaded,

        /// <summary>
        /// Indicates that a tab page should be visibile only when
        /// not editing a file.
        /// </summary>
        WhenFileClosed
    }

    public abstract class TabPageViewModelBase : ObservableObject
    {
        private bool m_isVisible;

        /// <summary>
        /// Creates a new Page view model with the specified
        /// page title, page visibility, and reference to the main view model.
        /// </summary>
        /// <param name="title">The title of the page.</param>
        /// <param name="visibility">A value indicating when the page should be visible.</param>
        /// <param name="mainViewModel">A reference to the main view model.</param>
        public TabPageViewModelBase(string title, PageVisibility visibility, MainViewModel mainViewModel)
        {
            Title = title;
            Visibility = visibility;
            MainViewModel = mainViewModel;
            m_isVisible = visibility == PageVisibility.Always;

            MainViewModel.TabRefresh += MainViewModel_TabRefresh;
        }

        /// <summary>
        /// Gets the title of this page.
        /// </summary>
        public string Title
        {
            get;
        }

        /// <summary>
        /// Gets the page's visibility setting.
        /// </summary>
        public PageVisibility Visibility
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating wheter this page is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the main window's view model for accessing functions
        /// visible to the whole program.
        /// </summary>
        public MainViewModel MainViewModel
        {
            get;
        }

        private void MainViewModel_TabRefresh(object sender, TabRefreshEventArgs e)
        {
            if (Visibility == PageVisibility.Always)
            {
                IsVisible = true;
                return;
            }

            switch (e.Trigger)
            {
                case TabRefreshTrigger.WindowLoaded:
                case TabRefreshTrigger.FileClosed:
                    IsVisible = Visibility == PageVisibility.WhenFileClosed;
                    break;
                case TabRefreshTrigger.FileLoaded:
                    IsVisible = Visibility == PageVisibility.WhenFileLoaded;
                    break;
            }
        }
    }
}
