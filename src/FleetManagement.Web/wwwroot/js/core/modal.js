async function openEntityModal(url, onSuccessRefresh) {
    const modalOverlay = document.getElementById('modalOverlay');
    const modalContent = document.getElementById('modalContent');

    const response = await fetch(url);
    if (!response.ok) {
        showToast('Failed to load modal', 'error');
        return;
    }

    const html = await response.text();
    modalContent.innerHTML = html;
    modalOverlay.classList.add('active');

    modalContent.querySelectorAll('[data-action="close-modal"]')
        .forEach(btn => btn.addEventListener('click', closeModal));

    const form = modalContent.querySelector('form');
    if (!form) return;

    form.addEventListener('submit', e => submitEntityForm(e, onSuccessRefresh));
}


function closeModal() {
    const modalOverlay = document.getElementById('modalOverlay');
    modalOverlay.classList.remove('active');
    document.getElementById('modalContent').innerHTML = '';
}



function closeModalOnOverlay(e) {
    if (e.target.id === 'modalOverlay') closeModal();
}