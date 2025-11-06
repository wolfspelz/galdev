// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function initializeImagePreview() {
    // Create a container for the preview image
    const imagePreview = document.createElement("img");
    imagePreview.className = "gd-preview";
    document.body.appendChild(imagePreview);

    // Function to show the image preview
    function showImage(event) {
        const imageUrl = event.target.getAttribute("data-image");
        if (imageUrl) {
            imagePreview.src = "/Image/" + imageUrl;
            imagePreview.style.display = "block";
            moveImage(event);
        }
    }

    // Function to hide the image preview
    function hideImage() {
        imagePreview.style.display = "none";
    }

    // Function to move the image with the mouse
    function moveImage(event) {
        // Determine mouse position and window dimensions
        const mouseX = event.pageX;
        const mouseY = event.pageY;
        const windowWidth = window.innerWidth;
        const windowHeight = window.innerHeight;
        const imageWidth = imagePreview.offsetWidth;
        const imageHeight = imagePreview.offsetHeight;
        const offset = 80; // Distance from the mouse

        // Determine the image position based on mouse position
        let posX;
        let posY = mouseY;

        if (mouseX < windowWidth / 2) {
            // Mouse is in the left half of the content
            posX = mouseX + offset;
            if (posX + imageWidth > windowWidth) {
                posX = windowWidth - imageWidth - 20; // Adjust if image goes beyond the right edge
            }
        } else {
            // Mouse is in the right half of the content
            posX = mouseX - imageWidth - offset;
            if (posX < 0) {
                posX = 20; // Adjust if image goes beyond the left edge
            }
        }

        // Adjust the vertical position to ensure the image is completely within the viewport
        if (posY + imageHeight > windowHeight + window.scrollY) {
            posY = windowHeight + window.scrollY - imageHeight - 20; // Adjust if image goes beyond the bottom edge
        }

        imagePreview.style.left = posX + "px";
        imagePreview.style.top = posY + "px";
    }

    // Attach mouseover, mouseout, and mousemove events to all <a> elements with data-image attribute
    const links = document.querySelectorAll("a[data-image]");
    links.forEach(function (link) {
        link.addEventListener("mouseover", showImage);
        link.addEventListener("mouseout", hideImage);
        link.addEventListener("mousemove", moveImage);
    });
}

function initializeVideoPreview() {
    // Create a container for the preview video
    const videoPreview = document.createElement("video");
    videoPreview.className = "gd-preview";
    videoPreview.autoplay = true;
    videoPreview.loop = true;
    videoPreview.muted = true;
    videoPreview.style.maxWidth = "30rem";
    videoPreview.style.maxHeight = "30rem";
    videoPreview.style.objectFit = "contain";
    document.body.appendChild(videoPreview);

    // Function to show the video preview
    function showVideo(event) {
        const videoUrl = event.target.getAttribute("data-video");
        if (videoUrl) {
            // Get custom max dimensions if specified
            const maxWidth = event.target.getAttribute("data-video-max-width");
            const maxHeight = event.target.getAttribute("data-video-max-height");
            
            if (maxWidth) {
                videoPreview.style.maxWidth = maxWidth;
            } else {
                videoPreview.style.maxWidth = "30rem";
            }
            
            if (maxHeight) {
                videoPreview.style.maxHeight = maxHeight;
            } else {
                videoPreview.style.maxHeight = "30rem";
            }
            
            videoPreview.src = videoUrl;
            videoPreview.style.display = "block";
            videoPreview.play();
            moveVideo(event);
        }
    }

    // Function to hide the video preview
    function hideVideo() {
        videoPreview.style.display = "none";
        videoPreview.pause();
    }

    // Function to move the video with the mouse
    function moveVideo(event) {
        // Determine mouse position and window dimensions
        const mouseX = event.pageX;
        const mouseY = event.pageY;
        const windowWidth = window.innerWidth;
        const windowHeight = window.innerHeight;
        const videoWidth = videoPreview.offsetWidth;
        const videoHeight = videoPreview.offsetHeight;
        const offset = 80; // Distance from the mouse

        // Determine the video position based on mouse position
        let posX;
        let posY = mouseY;

        if (mouseX < windowWidth / 2) {
            // Mouse is in the left half of the content
            posX = mouseX + offset;
            if (posX + videoWidth > windowWidth) {
                posX = windowWidth - videoWidth - 20; // Adjust if video goes beyond the right edge
            }
        } else {
            // Mouse is in the right half of the content
            posX = mouseX - videoWidth - offset;
            if (posX < 0) {
                posX = 20; // Adjust if video goes beyond the left edge
            }
        }

        // Adjust the vertical position to ensure the video is completely within the viewport
        if (posY + videoHeight > windowHeight + window.scrollY) {
            posY = windowHeight + window.scrollY - videoHeight - 20; // Adjust if video goes beyond the bottom edge
        }

        videoPreview.style.left = posX + "px";
        videoPreview.style.top = posY + "px";
    }

    // Attach mouseover, mouseout, and mousemove events to all <a> elements with data-video attribute
    const videoLinks = document.querySelectorAll("a[data-video]");
    videoLinks.forEach(function (link) {
        link.addEventListener("mouseover", showVideo);
        link.addEventListener("mouseout", hideVideo);
        link.addEventListener("mousemove", moveVideo);
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
    initializeImagePreview()
    initializeVideoPreview()
    setHiliteClass()
    $(window).on('hashchange', setHiliteClass)
}

document.addEventListener("DOMContentLoaded", init);
