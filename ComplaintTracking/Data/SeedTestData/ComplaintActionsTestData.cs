﻿using System;
using ComplaintTracking.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ComplaintTracking.Data
{
    public partial class SeedTestData
    {
        public static async Task<ComplaintAction[]> GetComplaintActions(
            ApplicationDbContext _context,
            ApplicationUser user
            )
        {
            ComplaintAction[] c = {
                new ComplaintAction {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Deleted == true),
                    ActionDate = DateTime.Parse("Jun 13, 2017"),
                    ActionType = await _context.LookupActionTypes.FirstOrDefaultAsync(e => e.Name == "Initial investigation"),
                    Comments = "Investigation comments.",
                    Investigator = "I. N. Vestigator",
                },
                new ComplaintAction {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Deleted == true),
                    ActionDate = DateTime.Parse("Jun 16, 2017"),
                    ActionType = await _context.LookupActionTypes.FirstOrDefaultAsync(e => e.Name == "Referred to"),
                    Comments = "Investigation comments.",
                    Investigator = "I. N. Vestigator",
                },
                new ComplaintAction {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.Closed),
                    ActionDate = DateTime.Parse("Jun 13, 2017"),
                    ActionType = await _context.LookupActionTypes.FirstOrDefaultAsync(e => e.Name == "Initial investigation"),
                    Comments = "Investigation comments.",
                    Investigator = "I. N. Vestigator",
                },
                new ComplaintAction {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.Closed),
                    ActionDate = DateTime.Parse("Jun 16, 2017"),
                    ActionType = await _context.LookupActionTypes.FirstOrDefaultAsync(e => e.Name == "Referred to"),
                    Comments = "Investigation comments.",
                    Investigator = "I. N. Vestigator",
                },
                new ComplaintAction {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.Closed),
                    ActionDate = DateTime.Parse("Jun 16, 2017"),
                    ActionType = await _context.LookupActionTypes.FirstOrDefaultAsync(e => e.Name == "Referred to"),
                    Comments = "Deleted complaint action",
                    Investigator = "I. N. Vestigator",
                    Deleted = true,
                    DeletedBy = user,
                    DateDeleted = DateTime.Parse("Jun 16, 2017"),
                },
                new ComplaintAction {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.ReviewPending && e.Deleted == false),
                    ActionDate = DateTime.Parse("Jun 13, 2017"),
                    ActionType = await _context.LookupActionTypes.FirstOrDefaultAsync(e => e.Name == "Initial investigation"),
                    Comments = "Investigation comments.",
                    Investigator = "I. N. Vestigator",
                },
                new ComplaintAction {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.ReviewPending && e.Deleted == false),
                    ActionDate = DateTime.Parse("Jun 16, 2017"),
                    ActionType = await _context.LookupActionTypes.FirstOrDefaultAsync(e => e.Name == "Referred to"),
                    Comments = "Investigation comments.",
                    Investigator = "I. N. Vestigator",
                },
                new ComplaintAction {
                    Complaint = await _context.Complaints.FirstOrDefaultAsync(e => e.Status == ComplaintStatus.UnderInvestigation),
                    ActionDate = DateTime.Parse("Jun 13, 2017"),
                    ActionType = await _context.LookupActionTypes.FirstOrDefaultAsync(e => e.Name == "Initial investigation"),
                    Comments = "Investigation comments.",
                    Investigator = "I. N. Vestigator",
                },
            };

            return c;
       }
    }
}