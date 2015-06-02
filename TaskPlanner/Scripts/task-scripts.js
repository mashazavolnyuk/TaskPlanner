function UpdateTask(taskId) {
    var url = "/Task/UpdateTaskState?taskId=" + taskId;
    SendAjax(url);
}

function UpdateSubTask(subTaskId) {

    var url = "/Task/UpdateSubTaskState?subTaskId=" + subTaskId;
    SendAjax(url);
}

function SendAjax(url) {
    $.ajax({
        url: url,
        type: "POST",
        success: function(data) {
            UpdateTaskView(data);
        },
        error: function() {
            console.log("Ajax not working");
        }
    });
}

function UpdateTaskView(task) {
    var taskBlock = $("#Finish_" + task.Id);
    taskBlock[0].checked = task.isFinish;
    for (var i = 0; i < task.subtasks.length; i++) {
        var subtaskBlock = $("#subtask_isFinish_" + task.subtasks[i].Id);
        subtaskBlock[0].checked = task.subtasks[i].isFinish;
    }
}

function AddSubTask(subtask) {
    var newSubTask = $(subtask);
    var input = newSubTask.find("input");
    var name = input[0].className;
    var id = name.split("_")[1];
    var wrapper = $("#list_" + id);
    wrapper.append(newSubTask);
    var parentTask = $("#Finish_" + id);
    parentTask[0].checked = false;
}

function ShowCreateSubTaskPanel(id) {
    $("#form_" + id).show();

}

function HideCreateSubTaskPanel(id) {
    $("#form_" + id).hide();
}