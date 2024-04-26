// Copy Caller contact info to Source contact info
const copyInfoButton = document.getElementById("copy-caller-info");

copyInfoButton.addEventListener("click", () => {
    document.getElementById("NewComplaint_SourceContactName").value = document.getElementById("NewComplaint_CallerName").value;
    document.getElementById("NewComplaint_SourceEmail").value = document.getElementById("NewComplaint_CallerEmail").value;
    document.getElementById("NewComplaint_SourcePhoneNumber_Number").value = document.getElementById("NewComplaint_CallerPhoneNumber_Number").value;
    document.getElementById("NewComplaint_SourcePhoneNumber_Type").value = document.getElementById("NewComplaint_CallerPhoneNumber_Type").value;
    document.getElementById("NewComplaint_SourceSecondaryPhoneNumber_Number").value = document.getElementById("NewComplaint_CallerSecondaryPhoneNumber_Number").value;
    document.getElementById("NewComplaint_SourceSecondaryPhoneNumber_Type").value = document.getElementById("NewComplaint_CallerSecondaryPhoneNumber_Type").value;
    document.getElementById("NewComplaint_SourceTertiaryPhoneNumber_Number").value = document.getElementById("NewComplaint_CallerTertiaryPhoneNumber_Number").value;
    document.getElementById("NewComplaint_SourceTertiaryPhoneNumber_Type").value = document.getElementById("NewComplaint_CallerTertiaryPhoneNumber_Type").value;
    document.getElementById("NewComplaint_SourceAddress_Street").value = document.getElementById("NewComplaint_CallerAddress_Street").value;
    document.getElementById("NewComplaint_SourceAddress_Street2").value = document.getElementById("NewComplaint_CallerAddress_Street2").value;
    document.getElementById("NewComplaint_SourceAddress_City").value = document.getElementById("NewComplaint_CallerAddress_City").value;
    document.getElementById("NewComplaint_SourceAddress_State").value = document.getElementById("NewComplaint_CallerAddress_State").value;
    document.getElementById("NewComplaint_SourceAddress_PostalCode").value = document.getElementById("NewComplaint_CallerAddress_PostalCode").value;
});
