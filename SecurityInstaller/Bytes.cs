using System;
using System.Runtime.InteropServices;

/**
 * This class exists to help format strings denoting byte sizes (KB, MB, etc.)
 * @author Cameron Bass
 */
public static class Bytes {
    [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
    public static extern int StrFormatByteSize(long fileSize, System.Text.StringBuilder pwszBuff, int cchBuff);

    /**
     * Formats the input size to a string with the appropriate byte size.
     * 
     * @param  size  Size of input data in bytes
     * @return Formatted string
     */
    public static string Format(long size) {
        System.Text.StringBuilder s = new System.Text.StringBuilder("", 8);
        StrFormatByteSize(size, s, 8);
        return s.ToString();
    }
}