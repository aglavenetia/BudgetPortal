using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BudgetPortal.ViewModel
{
    public class PercentageModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelMetadata.AdditionalValues.ContainsKey("Percentage"))
            {
                var format = (string)bindingContext.ModelMetadata.AdditionalValues["Percentage"];
                // TODO: do the custom parsing here
                throw new NotImplementedException();
            }

            return Task.CompletedTask;
        }
    }
}
