#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="PasswordBoxEx.cs" company="Tethys">
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
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for PasswordBoxEx.
    /// </summary>
    public class PasswordBoxEx : Control
    {
        #region PRIVATE PROPERTIES        
        /// <summary>
        /// The password box control.
        /// </summary>
        private PasswordBox passwordBox;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the Password property on the PasswordBox control. 
        /// This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
              DependencyProperty.Register("Password", typeof(string),
              typeof(PasswordBoxEx), new FrameworkPropertyMetadata(
                  string.Empty, null));

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get
            {
                return (string)this.GetValue(PasswordProperty);
            }

            set
            {
                this.SetValue(PasswordProperty, value);
            }
        }

        /// <summary>
        /// Gets the secure password.
        /// </summary>
        public SecureString SecurePassword
        {
            get
            {
                if (this.passwordBox == null)
                {
                    return null;
                } // if

                return this.passwordBox.SecurePassword;
            }
        }

        /// <summary>
        /// The password changed routed event.
        /// </summary>
        public static readonly RoutedEvent PasswordChangedEvent
            = EventManager.RegisterRoutedEvent("PasswordChanged", 
            RoutingStrategy.Bubble, typeof(RoutedEventHandler),
            typeof(PasswordBoxEx));

        /// <summary>
        /// Occurs when the value of the Password property changes. 
        /// </summary>
        public event RoutedEventHandler PasswordChanged
        {
            add { this.AddHandler(PasswordChangedEvent, value); }
            remove { this.RemoveHandler(PasswordChangedEvent, value); }
        }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes static members of the <see cref="PasswordBoxEx"/> class.
        /// </summary>
        static PasswordBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(PasswordBoxEx),
                new FrameworkPropertyMetadata(typeof(PasswordBoxEx)));
        } // PasswordBoxEx()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal 
        /// processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            this.passwordBox = this.GetTemplateChild("PART_PasswordBox") as PasswordBox;
            if (this.passwordBox != null)
            {
                this.passwordBox.PasswordChanged += this.OnPasswordChanged;
            } // if

            base.OnApplyTemplate();
        } // OnApplyTemplate()

        /// <summary>
        /// Invoked whenever a <see cref="E:System.Windows.UIElement.GotFocus" /> event 
        /// reaches this element in its route.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" />
        /// that contains the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.passwordBox?.Focus();

            base.OnGotFocus(e);
        } // OnGotFocus()

        /// <summary>
        /// Invoked when a MouseEnter event is raised on this element.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" />
        /// that contains the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "MouseOver", true);
            base.OnMouseEnter(e);
        } // OnMouseEnter()

        /// <summary>
        /// Invoked when a MouseLeave event is raised on this element.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" />
        /// that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
            base.OnMouseLeave(e);
        } // OnMouseLeave()

        #endregion // UI HANDLING

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Handle the 'PasswordChanged'-event on the PasswordBox
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var currentPasswordBox = sender as PasswordBox;

            if (currentPasswordBox == null)
            {
                return;
            } // if

            this.Password = currentPasswordBox.Password;
            this.RaisePasswordChangedEvent();
        } // OnPasswordChanged()

        /// <summary>
        /// Raises the password changed event.
        /// </summary>
        private void RaisePasswordChangedEvent()
        {
            var newEventArgs = new RoutedEventArgs(PasswordChangedEvent);
            this.RaiseEvent(newEventArgs);
        } // RaisePasswordChangedEvent()
        #endregion // PRIVATE METHODS
    } // PasswordBoxEx
} // Tethys.Silverlight.Controls.WPF
