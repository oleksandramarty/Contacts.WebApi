namespace Contacts.Kernel.Extensions;

public static class StringExtension
{
    public static bool NotNullOrEmpty(this string str)
    {
        return !string.IsNullOrEmpty(str);
    }
}