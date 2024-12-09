using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Application.Mapping;
using Notebook.Application.Services.Implementation;
using Notebook.DataAccess;
using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Domain.Responses;
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
            if (_mapper == null)
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
            await _serviceManager.ContactService.CreateContactAsync(new CreateContactRequest
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
            await _serviceManager.ContactService.CreateContactAsync(new CreateContactRequest
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
            await _serviceManager.ContactService.CreateContactAsync(new CreateContactRequest
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

        [TestMethod]
        public async Task DeleteContact()
        {
            var contact = new Contact()
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                PhoneNumber = "+380996064050",
                Email = "test@gmail.com",
                DateOfBirth = new DateTime(1994, 11, 26)
            };

            var deleteContact = await _serviceManager.ContactService.GetContactByIdAsync(contact.Id);

            if(deleteContact != null)
            {
                await _serviceManager.ContactService.DeleteContactAsync(deleteContact);

                using (var context = new RepositoryContext(_options))
                {
                    Assert.AreEqual(0, context.Contacts.Count());
                }
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
                await _serviceManager.ContactService.CreateContactAsync(new CreateContactRequest
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

        [TestMethod]
        public async Task GetAllContactsThroughFields()
        {
            var date = new DateTime(1994, 11, 26);

            await _serviceManager.ContactService.CreateContactAsync(
                new CreateContactRequest
                {
                    FirstName = $"TestFN1",
                    LastName = $"TestLN1",
                    PhoneNumber = $"+380996064051",
                    Email = $"test1@gmail.com",
                    DateOfBirth = date
                });

            await _serviceManager.ContactService.CreateContactAsync(
                new CreateContactRequest
                {
                    FirstName = $"TestFN1",
                    LastName = $"TestLN2",
                    PhoneNumber = $"+380996064052",
                    Email = $"test2@gmail.com",
                    DateOfBirth = date
                });

            await _serviceManager.ContactService.CreateContactAsync(
                new CreateContactRequest
                {
                    FirstName = $"TestFN3",
                    LastName = $"TestLN3",
                    PhoneNumber = $"+380996064053",
                    Email = $"test3@gmail.com",
                    DateOfBirth = date
                });


            var contactRequest = new GetContactRequest();
            contactRequest.FirstName = "TestFN1";

            if (contactRequest != null)
            {
                var contact = await _serviceManager.ContactService.GetContactByFieldAsync(contactRequest);

                using (var context = new RepositoryContext(_options))
                {
                    Assert.AreEqual(contact.Count(), 2);
                }
            }
        }

        [TestMethod]
        public async Task UpdateContact()
        {
            await AddingTESTContactToDB();

            var contactRequest = new GetContactRequest()
            {
                FirstName = "TestFN"
            };

            var getExistContact = await _serviceManager.ContactService.GetContactByFieldAsync(contactRequest);

            var newContactRequest = new UpdateContactRequest()
            {
                FirstName = "TestNewFirstName",
                LastName = "TestNewLastName",
                PhoneNumber = "+380996064050",
                Email = "test@gmail.com",
                DateOfBirth = new DateTime(1994, 11, 26)
            };

            await _serviceManager.ContactService.UpdateContactAsync(getExistContact.ToList().First().Id, newContactRequest);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestNewFirstName", context.Contacts.Single().FirstName);
                Assert.AreEqual("TestNewLastName", context.Contacts.Single().LastName);
                Assert.AreEqual("+380996064050", context.Contacts.Single().PhoneNumber);
                Assert.AreEqual("test@gmail.com", context.Contacts.Single().Email);
                Assert.AreEqual(newContactRequest.DateOfBirth, context.Contacts.Single().DateOfBirth);
            }
        }

        [TestMethod]
        public async Task UpdateContactAFewFieldsAreEmpty()
        {
            await AddingTESTContactToDB();

            var contactRequest = new GetContactRequest()
            {
                FirstName = "TestFN"
            };

            var getExistContact = await _serviceManager.ContactService.GetContactByFieldAsync(contactRequest);

            var newContactRequest = new UpdateContactRequest()
            {
                FirstName = "TestNewFirstName",
                LastName = "",
                PhoneNumber = "",
                Email = "test2@gmail.com"
            };

            await _serviceManager.ContactService.UpdateContactAsync(getExistContact.ToList().First().Id, newContactRequest);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual("TestNewFirstName", context.Contacts.Single().FirstName);
                Assert.AreEqual("", context.Contacts.Single().LastName);
                Assert.AreEqual("", context.Contacts.Single().PhoneNumber);
                Assert.AreEqual("test2@gmail.com", context.Contacts.Single().Email);
            }
        }

        [TestMethod]
        public async Task UpdateContactWithoutNewFields() 
        {
            await AddingTESTContactToDB();

            var contactRequest = new GetContactRequest()
            {
                FirstName = "TestFN"
            };

            var getExistContact = await _serviceManager.ContactService.GetContactByFieldAsync(contactRequest);

            var newContactRequest = new UpdateContactRequest();

            try
            {
                await _serviceManager.ContactService.UpdateContactAsync(getExistContact.ToList().First().Id, newContactRequest);
            }
            catch
            {
                using (var context = new RepositoryContext(_options))
                {
                    Assert.AreEqual(1, context.Contacts.Count());
                    Assert.AreEqual("TestFN", context.Contacts.Single().FirstName);
                    Assert.AreEqual("TestLN", context.Contacts.Single().LastName);
                    Assert.AreEqual("+380996064050", context.Contacts.Single().PhoneNumber);
                    Assert.AreEqual("test@gmail.com", context.Contacts.Single().Email);
                }
            }
        }

        private async Task AddingTESTContactToDB()
        {
            await _serviceManager.ContactService.CreateContactAsync(new CreateContactRequest
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                PhoneNumber = "+380996064050",
                Email = "test@gmail.com",
                DateOfBirth = new DateTime(1994, 11, 26)
            });
        }

        [TestMethod]
        public async Task AddNewAddress()
        {
            await AddingTESTAddressToDB();

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual("Zhulianska", context.Addresses.Single().Street);
            }
        }

        [TestMethod]
        public async Task UpdateAddress()
        {
            await AddingTESTAddressToDB();

            var addressRequest = new GetAddressRequest
            {
                Country = "Ukraine",
                Region = "Kyiv Oblast",
                City = "Kyiv"
            };

            var existAddress = await _serviceManager.AddressService.GetAddressByFieldsAsync(addressRequest);

            var updateAddressRequest = new UpdateAddressRequest
            {
                Id = existAddress.First().Id,
                AddressType = (Domain.AddressType?)1,
                Country = "USA",
                Region = "California",
                City = "Los-Angeles",
                Street = "Jack Black",
                BuildingNumber = 100
            };

            await _serviceManager.AddressService.UpdateAddressAsync(updateAddressRequest);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual("USA", context.Addresses.Single().Country);
                Assert.AreEqual("California", context.Addresses.Single().Region);
                Assert.AreEqual("Los-Angeles", context.Addresses.Single().City);
                Assert.AreEqual("Jack Black", context.Addresses.Single().Street);
            }
        }

        [TestMethod]
        public async Task UpdateAddressAFewFieldsAreEmpty()
        {
            await AddingTESTAddressToDB();

            var addressRequest = new GetAddressRequest
            {
                Country = "Ukraine",
                Region = "Kyiv Oblast",
                City = "Kyiv"
            };

            var existAddress = await _serviceManager.AddressService.GetAddressByFieldsAsync(addressRequest);

            var updateAddressRequest = new UpdateAddressRequest
            {
                Id = existAddress.First().Id,
                AddressType = (Domain.AddressType?)1,
                Country = "",
                Region = "",
                City = "Los-Angeles",
                Street = "",
                BuildingNumber = 100
            };

            await _serviceManager.AddressService.UpdateAddressAsync(updateAddressRequest);

            using (var context = new RepositoryContext(_options))
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual("", context.Addresses.Single().Country);
                Assert.AreEqual("", context.Addresses.Single().Region);
                Assert.AreEqual("Los-Angeles", context.Addresses.Single().City);
                Assert.AreEqual("", context.Addresses.Single().Street);
            }
        }

        [TestMethod]
        public async Task UpdateAddressWithoutNewFields()
        {
            await AddingTESTAddressToDB();

            var addressRequest = new GetAddressRequest
            {
                Country = "Ukraine",
                Region = "Kyiv Oblast",
                City = "Kyiv"
            };

            var existAddress = await _serviceManager.AddressService.GetAddressByFieldsAsync(addressRequest);

            var updateAddressRequest = new UpdateAddressRequest()
            {
                Id = existAddress.First().Id
            };

            try
            {
                await _serviceManager.AddressService.UpdateAddressAsync(updateAddressRequest);
            }
            catch
            {
                using (var context = new RepositoryContext(_options))
                {
                    Assert.AreEqual(1, context.Addresses.Count());
                    Assert.AreEqual("Ukraine", context.Addresses.Single().Country);
                    Assert.AreEqual("Kyiv Oblast", context.Addresses.Single().Region);
                    Assert.AreEqual("Kyiv", context.Addresses.Single().City);
                    Assert.AreEqual("Zhulianska", context.Addresses.Single().Street);
                }
            }            
        }




        private async Task AddingTESTAddressToDB()
        {
            await AddingTESTContactToDB();

            var getContactRequest = new GetContactRequest()
            {
                FirstName = "TestFN"
            };

            var contact = await _serviceManager.ContactService.GetContactByFieldAsync(getContactRequest);

            await _serviceManager.AddressService.CreateAddressAsync(new CreateAddressRequest
            {
                AddressType = 0,
                Country = "Ukraine",
                Region = "Kyiv Oblast",
                City = "Kyiv",
                Street = "Zhulianska",
                BuildingNumber = 1,
                ContactId = contact.ToList().First().Id
            });
        }
    }
}

        
        
   

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

        /**//*

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

        *//*[TestMethod]
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
        }*//*

        [TestMethod]
        public async Task DeleteAddressNull()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _serviceManager.AddressService.DeleteAddressAsync(null));
        }

        

        *//**//*
    }*/
