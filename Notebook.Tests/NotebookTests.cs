using Microsoft.EntityFrameworkCore;
using Notebook.Application.Services.Implementation;
using Notebook.DataAccess;
using Notebook.Repositories.Implementation;
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
            await AddingTESTContactToDB();

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }

        [TestMethod]
        public async Task AddNewContactWithoutEmail()
        {
            await AddingTESTContactToDB();

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

        [TestMethod]
        public async Task UpdateContact()
        {
            await AddingTESTContactToDB();

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            await _serviceManager.ContactService.UpdateContactAsync(
                contact.Id,
                newFirstName: "NewTestFN",
                newLastName: "NewTestLN",
                newPhoneNumber: "+380996064051",
                newEmail: "newTest@gmail.com",
                newDataOfBirth: null);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("NewTestFN", context.Contacts.Single().FirstName);
            }
        }


        [TestMethod]
        public async Task UpdateContactTwoFields()
        {
            await AddingTESTContactToDB();

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            await _serviceManager.ContactService.UpdateContactAsync(
                contact.Id,
                newFirstName: "NewTestFN",
                newLastName: "NewTestLN",
                newPhoneNumber: null,
                newEmail: null,
                newDataOfBirth: null);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("NewTestFN", context.Contacts.Single().FirstName);
                Assert.AreEqual("NewTestLN", context.Contacts.Single().LastName);
            }
        }

        [TestMethod]
        public async Task UpdateContactWithoutNewFields()
        {
            await AddingTESTContactToDB();

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            await _serviceManager.ContactService.UpdateContactAsync(
                contact.Id,
                newFirstName: null,
                newLastName: null,
                newPhoneNumber: null,
                newEmail: null,
                newDataOfBirth: null);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }

        [TestMethod]
        public async Task DeleteContact()
        {
            await AddingTESTContactToDB();

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            await _serviceManager.ContactService.DeleteContactAsync(contact);

            using(var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(0, context.Contacts.Count());
            }
        }

        [TestMethod]
        public async Task DeleteContactNull()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _serviceManager.ContactService.DeleteContactAsync(null));
        }

        private async Task AddingTESTContactToDB()
        {
            DateTime dt = DateTime.ParseExact("21.05.1994", "dd.MM.yyyy", CultureInfo.InvariantCulture);

            await _serviceManager.ContactService.CreateContactAsync(
                firstName: "TestFN",
                lastName: "TestLN",
                phoneNumber: "+380996064050",
                email: "test@gmail.com",
                dataOfBirth: dt);
        }
    }
}
