interface TaskCreateDTO {
    title: string;
    description: string;
    deadline: string | null;
    tagIds: number[];
}

export default TaskCreateDTO;
