#region Header
// --------------------------------------------------------------------------
// Tethys.Silverlight.Controls.WPF
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="MessageBoxSymbolConverter.cs" company="Tethys">
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
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Message box symbols used for dialogs.
    /// </summary>
    public enum MessageBoxSymbol
    {
        /// <summary>
        /// No symbol.
        /// </summary>
        None,

        /// <summary>
        /// An information symbol.
        /// </summary>
        Information,

        /// <summary>
        /// A question mark symbol.
        /// </summary>
        Question,

        /// <summary>
        /// An error symbol.
        /// </summary>
        Error,

        /// <summary>
        /// A warning symbol.
        /// </summary>
        Warning
    } // MessageBoxSymbol

    /// <summary>
    /// A converter that translates a MessageBoxSymbol enumeration value first to a 
    /// resource name and then to a Brush resource.
    /// </summary>
    public class MessageBoxSymbolConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            } // if

            var symbol = (MessageBoxSymbol)value;

            var resourceName = string.Empty;
            if (parameter is string)
            {
                resourceName = parameter as string;
            }
            else
            {
                switch (symbol)
                {
                    case MessageBoxSymbol.None:
                        resourceName = string.Empty;
                        break;
                    case MessageBoxSymbol.Information:
                        resourceName = "SymbolInfo";
                        break;
                    case MessageBoxSymbol.Question:
                        resourceName = "SymbolQuestion";
                        break;
                    case MessageBoxSymbol.Error:
                        resourceName = "SymbolError";
                        break;
                    case MessageBoxSymbol.Warning:
                        resourceName = "SymbolWarning";
                        break;
                } // switch
            } // if

            object result = null;
            if (System.Windows.Application.Current.Resources.Contains(resourceName))
            {
                result = System.Windows.Application.Current.Resources[resourceName];
            } // if

            if (symbol == MessageBoxSymbol.None)
            {
                return null;
            } // if

            return result;
        } // Convert()

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        } // ConvertBack()
    } // MessageBoxSymbolConverter
} // Tethys.Silverlight.Controls.WPF
