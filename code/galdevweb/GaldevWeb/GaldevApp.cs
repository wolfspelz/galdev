namespace GaldevWeb;

public class GaldevApp
{
    public GaldevConfig Config = new GaldevConfig();
    public ICallbackLogger Log = new NullCallbackLogger();
    public ITimeProvider Time = new RealTime();
    public TimelineIndex Timelines = new();
}
