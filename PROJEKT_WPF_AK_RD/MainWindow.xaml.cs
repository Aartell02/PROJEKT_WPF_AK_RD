﻿using System.Windows;
using System.Windows.Input;


namespace PROJEKT_WPF_AK_RD;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void Close_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
}