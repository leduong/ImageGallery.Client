using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Client.Configuration;
using ImageGallery.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace ImageGallery.Client.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/Purchase")]
    public class PurchaseController : Controller
    {
        //private string STRIPE_PRIVATE_KEY = "sk_test_UKeLbhgP2vUGkYvGHSAPlKyZ";
        private StripApiConfig StripApiSettings { get; }

        public PurchaseController(IOptions<StripApiConfig> settings)
        {
            StripApiSettings = settings.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("StripeCharge")]
        public async Task<JsonResult> StripeCharge([FromBody]StripeChargeModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(BadRequest());
            }

            var chargeId = await ProcessPayment(model);

            // TODO: You should do something with the chargeId --> Persist it maybe?
            return Json(Ok(chargeId));
        }

        private async Task<string> ProcessPayment(StripeChargeModel model)
        {
            // return String.Empty;
            return await Task.Run(() =>
            {
                var myCharge = new StripeChargeCreateOptions
                {
                    Amount = (int)(model.Amount * 100),
                    Currency = model.Currency,
                    Description = model.Description,
                    SourceTokenOrExistingSourceId = model.Token,
                    ReceiptEmail = model.Email,
                    Capture = true,
                };

                var chargeService = new StripeChargeService(StripApiSettings.StripSecretKey);
                var stripeCharge = chargeService.Create(myCharge);

                return stripeCharge.Id;
            });
        }
    }
}