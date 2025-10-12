interface DateTimeOffset {
    days: number;
    hours: number;
    minutes: number;
    seconds: number;
    milliseconds: number;
}

class Milliseconds {
    /*
       Thought adding the line below would be funny
       but whoever sees this code in the future might
       not agree so it's gonna be a comment

       public static readonly PER_MILLISECOND = 1;
    */
    public static readonly PER_SECOND = 1_000;
    public static readonly PER_MINUTE = 60 * Milliseconds.PER_SECOND;
    public static readonly PER_HOUR = 60 * Milliseconds.PER_MINUTE;
    public static readonly PER_DAY = 24 * Milliseconds.PER_HOUR;
}

const getTotalMillisecondsFromDateTimeOffset = (
    offset: Partial<DateTimeOffset>,
): number => {
    const totalOffsetInMilliseconds =
        (offset?.days ?? 0) * Milliseconds.PER_DAY +
        (offset?.hours ?? 0) * Milliseconds.PER_HOUR +
        (offset?.minutes ?? 0) * Milliseconds.PER_MINUTE +
        (offset?.seconds ?? 0) * Milliseconds.PER_SECOND +
        (offset?.milliseconds ?? 0);

    return totalOffsetInMilliseconds;
};

export const applyOffset = (
    dateTime: Date,
    offset: Partial<DateTimeOffset>,
): Date => {
    return new Date(
        dateTime.getTime() + getTotalMillisecondsFromDateTimeOffset(offset),
    );
};
