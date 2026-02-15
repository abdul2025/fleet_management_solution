// Sidebar Elements
const sidebarToggle = document.getElementById('sidebarToggle');
const sidebar = document.getElementById('sidebar');
const toggleIcon = sidebarToggle.querySelector('.toggle-icon');

// Sidebar state
let sidebarOpen = false;

// Apply UI state
function applySidebarState() {
    sidebar.classList.toggle('closed', !sidebarOpen);
    sidebarToggle.classList.toggle('open', sidebarOpen);

    // Icon flip
    toggleIcon.classList.toggle('fa-xmark', sidebarOpen);
    toggleIcon.classList.toggle('fa-chevron-right', !sidebarOpen);

    // Desktop behavior (shift layout)
    if (window.innerWidth > 992) {
        sidebarToggle.classList.toggle('shifted', sidebarOpen);
    } else {
        sidebarToggle.classList.remove('shifted');
    }
}

// Toggle sidebar
function toggleSidebar() {
    sidebarOpen = !sidebarOpen;
    applySidebarState();
}

sidebarToggle.addEventListener('click', toggleSidebar);

// Window resize behavior
window.addEventListener('resize', () => {
    applySidebarState();
});

// Close sidebar when clicking menu item on mobile
document.querySelectorAll('.menu-item').forEach(item => {
    item.addEventListener('click', () => {
        if (window.innerWidth <= 768) {
            sidebarOpen = false;
            applySidebarState();
        }
    });
});

// Close sidebar when clicking outside (mobile only)
document.addEventListener('click', (e) => {
    if (window.innerWidth <= 768 && sidebarOpen) {
        if (!sidebar.contains(e.target) && !sidebarToggle.contains(e.target)) {
            sidebarOpen = false;
            applySidebarState();
        }
    }
});

// ------------------------------------
// INITIALIZATION ON PAGE LOAD
// ------------------------------------
window.addEventListener('DOMContentLoaded', () => {
    sidebarOpen = false;  // Force closed
    applySidebarState();  // Apply UI
});


