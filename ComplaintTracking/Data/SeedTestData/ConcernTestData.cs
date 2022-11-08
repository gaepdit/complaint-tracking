﻿using System.Linq;
using ComplaintTracking.Models;

namespace ComplaintTracking.Data
{
    public static partial class SeedTestData
    {
        public static Concern[] GetConcerns()
        {
            string[] items =
            {
                "Agricultural Ground Water Use",
                "Agricultural Surface Water Use",
                "Air Quality Control",
                "Asbestos",
                "Clean Fueled Fleets",
                "Comprehensive Solid Waste",
                "Diesel fuel spill",
                "Enhanced Inspection & Maintenance",
                "Environmental Policy",
                "Environmentally Sensitive Properties",
                "Erosion & Sedimentation Control",
                "Gasoline fuel spill",
                "Grants",
                "Ground Water & Surface Water Withdrawals",
                "Ground Water Use",
                "Hazardous Site Response",
                "Hazardous Waste Management",
                "Hazardous material spill",
                "Lead paint abatement",
                "Motor Vehicle Inspection & Maintenance",
                "Nuclear Power incident",
                "Oil & Gas Deep Drilling",
                "Oil spill",
                "Radioactive Materials - Allegation",
                "Radioactive Materials - DOT Exemption",
                "Radioactive Materials - Incident",
                "Radioactive Waste Disposal",
                "Safe Dams",
                "Safe Drinking Water",
                "Scrap Tire",
                "Sewage spill",
                "Sewerage Holding Tanks",
                "Surface Mining",
                "Underground Gas Storage",
                "Underground Storage Tanks",
                "Water Quality Confined Animal Operations",
                "Water Quality Control",
                "Water Quality Stormwater Construction",
                "Water Quality Stormwater Industrial",
                "Water Quality Stormwater Urban"
            };

            return items.Select(item => new Concern {Name = item}).ToArray();
        }
    }
}