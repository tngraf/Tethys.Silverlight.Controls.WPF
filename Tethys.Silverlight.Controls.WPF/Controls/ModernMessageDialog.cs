#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="ModernMessageDialog.cs" company="Tethys">
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
    using System.Windows;

    //[TemplatePart(Name = PartOuterBorderName, Type = typeof(Border))]

    /// <summary>
    /// Interaction logic for ModernMessageDialog.
    /// </summary>
    public class ModernMessageDialog : ModernWindow
    {
        #region PRIVATE PROPERTIES
        // private const string PartOuterBorderName = "PART_OuterBorder";

        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes static members of the <see cref="ModernMessageDialog"/> class.
        /// </summary>
        static ModernMessageDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ModernMessageDialog),
                new FrameworkPropertyMetadata(typeof(ModernMessageDialog)));
        } // ModernMessageDialog()

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernMessageDialog"/> class.
        /// </summary>
        public ModernMessageDialog()
        {
        } // ModernMessageDialog()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal 
        /// processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            // here

            base.OnApplyTemplate();
        } // OnApplyTemplate()

        #endregion // UI HANDLING

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        #endregion // PRIVATE METHODS
    } // ModernMessageDialog
} // Tethys.Silverlight.Controls.WPF
