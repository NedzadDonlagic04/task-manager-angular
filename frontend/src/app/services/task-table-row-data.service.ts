import TaskReadDTO from "../dtos/task-read.dto";

export class TaskTableRowData {
    rowNum: number;
    private task: TaskReadDTO;

    constructor(rowNum: number, task: TaskReadDTO) {
        this.rowNum = rowNum;
        this.task = task;
    }

    get title(): string {
        return this.task.title;
    }

    get description(): string {
        return this.task.description;
    }

    get deadline(): string {
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
