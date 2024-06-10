using Microsoft.EntityFrameworkCore;
using Notebook.Application.Services.Implementation;
using Notebook.DataAccess;
using Notebook.Domain.Entities;
using Notebook.Repositories.Implementation;
using Notebook.Shared.RequestFeatures;
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
        public async Task UpdateAddress()
        {
            await AddingTESTAddressToDB();

            var address = await _serviceManager.AddressService.GetAddressByFields(
                contactId: null,
                addressType: null,
                country: "Ukraine",
                region: null,
                city: "Kyiv",
                street: null,
                buildingNumber: 1);

            await _serviceManager.AddressService.UpdateAddressAsync(
                id: address.Id,
                addressType: 0,
                country: null,
                region: null,
                city: null,
                street: "NewStreet",
                buildingNumber: 100);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual("Ukraine", context.Addresses.Single().Country);
                Assert.AreEqual("NewStreet", context.Addresses.Single().Street);
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

        [TestMethod]
        public async Task GetAllContacts()
        {
            var date = new DateTime(1994, 11, 26);

            for (var i = 0; i < 3; i++)
            {
                await _serviceManager.ContactService.CreateContactAsync(
                firstName: $"TestFN{i}",
                lastName: $"TestLN{i}",
                phoneNumber: $"+38099606405{i}",
                email: $"test{i}@gmail.com",
                dataOfBirth: date);
            }

            var contactParameters = new ContactParameters();
            var allContacts = await _serviceManager.ContactService.GetAllContactsAsync(contactParameters);

            using(var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(3, context.Contacts.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAddresses()
        {
            for (var i = 0; i < 3; i++)
            {
                await _serviceManager.ContactService.CreateContactAsync(
                firstName: $"TestFN{i}",
                lastName: $"TestLN{i}",
                phoneNumber: $"+38099606405{i}",
                email: $"test{i}@gmail.com",
                dataOfBirth: new DateTime(1994, 11, 26));
            }
            
            Contact[] contacts = new Contact[3];

            for(var i = 0; i < contacts.Length; i++)
            {
                contacts[i] = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: $"TestFN{i}",
                lastName: null,
                phoneNumber: null,
                email: null);
            }

            for(var i = 0; i < contacts.Length; i++)
            {
                await _serviceManager.AddressService.CreateAddressAsync(
                addressType: 0,
                country: "Ukraine",
                region: "Kyiv Oblast",
                city: "Kyiv",
                street: "Zhulianska",
                buildingNumber: i + 1,
                contactId: contacts[i].Id);
            }

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(3, context.Addresses.Count());
            }
        }

        [TestMethod]
        public async Task AddNewAddress()
        {
            await AddingTESTAddressToDB();

            using(var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual("Zhulianska", context.Addresses.Single().Street);
            }
        }

        [TestMethod]
        public async Task AddNewAddressWithoutAnyField()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _serviceManager.AddressService.CreateAddressAsync(
                addressType: 0,
                country: "Ukraine",
                region: "Kyiv Oblast",
                city: "Kyiv",
                street: "Zhulianska",
                buildingNumber: 1,
                contactId: Guid.Empty));
        }

        [TestMethod]
        public async Task DeleteAddress()
        {
            await AddingTESTAddressToDB();

            var address = await _serviceManager.AddressService.GetAddressByFields(
                addressType: null,
                country: "Ukraine",
                region: null,
                city: "Kyiv",
                street: "Zhulianska",
                buildingNumber: null,
                contactId: null);

            await _serviceManager.AddressService.DeleteAddressAsync(address);

            using( var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(0, context.Addresses.Count());
            }
        }

        [TestMethod]
        public async Task DeleteAddressNull()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _serviceManager.AddressService.DeleteAddressAsync(null));
        }

        private async Task AddingTESTContactToDB()
        {
            await _serviceManager.ContactService.CreateContactAsync(
                firstName: "TestFN",
                lastName: "TestLN",
                phoneNumber: "+380996064050",
                email: "test@gmail.com",
                dataOfBirth: new DateTime(1994, 11, 26));
        }

        private async Task AddingTESTAddressToDB()
        {
            AddingTESTContactToDB();

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            await _serviceManager.AddressService.CreateAddressAsync(
                addressType: 0,
                country: "Ukraine",
                region: "Kyiv Oblast",
                city: "Kyiv",
                street: "Zhulianska",
                buildingNumber: 1,
                contactId: contact.Id);
        }
    }
}
