using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.NUnit;
using NUnit.Framework;
using System;
using System.Linq;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Identity.Tests.Architecture
{
    [TestFixture]
    public class ArchitecturalTest
    {
        [Test]
        public void TestThatDomainLayerIsntVisibleFromOtherLayersExceptExceptions()
        {
            System.Reflection.Assembly coreAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(f => f.GetName().Name == "Identity.Core");
            ArchUnitNET.Domain.Architecture architecture = new ArchLoader()
                .LoadAssemblies(coreAssembly)
                .Build();

            IArchRule archRule = Classes()
                .That()
                .ResideInAssembly("Identity.Core")
                .And()
                .ResideInNamespace("Identity.Core.Domain")
                .And()
                .DoNotDependOnAny(typeof(Exception))
                .Should()
                .NotBePublic();

            archRule.Check(architecture);
        }
    }
}