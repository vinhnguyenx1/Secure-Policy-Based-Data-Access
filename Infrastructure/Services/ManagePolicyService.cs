public class ManagePolicyService
    {
        private string _connectionString;
        public ManagePolicyService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SavePolicyAsync(Poort8PolicyModel model, string policyId)
        {
            try
            {
                using var db = new MySqlConnection(_connectionString);
                await db.OpenAsync();
                string query = $"INSERT INTO Table Values ()";

                var dp = new DynamicParameters();
                dp.Add("@policyId", policyId);
                dp.Add("@fromDate", DateTime.UtcNow);
                dp.Add("@toDate", model.ToDate);

                int res = db.Execute(query, dp);
                if (res > 0)
                {
                    Log.Information("[Save Policy] Row inserted");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[SavePolicy] Inserted has error {ex.Message}", ex);
            }

            await Task.CompletedTask;
        }

        public async Task<string> GetDelegationInfoAsync(string deliveryNumber)
        {
            using var db = new MySqlConnection(_connectionString);
            await db.OpenAsync();

            string condition = "";

            string query = $"Select * from Table " +
                $"Where {condition}" +
                $"And DATE(fromDate) <= @today And DATE(toDate) >= @today";

            try
            {
                var genericKeys = await db.QueryAsync(query, new
                {
                    today = DateTime.UtcNow.Date
                });

                foreach (var itemKey in genericKeys)
                {
                    string genericKey = (string)itemKey.GenericKey;
                    if (genericKey == "*") return genericKey;

                    if (genericKey.EndsWith("*"))
                    {
                        string matches = genericKey.Substring(0, deliveryNumber.Length - 1);
                        if (deliveryNumber.StartsWith(matches))
                        {
                            return genericKey;
                        }
                    }

                    if (genericKey.StartsWith("*"))
                    {
                        string matches = genericKey.Substring(1);
                        if (deliveryNumber.EndsWith(matches))
                        {
                            return genericKey;
                        }
                    }

                    if (genericKey.Contains("*"))
                    {
                        var str = genericKey.Split("*");
                        if (str.Length == 2)
                        {
                            string startMatches = str[0];
                            string endMatches = str[1];

                            if (deliveryNumber.StartsWith(startMatches) && deliveryNumber.EndsWith(endMatches))
                            {
                                return genericKey;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error {ex.Message}", ex);
            }

            return "";
        }

        public Task<string> GetDataInfoAsync(string genericKey, int genericType, string issuer, string actor)
        {
            throw new NotImplementedException();
        }
    }
