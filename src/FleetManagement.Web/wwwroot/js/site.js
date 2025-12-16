document.addEventListener("DOMContentLoaded", () => {
    const toggleBtn = document.getElementById("theme-toggle");
    const icon = document.getElementById("theme-icon");

    const setTheme = (theme) => {
        document.documentElement.setAttribute("data-theme", theme);
        localStorage.setItem("theme", theme);
        icon.className = theme === "dark" ? "bi bi-moon-stars-fill" : "bi bi-sun-fill";
    };

    // Load saved theme
    const savedTheme = localStorage.getItem("theme") || "light";
    setTheme(savedTheme);

    toggleBtn.addEventListener("click", () => {
        const current = document.documentElement.getAttribute("data-theme");
        setTheme(current === "light" ? "dark" : "light");
    });



    
});

// for Main Menu Toggler
document.addEventListener("DOMContentLoaded", () => {

    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.getElementById('sidebar');
    const mainContent = document.getElementById('mainContent');
    const toggleIcon = sidebarToggle?.querySelector('.toggle-icon'); // optional chaining
    let sidebarOpen = true;

    const navbarCollapse = document.getElementById("navbarContent");
    const navbarToggler = document.querySelector(".navbar-toggler");
    const bsCollapse = new bootstrap.Collapse(navbarCollapse, { toggle: false });

    // -------------------------
    // Sidebar Toggle Function
    // -------------------------
    function toggleSidebar() {
        sidebarOpen = !sidebarOpen;

        sidebar.classList.toggle('closed', !sidebarOpen);
        sidebarToggle.classList.toggle('open', sidebarOpen);

        toggleIcon?.classList.toggle('fa-bars', !sidebarOpen);
        toggleIcon?.classList.toggle('fa-xmark', sidebarOpen);

        if (window.innerWidth > 992) {
            mainContent.classList.toggle('shifted', sidebarOpen);
            sidebarToggle.classList.toggle('shifted', sidebarOpen);
        } else {
            mainContent.classList.remove('shifted');
            sidebarToggle.classList.remove('shifted');
        }
    }

    sidebarToggle?.addEventListener('click', toggleSidebar);

    // -------------------------
    // Navbar Toggler
    // -------------------------
    navbarToggler.addEventListener("click", () => {
        const isShown = navbarCollapse.classList.contains("show");

        if (isShown) {
            bsCollapse.hide();
            if (sidebarToggle) sidebarToggle.style.display = "flex"; // show sidebar toggle
        } else {
            bsCollapse.show();
            if (sidebarToggle) sidebarToggle.style.display = "none"; // hide sidebar toggle
        }
    });

    // -------------------------
    // Close Navbar on Outside Click
    // -------------------------
    document.addEventListener("click", (e) => {
        if (window.innerWidth < 992 && navbarCollapse.classList.contains("show")) {
            if (!navbarCollapse.contains(e.target) && !navbarToggler.contains(e.target)) {
                bsCollapse.hide();
                if (sidebarToggle) sidebarToggle.style.display = "flex"; // restore sidebar toggle
            }
        }
    });

});
