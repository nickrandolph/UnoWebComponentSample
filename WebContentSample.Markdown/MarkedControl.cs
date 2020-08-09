using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace WebContentSample.Markdown
{
    public class MarkedControl : UserControl
    {
        private readonly WebView internalWebView;
        public MarkedControl()
        {
            Content = internalWebView = new WebView();

            internalWebView.NavigationCompleted += NavigationCompleted;
        }

        private void NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
        }

        public void AddWebAllowedObject(string name, object pObject)
        {
            internalWebView.AddWebAllowedObject(name, pObject);
        }

        public Task<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
        {
            return internalWebView.InvokeScriptAsync(scriptName, arguments).AsTask();
        }
    }
}
