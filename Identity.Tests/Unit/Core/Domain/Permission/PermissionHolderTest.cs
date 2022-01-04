using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.TestDoubles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class PermissionHolderTest
    {
        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            PermissionId addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            PermissionId[] permissions = new PermissionId[]
            {
                addSomethingPermission
            };
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub(permissions: permissions);

            Assert.That(permissionHolder.Permissions, Is.EqualTo(permissions));
        }

        private PermissionHolderStub GetPermissionHolderStub(
            Guid? id = null,
            IEnumerable<PermissionId> permissions = null)
        {
            PermissionId[] permissionsReplacement = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "AddSomething")
            };

            return new PermissionHolderStub(
                id ?? Guid.NewGuid(),
                permissions ?? permissionsReplacement);
        }

        [Test]
        public void TestConstructor_WhenPermissionsNotGiven_ThenPermissionsAreSetAsEmpty()
        {
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub(
                permissions: Enumerable.Empty<PermissionId>());

            Assert.That(permissionHolder.Permissions, Is.Empty);
        }

        [Test]
        public void TestIsPermitedTo_WhenNullPermissionIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissionId"),
                () => permissionHolder.IsPermittedTo(null));
        }

        [Test]
        public void TestIsPermitedTo_WhenHolderHasPermission_ThenTrueIsReturned()
        {
            PermissionId addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            PermissionId[] permissions = new PermissionId[]
            {
                addSomethingPermission
            };
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub(permissions: permissions);

            Assert.That(permissionHolder.IsPermittedTo(addSomethingPermission), Is.True);
        }

        [Test]
        public void TestIsPermitedTo_WhenHolderHasntPermission_ThenFalseIsReturned()
        {
            PermissionId addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub(
                permissions: Enumerable.Empty<PermissionId>());

            Assert.That(permissionHolder.IsPermittedTo(addSomethingPermission), Is.False);
        }

        [Test]
        public void TestObtainPermission_WhenNullPermissionIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissionId"),
                () => permissionHolder.ObtainPermission(null));
        }

        [Test]
        public void TestObtainPermission_WhenHolderWasPermissionWasAlreadyObtained_ThenInvalidOperationIsThrown()
        {
            PermissionId addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            PermissionId[] permissions = new PermissionId[]
            {
                addSomethingPermission
            };
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub(permissions: permissions);

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Permission was already obtained."),
                () => permissionHolder.ObtainPermission(addSomethingPermission));
        }

        [Test]
        public void TestObtainPermission_WhenPermissionGiven_ThenHolderHasPermission()
        {
            PermissionId addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub(
                permissions: Enumerable.Empty<PermissionId>());

            permissionHolder.ObtainPermission(addSomethingPermission);

            Assert.That(permissionHolder.IsPermittedTo(addSomethingPermission), Is.True);
        }

        [Test]
        public void TestRevokePermission_WhenNullPermissionIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissionId"),
                () => permissionHolder.RevokePermission(null));
        }

        [Test]
        public void TestRevokePermission_WhenHolderHasntObtainedGivenPermission_ThenInvalidOperationIsThrown()
        {
            PermissionId addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub(
                permissions: Enumerable.Empty<PermissionId>());

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Permission wasn't obtained."),
                () => permissionHolder.RevokePermission(addSomethingPermission));
        }

        [Test]
        public void TestRevokePermission_WhenPermissionIdGiven_ThenPermissionIsRevoked()
        {
            PermissionId addSomethingPermission = new PermissionId(new ResourceId("MyResource"), "AddSomething");
            PermissionId[] permissions = new PermissionId[]
            {
                addSomethingPermission
            };
            PermissionHolderStub permissionHolder = this.GetPermissionHolderStub(permissions: permissions);

            permissionHolder.RevokePermission(addSomethingPermission);

            Assert.That(permissionHolder.IsPermittedTo(addSomethingPermission), Is.False);
        }
    }
}