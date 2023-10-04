using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.Configuration
{
    public class JsonConfigurationRepository : IConfigurationRepository
    {
        public Task<IEnumerable<AvatarConfiguration>> ReadAllAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}
