using FeedbackSoftware.Classes.Dtos;
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
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionstring);
        }

        private static readonly string connectionstring = "Server=10.0.126.31;Port=3306;Database=Feedback;User ID=ExtUser;Password=!DevUser.69;Pooling=true;";

        #region User

        private const string SQL_INSERT_USER = "INSERT INTO `User` (Passwort, Benutzername, Rolle) VALUES (@Passwort, @Benutzername, @Rolle)";
        private const string SQL_SELECT_USER_BY_USERNAME = "SELECT ID, Benutzername, Rolle FROM User WHERE Benutzername = @Benutzername";
        private const string SQL_SELECT_ALL_USERS = "SELECT ID, Benutzername, Rolle FROM User";
        private const string SQL_SELECT_USER_BY_PASSWORT_AND_USERNAME = "SELECT Passwort, Benutzername, Rolle FROM User WHERE Passwort = @Passwort AND Benutzername = @Benutzername";
        
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

        #region UpdateUser
        public void UpdateUser(UserDto user)
        {
            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                // SQL-Anweisung zum Aktualisieren eines Nutzers
                string sql = "UPDATE User SET Passwort = @Passwort, Benutzername = @Benutzername, Rolle = @Rolle WHERE Id = @Id";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    // Parameter setzen, um SQL-Injection zu vermeiden
                    cmd.Parameters.AddWithValue("@Passwort", user.Passwort);
                    cmd.Parameters.AddWithValue("@Benutzername", user.Name);
                    cmd.Parameters.AddWithValue("@Rolle", user.Rolle);
                    cmd.Parameters.AddWithValue("@Id", user.UserID);

                    // Ausführen der SQL-Anweisung
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region DeleteUser
        public void DeleteUser(int userId)
        {
            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                // SQL-Anweisung zum Löschen eines Nutzers
                string sql = "DELETE FROM User WHERE ID = @ID";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    // Parameter setzen, um SQL-Injection zu vermeiden
                    cmd.Parameters.AddWithValue("@ID", userId);

                    // Ausführen der SQL-Anweisung
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region Formular
        private const string SQL_INSERT_FORMULAR = "INSERT INTO Formular (Schluessel, Data, Name) VALUES (@Schluessel, @Data, @Name)";
		private const string SQL_SELECT_ALL_FORMULARS_BY_KEY = "SELECT FormularID, Schluessel, Data, Name FROM Formular WHERE Schluessel = @Schluessel";

		#region InsertFormular
		public void InsertFormular(FormularDto formularDto)
		{
            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand(SQL_INSERT_FORMULAR, con))
                {
                    MySqlParameter[] parameters = GetFormularParameter(formularDto);
                    SetFormularParameter(parameters, cmd);
                    cmd.ExecuteNonQuery();
                }
            }
        }

		private MySqlParameter[] GetFormularParameter(FormularDto formularDto)
		{
			MySqlParameter[] param = new MySqlParameter[]
			{
                new MySqlParameter("@Schluessel", MySqlDbType.Int32) { Value = formularDto.Schluessel },
				new MySqlParameter("@Data", MySqlDbType.VarChar) { Value = formularDto.Data },
				new MySqlParameter("@Name", MySqlDbType.VarChar) { Value = formularDto.Name }
            };

			return param;
        }

		private void SetFormularParameter(MySqlParameter[] parameter, MySqlCommand cmd)
		{
            cmd.Parameters.Add(parameter[0]);
            cmd.Parameters.Add(parameter[1]);
            cmd.Parameters.Add(parameter[2]);
        }
		#endregion

		#region SelectAllByKey

		public List<FormularDto> SelectAllFormularsByKey(int key)
		{
			List<FormularDto> formulars = new List<FormularDto>();

            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand(SQL_SELECT_ALL_FORMULARS_BY_KEY, con))
                {
                    MySqlParameter parameter = GetSchluesselParameter(key);
                    SetSchluesselParameter(parameter, cmd);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormularDto formulardto = new FormularDto()
                            {
                                FormularId = reader.GetInt32(0),
                                Schluessel = reader.GetInt32(1),
                                Data = reader.GetString(2),
								Name = reader.GetString(3)
                            };

							formulars.Add(formulardto);
                        }
                    }
                }
            }

			return formulars;
        }

		private MySqlParameter GetSchluesselParameter(int key)
		{
			return new MySqlParameter("@Schluessel", MySqlDbType.Int32) { Value = key };
		}

		private void SetSchluesselParameter(MySqlParameter parameter, MySqlCommand cmd)
		{
			cmd.Parameters.Add(parameter);
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
        private const string SQL_INSERT_FEEDBACK = "INSERT INTO FeedbackVorgang (KlasseID, VorgangName, FeedbackArt) VALUES (@KlasseID ,@VorgangName, @FeedbackArt)";
		private const string SQL_SELECT_KEY = "SELECT Schluessel FROM FeedbackVorgang WHERE Schluessel = @Schluessel";

		#region InsertFeedback
		public void InsertFeedback(FeedbackDto feedbackDto)
		{
			using (MySqlConnection con = GetConnection())
			{
				con.Open();

                using (MySqlCommand cmd = new MySqlCommand(SQL_INSERT_FEEDBACK, con))
                {
                    MySqlParameter[] parameters = GetFeedbackParameter(feedbackDto);
                    SetFeedbackParameter(parameters, cmd);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private MySqlParameter[] GetFeedbackParameter(FeedbackDto feedbackDto)
        {
            MySqlParameter[] param = new MySqlParameter[]
            {
                new MySqlParameter("@KlasseID", MySqlDbType.Int32) { Value = feedbackDto.KlasseId },
                new MySqlParameter("@VorgangName", MySqlDbType.VarChar) { Value = feedbackDto.Name },
                new MySqlParameter("@FeedbackArt", MySqlDbType.VarChar) { Value = feedbackDto.FeedbackArt }
            };

            return param;
        }

		public void SetFeedbackParameter(MySqlParameter[] parameter, MySqlCommand cmd)
		{
			cmd.Parameters.Add(parameter[0]);
			cmd.Parameters.Add(parameter[1]);
			cmd.Parameters.Add(parameter[2]);
		}
		#endregion        

        #region ReadFeedback
        public List<string> GetVorgangName()
        {
            List<string> vorgangName = new List<string>();

            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                string sql = "SELECT DISTINCT VorgangName FROM FeedbackVorgang"; // Anpassen an deine Datenbankstruktur

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vorgangName.Add(reader["VorgangName"].ToString());
                        }
                    }
                }
            }

            return vorgangName;
        }

        public List<string> GetKlassenIds()
        {
            List<string> klassenIds = new List<string>();

            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                string sql = "SELECT DISTINCT KlasseId FROM FeedbackVorgang"; // Anpassen an deine Datenbankstruktur

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            klassenIds.Add(reader["KlasseId"].ToString());
                        }
                    }
                }
            }

            return klassenIds;
        }

        public List<string> GetFeedbackArt()
        {
            List<string> feedbackArt = new List<string>();

            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                string sql = "SELECT DISTINCT FeedbackArt FROM FeedbackVorgang"; // Anpassen an deine Datenbankstruktur

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            feedbackArt.Add(reader["FeedbackArt"].ToString());
                        }
                    }
                }
            }
            return feedbackArt;
        }
		#endregion

		#region SelectKey
		public FeedbackDto SelectKey(string schluessel)
		{
			FeedbackDto fbDto = new FeedbackDto();

			using (MySqlConnection con = GetConnection())
			{
				con.Open();

				using (MySqlCommand cmd = new MySqlCommand(SQL_SELECT_KEY, con))
				{
					MySqlParameter[] param = GetKeyParameter(schluessel);
					SetKeyParameter(param, cmd);

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							fbDto.Schluessel = reader.GetInt32(0);
						}
					}
				}
			}

			return fbDto;
		}
		private MySqlParameter[] GetKeyParameter(string schluessel)
		{
			MySqlParameter[] param = new MySqlParameter[]
			{
				new MySqlParameter("@Schluessel", MySqlDbType.VarChar) { Value = schluessel},
			};
			return param;
		}

		private void SetKeyParameter(MySqlParameter[] param, MySqlCommand cmd)
		{
			cmd.Parameters.Add(param[0]);
		}
        #endregion
        #endregion

        #region Klasse

        private const string SQL_SELECT_KLASSENID_BY_NAME = "SELECT KlasseID FROM Klasse WHERE Name = @Name";
        public void CreateKlasse(KlasseDto newClass)
        {
            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                // SQL-Anweisung zum Einfügen eines neuen Nutzers
                string sql = "INSERT INTO Klasse (Name, Jahrgangsstufe, Schuljahr, Abteilung, Fach) VALUES (@Name, @Jahrgangsstufe, @Schuljahr, @Abteilung, @Fach)";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    // Parameter setzen, um SQL-Injection zu vermeiden
                    cmd.Parameters.AddWithValue("@Name", newClass.Name);
                    cmd.Parameters.AddWithValue("@Jahrgangsstufe", newClass.Jahrgangsstufe);
                    cmd.Parameters.AddWithValue("@Schuljahr", newClass.Schuljahr);
                    cmd.Parameters.AddWithValue("@Abteilung", newClass.Abteilung);
                    cmd.Parameters.AddWithValue("@Fach", newClass.Fach);

                    // Ausführen der SQL-Anweisung
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateKlasse(KlasseDto klasse)
        {
            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                // SQL-Anweisung zum Aktualisieren eines Nutzers
                string sql = "UPDATE Klasse SET Name = @Name, Jahrgangsstufe = @Jahrgangsstufe, Schuljahr = @Schuljahr, Abteilung = @Abteilung, Fach = @Fach WHERE ID = @ID";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    // Parameter setzen, um SQL-Injection zu vermeiden
                    cmd.Parameters.AddWithValue("@Name", klasse.Name);
                    cmd.Parameters.AddWithValue("@Jahrgangsstufe", klasse.Jahrgangsstufe);
                    cmd.Parameters.AddWithValue("@Schuljahr", klasse.Schuljahr);
                    cmd.Parameters.AddWithValue("@Abteilung", klasse.Abteilung);
                    cmd.Parameters.AddWithValue("@Fach", klasse.Fach);

                    // Ausführen der SQL-Anweisung
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteKlasse(int klasseId)
        {
            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                // SQL-Anweisung zum Löschen eines Nutzers
                string sql = "DELETE FROM Klasse WHERE ID = @ID";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    // Parameter setzen, um SQL-Injection zu vermeiden
                    cmd.Parameters.AddWithValue("@ID", klasseId);

                    // Ausführen der SQL-Anweisung
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<KlasseDto> GetKlassenNames()
        {
            List<KlasseDto> klassenNames = new List<KlasseDto>();

            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                string sql = "SELECT KlasseID, Name, Jahrgangsstufe, Schuljahr, Abteilung, Fach FROM Klasse"; // Anpassen an deine Datenbankstruktur

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KlasseDto dto = new KlasseDto();
                            dto.KlasseId = (int)reader["KlasseID"]; ;
                            dto.Name = reader["Name"].ToString();
                            dto.Abteilung = reader["Abteilung"].ToString();
                            dto.Schuljahr = reader["Schuljahr"].ToString();
                            dto.Jahrgangsstufe = reader["Jahrgangsstufe"].ToString();
                            dto.Fach = reader["Fach"].ToString();
                            klassenNames.Add(dto);
                        }
                    }
                }
            }

            return klassenNames;
        }

        public int GetKlassenIdByName(string name)
        {
            int klassenId = 0;

            using (MySqlConnection con = GetConnection())
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand(SQL_SELECT_KLASSENID_BY_NAME, con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            klassenId = (int)reader["KlasseID"];
                        }
                    }
                }
            }

            return klassenId;
        }

        #endregion
    }
}