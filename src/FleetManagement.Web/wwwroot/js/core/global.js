document.addEventListener('click', async e => {
    const btn = e.target.closest('[data-modal-url]');
    if (!btn) return;

    const refreshKey = btn.dataset.refresh;

    await openEntityModal(btn.dataset.modalUrl, () => {
        if (refreshKey === 'aircraft') refreshAircraftUI();
        if (refreshKey === 'maintenance') refreshMaintenanceUI();
    });
});