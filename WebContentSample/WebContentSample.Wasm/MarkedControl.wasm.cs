using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Uno.Foundation;
using Uno.Foundation.Interop;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace WebContentSample
{
    public partial class MarkedControl : Control, IJSObject
    {
        public event EventHandler MarkedReady;

        private readonly JSObjectHandle _handle;

        /// <inheritdoc />
        JSObjectHandle IJSObject.Handle => _handle;

        public MarkedControl()
        {
            _handle = JSObjectHandle.Create(this);
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //Background = new SolidColorBrush(Colors.White);
            MarkedReady?.Invoke(this, EventArgs.Empty);
        }

        public async Task InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
		{
            Func<string, string> replace = (String txt) => txt
            .Replace("\\", "\\\\")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\"", "\\\"")
            .Replace("\'", "\\\'")
            .Replace("`", "\\`")
            .Replace("^", "\\^")
            ;

            var newArgument = replace(arguments.Single());


            Console.WriteLine("+++++++++++++++++++++++++++++++ Invoke Script +++++++++++++++++++++++++++++++++++++++++++++++++++++++");
			//var script = $@"(function() {{
			//	try {{
			//		return window.eval(""{newArgument}"") || """";
			//	}}
			//	catch(err){{
			//		console.log(err);
			//	}}
			//	finally {{
			//		window.__evalMethod = null;
			//	}}
			//}})()";

            var script = $"javascript:{scriptName}(\"{newArgument}\");";
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
        }
	}
}
