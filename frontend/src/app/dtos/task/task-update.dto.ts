export default interface TaskUpdateDTO {
    title: string;
    description: string;
    deadline: string | null;
    taskStateId: number;
    tagIds: number[];
}
