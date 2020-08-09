using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;
#if !WINDOWS_UWP
using Uno.Foundation;
#endif
#if __WASM__
using Uno.Foundation.Interop;
#endif
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WebContentSample
{
    public abstract partial class JavaScriptControl :
#if !__WASM__
UserControl
#else
        Control, IJSObject
#endif
    {

#if !__WASM__
        private readonly WebView internalWebView;
#else
        private readonly JSObjectHandle _handle;
        JSObjectHandle IJSObject.Handle => _handle;
#endif

        public JavaScriptControl()
        {
#if !__WASM__
            Content = internalWebView = new WebView();
            internalWebView.NavigationCompleted += NavigationCompleted;
#else
            _handle = JSObjectHandle.Create(this);
#endif
            Loaded += JavaScriptControl_Loaded;
        }

        private string HtmlContentId =>
#if __WASM__
            this.GetHtmlId();
#else
            "content";
#endif

        private void JavaScriptControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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
            LoadJavaScript();
#endif
        }

        protected async Task LoadEmbeddedJavaScriptFile(string filename)
        {
            var markdownScript = (await GetEmbeddedFileStreamAsync(GetType(), filename)).ReadToEnd();

            await InvokeScriptAsync(markdownScript);
        }

        protected abstract Task LoadJavaScript();

#if !__WASM__
        private void NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            LoadJavaScript();
        }
#endif

        protected async Task UpdateHtmlFromScript(string contentScript)
        {
            var script = $@"document.getElementById('{HtmlContentId}').innerHTML = {contentScript};";
            await InvokeScriptAsync(script);
        }

        public async Task InvokeScriptAsync(string scriptToRun)
        {
                scriptToRun = ReplaceLiterals(scriptToRun);

#if !__WASM__
            var source = new CancellationTokenSource();
            await internalWebView.InvokeScriptAsync(
#if !WINDOWS_UWP
                source.Token,
#endif
                "eval", new []{scriptToRun}).AsTask();
#else
            var script = $"javascript:eval(\"{scriptToRun}\");";
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


        public static async Task<Stream> GetEmbeddedFileStreamAsync(Type assemblyType, string fileName)
        {
            await Task.Yield();

            var manifestName = assemblyType.GetTypeInfo().Assembly
                .GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(fileName.Replace(" ", "_").Replace("/", ".").Replace("\\", "."), StringComparison.OrdinalIgnoreCase));

            if (manifestName == null)
            {
                throw new InvalidOperationException($"Failed to find resource [{fileName}]");
            }

            return assemblyType.GetTypeInfo().Assembly.GetManifestResourceStream(manifestName);
        }
    }
}
