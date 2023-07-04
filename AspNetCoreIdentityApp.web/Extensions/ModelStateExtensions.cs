using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreIdentityApp.web.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddModelErrolList(this ModelStateDictionary modelState, List<string> errors)
        {
            errors.ForEach(x =>
            {
                modelState.AddModelError(string.Empty, x);
            });


            
        }
    }
}
