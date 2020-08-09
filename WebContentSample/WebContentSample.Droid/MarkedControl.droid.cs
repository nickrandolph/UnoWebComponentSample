using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WebContentSample
{
    public partial class MarkedControl
    {
        partial void PrepareArgument(string argument, ref string newArgument)
        {
            if (argument == null) return;

            Func<string, string> replace = (String txt) => txt.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"").Replace("\'", "\\\'").Replace("`", "\\`").Replace("^", "\\^");

            newArgument= replace(argument);
        }
    }
}