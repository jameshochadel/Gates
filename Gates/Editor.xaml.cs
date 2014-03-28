using Gates.Common;
using Gates.GElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Gates
{
    /// <summary>
    /// Contains the canvas on which elements can be placed and linked.
    /// </summary>
    public sealed partial class Editor : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private List<GPrimitive> GPrimitivesInFocus = new List<GPrimitive>();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public Editor()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.BottomAppBar = bottomAppBar;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        /// <summary>
        /// Delete the GPrimitive in focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBarDeletePrimitive_Click(object sender, RoutedEventArgs e)
        {
            foreach (GPrimitive element in GPrimitivesInFocus)
            {
                CircuitCanvas.Children.Remove(element); // does this get garbage collected..?
            }
            GPrimitivesInFocus.Clear();
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ToolbarPrimitiveAnd_DragLeave(object sender, DragEventArgs e)
        {
            GElements.GPrimitive g = new GElements.GPrimitive(0);
            //g.SetValue(Parent, EditorToolbar);
            g.Visibility = Windows.UI.Xaml.Visibility.Visible;
            //g.RenderTransform
            
        }

        #region Canvas zoom button event handlers
        private void CircuitCanvasZoom200_Click(object sender, RoutedEventArgs e)
        {
            CircuitCanvasScrollViewer.ChangeView(null, null, (float)2.0);
        }

        private void CircuitCanvasZoom100_Click(object sender, RoutedEventArgs e)
        {
            CircuitCanvasScrollViewer.ChangeView(null, null, (float)1.0);
        }

        private void CircuitCanvasZoom50_Click(object sender, RoutedEventArgs e)
        {
            CircuitCanvasScrollViewer.ChangeView(null, null, (float)0.5);
        }
        #endregion

        /// <summary>
        /// Add a new GPrimitive to the canvas when a CommandBar button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBarAdd_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton b = sender as AppBarButton;
            GElements.GPrimitive g = new GElements.GPrimitive(0);
            g.GPrimitive_PointerEntered += g_GPrimitive_PointerEntered;
            g.GPrimitive_PointerExited += g_GPrimitive_PointerExited;
            g.GPrimitive_Tapped += g_GPrimitive_Tapped;
            g.GPrimitive_RightTapped += g_GPrimitive_RightTapped;
            g.Visibility = Windows.UI.Xaml.Visibility.Visible;

            if (b == CommandBarAddPrimitiveAnd)
            {
                g.GateType = 0;
            }
            else if (b == CommandBarAddPrimitiveOr)
            {
                g.GateType = 1;
            }
            else if (b == CommandBarAddPrimitiveNand)
            {
                g.GateType = 2;
            }
            else if (b == CommandBarAddPrimitiveNor)
            {
                g.GateType = 3;
            }
            else if (b == CommandBarAddPrimitiveXand)
            {
                g.GateType = 4;
            }
            else if (b == CommandBarAddPrimitiveInv)
            {
                g.GateType = 5;
            }
            
            CircuitCanvas.Children.Add(g);
        }

        /// <summary>
        /// Clear the command bar of any contextual controls when it is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBar_Closed(object sender, object e)
        {
            if (bottomAppBar.SecondaryCommands.Count != 0) {
                bottomAppBar.SecondaryCommands.Clear();
            }
        }

        #region GPrimitive Event Handlers
        /// <summary>
        /// Show the i/o clickzones on pointer enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void g_GPrimitive_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            GPrimitive g = sender as GPrimitive;
            GWireHandle outputHandle = new GWireHandle();
            outputHandle.ParentPrimitive = g;
            g.ChildWireHandle = outputHandle;

            if (g.GateType == 5)
            {
                Canvas.SetLeft(outputHandle, Canvas.GetLeft(g) + 55);
                Canvas.SetTop(outputHandle, Canvas.GetTop(g) + 15);
            }
            else
            {
                CircuitCanvas.Children.Add(outputHandle);
                Canvas.SetLeft(outputHandle, Canvas.GetLeft(g) + 55);
                Canvas.SetTop(outputHandle, Canvas.GetTop(g) + 15);
            }
        }

        private void g_GPrimitive_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            GPrimitive g = sender as GPrimitive;
            //if it hasn't been attached to something, then remove it. If it has, return.
            CircuitCanvas.Children.Remove(g.ChildWireHandle);
        }

        private void g_GPrimitive_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GPrimitive g = sender as GPrimitive;
            // toggle visibility of GWireHandle
        }

        private void g_GPrimitive_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            GPrimitive g = sender as GPrimitive;
            if (this.BottomAppBar != null)
            {
                if(true) { //Replace condition with "If CommandBar does not contain a button named CommandBarDeletePrimitive"; also need to name it that
                    AppBarButton CommandBarDeletePrimitive = new AppBarButton();
                    CommandBarDeletePrimitive.Icon = new SymbolIcon(Symbol.Delete);
                    CommandBarDeletePrimitive.Label = "Delete Gate";
                    CommandBarDeletePrimitive.Click += CommandBarDeletePrimitive_Click; // need this click event to apply to sender parameter of this method
                    bottomAppBar.SecondaryCommands.Insert(0, CommandBarDeletePrimitive);

                    //bottomAppBar.Visibility = Windows.UI.Xaml.Visibility.Visible;

                    //CommandBar c = this.BottomAppBar as CommandBar;
                    //this.BottomAppBar.IsOpen = true;
                    //c.IsOpen = true;
                }
                GPrimitivesInFocus.Add(g);
            }
        }

        #endregion

        public class DeleteGPrimitiveEventArgs : RoutedEventArgs
        {
            public GPrimitive GPrimitiveInFocus { get; set; }
        }
    }
}
