using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;
using Windows.UI.Xaml.Controls;

namespace WebContentSample
{
#if !__WASM__
    public partial class MarkedControl : UserControl
    {
        private readonly WebView internalWebView;
        public event EventHandler MarkedReady;
        public MarkedControl()
        {
            Content = internalWebView = new WebView();

            internalWebView.NavigationCompleted += NavigationCompleted;
            Loaded += MarkedControl_Loaded;
        }

        private void MarkedControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var html = @"<html>
     <body>
       <div id = ""content"">This is default text</ div>
    </ body>
    </ html>
";
            internalWebView.NavigateToString(html);
        }

        private void NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            MarkedReady?.Invoke(this, EventArgs.Empty);
        }

        public void AddWebAllowedObject(string name, object pObject)
        {
            internalWebView.AddWebAllowedObject(name, pObject);
        }

        public Task<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
        {
            var newArguments = new List<string>();
            foreach (var arg in arguments)
            {
               var outArg = PrepareArgument(arg);
                newArguments.Add(outArg);
            }
            var source = new CancellationTokenSource();
            return internalWebView.InvokeScriptAsync(
#if !WINDOWS_UWP
                source.Token,
#endif
                scriptName, newArguments.ToArray()).AsTask();
        }

        private static Func<string, string> ReplaceLiterals = txt =>
#if WINDOWS_UWP
        txt;
#else
        txt.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"").Replace("\'", "\\\'").Replace("`", "\\`").Replace("^", "\\^");
#endif 

        private string PrepareArgument(string argument)
        {
            if (argument == null) return argument;
            return ReplaceLiterals(argument);
        }
    }
#endif
    }
