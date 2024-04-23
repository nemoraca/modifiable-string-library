namespace ModifiableStringLibrary
{
    public static class StringUtility
    {
        struct StringWrapper
        {
            public string s;
            public int i;
        }

        // This function returns the address of the first byte of an arbitrary string object.
        public unsafe static char* GetAddress(this string str)
        {
            StringWrapper stringWrapper;
            stringWrapper.s = str;

            int d = sizeof(char*) / sizeof(int);    // For 32-bit OS d = 1, for 64-bit OS d = 2
            char** p = (char**)(&stringWrapper.i - d);
            char* q = *p;       // char* q = &str[-6];
            return q;
        }

        public static void Modify(this string str, int pos, char c)
        {
            if (pos < 0 || pos >= str.Length) return;
            unsafe
            {
                char* q = str.GetAddress();
                q[pos + 6] = c;
            }
        }
    }

    public struct ModifiableString
    {
        private string s;

        public static implicit operator ModifiableString(string str)
        {
            ModifiableString ms;
            ms.s = str;
            return ms;
        }

        public static implicit operator string(ModifiableString ms)
        {
            return ms.s;
        }

        public readonly char this[int index]
        {
            get { return s[index]; }
            set { s.Modify(index, value); }
        }
    }
}
