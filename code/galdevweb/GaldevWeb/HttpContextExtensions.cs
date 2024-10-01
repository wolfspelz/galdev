using System.Net;

namespace n3q.FrameworkTools;

public static class HttpContextExtensions
{
    // Header: X-Real-IP=37.5.240.165
    // Header: X-Forwarded-For=37.5.240.165
    public static string GetRemoteIpAddressHashed(this HttpContext self)
    {
        var ipAddress = "";

        if (!Is.Value(ipAddress)) {
            ipAddress = self.Request.Headers.TryGetValue("X-Real-IP", out var realIpValues) ? realIpValues.FirstOrDefault() : "";
        }
        if (!Is.Value(ipAddress)) {
            ipAddress = self.Request.Headers.TryGetValue("X-Forwarded-For", out var values) ? values.FirstOrDefault() : "";
        }
        if (!Is.Value(ipAddress)) {
            ipAddress = self.Connection?.RemoteIpAddress?.ToString();
        }

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
