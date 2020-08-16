using System;
using Windows.UI.Xaml.Controls;

namespace WebContentSample
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            markedCtrl.MarkedReady += markedCtrl_MarkedReady;
        }

        private async void markedCtrl_MarkedReady(object sender, EventArgs args)
        {
            //await markedCtrl.LoadMarkdownFromFile("SharedAssets.md");
        }
    }
}
