using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;


/// <summary>Class that create html pages to display stats.</summary>
public class HtmlHelpers {

    /// <summary>Generic html header.</summary>
    static string head = @"<!DOCTYPE html>
            <html>
                <head>
                    <title>Answers</title>
                    <link rel='stylesheet' href='mystyle.css'>
                </head>";

    /// <summary>Generic html footer.</summary>
    static string foot = @"</html>";

    /// <summary>Creates the page that display the statistics of the clients answers to a question.</summary>
    /// <param name="filename">The name of the file we want to create</param>
    /// <param name="a1">The amount of answers for response 1</param>
    /// <param name="a2">The amount of answers for response 2</param>
    /// <param name="a3">The amount of answers for response 3</param>
    /// <param name="a4">The amount of answers for response 4</param>
    /// <param name="abs">The amount of people that didn't answer</param>
    /// <returns>void</returns>
    public static void createAnswerStatPage(string filename, float a1, float a2, float a3, float a4, float abs)
    {
        const int pixH = 165;
        float sum = a1 + a2 + a3 + a4 + abs;
        float a1p = 0;
        float a2p = 0;
        float a3p = 0;
        float a4p = 0;
        float absp = 0;
        if(sum != 0)
        {
            a1p = a1 / sum;
            a2p = a2 / sum;
            a3p = a3 / sum;
            a4p = a4 / sum;
            absp = abs / sum;
        }

        string page = head + @"
                <body>
                    <div id='vertgraph'>
                        <ul>
                            <li class='a1' style='height: " + (int)(10 + pixH * a1p) + @"px;'>" + (int)(a1p * 100) + @"</li>
                            <li class='a2' style='height: " + (int)(10 + pixH * a2p) + @"px;'>" + (int)(a2p * 100) + @"</li>
                            <li class='a3' style='height: " + (int)(10 + pixH * a3p) + @"px;'>" + (int)(a3p * 100) + @"</li>
                            <li class='a4' style='height: " + (int)(10 + pixH * a4p) + @"px;'>" + (int)(a4p * 100) + @"</li>
                            <li class='abstention' style='height: " + (int)(10 + pixH * absp) + @"px;'>" + (int)(absp * 100) + @"</li>
                        </ul>
                    </div>
                </body>"
            + foot;

        string path = Application.dataPath + @"/Resources/html/" + filename + ".html";
        writeFile(path, page);
        path = Application.dataPath +  @"/Resources/html/lastQuestionStat.html";
        writeFile(path, page);
    }

    /// <summary>Actualy writes the file. If another file of the smae name exists, deletes it.</summary>
    /// <param name="path">The full path (filename included) of the file.</param>
    /// <param name="content">The content to put in that file</param>
    /// <returns>void</returns>
    private static void writeFile(string path, string content)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        using (FileStream fs = File.Create(path))
        {
            AddText(fs, content);
        }
    }

    public static void createGameStatPage(string filename, float a1, float a2, float a3, float a4, float abs)
    {
        const int pixH = 165;
        float sum = a1 + a2 + a3 + a4 + abs;
        float a1p = a1 / sum;
        float a2p = a2 / sum;
        float a3p = a3 / sum;
        float a4p = a4 / sum;
        float absp = abs / sum;

        string page = head + @"
                <body>
                    <div id='vertgraph'>
                        <ul>
                            <li class='a1' style='height: " + (int)(10 + pixH * a1p) + @"px;'>" + (int)(a1p * 100) + @"</li>
                            <li class='a2' style='height: " + (int)(10 + pixH * a2p) + @"px;'>" + (int)(a2p * 100) + @"</li>
                            <li class='a3' style='height: " + (int)(10 + pixH * a3p) + @"px;'>" + (int)(a3p * 100) + @"</li>
                            <li class='a4' style='height: " + (int)(10 + pixH * a4p) + @"px;'>" + (int)(a4p * 100) + @"</li>
                            <li class='abstention' style='height: " + (int)(10 + pixH * absp) + @"px;'>" + (int)(absp * 100) + @"</li>
                        </ul>
                    </div>
                </body>"
            + foot;

        string path = @"Assets/Resources/html/" + filename + ".html";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        using (FileStream fs = File.Create(path))
        {
            AddText(fs, page);
        }
    }

    private static void AddText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }
}
