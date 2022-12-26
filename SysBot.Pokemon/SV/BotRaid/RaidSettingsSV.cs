﻿using System.Collections.Generic;
using PKHeX.Core;
using System.ComponentModel;
using System.Threading;
using SysBot.Base;

namespace SysBot.Pokemon
{
    public class RaidSettingsSV : IBotStateSettings, ICountSettings
    {
        private const string Hosting = nameof(Hosting);
        private const string Counts = nameof(Counts);
        private const string FeatureToggle = nameof(FeatureToggle);
        public override string ToString() => "Raid Bot Settings";

        [Category(FeatureToggle), Description("Optional description of the raid the bot is hosting.")]
        public string RaidDescription { get; set; } = string.Empty;

        [Category(FeatureToggle), Description("Optional description of the raid title the bot is hosting.")]
        public string RaidTitleDescription { get; set; } = string.Empty;

        [Category(FeatureToggle), Description("Optional footer description of the raid the bot is hosting.")]
        public string RaidFooterDescription { get; set; } = string.Empty;

        [Category(Hosting), Description("Minimum amount of seconds to wait before starting a raid. Ranges from 0 to 180 seconds.")]
        public int MinTimeToWait { get; set; } = 90;

        [Category(FeatureToggle), Description("If true, the bot will use a random code for the raid.")]
        public bool CodeTheRaid { get; set; } = true;

        [Category(Hosting), Description("Amount of raids to complete before rolling time back 1 hour.")]
        public int RollbackTimeAfterThisManyRaids { get; set; } = 10;

        [Category(Hosting), Description("Enter Discord channel ID(s) to post raid embeds to. Feature has to be initialized via \"$resv\" after every client restart.")]
        public string RaidEmbedChannelsSV { get; set; } = string.Empty;

        [Category(FeatureToggle), Description("When enabled, the screen will be turned off during normal bot loop operation to save power.")]
        public bool ScreenOff { get; set; } = false;

        private int _completedRaids;

        [Category(Counts), Description("Raids Started")]
        public int CompletedRaids
        {
            get => _completedRaids;
            set => _completedRaids = value;
        }

        [Category(Counts), Description("When enabled, the counts will be emitted when a status check is requested.")]
        public bool EmitCountsOnStatusCheck { get; set; }

        public int AddCompletedRaids() => Interlocked.Increment(ref _completedRaids);

        public IEnumerable<string> GetNonZeroCounts()
        {
            if (!EmitCountsOnStatusCheck)
                yield break;
            if (CompletedRaids != 0)
                yield return $"Started Raids: {CompletedRaids}";
        }
    }
}