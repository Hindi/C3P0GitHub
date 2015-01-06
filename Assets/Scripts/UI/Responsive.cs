using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class Responsive
{
    public const int baseWidth = 1024;
    public const int baseFontSize = 18;

    public static float responsiveFactor()
    {
        return (float)Screen.width / (float)Responsive.baseWidth;
    }

}
