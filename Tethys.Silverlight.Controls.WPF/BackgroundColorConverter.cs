#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="BackgroundColorConverter.cs" company="Tethys">
// Copyright  2015 by T. Graf (for the modifications)
// Copyright (c) 2009 T.Evelyn (evescode@gmail.com) 
//            All rights reserved.
//            Redistribution and use in source and binary forms, with or 
//            without modification, are permitted provided that the following 
//            conditions are met:
//            1.Redistributions of source code must retain the above copyright
//              notice, this list of conditions and the following disclaimer.
//
//            2.Redistributions in binary form must reproduce the above 
//              copyright notice, this list of conditions and the following 
//              disclaimer in the documentation and/or other materials provided
//              with the distribution.
//
//            THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//            ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//            LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
//            FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
//            COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//            INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
//            BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
//            LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
//            CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
//            LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
//            ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//            POSSIBILITY OF SUCH DAMAGE.
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
    using System.Windows.Media;

    /// <summary>
    /// Converts background color to Gradient effect
    /// </summary>
    public class BackgroundColorConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts a background color to a gradient effect.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the 
        /// <see cref="T:System.Windows.Data.MultiBinding" /> produces.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value.
        /// </returns>
        public object Convert(object[] values, Type targetType,
            object parameter, CultureInfo culture)
        {
            var flag = (bool)values[1];
            var c = (Color)values[0];
            if (!flag)
            {
                return c;
            } // if
            
            var radBrush = new RadialGradientBrush();
            var g1 = new GradientStop();
            g1.Offset = 0.982;
            g1.Color = c;
            var g2 = new GradientStop();
            g2.Color = Color.FromArgb(0xFF, 0xAF, 0xB2, 0xB0);
            radBrush.GradientStops.Add(g1);
            radBrush.GradientStops.Add(g2);
            return radBrush;
        } // Convert()

        /// <summary>
        /// Backwards conversion is NOT supported.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetTypes">The types to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        } // ConvertBack()
    } // BackgroundColorConverter
} // Tethys.Silverlight.Controls.WPFs
