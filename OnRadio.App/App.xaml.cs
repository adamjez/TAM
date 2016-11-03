using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Threading;
using OnRadio.App.ViewModels;
using OnRadio.App.Views;

namespace OnRadio.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Construct();

            //InitializeCore();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif

            DispatcherHelper.Initialize();

            CreateRootFrame(e.PreviousExecutionState, e.Arguments, e.PrelaunchActivated);

            // Ensure the current window is active
            Window.Current.Activate();
        }


        //public static IContainer AutofacContainer;
        //private void InitializeCore()
        //{
        //    var builder = new ContainerBuilder();

        //    builder.RegisterModule<IoCInstaller>();
        //    builder.RegisterModule<ViewModelInstaller>();


        //    AutofacContainer = builder.Build();
        //}

        /// <summary>
        /// Creates the frame containing the view
        /// </summary>
        /// <remarks>
        /// This is the same code that was in OnLaunched() initially.
        /// It is moved to a separate method so the view can be restored
        /// when leaving the background if it was unloaded when the app
        /// entered the background.
        /// </remarks>
        private void CreateRootFrame(ApplicationExecutionState previousExecutionState, string arguments, bool prelaunchActivated)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                System.Diagnostics.Debug.WriteLine("CreateFrame: Initializing root frame ...");

                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame
                {
                    // Set the default language
                    Language = Windows.Globalization.ApplicationLanguages.Languages[0]
                };


                rootFrame.NavigationFailed += OnNavigationFailed;

                if (previousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;

                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            }

            if (!prelaunchActivated)
            {
                
                if (!string.IsNullOrEmpty(arguments))
                {
                    // User launched app throw secondary tile
                    var page = rootFrame.Content as Player;
                    var viewModel = page?.DataContext as PlayerViewModel;

                    if (page == null || viewModel == null || viewModel.Radio.Id != arguments)
                    {
                        rootFrame.Navigate(typeof(Player), arguments);
                    }
                }
                else if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(RadioList), arguments);
                }
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null && rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        // Add any application contructor code in here.
        partial void Construct();
    }
}