using System;
using System.Threading.Tasks;
using Uno.Extensions;

namespace WebContentSample
{
    public partial class MarkedControl : JavaScriptControl
    {
        public event EventHandler MarkedReady;

        public string MarkedEmbeddedJavaScriptFile { get; set; } = "marked.min.js";

        protected override async Task LoadJavaScript()
        {
            await LoadEmbeddedJavaScriptFile(MarkedEmbeddedJavaScriptFile);

            MarkedReady?.Invoke(this, EventArgs.Empty);
        }

        public async Task DisplayMarkdown(string markdown)
        {
            markdown = markdown.Replace("\n", "\\n").Replace("\r", "\\r");

            await UpdateHtmlFromScript($"marked('{markdown}')");
        }

        public async Task LoadMarkdownFromFile(string embeddedFileName)
        {
            var markdown = (await GetEmbeddedFileStreamAsync(GetType(), embeddedFileName)).ReadToEnd();
            await DisplayMarkdown(markdown);
        }
    }
}
