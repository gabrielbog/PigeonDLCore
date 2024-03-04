function getCurrentThemeString() {
    const themeSetting = localStorage.getItem("themeSetting");
    if (themeSetting == "light") {
        return "Theme: Light";
    }
    else if (themeSetting == "dark") {
        return "Theme: Dark";
    }
}

function setLightTheme() {
    localStorage.setItem("themeSetting", "light");
    document.documentElement.setAttribute("data-bs-theme", "light");
}

function setDarkTheme() {
    localStorage.setItem("themeSetting", "dark");
    document.documentElement.setAttribute("data-bs-theme", "dark");
}

function setDefaultTheme() {
    const themeSetting = localStorage.getItem("themeSetting");

    if (themeSetting == "light") {
        setLightTheme();
    }
    else if (themeSetting == "dark") {
        setDarkTheme();
    }
    else {
        if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            setLightTheme();
        }
        else {
            setDarkTheme();
        }
    }
}

setDefaultTheme();

window.addEventListener("DOMContentLoaded", function () {

    const settingsDropdown = document.getElementById("settings-dropdown");
    settingsDropdown.addEventListener("show.bs.dropdown", function () {

        const currentThemeSetting = localStorage.getItem("themeSetting");
        var themeToggle = document.getElementById("theme-toggle");
        themeToggle.innerHTML = getCurrentThemeString();

        themeToggle.addEventListener("click", function () {

            if (currentThemeSetting == "dark") {
                setLightTheme();
                themeToggle.innerHTML = getCurrentThemeString();
            }
            else if (currentThemeSetting == "light") {
                setDarkTheme();
                themeToggle.innerHTML = getCurrentThemeString();
            }
        });
    });
});