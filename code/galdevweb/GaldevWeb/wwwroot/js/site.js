// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function initializeImagePreview() {
    // Create a container for the preview image
    const imagePreview = document.createElement("img");
    imagePreview.style.position = "absolute";
    imagePreview.style.display = "none";
    imagePreview.style.width = "25%"; // Set image width to 1/3rd of the page width
    imagePreview.style.filter = "drop-shadow(2px 2px 8px rgba(0, 0, 0, 0.5))"; // Add drop-shadow style
    imagePreview.style.borderRadius = ".5rem";
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
        const imageWidth = imagePreview.offsetWidth;
        const offset = 80; // Distance from the mouse

        // Determine the image position based on mouse position
        let posX;
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

        imagePreview.style.left = posX + "px";
        imagePreview.style.top = mouseY + "px"; // Top of the image at the mouse position
    }

    // Attach mouseover, mouseout, and mousemove events to all <a> elements with data-image attribute
    const links = document.querySelectorAll("a[data-image]");
    links.forEach(function (link) {
        link.addEventListener("mouseover", showImage);
        link.addEventListener("mouseout", hideImage);
        link.addEventListener("mousemove", moveImage);
    });
}
