using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SmearAdmin.Helpers
{
    public static class Errors
    {
        public static ModelStateDictionary AddErrorsToModelState(IdentityResult identityResult, ModelStateDictionary modelState)
        {
            foreach (var e in identityResult.Errors)
            {
                modelState.TryAddModelError(e.Code, e.Description);
            }

            return modelState;
        }

        public static ModelStateDictionary AddErrorToModelState(string code, string description, ModelStateDictionary modelState)
        {
            modelState.TryAddModelError(code, description);
            return modelState;
        }

        public static object AddError(Exception exception, ModelStateDictionary modelState)
        {
            var error = new
            {
                message = $"Message:- {exception?.Message.ToString()} | InnerMsg:- {exception.InnerException?.Message.ToString()}",
                error = modelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
            };
            return error;
        }

        public static object AddError(string errMsg, ModelStateDictionary modelState)
        {
            var error = new
            {
                message = errMsg,
                error = modelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
            };
            return error;
        }
    }
}
