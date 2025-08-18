interface TaskReadDTO {
    id: string;
    title: string;
    description: string;
    deadline: string;
    taskState: string;
    tags: string[];
}

export default TaskReadDTO;
