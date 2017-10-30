#region Header
// --------------------------------------------------------------------------
// Tethys.Silverlight.Controls.WPF
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
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ModernMessageDialog.
    /// </summary>
    [TemplatePart(Name = PartButton1Name, Type = typeof(Button))]
    [TemplatePart(Name = PartButton2Name, Type = typeof(Button))]
    public class ModernMessageDialog : ModernWindow
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The part button1 name.
        /// </summary>
        private const string PartButton1Name = "PART_Button1";

        /// <summary>
        /// The part button1 name.
        /// </summary>
        private const string PartButton2Name = "PART_Button2";

        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES

        #region DP-Button1Text
        /// <summary>
        /// Dependency property for the text of the first button.
        /// </summary>
        public static readonly DependencyProperty Button1TextProperty =
          DependencyProperty.Register("Button1Text", typeof(string),
          typeof(ModernMessageDialog), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the button 1 text.
        /// </summary>
        public string Button1Text
        {
            get { return (string)this.GetValue(Button1TextProperty); }
            set { this.SetValue(Button1TextProperty, value); }
        }
        #endregion // DP-Button1Text

        #region DP-Button1Command
        /// <summary>
        /// Dependency property for the command of the first button.
        /// </summary>
        public static readonly DependencyProperty Button1CommandProperty =
          DependencyProperty.Register("Button1Command", typeof(ICommand),
          typeof(ModernMessageDialog), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the button 1 command.
        /// </summary>
        public ICommand Button1Command
        {
            get { return (ICommand)this.GetValue(Button1CommandProperty); }
            set { this.SetValue(Button1CommandProperty, value); }
        }
        #endregion // DP-Button1Command

        #region DP-Button1CommandParameter
        /// <summary>
        /// Dependency property for the parameter for the command of the first button.
        /// </summary>
        public static readonly DependencyProperty Button1CommandParameterProperty =
          DependencyProperty.Register("Button1CommandParameter", typeof(object),
          typeof(ModernMessageDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the button 1 command parameter.
        /// </summary>
        public object Button1CommandParameter
        {
            get { return this.GetValue(Button1CommandParameterProperty); }
            set { this.SetValue(Button1CommandParameterProperty, value); }
        }
        #endregion // DP-Button1CommandParameter

        #region DP-Button2Text
        /// <summary>
        /// Dependency property for the text of the second button.
        /// </summary>
        public static readonly DependencyProperty Button2TextProperty =
          DependencyProperty.Register("Button2Text", typeof(string),
          typeof(ModernMessageDialog), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the button 2 text.
        /// </summary>
        public string Button2Text
        {
            get { return (string)this.GetValue(Button2TextProperty); }
            set { this.SetValue(Button2TextProperty, value); }
        }
        #endregion // DP-Button2Text

        #region DP-Button2Command
        /// <summary>
        /// Dependency property for the command of the second button.
        /// </summary>
        public static readonly DependencyProperty Button2CommandProperty =
          DependencyProperty.Register("Button2Command", typeof(ICommand),
          typeof(ModernMessageDialog), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the button 2 command.
        /// </summary>
        public ICommand Button2Command
        {
            get { return (ICommand)this.GetValue(Button2CommandProperty); }
            set { this.SetValue(Button2CommandProperty, value); }
        }
        #endregion // DP-Button2Command

        #region DP-Button2CommandParameter
        /// <summary>
        /// Dependency property for the parameter for the command of the second button.
        /// </summary>
        public static readonly DependencyProperty Button2CommandParameterProperty =
          DependencyProperty.Register("Button2CommandParameter", typeof(object),
          typeof(ModernMessageDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the button 2 command parameter.
        /// </summary>
        public object Button2CommandParameter
        {
            get { return this.GetValue(Button2CommandParameterProperty); }
            set { this.SetValue(Button2CommandParameterProperty, value); }
        }
        #endregion // DP-Button2CommandParameter

        #region DP-DialogResult
        /// <summary>
        /// Dependency property for the DialogResult.
        /// </summary>
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("DialogResult", typeof(bool?),
                typeof(ModernMessageDialog), new PropertyMetadata(DialogResultChanged));

        /// <summary>
        /// Gets or sets the dialog result value, which is the value that is returned 
        /// from the <see cref="M:System.Windows.Window.ShowDialog" /> method.
        /// </summary>
        public new bool? DialogResult
        {
            get { return (bool?)this.GetValue(DialogResultProperty); }
            set { this.SetValue(DialogResultProperty, value); }
        }

        /// <summary>Sets the dialog result.</summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public static void SetDialogResult(DependencyObject target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        } // SetDialogResult()

        /// <summary>Handles a change of the dialog result.</summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
        } // DialogResultChanged()
        #endregion // DP-DialogResult

        #region DP - BottomBrush
        /// <summary>
        /// Dependency property for the bottom brush.
        /// </summary>
        public static readonly DependencyProperty BottomBrushProperty =
          DependencyProperty.Register("BottomBrush", typeof(Brush),
          typeof(ModernWindow), new PropertyMetadata(Brushes.LightSteelBlue));

        /// <summary>
        /// Gets or sets the bottom brush.
        /// </summary>
        public Brush BottomBrush
        {
            get { return (Brush)this.GetValue(BottomBrushProperty); }
            set { this.SetValue(BottomBrushProperty, value); }
        }
        #endregion

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
            var button1 = this.GetTemplateChild(PartButton1Name) as Button;
            if (button1 != null)
            {
                button1.Click += this.OnButton1Click;
            } // if

            var button2 = this.GetTemplateChild(PartButton2Name) as Button;
            if (button2 != null)
            {
                button2.Click += this.OnButton2Click;
            } // if

            base.OnApplyTemplate();
        } // OnApplyTemplate()

        /// <summary>
        /// Called when the first button has been clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void OnButton1Click(object sender, RoutedEventArgs routedEventArgs)
        {
            if (this.Button1Command != null)
            {
                this.Button1Command.Execute(this.Button1CommandParameter);
            } // if

            base.DialogResult = this.DialogResult;
        } // OnButton1Click()

        /// <summary>
        /// Called when the second button has been clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void OnButton2Click(object sender, RoutedEventArgs routedEventArgs)
        {
            if (this.Button2Command != null)
            {
                this.Button2Command.Execute(null);
            } // if

            base.DialogResult = this.DialogResult;
        } // OnButton2Click()
        #endregion // UI HANDLING

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        #endregion // PRIVATE METHODS
    } // ModernMessageDialog
} // Tethys.Silverlight.Controls.WPF
