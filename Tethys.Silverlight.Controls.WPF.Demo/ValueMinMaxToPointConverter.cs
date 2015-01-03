namespace Tethys.Silverlight.Controls.WPF.Demo
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// A converter for drawing arcs. For details 
    /// <see cref="ValueMinMaxToPointConverter.Convert"/>.
    /// </summary>
    public class ValueMinMaxToPointConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. 
        /// source values are a value, a minimum and a maximum with the condition
        /// minimum &lt; value &lt; maximum. The converter calculates the point of the
        /// arc the represents the relative value.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the
        /// <see cref="T:System.Windows.Data.MultiBinding" /> produces.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// The converted value, a <see cref="Point"/>.
        /// If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter,
           CultureInfo culture)
        {
            var value = (double)values[0];
            var minimum = (double)values[1];
            var maximum = (double)values[2];

            // Convert the value to one between 0 and 360
            var current = (value / (maximum - minimum)) * 360;
            
            // Adjust the finished state so the ArcSegment gets drawn as a whole circle
            if (Math.Abs(current - 360) < 0.001)
            {
                current = 359.999;
            } // if
            
            // Shift by 90 degrees so 0 starts at the top of the circle
            current = current - 90;
            
            // Convert the angle to radians
            current = current * 0.017453292519943295;
            
            // Calculate the circle's point
            var x = 10 + (10 * Math.Cos(current));
            var y = 10 + (10 * Math.Sin(current));
            
            return new Point(x, y);
        } // Convert()

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array 
        /// length indicates the number and types of values that are suggested for
        /// the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// An array of values that have been converted from the target value
        /// back to the source values.
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        } // ConvertBack()
    } // ValueMinMaxToPointConverter
}