using System;
using System.Text;
using Random = UnityEngine.Random;

namespace Content.Scripts.Anticheat
{
    public static class ObfuseExtensions
    {
        private static int random = -25125;

        public static void Init()
        {
            if (random == -25125)
            {
                random = Random.Range(-999, 999);
            }
        }

        public static float Obf(this float f) => f + random;
        public static float ObfUn(this float f) => f - random;
        
        
        
        public static int Obf(this int f) => f + random;
        public static int ObfUn(this int f) => f - random;


        public static string Obf(this string f)
        {
            if (f == null) return null;
            return f.EncodeBase64();
        }

        public static string ObfUn(this string f)
        {
            if (f == null) return null;
            return f.DecodeBase64();
        }


        public static float ObfAdd(this ref float f, float value)
        {
            f = (f.ObfUn() + value).Obf();
            return f;
        }
        public static int ObfAdd(this ref int f, int value)
        {
            f = (f.ObfUn() + value).Obf();
            return f;
        }
        
        
        public static string EncodeBase64(this string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }

        public static string DecodeBase64(this string value)
        {
            var valueBytes = System.Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }
    }
}
