using FluentValidation;

namespace TaskList.Application.TaskLists.Commands.CreateTaskList;

public class UpdateTaskListCommandValidator : AbstractValidator<UpdateTaskListCommand>
{
    public UpdateTaskListCommandValidator()
    {
        RuleFor(v => v.Title)
            .MinimumLength(1)
            .MaximumLength(255)
            .NotEmpty();
    }
}
