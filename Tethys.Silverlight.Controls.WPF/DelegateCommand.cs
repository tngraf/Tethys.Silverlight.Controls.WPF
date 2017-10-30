#region Header
// --------------------------------------------------------------------------
// Tethys.Silverlight.Controls.WPF
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ===========================================================================
//
// <copyright file="DelegateCommand.cs" company="Tethys">
// Copyright  2010-2015 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
//
// System ... Microsoft .Net Framework 4.5
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.Silverlight.Controls.WPF
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Concrete implementation for ICommand.<br/>
    /// Based on the following article
    /// <see cref="http://msdn.microsoft.com/de-de/magazine/dd419663.aspx#id0090030" />
    /// </summary>
    /// <remarks>
    /// REMARK 1<br/>
    /// To work properly with commands that are assigned to menu entries or toolbar entries
    /// a modified CanExecuteChange handler (using 
    /// <see cref="CommandManager.RequerySuggested"/>) is required.
    /// <br/>
    /// REMARK 2<br/>
    /// CommandManager does not exist on WinRT! See the following article on StackOverflow:
    /// <a href="http://stackoverflow.com/questions/12030697/what-replaces-commandmanager-in-winrt">link</a>
    /// and especially the answer of Jerry Nixon (MSFT).
    /// </remarks>
    public class DelegateCommand : ICommand
    {
        #region EVENTS
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command
        /// should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        } // CanExecuteChanged
        #endregion // EVENTS

        //// -----------------------------------------------------------------------

        #region PROPERTIES
        /// <summary>
        /// Command execute action.
        /// </summary>
        private readonly Action<object> execute;

        /// <summary>
        /// Command canExecute predicate.
        /// </summary>
        private readonly Predicate<object> canExecute;
        #endregion // PROPERTIES

        //// -----------------------------------------------------------------------

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        } // DelegateCommand()

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        } // DelegateCommand()
        #endregion // CONSTRUCTORS

        //// -----------------------------------------------------------------------

        #region ICOMMAND MEMBERS
        /// <summary>
        /// Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can execute the specified parameter; 
        /// otherwise, <c>false</c>.
        /// </returns>
        //// [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        } // CanExecute()

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        } // Execute()
        #endregion // ICOMMAND MEMBERS
    } // DelegateCommand
} // Tethys.Silverlight.Controls.WPF
