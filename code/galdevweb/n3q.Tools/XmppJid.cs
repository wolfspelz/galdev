namespace n3q.Tools
{
    public class XmppJid
    {
        public string Scheme { get; set; } = "xmpp";
        public string User { get; set; } = "";
        public string Domain { get; set; } = "";
        public string Resource { get; set; } = "";
        public string Base => User + (string.IsNullOrEmpty(User) ? "" : "@") + Domain;
        public string Full => Base + (string.IsNullOrEmpty(Resource) ? "" : "/") + Resource;
        public string URI => Scheme + (string.IsNullOrEmpty(Scheme) ? "" : ":") + Full;

        public override string ToString()
        {
            return Full;
        }

        public XmppJid(string jid)
        {
            Parse(jid);
        }

        public XmppJid(string user, string domain)
        {
            User = user;
            Domain = domain;
        }

        public XmppJid(string user, string domain, string resource)
        {
            User = user;
            Domain = domain;
            Resource = resource;
        }

        private void Parse(string jid)
        {
            jid = GetScheme(jid);
            jid = GetUser(jid);
            jid = GetDomain(jid);
            _ = GetResource(jid);
        }

        private string GetScheme(string jid)
        {
            var idx = jid.IndexOf(":");
            if (idx >= 0) {
                Scheme = jid.Substring(0, idx);
                if (Scheme == "jabber") { Scheme = "xmpp"; }
                jid = jid.Substring(idx + 1);
            }
            return jid;
        }

        private string GetUser(string jid)
        {
            var idx = jid.IndexOf("@");
            if (idx >= 0) {
                User = jid.Substring(0, idx);
                jid = jid.Substring(idx + 1);
            }
            return jid;
        }

        private string GetDomain(string jid)
        {
            var idx = jid.IndexOf("/");
            if (idx >= 0) {
                Domain = jid.Substring(0, idx);
                jid = jid.Substring(idx + 1);
            } else {
                var idxSlash = jid.IndexOf("/");
                if (idxSlash < 0) {
                    Domain = jid;
                    jid = "";
                }
            }
            return jid;
        }

        private string GetResource(string jid)
        {
            Resource = jid;
            if (string.IsNullOrEmpty(User) && string.IsNullOrEmpty(Domain) && !string.IsNullOrEmpty(Resource)) {
                Domain = Resource;
                Resource = "";
            }
            return "";
        }
    }
}
