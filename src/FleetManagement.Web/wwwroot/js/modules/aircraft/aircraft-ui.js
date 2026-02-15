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



async function refreshAircraftUI() {
    await refreshAircraftList();
    await refreshAircraftStats();
}



