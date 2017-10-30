#region Header
// --------------------------------------------------------------------------
// Tethys.Silverlight.Controls.WPF
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="ToolbarButtonEx.cs" company="Tethys">
// Copyright  2015 by T. Graf
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

namespace Tethys.Silverlight.Controls.WPF.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// An extended toolbar button. The visual content is not the button
    /// content, but the PathGeometry specific as PathData.
    /// </summary>
    public class ToolbarButtonEx : Button
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// Dependency property for xxx.
        /// </summary>
        public static readonly DependencyProperty PathDataProperty =
          DependencyProperty.Register("PathData", typeof(PathGeometry),
          typeof(ToolbarButtonEx), new PropertyMetadata(default(PathGeometry)));

        /// <summary>
        /// Gets or sets the xxx.
        /// </summary>
        public PathGeometry PathData
        {
            get { return (PathGeometry)this.GetValue(PathDataProperty); }
            set { this.SetValue(PathDataProperty, value); }
        }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes the <see cref="ToolbarButtonEx"/> class.
        /// </summary>
        static ToolbarButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolbarButtonEx),
                new FrameworkPropertyMetadata(typeof(ToolbarButtonEx)));
        } // ToolbarButtonEx()
        #endregion // CONSTRUCTION
    } // ToolbarButtonEx
} // PasswordSafeStyle.Controls
