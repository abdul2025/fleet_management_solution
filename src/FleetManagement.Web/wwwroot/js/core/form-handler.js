async function submitEntityForm(event, onSuccessRefresh) {
    event.preventDefault();

    const form = event.target;
    const url = form.action;
    const formData = new FormData(form);

    showSpinner();

    const response = await fetch(url, {
        method: 'POST',
        body: formData
    });

    const result = await response.json();

    if (result.success) {
        closeModal();
        showToast('Saved successfully', 'success');

        if (onSuccessRefresh)
            await onSuccessRefresh();
    } else {
        displayFormErrors(form, result.errors);
    }

    hideSpinner();
}


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
