// =======================
// MODAL HANDLING
// =======================





function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
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
