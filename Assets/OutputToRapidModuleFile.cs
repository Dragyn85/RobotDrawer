using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OutputToRapidModuleFile : ITakePositionData
{
    const string ROB_TARGET_BASE = "CONST robtarget zeroPos := [[0,0,-2],[0.002364117,0.992700113,0.055506584,-0.107050994],[-1,-1,0,1],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];";
    const string PUBLIC_ROUTINE_NAME = "PROC Draw(PERS ToolData tool, PERS WobjData paper)";
    const string MOVE_TO_SAFE_POS = "MOVEJ Offs(zeroPos, 100,100, 150), v1000, z50, tool\\wobj:=paper;";
    const string ROUTINE_ENDING = "ENDPROC";
    const string LINE_START = "MOVEL Offs(zeroPos,";
    const string LINE_ENDING = "), v200, z0, tool\\wobj:=paper;\n";
    const string FILE_NAME = "MyDrawing";
    const string FILE_EXTENSION = ".MOD";

    public bool ReadyToRecieve => true;

    private string fullPath;
    int suffix;

    public OutputToRapidModuleFile(string path)
    {
        var directory = Application.persistentDataPath + "/";
        if (Directory.Exists(path))
        {
            directory = path + "/";
        }
        bool fileExists = true;
        int suffix = 1;

        if(!Directory.Exists(directory))
        {
            Debug.Log("No directory");
        }
        while (fileExists)
        {
            var filepath = directory + GetFileNameWithSuffix(suffix);
            if (!File.Exists(filepath))
            {
                fullPath = filepath;
                fileExists = false;
                this.suffix= suffix;
            }
            suffix++;
        }
        InitializeFile();
    }
    string FloatToString(float value)
    {
        string floatValue = value.ToString();
        return floatValue.Replace(',','.');
    }

    private void InitializeFile()
    {
        OutputLineToFile("MODULE MyDrawing" + suffix);
        OutputLineToFile(ROB_TARGET_BASE);
        OutputLineToFile("");
        OutputLineToFile(PUBLIC_ROUTINE_NAME);
        OutputLineToFile(MOVE_TO_SAFE_POS);
    }
    public void EndProces()
    {
        OutputLineToFile(ROUTINE_ENDING);
        OutputLineToFile("ENDMODULE");
    }
    void OutputLineToFile(string content)
    {
        string contentWithLineEnding = content + "\n";
        File.AppendAllText(fullPath, contentWithLineEnding);
    }
    private string GetFileNameWithSuffix(int suffix)
    {
        return $"{FILE_NAME}{suffix}{FILE_EXTENSION}";

    }

    public void AddNewPositionData(SendData data)
    {
        if (data.beginDraw)
        {
            string preOutput = $"{LINE_START}{FloatToString(210-data.posy)},{FloatToString(data.posx)},15{LINE_ENDING}";
            OutputLineToFile(preOutput);
            
        }


        string output = $"{LINE_START}{FloatToString(210 - data.posy)},{FloatToString(data.posx)},0{LINE_ENDING}";
        OutputLineToFile(output);


        if (data.endDraw)
        {
            string reOutput = $"{LINE_START}{FloatToString(210 - data.posy)} , {FloatToString(data.posx)},15{LINE_ENDING}";
            OutputLineToFile(reOutput);
        }
    }


}


