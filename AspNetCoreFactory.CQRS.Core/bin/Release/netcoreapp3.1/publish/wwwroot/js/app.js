
$(function () {

   // Bootstrap 'disabled' class does not stop click events.
   // This will prevent this.

   $('.disabled').on('click', function (e) { e.preventDefault(); return false; });
});
