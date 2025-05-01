using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aegis_technical_tes.Model;
using aegis_technical_tes.Repositories.Contracts;
using System.Data.SqlClient;
using System.Data;

namespace aegis_technical_tes.Repositories
{
    public class SiswaRepository : ISiswaRepository
    {
        private readonly string _connectionString;

        public SiswaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<Siswa> GetAll()
        {
            var result = new List<Siswa>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

            using (var command = new SqlCommand("GetSiswa", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Siswa
                        {
                            Id = reader.GetInt32(0),
                            Nama = reader.GetString(1),
                            Umur = reader.GetInt32(2),
                            Alamat = reader.GetString(3)
                        });
                    }
                }
            }
            }
            return result;
        }
    }
}