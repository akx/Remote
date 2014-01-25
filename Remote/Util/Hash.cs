namespace Remote.Util
{
    public static class Hash
    {
        public static long Djb2(string input)
        {
            var hashAddress = 5381L;
            for (var i = 0; i < input.Length; i++){
                hashAddress = ((hashAddress << 5) + hashAddress) + input[i];
            }
            return hashAddress;
        }
    }
}
