using FeedbackSoftware.Classes.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackSoftware.Classes
{
	public class Database
	{
		private const string connectionstring = "Data Source = 10.0.126.31; Initial Catalog = RoleAuthentication; Uid=ExtUser; Pwd=!DevUser.69;";

		private const string SQL_UPDATE = "Update FeedbackSoftware Set Data=@data, Formular_Schlüssel=@formular_schlüssel";
		private const string SQL_INSERT = "Insert Into TeacherAccount Values (@Passwort, @Benutzername, @Rolle)";

		public Database() { }

		public void InsertUser(UserDto userdto)
		{
			using (SqlConnection con = new SqlConnection(connectionstring))
			{
				SqlCommand cmd = new SqlCommand(SQL_INSERT, con);
				SqlParameter[] parameters = GetUserParameter(userdto);
				SetUserParameter(parameters, cmd);
				con.Open();
				cmd.ExecuteNonQuery();
			}
		}

		private SqlParameter[] GetUserParameter(UserDto userdto) 
		{
			SqlParameter[] param = new SqlParameter[]
			{
				new SqlParameter(userdto.Name, SqlDbType.VarChar),
				new SqlParameter(userdto.Passwort, SqlDbType.VarChar),
				new SqlParameter(userdto.Rolle, SqlDbType.VarChar),
			};

			return param;
		}

		public void SetUserParameter(SqlParameter[] parameter, SqlCommand cmd)
		{
			cmd.Parameters.AddWithValue("@Passwort", parameter[0]);
			cmd.Parameters.AddWithValue("@Benutzername", parameter[1]);
			cmd.Parameters.AddWithValue("@Rolle", parameter[2]);
		}
	}
}
