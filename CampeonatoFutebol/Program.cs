using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace CampeonatoFutebol
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string conexaoSql = new Conexao().ObterCaminho();

            int escolha = 0;
            do
            {
                Console.Clear();
                Console.WriteLine(" Campeonato de Futebol ");
                Console.WriteLine("1 - Inserir Time ");
                Console.WriteLine("2 - Inserir Partida ");
                Console.WriteLine("3 - Inserir Campeonato ");
                Console.WriteLine("4 - Inserir/Atualizar Gols ");
                Console.WriteLine("5 - Atualizar Pontuação ");
                Console.WriteLine("6 - Ver Classificação ");
                Console.WriteLine("7 - Ver Time com Mais Gols ");
                Console.WriteLine("8 - Ver Time com Menos Gols ");
                Console.WriteLine("9 - Ver Partida com Mais Gols ");
                Console.WriteLine("10 - Ver Time Campeão ");
                Console.WriteLine("0 - Sair ");

                Console.Write("Escolha uma opção:  ");
                try
                {
                    escolha = Convert.ToInt32(Console.ReadLine());
                    switch (escolha) //feito em Banco´de Dados
                    {
                        case 1:
                            InserirTimeFutebol(conexaoSql);
                            break;
                        case 2:
                            InserirPartidaFutebol(conexaoSql);
                            break;
                        case 3:
                            InserirCampeonatoFutebol(conexaoSql);
                            break;
                        case 4:
                            InserirGolsFutebol(conexaoSql);
                            break;
                        case 5:
                            AtualizarPontuacaoFutebol(conexaoSql);
                            break;
                        case 6:
                            VerClassificacao(conexaoSql);
                            break;
                        case 7:
                            ObterTimeMaisGolsFutebol(conexaoSql);
                            break;
                        case 8:
                            ObterTimeMenosGolsFutebol(conexaoSql);
                            break;
                        case 9:
                            ObterPartidaMaisGolsFutebol(conexaoSql);
                            break;
                        case 10:
                            ObterTimeCampeaoFutebol(conexaoSql);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocorreu um erro: " + ex.Message);
                }
            } while (escolha != 0);
            Console.WriteLine(" Fim do Programa ");
        }
        public static void InserirTimeFutebol(string conexaoSql)
        {
            try
            {
                Console.Write("Nome do time: ");
                string nomeTime = Console.ReadLine();                
                Console.Write("Data de criação: (AAAA-MM-DD)");
                string dataCriacao = Console.ReadLine();
                //#region formatando a data // Convertendo string 
                //DateTime dataCriacaocorreta;
                //if (DateTime.TryParseExact(dataCriacao, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dataCriacaocorreta))
                //{
                //    // Formatando a data para dia mês e ano
                //    string dataFormatada = dataCriacaocorreta.ToString("dd/MM/yyyy");                   
                //    Console.WriteLine("Data formatada: " + dataFormatada);
                //}
                //else
                //{
                //    Console.WriteLine("Formato de data inválido. Use o formato AAAA-MM-DD.");
                //}
                //#endregion
                Console.Write("Apelido do time: ");
                string apelidoTime = Console.ReadLine();

                using (SqlConnection conexao = new SqlConnection(conexaoSql))
                {
                    conexao.Open();
                    SqlCommand cmd = new SqlCommand("InserirTimeFutebol", conexao);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nome_time_futebol", nomeTime);
                    cmd.Parameters.AddWithValue("@data_criacao", dataCriacao);
                    cmd.Parameters.AddWithValue("@apelido_time", apelidoTime);

                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Time inserido com sucesso!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocorreu um erro ao inserir o time: " + e.Message);
            }
        }

        public static void InserirPartidaFutebol(string conexaoSql)
        {
            try
            {
                Console.Write("Nome do campeonato: ");
                string nomeCampeonato = Console.ReadLine();
                Console.Write("Time mandante: ");
                string timeMandante = Console.ReadLine();
                Console.Write("Time visitante: ");
                string timeVisitante = Console.ReadLine();

                using (SqlConnection conexao = new SqlConnection(conexaoSql))
                {
                    conexao.Open();
                    SqlCommand cmd = new SqlCommand("InserirPartidaFutebol", conexao);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nome_campeonato", nomeCampeonato);
                    cmd.Parameters.AddWithValue("@time_mandante", timeMandante);
                    cmd.Parameters.AddWithValue("@time_visitante", timeVisitante);

                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Partida inserida com sucesso!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocorreu um erro ao inserir a partida: " + e.Message);
            }
        }

        public static void InserirCampeonatoFutebol(string conexaoSql)
        {
            try
            {
                Console.Write("Nome do campeonato: ");
                string nomeCampeonato = Console.ReadLine();

                Console.Write("Data de início do campeonato (AAAA-MM-DD): ");
                string dataInicio = Console.ReadLine();

                using (SqlConnection conexao = new SqlConnection(conexaoSql))
                {
                    conexao.Open();
                    SqlCommand cmd = new SqlCommand("InserirCampeonatoFutebol", conexao);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nome_campeonato", nomeCampeonato);
                    cmd.Parameters.AddWithValue("@data_inicio", dataInicio);
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Campeonato inserido com sucesso!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocorreu um erro ao inserir o campeonato: " + e.Message);
            }
        }

        public static void InserirGolsFutebol(string conexaoSql)
        {
            try
            {
                Console.Write("ID da partida: ");
                int idPartida = Convert.ToInt32(Console.ReadLine());

                Console.Write("Quantidade de gols do time mandante: ");
                int golsMandante = Convert.ToInt32(Console.ReadLine());

                Console.Write("Quantidade de gols do time visitante: ");
                int golsVisitante = Convert.ToInt32(Console.ReadLine());

                Console.Write("Nome do time que marcou os gols: ");
                string nomeTime = Console.ReadLine();

                using (SqlConnection conexao = new SqlConnection(conexaoSql))
                {
                    conexao.Open();
                    SqlCommand cmd = new SqlCommand("InserirGolsFutebol", conexao);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_partida_futebol", idPartida);
                    cmd.Parameters.AddWithValue("@gols_mandante", golsMandante);
                    cmd.Parameters.AddWithValue("@gols_visitante", golsVisitante);
                    cmd.Parameters.AddWithValue("@nome_time_futebol", nomeTime);
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Gols inseridos/atualizados com sucesso!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocorreu um erro ao inserir/atualizar os gols: " + e.Message);
            }
        }

        public static void AtualizarPontuacaoFutebol(string conexaoSql)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection(conexaoSql))
                {
                    conexao.Open();
                    SqlCommand command = new SqlCommand("AtualizarPontuacaoFutebol", conexao);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Pontuações atualizadas com sucesso!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao atualizar as pontuações: {ex.Message}");
            }
        }

        public static void VerClassificacao(string conexaoSql)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection(conexaoSql))
                {
                    conexao.Open();
                    SqlCommand command = new SqlCommand("ObterClassificacaoFutebol", conexao);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                    Console.WriteLine("Classificação:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Time: {reader["nome_time_futebol"]}, Pontuação: {reader["pontuacao"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao obter a classificação: {ex.Message}");
            }
        }

        public static void ObterTimeMaisGolsFutebol(string conexaoSql)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection(conexaoSql))
                {
                    conexao.Open();
                    SqlCommand command = new SqlCommand("ObterTimeMaisGolsFutebol", conexao);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                    Console.WriteLine("Time com mais gols:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Time: {reader["nome_time_futebol"]}, Gols Marcados: {reader["gols_marcados"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao obter o time com mais gols: {ex.Message}");
            }
        }

        public static void ObterTimeMenosGolsFutebol(string conexaoSql)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection(conexaoSql))
                {
                    conexao.Open();
                    SqlCommand command = new SqlCommand("ObterTimeMenosGolsFutebol", conexao);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                    Console.WriteLine("Time com menos gols:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Time: {reader["nome_time_futebol"]}, Gols Sofridos: {reader["gols_sofridos"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao obter o time com menos gols: {ex.Message}");
            }
        }

        public static void ObterPartidaMaisGolsFutebol(string conexaoSql)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection(conexaoSql))
                {
                    conexao.Open();
                    SqlCommand command = new SqlCommand("ObterPartidaMaisGolsFutebol", conexao);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                    Console.WriteLine("Partida com mais gols:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID da Partida: {reader["id_partida_futebol"]}, Total de Gols: {reader["total_gols"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao obter a partida com mais gols: {ex.Message}");
            }
        }

        public static void ObterTimeCampeaoFutebol(string conexaoSql)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection(conexaoSql))
                {
                    conexao.Open();
                    SqlCommand command = new SqlCommand("ObterTimeCampeaoFutebol", conexao);
                    command.CommandType = CommandType.StoredProcedure;

                    object result = command.ExecuteScalar();
                    Console.WriteLine($"O time campeão é: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao obter o time campeão: {ex.Message}");
            }
        }
    }
}
                