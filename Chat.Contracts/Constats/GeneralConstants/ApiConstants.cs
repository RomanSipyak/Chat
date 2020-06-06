using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Constats.GeneralConstants
{
    public static class ApiConstants
    {
        #region SwaggerConstants
        public const string Configuration_SwaggerEndpoint = "Swagger:Endpoint";
        public const string Configuration_SwaggerDoc = "Swagger:Doc";
        public const string Configuration_SwaggerApiTitle = "ChatApi";
        public const string Configuration_SwaggerApiVersion = "v1";
        public const string Configuration_SwaggerApiSecuritySchemeDescription = "JWT Authorization header using the Bearer scheme. " +
                                                                   "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
                                                                   "\r\n\r\nExample: \"Bearer 12345abcdef\"";
        public const string Configuration_SwaggerApiSecuritySchemeName = "Authorization";
        public const string Configuration_SwaggerApiSecurityScheme = "oath2";
        public const string Configuration_SwaggerBearer = "Bearer";
        #endregion SwaggerConstants

        #region DbConstants
        public const string Configuration_DbConnection = "AppDbConnection";
        #endregion DbConstants

        public const string PolicyForCors = "Policy";
        public const string Token = "Token";
    }
}
