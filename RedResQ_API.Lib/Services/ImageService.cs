using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Buffers.Text;
using System.Security.Claims;

namespace RedResQ_API.Lib.Services
{
    public static class ImageService
    {
        public static Image Get(JwtClaims claims, long id)
        {
            if(PermissionService.IsPermitted("getImage", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Im_GetImage";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                DataTable imageTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (imageTable.Rows.Count == 1)
                {
                    return Image.ConvertToImage(imageTable.Rows[0]);
                }
            }

            throw new NotFoundException();
        }

        public static long Add(JwtClaims claims, string desc, byte[] bytes)
        {
            if(PermissionService.IsPermitted("addImage", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Im_AddImage";

                parameters.Add(new SqlParameter { ParameterName = "@base64", SqlDbType = SqlDbType.VarChar, Value = base64 });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                return Search(claims, base64);
            }

            throw new NotFoundException();
        }

        public static bool Delete(JwtClaims claims, long id)
        {
            if(PermissionService.IsPermitted("deleteImage", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Im_DeleteImage";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if (rowsAffected == 1)
                {
                    return true;
                }
            }

            return false;
        }

        public static long Search(JwtClaims claims, string desc)
        {
            if (PermissionService.IsPermitted("searchImage", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Im_SearchImage";

                parameters.Add(new SqlParameter { ParameterName = "@base64", SqlDbType = SqlDbType.VarChar, Value = base64 });

                DataTable imageTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if(imageTable.Rows.Count > 0)
                {
                    return Image.ConvertToImage(imageTable.Rows[0]).Id;
                }
            }

            throw new NotFoundException();
        }
    }
}
