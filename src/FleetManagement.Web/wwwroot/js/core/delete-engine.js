document.addEventListener('click', async e => {
    const btn = e.target.closest('[data-delete-url]');
    if (!btn) return;

    e.preventDefault();

    const url = btn.dataset.deleteUrl;
    const name = btn.dataset.deleteName;
    const refreshKey = btn.dataset.refresh;

    const confirmed = await showDeleteConfirmation(name);
    if (!confirmed) return;

    await executeDelete(url, refreshKey, btn);
});

async function executeDelete(url, refreshKey, btn) {
    try {
        showSpinner();

        btn.disabled = true;

        const response = await fetch(url, { method: 'POST' });
        const result = await response.json();

        if (result.success) {
            showToast('Deleted successfully', 'success');
            triggerRefresh(refreshKey);
        } else {
            showToast(result.message || 'Delete failed', 'error');
        }
    } catch (err) {
        console.error(err);
        showToast('Network error', 'error');
    } finally {
        btn.disabled = false;
        hideSpinner();
    }
}
