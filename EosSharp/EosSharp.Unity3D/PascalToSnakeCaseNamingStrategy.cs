using EosSharp.Core.Helpers;
using Newtonsoft.Json.Serialization;

namespace EosSharp.Unity3D
{
    public class PascalToSnakeCaseNamingStrategy : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            return SerializationHelper.PascalCaseToSnakeCase(name);
        }
    }
}