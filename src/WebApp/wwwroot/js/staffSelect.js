// Populated the assigned associate dropdown when an assigned office selection is made.
const officeSelect = document.getElementById("Spec_Office");

officeSelect.addEventListener("change", () => {
    const staffSelect = document.getElementById("Spec_Assigned");
    staffSelect.innerHTML = '<option value="">(any)</option>';
    staffSelect.disabled = true;
    if (officeSelect.value === '') return;

    axios.get(`/api/offices/${officeSelect.value}/staff`)
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
