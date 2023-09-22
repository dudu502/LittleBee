using TrueSync;

public class Const
{
    public const byte CLIENT_SIMULATION_ID = 1;
    public const byte REPLAY_SIMULATION_ID = 2;


    public const byte INPUT_TYPE_KEYBOARD = 0;
    public const byte INPUT_TYPE_JOYSTICK = 1;

    public const string EXTENSION_TYPE_REPLAY = ".rep";
    public const string EXTENSION_TYPE_PATTERN_REPLAY = "*.rep";

    public const string EXTENSION_TYPE_SNAP = ".snap";
    public const string EXTENSION_TYPE_PATTERN_SNAP = "*.snap";

    public const string LAST_REPLAY_FILENAME = "/LastReplay.rep";

    public static readonly FP GRAVITY = 0.3f;
    public static readonly FP MIN_LENGTHSQUARED = 0.1f;
    public static readonly FP MIN_GRAVITY = 0.0001f;
    public static readonly FP MAX_WORLD_SPEED = 1;
}

