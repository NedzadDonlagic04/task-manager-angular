import { Type } from 'class-transformer';

export default class TaskReadDTO {
    public readonly id!: string;

    public readonly title!: string;

    public readonly description!: string;

    @Type(() => Date)
    public readonly createdAt!: Date;

    @Type(() => Date)
    public readonly deadline!: Date | null;

    public readonly taskStateName!: string;

    public readonly tagNames!: string[];
}
