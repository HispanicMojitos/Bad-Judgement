using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class MissionSave
{
    private static readonly string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    private static readonly string gamePath = string.Format(@"{0}\BadJudgement", directoryPath);
    private static readonly string saveFolder = string.Format(@"{0}\Saves", gamePath);
    private static readonly string savePath = string.Format(@"{0}\save.bjg", saveFolder);

    public static List<string> missionNames = new List<string>() { "First scene", "Mission 1" };

    public static void CreateSaveFolder()
    {
        if (!Directory.Exists(gamePath)) Directory.CreateDirectory(gamePath);
        if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
        if (!File.Exists(savePath))
        {
            File.Create(savePath).Dispose();
            File.WriteAllText(savePath, missionNames[0]);
        }
    }

    public static void DeleteAndReCreateSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            File.Create(savePath).Dispose();
            File.WriteAllText(savePath, missionNames[0]);
        }
    }

    public static string GetActualMissionName()
    {
        string missionName = File.ReadAllText(savePath);

        if (missionNames.Exists(x => x == missionName)) return missionName;
        else return null;
    }

    public static int IndexOfActualMission()
    {
        string missionName = GetActualMissionName();
        int indexOf = missionNames.IndexOf(missionName);

        return indexOf;
    }

    public static void NextMission()
    {
        int actualMission = IndexOfActualMission();

        if (actualMission < missionNames.Count - 1) File.WriteAllText(savePath, missionNames[actualMission + 1]);
    }
}