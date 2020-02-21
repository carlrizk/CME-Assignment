using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_2_3_CarlRizk.Services
{
    public class Validator
    {
        public IEnumerable<string> GetErrors(ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
        }
    }
}
