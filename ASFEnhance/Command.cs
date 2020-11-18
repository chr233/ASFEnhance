using System;
using System.Composition;
using System.Threading.Tasks;
using ArchiSteamFarm;
using ArchiSteamFarm.Json;
using ArchiSteamFarm.Plugins;

namespace RedeemPlus {
	[Export(typeof(IPlugin))]
	internal sealed class RedeemPlus : IBotCommand {
		public string Name => nameof(RedeemPlus);

		// This will be displayed to the user and written in the log file, typically you should point it to the version of your library, but alternatively you can do some more advanced logic if you'd like to
		// Please note that this property can have direct dependencies only on structures that were initialized by the constructor, as it's possible to be called before OnLoaded() takes place
		public Version Version => typeof(RedeemPlus).Assembly.GetName().Version ?? throw new ArgumentNullException(nameof(Version));

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
		public async Task<string?> OnBotCommand(Bot bot, ulong steamID, string message, string[] args) {
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
			// In comparison with OnBotMessage(), we're using asynchronous CatAPI call here, so we declare our method as async and return the message as usual
			// Notice how we handle access here aswell, it'll work only for FamilySharing+
			switch (args[0].ToUpperInvariant()) {
				case "K":
				case "KEY":
					string rs = GrubKeys.String2keysString(message);
					return rs;
				default:
					return null;
			}
		}


		public void OnLoaded() {
			ASF.ArchiLogger.LogGenericInfo("色图插件已加载");
		}
	}
}
