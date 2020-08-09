using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebContentSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            webView.NavigationCompleted += WebView_NavigationCompleted;
            markedCtrl.MarkedReady += markedCtrl_MarkedReady;
        }

        private async void markedCtrl_MarkedReady(object sender, EventArgs args)
        {
            await markedCtrl.LoadMarkdownFromFile("SharedAssets.md");

            //var markdownScript = (await GetEmbeddedFileStreamAsync(GetType(), "marked.min.js")).ReadToEnd();

            //await markedCtrl.InvokeScriptAsync("eval", new[] { markdownScript });

            //var markdown = (await GetEmbeddedFileStreamAsync(GetType(), "SharedAssets.md")).ReadToEnd();

            ////markdown = markdown.Replace("\\n", "\\\\n").Replace("\\r", "\\\\r");
            //markdown = markdown.Replace("\n", "\\n").Replace("\r", "\\r");

            //var id = HtmlContentId;

            //var script = $@"document.getElementById('{id}').innerHTML = marked('{markdown}');";
            //await markedCtrl.InvokeScriptAsync("eval", new[] { script });
        }


        private async void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //var markdownScript = (await GetEmbeddedFileStreamAsync(GetType(), "marked.min.js")).ReadToEnd();

            //Func<string,string> replace = (String txt) => txt.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"").Replace("\'", "\\\'").Replace("`", "\\`").Replace("^", "\\^");

            //markdownScript = replace(markdownScript);
            //var token = new CancellationTokenSource();
            //var result = await webView.InvokeScriptAsync(token.Token, "eval", new[] { markdownScript });

            //var markdown = (await GetEmbeddedFileStreamAsync(GetType(), "SharedAssets.md")).ReadToEnd();

            //markdown = replace(markdown).Replace("\\n", "\\\\n").Replace("\\r", "\\\\r");

            //var script = $@"document.getElementById('content').innerHTML = marked('{markdown}');";
            //result =await webView.InvokeScriptAsync(token.Token, "eval", new[] { script });
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //webView.Source = new Uri("https://builttoroam.com");

            var html = @"<html>
     <body>
       <div id = ""content"">This is default text</ div>
    </ body>
    </ html>
";
            webView.NavigateToString(html);

            /*
                     <script src = ""https://cdn.jsdelivr.net/npm/marked/marked.min.js""></ script>
                     <script>
                       document.getElementById('content').innerHTML =
                         marked('# Marked in browser\n\nRendered by **marked**.');
              </ script>

             */



        }

       


        //public static async Task<Stream> GetEmbeddedFileStreamAsync(Type assemblyType, string fileName)
        //{
        //    await Task.Yield();

        //    var manifestName = assemblyType.GetTypeInfo().Assembly
        //        .GetManifestResourceNames()
        //        .FirstOrDefault(n => n.EndsWith(fileName.Replace(" ", "_"), StringComparison.OrdinalIgnoreCase));

        //    if (manifestName == null)
        //    {
        //        throw new InvalidOperationException($"Failed to find resource [{fileName}]");
        //    }

        //    return assemblyType.GetTypeInfo().Assembly.GetManifestResourceStream(manifestName);
        //}
    }
}
