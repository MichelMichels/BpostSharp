﻿using MichelMichels.BpostSharp.Demo.ViewModels;
using System.Windows;

namespace MichelMichels.BpostSharp.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}