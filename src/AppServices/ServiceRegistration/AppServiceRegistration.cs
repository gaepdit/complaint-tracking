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
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.ServiceRegistration;

public static class AppServiceRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services) => services
        // Action Types
        .AddScoped<IActionTypeManager, ActionTypeManager>()
        .AddScoped<IActionTypeService, ActionTypeService>()

        // Attachments
        .AddScoped<IAttachmentManager, AttachmentManager>()
        .AddScoped<IAttachmentService, AttachmentService>()

        // Complaints
        .AddScoped<IComplaintManager, ComplaintManager>()
        .AddScoped<IComplaintService, ComplaintService>()

        // Complaint Actions
        .AddScoped<IActionService, ActionService>()

        // Concerns
        .AddScoped<IConcernManager, ConcernManager>()
        .AddScoped<IConcernService, ConcernService>()

        // Notifications
        .AddScoped<INotificationService, NotificationService>()

        // Offices
        .AddScoped<IOfficeManager, OfficeManager>()
        .AddScoped<IOfficeService, OfficeService>()

        // Reporting
        .AddScoped<IReportingService, ReportingService>()

        // Data Export
        .AddScoped<IDataExportService, DataExportService>()
        .AddScoped<ISearchResultsExportService, SearchResultsExportService>()

        // Validators
        .AddValidatorsFromAssemblyContaining(typeof(AppServiceRegistration));
}
