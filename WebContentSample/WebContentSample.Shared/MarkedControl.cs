using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;
using Uno.Foundation;
using Uno.Foundation.Interop;
using Windows.UI.Xaml.Controls;

namespace WebContentSample
{
    public partial class MarkedControl :
#if !__WASM__
UserControl
#else
        Control, IJSObject
#endif
    {
        public event EventHandler MarkedReady;

#if !__WASM__
        private readonly WebView internalWebView;
#else
        private readonly JSObjectHandle _handle;
        JSObjectHandle IJSObject.Handle => _handle;
#endif

        public MarkedControl()
        {
#if !__WASM__
            Content = internalWebView = new WebView();
            internalWebView.NavigationCompleted += NavigationCompleted;
#else
            _handle = JSObjectHandle.Create(this);
#endif
            Loaded += MarkedControl_Loaded;
        }

        private void MarkedControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
#if !__WASM__
            var html = @"<html>
     <body>
       <div id = ""content"">This is default text</ div>
    </ body>
    </ html>
";
            internalWebView.NavigateToString(html);
#else
            MarkedReady?.Invoke(this, EventArgs.Empty);
#endif
        }


#if !__WASM__
        private void NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            MarkedReady?.Invoke(this, EventArgs.Empty);
        }
#endif

        public async Task InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
        {
            var newArguments = new List<string>();
            foreach (var arg in arguments)
            {
                var outArg = ReplaceLiterals(arg);
                newArguments.Add(outArg);
            }


#if !__WASM__
            var source = new CancellationTokenSource();
            return internalWebView.InvokeScriptAsync(
#if !WINDOWS_UWP
                source.Token,
#endif
                scriptName, newArguments.ToArray()).AsTask();
#else
            var script = $"javascript:{scriptName}(\"{newArguments.FirstOrDefault()}\");";
            Console.Error.WriteLine(script);

            try
            {
                var result = WebAssemblyRuntime.InvokeJS(script);
                Console.WriteLine($"Result: {result}");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("FAILED " + e);
            }

#endif
        }

        private static Func<string, string> ReplaceLiterals = txt =>
#if WINDOWS_UWP
        txt;
#else
        txt.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"").Replace("\'", "\\\'").Replace("`", "\\`").Replace("^", "\\^");
#endif

    }
}
