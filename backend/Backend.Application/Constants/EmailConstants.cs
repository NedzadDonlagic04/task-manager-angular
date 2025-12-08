namespace Backend.Application.Constants;

public static class EmailConstants
{
    public static class LocalPart
    {
        public const int MinLength = 1;
        public const int MaxLength = 64;
    }

    public static class DomainPart
    {
        public const int MinLength = 1;
        public const int MaxLength = 255;

        public const int MinLabelsCount = 2;
        public const int MaxLabelsCount = 20;

        public static class Label
        {
            public const int MinLength = 1;
            public const int MaxLength = 63;
        }
    }
}
