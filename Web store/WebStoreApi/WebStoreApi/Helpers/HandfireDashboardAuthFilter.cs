using Hangfire.Annotations;
using Hangfire.Dashboard;
using WebStore.Api.Constants;

namespace WebStore.Api.Helpers
{
    public class HandfireDashboardAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return context.GetHttpContext().User.IsInRole(RoleNames.Admin);
        }
    }
}
