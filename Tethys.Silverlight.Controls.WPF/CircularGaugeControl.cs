#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="CircularGaugeControl.cs" company="Tethys">
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// Represents a Circular Gauge control.
    /// </summary>
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "Pointer", Type = typeof(Path))]
    [TemplatePart(Name = "RangeIndicatorLight", Type = typeof(Ellipse))]
    [TemplatePart(Name = "PointerCap", Type = typeof(Ellipse))]
    public class CircularGaugeControl : Control
    {
        #region Private variables
        /// <summary>
        /// The root grid.
        /// </summary>
        private Grid rootGrid;

        /// <summary>
        /// The range indicator.
        /// </summary>
        private Path rangeIndicator;

        /// <summary>
        /// The pointer.
        /// </summary>
        private Path pointer;

        /// <summary>
        /// The pointer cap.
        /// </summary>
        private Ellipse pointerCap;

        /// <summary>
        /// The light indicator.
        /// </summary>
        private Ellipse lightIndicator;

        /// <summary>
        /// The 'is initial value set' flag.
        /// </summary>
        private bool isInitialValueSet;

        /// <summary>
        /// The arc radius 1.
        /// </summary>
        private double arcradius1;

        /// <summary>
        /// The arc radius 2.
        /// </summary>
        private double arcradius2;

        /// <summary>
        /// The animation speed factor.
        /// </summary>
        private int animatingSpeedFactor = 6;
        #endregion

        //// ---------------------------------------------------------------------

        #region Dependency properties
        /// <summary>
        /// Dependency property to Get/Set the current value 
        /// </summary>
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(double), 
            typeof(CircularGaugeControl),
            new PropertyMetadata(double.MinValue, OnCurrentValuePropertyChanged));

        /// <summary>
        /// Dependency property to Get/Set the Minimum Value 
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Maximum Value 
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Radius of the gauge
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Pointer cap Radius
        /// </summary>
        public static readonly DependencyProperty PointerCapRadiusProperty =
            DependencyProperty.Register("PointerCapRadius", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the pointer length
        /// </summary>
        public static readonly DependencyProperty PointerLengthProperty =
            DependencyProperty.Register("PointerLength", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the scale Radius
        /// </summary>
        public static readonly DependencyProperty ScaleRadiusProperty =
            DependencyProperty.Register("ScaleRadius", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the starting angle of scale
        /// </summary>
        public static readonly DependencyProperty ScaleStartAngleProperty =
            DependencyProperty.Register("ScaleStartAngle", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the sweep angle of scale
        /// </summary>
        public static readonly DependencyProperty ScaleSweepAngleProperty =
            DependencyProperty.Register("ScaleSweepAngle", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the number of major divisions on the scale
        /// </summary>
        public static readonly DependencyProperty MajorDivisionsCountProperty =
            DependencyProperty.Register("MajorDivisionsCount", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the number of minor divisions on the scale
        /// </summary>
        public static readonly DependencyProperty MinorDivisionsCountProperty =
            DependencyProperty.Register("MinorDivisionsCount", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set Optimal Range End Value
        /// </summary>
        public static readonly DependencyProperty OptimalRangeEndValueProperty =
           DependencyProperty.Register("OptimalRangeEndValue", typeof(double), 
           typeof(CircularGaugeControl), 
           new PropertyMetadata(OnOptimalRangeEndValuePropertyChanged));

        /// <summary>
        /// Dependency property to Get/Set Optimal Range Start Value
        /// </summary>
        public static readonly DependencyProperty OptimalRangeStartValueProperty =
           DependencyProperty.Register("OptimalRangeStartValue", typeof(double), 
           typeof(CircularGaugeControl), 
           new PropertyMetadata(OnOptimalRangeStartValuePropertyChanged));

        /// <summary>
        /// Dependency property to Get/Set the image source
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
          DependencyProperty.Register("ImageSource", typeof(ImageSource), 
          typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the image offset
        /// </summary>
        public static readonly DependencyProperty ImageOffsetProperty =
          DependencyProperty.Register("ImageOffset", typeof(double), 
          typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the range indicator light offset
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorLightOffsetProperty =
          DependencyProperty.Register("RangeIndicatorLightOffset", 
          typeof(double), typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the image Size
        /// </summary>
        public static readonly DependencyProperty ImageSizeProperty =
          DependencyProperty.Register("ImageSize", typeof(Size), 
          typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator Radius
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorRadiusProperty =
          DependencyProperty.Register("RangeIndicatorRadius", typeof(double), 
          typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator Thickness
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorThicknessProperty =
         DependencyProperty.Register("RangeIndicatorThickness", typeof(double), 
         typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the scale label Radius
        /// </summary>
        public static readonly DependencyProperty ScaleLabelRadiusProperty =
            DependencyProperty.Register("ScaleLabelRadius", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Scale Label Size
        /// </summary>
        public static readonly DependencyProperty ScaleLabelSizeProperty =
         DependencyProperty.Register("ScaleLabelSize", typeof(Size), 
         typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Scale Label FontSize
        /// </summary>
        public static readonly DependencyProperty ScaleLabelFontSizeProperty =
            DependencyProperty.Register("ScaleLabelFontSize", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Scale Label Foreground
        /// </summary>
        public static readonly DependencyProperty ScaleLabelForegroundProperty =
            DependencyProperty.Register("ScaleLabelForeground", typeof(Color), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Major Tick Size
        /// </summary>
        public static readonly DependencyProperty MajorTickSizeProperty =
          DependencyProperty.Register("MajorTickRectSize", typeof(Size), 
          typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Minor Tick Size
        /// </summary>
        public static readonly DependencyProperty MinorTickSizeProperty =
          DependencyProperty.Register("MinorTickSize", typeof(Size), 
          typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Major Tick Color
        /// </summary>
        public static readonly DependencyProperty MajorTickColorProperty =
           DependencyProperty.Register("MajorTickColor", typeof(Color), 
           typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Minor Tick Color
        /// </summary>
        public static readonly DependencyProperty MinorTickColorProperty =
          DependencyProperty.Register("MinorTickColor", typeof(Color), 
          typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Gauge Background Color
        /// </summary>
        public static readonly DependencyProperty GaugeBackgroundColorProperty =
          DependencyProperty.Register("GaugeBackgroundColor", typeof(Color), 
          typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Pointer Thickness
        /// </summary>
        public static readonly DependencyProperty PointerThicknessProperty =
        DependencyProperty.Register("PointerThickness", typeof(double), 
        typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the an option to reset the pointer on start up to the minimum value
        /// </summary>
        public static readonly DependencyProperty ResetPointerOnStartUpProperty =
        DependencyProperty.Register("ResetPointerOnStartUp", typeof(bool), 
        typeof(CircularGaugeControl), new PropertyMetadata(false, null));

        /// <summary>
        /// Dependency property to Get/Set the Scale Value Precision
        /// </summary>
        public static readonly DependencyProperty ScaleValuePrecisionProperty =
        DependencyProperty.Register("ScaleValuePrecision", typeof(int), typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Below Optimal Range Color
        /// </summary>
        public static readonly DependencyProperty BelowOptimalRangeColorProperty =
            DependencyProperty.Register("BelowOptimalRangeColor", typeof(Color), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Optimal Range Color
        /// </summary>
        public static readonly DependencyProperty OptimalRangeColorProperty =
            DependencyProperty.Register("OptimalRangeColor", typeof(Color), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Above Optimal Range Color
        /// </summary>
        public static readonly DependencyProperty AboveOptimalRangeColorProperty =
            DependencyProperty.Register("AboveOptimalRangeColor", typeof(Color), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Dial Text
        /// </summary>
        public static readonly DependencyProperty DialTextProperty =
            DependencyProperty.Register("DialText", typeof(string), typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Dial Text Color
        /// </summary>
        public static readonly DependencyProperty DialTextColorProperty =
            DependencyProperty.Register("DialTextColor", typeof(Color), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Dial Text Font Size
        /// </summary>
        public static readonly DependencyProperty DialTextFontSizeProperty =
            DependencyProperty.Register("DialTextFontSize", typeof(int), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to get/set the horizontal dial text offset
        /// </summary>
        public static readonly DependencyProperty DialTextOffsetXProperty =
            DependencyProperty.Register("DialTextOffsetX", typeof(double), 
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to get/set the vertical dial text offset
        /// </summary>
        public static readonly DependencyProperty DialTextOffsetYProperty =
            DependencyProperty.Register("DialTextOffsetY", typeof(double),
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator light Radius
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorLightRadiusProperty =
            DependencyProperty.Register("RangeIndicatorLightRadius", typeof(double),
            typeof(CircularGaugeControl));

        /// <summary>
        /// Dependency property to get/set the fitting brush.
        /// </summary>
        public static readonly DependencyProperty FittingBrushProperty =
            DependencyProperty.Register("FittingBrush", typeof(Brush),
            typeof(CircularGaugeControl));

        /// <summary>
        /// Dependency property to get/set the fitting width.
        /// </summary>
        public static readonly DependencyProperty FittingWidthProperty =
            DependencyProperty.Register("FittingWidth", typeof(double),
            typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to get/set the show background flag.
        /// </summary>
        public static readonly DependencyProperty ShowBackgroundProperty =
            DependencyProperty.Register("ShowBackground", typeof(bool),
            typeof(CircularGaugeControl), new PropertyMetadata(true));
        #endregion

        //// ---------------------------------------------------------------------

        #region Wrapper properties
        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public double CurrentValue
        {
            get
            {
                return (double)this.GetValue(CurrentValueProperty);
            }

            set
            {
                this.SetValue(CurrentValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public double MinValue
        {
            get
            {
                return (double)this.GetValue(MinValueProperty);
            }

            set
            {
                this.SetValue(MinValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public double MaxValue
        {
            get
            {
                return (double)this.GetValue(MaxValueProperty);
            }

            set
            {
                this.SetValue(MaxValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        public double Radius
        {
            get
            {
                return (double)this.GetValue(RadiusProperty);
            }

            set
            {
                this.SetValue(RadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the pointer cap radius.
        /// </summary>
        public double PointerCapRadius
        {
            get
            {
                return (double)this.GetValue(PointerCapRadiusProperty);
            }

            set
            {
                this.SetValue(PointerCapRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the pointer length.
        /// </summary>
        public double PointerLength
        {
            get
            {
                return (double)this.GetValue(PointerLengthProperty);
            }

            set
            {
                this.SetValue(PointerLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the pointer thickness.
        /// </summary>
        public double PointerThickness
        {
            get
            {
                return (double)this.GetValue(PointerThicknessProperty);
            }

            set
            {
                this.SetValue(PointerThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the scale radius.
        /// </summary>
        public double ScaleRadius
        {
            get
            {
                return (double)this.GetValue(ScaleRadiusProperty);
            }

            set
            {
                this.SetValue(ScaleRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the scale start angle.
        /// </summary>
        public double ScaleStartAngle
        {
            get
            {
                return (double)this.GetValue(ScaleStartAngleProperty);
            }

            set
            {
                this.SetValue(ScaleStartAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the scale sweep angle.
        /// </summary>
        public double ScaleSweepAngle
        {
            get
            {
                return (double)this.GetValue(ScaleSweepAngleProperty);
            }

            set
            {
                this.SetValue(ScaleSweepAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of major divisions on the scale.
        /// </summary>
        public double MajorDivisionsCount
        {
            get
            {
                return (double)this.GetValue(MajorDivisionsCountProperty);
            }

            set
            {
                this.SetValue(MajorDivisionsCountProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of minor divisions on the scale.
        /// </summary>
        public double MinorDivisionsCount
        {
            get
            {
                return (double)this.GetValue(MinorDivisionsCountProperty);
            }

            set
            {
                this.SetValue(MinorDivisionsCountProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the optimal range end value.
        /// </summary>
        public double OptimalRangeEndValue
        {
            get
            {
                return (double)this.GetValue(OptimalRangeEndValueProperty);
            }

            set
            {
                this.SetValue(OptimalRangeEndValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the optimal range start value.
        /// </summary>
        public double OptimalRangeStartValue
        {
            get
            {
                return (double)this.GetValue(OptimalRangeStartValueProperty);
            }

            set
            {
                this.SetValue(OptimalRangeStartValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the gauge image source.
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return (ImageSource)this.GetValue(ImageSourceProperty);
            }

            set
            {
                this.SetValue(ImageSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the image offset.
        /// </summary>
        public double ImageOffset
        {
            get
            {
                return (double)this.GetValue(ImageOffsetProperty);
            }

            set
            {
                this.SetValue(ImageOffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the range indicator light offset.
        /// </summary>
        public double RangeIndicatorLightOffset
        {
            get
            {
                return (double)this.GetValue(RangeIndicatorLightOffsetProperty);
            }

            set
            {
                this.SetValue(RangeIndicatorLightOffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the image width and height.
        /// </summary>
        public Size ImageSize
        {
            get
            {
                return (Size)this.GetValue(ImageSizeProperty);
            }

            set
            {
                this.SetValue(ImageSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the range indicator radius.
        /// </summary>
        public double RangeIndicatorRadius
        {
            get
            {
                return (double)this.GetValue(RangeIndicatorRadiusProperty);
            }

            set
            {
                this.SetValue(RangeIndicatorRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the range indicator thickness.
        /// </summary>
        public double RangeIndicatorThickness
        {
            get
            {
                return (double)this.GetValue(RangeIndicatorThicknessProperty);
            }

            set
            {
                this.SetValue(RangeIndicatorThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the scale label radius.
        /// </summary>
        public double ScaleLabelRadius
        {
            get
            {
                return (double)this.GetValue(ScaleLabelRadiusProperty);
            }

            set
            {
                this.SetValue(ScaleLabelRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the scale label size.
        /// </summary>
        public Size ScaleLabelSize
        {
            get
            {
                return (Size)this.GetValue(ScaleLabelSizeProperty);
            }

            set
            {
                this.SetValue(ScaleLabelSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the scale label font size.
        /// </summary>
        public double ScaleLabelFontSize
        {
            get
            {
                return (double)this.GetValue(ScaleLabelFontSizeProperty);
            }

            set
            {
                this.SetValue(ScaleLabelFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the scale label foreground.
        /// </summary>
        public Color ScaleLabelForeground
        {
            get
            {
                return (Color)this.GetValue(ScaleLabelForegroundProperty);
            }

            set
            {
                this.SetValue(ScaleLabelForegroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the major tick size.
        /// </summary>
        public Size MajorTickSize
        {
            get
            {
                return (Size)this.GetValue(MajorTickSizeProperty);
            }

            set
            {
                this.SetValue(MajorTickSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the minor tick size.
        /// </summary>
        public Size MinorTickSize
        {
            get
            {
                return (Size)this.GetValue(MinorTickSizeProperty);
            }

            set
            {
                this.SetValue(MinorTickSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the major tick color.
        /// </summary>
        public Color MajorTickColor
        {
            get
            {
                return (Color)this.GetValue(MajorTickColorProperty);
            }

            set
            {
                this.SetValue(MajorTickColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the minor tick color.
        /// </summary>
        public Color MinorTickColor
        {
            get
            {
                return (Color)this.GetValue(MinorTickColorProperty);
            }

            set
            {
                this.SetValue(MinorTickColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the gauge background color.
        /// </summary>
        public Color GaugeBackgroundColor
        {
            get
            {
                return (Color)this.GetValue(GaugeBackgroundColorProperty);
            }

            set
            {
                this.SetValue(GaugeBackgroundColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to reset the pointer to minimum 
        /// on start up; default is true.
        /// </summary>
        public bool ResetPointerOnStartUp
        {
            get
            {
                return (bool)this.GetValue(ResetPointerOnStartUpProperty);
            }

            set
            {
                this.SetValue(ResetPointerOnStartUpProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the scale value precision.
        /// </summary>
        public int ScaleValuePrecision
        {
            get
            {
                return (int)this.GetValue(ScaleValuePrecisionProperty);
            }

            set
            {
                this.SetValue(ScaleValuePrecisionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the below optimal range color.
        /// </summary>
        public Color BelowOptimalRangeColor
        {
            get
            {
                return (Color)this.GetValue(BelowOptimalRangeColorProperty);
            }

            set
            {
                this.SetValue(BelowOptimalRangeColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the optimal range color.
        /// </summary>
        public Color OptimalRangeColor
        {
            get
            {
                return (Color)this.GetValue(OptimalRangeColorProperty);
            }

            set
            {
                this.SetValue(OptimalRangeColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the above optimal range color.
        /// </summary>
        public Color AboveOptimalRangeColor
        {
            get
            {
                return (Color)this.GetValue(AboveOptimalRangeColorProperty);
            }

            set
            {
                this.SetValue(AboveOptimalRangeColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the dial text.
        /// </summary>
        public string DialText
        {
            get
            {
                return (string)this.GetValue(DialTextProperty);
            }

            set
            {
                this.SetValue(DialTextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the dial text color.
        /// </summary>
        public Color DialTextColor
        {
            get
            {
                return (Color)this.GetValue(DialTextColorProperty);
            }

            set
            {
                this.SetValue(DialTextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the dial text font size.
        /// </summary>
        public int DialTextFontSize
        {
            get
            {
                return (int)this.GetValue(DialTextFontSizeProperty);
            }

            set
            {
                this.SetValue(DialTextFontSizeProperty, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the horizontal dial text offset.
        /// </summary>
        public double DialTextOffsetX
        {
            get
            {
                return (double)this.GetValue(DialTextOffsetXProperty);
            }

            set
            {
                this.SetValue(DialTextOffsetXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical dial text offset.
        /// </summary>
        public double DialTextOffsetY
        {
            get
            {
                return (double)this.GetValue(DialTextOffsetYProperty);
            }

            set
            {
                this.SetValue(DialTextOffsetYProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the range indicator light radius.
        /// </summary>
        public double RangeIndicatorLightRadius
        {
            get
            {
                return (double)this.GetValue(RangeIndicatorLightRadiusProperty);
            }

            set
            {
                this.SetValue(RangeIndicatorLightRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the fitting brush.
        /// </summary>
        public Brush FittingBrush
        {
            get
            {
                return (Brush)this.GetValue(FittingBrushProperty);
            }

            set
            {
                this.SetValue(FittingBrushProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the fitting width.
        /// </summary>
        public double FittingWidth
        {
            get
            {
                return (double)this.GetValue(FittingWidthProperty);
            }

            set
            {
                this.SetValue(FittingWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the background
        /// gradient and glass effect.
        /// </summary>
        public bool ShowBackground
        {
            get
            {
                return (bool)this.GetValue(ShowBackgroundProperty);
            }

            set
            {
                this.SetValue(ShowBackgroundProperty, value);
            }
        }
        #endregion

        //// ---------------------------------------------------------------------

        #region Constructor
        /// <summary>
        /// Initializes static members of the <see cref="CircularGaugeControl"/> class.
        /// </summary>
        static CircularGaugeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularGaugeControl),
                new FrameworkPropertyMetadata(typeof(CircularGaugeControl)));
        } // CircularGaugeControl()
        #endregion

        //// ---------------------------------------------------------------------

        #region Methods
        /// <summary>
        /// Called when the CurrentValue property is changed.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnCurrentValuePropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var gauge = d as CircularGaugeControl;
            if (gauge == null)
            {
                return;
            } // if

            gauge.OnCurrentValueChanged(e);
        } // OnCurrentValuePropertyChanged()

        /// <summary>
        /// Called when the OptimalRangeEndValue property is changed.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnOptimalRangeEndValuePropertyChanged(DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            var gauge = d as CircularGaugeControl;
            if (gauge == null)
            {
                return;
            } // if

            if ((double)e.NewValue > gauge.MaxValue)
            {
                gauge.OptimalRangeEndValue = gauge.MaxValue;
            } // if
        } // OnOptimalRangeEndValuePropertyChanged()

        /// <summary>
        /// Called when the OptimalRangeStartValue property is changed.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnOptimalRangeStartValuePropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var gauge = d as CircularGaugeControl;
            if (gauge == null)
            {
                return;
            } // if

            if ((double)e.NewValue < gauge.MinValue)
            {
                gauge.OptimalRangeStartValue = gauge.MinValue;
            } // if
        } // OnOptimalRangeStartValuePropertyChanged()

        /// <summary>
        /// Called when the CurrentValue property is changed.
        /// </summary>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        public virtual void OnCurrentValueChanged(DependencyPropertyChangedEventArgs e)
        {
            // validate and set the new value
            var newValue = (double)e.NewValue;
            var oldValue = (double)e.OldValue;

            if (newValue > this.MaxValue)
            {
                newValue = this.MaxValue;
            }
            else if (newValue < this.MinValue)
            {
                newValue = this.MinValue;
            }

            if (oldValue > this.MaxValue)
            {
                oldValue = this.MaxValue;
            }
            else if (oldValue < this.MinValue)
            {
                oldValue = this.MinValue;
            } // if

            if (this.pointer != null)
            {
                double db1;
                double oldCurrRealworldUnit;
                double newCurrRealworldUnit;
                var realworldunit = (this.ScaleSweepAngle / (this.MaxValue - this.MinValue));
                
                // resetting the old value to min value the very first time.
                if ((oldValue == 0) && !this.isInitialValueSet)
                {
                    oldValue = this.MinValue;
                    this.isInitialValueSet = true;
                } // if

                if (oldValue < 0)
                {
                    db1 = this.MinValue + Math.Abs(oldValue);
                    oldCurrRealworldUnit = Math.Abs(db1 * realworldunit);
                }
                else
                {
                    db1 = Math.Abs(this.MinValue) + oldValue;
                    oldCurrRealworldUnit = db1 * realworldunit;
                } // if

                if (newValue < 0)
                {
                    db1 = this.MinValue + Math.Abs(newValue);
                    newCurrRealworldUnit = Math.Abs(db1 * realworldunit);
                }
                else
                {
                    db1 = Math.Abs(this.MinValue) + newValue;
                    newCurrRealworldUnit = db1 * realworldunit;
                } // if

                var oldcurrentvalueAngle = (this.ScaleStartAngle + oldCurrRealworldUnit);
                var newcurrentvalueAngle = (this.ScaleStartAngle + newCurrRealworldUnit);

                // animate the pointer from the old value to the new value
                this.AnimatePointer(oldcurrentvalueAngle, newcurrentvalueAngle);
            } // if
        } // OnCurrentValueChanged()

        /// <summary>
        /// Animates the pointer to the current value to the new one
        /// </summary>
        /// <param name="oldcurrentvalueAngle">The old current value angle.</param>
        /// <param name="newcurrentvalueAngle">The new current value angle.</param>
        private void AnimatePointer(double oldcurrentvalueAngle, double newcurrentvalueAngle)
        {
            if (this.pointer != null)
            {
                var da = new DoubleAnimation();
                da.From = oldcurrentvalueAngle;
                da.To = newcurrentvalueAngle;

                var animDuration = Math.Abs(oldcurrentvalueAngle - newcurrentvalueAngle) * this.animatingSpeedFactor;
                da.Duration = new Duration(TimeSpan.FromMilliseconds(animDuration));

                var sb = new Storyboard();
                sb.Completed += this.PointerAnimationCompleted;
                sb.Children.Add(da);
                Storyboard.SetTarget(da, this.pointer);
                Storyboard.SetTargetProperty(da, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));

                if (Math.Abs(newcurrentvalueAngle - oldcurrentvalueAngle) > 0.01)
                {
                    sb.Begin();
                } // if
            } // if
        } // AnimatePointer()

        /// <summary>
        /// Move pointer without animating
        /// </summary>
        /// <param name="angleValue">The angle value.</param>
        private void MovePointer(double angleValue)
        {
            if (this.pointer != null)
            {
                var tg = this.pointer.RenderTransform as TransformGroup;
                if (tg == null)
                {
                    return;
                } // if

                var rt = tg.Children[0] as RotateTransform;
                if (rt == null)
                {
                    return;
                } // if

                rt.Angle = angleValue;
            } // if
        } // MovePointer()

        /// <summary>
        /// Switch on the Range indicator light after the pointer completes animating
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private void PointerAnimationCompleted(object sender, EventArgs e)
        {
            if (this.CurrentValue > this.OptimalRangeEndValue)
            {
                this.lightIndicator.Fill = GetRangeIndicatorGradEffect(this.AboveOptimalRangeColor);
            }
            else if (this.CurrentValue <= this.OptimalRangeEndValue && this.CurrentValue >= this.OptimalRangeStartValue)
            {
                this.lightIndicator.Fill = GetRangeIndicatorGradEffect(this.OptimalRangeColor);
            }
            else if (this.CurrentValue < this.OptimalRangeStartValue)
            {
                this.lightIndicator.Fill = GetRangeIndicatorGradEffect(this.BelowOptimalRangeColor);
            } // if
        } // PointerAnimationCompleted()

        /// <summary>
        /// Get gradient brush effect for the range indicator light
        /// </summary>
        /// <param name="gradientColor">The color for the gradient.</param>
        /// <returns>
        /// A <see cref="GradientBrush" />.
        /// </returns>
        private static GradientBrush GetRangeIndicatorGradEffect(Color gradientColor)
        {
            var gradient = new LinearGradientBrush();
            gradient.StartPoint = new Point(0, 0);
            gradient.EndPoint = new Point(1, 1);
            var color1 = new GradientStop();
            if (gradientColor == Colors.Transparent)
            {
                color1.Color = gradientColor;
            }
            else
            {
                color1.Color = Colors.LightGray;
            } // if

            color1.Offset = 0.2;
            gradient.GradientStops.Add(color1);
            var color2 = new GradientStop();
            color2.Color = gradientColor; 
            color2.Offset = 0.5;
            gradient.GradientStops.Add(color2);
            var color3 = new GradientStop();
            color3.Color = gradientColor; 
            color3.Offset = 0.8;
            gradient.GradientStops.Add(color3);
            return gradient;
        } // GetRangeIndicatorGradEffect()

        /// <summary>
        /// Invoked whenever application code or internal 
        /// processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            // get reference to known elements on the control template
            this.rootGrid = this.GetTemplateChild("LayoutRoot") as Grid;
            this.pointer = this.GetTemplateChild("Pointer") as Path;
            this.pointerCap = this.GetTemplateChild("PointerCap") as Ellipse;
            this.lightIndicator = this.GetTemplateChild("RangeIndicatorLight") as Ellipse;

            // draw scale and range indicator
            this.DrawScale();
            this.DrawRangeIndicator();

            // set Zindex of pointer and pointer cap to a really high number 
            // so that it stays on top of the scale and the range indicator
            Panel.SetZIndex(this.pointer, 100000);
            Panel.SetZIndex(this.pointerCap, 100001);

            if (this.ResetPointerOnStartUp)
            {
                // reset Pointer
                this.MovePointer(this.ScaleStartAngle);
            } // if
        } // OnApplyTemplate()

        /// <summary>
        /// Drawing the scale with the Scale Radius.
        /// </summary>
        private void DrawScale()
        {
            // calculate one major tick angle 
            var majorTickUnitAngle = this.ScaleSweepAngle / this.MajorDivisionsCount;

            // obtaining One minor tick angle 
            var minorTickUnitAngle = this.ScaleSweepAngle / this.MinorDivisionsCount;

            // obtaining One major ticks value
            var majorTicksUnitValue = (this.MaxValue - this.MinValue) / this.MajorDivisionsCount;
            majorTicksUnitValue = Math.Round(majorTicksUnitValue, this.ScaleValuePrecision);

            var minvalue = this.MinValue;

            // drawing Major scale ticks
            for (var i = this.ScaleStartAngle; i <= (this.ScaleStartAngle + this.ScaleSweepAngle); 
                i = i + majorTickUnitAngle)
            {
                // majortick is drawn as a rectangle 
                var majortickrect = new Rectangle();
                majortickrect.Height = this.MajorTickSize.Height;
                majortickrect.Width = this.MajorTickSize.Width;
                majortickrect.Fill = new SolidColorBrush(this.MajorTickColor);
                var p = new Point(0.5, 0.5);
                majortickrect.RenderTransformOrigin = p;
                majortickrect.HorizontalAlignment = HorizontalAlignment.Center;
                majortickrect.VerticalAlignment = VerticalAlignment.Center;

                var majortickgp = new TransformGroup();
                var majortickrt = new RotateTransform();

                // obtaining the angle in radians for calulating the points
                var asRadian = (i * Math.PI) / 180;
                majortickrt.Angle = i;
                majortickgp.Children.Add(majortickrt);
                var majorticktt = new TranslateTransform();

                // finding the point on the Scale where the major ticks are drawn
                // here drawing the points with center as (0,0)
                majorticktt.X = (int)((this.ScaleRadius) * Math.Cos(asRadian));
                majorticktt.Y = (int)((this.ScaleRadius) * Math.Sin(asRadian));

                // points for the textblock which hold the scale value
                var majorscalevaluett = new TranslateTransform();

                // here drawing the points with center as (0,0)
                majorscalevaluett.X = (int)((this.ScaleLabelRadius) * Math.Cos(asRadian));
                majorscalevaluett.Y = (int)((this.ScaleLabelRadius) * Math.Sin(asRadian));

                // defining the properties of the scale value textbox
                var tb = new TextBlock();

                tb.Height = this.ScaleLabelSize.Height;
                tb.Width = this.ScaleLabelSize.Width;
                tb.FontSize = this.ScaleLabelFontSize;
                tb.Foreground = new SolidColorBrush(this.ScaleLabelForeground);
                tb.TextAlignment = TextAlignment.Center;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.HorizontalAlignment = HorizontalAlignment.Center;

                // writing and appending the scale value

                // checking minvalue < maxvalue w.r.t scale precion value
                if (Math.Round(minvalue, this.ScaleValuePrecision) <= Math.Round(this.MaxValue, this.ScaleValuePrecision))
                {
                    minvalue = Math.Round(minvalue, this.ScaleValuePrecision);
                    tb.Text = minvalue.ToString(CultureInfo.InvariantCulture);
                    minvalue = minvalue + majorTicksUnitValue;
                }
                else
                {
                    break;
                } // if

                majortickgp.Children.Add(majorticktt);
                majortickrect.RenderTransform = majortickgp;
                tb.RenderTransform = majorscalevaluett;
                this.rootGrid.Children.Add(majortickrect);
                this.rootGrid.Children.Add(tb);

                // drawing the minor axis ticks
                var onedegree = ((i + majorTickUnitAngle) - i) / (this.MinorDivisionsCount);

                if ((i < (this.ScaleStartAngle + this.ScaleSweepAngle)) 
                    && (Math.Round(minvalue, this.ScaleValuePrecision) 
                    <= Math.Round(this.MaxValue, this.ScaleValuePrecision)))
                {
                    // drawing the minor scale
                    for (var mi = i + onedegree; mi < (i + majorTickUnitAngle); mi = mi + onedegree)
                    {
                        // here the minortick is drawn as a rectangle 
                        var mr = new Rectangle();
                        mr.Height = this.MinorTickSize.Height;
                        mr.Width = this.MinorTickSize.Width;
                        mr.Fill = new SolidColorBrush(this.MinorTickColor);
                        mr.HorizontalAlignment = HorizontalAlignment.Center;
                        mr.VerticalAlignment = VerticalAlignment.Center;
                        var p1 = new Point(0.5, 0.5);
                        mr.RenderTransformOrigin = p1;

                        var minortickgp = new TransformGroup();
                        var minortickrt = new RotateTransform();
                        minortickrt.Angle = mi;
                        minortickgp.Children.Add(minortickrt);
                        var minorticktt = new TranslateTransform();

                        // obtaining the angle in radians for calulating the points
                        var radian = (mi * Math.PI) / 180;
                        
                        // finding the point on the Scale where the minor ticks are drawn
                        minorticktt.X = (int)((this.ScaleRadius) * Math.Cos(radian));
                        minorticktt.Y = (int)((this.ScaleRadius) * Math.Sin(radian));

                        minortickgp.Children.Add(minorticktt);
                        mr.RenderTransform = minortickgp;
                        this.rootGrid.Children.Add(mr);
                    } // for
                } // if
            } // for
        } // DrawScale()

        /// <summary>
        /// Obtaining the Point (x,y) in the circumference.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>A <see cref="Point"/> on the circumference.</returns>
        private Point GetCircumferencePoint(double angle, double radius)
        {
            var angleRadian = (angle * Math.PI) / 180;
            
            // radius is the Radius of the gauge
            var x = (this.Radius) + (radius * Math.Cos(angleRadian));
            var y = (this.Radius) + (radius * Math.Sin(angleRadian));
            var p = new Point(x, y);
            return p;
        } // GetCircumferencePoint()

        /// <summary>
        /// Draw the range indicator
        /// </summary>
        private void DrawRangeIndicator()
        {
            var realworldunit = (this.ScaleSweepAngle / (this.MaxValue - this.MinValue));
            double optimalStartAngle;
            double optimalEndAngle;
            double db;

            // checking whether the  OptimalRangeStartvalue is -ve 
            if (this.OptimalRangeStartValue < 0)
            {
                db = this.MinValue + Math.Abs(this.OptimalRangeStartValue);
                optimalStartAngle = Math.Abs(db * realworldunit);
            }
            else
            {
                db = Math.Abs(this.MinValue) + this.OptimalRangeStartValue;
                optimalStartAngle = db * realworldunit;
            } // if

            // checking whether the  OptimalRangeEndvalue is -ve
            if (this.OptimalRangeEndValue < 0)
            {
                db = this.MinValue + Math.Abs(this.OptimalRangeEndValue);
                optimalEndAngle = Math.Abs(db * realworldunit);
            }
            else
            {
                db = Math.Abs(this.MinValue) + this.OptimalRangeEndValue;
                optimalEndAngle = db * realworldunit;
            } // if

            // calculating the angle for optimal Start value
            var optimalStartAngleFromStart = (this.ScaleStartAngle + optimalStartAngle);

            // calculating the angle for optimal Start value
            var optimalEndAngleFromStart = (this.ScaleStartAngle + optimalEndAngle);

            // calculating the Radius of the two arc for segment 
            this.arcradius1 = (this.RangeIndicatorRadius + this.RangeIndicatorThickness);
            this.arcradius2 = this.RangeIndicatorRadius;

            var endAngle = this.ScaleStartAngle + this.ScaleSweepAngle;

            // calculating the Points for the below Optimal Range segment from the center of the gauge
            var a = this.GetCircumferencePoint(this.ScaleStartAngle, this.arcradius1);
            var b = this.GetCircumferencePoint(this.ScaleStartAngle, this.arcradius2);
            var c = this.GetCircumferencePoint(optimalStartAngleFromStart, this.arcradius2);
            var d = this.GetCircumferencePoint(optimalStartAngleFromStart, this.arcradius1);

            var isReflexAngle = Math.Abs(optimalStartAngleFromStart - this.ScaleStartAngle) > 180.0;
            this.DrawSegment(a, b, c, d, isReflexAngle, this.BelowOptimalRangeColor);

            // calculating the Points for the Optimal Range segment from the center of the gauge
            var a1 = this.GetCircumferencePoint(optimalStartAngleFromStart, this.arcradius1);
            var b1 = this.GetCircumferencePoint(optimalStartAngleFromStart, this.arcradius2);
            var c1 = this.GetCircumferencePoint(optimalEndAngleFromStart, this.arcradius2);
            var d1 = this.GetCircumferencePoint(optimalEndAngleFromStart, this.arcradius1);
            var isReflexAngle1 = Math.Abs(optimalEndAngleFromStart - optimalStartAngleFromStart) > 180.0;
            this.DrawSegment(a1, b1, c1, d1, isReflexAngle1, this.OptimalRangeColor);

            // calculating the Points for the Above Optimal Range segment from the center of the gauge
            var a2 = this.GetCircumferencePoint(optimalEndAngleFromStart, this.arcradius1);
            var b2 = this.GetCircumferencePoint(optimalEndAngleFromStart, this.arcradius2);
            var c2 = this.GetCircumferencePoint(endAngle, this.arcradius2);
            var d2 = this.GetCircumferencePoint(endAngle, this.arcradius1);
            var isReflexAngle2 = Math.Abs(endAngle - optimalEndAngleFromStart) > 180.0;
            this.DrawSegment(a2, b2, c2, d2, isReflexAngle2, this.AboveOptimalRangeColor);
        } // DrawRangeIndicator()

        /// <summary>
        /// Drawing the segment with two arc and two line.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="reflexangle">if set to <c>true</c> this ought to be 
        /// a large arc (more than 180°).</param>
        /// <param name="clr">The color.</param>
        private void DrawSegment(Point p1, Point p2, Point p3, Point p4, bool reflexangle, Color clr)
        {
            // segment Geometry
            var segments = new PathSegmentCollection();

            // first line segment from pt p1 - pt p2
            segments.Add(new LineSegment { Point = p2 });

            // arc drawn from pt p2 - pt p3 with the RangeIndicatorRadius 
            segments.Add(new ArcSegment
            {
                Size = new Size(this.arcradius2, this.arcradius2),
                Point = p3,
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = reflexangle
            });

            // second line segment from pt p3 - pt p4
            segments.Add(new LineSegment { Point = p4 });

            // arc drawn from pt p4 - pt p1 with the Radius of arcradius1 
            segments.Add(new ArcSegment 
            {
                Size = new Size(this.arcradius1, this.arcradius1),
                Point = p1,
                SweepDirection = SweepDirection.Counterclockwise,
                IsLargeArc = reflexangle
            });

            // defining the segment path properties
            Color rangestrokecolor;
            if (clr == Colors.Transparent)
            {
                rangestrokecolor = clr;
            }
            else
            {
                rangestrokecolor = Colors.White;
            } // if

            this.rangeIndicator = new Path 
            {
                StrokeLineJoin = PenLineJoin.Round,
                Stroke = new SolidColorBrush(rangestrokecolor),
                Fill = new SolidColorBrush(clr),
                Opacity = 0.65,
                StrokeThickness = 0.25,
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection 
                    {
                        new PathFigure 
                        {
                            IsClosed = true,
                            StartPoint = p1,
                            Segments = segments
                        }
                    }
                }
            };

            // set Z index of range indicator
            this.rangeIndicator.SetValue(Panel.ZIndexProperty, 150);

            // adding the segment to the root grid 
            this.rootGrid.Children.Add(this.rangeIndicator);
        } // DrawSegment()
        #endregion
    } // CircularGaugeControl
}
