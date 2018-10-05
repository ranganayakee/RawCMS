﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawCMS.Library.Core.Interfaces;
using RawCMS.Library.DataModel;
using RawCMS.Library.Service;
using RawCMS.Plugins.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RawCMS.Plugins.Core.Stores
{
    public class RawRoleStore : IRoleStore<IdentityRole>, IRequireCrudService,IRequireLog
    {
        ILogger logger;
        CRUDService service;
        const string collection = "_roles";

        public void SetCRUDService(CRUDService service)
        {
            this.service = service;

        }

        public void SetLogger(ILogger logger)
        {
            this.logger = logger;
        }


        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            role.RoleId = role.RoleId.ToLower();
            //TODO: Add check to avoid duplicates 
            this.service.Insert(collection, JObject.FromObject(role));
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            this.service.Delete(collection, role.RoleId);
            return IdentityResult.Success;

        }

        public void Dispose()
        {
            
        }

        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var query = new DataQuery();
            //TODO: Check if serialization esclude null values
            query.RawQuery = JsonConvert.SerializeObject(new IdentityRole() {
                RoleId=roleId
            });
            //TODO: check for result count
            return this.service.Query(collection, query).Items.First.First.ToObject<IdentityRole>();
        }

        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await FindByIdAsync(normalizedRoleName, cancellationToken);
        }

        public async Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return role.RoleId;
        }

        public async Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return role.RoleId;
        }

        public async Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return  role.RoleId;
        }

        
      

        public async Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.RoleId = normalizedName;
        }

        public async Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            role.RoleId = roleName;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            this.service.Update(collection, JObject.FromObject(role), true);
            return IdentityResult.Success;
        }
    }
}
