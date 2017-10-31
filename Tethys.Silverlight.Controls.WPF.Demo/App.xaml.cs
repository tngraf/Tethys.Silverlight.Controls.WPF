#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="App.xaml.cs" company="Tethys">
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

namespace Tethys.Silverlight.Controls.WPF.Demo
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Called at startup of the application.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> 
        /// that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ////ThemeManager.Current.SetLightTheme();
            ThemeManager.Current.SetDarkTheme();
        } // OnStartup()
    } // App
} // Tethys.Silverlight.Controls.WPF.Demo
