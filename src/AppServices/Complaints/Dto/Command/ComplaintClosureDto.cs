﻿namespace Cts.AppServices.Complaints;

// Used for closing, reopening, deleting, and restoring complaints.
public record ComplaintClosureDto(int ComplaintId)
{
    public string? Comment { get; init; } = string.Empty;
}