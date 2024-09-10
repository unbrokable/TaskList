using FluentValidation;

namespace TaskList.Application.TaskLists.Commands.CreateTaskList;

public class CreateTaskListCommandValidator : AbstractValidator<CreateTaskListCommand>
{
    public CreateTaskListCommandValidator()
    {
        RuleFor(v => v.Title)
            .MinimumLength(1)
            .MaximumLength(255)
            .NotEmpty();
    }
}
