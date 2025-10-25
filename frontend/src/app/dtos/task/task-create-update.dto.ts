export default class TaskCreateUpdateDTO {
    public constructor(
        public readonly title: string,
        public readonly description: string,
        public readonly deadline: Date | null,
        public readonly tagIds: string[],
    ) {}
}
