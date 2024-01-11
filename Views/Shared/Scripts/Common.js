// Shared/Scripts/Common.js

function changeURL(url) {
    var links = document.querySelectorAll('#menu a');
    links.forEach(function (link) {
        link.classList.remove('active');
    });

    var activeLink = document.querySelector(`#menu a[href="${url}"]`);
    if (activeLink) {
        activeLink.classList.add('active');
    }

    history.pushState(null, null, url);
}
