using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using MvcCookieAuthSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCookieAuthSample.Services
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentService(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        private ScopeViewModel CreateScopViewModel(IdentityResource identityResource)
        {
            return new ScopeViewModel
            {
                Checked=identityResource.Required,
                Description=identityResource.Description,
                DisplayName=identityResource.DisplayName,
                Emphasize=identityResource.Emphasize,
                Name=identityResource.Name,
                Required=identityResource.Required
            };
        }

        private ScopeViewModel CreateScopViewModel(Scope scope)
        {
            return new ScopeViewModel
            {
                Checked=scope.Required,
                Description=scope.Description,
                DisplayName=scope.DisplayName,
                Emphasize=scope.Emphasize,
                Name=scope.Name,
                Required=scope.Required
            };
        }
        public async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
                return null;

            var client = await _clientStore.FindClientByIdAsync(request.ClientId);
            if (client == null)
                return null;

            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
            if (resources == null)
                return null;

            return new ConsentViewModel
            {
                RememberConsent = client.AllowRememberConsent,
                ClientId = client.ClientId,
                ClientLogoUrl = client.LogoUri,
                ClientName=client.ClientName,
                ClientUrl=client.ClientUri,
                ReturnUrl=returnUrl,
                IdentityScopes=resources.IdentityResources.Select(o=>CreateScopViewModel(o)),
                ResourceScopes=resources.ApiResources.SelectMany(o=>o.Scopes).Select(p=>CreateScopViewModel(p))
            };
        }

        public async Task SaveConsentViewModel(ConsentResponse consentResponse,string returnUrl)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);
        }
    }
}
