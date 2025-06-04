namespace MyLib.Extensions;

/// <summary>
/// 數字格式處理
/// </summary>
public static class NumberFormatExtension
{
    /// <summary>
    /// string.Format("{0:#,##0}", value)。
    /// <para>input: 0 ---> return: 0</para>
    /// <para>input: 123 ---> return: 123</para>
    /// <para>input: 1234 ---> return: 1,234</para>
    /// <para>input: -1234 ---> return: -1,234</para>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string WithThousandsSeparator(this int value)
    {
        return string.Format("{0:#,##0}", value);
    }
}

