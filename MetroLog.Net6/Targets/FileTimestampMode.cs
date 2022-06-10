namespace MetroLog.Targets;

[Flags]
public enum FileTimestampMode
{
    None = 0,
    Date = 1,
    Time = 2,
    DateTime = Date | Time
}