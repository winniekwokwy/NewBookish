$(document).ready(function () {
    // DELETE logic
    $(document).on('click', '.delete-btn', function () {
        var bookId = $(this).data('id');
        if (confirm("Are you sure you want to delete this book?")) {
            $.ajax({
                url: '/Home/Delete',
                type: 'POST',
                data: { id: bookId },
                success: function (response) {
                    if (response.success) {
                        $('.delete-btn[data-id="' + bookId + '"]').closest('tr').remove();
                    }
                }
            });
        }
    });

    function validateInput(input, form) {
        var value = input.val().trim(); // Get the value of the current input field
        var totalCopies = form.find('input[name="NoOfCopies"]').val(); // Get TotalCopies value
        var availableCopies = form.find('input[name="AvailableCopies"]').val(); // Get AvailableCopies value
        const error = document.getElementById("Error");
        //error.textContent = ""; // Clear the error message
    
        // Check if the field is empty
        if (value === '') {
            input.addClass('is-invalid'); // Add Bootstrap invalid class
            input.next('.invalid-feedback').remove(); // Remove existing error message
            error.textContent = "This field is required.";
            //input.after('<div class="invalid-feedback">This field is required.</div>'); // Add error message
            return false;
        }
    
        // Check specific validation rules
        if (input.attr('name') === "AvailableCopies" && parseInt(value) > parseInt(totalCopies)) {
            input.addClass('is-invalid');
            input.next('.invalid-feedback').remove();
            //input.after('<div class="invalid-feedback">Available copies should not be greater than total copies.</div>');
            error.textContent = "Available copies should not be greater than total copies.";
            return false;
        }
    
        if (input.attr('name') === "NoOfCopies" && parseInt(value) < parseInt(availableCopies)) {
            input.addClass('is-invalid');
            input.next('.invalid-feedback').remove();
            error.textContent = "Total copies should not be less than available copies.";
            //input.after('<div class="invalid-feedback">Total copies should not be less than available copies.</div>');
            return false;
        }
    
        // If all validations pass
        input.removeClass('is-invalid'); // Remove invalid class
        input.next('.invalid-feedback').remove(); // Remove error message
        return true;
    }

    $(document).on('input', 'form[id^="editForm_"] input', function () {
        var input = $(this); // The current input field
        var form = input.closest('form'); // Get the parent form of the current input field
    
        // Call the reusable validation function
        validateInput(input, form);
    });

   /*  // Real-time validation for input fields
    $(document).on('input', 'form[id^="editForm_"] input', function () {
        var input = $(this);
        var value = input.val().trim();
        var form = input.closest('form'); // Get the parent form of the current input field
        var totalCopies = form.find('input[name="NoOfCopies"]').val(); // Get TotalCopies value
        var availableCopies = form.find('input[name="AvailableCopies"]').val(); // Get AvailableCopies value
        const error = document.getElementById("Error");
        error.textContent = "";

        // Example validation: Check if the field is empty
        if (value === '') {
            input.addClass('is-invalid'); // Add Bootstrap invalid class
            input.next('.invalid-feedback').remove(); // Remove existing error message
            error.textContent = "This field is required.";
            //input.after('<div class="invalid-feedback">This field is required.</div>'); // Add error message
        } else {
            console.log("input: " + input + ", value: " + value);
            console.log("TotalCopies: " + totalCopies+ ", AvailableCopies: " + availableCopies);
            if (input.attr('name') == "AvailableCopies" && parseInt(value) > totalCopies) {
                    input.addClass('is-invalid'); // Add Bootstrap invalid class
                    input.next('.invalid-feedback').remove(); // Remove existing error message
                    error.textContent = "Available copies should not be greater than total copies.";
                    //input.after('<div class="invalid-feedback">Available copies should not be greater than total copies.</div>'); // Add error message
            }
            else if (input.attr('name') == "NoOfCopies" && parseInt(value) < availableCopies) {
                input.addClass('is-invalid'); // Add Bootstrap invalid class
                input.next('.invalid-feedback').remove(); // Remove existing error message
                error.textContent = "Total copies should not be less than available copies.";
                //input.after('<div class="invalid-feedback">Total copies should not be less than available copies.</div>'); // Add error message
            }
            else{
                console.log("input validation passed");
                input.removeClass('is-invalid'); // Remove invalid class
                input.next('.invalid-feedback').remove(); // Remove error message
            }
        }
    });
 */
    // EDIT logic
    $(document).on('click', '.edit-btn', function () {
        var bookId = $(this).data('id');
        var originalRow = $('#row_' + bookId); // The original row
        var editRow = $('#editRow_' + bookId); // The edit row

        // Check if the edit row is already visible
        if (editRow.is(':visible')) {
            editRow.hide(); // Hide the edit row if it's already open
            originalRow.show(); // Show the original row again
            return;
        }

        // Hide the original row and load the partial view into the edit row
        originalRow.hide();
        $.get('/Home/EditPartial', { id: bookId }, function (data) {
            editRow.find('td').html(data); // Insert the partial view into the row
            editRow.show(); // Show the edit row
        });
    });

    // CANCEL edit
    $(document).on('click', '.cancel-edit-btn', function () {
        var bookId = $(this).data('id');
        var originalRow = $('#row_' + bookId); // The original row
        var editRow = $('#editRow_' + bookId); // The edit row

        // Clear the input fields in the edit form
        editRow.find('form')[0].reset(); // Reset the form fields

        editRow.hide(); // Hide the edit row
        originalRow.show(); // Show the original row
    });

    // Validate the form before submission
    $(document).on('submit', 'form[id^="editForm_"]', function (e) {
        e.preventDefault(); // Prevent the default form submission behavior
    
        var form = $(this);
        var isValid = true;
        const error = document.getElementById("Error");
        //error.textContent = ""; 
    
        // Validate all input fields in the form
        form.find('input').each(function () {
            var input = $(this);
            if (!validateInput(input, form)) {
                isValid = false; // If any field is invalid, set isValid to false
        
            }
        });
    
        // If the form is valid, proceed with the AJAX request
        if (isValid) {
            var bookId = form.find('input[name="Id"]').val(); // Extract the book ID
            var formData = form.serialize(); // Serialize the form data

    
            // Send the AJAX request to update the book
            $.post('/Home/Update', formData, function (response) {
                if (response.success) {
                    // Update the table row with the new data
                    var row = $('#row_' + bookId);
                    row.find('td:nth-child(1)').text(response.data.title);
                    row.find('td:nth-child(2)').text(response.data.author);
                    row.find('td:nth-child(3)').text(response.data.noOfCopies);
                    row.find('td:nth-child(4)').text(response.data.availableCopies);
    
                    // Hide the edit row and show the updated original row
                    $('#editRow_' + bookId).hide();
                    row.show();
                } else {
                    error.textContent = response.message;
                }
            });
        } else{
            if (!error.textContent){
                error.textContent = "The change is not valid. Please check and try again.";
            }
        }
    });

    // SAVE edited book
   /*  $(document).on('submit', 'form[id^="editForm_"]', function (e) {
        e.preventDefault(); // Prevent the default form submission behavior

        var form = $(this); // Get the specific form being submitted
        var bookId = form.find('input[name="Id"]').val(); // Extract the book ID
        var formData = form.serialize(); // Serialize the form data
        const error = document.getElementById("Error");
        error.textContent = ""; 
        
        // Send the AJAX request to update the book
        $.post('/Home/Update', formData, function (response) {
            if (response.success) {
                // Update the table row with the new data
                var row = $('#row_' + bookId);
                row.find('td:nth-child(1)').text(response.data.title);
                row.find('td:nth-child(2)').text(response.data.author);
                row.find('td:nth-child(3)').text(response.data.noOfCopies);
                row.find('td:nth-child(4)').text(response.data.availableCopies);

                // Hide the edit row and show the updated original row
                $('#editRow_' + bookId).hide();
                row.show();
            }  else {
                error.textContent = response.message;
            } 
        });
    }); */
});
