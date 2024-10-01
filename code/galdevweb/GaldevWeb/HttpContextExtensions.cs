namespace n3q.FrameworkTools;

public static class HttpContextExtensions
{
    public static string GetRemoteIpAddressHashed(this HttpContext self)
    {
        var ipAddress = self.Connection?.RemoteIpAddress?.ToString();
        if (Is.Value(ipAddress)) {
            var hashedIp = Crc32.Compute(ipAddress).ToString("X8");
            return hashedIp + "-" + ipAddress;
        }
        return "";
    }

    public static string GetUri(this HttpContext self)
    {
        var uri = self.Request?.Path.Value + self.Request?.QueryString.Value;
        if (Is.Value(uri)) {
            return uri;
        }
        return "";
    }
}
