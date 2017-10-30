#region Header
// --------------------------------------------------------------------------
// Tethys.Silverlight.Controls.WPF
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="StringToVisibilityConverter.cs" company="Tethys">
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
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// A simple string to visibility converter.
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        #region IVALUECONVERTER MEMBERS
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter,
          System.Globalization.CultureInfo culture)
        {
            bool noshow;

            // If the data isn't a string, bail.
            if (value is string == false)
            {
                noshow = true;
            }
            else
            {
                // Cast the data.
                noshow = string.IsNullOrEmpty((string)value);
            } // if

            // If we have the invert string, return the inverted value.
            if (parameter != null && parameter.ToString() == "Invert")
            {
                return noshow ? Visibility.Visible : Visibility.Collapsed;
            } // if

            return noshow ? Visibility.Collapsed : Visibility.Visible;
        } // Convert()

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter,
          System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        } // ConvertBack()
        #endregion // IVALUECONVERTER MEMBERS
    } // StringToVisibilityConverter
} // Tethys.Silverlight.Controls.WPF
