﻿/**
* @file App.xaml.cs
* @brief Contains the App class, which represents the application.
*/

using Microsoft.Maui.Controls;
namespace Image_View_V1._0;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        MainPage = new AppShell();
	}

}
