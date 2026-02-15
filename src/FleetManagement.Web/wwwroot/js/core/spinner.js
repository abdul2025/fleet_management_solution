// =======================
// TOAST & SPINNER
// =======================
function showToast(message, type = 'success') {
    const toast = document.createElement('div');
    toast.className = `toast ${type} show`;
    toast.textContent = message;
    document.body.appendChild(toast);

    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

function showSpinner() {
    const overlay = document.getElementById('loadingSpinnerOverlay');
    if (overlay) overlay.style.display = 'flex';
}

function hideSpinner() {
    const overlay = document.getElementById('loadingSpinnerOverlay');
    if (overlay) overlay.style.display = 'none';
}

