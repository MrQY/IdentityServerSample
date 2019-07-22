using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using MvcCookieAuthSample.Models;
using MvcCookieAuthSample.Services;

namespace MvcCookieAuthSample.Controllers
{
    public class ConsentController : Controller
    {
        private readonly ConsentService consentService;

        public ConsentController(ConsentService consentService)
        {
            this.consentService = consentService;
        }
        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await consentService.BuildConsentViewModel(returnUrl);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputConsentViewModel viewModel)
        {
            ConsentResponse consentResponse = null;
            if (viewModel.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if(viewModel.Button=="yes")
            {
                if(viewModel.ScopesConsented!=null && viewModel.ScopesConsented.Any())
                {
                    consentResponse = new ConsentResponse
                    {
                        ScopesConsented = viewModel.ScopesConsented,
                        RememberConsent=viewModel.RememberConsent
                    };
                }
            }
            if (consentResponse != null)
            {
                await consentService.SaveConsentViewModel(consentResponse, viewModel.ReturnUrl);
                return Redirect(viewModel.ReturnUrl);
            }
            if (consentResponse == null)
            {
                ModelState.AddModelError("", "请至少选中一个权限");
            }
            return View();
        }
    }
}