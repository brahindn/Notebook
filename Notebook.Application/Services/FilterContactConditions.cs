using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notebook.Domain;
using Notebook.Domain.Entities;

namespace Notebook.Application.Services
{
    public class FilterContactConditions
    {
        public List<Guid> ContactId { get; set; }
        public List<AddressType> AddressType { get; set; }
        public List<string> Country {  get; set; }
        public List<string> Region { get; set; }
        public List<string> City { get; set; }
        public List<string> Street { get; set; }
        public List<int> BuildingNumber { get; set; }
    }
}
