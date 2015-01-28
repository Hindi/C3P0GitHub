using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class HtmlHelpers {

    static string head = @"<!DOCTYPE html>
            <html>
                <head>
                    <title>Answers</title>
                    <link rel='stylesheet' href='mystyle.css'>
                </head>";
    static string foot = @"</html>";

    public static void createAnswerStatPage(string filename, float a1, float a2, float a3, float a4, float abs)
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

        string path = @"Assets/Resources/html/"+ filename + ".html";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        using (FileStream fs = File.Create(path))
        {
            AddText(fs, page);
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
