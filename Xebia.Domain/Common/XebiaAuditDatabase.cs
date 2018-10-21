namespace Xebia.DatabaseCore.Common
{
    public class XebiaAuditDatabase
        : XebiaDatabase, IXebiaAuditDatabase
    {
        public XebiaAuditDatabase(IXebiaAuditConnection connection)
            : base(connection)
        {
        }
    }
}
