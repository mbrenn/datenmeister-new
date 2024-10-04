// Class representing the result of a popup creation
export class PopupResult {
    htmlPopupWindow: HTMLElement; // The outer popup window element
    htmlContent: HTMLElement; // The inner content element of the popup

    // Method to close the popup by removing the outer popup window element
    closePopup() {
        this.htmlPopupWindow.remove();
    }
}

// Function to create a popup and return a PopupResult instance
export function createPopup(): PopupResult {
    const result = new PopupResult();

    // Create the outer popup container
    const popup = document.createElement('div');
    popup.classList.add('burn-popup');
    document.body.appendChild(popup);

    // Create the inner content container
    const innerPopup = document.createElement('div');
    innerPopup.classList.add('burn-popup-inner');
    popup.appendChild(innerPopup);

    // Create a close button and add it to the inner content container
    const closeButton = document.createElement('span');
    closeButton.classList.add('burn-popup-close');
    closeButton.innerHTML = '&times;'; // HTML entity for the multiplication sign (×)
    closeButton.onclick = () => result.closePopup(); // Set the click handler to close the popup
    innerPopup.appendChild(closeButton);

    // Assign the created elements to the result object
    result.htmlPopupWindow = popup;
    result.htmlContent = innerPopup;

    // Return the result object, allowing the caller to interact with the popup
    return result;
}