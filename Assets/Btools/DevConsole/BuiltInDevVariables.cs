using System.Collections.Generic;
using UnityEngine;

namespace Btools.DevConsole
{
    internal class BuiltInDevVariables
    {
        public static Dictionary<string, DevConsoleVariable> Builtins => new Dictionary<string, DevConsoleVariable>()
        {
            {"", new DevConsoleVariable(
                "",
                "empty variable",
                typeof(string), () => "") },

            {"null", new DevConsoleVariable(
                "null",
                "null (nothing) or empty",
                typeof(object), () => null) },

            {"true", new DevConsoleVariable(
                "true",
                "true",
                typeof(bool), () => true
                ) },

            {"false", new DevConsoleVariable(
                "false",
                "false",
                typeof(bool), () => false
                ) },

            {"timescale", new DevConsoleVariable(
                "timescale",
                "timescale that unity runs at. default = 1",
                typeof(float), () => Time.timeScale, x => Time.timeScale = float.Parse(x)) },

            {"maxfps", new DevConsoleVariable(
                "maxfps",
                "if the framerate runs quicker, pause execution until target framerate met.\nuncapped = -1",
                typeof(int), () => Application.targetFrameRate, x => Application.targetFrameRate = int.Parse(x)) },

            {"fps", new DevConsoleVariable(
                "fps",
                "1 / deltatime",
                typeof(float), () => 1 / Time.deltaTime) },

            {"deltatime", new DevConsoleVariable(
                "deltatime",
                "time since last frame",
                typeof(float), () => Time.deltaTime) },

            {"fixed_delta_time", new DevConsoleVariable("fixed_delta_time",
                "the time between frames for fixed update",
                typeof(float), () => Time.fixedDeltaTime, x => Time.fixedDeltaTime = float.Parse(x)) },

            {"fixed_fps", new DevConsoleVariable("fixed_fps", "1 / fixed_delta_time",
                typeof(float), () => 1 / Time.fixedDeltaTime, x => Time.fixedDeltaTime = 1 / float.Parse(x)) },

            {"cam_size", new DevConsoleVariable(
                "cam_size", 
                "camera orthographic size for default camera", 
                typeof(float), () => Camera.main.orthographicSize, x => Camera.main.orthographicSize = float.Parse(x)) },

            {"platform", new DevConsoleVariable(
                "platform",
                "the operating system that the current user is using",
                typeof(RuntimePlatform), () => Application.platform.ToString()) },

            {"unity_pro_license", new DevConsoleVariable(
                "unity_pro_license",
                "does the user have unity pro license", 
                typeof(bool), () => Application.HasProLicense()) },

            {"version", new DevConsoleVariable(
                "version",
                "the version of the current application",
                typeof(string), () => Application.version) },

            {"unity_version", new DevConsoleVariable(
                "unity_version",
                "the version of unity that the project uses",
                typeof(string), () => Application.unityVersion) },

            {"system_lang", new DevConsoleVariable(
                "system_lang",
                "the language of the operating system",
                typeof(SystemLanguage), () => Application.systemLanguage) },

            {"run_in_background", new DevConsoleVariable(
                "run_in_background",
                "can the current project run when not focused",
                typeof(bool), () => Application.runInBackground, x => Application.runInBackground = bool.Parse(x)) },

            {"product_name", new DevConsoleVariable(
                "product_name",
                "the name of the product",
                typeof(string), () => Application.productName) },

            {"focused", new DevConsoleVariable(
                "focused",
                "is the current application focused",
                typeof(string), () => Application.isFocused) },

            {"company", new DevConsoleVariable(
                "company", 
                "the company that made the game", 
                typeof(string), () => Application.companyName) },

            {"splash_screen_on", new DevConsoleVariable(
                "splash_screen_on", "is the unity splash screen showing",
                typeof(string), () => !UnityEngine.Rendering.SplashScreen.isFinished) },

            {"glitch_variable", new DevConsoleVariable(
                "glitch_variable",
                "a variable that throws an error when attempting to get it's value",
                typeof(void), null) },

            {"elapsed_time", new DevConsoleVariable(
                "elapsed_time",
                "the total time elapsed since the beginning of the game",
                typeof(float), () => Time.time)},

            {"frame_count", new DevConsoleVariable(
                "frame_count",
                "the frames elapsed since the beginning of the game",
                typeof(int), () => Time.frameCount)},

            {"current_date", new DevConsoleVariable(
                "current_date",
                "the date and time",
                typeof(System.DateTime), () => System.DateTime.Now)},

            {"current_date_utc", new DevConsoleVariable(
                "current_date",
                "the date and time in universal time",
                typeof(System.DateTime), () => System.DateTime.UtcNow)},

            {"$", new DevConsoleVariable(
                "$",
                "the dollar symbol",
                typeof(string),
                () =>"$"
                )},

            {"dollar", new DevConsoleVariable(
                "dollar",
                "the dollar symbol (alias for $)",
                typeof(string),
                () =>"$"
                )},

            {"inf", new DevConsoleVariable(
                "inf",
                "infinity",
                typeof(float),
                () => float.PositiveInfinity
                )},

            {"-inf", new DevConsoleVariable(
                "-inf",
                "negative infinity",
                typeof(float),
                () => float.NegativeInfinity
                )},

            {"nan", new DevConsoleVariable(
                "nan",
                "nan (not a number)",
                typeof(float),
                () => float.NaN
                )},

            {"epsilon", new DevConsoleVariable(
                "epsilon",
                "float epsilon",
                typeof(float),
                () => float.Epsilon
                )},

            #region Integer Limits

            {"floatmax", new DevConsoleVariable(
                "floatmax",
                $"float maximum value ({float.MaxValue})",
                typeof(float),
                () => float.MaxValue
                ) },

            {"floatmin", new DevConsoleVariable(
                "floatmin",
                $"float minimum value ({float.MinValue})",
                typeof(float),
                () => float.MinValue
                ) },

            {"doublemax", new DevConsoleVariable(
                "doublemax",
                $"double maximum value ({double.MaxValue})",
                typeof(double),
                () => double.MaxValue
                ) },

            {"doublemin", new DevConsoleVariable(
                "doublemin",
                $"double minimum value ({double.MinValue})",
                typeof(double),
                () => double.MinValue
                ) },

            {"int8max", new DevConsoleVariable(
                "int8max",
                "the highest value of an signed 8 bit number",
                typeof(sbyte),
                () => sbyte.MaxValue
                )},

            {"int8min", new DevConsoleVariable(
                "int8min",
                "the lowest value of an signed 8 bit number",
                typeof(sbyte),
                () => sbyte.MinValue
                )},

            {"uint8max", new DevConsoleVariable(
                "uint8max",
                "the highest value of an unsigned 8 bit number",
                typeof(byte),
                () => byte.MaxValue
                )},

            {"int16min", new DevConsoleVariable(
                "int16min",
                "the lowest value of a signed 16 bit number",
                typeof(short),
                () => short.MinValue
                )},

            {"int16max", new DevConsoleVariable(
                "int16max",
                "the highest value of a signed 16 bit number",
                typeof(short),
                () => short.MaxValue
                )},

            {"uint16max", new DevConsoleVariable(
                "uint16max",
                "the highest value of an unsigned 16 bit number",
                typeof(ushort),
                () => ushort.MaxValue
                )},

            {"int32min", new DevConsoleVariable(
                "int32min",
                "the lowest value of a signed 32 bit number",
                typeof(int),
                () => int.MinValue
                )},

            {"int32max", new DevConsoleVariable(
                "int32max",
                "the highest value of a signed 32 bit number",
                typeof(int),
                () => int.MaxValue
                )},

            {"uint32max", new DevConsoleVariable(
                "uint32max",
                "the highest value of an unsigned 32 bit number",
                typeof(uint),
                () => uint.MaxValue
                )},

            {"int64min", new DevConsoleVariable(
                "int64min",
                "the lowest value of a signed 64 bit number",
                typeof(long),
                () => long.MinValue
                )},

            {"int64max", new DevConsoleVariable(
                "int64max",
                "the highest value of a signed 64 bit number",
                typeof(long),
                () => long.MaxValue
                )},

            {"uint64max", new DevConsoleVariable(
                "uint64max",
                "the highest value of an unsigned 64 bit number",
                typeof(ulong),
                () => ulong.MaxValue
                )},
#endregion
        };
    }
}
