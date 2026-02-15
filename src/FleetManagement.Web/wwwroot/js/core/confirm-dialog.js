async function showDeleteConfirmation(name) {
    return new Promise(resolve => {
        const overlay = document.getElementById('deleteModalOverlay');
        const label = document.getElementById('deleteAircraftReg');
        const confirmBtn = document.getElementById('confirmDeleteBtn');
        const cancelBtn = document.getElementById('cancelDeleteBtn');

        label.textContent = name;
        overlay.classList.add('active');

        const cleanup = () => {
            overlay.classList.remove('active');
            confirmBtn.onclick = null;
            cancelBtn.onclick = null;
        };

        confirmBtn.onclick = () => {
            cleanup();
            resolve(true);
        };

        cancelBtn.onclick = () => {
            cleanup();
            resolve(false);
        };
    });
}
