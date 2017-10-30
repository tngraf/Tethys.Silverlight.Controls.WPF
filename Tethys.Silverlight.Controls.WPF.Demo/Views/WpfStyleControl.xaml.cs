namespace Tethys.Silverlight.Controls.WPF.Demo.Views
{
    using Tethys.Silverlight.Controls.WPF;

    /// <summary>
    /// Interaction logic for WpfStyleControl.
    /// </summary>
    public partial class WpfStyleControl 
    {
        public WpfStyleControl()
        {
            this.InitializeComponent();

            this.DataContext = ThemeManager.Current;
        }
    }
}
