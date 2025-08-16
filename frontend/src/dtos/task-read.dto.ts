interface TaskReadDTO {
    id: string;
    title: string;
    description: string;
    created_At: string;
    deadline: string;
    taskStateName: string;
    tagNames: string[];
}

export default TaskReadDTO;
