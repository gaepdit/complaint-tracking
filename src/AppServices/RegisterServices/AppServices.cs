using Cts.AppServices.ActionTypes;
using Cts.AppServices.Attachments;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints;
using Cts.AppServices.Concerns;
using Cts.AppServices.DataExport;
using Cts.AppServices.Notifications;
using Cts.AppServices.Offices;
using Cts.AppServices.Reporting;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.RegisterServices;

public static class AppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
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
        services.AddScoped<IActionService, ActionService>();

        // Concerns
        services.AddScoped<IConcernManager, ConcernManager>();
        services.AddScoped<IConcernService, ConcernService>();

        // Notifications
        services.AddScoped<INotificationService, NotificationService>();

        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeService, OfficeService>();

        // Reporting
        services.AddScoped<IReportingService, ReportingService>();

        // Data Export
        services.AddScoped<IDataExportService, DataExportService>();
        services.AddScoped<ISearchResultsExportService, SearchResultsExportService>();

        return services;
    }
}
