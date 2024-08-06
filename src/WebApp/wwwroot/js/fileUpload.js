document.addEventListener('DOMContentLoaded', (event) => {
    let dropAreas = document.querySelectorAll('.drop-area');
    dropAreas.forEach(dropArea => {
        let successMessageArea = dropArea.querySelector('.upload-success');
        let fileInput = dropArea.querySelector('.fileElem');
        let selectFilesBtn = dropArea.querySelector('.select-files-button');

        // Prevent default drag behaviors
        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
            dropArea.addEventListener(eventName, preventDefaults, false);
        });

        function preventDefaults(e) {
            e.preventDefault();
            e.stopPropagation();
        }

        // Highlight drop area when item is dragged over it
        ['dragenter', 'dragover'].forEach(eventName => {
            dropArea.addEventListener(eventName, () => dropArea.classList.add('hover'), false);
        });

        ['dragleave', 'drop'].forEach(eventName => {
            dropArea.addEventListener(eventName, () => dropArea.classList.remove('hover'), false);
        });

        // Handle dropped files
        dropArea.addEventListener('drop', handleDrop, false);

        function handleDrop(e) {
            let dt = e.dataTransfer;
            let files = dt.files;
            handleFiles(files);
            displaySuccessMessage(files);
        }

        // Handle file selection
        fileInput.addEventListener('change', (e) => {
            let files = e.target.files;
            displaySuccessMessage(files);
        });

        // Add click event to the button
        selectFilesBtn.addEventListener('click', (e) => {
            e.preventDefault();
            fileInput.click();
        });

        function handleFiles(files) {
            let dataTransfer = new DataTransfer();
            for (let i = 0; i < fileInput.files.length; i++) {
                dataTransfer.items.add(fileInput.files[i]);
            }
            for (let i = 0; i < files.length; i++) {
                dataTransfer.items.add(files[i]);
            }
            fileInput.files = dataTransfer.files;
        }

        function displaySuccessMessage(files) {
            successMessageArea.innerHTML = "";
            if (files.length > 1) {
                let message = document.createElement('p');
                message.textContent = "Multiple files selected.";
                successMessageArea.appendChild(message);
            } else {
                for (let i = 0; i < files.length; i++) {
                    let file = files[i];
                    let message = document.createElement('p');
                    message.textContent = `${file.name} selected.`;
                    successMessageArea.appendChild(message);
                }
            }
        }
    });
});
