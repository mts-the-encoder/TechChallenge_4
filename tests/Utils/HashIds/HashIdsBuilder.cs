namespace Utils.HashIds;

public class HashIdsBuilder
{
    private static HashIdsBuilder _instance;
    private readonly HashidsNet.Hashids _encryptor;

    private HashIdsBuilder()
    {
        _encryptor ??= new HashidsNet.Hashids("qj328TtMDy", 3);
    }

    public static HashIdsBuilder Instance()
    {
        _instance = new HashIdsBuilder();
        return _instance;
    }

    public HashidsNet.Hashids Build()
    {
        return _encryptor;
    }
}