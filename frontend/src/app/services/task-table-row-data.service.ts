import TaskReadDTO from "../dtos/task-read.dto";

export class TaskTableRowData {
    readonly rowNum: number;
    private readonly task: TaskReadDTO;

    constructor(rowNum: number, task: TaskReadDTO) {
        this.rowNum = rowNum;
        this.task = task;
    }

    get id(): string {
        return this.task.id;
    }

    get title(): string {
        return this.task.title;
    }

    get description(): string {
        return this.task.description;
    }

    get deadline(): string | null {
        return this.task.deadline;
    }

    get createdAt(): string {
        return this.task.created_At;
    }

    get taskStateName(): string {
        return this.task.taskStateName;
    }

    get tagNames(): string[] {
        return this.task.tagNames;
    }
}
