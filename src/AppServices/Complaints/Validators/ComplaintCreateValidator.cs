﻿using Cts.AppServices.Complaints.Dto;
using FluentValidation;

namespace Cts.AppServices.Complaints.Validators;

public class ComplaintCreateValidator : AbstractValidator<ComplaintCreateDto>
{
    public ComplaintCreateValidator()
    {
        RuleFor(e => e.CurrentOfficeId).NotEmpty();
    }
}
