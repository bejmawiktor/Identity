using Identity.Domain;
using Identity.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class PermissionHolderTest
    {
        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            var addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            var permissions = new PermissionId[]
            {
                addSomethingPermission
            };
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid(), permissions);

            Assert.That(permissionHolder.Permissions, Is.EqualTo(permissions));
        }

        [Test]
        public void TestConstructor_WhenPermissionsNotGiven_ThenPermissionsAreSetAsEmpty()
        {
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid());

            Assert.That(permissionHolder.Permissions, Is.Empty);
        }

        [Test]
        public void TestIsPermitedTo_WhenNullPermissionIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid());

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissionId"),
                () => permissionHolder.IsPermittedTo(null));
        }

        [Test]
        public void TestIsPermitedTo_WhenHolderHasPermission_ThenTrueIsReturned()
        {
            var addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            var permissions = new PermissionId[]
            {
                addSomethingPermission
            };
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid(), permissions);

            Assert.That(permissionHolder.IsPermittedTo(addSomethingPermission), Is.True);
        }

        [Test]
        public void TestIsPermitedTo_WhenHolderHasntPermission_ThenFalseIsReturned()
        {
            var addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid());

            Assert.That(permissionHolder.IsPermittedTo(addSomethingPermission), Is.False);
        }

        [Test]
        public void TestObtainPermission_WhenNullPermissionIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid());

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissionId"),
                () => permissionHolder.ObtainPermission(null));
        }

        [Test]
        public void TestObtainPermission_WhenHolderWasPermissionWasAlreadyObtained_ThenInvalidOperationIsThrown()
        {
            var addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            var permissions = new PermissionId[]
            {
                addSomethingPermission
            };
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid(), permissions);

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Permission was already obtained."),
                () => permissionHolder.ObtainPermission(addSomethingPermission));
        }

        [Test]
        public void TestObtainPermission_WhenPermissionGiven_ThenHolderHasPermission()
        {
            var addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid());

            permissionHolder.ObtainPermission(addSomethingPermission);

            Assert.That(permissionHolder.IsPermittedTo(addSomethingPermission), Is.True);
        }

        [Test]
        public void TestRevokePermission_WhenNullPermissionIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid());

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissionId"),
                () => permissionHolder.RevokePermission(null));
        }

        [Test]
        public void TestRevokePermission_WhenHolderHasntObtainedGivenPermission_ThenInvalidOperationIsThrown()
        {
            var addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid());

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Permission wasn't obtained."),
                () => permissionHolder.RevokePermission(addSomethingPermission));
        }

        [Test]
        public void TestRevokePermission_WhenPermissionIdGiven_ThenPermissionIsRevoked()
        {
            var addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            var permissions = new PermissionId[]
            {
                addSomethingPermission
            };
            var permissionHolder = new PermissionHolderStub(Guid.NewGuid(), permissions);

            permissionHolder.RevokePermission(addSomethingPermission);

            Assert.That(permissionHolder.IsPermittedTo(addSomethingPermission), Is.False);
        }
    }
}