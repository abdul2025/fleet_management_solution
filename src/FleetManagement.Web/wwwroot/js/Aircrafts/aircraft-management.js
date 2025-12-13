// START ** Modal handling for Aircraft Create/Edit JUST OPENING AND CLOSING

async function openCreateModal() {
    await openAircraftModal('/Aircrafts/CreateModal');
}

async function openEditModal(id) {
    await openAircraftModal(`/Aircrafts/EditModal?id=${id}`);
}

async function openAircraftModal(url) {
    const modalOverlay = document.getElementById('modalOverlay');
    const modalContent = document.getElementById('modalContent');

    const response = await fetch(url);
    const html = await response.text();

    modalContent.innerHTML = html;
    modalOverlay.classList.add('active'); // ✅ Show modal with CSS

    // Bind close buttons
    modalContent.querySelectorAll('[data-action="close-modal"]')
        .forEach(btn => btn.addEventListener('click', closeModal));

    // Optional: bind form submit if you have a handler
    const form = modalContent.querySelector('form');
    if (!form) {
        console.warn('No form found in modal');
        return;
    }

    form.addEventListener('submit', createOrUpdateAircraft); // only if you handle form via JS
}

function closeModal() {
    const modalOverlay = document.getElementById('modalOverlay');
    modalOverlay.classList.remove('active'); // ✅ Hide modal
    document.getElementById('modalContent').innerHTML = '';
}

function closeModalOnOverlay(e) {
    if (e.target.id === 'modalOverlay') closeModal();
}
// END ** Modal handling for Aircraft Create/Edit JUST OPENING AND CLOSING







// START ** createOrUpdateAircraft - handles form submission via AJAX


async function createOrUpdateAircraft(event) {
    event.preventDefault(); // Stop the form from submitting normally

    const form = event.target;
    const url = form.action; // Gets the URL from asp-action and asp-route-id
    const formData = new FormData(form);

    const submitBtn = form.querySelector('#submitBtn');
    submitBtn.disabled = true;
    submitBtn.classList.add('loading');

    try {
        const response = await fetch(url, {
            method: 'POST',
            body: formData,
        });

        if (response.ok) {
            const result = await response.json(); // Expect JSON from your controller
            if (result.success) {
                showToast('Aircraft saved successfully', 'success');
                closeModal();
                await refreshAircraftList()
                await refreshAircraftStats();
            } else {
                // Show validation errors returned from server
                displayFormErrors(form, result.errors);
            }
        } else {
            showToast('Server error. Please try again.', 'error');
        }
    } catch (error) {
        console.error(error);
        showToast('Network error. Please try again.', 'error');
    } finally {
        submitBtn.disabled = false;
        submitBtn.classList.remove('loading');
    }
}

// Display server-side validation errors
function displayFormErrors(form, errors) {
    console.log('Displaying form errors:', errors);
    if (!form) return;

    // Clear previous errors
    form.querySelectorAll('.validation-error').forEach(span => {
        span.textContent = '';
        span.classList.remove('show');
    });

    // Loop through errors
    for (const field in errors) {
        const messages = errors[field];
        if (!messages || messages.length === 0) continue;

        // Convert nested field names from dot to underscore
        // const inputName = field.replace(/\./g, '_');
        const errorSpan = form.querySelector(
            `[data-valmsg-for="${field}"], [data-valmsg-for="${field.replace(/\./g, '_')}"]`
        );
        if (errorSpan) {
            errorSpan.textContent = messages.join(', ');
            errorSpan.classList.add('show');
        }
    }
}



// END ** createOrUpdateAircraft - handles form submission via AJAX



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
        const response = await fetch(`/Aircrafts/Delete/${aircraftId}`, {
            method: 'POST'
        });

        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                showToast('Aircraft deleted successfully', 'success');
                await refreshAircraftList();
                await refreshAircraftStats();  // refresh statistics

            } else {
                showToast(result.message || 'Failed to delete aircraft.', 'error');
            }
        } else {
            showToast('Server error. Please try again.', 'error');
        }
    } catch (error) {
        console.error(error);
        showToast('Network error. Please try again.', 'error');
    } finally {
        if (btn) {
            btn.disabled = false;
            btn.classList.remove('loading');
        }
    }
}



function showDeleteConfirmation(regNumber = '') {
    return new Promise((resolve) => {
        const overlay = document.getElementById('deleteModalOverlay');
        const confirmBtn = document.getElementById('confirmDeleteBtn');
        const cancelBtn = document.getElementById('cancelDeleteBtn');
        const regSpan = document.getElementById('deleteAircraftReg');

        regSpan.textContent = regNumber; // show registration number

        overlay.classList.add('active'); // show modal

        const cleanup = () => {
            overlay.classList.remove('active');
            confirmBtn.removeEventListener('click', onConfirm);
            cancelBtn.removeEventListener('click', onCancel);
        };

        const onConfirm = () => { cleanup(); resolve(true); };
        const onCancel = () => { cleanup(); resolve(false); };

        confirmBtn.addEventListener('click', onConfirm);
        cancelBtn.addEventListener('click', onCancel);
    });
}





async function refreshAircraftStats() {
    const statsContainer = document.getElementById('aircraftStats');
    if (!statsContainer) return;

    try {
        const response = await fetch('/Aircrafts/AircraftStatsPartial');
        if (response.ok) {
            const html = await response.text();
            statsContainer.innerHTML = html;
        } else {
            console.warn('Failed to refresh stats');
        }
    } catch (error) {
        console.error(error);
    }
}







// Refresh aircraft list dynamically
async function refreshAircraftList() {
    const aircraftList = document.getElementById('aircraftList');
    if (!aircraftList) return;

    const response = await fetch('/Aircrafts/AircraftListPartial'); // Make sure this returns the partial view of aircraft cards
    if (response.ok) {
        const html = await response.text();
        aircraftList.innerHTML = html;
    }
}

// Get AntiForgeryToken value
function getAntiForgeryToken() {
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    return tokenInput ? tokenInput.value : '';
}





// Simple toast notification
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



