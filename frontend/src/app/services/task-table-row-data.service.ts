import TaskReadDTO from '../dtos/task-read.dto';

export class TaskTableRowData {
    public readonly rowNum: number;
    private readonly task: TaskReadDTO;

    public constructor(rowNum: number, task: TaskReadDTO) {
        this.rowNum = rowNum;
        this.task = task;
    }

    public get id(): string {
        return this.task.id;
    }

    public get title(): string {
        return this.task.title;
    }

    public get description(): string {
        return this.task.description;
    }

    public get deadline(): string | null {
        return this.task.deadline;
    }

    public get createdAt(): string {
        return this.task.created_At;
    }

    public get taskStateName(): string {
        return this.task.taskStateName;
    }

    public get tagNames(): string[] {
        return this.task.tagNames;
    }
}
