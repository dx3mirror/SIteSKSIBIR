using WebApplication2;

namespace WebApplication1.Factory
{
    public class DoljnostFactory : IDoljnostFactory
    {
        public Doljnost CreateDoljnostForUpdate(int id, string naimenoviyDoljnosti, string skogo, string poKokoe, int kolVo, string otvetstveniy)
        {
            return new Doljnost
            {
                Id = id,
                NaimenoviyDoljnosti = naimenoviyDoljnosti,
                SKogo = skogo,
                PoKokoe = poKokoe,
                KolVo = kolVo,
                Otvetstveniy = otvetstveniy
            };
        }

        public Doljnost CreateDoljnostForAdd(int? sotrudnikId, string naimenoviyDoljnosti, string skogo, string poKokoe, int kolVo, string otvetstveniy)
        {
            return new Doljnost
            {
                FkSotrudnik = sotrudnikId,
                NaimenoviyDoljnosti = naimenoviyDoljnosti,
                SKogo = skogo,
                PoKokoe = poKokoe,
                KolVo = kolVo,
                Otvetstveniy = otvetstveniy
            };
        }
    }
}
