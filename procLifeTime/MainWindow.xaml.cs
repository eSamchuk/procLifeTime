using System;
using System.Windows;


namespace procLifeTime
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vModel;


        public MainWindow()
        {
            InitializeComponent();
            vModel = new ViewModel();
            this.DataContext = vModel;
        }

        private void btAbort_Click(object sender, RoutedEventArgs e)
        {
            vModel.stop();
           
        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            vModel.clear();
        }
    }
}
