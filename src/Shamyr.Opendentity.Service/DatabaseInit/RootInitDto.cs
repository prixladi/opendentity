using System;
using System.Collections.Generic;

namespace Shamyr.Opendentity.Service.DatabaseInit
{
    public record RootInitDto
    {
        public ICollection<UserInitDto> Users { get; init; } = Array.Empty<UserInitDto>();
        public ICollection<ApplicationInitDto> Applications { get; init; } = Array.Empty<ApplicationInitDto>();
    }
}
