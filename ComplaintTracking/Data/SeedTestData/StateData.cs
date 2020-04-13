using ComplaintTracking.Models;

namespace ComplaintTracking.Data
{
    public partial class SeedTestData
    {
        public static State[] GetStates()
        {
            State[] states = {
                new State { Name = "Alabama", PostalAbbreviation = "AL" },
                new State { Name = "Alaska", PostalAbbreviation = "AK" },
                new State { Name = "Arizona", PostalAbbreviation = "AZ" },
                new State { Name = "Arkansas", PostalAbbreviation = "AR" },
                new State { Name = "California", PostalAbbreviation = "CA" },
                new State { Name = "Colorado", PostalAbbreviation = "CO" },
                new State { Name = "Connecticut", PostalAbbreviation = "CT" },
                new State { Name = "Delaware", PostalAbbreviation = "DE" },
                new State { Name = "District of Columbia", PostalAbbreviation = "DC" },
                new State { Name = "Florida", PostalAbbreviation = "FL" },
                new State { Name = "Georgia", PostalAbbreviation = "GA" },
                new State { Name = "Hawaii", PostalAbbreviation = "HI" },
                new State { Name = "Idaho", PostalAbbreviation = "ID" },
                new State { Name = "Illinois", PostalAbbreviation = "IL" },
                new State { Name = "Indiana", PostalAbbreviation = "IN" },
                new State { Name = "Iowa", PostalAbbreviation = "IA" },
                new State { Name = "Kansas", PostalAbbreviation = "KS" },
                new State { Name = "Kentucky", PostalAbbreviation = "KY" },
                new State { Name = "Louisiana", PostalAbbreviation = "LA" },
                new State { Name = "Maine", PostalAbbreviation = "ME" },
                new State { Name = "Maryland", PostalAbbreviation = "MD" },
                new State { Name = "Massachusetts", PostalAbbreviation = "MA" },
                new State { Name = "Michigan", PostalAbbreviation = "MI" },
                new State { Name = "Minnesota", PostalAbbreviation = "MN" },
                new State { Name = "Mississippi", PostalAbbreviation = "MS" },
                new State { Name = "Missouri", PostalAbbreviation = "MO" },
                new State { Name = "Montana", PostalAbbreviation = "MT" },
                new State { Name = "Nebraska", PostalAbbreviation = "NE" },
                new State { Name = "Nevada", PostalAbbreviation = "NV" },
                new State { Name = "New Hampshire", PostalAbbreviation = "NH" },
                new State { Name = "New Jersey", PostalAbbreviation = "NJ" },
                new State { Name = "New Mexico", PostalAbbreviation = "NM" },
                new State { Name = "New York", PostalAbbreviation = "NY" },
                new State { Name = "North Carolina", PostalAbbreviation = "NC" },
                new State { Name = "North Dakota", PostalAbbreviation = "ND" },
                new State { Name = "Ohio", PostalAbbreviation = "OH" },
                new State { Name = "Oklahoma", PostalAbbreviation = "OK" },
                new State { Name = "Oregon", PostalAbbreviation = "OR" },
                new State { Name = "Pennsylvania", PostalAbbreviation = "PA" },
                new State { Name = "Rhode", PostalAbbreviation = "RI" },
                new State { Name = "South Carolina", PostalAbbreviation = "SC" },
                new State { Name = "South Dakota", PostalAbbreviation = "SD" },
                new State { Name = "Tennessee", PostalAbbreviation = "TN" },
                new State { Name = "Texas", PostalAbbreviation = "TX" },
                new State { Name = "Utah", PostalAbbreviation = "UT" },
                new State { Name = "Vermont", PostalAbbreviation = "VT" },
                new State { Name = "Virginia", PostalAbbreviation = "VA" },
                new State { Name = "Washington", PostalAbbreviation = "WA" },
                new State { Name = "West Virginia", PostalAbbreviation = "WV" },
                new State { Name = "Wisconsin", PostalAbbreviation = "WI" },
                new State { Name = "Wyoming", PostalAbbreviation = "WY" }
            };

            return states;
        }
    }
}