using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RedeemPlus {
	// This is example class that shows how you can call third-party services within your plugin
	// You've always wanted from your ASF to post cats, right? Now is your chance!
	// P.S. The code is almost 1:1 copy from the one I use in ArchiBot, you can thank me later
	internal static class SetuAPI {
		private const string URL = "https://api.dongmanxingkong.com/suijitupian/acg/1080p/index.php";

		internal static string GetSetu() {
			return URL;
		}
	}
}
