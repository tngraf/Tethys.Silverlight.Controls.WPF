#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="MainWindow.xaml.cs" company="Tethys">
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

namespace Tethys.Silverlight.Controls.WPF.Demo.Views
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the OnClick event of the <c>BtnAllWhiteWindow</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnAllWhiteWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new ModernWindow();
            dlg.Title = "White";
            dlg.Background = Brushes.White;
            dlg.Foreground = Brushes.Black;
            dlg.TitleBrush = Brushes.White;
            dlg.ControlBrush = Brushes.Black;
            dlg.BorderBrush = Brushes.Black;
            dlg.BorderThickness = new Thickness(1);
            dlg.ResizeGridBrush = Brushes.Black;
            dlg.InnerBorderBrush = Brushes.Transparent;
            dlg.ResizeMode = ResizeMode.CanResizeWithGrip;
            dlg.MinHeight = 200;
            dlg.MinWidth = 200;
            dlg.Width = 400;
            dlg.Height = 300;

            dlg.ShowDialog();
        }

        /// <summary>
        /// Handles the OnClick event of the <c>BtnAllDarkWindow</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnAllDarkWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new ModernWindow();
            dlg.Title = "Black";
            dlg.Background = Brushes.Black;
            dlg.Foreground = Brushes.White;
            dlg.TitleBrush = Brushes.Black;
            dlg.ControlBrush = Brushes.White;
            dlg.BorderBrush = Brushes.White;
            dlg.ResizeGridBrush = Brushes.White;
            dlg.InnerBorderBrush = Brushes.Transparent;
            dlg.ResizeMode = ResizeMode.CanResizeWithGrip;
            dlg.MinHeight = 200;
            dlg.MinWidth = 200;
            dlg.Width = 400;
            dlg.Height = 300;

            dlg.ShowDialog();
        }

        /// <summary>
        /// Handles the OnClick event of the <c>BtnCustomWindow</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnCustomWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new TestWindow();
            dlg.ShowDialog();
        }

        /// <summary>
        /// Handles the OnClick event of the <c>BtnDefaultWindow</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnDefaultWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new ModernWindow();
            dlg.ShowDialog();
        }

        /// <summary>
        /// Handles the OnClick event of the <c>BtnTestDialog</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnTestDialog_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new TestDialog();
            dlg.ShowDialog();
        }
    }
}
