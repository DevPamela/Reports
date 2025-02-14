document.addEventListener("DOMContentLoaded", function () {
    // Configure global Toastr options
    toastr.options = {
        "closeButton": true,                 // Display a close button on the notification
        "debug": false,                      // Disable debug mode
        "newestOnTop": true,                 // Display newer notifications on top
        "progressBar": true,                 // Show a progress bar for the timeout
        "positionClass": "toast-top-right",  // Position: top-right corner
        "preventDuplicates": true,           // Prevent duplicate notifications
        "onclick": null,                     // Action when the notification is clicked
        "showDuration": "300",               // Animation duration (show) in milliseconds
        "hideDuration": "1000",              // Animation duration (hide) in milliseconds
        "timeOut": "5000",                   // Time before the notification disappears (milliseconds)
        "extendedTimeOut": "1000",           // Extra time before hiding on hover
        "showEasing": "swing",               // Easing function for show animation
        "hideEasing": "linear",              // Easing function for hide animation
        "showMethod": "fadeIn",              // Animation method for showing
        "hideMethod": "fadeOut"              // Animation method for hiding
    };

    // Fetch notification meta tags
    const notificationMeta = document.querySelector('meta[name="x-notification"]');
    const notificationTypeMeta = document.querySelector('meta[name="x-notification-type"]');

    // Display notification if present
    if (notificationMeta) {
        const notification = notificationMeta.getAttribute('content');
        const notificationType = notificationTypeMeta?.getAttribute('content') || "info";

        // Show the notification based on its type
        switch (notificationType) {
            case "success":
                toastr.success(notification);
                break;
            case "error":
                toastr.error(notification);
                break;
            case "warning":
                toastr.warning(notification);
                break;
            default:
                toastr.info(notification);
                break;
        }
    }
});
