function DeleteConfirm(title) {
    var self = this;
    self.onConfirm = null;
    self.title = title;

    $('#deleteConfirm .modal-title').text(self.title);

    $('#deleteConfirm').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); // Button that triggered the modal
        var deleteUrl = button.data('deleteurl');
        var modal = $(this);

        modal.find('button[data-confirm=true]').click(function () {
            modal.modal('hide');

            $.ajax({
                url: deleteUrl,
                type: 'DELETE'
            }).done(function () {
                if (isFunction(self.onConfirm))
                    self.onConfirm();
            });
        });
    });
}