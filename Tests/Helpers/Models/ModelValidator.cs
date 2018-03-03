using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tests.Models.Helpers
{
    /// <summary>
    /// Static wrapper for a model validation method.
    /// </summary>
    public static class ModelValidator
    {
        /// <summary>
        /// A method to validate models outside a controller in tests.
        /// </summary>
        /// <param name="model">A view model that uses annotations</param>
        /// <returns>A list of errors when validating the model</returns>
        public static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}