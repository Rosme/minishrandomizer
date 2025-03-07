﻿using System.Text;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;
using RandomizerCore.Utilities.Logging;
using RandomizerCore.Utilities.Util;

namespace RandomizerCore.Randomizer.Logic.Options;

public class LogicDropdown : LogicOptionBase
{
    public LogicDropdown(
        string name,
        string niceName,
        string settingGroup,
        string settingPage,
        string descriptionText,
        string defaultSelection,
        LogicOptionType type,
        Dictionary<string, string> selections) :
        base(name, niceName, true, settingGroup, settingPage, descriptionText, type)
    {
        Selections = selections;
        Selection = selections.Keys.ToList()[
            selections.Values.ToList()
                .IndexOf(defaultSelection)]; //Not sure if this works, it should but needs to be tested
    }

    public string Selection { get; set; }
    public Dictionary<string, string> Selections { get; }

    public override List<LogicDefine> GetLogicDefines()
    {
        var defineList = new List<LogicDefine>(3);

        // Only true if a color has been selected
        if (!Active) return defineList;


        if (!Selections.TryGetValue(Selection, out var content)) return defineList;

        Logger.Instance.LogInfo($"Active Define: {Name}");
        defineList.Add(new LogicDefine(Name, content));

        return defineList;
    }

    public override byte GetHashByte()
    {
        return Active ? Encoding.ASCII.GetBytes(Selection).Crc8() : (byte)0x0b;
    }

    public override string GetOptions()
    {
        var builder = new StringBuilder();
        foreach (var selection in Selections)
            builder.Append("{Key: ").Append(selection.Key).Append(" Value: ").Append(selection.Value).Append("}, ");

        return builder.ToString();
    }

    public override string GetOptionUiType()
    {
        return "Dropdown";
    }
}
