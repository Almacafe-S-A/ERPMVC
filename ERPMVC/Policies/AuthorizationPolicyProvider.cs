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
                    string webtoken = "";
                    UserToken _userToken = new UserToken();
                    if (resultlogin.IsSuccessStatusCode)
                    {

                        webtoken = await (resultlogin.Content.ReadAsStringAsync());
                        _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                        var result = await _client.GetAsync(baseadress + "api/Policies/GetPolicies");
                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _policies = JsonConvert.DeserializeObject<List<Policy>>(valorrespuesta);

                        }

                     //   _policies = _policies.Where(q => q.Name == policyName).ToList();

                        foreach (var item in _policies)
                        {

                            policyName = item.Name;
                            string valor = "";
                        

                            switch (item.type)
                            {
                                case "Roles":
                                    valor = await _client.GetAsync(baseadress + "api/Policies/GetRolesByPolicy/" + item.Id).Result.Content.ReadAsStringAsync();
                                    List<ApplicationRole> _policiesroles = JsonConvert.DeserializeObject<List<ApplicationRole>>(valor);
                                    string[] _rols = _policiesroles.Select(q => q.Name).ToArray<string>();
                                    policy = new AuthorizationPolicyBuilder()
                                     .RequireRole(_rols)

                                   //.AddRequirements(new HasScopeRequirement(policyName, $"https://{_configuration["Auth0:Domain"]}/"))
                                   // .AddRequirements(new HasScopeRequirement(policyName,_policiesroles))
                                   .Build();
                                    _options.AddPolicy(policyName, policy);
                                    break;
                                case "UserClaimRequirement":
                                    List<IdentityUserClaim<string>> _userclaims = new List<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>();
                                    // _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                                     result = await _client.GetAsync(baseadress + "api/Policies/GetUserClaims/" + item.Id);
                                     valorrespuesta = "";
                                    if (result.IsSuccessStatusCode)
                                    {
                                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                                        _userclaims = JsonConvert.DeserializeObject<List<IdentityUserClaim<string>>>(valorrespuesta);

                                    }

                                    foreach (var _uclaim in _userclaims)
                                    {
                                        policy = new AuthorizationPolicyBuilder()
                                       .AddRequirements(new HasScopeRequirement(policyName, _uclaim.ClaimValue, _uclaim.ClaimType))
                                       .Build();
                                    }
                                   _options.AddPolicy(policyName, policy);
                                    break;
                                    

                            }

                            // Add policy to the AuthorizationOptions, so we don't have to re-create it each time
                         //   _options.AddPolicy(policyName, policy);
                        }
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
