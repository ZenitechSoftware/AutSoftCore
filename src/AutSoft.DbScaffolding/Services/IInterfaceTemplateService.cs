using System.Collections.Generic;

namespace AutSoft.DbScaffolding.Services;

internal interface IInterfaceTemplateService
{
    string GenerateInterface(Dictionary<string, object> data);
}
