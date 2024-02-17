using PKHeX.Core;

using System;
using SysBot.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

using Discord;

namespace SysBot.Pokemon;

public class TemplateRaid 
{
    private SAV9SV HostSAV = new();
    private PokeTradeHub<PK9> Hub;
    private int RotationCount = 0;
    private RotatingRaidSettingsSV.RotatingRaidParameters2 setting => Hub.Config.RotatingRaidSV.RaidEmbedParameters2[this.RotationCount];
    

    public TemplateRaid(SAV9SV HostSAV, PokeTradeHub<PK9> Hub, int RotationCount)
    {
        this.HostSAV = HostSAV;
        this.Hub = Hub;
        this.RotationCount = RotationCount;
    }

    private Color SetColor()
    {
        return Color.Green;
    }
    private string SetImageUrl()
    {

        return setting.ScreenShotImg;
    }


    private EmbedAuthorBuilder SetAuthor()
    {
        EmbedAuthorBuilder author = new EmbedAuthorBuilder
        {
            Name = setting.Title,
            IconUrl = setting.TeraTypeImg
        };
        return author;
    }
    private string SetThumbnailUrl()
    {
        return setting.PKMImg;
    }

    private EmbedFooterBuilder SetFooter()
    {
        string StartTime = $"{setting.StartTime - DateTime.Now:d\\.hh\\:mm\\:ss}";
        string RaidCount = $"{setting.RaidCount}";
        string WinCount = $"{setting.WinCount}";
        string LossCount = $"{setting.LossCount}";

        string Text = "";
        Text += $"Host: {HostSAV.OT} | Uptime: {StartTime}\n";
        Text += $"Raids: {RaidCount} | Wins: {WinCount} | Losses: {LossCount}\n";
        Text += $"Disclaimer: Raids are on rotation via seed injection.\n";

        string IconUrl = "https://i.postimg.cc/fR2gBJPm/Gatoraid.jpg";
        return  new EmbedFooterBuilder { Text = Text , IconUrl = IconUrl };
    }

    private void SetFiled1_1(EmbedBuilder embed)
    {
        // 构建信息
        string FiledName = $"RaidInfo:";
        string FiledValue = setting.RaidInfo;

        embed.AddField(FiledName, FiledValue, true);
    }

    private void SetFiled1_2(EmbedBuilder embed)
    {
        string movesInfo = setting.MovesInfo;
        string extraMovesInfo = string.Join("\n", setting.ExtraMovesInfo.Split("ㅤ"));
        extraMovesInfo = extraMovesInfo == "" ? "-" : extraMovesInfo;

        string content = "";
        content += $"{movesInfo}";
        content += $"**ExtraMoves:**\n";
        content += $"{extraMovesInfo}";

        // 构建信息
        string FiledName = $"**Moveset:**";
        string FiledValue = content;

        embed.AddField(FiledName, FiledValue, true);
    }
    private void SetFiled2_1(EmbedBuilder embed)
    {
        // 构建信息
        string FiledName = $"Special Rewards:";
        string FiledValue = setting.RewardsInfo;

        embed.AddField(FiledName, FiledValue, true);
    }

    private void SetFiled2_2(EmbedBuilder embed)
    {              
        var raidParams2 = setting;
        string raidCode = $"{setting.CodeInfo}";
        string Seed = $"{setting.Seed}";

        // 构建信息
        string Content = "";
        Content += $"- Raid Code:{raidCode}\n";
        Content += $"- Seed:{Seed}";
        
        // 构建信息
        string FiledName = $"Waiting in lobby!";
        string FiledValue = Content;

        embed.AddField(FiledName, FiledValue, true);
    }

    private void SetFiledTemp(EmbedBuilder embed)
    {                
        embed.AddField($"** **", $"** **", true);
    }

    public EmbedBuilder Generate()
    {   
        // 构建discord的Embed
        var embed = new EmbedBuilder { 
            Color = this.SetColor(), 
            ImageUrl = this.SetImageUrl(), 
            Author = this.SetAuthor(), 
            Footer = this.SetFooter(), 
            ThumbnailUrl = this.SetThumbnailUrl(),
            };

        // 构建Embed中的Filed        
        this.SetFiled1_1(embed);
        this.SetFiled1_2(embed);
        this.SetFiledTemp(embed);
        this.SetFiled2_1(embed);
        this.SetFiled2_2(embed);
        this.SetFiledTemp(embed);

        return embed;
    }
}

