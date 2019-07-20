using System;
using MiniCMS.Domain.Common;

namespace MiniCMS.Domain.Entities
{
    /// <summary>
    /// Represents an audit trail for system actions
    /// </summary>
    public class AuditLog : BaseEntity
    {
        public string Action { get; private set; }
        
        public string Resource { get; private set; }
        
        public string UserId { get; private set; }
        
        public string Details { get; private set; }
        
        public string IpAddress { get; private set; }

        private AuditLog() { }

        public AuditLog(string action, string resource, string userId, string details = null, string ipAddress = null)
        {
            Action = action;
            Resource = resource;
            UserId = userId;
            Details = details;
            IpAddress = ipAddress;
        }
    }
}
