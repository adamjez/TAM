using OnRadio.App.Installers;
using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Autofac;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using OnRadio.App.ViewModels;
using OnRadio.App.Views;
using OnRadio.App.Common;
using OnRadio.App.Services;
using OnRadio.DAL;
using Windows.Foundation;
using Microsoft.Toolkit.Uwp;

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
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            OnLaunchCore(e.PreviousExecutionState, e.Arguments, e.PrelaunchActivated);

            // Ensure the current window is active
            Window.Current.Activate();

            try
            {
                // Install the main VCD. Since there's no simple way to test that the VCD has been imported, or that it's your most recent
                // version, it's not unreasonable to do this upon app load.
                StorageFile vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"OnRadioCommands.xml");
                await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.
                    InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);

                //// Update phrase list.
                ViewModelLocator locator = new ViewModelLocator();

                var cortanaService = locator.Resolve<CortanaService>();
                await cortanaService.UpdateRadioPhraseList("OnRadioCommandSet_en-us");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Installing Voice Commands Failed: " + ex.ToString());
            }
        }

        private void OnLaunchCore(ApplicationExecutionState state, string arguments, bool prelaunchActivated)
        {
            DispatcherHelper.Initialize();

            CreateRootFrame(state, arguments, prelaunchActivated);
            LocalDatabaseStorage.CreateDatabase();
            SaveRoamingSettings();
            LoadRoamingSettings();


        }


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

                        NormalizeFrameBackStack();
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
        /// OnActivated is the entry point for an application when it is launched via
        /// means other normal user interaction. This includes Voice Commands, URI activation,
        /// being used as a share target from another app, etc. Here, we're going to handle the
        /// Voice Command activation from Cortana.
        /// 
        /// Note: Be aware that an older VCD could still be in place for your application if you
        /// modify it and update your app via the store. You should be aware that you could get 
        /// activations that include commands in older versions of your VCD, and you should try
        /// to handle them gracefully.
        /// </summary>
        /// <param name="args">Details about the activation method, including the activation
        /// phrase (for voice commands) and the semantic interpretation, parameters, etc.</param>
        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            string navigationArgument = null;
            //ViewModel.TripVoiceCommand? navigationCommand = null;

            // If the app was launched via a Voice Command, this corresponds to the "show trip to <location>" command. 
            // Protocol activation occurs when a tile is clicked within Cortana (via the background task)
            if (args.Kind == ActivationKind.VoiceCommand)
            {
                // The arguments can represent many different activation types. Cast it so we can get the
                // parameters we care about out.
                var commandArgs = args as VoiceCommandActivatedEventArgs;

                SpeechRecognitionResult speechRecognitionResult = commandArgs.Result;

                // The commandMode is either "voice" or "text", and it indictes how the voice command
                // was entered by the user.
                // Apps should respect "text" mode by providing feedback in silent form.
                navigationArgument = this.SemanticInterpretation("radio", speechRecognitionResult);
            }


            OnLaunchCore(args.PreviousExecutionState, navigationArgument, false);

            NormalizeFrameBackStack();


            // Ensure the current window is active
            Window.Current.Activate();
        }

        private static void NormalizeFrameBackStack()
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null)
            {
                if (rootFrame.BackStackDepth == 0)
                {
                    rootFrame.BackStack.Insert(0,
                        new PageStackEntry(typeof(RadioList), null, new SuppressNavigationTransitionInfo()));
                }
                else if (rootFrame.BackStackDepth > 1)
                {
                    for (int i = rootFrame.BackStackDepth - 1; i > 0; i--)
                    {
                        rootFrame.BackStack.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the semantic interpretation of a speech result. Returns null if there is no interpretation for
        /// that key.
        /// </summary>
        /// <param name="interpretationKey">The interpretation key.</param>
        /// <param name="speechRecognitionResult">The result to get an interpretation from.</param>
        /// <returns></returns>
        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
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

        private void SaveRoamingSettings()
        {
            var helper = new RoamingObjectStorageHelper();
            helper.Save("isHQ", true);
            helper.Save("lastRadio", "radio_name");
        }

        private void LoadRoamingSettings()
        {
            var helper = new RoamingObjectStorageHelper();
            string keyIsHQ = "isHQ";
            string keyLastRadio = "lastRadio";
            if (helper.KeyExists(keyIsHQ))
            {
                string result = helper.Read<string>(keyIsHQ);
                //Debug.WriteLine(result);
            }
            if (helper.KeyExists(keyLastRadio))
            {
                string result = helper.Read<string>(keyLastRadio);
                //Debug.WriteLine(result);
            }
        }
    }
}