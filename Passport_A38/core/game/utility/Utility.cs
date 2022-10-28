using System.Text;

namespace Passport_A38.core.game.utility;

public static class Utility
{

    /*
     * Parses an integer value to its roman counterpart.
    */
    public static string IntToRoman(int x)
    {
        var map = new Dictionary<int, string>
        {
            {1000,"M"},
            {900, "CM"},
            {500,"D"},
            {400, "CD"},
            {100,"C"},
            {90, "XC"},
            {50,"L"},
            {40, "XL"},
            {10,"X"},
            {9, "IX"},
            {5,"I"},
            {4, "IV"},
            {1,"I"}
        };
        var roman = new StringBuilder();
        
        while (x>0)
        {
            for (var i = 0; i < map.Count; i++)
            {
                if (x / map.Keys.ToArray()[i] == 0) continue;   //number doesn't fit
                
                x -= map.Keys.ToArray()[i];
                roman.Append(map.Values.ToArray()[i]);
                break;
            }
        }
        return roman.ToString();
    }
}