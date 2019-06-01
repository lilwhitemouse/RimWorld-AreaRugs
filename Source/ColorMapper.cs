using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;


namespace LWM.Dyeable
{
    
    public class ColorMapper {
        
        //create the dictionary with the elements you are interested in

        // initialized in static class constructor
        //   in case language settings get changed without restart, we want
        //   to make sure our translations (which we do ONCE) are after that.
        private static Dictionary<uint, String> colorMap;
        
//        public static String GetName(UnityEngine.Color color)
        public static String GetNameExact(uint color)
        {
//            int myRgb = (int)(color.ToArgb() & 0x00FFFFFF);
//            uint myRgb = GetRGB(color);
            if (colorMap.ContainsKey(color))
            {
//                Log.Warning("Found key "+myRgb+" ("+myRgb.ToString("X6")+"): ->"+colorMap[myRgb]+"<");
                return colorMap[color];
            }
            return null;
        }
        public static String GetNearestName(UnityEngine.Color unityColor) {
            uint tmp;
            return GetNearestColor(unityColor,out tmp);
        }


        public static String GetNearestColor(UnityEngine.Color unityColor, out uint closestColorResult)
        {
            Log.Error("Getting nearest name for: "+unityColor);
            //check first for an exact match
            uint color = GetRGB(unityColor);
            String name = GetNameExact(color);
            if (name != null) 
            {
                closestColorResult=color;
                return name;
            }
            //mask out the alpha channel
//            int myRgb = (int)(color.ToArgb() & 0x00FFFFFF);
            //retrieve the color from the dictionary with the closest measure
            int min=3*0xFF;
            uint closestColor= 0;
//            Log.Warning("Starting key search with a lot of keys? "+colorMap.Keys.ToList().Count);
            foreach (uint c in colorMap.Keys) {
//                Log.Message("Testing vs "+c.ToString("X6"));
                int distance=ColorDistance(color,c);
//                Log.Message("  Distance is "+distance);
                if (distance < min) {
//                    Log.Message("It's the new shortest distance!");
                    closestColor=c;
                    min=distance;
                }
            }
/*            foreach (KeyValuePair<uint,string> c in colorMap) {
                int distance=ColorDistance(myRgb,c.Key);
                if (distance < min) {
                    closestColor=c.
                    min=distance;
                }
                
            }
            for (int i=0; i<colorMap.Keys.Count; i++) { //   .Select(colorKey => new ColorDistance(colorKey, myRgb)).MinBy(d => d.distance).colorKey;
                int distance=ColorDistance(myRgb,colorMap.Keys[i]);
                if (distance < min) {
                    closestColor=colorMap.Keys[i];
                    min=distance;
                }
            }*/
            //return the name
            Log.Warning("Closest Color for "+color.ToString("X6")+" is "+closestColor.ToString("X6")+" ("+colorMap[closestColor]+")");
            closestColorResult=closestColor;
            return colorMap[closestColor];
        }

        public static uint GetRGB(UnityEngine.Color uc) {
//            Log.Message("Converting color "+uc+": ToHtmlStringRGB: ["+ColorUtility.ToHtmlStringRGB(uc)+"]");
//            Log.Message("That converts to ["+Convert.ToUInt32(ColorUtility.ToHtmlStringRGB(uc),16)+"]");
//            string s = ColorUtility.ToHtmlStringRGB(c);
            return Convert.ToUInt32(ColorUtility.ToHtmlStringRGB(uc),16);
        }

        public static UnityEngine.Color GetUnityColor(uint c) {
            UnityEngine.Color tc;
            if (ColorUtility.TryParseHtmlString("#"+c.ToString("X6"), out tc)) {
                return tc;
            }
            Log.Error("LWM.Dyeable failed to parse color "+c.ToString("X6"));
            return new UnityEngine.Color(0,0,0,0);
        }

        public static int ColorDistance(uint c1, uint c2) {
            return Math.Abs((int)(c1&0xFF-c2&0xFF))+
                Math.Abs((int)(((c1>>8)&0xFF)-((c2>>8)&0xFF)))+
                Math.Abs((int)((c1>>16)-(c2>>16)));
        }

//        public static int GetRGB(System.Drawing.Color sc) {
//            return (int)(sc.ToArgb()() & 0x00FFFFFF);
//        }

        static ColorMapper()
        {
        /*****  Dictionary of Color with names used:   *****
         * This list was generated from the C# "System.Color.KnownColor"s list.
         * Code used was:
            System.Collections.Generic.List<string> colorList=new System.Collections.Generic.List<string>();
            foreach (System.Drawing.KnownColor kc in Enum.GetValues(typeof(System.Drawing.KnownColor)))
            {
                if ((int)kc > 27 && (int)kc < 168)
                // limits based off of https://docs.microsoft.com/en-us/dotnet/api/system.drawing.knowncolor?view=netframework-3.5
                {
                    string c=(System.Drawing.Color.FromKnownColor(kc).ToArgb()&0xFFFFFF).ToString("x6");
                    colorList.Add("{0x"+c+", \"x"+c+"\".Translate()}"); // Remove duplicates by hand :p
                }
            }
            Console.WriteLine("        "+String.Join(", ", colorList.ToArray()));
         *
         * This corresponds to the color names in Languages/English/Keyed/ColorNames.xml
         */
            colorMap = new Dictionary<uint, String>()
                {
                    // {0xFFB6C1, "Light Pink"}, &c
                    // This is the longest line I have EVER put into a program:
                    //  (removed duplicates by hand :p)
                    {0xf0f8ff, "xf0f8ff".Translate()}, {0xfaebd7, "xfaebd7".Translate()}, {0x00ffff, "x00ffff".Translate()}, {0x7fffd4, "x7fffd4".Translate()}, {0xf0ffff, "xf0ffff".Translate()}, {0xf5f5dc, "xf5f5dc".Translate()}, {0xffe4c4, "xffe4c4".Translate()}, {0x000000, "x000000".Translate()}, {0xffebcd, "xffebcd".Translate()}, {0x0000ff, "x0000ff".Translate()}, {0x8a2be2, "x8a2be2".Translate()}, {0xa52a2a, "xa52a2a".Translate()}, {0xdeb887, "xdeb887".Translate()}, {0x5f9ea0, "x5f9ea0".Translate()}, {0x7fff00, "x7fff00".Translate()}, {0xd2691e, "xd2691e".Translate()}, {0xff7f50, "xff7f50".Translate()}, {0x6495ed, "x6495ed".Translate()}, {0xfff8dc, "xfff8dc".Translate()}, {0xdc143c, "xdc143c".Translate()}, {0x00008b, "x00008b".Translate()}, {0x008b8b, "x008b8b".Translate()}, {0xb8860b, "xb8860b".Translate()}, {0xa9a9a9, "xa9a9a9".Translate()}, {0x006400, "x006400".Translate()}, {0xbdb76b, "xbdb76b".Translate()}, {0x8b008b, "x8b008b".Translate()}, {0x556b2f, "x556b2f".Translate()}, {0xff8c00, "xff8c00".Translate()}, {0x9932cc, "x9932cc".Translate()}, {0x8b0000, "x8b0000".Translate()}, {0xe9967a, "xe9967a".Translate()}, {0x8fbc8b, "x8fbc8b".Translate()}, {0x483d8b, "x483d8b".Translate()}, {0x2f4f4f, "x2f4f4f".Translate()}, {0x00ced1, "x00ced1".Translate()}, {0x9400d3, "x9400d3".Translate()}, {0xff1493, "xff1493".Translate()}, {0x00bfff, "x00bfff".Translate()}, {0x696969, "x696969".Translate()}, {0x1e90ff, "x1e90ff".Translate()}, {0xb22222, "xb22222".Translate()}, {0xfffaf0, "xfffaf0".Translate()}, {0x228b22, "x228b22".Translate()}, {0xff00ff, "xff00ff".Translate()}, {0xdcdcdc, "xdcdcdc".Translate()}, {0xf8f8ff, "xf8f8ff".Translate()}, {0xffd700, "xffd700".Translate()}, {0xdaa520, "xdaa520".Translate()}, {0x808080, "x808080".Translate()}, {0x008000, "x008000".Translate()}, {0xadff2f, "xadff2f".Translate()}, {0xf0fff0, "xf0fff0".Translate()}, {0xff69b4, "xff69b4".Translate()}, {0xcd5c5c, "xcd5c5c".Translate()}, {0x4b0082, "x4b0082".Translate()}, {0xfffff0, "xfffff0".Translate()}, {0xf0e68c, "xf0e68c".Translate()}, {0xe6e6fa, "xe6e6fa".Translate()}, {0xfff0f5, "xfff0f5".Translate()}, {0x7cfc00, "x7cfc00".Translate()}, {0xfffacd, "xfffacd".Translate()}, {0xadd8e6, "xadd8e6".Translate()}, {0xf08080, "xf08080".Translate()}, {0xe0ffff, "xe0ffff".Translate()}, {0xfafad2, "xfafad2".Translate()}, {0xd3d3d3, "xd3d3d3".Translate()}, {0x90ee90, "x90ee90".Translate()}, {0xffb6c1, "xffb6c1".Translate()}, {0xffa07a, "xffa07a".Translate()}, {0x20b2aa, "x20b2aa".Translate()}, {0x87cefa, "x87cefa".Translate()}, {0x778899, "x778899".Translate()}, {0xb0c4de, "xb0c4de".Translate()}, {0xffffe0, "xffffe0".Translate()}, {0x00ff00, "x00ff00".Translate()}, {0x32cd32, "x32cd32".Translate()}, {0xfaf0e6, "xfaf0e6".Translate()}, {0x800000, "x800000".Translate()}, {0x66cdaa, "x66cdaa".Translate()}, {0x0000cd, "x0000cd".Translate()}, {0xba55d3, "xba55d3".Translate()}, {0x9370db, "x9370db".Translate()}, {0x3cb371, "x3cb371".Translate()}, {0x7b68ee, "x7b68ee".Translate()}, {0x00fa9a, "x00fa9a".Translate()}, {0x48d1cc, "x48d1cc".Translate()}, {0xc71585, "xc71585".Translate()}, {0x191970, "x191970".Translate()}, {0xf5fffa, "xf5fffa".Translate()}, {0xffe4e1, "xffe4e1".Translate()}, {0xffe4b5, "xffe4b5".Translate()}, {0xffdead, "xffdead".Translate()}, {0x000080, "x000080".Translate()}, {0xfdf5e6, "xfdf5e6".Translate()}, {0x808000, "x808000".Translate()}, {0x6b8e23, "x6b8e23".Translate()}, {0xffa500, "xffa500".Translate()}, {0xff4500, "xff4500".Translate()}, {0xda70d6, "xda70d6".Translate()}, {0xeee8aa, "xeee8aa".Translate()}, {0x98fb98, "x98fb98".Translate()}, {0xafeeee, "xafeeee".Translate()}, {0xdb7093, "xdb7093".Translate()}, {0xffefd5, "xffefd5".Translate()}, {0xffdab9, "xffdab9".Translate()}, {0xcd853f, "xcd853f".Translate()}, {0xffc0cb, "xffc0cb".Translate()}, {0xdda0dd, "xdda0dd".Translate()}, {0xb0e0e6, "xb0e0e6".Translate()}, {0x800080, "x800080".Translate()}, {0xff0000, "xff0000".Translate()}, {0xbc8f8f, "xbc8f8f".Translate()}, {0x4169e1, "x4169e1".Translate()}, {0x8b4513, "x8b4513".Translate()}, {0xfa8072, "xfa8072".Translate()}, {0xf4a460, "xf4a460".Translate()}, {0x2e8b57, "x2e8b57".Translate()}, {0xfff5ee, "xfff5ee".Translate()}, {0xa0522d, "xa0522d".Translate()}, {0xc0c0c0, "xc0c0c0".Translate()}, {0x87ceeb, "x87ceeb".Translate()}, {0x6a5acd, "x6a5acd".Translate()}, {0x708090, "x708090".Translate()}, {0xfffafa, "xfffafa".Translate()}, {0x00ff7f, "x00ff7f".Translate()}, {0x4682b4, "x4682b4".Translate()}, {0xd2b48c, "xd2b48c".Translate()}, {0x008080, "x008080".Translate()}, {0xd8bfd8, "xd8bfd8".Translate()}, {0xff6347, "xff6347".Translate()}, {0x40e0d0, "x40e0d0".Translate()}, {0xee82ee, "xee82ee".Translate()}, {0xf5deb3, "xf5deb3".Translate()}, {0xffffff, "xffffff".Translate()}, {0xf5f5f5, "xf5f5f5".Translate()}, {0xffff00, "xffff00".Translate()}, {0x9acd32, "x9acd32".Translate()}
                };

            return;

/*            
            foreach (System.Drawing.KnownColor kc in Enum.GetValues(typeof(System.Drawing.KnownColor)))
            {
                if ((int)kc > 27 && (int)kc < 168)
                // limits based off of https://docs.microsoft.com/en-us/dotnet/api/system.drawing.knowncolor?view=netframework-3.5
                {
                    Log.Error("Found KnownColor: "+(int)kc+", "+kc);
                    System.Drawing.Color c;
                    try {
                        System.Drawing.Color.FromKnownColor(kc);
                        //Log.Message("Got color c: "+c);
                    } catch {
                        Log.Error("Well that failed.");
                    }
                    try
                    {
                        colorMap.Add((uint)(c.ToArgb() & 0x00FFFFFF), c.Name);
                        Log.Message("Added...");
                    }
                    //duplicate colors cause an exception
                    catch { }
                }
            }
            */
        }
    } // end ColorMapper


}
