export default interface TaskReadDTO {
    id: string;
    title: string;
    description: string;
    created_At: string;
    deadline: string | null;
    taskStateName: string;
    tagNames: string[];
}
