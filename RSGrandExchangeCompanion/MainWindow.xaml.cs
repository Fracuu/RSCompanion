using System.Windows;

namespace RSGrandExchangeCompanion
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(); 
        }
    }
}
