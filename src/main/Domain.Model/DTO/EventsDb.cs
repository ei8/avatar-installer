using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.DTO;

public class NotificationDto
{
    public int SequenceId { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public string? TypeName { get; set; }
    public Guid Id { get; set; }
    public int Version { get; set; }
    public Guid AuthorId { get; set; }
    public string? Data { get; set; }
}