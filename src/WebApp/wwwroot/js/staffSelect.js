﻿// Populated the assigned associate dropdown when an assigned office selection is made.
function setUpStaffDropdown(officeElement, staffElement, forAssignment) {
    let apiPath;
    let staffPlaceholder;

    if (forAssignment === true) {
        apiPath = "staff-for-assignment";
        staffPlaceholder = "[Default Assignor]";
    } else {
        apiPath = "staff";
        staffPlaceholder = "(any)";
    }

    const officeSelect = document.getElementById(officeElement);

    officeSelect.addEventListener("change", () => {
        const staffSelect = document.getElementById(staffElement);
        staffSelect.innerHTML = `<option value="">${staffPlaceholder}</option>`;
        staffSelect.disabled = true;
        if (officeSelect.value === '') return;

        axios.get(`/api/offices/${officeSelect.value}/${apiPath}`)
            .then(function (response) {
                const data = response.data;
                if (data == null || data.length === 0) return;

                staffSelect.disabled = false;
                let opt;
                for (const item of data) {
                    opt = document.createElement('option');
                    opt.text = item.name;
                    opt.value = item.id;
                    staffSelect.add(opt);
                }
            })
            .catch(function errorHandler(error) {
                staffSelect.innerHTML = '<option value="">Error</option>';
                if (error instanceof Error && typeof rg4js === "function") {
                    rg4js('send', {error: error, tags: ['handled_promise_rejection']});
                }
            });
    });
}
