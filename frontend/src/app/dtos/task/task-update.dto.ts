export default class TaskUpdateDTO {
    public constructor(
        public readonly title: string,
        public readonly description: string,
        public readonly deadline: Date | null,
        public readonly taskStateId: number,
        public readonly tagIds: number[],
    ) {}
}
