using ComplaintTracking.Models;

namespace ComplaintTracking.Data
{
    public partial class SeedTestData
    {
        public static Office[] GetOffices(ApplicationUser user)
        {
            Office[] offices = {
                new Office { Name = "Air Protection Branch", Active = true, MasterUser = user },
                new Office { Name = "Coastal District", Active = true, MasterUser = user },
                new Office { Name = "Director's Office", Active = true, MasterUser = user },
                new Office { Name = "East Central District/Augusta", Active = true, MasterUser = user },
                new Office { Name = "Emergency Response Team", Active = true, MasterUser = user },
                new Office { Name = "Geologic Survey Branch", Active = true, MasterUser = user },
                new Office { Name = "Land Protection Branch", Active = true, MasterUser = user },
                new Office { Name = "Middle Region", Active = true, MasterUser = user },
                new Office { Name = "Mountain District", Active = true, MasterUser = user },
                new Office { Name = "Northeast Region", Active = true, MasterUser = user },
                new Office { Name = "Small Business Environmental Assistance Program", Active = true, MasterUser = user },
                new Office { Name = "Southwest Region", Active = true, MasterUser = user },
                new Office { Name = "Water Resources Branch", Active = true, MasterUser = user },
                new Office { Name = "Watershed Protection Branch", Active = true, MasterUser = user },
                new Office { Name = "Air Laboratory", Active = false, MasterUser = user },
                new Office { Name = "Air Protection Branch (Not used)", Active = false, MasterUser = user },
                new Office { Name = "Bacteriology Laboratory", Active = false, MasterUser = user },
                new Office { Name = "Environmental Toxicology", Active = false, MasterUser = user },
                new Office { Name = "Information Management", Active = false, MasterUser = user },
                new Office { Name = "Inorganic Laboratory", Active = false, MasterUser = user },
                new Office { Name = "Laboratories", Active = false, MasterUser = user },
                new Office { Name = "Metals Laboratory", Active = false, MasterUser = user },
                new Office { Name = "Northwest Region", Active = false, MasterUser = user },
                new Office { Name = "Organic Laboratory", Active = false, MasterUser = user },
                new Office { Name = "Radioactive Material Program (not used)", Active = false, MasterUser = user },
                new Office { Name = "Radioactive Surveillance Program (not used)", Active = false, MasterUser = user },
                new Office { Name = "Right To Know/SARA Title III", Active = false, MasterUser = user },
                new Office { Name = "Training", Active = false, MasterUser = user }
            };

            return offices;
       }
    }
}