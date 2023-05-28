namespace n3q.Tools
{
    public static class DottedVersion
    {
        // Works until 99999 per component except the major, which is limited to 9999
        private const ulong Factor = 100000;

        public static ulong Long(string s)
        {
            ulong result = 0;
            var parts = s.Split(new[] { '.' });
            for (var i = 0; i < 4; i++)
            {
                ulong upart = 0;
                if (parts.Length >= i + 1) {
                    long.TryParse(parts[i], out var part);
                    if (part >= 0) {
                        upart = (ulong)part;
                    }
                }
                result *= Factor;
                result += upart;
            }
            return result;
        }
    }
}
