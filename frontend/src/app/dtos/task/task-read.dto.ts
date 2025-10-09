export default interface TaskReadDTO {
    id: string;
    title: string;
    description: string;
    createdAt: string;
    deadline: string | null;
    taskStateName: string;
    tagNames: string[];
}
