// Copy Caller contact info to Source contact info
const copyInfoButton = document.getElementById("copy-caller-info");

copyInfoButton.addEventListener("click", () => {
    document.getElementById("Item_SourceContactName").value = document.getElementById("Item_CallerName").value;
    document.getElementById("Item_SourceEmail").value = document.getElementById("Item_CallerEmail").value;
    document.getElementById("Item_SourcePhoneNumber_Number").value = document.getElementById("Item_CallerPhoneNumber_Number").value;
    document.getElementById("Item_SourcePhoneNumber_Type").value = document.getElementById("Item_CallerPhoneNumber_Type").value;
    document.getElementById("Item_SourceSecondaryPhoneNumber_Number").value = document.getElementById("Item_CallerSecondaryPhoneNumber_Number").value;
    document.getElementById("Item_SourceSecondaryPhoneNumber_Type").value = document.getElementById("Item_CallerSecondaryPhoneNumber_Type").value;
    document.getElementById("Item_SourceTertiaryPhoneNumber_Number").value = document.getElementById("Item_CallerTertiaryPhoneNumber_Number").value;
    document.getElementById("Item_SourceTertiaryPhoneNumber_Type").value = document.getElementById("Item_CallerTertiaryPhoneNumber_Type").value;
    document.getElementById("Item_SourceAddress_Street").value = document.getElementById("Item_CallerAddress_Street").value;
    document.getElementById("Item_SourceAddress_Street2").value = document.getElementById("Item_CallerAddress_Street2").value;
    document.getElementById("Item_SourceAddress_City").value = document.getElementById("Item_CallerAddress_City").value;
    document.getElementById("Item_SourceAddress_State").value = document.getElementById("Item_CallerAddress_State").value;
    document.getElementById("Item_SourceAddress_PostalCode").value = document.getElementById("Item_CallerAddress_PostalCode").value;
});
