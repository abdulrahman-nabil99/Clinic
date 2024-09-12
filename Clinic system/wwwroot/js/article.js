// Insert image by URL
function insertImageByUrl() {
    const url = prompt("Enter image URL:");
    if (url) {
        const image = new Image();
        image.src = url;
        image.style.maxWidth = "100%"; // Ensure the image fits within the editor
        const editor = document.getElementById("content");
        editor.appendChild(image);
    }
}

// Apply the inline style (for color or font size)
function applyStyle(style, value) {
    document.execCommand("styleWithCSS", false, true);
    document.execCommand("foreColor", false, value); // Apply the style

    clearSelection(); // Unselect the text after applying the style
}

// Handle font size specifically
function applyFontSize(fontSize) {
    const selection = window.getSelection();
    if (!selection.rangeCount) return;

    const range = selection.getRangeAt(0);
    const span = document.createElement("span");
    span.style.fontSize = fontSize + "px";

    range.surroundContents(span);

    clearSelection(); // Unselect the text after applying the font size
}

// Clear the current text selection
function clearSelection() {
    if (window.getSelection) {
        window.getSelection().removeAllRanges();
    } else if (document.selection) {
        document.selection.empty();
    }
}
const postForm = document.getElementById("postForm");
const articelForm = document.getElementById("article-content");
const content = document.getElementById("content");
postForm.addEventListener("submit", (e) => {
    articelForm.value = content.innerHTML;
})