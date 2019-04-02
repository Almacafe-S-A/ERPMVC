using ERPMVC.Helpers;
using ERPMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ERPMVC.Policies
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IOptions<MyConfig> config;

        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IConfiguration configuration
            ,IOptions<MyConfig> config) : base(options)
        {
             this.config = config;
             _options = options.Value;
             _configuration = configuration;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // Check static policies first
            var policy = await base.GetPolicyAsync(policyName);

            try
            {
                if (policy == null)
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    List<Policy> _policies = new List<Policy>();

                    var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.Value.UserEmail, Password = config.Value.UserPassword });
                    if (resultlogin.IsSuccessStatusCode)
                    {

                        string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                        UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                        var result = await _client.GetAsync(baseadress + "api/Policies/GetPolicies");
                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _policies = JsonConvert.DeserializeObject<List<Policy>>(valorrespuesta);

                        }
                    }

                    foreach (var item in _policies)
                    {
                        string valor = "";
                        valor = await _client.GetAsync(baseadress + "api/Policies/GetRolesByPolicy/" + item.Id).Result.Content.ReadAsStringAsync();
                        List<IdentityRole> _policiesroles = JsonConvert.DeserializeObject<List<IdentityRole>>(valor);
                        string[] _rols = _policiesroles.Select(q => q.Name).ToArray<string>();

                        switch (item.type)
                        {
                            case "Roles":
                                policy = new AuthorizationPolicyBuilder()
                                 .RequireRole(_rols)
                                 
                               //.AddRequirements(new HasScopeRequirement(policyName, $"https://{_configuration["Auth0:Domain"]}/"))
                               // .AddRequirements(new HasScopeRequirement(policyName,_policiesroles))
                               .Build();
                                break;
                            case "Requirement":
                                //policy = new AuthorizationPolicyBuilder()                         
                                //.AddRequirements(new HasScopeRequirement(policyName))
                                //.Build();
                                break;

                        }

                        // Add policy to the AuthorizationOptions, so we don't have to re-create it each time
                        _options.AddPolicy(policyName, policy);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return policy;
        }




    }


}
