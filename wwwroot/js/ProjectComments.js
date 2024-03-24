// Function to load comments
function loadComments(projectId) {
    $.ajax({
        url: '/ProjectManagement/ProjectComment/GetComments?projectId=' + projectId,
        method: 'GET',
        success: function (data) {
            var commentsHtml = '';
            if (Array.isArray(data)) {
                for (var i = 0; i < data.length; i++) {
                    commentsHtml += '<div class="comment">';
                    commentsHtml += '<p>' + data[i].content + '</p>';
                    commentsHtml += '<span>Posted on: ' + new Date(data[i].createdDate).toLocaleString() + '<span>';
                    commentsHtml += '</div>';
                }
                $('#commentsList').html(commentsHtml);
            } else {
                console.error('Invalid data format received for comments.');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error loading comments:', error);
        }
    });
}

$(document).ready(function () {
    var projectId = $('#projectComments input[name="ProjectId"]').val();
    // Call loadComments on page load
    loadComments(projectId);
    // Submit event for addCommentForm
    $('#addCommentForm').submit(function (e) {
        e.preventDefault();
        var formData = {
            ProjectId: projectId,
            Content: $('#projectComments textarea[name="Content"]').val()
        };
        $.ajax({
            url: '/ProjectManagement/ProjectComment/AddComment',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    $('#projectComments textarea[name="Content"]').val('');
                    // Reload comments after successful addition
                    loadComments(projectId);
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error adding comment:', error);
            }
        });
    });
});