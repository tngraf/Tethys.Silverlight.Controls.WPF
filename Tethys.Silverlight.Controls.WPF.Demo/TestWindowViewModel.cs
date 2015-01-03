namespace Tethys.Silverlight.Controls.WPF.Demo
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    /// <summary>
    /// View model for the test dialog.
    /// </summary>
    public class TestWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// This event is raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the loaded command.
        /// </summary>
        public ICommand LoadedCommand { get; private set; }

        /// <summary>
        /// Gets the activated command.
        /// </summary>
        public ICommand ActivatedCommand { get; private set; }

        /// <summary>
        /// Gets the closing command.
        /// </summary>
        public ICommand ClosingCommand { get; private set; }

        /// <summary>
        /// Gets the resized command.
        /// </summary>
        public ICommand ResizedCommand { get; private set; }

        /// <summary>
        /// Gets the window state change command.
        /// </summary>
        public ICommand WindowStateChangeCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWindowViewModel"/> class.
        /// </summary>
        public TestWindowViewModel()
        {
            this.LoadedCommand = new DelegateCommand(this.ExecuteLoadedCommand);
            this.ActivatedCommand = new DelegateCommand(this.ExecuteActivatedCommand);
            this.ClosingCommand = new DelegateCommand(this.ExecuteClosingCommand);
            this.ResizedCommand = new DelegateCommand(this.ExecuteResizedCommand);
            this.WindowStateChangeCommand = new DelegateCommand(this.ExecuteWindowStateChangeCommand);
        } // TestWindowViewModel()

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property name of the property that has
        /// changed.</param>
        protected virtual void RaisePropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            } // if
        } // RaisePropertyChanged()

        /// <summary>
        /// Executes the loaded command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void ExecuteLoadedCommand(object parameter)
        {
            Debug.WriteLine("Loaded");
        } // ExecuteLoadedCommand()

        /// <summary>
        /// Executes the activated command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void ExecuteActivatedCommand(object parameter)
        {
            Debug.WriteLine("Activated");
        } // ExecuteActivatedCommand()

        /// <summary>
        /// Executes the closing command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void ExecuteClosingCommand(object parameter)
        {
            Debug.WriteLine("Closing");
        } // ExecuteClosingCommand()

        /// <summary>
        /// Executes the resized command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void ExecuteResizedCommand(object parameter)
        {
            Debug.WriteLine("Resized");
        } // ExecuteResizedCommand()

        /// <summary>
        /// Executes the loaded command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void ExecuteWindowStateChangeCommand(object parameter)
        {
            Debug.WriteLine("WindowStateChange");
        } // ExecuteWindowStateChangeCommand
    } // TestWindowViewModel
}
