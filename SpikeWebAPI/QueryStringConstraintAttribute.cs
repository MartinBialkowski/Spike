using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace Spike.WebApi
{
    public class QueryStringConstraintAttribute : ActionMethodSelectorAttribute
    {
        public string ValueName { get; private set; }
        public bool IsPresent { get; private set; }
        public QueryStringConstraintAttribute(string valueName, bool isPresent)
        {
            ValueName = valueName;
            IsPresent = isPresent;
        }

        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var value = routeContext.HttpContext.Request.Query[ValueName];
            return IsPresent ? !StringValues.IsNullOrEmpty(value) : StringValues.IsNullOrEmpty(value);
        }
    }
}
