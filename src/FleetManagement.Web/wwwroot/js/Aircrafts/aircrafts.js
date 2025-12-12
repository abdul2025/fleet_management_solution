// Sidebar Toggle Functionality
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.getElementById('sidebar');
    const mainContent = document.getElementById('mainContent');
    const toggleIcon = sidebarToggle.querySelector('.toggle-icon');




    let sidebarOpen = true;

    // Toggle Sidebar
    function toggleSidebar() {
        sidebarOpen = !sidebarOpen;

        sidebar.classList.toggle('closed', !sidebarOpen);
        sidebarToggle.classList.toggle('open', sidebarOpen);

        toggleIcon.classList.toggle('fa-chevron-right', !sidebarOpen);
        toggleIcon.classList.toggle('fa-xmark', sidebarOpen);

        if (window.innerWidth > 992) {
            mainContent.classList.toggle('shifted', sidebarOpen);
            sidebarToggle.classList.toggle('shifted', sidebarOpen);
        } else {
            mainContent.classList.remove('shifted');
            sidebarToggle.classList.remove('shifted');
        }
    }

    sidebarToggle.addEventListener('click', toggleSidebar);

    // Handle Window Resize
    window.addEventListener('resize', () => {
        if (window.innerWidth <= 992) {
            mainContent.classList.remove('shifted');
            sidebarToggle.classList.remove('shifted');
        } else if (sidebarOpen) {
            mainContent.classList.add('shifted');
            sidebarToggle.classList.add('shifted');
        }
    });

    // Active Menu Item
    const menuItems = document.querySelectorAll('.menu-item');

    menuItems.forEach(item => {
        item.addEventListener('click', function(e) {
            e.preventDefault();

            menuItems.forEach(i => i.classList.remove('active'));
            this.classList.add('active');

            // Close sidebar on mobile after clicking
            if (window.innerWidth <= 768) {
                setTimeout(() => {
                    sidebar.classList.add('closed');
                    sidebarToggle.classList.remove('open');
                    toggleIcon.classList.remove('fa-xmark');
                    toggleIcon.classList.add('fa-chevron-right');
                    sidebarOpen = false;
                }, 300);
            }
        });
    });

    // Close sidebar when clicking outside on mobile
    document.addEventListener('click', (e) => {
        if (window.innerWidth <= 768 && sidebarOpen) {
            if (!sidebar.contains(e.target) && !sidebarToggle.contains(e.target)) {
                sidebar.classList.add('closed');
                sidebarToggle.classList.remove('open');
                toggleIcon.classList.remove('fa-xmark');
                toggleIcon.classList.add('fa-chevron-right');
                sidebarOpen = false;
            }
        }
    });

    // Initialize on page load
    if (window.innerWidth > 992 && sidebarOpen) {
        mainContent.classList.add('shifted');
        sidebarToggle.classList.add('shifted');
        sidebarToggle.classList.add('open');
    }
