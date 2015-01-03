#region Header
// ---------------------------------------------------------------------------
// Tethys.Silverlight
// ===========================================================================
//
// This library contains common code of WPF, Silverlight, Windows Phone and
// Windows 8 projects of Thomas Graf.
//
// ===========================================================================
// <copyright file="DelegateCommand.cs" company="Thomas Graf">
// Copyright  2012 - 2013 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Microsoft .Net Framework 4.5
// Tools .... Microsoft Visual Studio 2012
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.Silverlight.Controls.WPF.Demo
{
    using System;
    using System.Windows.Input;

    /// <summary>
  /// Concrete implementation for ICommand.<br/>
  /// Based on the following article
  /// <see cref="http://msdn.microsoft.com/de-de/magazine/dd419663.aspx#id0090030" />
  /// </summary>
  public class DelegateCommand : ICommand
  {
    #region EVENTS
    /// <summary>
    /// CanExecuteChanged event handler for this command.
    /// </summary>
    public event EventHandler CanExecuteChanged;
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
    public DelegateCommand(Action<object> execute) : this(execute, null)
    {
    } // Command()

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
    /// </summary>
    /// <param name="execute">The execute.</param>
    /// <param name="canExecute">The can execute.</param>
    public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
    {
      this.execute = execute;
      this.canExecute = canExecute;
    } // Command()
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
    /*public*/ bool ICommand.CanExecute(object parameter)
    {
      return this.canExecute == null ? true : this.canExecute(parameter);
    } // CanExecute()

    /// <summary>
    /// Raises the can execute changed.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
      if (this.CanExecuteChanged != null)
      {
        this.CanExecuteChanged(this, EventArgs.Empty);
      } // if
    } // RaiseCanExecuteChanged()

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    public void Execute(object parameter)
    {
      this.execute(parameter);
    } // Execute()
    #endregion // ICOMMAND MEMBERS
  } // Command
} // WpfDialogDemo.UI.ViewModel
