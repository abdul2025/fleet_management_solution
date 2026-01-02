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

// =======================
// DELETE CONFIRMATION MODAL
// =======================
async function showDeleteConfirmation(regNumber) {
    try {
        return await new Promise(resolve => {
            const overlay = document.getElementById('deleteModalOverlay');
            const regText = document.getElementById('deleteAircraftReg');
            const confirmBtn = document.getElementById('confirmDeleteBtn');
            const cancelBtn = document.getElementById('cancelDeleteBtn');

            if (!overlay || !confirmBtn || !cancelBtn || !regText) {
                console.error('Delete confirmation modal elements not found');
                resolve(false);
                return;
            }

            // Set aircraft registration number
            regText.textContent = regNumber;

            // Show modal
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
    } catch (error) {
        console.error(error);
        showToast('Failed to open delete confirmation', 'error');
        return false;
    }
}


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


function displayFormErrors(form, errors) {
    // Clear previous errors
    form.querySelectorAll('.field-error').forEach(el => el.remove());

    if (!errors) return;

    for (const fieldName in errors) {
        const field = form.querySelector(`[name="${fieldName}"]`);
        if (!field) continue;

        const errorDiv = document.createElement('div');
        errorDiv.className = 'field-error';
        errorDiv.style.color = 'red';
        errorDiv.style.fontSize = '0.85rem';
        errorDiv.textContent = errors[fieldName];
        
        // Insert the error message after the input field
        field.insertAdjacentElement('afterend', errorDiv);
    }
}


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



function initJsonDropUploader() {
    try {
        const dropZone = document.getElementById('aircraftList');
        const fileInput = document.getElementById('jsonFileInput');

        const title = document.getElementById('uploadTitle');
        const hint = document.getElementById('uploadHint');
        const status = document.getElementById('jsonUploadStatus');
        const fileNameLabel = document.getElementById('uploadedFileName');
        const processBtn = document.getElementById('processJsonBtn');
        const cancelBtn = document.getElementById('cancelJsonBtn');

        if (!dropZone || !fileInput) return;

        let pendingFile = null;

        // ========================
        // Prevent default drag/drop on the whole document
        // ========================
        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
            document.addEventListener(eventName, e => {
                e.preventDefault();
                e.stopPropagation();
            });
        });

        // ========================
        // Drop zone click
        // ========================
        dropZone.addEventListener('click', e => {
            if (e.target.closest('button')) return;
            fileInput.click();
        });

        // ========================
        // File input change
        // ========================
        fileInput.addEventListener('change', () => {
            if (fileInput.files.length > 0) {
                handleFile(fileInput.files[0]);
            }
        });

        // ========================
        // Drag over / drag leave
        // ========================
        dropZone.addEventListener('dragover', e => {
            e.preventDefault();
            dropZone.classList.add('drag-over');
        });

        dropZone.addEventListener('dragleave', () => {
            dropZone.classList.remove('drag-over');
        });

        // ========================
        // Drop handler
        // ========================
        dropZone.addEventListener('drop', e => {
            e.preventDefault();
            e.stopPropagation(); // <- important
            dropZone.classList.remove('drag-over');

            const file = e.dataTransfer.files?.[0];
            if (!file) return;

            handleFile(file);
        });

        // ========================
        // Handle file
        // ========================
        function handleFile(file) {
            if (file.type !== 'application/json' && !file.name.endsWith('.json')) {
                showToast('Only JSON files are allowed', 'error');
                return;
            }

            if (file.size > 5 * 1024 * 1024) {
                showToast('File size exceeds 5MB', 'error');
                return;
            }

            pendingFile = file;

            title.textContent = 'JSON File Uploaded';
            hint.textContent = 'Review and confirm before processing';

            fileNameLabel.textContent =
                `${file.name} (${(file.size / 1024).toFixed(1)} KB)`;

            status.classList.remove('hidden');

            // Hide the "Add First Aircraft" button
            const addBtn = dropZone.querySelector('.btn-new-aircraft');
            if (addBtn) addBtn.style.display = 'none';
        }

        // ========================
        // Process file
        // ========================
        processBtn?.addEventListener('click', async () => {
            if (!pendingFile) {
                showToast('No file selected', 'error');
                return;
            }

            try {
                showSpinner();

                const formData = new FormData();
                formData.append('file', pendingFile);

                const response = await fetch('/api/aircrafts/upload-json', {
                    method: 'POST',
                    body: formData
                });

                if (!response.ok) {
                    const msg = await response.text();
                    showToast(msg || 'Failed to upload file', 'error');
                    return;
                }

                const result = await response.json();

                showToast(
                    `Import completed: ${result.successCount} aircraft created`,
                    'success'
                );

                if (result.errors?.length) {
                    console.warn('Import errors:', result.errors);
                    showToast(
                        `${result.errors.length} records failed. Check console.`,
                        'warning'
                    );
                }

                // Reset UI
                resetUploader();

                // Refresh UI
                await refreshAircraftList();
                await refreshAircraftStats();

            } catch (error) {
                console.error(error);
                showToast('Upload failed. Please try again.', 'error');
            } finally {
                hideSpinner();
            }
        });

        // ========================
        // Cancel file
        // ========================
        cancelBtn?.addEventListener('click', resetUploader);

        function resetUploader() {
            pendingFile = null;
            fileInput.value = '';

            title.textContent = 'No Aircraft Found';
            hint.innerHTML =
                `Get started by adding your first aircraft<br /><small>or drag & drop a JSON file</small>`;

            status.classList.add('hidden');

            const addBtn = dropZone.querySelector('.btn-new-aircraft');
            if (addBtn) addBtn.style.display = 'inline-flex';
        }

    } catch (error) {
        console.error(error);
        showToast('Failed to initialize JSON uploader', 'error');
    }
}

document.addEventListener('DOMContentLoaded', () => {
    initJsonDropUploader();
});
