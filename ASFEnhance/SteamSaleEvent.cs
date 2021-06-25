//     _                _      _  ____   _                           _____
//    / \    _ __  ___ | |__  (_)/ ___| | |_  ___   __ _  _ __ ___  |  ___|__ _  _ __  _ __ ___
//   / _ \  | '__|/ __|| '_ \ | |\___ \ | __|/ _ \ / _` || '_ ` _ \ | |_  / _` || '__|| '_ ` _ \
//  / ___ \ | |  | (__ | | | || | ___) || |_|  __/| (_| || | | | | ||  _|| (_| || |   | | | | | |
// /_/   \_\|_|   \___||_| |_||_||____/  \__|\___| \__,_||_| |_| |_||_|   \__,_||_|   |_| |_| |_|
// |
// Copyright 2015-2021 Łukasz "JustArchi" Domeradzki
// Contact: JustArchi@JustArchi.net
// |
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// |
// http://www.apache.org/licenses/LICENSE-2.0
// |
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Web.Responses;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Threading.Tasks;

namespace Chrxw.ASFEnhance
{
    internal sealed class SteamSaleEvent
    {
        private const byte MaxSingleQueuesDaily = 2; // This is only a failsafe for infinite queue clearing (in case IsDiscoveryQueueAvailable() would fail us)
        internal static async Task<ImmutableHashSet<uint>?> GenerateNewDiscoveryQueue(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/explore/generatenewdiscoveryqueue");

            // Extra entry for sessionID
            Dictionary<string, string> data = new(2, StringComparer.Ordinal) { { "queuetype", "0" } };

            ObjectResponse<NewDiscoveryQueueResponse>? response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<NewDiscoveryQueueResponse>(request, data: data).ConfigureAwait(false);

            return response?.Content.Queue;
        }

        internal static async Task<IDocument?> GetDiscoveryQueuePage(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/explore?l=english");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            return response?.Content;
        }

        internal static async Task<bool> ClearFromDiscoveryQueue(uint appID, Bot bot)
        {
            if (appID == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(appID));
            }

            Uri request = new(SteamStoreURL, "/app/" + appID);

            // Extra entry for sessionID
            Dictionary<string, string> data = new(2, StringComparer.Ordinal) { { "appid_to_clear_from_queue", appID.ToString(CultureInfo.InvariantCulture) } };

            return await bot.ArchiWebHandler.UrlPostWithSession(request, data: data).ConfigureAwait(false);
        }

        internal static async Task<bool> ExploreDiscoveryQueue(Bot bot)
        {
            if (!(await IsDiscoveryQueueAvailable(bot).ConfigureAwait(false)).GetValueOrDefault())
            {
                return false;
            }
            for (byte i = 0; (i < MaxSingleQueuesDaily); i++)
            {
                ImmutableHashSet<uint>? queue = await GenerateNewDiscoveryQueue(bot).ConfigureAwait(false);
                if ((queue == null) || (queue.Count == 0))
                {
                    bot.ArchiLogger.LogGenericTrace(string.Format(CultureInfo.CurrentCulture, Strings.ErrorIsEmpty, nameof(queue)));
                    break;
                }
                bot.ArchiLogger.LogGenericInfo(string.Format(CultureInfo.CurrentCulture, Strings.ClearingDiscoveryQueue, i));

                // We could in theory do this in parallel, but who knows what would happen...
                foreach (uint queuedAppID in queue)
                {
                    if (await ClearFromDiscoveryQueue(queuedAppID, bot).ConfigureAwait(false))
                    {
                        continue;
                    }
                    return false;
                }
            }
            return true;
        }

        internal static async Task<bool?> IsDiscoveryQueueAvailable(Bot bot)
        {
            Uri request = new(SteamStoreURL, "/explore?l=english");

            HtmlDocumentResponse? response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

            IDocument? htmlDocument = response?.Content;

            if (htmlDocument == null)
            {
                return null;
            }

            IElement? htmlNode = htmlDocument.SelectSingleNode("//div[@class='subtext']");

            if (htmlNode == null)
            {
                // Valid, no cards for exploring the queue available
                return false;
            }

            string text = htmlNode.TextContent;

            if (string.IsNullOrEmpty(text))
            {
                bot.ArchiLogger.LogNullError(nameof(text));

                return null;
            }

            bot.ArchiLogger.LogGenericTrace(text);

            // It'd make more sense to check against "Come back tomorrow", but it might not cover out-of-the-event queue
            return text.StartsWith("You can get ", StringComparison.Ordinal);
        }

        internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;
    }
}
