using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aegis_technical_tes.Model;

namespace aegis_technical_tes.Repositories.Contracts
{
    public interface ISiswaRepository
    {
        List<Siswa> GetAll();
    }
}