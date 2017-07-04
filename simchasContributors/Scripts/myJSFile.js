$(function () {
    $(".checkbox-switch").bootstrapSwitch();

    $('.add-Deposit').on("click", function () {
        var id = $(this).closest("tr").attr("data-personID");
        $('#DepositpersonId').val(id);
    })

    $('.btn-edit-person').on('click', function () {
        var row = $(this).closest('tr');
        $('#edit_Id').val($(row).attr('data-personID'));
        $('#edit_firstName').val($(row).find('.first-name').text());
        $('#edit_lastName').val($(row).find('.last-name').text());
        $('#edit_email').val($(row).find('.email').text());
        $('#edit_cellPhone').val($(row).find('.cell-phone').text());
        if ($(row).find('.alwaysInclude').attr('data-include') === "True") {
            $('#edit_alwaysInclide').prop('checked', true)
        } else {
            $('#edit_alwaysInclide').prop('checked', false)
        }
        alwaysInclude
    })

    $('#contributorSearch').keyup(function () {
        var valThis = $(this).val().toLowerCase();
        if (valThis == "") {
            $('.table-row').show();
        } else {
            $('tr.table-row').each(function () {
                var text = $(this).text().toLowerCase();
                (text.indexOf(valThis) >= 0) ? $(this).show() : $(this).hide();
            });
        };
    });

    $('#clearSearch').on('click', function () {
        $('#contributorSearch').val('');
        $('.table-row').show();
    })

})