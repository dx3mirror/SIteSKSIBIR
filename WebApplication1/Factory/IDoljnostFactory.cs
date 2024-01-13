using WebApplication2;

namespace WebApplication1.Factory
{
    public interface IDoljnostFactory
    {
        Doljnost CreateDoljnostForUpdate(int id, string naimenoviyDoljnosti, string skogo, string poKokoe, int kolVo, string otvetstveniy);

        Doljnost CreateDoljnostForAdd(int? sotrudnikId, string naimenoviyDoljnosti, string skogo, string poKokoe, int kolVo, string otvetstveniy);
    }
}
