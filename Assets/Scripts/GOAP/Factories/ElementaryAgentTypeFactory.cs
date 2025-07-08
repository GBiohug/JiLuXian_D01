using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace GOAP.Factories
{
    public class ElementaryAgentTypeFactory:AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var factory = new AgentTypeBuilder("ElementaryAgent01");
            factory.AddCapability<ElementaryCapability>();
            return factory.Build();
        }
    }
}