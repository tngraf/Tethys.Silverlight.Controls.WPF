#region Header
// --------------------------------------------------------------------------
// Tethys.Silverlight.Controls.WPF
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="ModernWindow.cs" company="Tethys">
// Copyright  2014-2015 by T. Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Microsoft .Net Framework 4.5. 
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

// ReSharper disable once CheckNamespace
namespace Tethys.Silverlight.Controls.WPF
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Windows.Shell;

    /********************************************************************************
     * Many thanks to Magnus for his great article
     * http://blog.magnusmontin.net/2013/03/16/how-to-create-a-custom-window-in-wpf/
     *******************************************************************************/

    /// <summary>
    /// Interaction logic for ModernWindow.
    /// </summary>
    [TemplatePart(Name = PartOuterBorderName, Type = typeof(Border))]
    [TemplatePart(Name = PartInnerBorderName, Type = typeof(Border))]
    [TemplatePart(Name = PartMoveRectangleName, Type = typeof(Rectangle))]
    [TemplatePart(Name = PartCaptionImageName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PartTitleTextBlockName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PartMinimizeButtonName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PartMaximizeButtonName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PartRestoreButtonName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PartCloseButtonName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PartResizeGripName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PartResizeGridName, Type = typeof(FrameworkElement))]
    public class ModernWindow : Window
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The part outer border name.
        /// </summary>
        private const string PartOuterBorderName = "PART_OuterBorder";

        /// <summary>
        /// The part inner border name.
        /// </summary>
        private const string PartInnerBorderName = "PART_InnerBorder";

        /// <summary>
        /// The part move rectangle name.
        /// </summary>
        private const string PartMoveRectangleName = "PART_MoveRectangle";

        /// <summary>
        /// The part caption image name.
        /// </summary>
        private const string PartCaptionImageName = "PART_CaptionImage";

        /// <summary>
        /// The part title text block name.
        /// </summary>
        private const string PartTitleTextBlockName = "PART_TitleTextBlock";

        /// <summary>
        /// The part minimize button name.
        /// </summary>
        private const string PartMinimizeButtonName = "PART_MinimizeButton";

        /// <summary>
        /// The part maximize button name.
        /// </summary>
        private const string PartMaximizeButtonName = "PART_MaximizeButton";

        /// <summary>
        /// The part restore button name.
        /// </summary>
        private const string PartRestoreButtonName = "PART_RestoreButton";

        /// <summary>
        /// The part close button name.
        /// </summary>
        private const string PartCloseButtonName = "PART_CloseButton";

        /// <summary>
        /// The part resize grip name.
        /// </summary>
        private const string PartResizeGripName = "PART_ResizeGrip";

        /// <summary>
        /// The part resize grid name.
        /// </summary>
        private const string PartResizeGridName = "PART_ResizeGrid";

        /// <summary>
        /// The invisible chrome.
        /// </summary>
        private readonly WindowChrome invisibleChrome = new WindowChrome
            {
                GlassFrameThickness = new Thickness(0), 
                CaptionHeight = 0, 
                ResizeBorderThickness = new Thickness(0), 
                CornerRadius = new CornerRadius(0.0)
            };

        /// <summary>
        /// The window handle.
        /// </summary>
        private HwndSource hwndSource;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region DEPENDENCY PROPERTIES
        #region DP - LoadedCommand
        /// <summary>
        /// Dependency property for the loaded command.
        /// </summary>
        public static readonly DependencyProperty LoadedCommandProperty =
          DependencyProperty.Register("LoadedCommand", typeof(ICommand),
          typeof(ModernWindow), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the loaded command.
        /// </summary>
        public ICommand LoadedCommand
        {
            get { return (ICommand)this.GetValue(LoadedCommandProperty); }
            set { this.SetValue(LoadedCommandProperty, value); }
        }
        #endregion

        #region DP - ActivatedCommand
        /// <summary>
        /// Dependency property for the activated command.
        /// </summary>
        public static readonly DependencyProperty ActivatedCommandProperty =
          DependencyProperty.Register("ActivatedCommand", typeof(ICommand),
          typeof(ModernWindow), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the activated command.
        /// </summary>
        public ICommand ActivatedCommand
        {
            get { return (ICommand)this.GetValue(ActivatedCommandProperty); }
            set { this.SetValue(ActivatedCommandProperty, value); }
        }
        #endregion

        #region DP - ClosingCommand
        /// <summary>
        /// Dependency property for the closing command.
        /// </summary>
        public static readonly DependencyProperty ClosingCommandProperty =
          DependencyProperty.Register("ClosingCommand", typeof(ICommand),
          typeof(ModernWindow), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the closing command.
        /// </summary>
        public ICommand ClosingCommand
        {
            get { return (ICommand)this.GetValue(ClosingCommandProperty); }
            set { this.SetValue(ClosingCommandProperty, value); }
        }
        #endregion

        #region DP - ResizedCommand
        /// <summary>
        /// Dependency property for the resized command.
        /// </summary>
        public static readonly DependencyProperty ResizedCommandProperty =
          DependencyProperty.Register("ResizedCommand", typeof(ICommand),
          typeof(ModernWindow), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the resized command.
        /// </summary>
        public ICommand ResizedCommand
        {
            get { return (ICommand)this.GetValue(ResizedCommandProperty); }
            set { this.SetValue(ResizedCommandProperty, value); }
        }
        #endregion

        #region DP - WindowStateChangeCommand
        /// <summary>
        /// Dependency property for the resized window state change command.
        /// </summary>
        public static readonly DependencyProperty WindowStateChangeCommandProperty =
          DependencyProperty.Register("WindowStateChangeCommand", typeof(ICommand),
          typeof(ModernWindow), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the window state change command.
        /// </summary>
        public ICommand WindowStateChangeCommand
        {
            get { return (ICommand)this.GetValue(WindowStateChangeCommandProperty); }
            set { this.SetValue(WindowStateChangeCommandProperty, value); }
        }
        #endregion

        #region DP - ShowMinButton
        /// <summary>
        /// Dependency property for the show minimum button flag.
        /// </summary>
        public static readonly DependencyProperty ShowMinButtonProperty =
          DependencyProperty.Register("ShowMinButton", typeof(bool),
          typeof(ModernWindow), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether to show the minimum button.
        /// </summary>
        public bool ShowMinButton
        {
            get { return (bool)this.GetValue(ShowMinButtonProperty); }
            set { this.SetValue(ShowMinButtonProperty, value); }
        }
        #endregion

        #region DP - ShowCloseButton
        /// <summary>
        /// Dependency property for the show close button flag.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty =
          DependencyProperty.Register("ShowCloseButton", typeof(bool),
          typeof(ModernWindow), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether to show the close button.
        /// </summary>
        public bool ShowCloseButton
        {
            get { return (bool)this.GetValue(ShowCloseButtonProperty); }
            set { this.SetValue(ShowCloseButtonProperty, value); }
        }
        #endregion

        #region DP - ResizeGridBrush
        /// <summary>
        /// Dependency property for the resize grip brush.
        /// </summary>
        public static readonly DependencyProperty ResizeGridBrushProperty =
          DependencyProperty.Register("ResizeGridBrush", typeof(Brush),
          typeof(ModernWindow), new PropertyMetadata(Brushes.White));

        /// <summary>
        /// Gets or sets the resize grip brush.
        /// </summary>
        public Brush ResizeGridBrush
        {
            get { return (Brush)this.GetValue(ResizeGridBrushProperty); }
            set { this.SetValue(ResizeGridBrushProperty, value); }
        }
        #endregion

        #region DP - InnerBorderBrush
        /// <summary>
        /// Dependency property for the inner border brush.
        /// </summary>
        public static readonly DependencyProperty InnerBorderBrushProperty =
          DependencyProperty.Register("InnerBorderBrush", typeof(Brush),
          typeof(ModernWindow), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// Gets or sets the inner border brush.
        /// </summary>
        public Brush InnerBorderBrush
        {
            get { return (Brush)this.GetValue(InnerBorderBrushProperty); }
            set { this.SetValue(InnerBorderBrushProperty, value); }
        }
        #endregion

        #region DP - ControlBrush
        /// <summary>
        /// Dependency property for the control brush.
        /// </summary>
        public static readonly DependencyProperty ControlBrushProperty =
          DependencyProperty.Register("ControlBrush", typeof(Brush),
          typeof(ModernWindow), new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// Gets or sets the control brush.
        /// </summary>
        public Brush ControlBrush
        {
            get { return (Brush)this.GetValue(ControlBrushProperty); }
            set { this.SetValue(ControlBrushProperty, value); }
        }
        #endregion

        #region DP - TitleBrush
        /// <summary>
        /// Dependency property for the title brush.
        /// </summary>
        public static readonly DependencyProperty TitleBrushProperty =
          DependencyProperty.Register("TitleBrush", typeof(Brush),
          typeof(ModernWindow), new PropertyMetadata(Brushes.LightSteelBlue));

        /// <summary>
        /// Gets or sets the title brush.
        /// </summary>
        public Brush TitleBrush
        {
            get { return (Brush)this.GetValue(TitleBrushProperty); }
            set { this.SetValue(TitleBrushProperty, value); }
        }
        #endregion

        #region DP - IsMaximized
        /// <summary>
        /// Dependency property for the IsMaximized flag.
        /// </summary>
        public static readonly DependencyProperty IsMaximizedProperty =
          DependencyProperty.Register("IsMaximized", typeof(bool),
          typeof(ModernWindow), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether the window is maximized.
        /// </summary>
        public bool IsMaximized
        {
            get { return (bool)this.GetValue(IsMaximizedProperty); }
            set { this.SetValue(IsMaximizedProperty, value); }
        }
        #endregion

        #region DP - LargeIcon
        /// <summary>
        /// Dependency property for the LargeIcon flag.
        /// </summary>
        public static readonly DependencyProperty LargeIconProperty =
          DependencyProperty.Register("LargeIcon", typeof(bool),
          typeof(ModernWindow), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether the icon shall be shown
        /// in size 32x32 pixels.
        /// </summary>
        public bool LargeIcon
        {
            get { return (bool)this.GetValue(LargeIconProperty); }
            set { this.SetValue(LargeIconProperty, value); }
        }
        #endregion
        #endregion // #region DEPENDENCY PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes static members of the <see cref="ModernWindow"/> class.
        /// </summary>
        static ModernWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ModernWindow),
                new FrameworkPropertyMetadata(typeof(ModernWindow)));
        } // ModernWindow()

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernWindow"/> class.
        /// </summary>
        public ModernWindow()
        {
            this.PreviewMouseMove += this.OnPreviewMouseMove;
            this.Loaded += this.OnLoaded;
            this.Activated += this.OnActivated;
            this.Closing += this.OnClosing;
            this.StateChanged += this.OnStateChanged;
            this.SizeChanged += this.OnSizeChanged;

            // Setting a custom window chrome is required to disallow
            // Windows to change the chrome when the ResizeMode has changed.
            WindowChrome.SetWindowChrome(this, this.invisibleChrome);
        } // ModernWindow()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING
        /// <summary>
        /// An application-defined function that processes messages sent to a window.
        /// We handle here <c>WM_GETMINMAXINFO</c>.
        /// </summary>
        /// <param name="hwnd">Handle to the window.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">Additional message information (<c>wParam</c>).</param>
        /// <param name="lParam">Additional message information (<c>lParam</c>).</param>
        /// <param name="handled">if set to <c>true</c> the message has been handled.</param>
        /// <returns>For this application always 0.</returns>
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam,
              IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case NativeMethods.WmGetMinMaxInfo:
                    NativeMethods.GetMinMaxInfo(hwnd, lParam, 
                        (int)this.MinWidth, (int)this.MinHeight);
                    handled = true;
                    break;
            } // switch

            return (IntPtr)0;
        } // WindowProc()

        /// <summary>
        /// Called when the user has clicked the minimize button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        protected void MinimizeClick(object sender, RoutedEventArgs e)
        {
            if (this.ResizeMode != ResizeMode.NoResize)
            {
                this.WindowState = WindowState.Minimized;
                this.IsMaximized = false;
            } // if
        } // MinimizeClick()

        /// <summary>
        /// Called when the user has clicked the restore or maximize button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        protected void RestoreClick(object sender, RoutedEventArgs e)
        {
            this.ToggleMaximize();
        } // RestoreClick()

        /// <summary>
        /// Called when the user has clicked the close button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        protected void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        } // CloseClick()

        /// <summary>
        /// Handles the MouseMove event of the ResizeRectangle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance
        /// containing the event data.</param>
        protected void ResizeRectangleMouseMove(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if ((rectangle == null) || (this.ResizeMode == ResizeMode.NoResize))
            {
                return;
            } // if

            switch (rectangle.Name)
            {
                case "Top":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "Bottom":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "Left":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "Right":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "TopLeft":
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                case "TopRight":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                case "BottomLeft":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                case "BottomRight":
                    this.Cursor = Cursors.SizeNWSE;
                    break;
            } // switch
        } // ResizeRectangleMouseMove()

        /// <summary>
        /// Preview event for mouse move. Needed to handle resizing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance
        /// containing the event data.</param>
        protected void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.ResizeMode == ResizeMode.NoResize)
            {
                return;
            } // if

            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                this.Cursor = Cursors.Arrow;
            } // if
        } // OnPreviewMouseMove()

        /// <summary>
        /// Handles the PreviewMouseDown event of the ResizeRectangle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance
        /// containing the event data.</param>
        protected void ResizeRectanglePreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if ((rectangle == null) || (this.ResizeMode == ResizeMode.NoResize))
            {
                return;
            } // if
            
            switch (rectangle.Name)
            {
                case "Top":
                    this.Cursor = Cursors.SizeNS;
                    this.ResizeWindow(ResizeDirection.Top);
                    break;
                case "Bottom":
                    this.Cursor = Cursors.SizeNS;
                    this.ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "Left":
                    this.Cursor = Cursors.SizeWE;
                    this.ResizeWindow(ResizeDirection.Left);
                    break;
                case "Right":
                    this.Cursor = Cursors.SizeWE;
                    this.ResizeWindow(ResizeDirection.Right);
                    break;
                case "TopLeft":
                    this.Cursor = Cursors.SizeNWSE;
                    this.ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "TopRight":
                    this.Cursor = Cursors.SizeNESW;
                    this.ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "BottomLeft":
                    this.Cursor = Cursors.SizeNESW;
                    this.ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                case "BottomRight":
                    this.Cursor = Cursors.SizeNWSE;
                    this.ResizeWindow(ResizeDirection.BottomRight);
                    break;
            } // switch
        } // ResizeRectanglePreviewMouseDown()

        /// <summary>
        /// Resizes the window.
        /// </summary>
        /// <param name="direction">The direction.</param>
        private void ResizeWindow(ResizeDirection direction)
        {
            NativeMethods.ResizeWindow(this.hwndSource.Handle, direction);
        } // ResizeWindow()

        /// <summary>
        /// Invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> 
        /// is set to true internally.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" />
        /// that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            this.SourceInitialized += this.OnSourceInitialized;
            base.OnInitialized(e);
        } // OnInitialized()

        /// <summary>
        /// Called when the source has been initialized.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSourceInitialized(object sender, EventArgs e)
        {
            this.hwndSource = (HwndSource)PresentationSource.FromVisual(this);

            if (this.hwndSource != null)
            {
                this.hwndSource.AddHook(this.WindowProc);
            } // if
        } // OnSourceInitialized()

        /// <summary>
        /// Toggles the maximize window state.
        /// </summary>
        private void ToggleMaximize()
        {
            this.WindowState = (this.WindowState == WindowState.Normal)
                ? WindowState.Maximized : WindowState.Normal;

            this.IsMaximized = (this.WindowState == WindowState.Maximized);
        } // ToggleMaximize()

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal 
        /// processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            var minimizeButton = this.GetTemplateChild(PartMinimizeButtonName) as Button;
            if (minimizeButton != null)
            {
                minimizeButton.Click += this.MinimizeClick;
            } // if

            var maximizeButton = this.GetTemplateChild(PartMaximizeButtonName) as Button;
            if (maximizeButton != null)
            {
                maximizeButton.Click += this.RestoreClick;
            } // if

            var restoreButton = this.GetTemplateChild(PartRestoreButtonName) as Button;
            if (restoreButton != null)
            {
                restoreButton.Click += this.RestoreClick;
            } // if

            var closeButton = this.GetTemplateChild(PartCloseButtonName) as Button;
            if (closeButton != null)
            {
                closeButton.Click += this.CloseClick;
            } // if

            var moveRectangle = this.GetTemplateChild(PartMoveRectangleName) as Rectangle;
            if (moveRectangle != null)
            {
                moveRectangle.PreviewMouseDown += this.MoveRectanglePreviewMouseDown;
            } // if

            var titleTextBlock = this.GetTemplateChild(PartTitleTextBlockName) as TextBlock;
            if (titleTextBlock != null)
            {
                titleTextBlock.PreviewMouseDown += this.TitleBlockPreviewMouseDown;
            } // if

            var captionIcon = this.GetTemplateChild(PartCaptionImageName) as Image;
            if (captionIcon != null)
            {
                captionIcon.MouseDown += this.CaptionIconOnMouseDown;
            } // if

#if true
            var resizeGrid = this.GetTemplateChild(PartResizeGridName) as Grid;
            if (resizeGrid != null)
            {
                foreach (UIElement element in resizeGrid.Children)
                {
                    var resizeRectangle = element as Rectangle;
                    if (resizeRectangle != null)
                    {
                        resizeRectangle.PreviewMouseDown += this.ResizeRectanglePreviewMouseDown;
                        resizeRectangle.MouseMove += this.ResizeRectangleMouseMove;
                    } // if
                } // foreach
            } // if
#endif

            base.OnApplyTemplate();
        } // OnApplyTemplate()
        #endregion // UI HANDLING

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Called when the object is getting loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the 
        /// event data.</param>
        protected void OnLoaded(object sender, EventArgs eventArgs)
        {
            var command = this.LoadedCommand;
            if (command != null)
            {
                command.Execute(null);
            } // if
        } // OnLoaded()

        /// <summary>
        /// Called when the object is getting activated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the 
        /// event data.</param>
        private void OnActivated(object sender, EventArgs eventArgs)
        {
            var command = this.ActivatedCommand;
            if (command != null)
            {
                command.Execute(null);
            } // if
        } // OnActivated

        /// <summary>
        /// Called when the window is closing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance
        /// containing the event data.</param>
        private void OnClosing(object sender, CancelEventArgs eventArgs)
        {
            var command = this.ClosingCommand;
            if (command != null)
            {
                command.Execute(eventArgs);
            } // if
        } // OnClosing()

        /// <summary>
        /// Handles the size changed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SizeChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            var command = this.ResizedCommand;
            if (command != null)
            {
                command.Execute(args);
            } // if
        } // OnSizeChanged()

        /// <summary>
        /// Called when the window state has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance 
        /// containing the event data.</param>
        private void OnStateChanged(object sender, EventArgs args)
        {
            var command = this.WindowStateChangeCommand;
            if (command != null)
            {
                command.Execute(this.WindowState);
            } // if
        } // OnStateChanged()

        /// <summary>
        /// Handles a left mouse down event on the move rectangle. This is required
        /// to move the window around.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance
        /// containing the event data.</param>
        private void MoveRectanglePreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            } // if
        } // MoveRectanglePreviewMouseDown()

        /// <summary>
        /// Handles a left mouse down event on the move rectangle. This is required
        /// to move the window around. A double-click on the title bar toggles also
        /// the window maximized state.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance
        /// containing the event data.</param>
        private void TitleBlockPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    this.ToggleMaximize();
                    return;
                } // if
            } // if

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            } // if
        } // TitleBlockPreviewMouseDown()

        /// <summary>
        /// A double-click on the icon closes the dialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/>
        /// instance containing the event data.</param>
        private void CaptionIconOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 1)
                {
                    this.ShowSystemMenu();
                } // if

                if (e.ClickCount == 2)
                {
                    this.Close();
                } // if
            } // if
        } // CaptionIconOnMouseDown()

        /// <summary>
        /// Shows the system menu.
        /// </summary>
        private void ShowSystemMenu()
        {
            NativeMethods.ShowSystemMenu(this, this.WindowState);
            this.IsMaximized = (this.WindowState == WindowState.Maximized);
        } // ShowSystemMenu()
        #endregion // PRIVATE METHODS
    } // ModernWindow
} // Tethys.Silverlight.Controls.WPF
