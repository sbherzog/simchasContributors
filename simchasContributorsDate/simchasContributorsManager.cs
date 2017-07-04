using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simchasContributorsData
{
    public class simchasContributorsManager
    {

        private string _connectonString;
        public simchasContributorsManager(string connectionsString)
        {
            _connectonString = connectionsString;
        }

        public IEnumerable<simchaClass> GetAllSimchas()
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT S.*, sum(C.amount) AS simchaTotalMoney, COUNT (C.personID) AS simchaContributionsCount FROM simchas S
                                LEFT JOIN Contributors C ON S.id = C.simchID
                                Group By S.id, s.simchaName, s.simchaDate";
            connection.Open();
            SqlDataReader reder = command.ExecuteReader();
            List<simchaClass> result = new List<simchaClass>();
            while (reder.Read())
            {
                simchaClass simcha = new simchaClass();
                simcha.simchaId = (int)reder["id"];
                simcha.simchaName = (string)reder["simchaName"];
                simcha.simchaDate = (DateTime)reder["simchaDate"];
                if (reder["simchaContributionsCount"] != DBNull.Value)
                {
                    simcha.simchaContributionsCount = (int)reder["simchaContributionsCount"];
                }

                if (reder["simchaTotalMoney"] != DBNull.Value)
                {
                    simcha.simchaTotalMoney = (decimal)reder["simchaTotalMoney"];
                }

                result.Add(simcha);
            }
            return result;
        }

        public IEnumerable<PeopleClass> GetAllContributors()
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT P.*, SUM(D.depositAmount) AS depositAmount FROM People P 
                    LEFT JOIN Deposit D ON P.id = D.personId 
                    GROUP BY P.id, P.firstName, p.lastName, p.cellPhone, p.email, p.alwaysInclude";
            connection.Open();
            SqlDataReader reder = command.ExecuteReader();
            List<PeopleClass> result = new List<PeopleClass>();
            while (reder.Read())
            {
                PeopleClass person = new PeopleClass();
                person.personId = (int)reder["id"];
                person.firstName = (string)reder["firstName"];
                person.lastName = (string)reder["lastName"];
                person.email = (string)reder["email"];
                person.cellPhone = (string)reder["cellPhone"];
                if (reder["alwaysInclude"] != DBNull.Value)
                {
                    person.alwaysInclude = (bool)reder["alwaysInclude"];
                }

                SqlConnection connection4 = new SqlConnection(_connectonString);
                SqlCommand command4 = connection4.CreateCommand();
                command4.CommandText = @"SELECT SUM(amount)FROM Contributors WHERE personID = " + person.personId;
                connection4.Open();
                decimal sum = 0;
                object value = command4.ExecuteScalar();
                if (value != DBNull.Value)
                {
                    sum = (decimal)value;
                }

                if (reder["depositAmount"] != DBNull.Value)
                {
                    person.DepositAmount = (decimal)reder["depositAmount"] - sum;
                }
                else
                {
                    person.DepositAmount = 0 - sum;
                }
                result.Add(person);
                connection4.Dispose();
            }
            return result;
        }

        public IEnumerable<DepositClass> GetHistory(int id)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT 'Deposit' AS Action, depositDate AS Date, depositAmount AS Amount  FROM Deposit WHERE personId = @id
                UNION SELECT S.simchaName AS Action, S.simchaDate AS Date, C.amount AS Amount FROm Contributors C LEFT JOIN simchas S ON C.simchID = s.id WHERE C.personID = @id
                ORDER BY Date";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reder = command.ExecuteReader();
            List<DepositClass> result = new List<DepositClass>();
            while (reder.Read())
            {
                DepositClass history = new DepositClass();
                history.name = (string)reder["Action"];
                history.DepositAmount = (decimal)reder["Amount"];
                history.DepositDate = (DateTime)reder["Date"];
                result.Add(history);
            }
            return result;
        }

        public int GetAmountPeoplre()
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(id) AS sum FROM People";
            connection.Open();
            int id = 0;
            if (command.ExecuteScalar() != DBNull.Value)
            {
                id = (int)command.ExecuteScalar();
            }
            return id;
        }


        public void AddNewSimcha(simchaClass s)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO simchas (simchaName, simchaDate) VALUES (@simchaName, @simchaDate)";
            command.Parameters.AddWithValue("@simchaName", s.simchaName);
            command.Parameters.AddWithValue("@simchaDate", s.simchaDate);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public int addNewContributors(PeopleClass p)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO People (FirstName, LastName, cellPhone, email, alwaysInclude) 
                    VALUES (@FirstName, @LastName, @cellPhone, @email, @alwaysInclude) SELECT @@Identity";
            command.Parameters.AddWithValue("@FirstName", p.firstName);
            command.Parameters.AddWithValue("@LastName", p.lastName);
            command.Parameters.AddWithValue("@cellPhone", p.cellPhone);
            command.Parameters.AddWithValue("@email", p.email);
            command.Parameters.AddWithValue("@alwaysInclude", p.alwaysInclude);
            connection.Open();
            int id = (int)(decimal)command.ExecuteScalar();
            return id;
        }

        public void UpdateContributors(PeopleClass p)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"UPDATE People SET FirstName = @FirstName, LastName = @LastName, cellPhone = @cellPhone, email = @email, alwaysInclude = @alwaysInclude WHERE id = @id";
            command.Parameters.AddWithValue("@FirstName", p.firstName);
            command.Parameters.AddWithValue("@LastName", p.lastName);
            command.Parameters.AddWithValue("@cellPhone", p.cellPhone);
            command.Parameters.AddWithValue("@email", p.email);
            command.Parameters.AddWithValue("@alwaysInclude", p.alwaysInclude);
            command.Parameters.AddWithValue("@id", p.personId);
            connection.Open();
            command.ExecuteScalar();
        }

        public void addNewDeposit(DepositClass d)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Deposit (depositDate, depositAmount, personId) VALUES (@depositDate, @depositAmount, @personId)";
            command.Parameters.AddWithValue("@depositDate", d.DepositDate);
            command.Parameters.AddWithValue("@depositAmount", d.DepositAmount);
            command.Parameters.AddWithValue("@personId", d.DepositpersonId);
            connection.Open();
            command.ExecuteScalar();
        }


        public IEnumerable<ContributorsSimchaClass> GetContributorsSimcha(int simchaId)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT p.id, P.firstName, P.lastName, P.alwaysInclude, C.amount,
              CASE WHEN EXISTS (SELECT* FROM Contributors C WHERE C.personID = P.id AND C.simchID = @simchaId)
                   THEN 'TRUE' 
                   ELSE 'FALSE'
              END AS ContributorBool
            FROM People P
            LEFT JOIN Contributors C ON C.personID = P.id and C.simchID = @simchaId";
            command.Parameters.AddWithValue("@simchaId", simchaId);
            connection.Open();
            SqlDataReader reder = command.ExecuteReader();
            List<ContributorsSimchaClass> result = new List<ContributorsSimchaClass>();
            while (reder.Read())
            {
                ContributorsSimchaClass CS = new ContributorsSimchaClass();
                CS.personId = (int)reder["id"];
                CS.firstName = (string)reder["firstName"];
                CS.lastName = (string)reder["lastName"];
                CS.alwaysInclude = (bool)reder["alwaysInclude"];
                CS.ContributeBool = (string)reder["ContributorBool"];

                SqlConnection connection2 = new SqlConnection(_connectonString);
                SqlCommand command2 = connection2.CreateCommand();
                command2.CommandText = @"SELECT SUM(amount)FROM Contributors WHERE personID = " + CS.personId;
                connection2.Open();
                decimal ContributorSum = 0;

                object value = command2.ExecuteScalar();
                if (value != DBNull.Value)
                {
                    ContributorSum = (decimal)value;
                }


                SqlConnection connection3 = new SqlConnection(_connectonString);
                SqlCommand command3 = connection3.CreateCommand();
                command3.CommandText = @"SELECT SUM(depositAmount)AS depositAmount FROM Deposit WHERE personID = " + CS.personId;
                connection3.Open();
                decimal DepositSum = 0;
                object value2 = command2.ExecuteScalar();
                if (value2 != DBNull.Value)
                {
                    DepositSum = (decimal)value2;
                }

                CS.DepositAmount = DepositSum - ContributorSum;

                if (reder["amount"] != DBNull.Value)
                {
                    CS.amount = (decimal)reder["amount"];
                }
                result.Add(CS);
                connection2.Dispose();
                connection3.Dispose();
            }
            return result;
        }

        public string GetSimchaName(int simchaId)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT simchaName FROM simchas WHERE id = @id;";
            command.Parameters.AddWithValue("@id", simchaId);
            connection.Open();
            string simchaName = (string)command.ExecuteScalar();
            return simchaName;
        }

        public decimal? GetSimchaAmount(int simchaId)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT sum(amount) AS simchaTotalMoney FROM Contributors WHERE simchID = @simchaId;";
            command.Parameters.AddWithValue("@simchaId", simchaId);
            connection.Open();
            decimal? simchaAmont = 0;
            if (command.ExecuteScalar() != DBNull.Value)
            {
                simchaAmont = (decimal?)command.ExecuteScalar();
                return simchaAmont;
            }
            return simchaAmont;
        }


        public PeopleClass GetPersonByID(int personId)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM People WHERE id = @personId;";
            command.Parameters.AddWithValue("@personId", personId);
            connection.Open();
            SqlDataReader reder = command.ExecuteReader();
            reder.Read();
            PeopleClass p = new PeopleClass();
            p.firstName = (string)reder["firstName"];
            p.lastName = (string)reder["lastName"];
            return p;
        }

        public void DeletContributions(int simchID)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM Contributors WHERE simchID = @id";
            command.Parameters.AddWithValue("@id", simchID);
            connection.Open();
            command.ExecuteScalar();
        }

        public void Contributions(IEnumerable<ContributorsSimchaClass> c)
        {
            foreach (ContributorsSimchaClass i in c)
            {
                if (i.Contribute)
                {
                    SqlConnection connection = new SqlConnection(_connectonString);
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = @"INSERT INTO Contributors (personID, simchID, amount) VALUES (@personID, @simchID, @amount)";
                    command.Parameters.AddWithValue("@personID", i.personId);
                    command.Parameters.AddWithValue("@simchID", i.simchaID);
                    command.Parameters.AddWithValue("@amount", i.amount);
                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }
        public decimal Total()
        {
            decimal total = 0;
            SqlConnection connection5 = new SqlConnection(_connectonString);
            SqlCommand command5 = connection5.CreateCommand();
            command5.CommandText = @"SELECT SUM(amount)FROM Contributors";
            connection5.Open();
            decimal ContributorSum = 0;
            if (command5.ExecuteScalar() != DBNull.Value)
            {
                ContributorSum = (decimal)command5.ExecuteScalar();
            }

            SqlConnection connection6 = new SqlConnection(_connectonString);
            SqlCommand command6 = connection6.CreateCommand();
            command6.CommandText = @"SELECT SUM(depositAmount)AS depositAmount FROM Deposit";
            connection6.Open();
            decimal DepositSum = 0;
            if (command6.ExecuteScalar() != DBNull.Value)
            {
                DepositSum = (decimal)command6.ExecuteScalar();
            }
            total = DepositSum - ContributorSum;
            return total;
        }
    }
}
