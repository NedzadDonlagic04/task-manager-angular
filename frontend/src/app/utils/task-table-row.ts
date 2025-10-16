import TaskReadDTO from '../dtos/task/task-read.dto';

export class TaskTableRow {
    public readonly rowNum: number;
    private readonly _task: TaskReadDTO;

    public constructor(rowNum: number, task: TaskReadDTO) {
        this.rowNum = rowNum;
        this._task = task;
    }

    public get id(): string {
        return this._task.id;
    }

    public get title(): string {
        return this._task.title;
    }

    public get description(): string {
        return this._task.description;
    }

    public get deadline(): Date | null {
        return this._task.deadline;
    }

    public get createdAt(): Date {
        return this._task.createdAt;
    }

    public get taskStateName(): string {
        return this._task.taskStateName;
    }

    public get tagNames(): string[] {
        return this._task.tagNames;
    }
}
