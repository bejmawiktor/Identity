﻿using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class ResourceIdTest
    {
        [Test]
        public void TestConstructor_WhenEmptyNameGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Resource id can't be empty."),
              () => new ResourceId(string.Empty));
        }

        [TestCase("My resource")]
        [TestCase("My$resource")]
        [TestCase("$%^MyResource!@#")]
        [TestCase("My resource 1")]
        [TestCase("My resource 1 !@")]
        public void TestConstructor_WhenIncorrectNameGiven_ThenArgumentExceptionIsThrown(string name)
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Resource id must contain only alphanumeric characters without spaces."),
              () => new ResourceId(name));
        }

        [TestCase("MyResource")]
        [TestCase("example1resource")]
        [TestCase("GetProducts")]
        [TestCase("asdghxcbnm123456890asdggh")]
        public void TestConstructor_WhenCorrectNameGiven_ThenNameIsSet(string name)
        {
            ResourceId resourceId = new(name);

            Assert.That(resourceId.ToString(), Is.EqualTo(name));
        }
    }
}