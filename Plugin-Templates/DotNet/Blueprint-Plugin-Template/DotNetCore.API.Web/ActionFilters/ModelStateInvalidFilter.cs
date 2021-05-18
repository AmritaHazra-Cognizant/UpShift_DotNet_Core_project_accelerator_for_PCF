using DotNetCore.API.Contract.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web.ActionFilters
{
    /* 
     * This filter packages any model state errors into a custom response model.
     * This is presented strictly as an example. The default model state action
     * Filter dll return the ModelState object verbatin without any custom
     * formatting. See Startup.cs for more details.

    **/
    public class ModelStateInvalidFilter : ActionFilterAttribute
    {


        private readonly ILogger<ModelStateInvalidFilter> _logger;
        public ModelStateInvalidFilter(ILogger<ModelStateInvalidFilter> logger)
        {
            _logger = logger;
        }
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                _logger.LogWarning("Bad Request - model state: {@ModelStateValues)", context.ModelState.Values);
                // short-circuit the pipeline execution by setting the result property
                context.Result = new BadRequestObjectResult(PackageModelStateErrors(context));
            }
            return base.OnActionExecutionAsync(context, next);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                _logger.LogWarning("Bad Request - model state: (ModelStateValues)", context.ModelState.Values);
                //short-circuit the pipeline execution by setting the result property
                context.Result = new BadRequestObjectResult(PackageModelStateErrors(context));

            }
        }


        private IModelStateErrorReport PackageModelStateErrors(ActionExecutingContext context)
        {
            var msVals = context.ModelState.Values;
            var modelErrors = msVals.SelectMany(v => v.Errors).ToList();
            List<string> validationErrors = modelErrors.Select(e => e.ErrorMessage).ToList();
            var report = new ModelStateErrorReport
            {
                InvalidFieldNames = context.ModelState.Keys,
                ErrorMessages = validationErrors,
                Summarization = $"Sorry, your request could not be proceesed. The system encountered {validationErrors.Count} problem(s) with the data provided. Please make corrections and try again."
            };
            report.ErrorDictionary = new Dictionary<string, List<string>>();
            foreach (string key in context.ModelState.Keys)
            {
                var valItem = context.ModelState[key];
                report.ErrorDictionary.Add(key, valItem.Errors.Select(e => e.ErrorMessage).ToList());


            }
            return report;
        }
    }
}
