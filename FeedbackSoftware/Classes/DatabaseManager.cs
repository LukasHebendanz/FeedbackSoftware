﻿using FeedbackSoftware.Classes.Dtos;
using MySql.Data.MySqlClient;
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
    public class DatabaseManager
    {
        private static readonly string connectionstring = "Server=10.0.126.31;Port=3306;Database=Feedback;User ID=ExtUser;Password=!DevUser.69;Pooling=true;";

        #region User
        private const string SQL_INSERT_USER = "INSERT INTO `User` (Passwort, Benutzername, Rolle) VALUES (@Passwort, @Benutzername, @Rolle)";
        private const string SQL_SELECT_USER_BY_USERNAME = "SELECT ID, Benutzername, Rolle FROM User WHERE Benutzername = @Benutzername";
        private const string SQL_SELECT_ALL_USERS = "SELECT ID, Benutzername, Rolle FROM User";
		private const string SQL_SELECT_USER_BY_PASSWORT_AND_USERNAME = "SELECT Passwort, Benutzername, Rolle FROM User WHERE Passwort = @Passwort AND Benutzername = @Benutzername";

		public static MySqlConnection GetConnection()
		{
			return new MySqlConnection(connectionstring);
		}

		#region InsertUser
		public void InsertUser(UserDto userdto)
		{
			using (MySqlConnection con = GetConnection())
			{
				con.Open();

				using (MySqlCommand cmd = new MySqlCommand(SQL_INSERT_USER, con))
				{
					MySqlParameter[] parameters = GetUserParameter(userdto);
					SetUserParameter(parameters, cmd);
					cmd.ExecuteNonQuery();
				}
			}
		}

		private MySqlParameter[] GetUserParameter(UserDto userdto)
		{
			MySqlParameter[] param = new MySqlParameter[]
			{
				new MySqlParameter("@Passwort", MySqlDbType.VarChar) { Value = userdto.Passwort },
				new MySqlParameter("@Benutzername", MySqlDbType.VarChar) { Value = userdto.Name },
				new MySqlParameter("@Rolle", MySqlDbType.VarChar) { Value = userdto.Rolle }
			};

			return param;
		}

		public void SetUserParameter(MySqlParameter[] parameter, MySqlCommand cmd)
		{
			cmd.Parameters.Add(parameter[0]);
			cmd.Parameters.Add(parameter[1]);
			cmd.Parameters.Add(parameter[2]);
		}
		#endregion

		#region SelectUserInfoByUsername
		public UserDto SelectUserInfoByUsername(string username)
		{
			UserDto user = new UserDto();

			using (MySqlConnection con = GetConnection())
			{
				con.Open();

                using (MySqlCommand cmd = new MySqlCommand(SQL_SELECT_USER_BY_USERNAME, con))
                {
                    MySqlParameter param = GetBenutzernameParameter(username);
                    SetBenutzernameParameter(param, cmd);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.UserID = reader.GetInt32(0);
                            user.Name = reader.GetString(1);
                            user.Rolle = reader.GetString(2);
                        }
                    }
                }
            }

            return user;
        }

        private MySqlParameter GetBenutzernameParameter(string username)
        {
            return new MySqlParameter("@Benutzername", MySqlDbType.VarChar) { Value = username };
        }

        private void SetBenutzernameParameter(MySqlParameter param, MySqlCommand cmd)
        {
            cmd.Parameters.Add(param);
        }
        #endregion

        #region GetAllUsers
        public List<UserDto> SelectAllUsers()
        {
            List<UserDto> users = new List<UserDto>();

            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand(SQL_SELECT_ALL_USERS, con))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserDto user = new UserDto();
                            user.UserID = reader.GetInt32(0);
                            user.Name = reader.GetString(1);
                            user.Rolle = reader.GetString(2);

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }
		#endregion

		#region SelectUserByPasswortAndUsername
		public UserDto SelectUserByPasswortAndUsername(string passwort, string username)
		{
			UserDto user = new UserDto();

			using (MySqlConnection con = GetConnection())
			{
				con.Open();

				using (MySqlCommand cmd = new MySqlCommand(SQL_SELECT_USER_BY_PASSWORT_AND_USERNAME, con))
				{
					MySqlParameter[] param = GetPasswortAndBenutzernameParameter(passwort, username);
					SetLoginParameter(param, cmd);

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							user.Passwort = reader.GetString(0);
							user.Name = reader.GetString(1);
							user.Rolle = reader.GetString(2);
						}
					}
				}
			}

			return user;
		}

		private MySqlParameter[] GetPasswortAndBenutzernameParameter(string passwort, string username)
		{
			MySqlParameter[] param = new MySqlParameter[]
			{
				new MySqlParameter("@Passwort", MySqlDbType.VarChar) {Value = passwort },
				new MySqlParameter("@Benutzername", MySqlDbType.VarChar) {Value = username},
			};
			return param;
		}

		private void SetLoginParameter(MySqlParameter[] param, MySqlCommand cmd)
		{
			cmd.Parameters.Add(param[0]);
			cmd.Parameters.Add(param[1]);
		}
		#endregion
		#endregion

		#region FeedbackVorgang
		private const string SQL_SELECT_KEY = "SELECT Schluessel FROM FeedbackVorgang WHERE Schluessel = @Schluessel";
		private const string SQL_INSERT_FEEDBACK = "INSERT INTO `FeedbackVorgang` (KlasseId, Name, FormularArt) VALUES (@KlasseId ,@Name, @FormularArt)";

		public void InsertFeedback(FeedbackDto feedbackDto)
		{
			using (MySqlConnection con = GetConnection())
			{
				con.Open();

				using (MySqlCommand cmd = new MySqlCommand(SQL_INSERT_FEEDBACK, con))
				{
					MySqlParameter[] parameters = GetFormularParameter(feedbackDto);
					SetFeedbackParameter(parameters, cmd);
					cmd.ExecuteNonQuery();
				}
			}
		}

		private MySqlParameter[] GetFormularParameter(FeedbackDto feedbackDto)
		{
			MySqlParameter[] param = new MySqlParameter[]
			{
				new MySqlParameter("@Schluessel", MySqlDbType.VarChar) { Value = feedbackDto.Schluessel },
				new MySqlParameter("@KlasseId", MySqlDbType.Int32) { Value = feedbackDto.KlasseId },
				new MySqlParameter("@Titel", MySqlDbType.VarChar) { Value = feedbackDto.Name },
				new MySqlParameter("@FormularArt", MySqlDbType.VarChar) { Value = feedbackDto.FormularArt }
			};

			return param;
		}

		public void SetFeedbackParameter(MySqlParameter[] parameter, MySqlCommand cmd)
		{
			cmd.Parameters.Add(parameter[0]);
			cmd.Parameters.Add(parameter[1]);
			cmd.Parameters.Add(parameter[2]);
			cmd.Parameters.Add(parameter[3]);
		}

		#region SelectKey
		public UserDto SelectKey()
		{
			UserDto user = new UserDto();

			using (MySqlConnection con = GetConnection())
			{
				con.Open();

				using (MySqlCommand cmd = new MySqlCommand(SQL_SELECT_KEY, con))
				{
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						reader.Read();
					}
				}
			}

			return user;
		}
		#endregion

		#endregion
	}
}
