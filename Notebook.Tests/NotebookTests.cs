using Microsoft.EntityFrameworkCore;
using Notebook.Application.Services.Implementation;
using Notebook.DataAccess;
using Notebook.Repositories.Implementation;
using System.Data;
using System.Globalization;

namespace Notebook.Tests
{
    [TestClass]
    public class NotebookTests
    {
        private readonly ServiceManager _serviceManager;
        private readonly DbContextOptions<RepositoryContext> _options;

        public NotebookTests()
        {
            _options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new RepositoryContext(_options);
            var repositoryManager = new RepositoryManager(context);

            _serviceManager = new ServiceManager(repositoryManager);

            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task AddNewContact()
        {
            DateTime dt = DateTime.ParseExact("21.05.1994", "dd.MM.yyyy", CultureInfo.InvariantCulture);

            await _serviceManager.ContactService.CreateContactAsync(
                firstName: "TestFN",
                lastName: "TestLN",
                phoneNumber: "+380996064050",
                email: "test@gmail.com",
                dataOfBirth: dt);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }

        [TestMethod]
        public async Task AddNewContactWithoutEmail()
        {
            DateTime dt = DateTime.ParseExact("21.05.1994", "dd.MM.yyyy", CultureInfo.InvariantCulture);

            await _serviceManager.ContactService.CreateContactAsync(
                firstName: "TestFN",
                lastName: "TestLN",
                phoneNumber: "+380996064050",
                email: null,
                dataOfBirth: dt);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }

        [TestMethod]
        public async Task AddNewContactWithoutDataOfBirth()
        {
            await _serviceManager.ContactService.CreateContactAsync(
                firstName: "TestFN",
                lastName: "TestLN",
                phoneNumber: "+380996064050",
                email: "test@gmail.com",
                dataOfBirth: null);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }

        [TestMethod]
        public async Task AddNewContactWithoutEmailAndDataOfBirth()
        {
            DateTime dt = DateTime.ParseExact("21.05.1994", "dd.MM.yyyy", CultureInfo.InvariantCulture);

            await _serviceManager.ContactService.CreateContactAsync(
                firstName: "TestFN",
                lastName: "TestLN",
                phoneNumber: "+380996064050",
                email: null,
                dataOfBirth: null);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }
    }
}
