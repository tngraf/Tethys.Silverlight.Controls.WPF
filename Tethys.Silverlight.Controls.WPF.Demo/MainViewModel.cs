namespace Tethys.Silverlight.Controls.WPF.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region PRIVATE PROPERTIES
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

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
        } // MainViewModel
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PROTECTED METHODS
        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property name of the property that has
        /// changed.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            } // if
        } // RaisePropertyChanged()

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The
        /// <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance
        /// containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            } // if
        } // OnPropertyChanged()
        #endregion // PROTECTED METHODS
        
        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Executes the login command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private static void ExecuteLoginCommand(object parameter)
        {
            var passwordExControl = parameter as PasswordBoxEx;
            if (passwordExControl != null)
            {
                // Get Password as Binding not supported for control-type PasswordBox
                var loginPassword = passwordExControl.Password;

                MessageBox.Show("Password = " + loginPassword);
            } // if

            var passwordControl = parameter as PasswordBox;
            if (passwordControl != null)
            {
                // Get Password as Binding not supported for control-type PasswordBox
                var loginPassword = passwordControl.Password;

                MessageBox.Show("Password = " + loginPassword);
            } // if
        } // ExecuteLoginCommand()
        #endregion // PRIVATE METHODS
    } // MainViewModel
} // Tethys.Silverlight.Controls.WPF.Demo
