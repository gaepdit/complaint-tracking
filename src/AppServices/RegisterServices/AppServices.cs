using Cts.AppServices.ActionTypes;
using Cts.AppServices.Attachments;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.RegisterServices;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        // Action Types
        services.AddScoped<IActionTypeManager, ActionTypeManager>();
        services.AddScoped<IActionTypeService, ActionTypeService>();

        // Attachments
        services.AddScoped<IAttachmentManager, AttachmentManager>();
        services.AddScoped<IAttachmentService, AttachmentService>();

        // Complaints
        services.AddScoped<IComplaintManager, ComplaintManager>();
        services.AddScoped<IComplaintService, ComplaintService>();

        // Complaint Actions
        services.AddScoped<IComplaintActionService, ComplaintActionService>();

        // Complaint Transitions
        services.AddScoped<IComplaintTransitionManager, ComplaintTransitionManager>();

        // Concerns
        services.AddScoped<IConcernManager, ConcernManager>();
        services.AddScoped<IConcernService, ConcernService>();

        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeService, OfficeService>();
    }
}
