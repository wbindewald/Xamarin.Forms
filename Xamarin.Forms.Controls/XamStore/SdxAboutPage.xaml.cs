using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Controls
{
	[QueryProperty("Test", "welcome")]
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SdxAboutPage : ContentPage
	{
		public SdxAboutPage()
		{
			InitializeComponent();
		}

		async void HandleToAbout(object sender, EventArgs e)
		{
			var shell = Application.Current.MainPage as Shell;
			await shell.GoToAsync("app:///sdxshell/about?welcome=sdx");
		}

		async void HandleToLeft(object sender, EventArgs e)
		{
			var shell = Application.Current.MainPage as Shell;
			await shell.GoToAsync("http://www.xamarin.com/sdxshell/home/left");
		}

		async void HandleToRight(object sender, EventArgs e)
		{
			var shell = Application.Current.MainPage as Shell;
			await shell.GoToAsync("app:///sdxshell/home/right");
		}

		public string Test {
			get;
			set;
		}
	}
}