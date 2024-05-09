using Microsoft.EntityFrameworkCore;
using Notebook.Application.Services.Implementation;
using Notebook.DataAccess;
using Notebook.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
    }
}
