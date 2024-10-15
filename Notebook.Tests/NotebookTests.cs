using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Application.Mapping;
using Notebook.Application.Services.Implementation;
using Notebook.DataAccess;
using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Repositories.Implementation;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Tests
{
    [TestClass]
    public class NotebookTests
    {
        private readonly ServiceManager _serviceManager;
        private readonly DbContextOptions<RepositoryContext> _options;
        private readonly IMapper _mapper;

        public NotebookTests()
        {
            if(_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });

                var mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new RepositoryContext(_options);
            var repositoryManager = new RepositoryManager(context);

            _serviceManager = new ServiceManager(repositoryManager, _mapper);

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
            await _serviceManager.ContactService.CreateContactAsync(new ContactForCreateDTO
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                PhoneNumber = "+380996064050",
                Email = null,
                DateOfBirth = new DateTime(1994, 11, 26)
            });

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }

        [TestMethod]
        public async Task AddNewContactWithoutDataOfBirth()
        {
            await _serviceManager.ContactService.CreateContactAsync(new ContactForCreateDTO
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                PhoneNumber = "+380996064050",
                Email = "test@gmail.com",
                DateOfBirth = null
            });

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }

        [TestMethod]
        public async Task AddNewContactWithoutEmailAndDataOfBirth()
        {
            await _serviceManager.ContactService.CreateContactAsync(new ContactForCreateDTO
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                PhoneNumber = "+380996064050",
                Email = null,
                DateOfBirth = null
            });

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }

        /*[TestMethod]
        public async Task UpdateContact()
        {
            await AddingTESTContactToDB();

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            await _serviceManager.ContactService.UpdateContactAsync(new ContactForUpdateDTO
            {
                Id = contact.Id,
                FirstName = "NewTestFN",
                LastName = "NewTestLN",
                PhoneNumber = "+380996064051",
                Email = "newTest@gmail.com",
                DateOfBirth = null
            });

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("NewTestFN", context.Contacts.Single().FirstName);
            }
        }*/

        /*[TestMethod]
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

            await _serviceManager.AddressService.UpdateAddressAsync(new AddressForUpdateDTO
            {
                Id = address.Id,
                AddressType = 0,
                Country = null,
                Region = null,
                City = null,
                Street = "NewStreet",
                BuildingNumber = 100
            });

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual("Ukraine", context.Addresses.Single().Country);
                Assert.AreEqual("NewStreet", context.Addresses.Single().Street);
            }
        }
*/

        /*[TestMethod]
        public async Task UpdateContactTwoFields()
        {
            await AddingTESTContactToDB();

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            await _serviceManager.ContactService.UpdateContactAsync(new ContactForUpdateDTO
            {
                Id = contact.Id,
                FirstName = "NewTestFN",
                LastName = "NewTestLN",
                PhoneNumber = null,
                Email = null,
                DateOfBirth = null
            });

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("NewTestFN", context.Contacts.Single().FirstName);
                Assert.AreEqual("NewTestLN", context.Contacts.Single().LastName);
            }
        }*/

        /*[TestMethod]
        public async Task UpdateContactWithoutNewFields()
        {
            await AddingTESTContactToDB();

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            await _serviceManager.ContactService.UpdateContactAsync(new ContactForUpdateDTO
            {
                Id = contact.Id,
                FirstName = null,
                LastName = null,
                PhoneNumber = null,
                Email = null,
                DateOfBirth = null
            });

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }*/

        /*[TestMethod]
        public async Task DeleteContact()
        {
            await AddingTESTContactToDB();

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            await _serviceManager.ContactService.DeleteContactAsync(contact);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(0, context.Contacts.Count());
            }
        }*/

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
                await _serviceManager.ContactService.CreateContactAsync(new ContactForCreateDTO
                {
                    FirstName = $"TestFN{i}",
                    LastName = $"TestLN{i}",
                    PhoneNumber = $"+38099606405{i}",
                    Email = $"test{i}@gmail.com",
                    DateOfBirth = date
                });
            }

            var contactParameters = new ContactParameters();
            var allContacts = await _serviceManager.ContactService.GetAllContactsAsync(contactParameters);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(3, context.Contacts.Count());
            }
        }

        /*[TestMethod]
        public async Task GetAllContactsThroughFields()
        {
            AddingTESTContactToDB();


            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
            }
        }*/

        /*[TestMethod]
        public async Task GetAllAddresses()
        {
            for (var i = 0; i < 3; i++)
            {
                await _serviceManager.ContactService.CreateContactAsync(new ContactForCreateDTO
                {
                    FirstName = $"TestFN{i}",
                    LastName = $"TestLN{i}",
                    PhoneNumber = $"+38099606405{i}",
                    Email = $"test{i}@gmail.com",
                    DateOfBirth = new DateTime(1994, 11, 26)
                });
            }

            Contact[] contacts = new Contact[3];

            for (var i = 0; i < contacts.Length; i++)
            {
                contacts[i] = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: $"TestFN{i}",
                lastName: null,
                phoneNumber: null,
                email: null);
            }

            for (var i = 0; i < contacts.Length; i++)
            {
                await _serviceManager.AddressService.CreateAddressAsync(new AddressForCreateDTO
                {
                    AddressType = 0,
                    Country = "Ukraine",
                    Region = "Kyiv Oblast",
                    City = "Kyiv",
                    Street = "Zhulianska",
                    BuildingNumber = i + 1,
                    ContactId = contacts[i].Id
                });
            }

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(3, context.Addresses.Count());
            }
        }*/

        /*[TestMethod]
        public async Task AddNewAddress()
        {
            await AddingTESTAddressToDB();

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual("Zhulianska", context.Addresses.Single().Street);
            }
        }*/

        [TestMethod]
        public async Task AddNewAddressWithoutAnyField()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _serviceManager.AddressService.CreateAddressAsync(new AddressForCreateDTO
                {
                    AddressType = 0,
                    Country = "Ukraine",
                    Region = "Kyiv Oblast",
                    City = "Kyiv",
                    Street = "Zhulianska",
                    BuildingNumber = 1,
                    ContactId = Guid.Empty
                });
            });
        }

        /*[TestMethod]
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

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(0, context.Addresses.Count());
            }
        }*/

        [TestMethod]
        public async Task DeleteAddressNull()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _serviceManager.AddressService.DeleteAddressAsync(null));
        }

        private async Task AddingTESTContactToDB()
        {
            await _serviceManager.ContactService.CreateContactAsync(new ContactForCreateDTO
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                PhoneNumber = "+380996064050",
                Email = "test@gmail.com",
                DateOfBirth = new DateTime(1994, 11, 26)
            });
        }

        /*private async Task AddingTESTAddressToDB()
        {
            AddingTESTContactToDB();

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(
                firstName: "TestFN",
                lastName: null,
                phoneNumber: null,
                email: null);

            await _serviceManager.AddressService.CreateAddressAsync(new AddressForCreateDTO
            {
                AddressType = 0,
                Country = "Ukraine",
                Region = "Kyiv Oblast",
                City = "Kyiv",
                Street = "Zhulianska",
                BuildingNumber = 1,
                ContactId = contact.Id
            });
        }*/
    }
}
