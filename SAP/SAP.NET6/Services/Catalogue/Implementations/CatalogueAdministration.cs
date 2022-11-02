﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAP.NET6.Data;
using SAP.NET6.Data.Models.Catalogue;
using SAP.NET6.ViewModels.Catalogue;
using SAP.NET6.ViewModels.Catalogue.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.NET6.Services.Catalogue.Implementations
{
    public class CatalogueAdministration : ICatalogueAdministration
    {
        private IMapper Mapper { get; }

        private ApplicationDbContext DbContext { get; set; }

        public CatalogueAdministration(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            Mapper = mapper;
            DbContext = applicationDbContext;
        }

        public async Task CreateCategoryAsync(CreateCategoryViewModel category)
        {
            DbContext.Categories.Add(Mapper.Map<Category>(category));
            await DbContext.SaveChangesAsync();
        }

        public async Task CreateItemAsync(CreateItemViewModel item)
        {
            DbContext.Items.Add(Mapper.Map<Item>(item));
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var item = await DbContext.Items.LastOrDefaultAsync(x => x.Id == id);
            DbContext.Items.Remove(item);
            await DbContext.SaveChangesAsync();
        }
    }
}
