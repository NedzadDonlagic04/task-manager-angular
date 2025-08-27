export default interface TaskCreateDTO {
    title: string;
    description: string;
    deadline: string | null;
    tagIds: number[];
}
