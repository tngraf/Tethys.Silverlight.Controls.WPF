#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="PieProgressControl.cs" company="Tethys">
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// A progress control in form of a pie (chart).
    /// </summary>
    public class PieProgressControl : Control
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The background ellipse.
        /// </summary>
        private Ellipse pieBackground;

        /// <summary>
        /// The grid.
        /// </summary>
        private Grid grid;

        /// <summary>
        /// The pie path figure.
        /// </summary>
        private PathFigure pathFigure;

        /// <summary>
        /// The line segment.
        /// </summary>
        private LineSegment lineSegment;

        /// <summary>
        /// The arc segment.
        /// </summary>
        private ArcSegment arcSegment;

        /// <summary>
        /// The Maximum dependency property.
        /// </summary>
        private static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), 
            typeof(PieProgressControl),
            new PropertyMetadata(100.0, OnMaximumChanged));

        /// <summary>
        /// The Minimum dependency property.
        /// </summary>
        private static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), 
            typeof(PieProgressControl),
            new PropertyMetadata(0.0, OnMinimumChanged));

        /// <summary>
        /// The Value dependency property.
        /// </summary>
        private static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double),
            typeof(PieProgressControl),
            new PropertyMetadata(0.0, OnValueChanged, OnCoerceValueProperty));

        /// <summary>
        /// The IsIndeterminate dependency property.
        /// </summary>
        private static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register("IsIndeterminate", typeof(bool), 
            typeof(PieProgressControl),
            new PropertyMetadata(false, OnIsIndeterminateChanged));
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        [Description("The Maximum property."), Category("Common Properties")]
        [BindableAttribute(true)]
        public double Maximum
        {
            get { return (double)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        [Description("The Minimum property."), Category("Common Properties")]
        [BindableAttribute(true)]
        public double Minimum
        {
            get { return (double)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Description("The Value property."), Category("Common Properties")]
        [BindableAttribute(true)]
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is indeterminate.
        /// </summary>
        [Description("The IsIndeterminate property."), Category("Common Properties")]
        [BindableAttribute(true)]
        public bool IsIndeterminate
        {
            get { return (bool)this.GetValue(IsIndeterminateProperty); }
            set { this.SetValue(IsIndeterminateProperty, value); }
        }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes static members of the <see cref="PieProgressControl"/> class.
        /// </summary>
        static PieProgressControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PieProgressControl),
                new FrameworkPropertyMetadata(typeof(PieProgressControl)));
        } // PieProgressControl()

        /// <summary>
        /// Initializes a new instance of the <see cref="PieProgressControl"/> class.
        /// </summary>
        public PieProgressControl()
        {
            // set default background
            this.Background = SystemColors.ControlLightBrush;
        } // PieProgressControl()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Invoked whenever application code or internal 
        /// processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            this.pieBackground = GetTemplateChild("PieBackground") as Ellipse;
            this.grid = GetTemplateChild("OuterGrid") as Grid;
            if (this.grid != null)
            {
                this.grid.SizeChanged += this.OnGridSizeChanged;
            } // if

            this.pathFigure = GetTemplateChild("PiePathFigure") as PathFigure;
            this.lineSegment = GetTemplateChild("PieLineSegment") as LineSegment;
            this.arcSegment = GetTemplateChild("PieArcSegment") as ArcSegment;

            this.UpdatePie();
        }
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Updates the pie.
        /// </summary>
        private void UpdatePie()
        {
            var radius = this.GetRadius();

            if (this.pieBackground != null)
            {
                this.pieBackground.Width = 2 * radius;
                this.pieBackground.Height = 2 * radius;
            } // if

            if (this.pathFigure != null)
            {
                this.pathFigure.StartPoint = this.CalculateCenter();

                if (this.lineSegment != null)
                {
                    this.lineSegment.Point = new Point(this.grid.Width / 2,
                        this.pathFigure.StartPoint.Y - radius);
                } // if
            } // if

            if (this.arcSegment != null)
            {
                this.arcSegment.Size = new Size(radius, radius);
                this.arcSegment.Point = this.CalculateArcPoint();
                this.arcSegment.IsLargeArc = this.IsLargeArc();
            } // if
        } // UpdatePie()

        /// <summary>
        /// Determines whether this has to be a large arc.
        /// </summary>
        /// <returns><c>true</c> if this has to be a large arc;
        /// otherwise <c>false</c>.</returns>
        private bool IsLargeArc()
        {
            // Only return true if the value is 50% of the range or greater
            return ((this.Value * 2) >= (this.Maximum - this.Minimum));
        } // IsLargeArc()

        /// <summary>
        /// Calculates the arc point, i.e. how far the arc shall reach.
        /// </summary>
        /// <returns>A <see cref="Point"/>.</returns>
        private Point CalculateArcPoint()
        {
            // Convert the value to one between 0 and 360
            var current = (this.Value / (this.Maximum - this.Minimum)) * 360;

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
            var halfWidth = this.Width / 2;
            var halfHeight = this.Height / 2;
            var radius = this.GetRadius();
            var x = halfWidth + (radius * Math.Cos(current));
            var y = halfHeight + (radius * Math.Sin(current));

            return new Point(x, y);
        } // CalculateArcPoint()

        /// <summary>
        /// Gets the radius.
        /// </summary>
        /// <returns>The radius.</returns>
        private double GetRadius()
        {
            var radius = this.Height / 2;
            if (radius > this.Width / 2)
            {
                radius = this.Width / 2;
            } // if

            return radius;
        } // GetRadius()

        /// <summary>
        /// Calculates the center point.
        /// </summary>
        /// <returns>The center point.</returns>
        private Point CalculateCenter()
        {
            var point = new Point(this.Width / 2, this.Height / 2);
            return point;
        } // CalculateCenter()

        /// <summary>
        /// Called when the grid size has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance
        /// containing the event data.</param>
        private void OnGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdatePie();
        } // OnGridSizeChanged()

        /// <summary>
        /// Called when the maximum property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">The 
        /// <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnMaximumChanged(DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs args)
        {
            var control = dependencyObject as PieProgressControl;
            if (control == null)
            {
                return;
            } // if

            control.UpdatePie();
        } // OnMaximumChanged()

        /// <summary>
        /// Called when the Minimum property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">The 
        /// <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnMinimumChanged(DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs args)
        {
            var control = dependencyObject as PieProgressControl;
            if (control == null)
            {
                return;
            } // if

            control.UpdatePie();
        } // OnMinimumChanged()

        /// <summary>
        /// Called when the Value property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">The 
        /// <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnValueChanged(DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs args)
        {
            var control = dependencyObject as PieProgressControl;
            if (control == null)
            {
                return;
            } // if

            control.UpdatePie();
        } // OnValueChanged()

        /// <summary>
        /// Called when the value property is about to change. We check
        /// here the minimum and maximum border and coerce value to if necessary.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="data">The data.</param>
        /// <returns>A valid value.</returns>
        private static object OnCoerceValueProperty(DependencyObject dependencyObject, 
            object data)
        {
            var control = dependencyObject as PieProgressControl;
            if (control == null)
            {
                return data;
            } // if

            var curValue = (double)data;

            if (curValue < control.Minimum)
            {
                curValue = control.Minimum;
            } // if

            if (curValue > control.Maximum)
            {
                curValue = control.Maximum;
            } // if

            return curValue;
        } // OnCoerceValueProperty()

        /// <summary>
        /// Called when the IsIndeterminate property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">The 
        /// <see cref="DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnIsIndeterminateChanged(DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs args)
        {
        } // OnIsIndeterminateChanged()
        #endregion // PRIVATE METHODS
    } // PieProgressControl
} // Tethys.Silverlight.Controls.WPF
