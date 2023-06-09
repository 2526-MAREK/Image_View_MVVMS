﻿/**
* @file Program.cs
* @brief Contains the Program class, which represents the PLTE chunk.
*/

using ObjCRuntime;
using UIKit;

namespace Image_View_V1._0;

public class Program
{
	// This is the main entry point of the application.
	static void Main(string[] args)
	{
		// if you want to use a different Application Delegate class from "AppDelegate"
		// you can specify it here.
		UIApplication.Main(args, null, typeof(AppDelegate));
	}
}
