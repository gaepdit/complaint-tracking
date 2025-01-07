document.addEventListener('DOMContentLoaded', () => {
    let dropAreas = document.querySelectorAll('.drop-area');
    dropAreas.forEach(dropArea => {
        let readyMessageArea = dropArea.querySelector('.upload-ready');
        let fileInput = dropArea.querySelector('.fileElem');
        let selectFilesBtn = dropArea.querySelector('.select-files-btn');

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
            handleFiles(e.dataTransfer.files);
            displaySuccessMessage();
        }

        // Handle file selection
        fileInput.addEventListener('change', () => {
            let originalCount = fileInput.files.length;
            if (originalCount > 10) {
                alert(`${originalCount} files were selected, but only the first 10 will be uploaded.`);
                let dataTransfer = new DataTransfer();
                for (let i = 0; i < 10; i++) {
                    dataTransfer.items.add(fileInput.files[i]);
                }
                fileInput.files = dataTransfer.files;
            }
            displaySuccessMessage();
        });

        // Add click event to the button
        selectFilesBtn.addEventListener('click', (e) => {
            e.preventDefault();
            fileInput.click();
        });

        function handleFiles(files) {
            if (fileInput.files.length >= 10) {
                alert("You can only upload up to 10 files at a time.");
            } else {
                let dataTransfer = new DataTransfer();
                for (const file of fileInput.files) {
                    dataTransfer.items.add(file);
                }
                for (const file of files) {
                    if (![...dataTransfer.items].some(item => item.getAsFile().name === file.name)) {
                        if (dataTransfer.items.length >= 10) {
                            alert("You can only upload up to 10 files at a time.");
                            break;
                        }
                        dataTransfer.items.add(file);
                    }
                }
                fileInput.files = dataTransfer.files;
            }
        }

        function displaySuccessMessage() {
            readyMessageArea.innerHTML = "";
            let messageHeader = document.createElement('div');
            messageHeader.setAttribute('class', 'mt-2');
            messageHeader.textContent = `Ready to upload:`;
            readyMessageArea.appendChild(messageHeader);

            let fileList = document.createElement('ul');
            fileList.setAttribute('class', 'm-0');
            for (const file of fileInput.files) {
                let message = document.createElement('li');
                message.textContent = file.name;
                fileList.appendChild(message);
            }
            readyMessageArea.appendChild(fileList);
        }
    });
});
