namespace GaldevWeb
{
    public class MyApp
    {
        public MyConfig Config = new MyConfig();
        public ICallbackLogger Log = new NullCallbackLogger();
        public ITimeProvider Time = new RealTime();
    }
}
