#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
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

namespace Tethys.Silverlight.Controls.WPF
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Shapes;

    /********************************************************************************
     * Many thanks to Magnus for his great article
     * http://blog.magnusmontin.net/2013/03/16/how-to-create-a-custom-window-in-wpf/
     *******************************************************************************/

    /// <summary>
    /// Interaction logic for ModernWindow.
    /// </summary>
    public class ModernWindow : Window
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Windows resize directions.
        /// </summary>
        private enum ResizeDirection
        {
            /// <summary>
            /// The left side.
            /// </summary>
            Left = 1,

            /// <summary>
            /// The right side.
            /// </summary>
            Right = 2,

            /// <summary>
            /// The top side.
            /// </summary>
            Top = 3,

            /// <summary>
            /// The top left side.
            /// </summary>
            TopLeft = 4,

            /// <summary>
            /// The top right side.
            /// </summary>
            TopRight = 5,

            /// <summary>
            /// The bottom side.
            /// </summary>
            Bottom = 6,

            /// <summary>
            /// The bottom left side.
            /// </summary>
            BottomLeft = 7,

            /// <summary>
            /// The bottom right side.
            /// </summary>
            BottomRight = 8,
        } // ResizeDirection()

        /// <summary>
        /// The window handle.
        /// </summary>
        private HwndSource hwndSource;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Dependency property for the loaded command.
        /// </summary>
        public static readonly DependencyProperty LoadedCommandProperty =
          DependencyProperty.Register("LoadedCommand", typeof(ICommand),
          typeof(ModernWindow), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Dependency property for the activated command.
        /// </summary>
        public static readonly DependencyProperty ActivatedCommandProperty =
          DependencyProperty.Register("ActivatedCommand", typeof(ICommand),
          typeof(ModernWindow), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Dependency property for the closing command.
        /// </summary>
        public static readonly DependencyProperty ClosingCommandProperty =
          DependencyProperty.Register("ClosingCommand", typeof(ICommand),
          typeof(ModernWindow), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Dependency property for the resized command.
        /// </summary>
        public static readonly DependencyProperty ResizedCommandProperty =
          DependencyProperty.Register("ResizedCommand", typeof(ICommand),
          typeof(ModernWindow), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Dependency property for the resized window state change command.
        /// </summary>
        public static readonly DependencyProperty WindowStateChangeCommandProperty =
          DependencyProperty.Register("WindowStateChangeCommand", typeof(ICommand),
          typeof(ModernWindow), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Dependency property for the show minimum button flag.
        /// </summary>
        public static readonly DependencyProperty ShowMinButtonProperty =
          DependencyProperty.Register("ShowMinButton", typeof(bool),
          typeof(ModernWindow), new PropertyMetadata(true));

        /// <summary>
        /// Dependency property for the show close button flag.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty =
          DependencyProperty.Register("ShowCloseButton", typeof(bool),
          typeof(ModernWindow), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the loaded command.
        /// </summary>
        public ICommand LoadedCommand
        {
            get { return (ICommand)this.GetValue(LoadedCommandProperty); }
            set { this.SetValue(LoadedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the activated command.
        /// </summary>
        public ICommand ActivatedCommand
        {
            get { return (ICommand)this.GetValue(ActivatedCommandProperty); }
            set { this.SetValue(ActivatedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the closing command.
        /// </summary>
        public ICommand ClosingCommand
        {
            get { return (ICommand)this.GetValue(ClosingCommandProperty); }
            set { this.SetValue(ClosingCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the resized command.
        /// </summary>
        public ICommand ResizedCommand
        {
            get { return (ICommand)this.GetValue(ResizedCommandProperty); }
            set { this.SetValue(ResizedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the window state change command.
        /// </summary>
        public ICommand WindowStateChangeCommand
        {
            get { return (ICommand)this.GetValue(WindowStateChangeCommandProperty); }
            set { this.SetValue(WindowStateChangeCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the minimum button.
        /// </summary>
        public bool ShowMinButton
        {
            get { return (bool)this.GetValue(ShowMinButtonProperty); }
            set { this.SetValue(ShowMinButtonProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the close button.
        /// </summary>
        public bool ShowCloseButton
        {
            get { return (bool)this.GetValue(ShowCloseButtonProperty); }
            set { this.SetValue(ShowCloseButtonProperty, value); }
        }
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
        } // ModernWindow()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING        
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
                case "top":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "bottom":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "left":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "right":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "topLeft":
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                case "topRight":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                case "bottomLeft":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                case "bottomRight":
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
        /// Sends the message.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">The word parameter.</param>
        /// <param name="lParam">The long parameter.</param>
        /// <returns>The result of the message processing; it depends on the 
        /// message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

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
                case "top":
                    this.Cursor = Cursors.SizeNS;
                    this.ResizeWindow(ResizeDirection.Top);
                    break;
                case "bottom":
                    this.Cursor = Cursors.SizeNS;
                    this.ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "left":
                    this.Cursor = Cursors.SizeWE;
                    this.ResizeWindow(ResizeDirection.Left);
                    break;
                case "right":
                    this.Cursor = Cursors.SizeWE;
                    this.ResizeWindow(ResizeDirection.Right);
                    break;
                case "topLeft":
                    this.Cursor = Cursors.SizeNWSE;
                    this.ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "topRight":
                    this.Cursor = Cursors.SizeNESW;
                    this.ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "bottomLeft":
                    this.Cursor = Cursors.SizeNESW;
                    this.ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                case "bottomRight":
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
            SendMessage(this.hwndSource.Handle, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
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
        } // OnSourceInitialized()

        /// <summary>
        /// Toggles the maximize window state.
        /// </summary>
        private void ToggleMaximize()
        {
            this.WindowState = (this.WindowState == WindowState.Normal)
                ? WindowState.Maximized : WindowState.Normal;
        } // ToggleMaximize()

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal 
        /// processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            var minimizeButton = this.GetTemplateChild("minimizeButton") as Button;
            if (minimizeButton != null)
            {
                minimizeButton.Click += this.MinimizeClick;
            } // if

            var restoreButton = this.GetTemplateChild("restoreButton") as Button;
            if (restoreButton != null)
            {
                restoreButton.Click += this.RestoreClick;
            } // if

            var closeButton = this.GetTemplateChild("closeButton") as Button;
            if (closeButton != null)
            {
                closeButton.Click += this.CloseClick;
            } // if

            var moveRectangle = this.GetTemplateChild("moveRectangle") as Rectangle;
            if (moveRectangle != null)
            {
                moveRectangle.PreviewMouseDown += this.MoveRectanglePreviewMouseDown;
            } // if

            var titleTextBlock = this.GetTemplateChild("TitleTextBlock") as TextBlock;
            if (titleTextBlock != null)
            {
                titleTextBlock.PreviewMouseDown += this.TitleBlockPreviewMouseDown;
            } // if

            var captionIcon = this.GetTemplateChild("CaptionImage") as Image;
            if (captionIcon != null)
            {
                captionIcon.MouseDown += this.CaptionIconOnMouseDown;
            } // if

            var resizeGrid = this.GetTemplateChild("resizeGrid") as Grid;
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

            base.OnApplyTemplate();
        } // OnApplyTemplate()

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
        #endregion // UI HANDLING

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS        
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
                if (e.ClickCount == 2)
                {
                    this.Close();
                } // if
            } // if
        } // CaptionIconOnMouseDown()
        #endregion // PRIVATE METHODS
    } // ModernWindow
} // Tethys.Silverlight.Controls.WPF
