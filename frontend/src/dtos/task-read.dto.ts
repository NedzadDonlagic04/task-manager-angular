interface TaskReadDTO {
    id: string;
    title: string;
    description: string;
    deadline: string;
    taskState: string;
    tagIds: string[];
}

export default TaskReadDTO;
