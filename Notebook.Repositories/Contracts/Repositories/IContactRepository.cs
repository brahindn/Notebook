﻿using Notebook.Domain.Entities;

namespace Notebook.Repositories.Contracts.Repositories
{
    public interface IContactRepository
    {
        void Create(Contact contact);
        Task<Contact> GetContactAsync(string firstName, string LastName);
        IQueryable<Contact> GetAll();
    }
}
