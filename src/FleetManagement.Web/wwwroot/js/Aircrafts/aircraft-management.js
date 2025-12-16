// =======================
// MODAL HANDLING
// =======================
async function openCreateModal() {
    try {
        await openAircraftModal('/Aircrafts/CreateModal');
    } catch (error) {
        console.error(error);
        showToast('Failed to open create modal', 'error');
    } 
}

async function openEditModal(id) {

    try {
        await openAircraftModal(`/Aircrafts/EditModal?id=${id}`);
    } catch (error) {
        console.error(error);
        showToast('Failed to open edit modal', 'error');
    } 
}

function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function openAircraftModal(url) {
    const modalOverlay = document.getElementById('modalOverlay');
    const modalContent = document.getElementById('modalContent');

    try {

        const response = await fetch(url);
        if (!response.ok) {
            showToast('Failed to load modal content', 'error');
            return;
        }

        const html = await response.text();
        modalContent.innerHTML = html;
        modalOverlay.classList.add('active');

        // Bind close buttons
        modalContent.querySelectorAll('[data-action="close-modal"]')
            .forEach(btn => btn.addEventListener('click', closeModal));

        // Bind form submit if exists
        const form = modalContent.querySelector('form');
        if (!form) return;
        form.addEventListener('submit', createOrUpdateAircraft);
    } catch (error) {
        console.error(error);
        showToast('Error loading modal', 'error');
    }
}

function closeModal() {
    const modalOverlay = document.getElementById('modalOverlay');
    modalOverlay.classList.remove('active');
    document.getElementById('modalContent').innerHTML = '';
}

function closeModalOnOverlay(e) {
    if (e.target.id === 'modalOverlay') closeModal();
}

// =======================
// CREATE / UPDATE AIRCRAFT
// =======================
async function createOrUpdateAircraft(event) {
    event.preventDefault();
    const form = event.target;
    const url = form.action;
    const formData = new FormData(form);

    const submitBtn = form.querySelector('#submitBtn');
    submitBtn.disabled = true;
    submitBtn.classList.add('loading');

    try {
        showSpinner();

        const response = await fetch(url, { method: 'POST', body: formData });
        if (!response.ok) {
            showToast('Server error. Please try again.', 'error');
            return;
        }

        const result = await response.json();
        if (result.success) {
            showToast('Aircraft saved successfully', 'success');
            closeModal();
            await refreshAircraftList();
            await refreshAircraftStats();
        } else {
            displayFormErrors(form, result.errors);
            showToast('Please fix the errors in the form', 'error');
        }
    } catch (error) {
        console.error(error);
        showToast('Network error. Please try again.', 'error');
    } finally {
        submitBtn.disabled = false;
        submitBtn.classList.remove('loading');
        hideSpinner();
    }
}

// =======================
// DELETE AIRCRAFT
// =======================
async function deleteAircraft(aircraftId, regNumber) {
    if (!aircraftId) return;

    const confirmed = await showDeleteConfirmation(regNumber);
    if (!confirmed) return;

    const btn = document.querySelector(`.aircraft-card[data-id="${aircraftId}"] .btn-delete`);
    if (btn) {
        btn.disabled = true;
        btn.classList.add('loading');
    }

    try {
        showSpinner();
        const response = await fetch(`/Aircrafts/Delete/${aircraftId}`, { method: 'POST' });
        if (!response.ok) {
            showToast('Server error. Could not delete aircraft', 'error');
            return;
        }

        const result = await response.json();
        if (result.success) {
            showToast('Aircraft deleted successfully', 'success');
            await refreshAircraftList();
            await refreshAircraftStats();
        } else {
            showToast(result.message || 'Failed to delete aircraft', 'error');
        }
    } catch (error) {
        console.error(error);
        showToast('Network error. Please try again.', 'error');
    } finally {
        if (btn) {
            btn.disabled = false;
            btn.classList.remove('loading');
        }
        hideSpinner();
    }
}

// =======================
// REFRESH LIST / STATS
// =======================
async function refreshAircraftStats() {
    const statsContainer = document.getElementById('aircraftStats');
    if (!statsContainer) return;

    try {
        showSpinner();
        const response = await fetch('/Aircrafts/AircraftStatsPartial');
        if (response.ok) {
            const html = await response.text();
            statsContainer.innerHTML = html;
        } else {
            showToast('Failed to refresh stats', 'error');
        }
    } catch (error) {
        console.error(error);
        showToast('Error refreshing stats', 'error');
    } finally {
        hideSpinner();
    }
}

async function refreshAircraftList() {
    const aircraftList = document.getElementById('aircraftList');
    if (!aircraftList) return;

    try {
        showSpinner();
        const response = await fetch('/Aircrafts/AircraftListPartial');
        if (response.ok) {
            const html = await response.text();
            aircraftList.innerHTML = html;
        } else {
            showToast('Failed to refresh aircraft list', 'error');
        }
    } catch (error) {
        console.error(error);
        showToast('Error refreshing list', 'error');
    } finally {
        hideSpinner();
    }
}

// =======================
// SEARCH AIRCRAFT
// =======================
const searchInput = document.getElementById('aircraftSearchInput');
let searchTimeout;

searchInput.addEventListener('input', function () {
    const query = this.value.trim();

    if (query.length < 2) {
        refreshAircraftList();
        return;
    }

    clearTimeout(searchTimeout);
    searchTimeout = setTimeout(() => searchAircraft(query), 300);
});

async function searchAircraft(query) {
    try {
        showSpinner();
        const response = await fetch(`/Aircrafts/Search?query=${encodeURIComponent(query)}`);
        if (response.ok) {
            const html = await response.text();
            document.getElementById('aircraftList').innerHTML = html;
        } else {
            showToast('Search failed', 'error');
        }
    } catch (error) {
        console.error(error);
        showToast('Error during search', 'error');
    } finally {
        hideSpinner();
    }
}

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
