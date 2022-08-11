using System.Collections.Generic;

namespace AutSoft.DbScaffolding.EntityAbstractions.Services;

public interface IInterfaceTemplateService
{
    string GenerateInterface(Dictionary<string, object> data);
}
