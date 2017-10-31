#region Header
// --------------------------------------------------------------------------
// Tethys.Silverlight.Controls.WPF
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="MainViewModel.cs" company="Tethys">
// Copyright  2014-2017 by T. Graf
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

namespace Tethys.Silverlight.Controls.WPF.Demo.ViewModel
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Support;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// This event is raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the login command.
        /// </summary>
        public ICommand LoginCommand { get; private set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.LoginCommand = new DelegateCommand(ExecuteLoginCommand);
        } // MainViewModel()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PROTECTED METHODS
        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property name of the property that has
        /// changed.</param>
        protected virtual void RaisePropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            } // if
        } // RaisePropertyChanged()
        #endregion // PROTECTED METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Executes the loaded command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private static void ExecuteLoginCommand(object parameter)
        {
            var pwdbox = parameter as PasswordBox;
            if (pwdbox != null)
            {
                MessageBox.Show("Password = " + pwdbox.Password);
                return;
            } // if

            var pwdboxex = parameter as PasswordBoxEx;
            if (pwdboxex != null)
            {
                MessageBox.Show("Password = " + pwdboxex.Password);
            } // if
        } // ExecuteLoginCommand()
        #endregion // PRIVATE METHODS
    } // MainViewModel
}
