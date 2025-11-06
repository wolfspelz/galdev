// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function initializeMediaPreview() {
    // Create containers for preview image and video
    const imagePreview = document.createElement("img");
    imagePreview.className = "gd-preview";
    document.body.appendChild(imagePreview);

    const videoPreview = document.createElement("video");
    videoPreview.className = "gd-preview";
    videoPreview.autoplay = true;
    videoPreview.loop = true;
    videoPreview.muted = true;
    videoPreview.style.objectFit = "contain";
    document.body.appendChild(videoPreview);

    // Generic function to move preview element with the mouse
    function movePreview(event, previewElement) {
        const mouseX = event.pageX;
        const mouseY = event.pageY;
        const windowWidth = window.innerWidth;
        const windowHeight = window.innerHeight;
        const previewWidth = previewElement.offsetWidth;
        const previewHeight = previewElement.offsetHeight;
        const offset = 80; // Distance from the mouse

        let posX;
        let posY = mouseY;

        if (mouseX < windowWidth / 2) {
            // Mouse is in the left half of the content
            posX = mouseX + offset;
            if (posX + previewWidth > windowWidth) {
                posX = windowWidth - previewWidth - 20;
            }
        } else {
            // Mouse is in the right half of the content
            posX = mouseX - previewWidth - offset;
            if (posX < 0) {
                posX = 20;
            }
        }

        // Adjust vertical position to ensure preview is within viewport
        if (posY + previewHeight > windowHeight + window.scrollY) {
            posY = windowHeight + window.scrollY - previewHeight - 20;
        }

        previewElement.style.left = posX + "px";
        previewElement.style.top = posY + "px";
    }

    // Generic function to show preview based on data attribute
    function showPreview(event) {
        const imageUrl = event.target.getAttribute("data-preview-image");
        const videoUrl = event.target.getAttribute("data-preview-video");
        const maxWidth = event.target.getAttribute("data-preview-max-width");
        const maxHeight = event.target.getAttribute("data-preview-max-height");

        if (imageUrl) {
            videoPreview.style.display = "none";
            videoPreview.pause();
            
            imagePreview.style.maxWidth = maxWidth || "30rem";
            imagePreview.style.maxHeight = maxHeight || "30rem";
            imagePreview.style.objectFit = "contain";
            
            imagePreview.src = "/Image/" + imageUrl;
            imagePreview.style.display = "block";
            movePreview(event, imagePreview);
        } else if (videoUrl) {
            imagePreview.style.display = "none";
            
            videoPreview.style.maxWidth = maxWidth || "30rem";
            videoPreview.style.maxHeight = maxHeight || "30rem";
            
            videoPreview.src = videoUrl;
            videoPreview.style.display = "block";
            videoPreview.play();
            movePreview(event, videoPreview);
        }
    }

    // Generic function to hide all previews
    function hidePreview() {
        imagePreview.style.display = "none";
        videoPreview.style.display = "none";
        videoPreview.pause();
    }

    // Generic function to move the currently visible preview
    function moveCurrentPreview(event) {
        if (imagePreview.style.display === "block") {
            movePreview(event, imagePreview);
        } else if (videoPreview.style.display === "block") {
            movePreview(event, videoPreview);
        }
    }

    // Attach events to all elements with data-preview-image or data-preview-video attributes
    const mediaLinks = document.querySelectorAll("a[data-preview-image], a[data-preview-video]");
    mediaLinks.forEach(function (link) {
        link.addEventListener("mouseover", showPreview);
        link.addEventListener("mouseout", hidePreview);
        link.addEventListener("mousemove", moveCurrentPreview);
    });
}

function setHiliteClass() {
    var hash = window.location.hash.substring(1);
    var $currentHilite = $(".card-body-hilite");
    if (hash) {
        var $targetElement = $("#id-" + hash);
        // Only add class if it's not already on the target element
        if (!$targetElement.hasClass("card-body-hilite")) {
            $currentHilite.removeClass("card-body-hilite");
            $targetElement.addClass("card-body-hilite");
        }
    } else {
        // Remove class when no hash is set
        $currentHilite.removeClass("card-body-hilite");
    }
}

function init() {
    initializeMediaPreview()
    setHiliteClass()
    $(window).on('hashchange', setHiliteClass)
}

document.addEventListener("DOMContentLoaded", init);
