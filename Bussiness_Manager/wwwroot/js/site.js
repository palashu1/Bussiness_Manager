if (TempData["ShowAlert"] != null) {
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            Swal.fire({
                title: 'error',
                text: '@ViewBag.message',
                icon: 'error',
                timer: 5000,
                timerProgressBar: true,
                didClose: () => {
                    window.location.href = '@Url.Action("addCustomer", "Selling")';
                }
            });
                });
    </script>
}





