using NUnit.Framework;
using System.Reflection;
using IoCContainer.Container.Tests.Entities;

namespace IoCContainer.Container.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        private Container container;

        [SetUp]
        public void Setup()
        {
            container = new Container();
        }

        [Test]
        public void AddTypeT_SingleLoggerRegistration_ReturnLogger()
        {
            container.AddType(typeof(Logger));

            var logger = container.Get<Logger>();

            Assert.That(logger, Is.Not.Null);
            Assert.That(logger, Is.InstanceOf<Logger>());
        }

        [Test]
        public void AddTypeTandTBase_SingleICustomerDALRegistration_ReturnICustomerDAL()
        {
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerDAL = container.Get<ICustomerDAL>();

            Assert.That(customerDAL, Is.Not.Null);
            Assert.That(customerDAL, Is.InstanceOf<ICustomerDAL>());
        }

        [Test]
        public void AddTypeTandTBase_ImportConstructor_ReturnCustomerBLL()
        {
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
            container.AddType(typeof(Logger));
            container.AddType(typeof(CustomerBLL));

            var ñustomerBLL = container.Get<CustomerBLL>();

            Assert.That(ñustomerBLL, Is.Not.Null);
            Assert.That(ñustomerBLL, Is.InstanceOf<CustomerBLL>());
        }

        [Test]
        public void GetT_NotRegistred_ThrowsError()
        {
            Assert.That(() => container.Get<ICustomerDAL>(), Throws.Exception);
        }

        [Test]
        public void GetT_NoDependencies_ThrowsError()
        {
            Assert.That(() => container.Get<CustomerBLL>(), Throws.Exception);
        }

        [Test]
        public void GetT_NotEnoughDependenciesForImportConstructor_ThrowsError()
        {
            container.AddType(typeof(Logger));
            Assert.That(() => container.Get<CustomerBLL>(), Throws.Exception);
        }

        [Test]
        public void AddAssembly_ImportConstructor_ReturnCustomerBLL()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var ñustomerBLL = container.Get<CustomerBLL>();

            Assert.That(ñustomerBLL, Is.Not.Null);
            Assert.That(ñustomerBLL, Is.InstanceOf<CustomerBLL>());
        }

        [Test]
        public void AddAssembly_GetLogger_ReturnLogger()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var ñustomerBLL = container.Get<Logger>();

            Assert.That(ñustomerBLL, Is.Not.Null);
            Assert.That(ñustomerBLL, Is.InstanceOf<Logger>());
        }

        [Test]
        public void AddAssembly_GetICustomerDAL_ReturnICustomerDAL()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var ñustomerBLL = container.Get<ICustomerDAL>();

            Assert.That(ñustomerBLL, Is.Not.Null);
            Assert.That(ñustomerBLL, Is.InstanceOf<ICustomerDAL>());
        }
    }
}