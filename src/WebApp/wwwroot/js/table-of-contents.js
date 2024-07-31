document.addEventListener("DOMContentLoaded", function () {

    const toc = document.getElementById('toc');
    const headers = document.querySelectorAll('h2');

    headers.forEach((header, index) => {
        //if the h2 does not have an ID, assign one so we can use a tag
        if (!header.id) {
            header.id = `section${index + 1}`;
        }

        const link = document.createElement('a');
        link.href = `#${header.id}`;
        link.textContent = header.textContent;
        link.classList.add('list-group-item', 'list-group-item-action');
        toc.appendChild(link);
    });
});
