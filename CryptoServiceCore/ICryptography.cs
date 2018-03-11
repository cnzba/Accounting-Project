using System.Threading.Tasks;

namespace CryptoService
{
    public interface ICryptography
    {
        string HashMD5(string input);
        string GenerateTempPassword(int length);
    }
}