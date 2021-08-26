using System;

namespace ComplaintTracking.AlertMessages
{
    public class AlertViewModel
    {
        public AlertViewModel(string message, AlertStatus status = AlertStatus.Information, string title = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message is empty");
            }

            Title = title;
            Message = message;
            Status = status;
        }

        public AlertViewModel(string message, string status, string title = null) :
            this(message, ConvertStatusToEnum(status), title) { }

        public string Title { get; set; }
        public string Message { get; set; }
        public AlertStatus Status { get; set; }

        public string AlertCssClass =>
            Status switch
            {
                AlertStatus.Success => "usa-alert-success",
                AlertStatus.Warning => "usa-alert-warning",
                AlertStatus.Error => "usa-alert-error",
                _ => "usa-alert-info",
            };

        private static AlertStatus ConvertStatusToEnum(string status) =>
            status switch
            {
                "Success" => AlertStatus.Success,
                "Warning" => AlertStatus.Warning,
                "Error" => AlertStatus.Error,
                _ => AlertStatus.Information,
            };
    }

    public enum AlertStatus
    {
        Success,
        Warning,
        Error,
        Information
    }
}
