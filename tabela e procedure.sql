use CampeonatoFutebol;

-- Criação das tabelas
CREATE TABLE CampeonatoFutebol (
    ano_campeonato DATE NOT NULL,
    nome_campeonato VARCHAR(200) NOT NULL,
    vencedor_campeonato VARCHAR(100),
    CONSTRAINT pk_CampeonatoFutebol PRIMARY KEY (nome_campeonato)
);
---------------------------------------------------------------------------------------
CREATE TABLE TimeFutebol (
    id_time_futebol INT IDENTITY (1,1) NOT NULL,
    data_criacao DATE NOT NULL,
    apelido_time VARCHAR(100) NOT NULL,
    nome_time_futebol VARCHAR(100) NOT NULL,
    pontuacao INT,
    gols_marcados INT,
    gols_sofridos INT,
    CONSTRAINT pk_TimeFutebol PRIMARY KEY (nome_time_futebol)
);
---------------------------------------------------------------------------------------
CREATE TABLE PartidaFutebol (
    id_partida_futebol INT IDENTITY (1,1) NOT NULL,
    nome_campeonato VARCHAR(200) NOT NULL,
    time_mandante VARCHAR(100) NOT NULL,
    time_visitante VARCHAR(100) NOT NULL,
    gols_mandante INT,
    gols_visitante INT,
    total_gols INT,
    vencedor_partida VARCHAR(100) NOT NULL,
    CONSTRAINT pk_PartidaFutebol PRIMARY KEY (id_partida_futebol),
    CONSTRAINT fk_time_mandante FOREIGN KEY (time_mandante) REFERENCES TimeFutebol (nome_time_futebol),
    CONSTRAINT fk_time_visitante FOREIGN KEY (time_visitante) REFERENCES TimeFutebol (nome_time_futebol),
    CONSTRAINT fk_vencedor_partida FOREIGN KEY (vencedor_partida) REFERENCES TimeFutebol (nome_time_futebol),
    CONSTRAINT fk_nome_campeonato FOREIGN KEY (nome_campeonato) REFERENCES CampeonatoFutebol (nome_campeonato)
);
---------------------------------------------------------------------------------------
CREATE PROCEDURE InserirTimeFutebol
    @nome_time_futebol VARCHAR(100),
    @data_criacao DATE,
    @apelido_time VARCHAR(100)
AS
BEGIN
    INSERT INTO TimeFutebol (nome_time_futebol, data_criacao, apelido_time)
    VALUES (@nome_time_futebol, @data_criacao, @apelido_time);
END;
---------------------------------------------------------------------------------------
CREATE PROCEDURE InserirPartidaFutebol
    @nome_campeonato VARCHAR(200),
    @time_mandante VARCHAR(100),
    @time_visitante VARCHAR(100)
AS
BEGIN
    INSERT INTO PartidaFutebol (nome_campeonato, time_mandante, time_visitante)
    VALUES (@nome_campeonato, @time_mandante, @time_visitante);
END;
---------------------------------------------------------------------------------------
CREATE PROCEDURE InserirCampeonatoFutebol
    @nome_campeonato VARCHAR(200),
    @ano_campeonato DATE
AS
BEGIN
    INSERT INTO CampeonatoFutebol (nome_campeonato, ano_campeonato)
    VALUES (@nome_campeonato, @ano_campeonato);
END;
---------------------------------------------------------------------------------------
CREATE PROCEDURE InserirGolsFutebol
    @id_partida_futebol INT,
    @gols_mandante INT,
    @gols_visitante INT,
    @nome_time_futebol VARCHAR(100)
AS
BEGIN
    UPDATE PartidaFutebol
    SET gols_mandante = @gols_mandante,
        gols_visitante = @gols_visitante
    WHERE id_partida_futebol = @id_partida_futebol;

    UPDATE TimeFutebol
    SET gols_marcados = gols_marcados + @gols_mandante,
        gols_sofridos = gols_sofridos + @gols_visitante
    WHERE nome_time_futebol = @nome_time_futebol;
END;
---------------------------------------------------------------------------------------
CREATE PROCEDURE AtualizarPontuacaoFutebol
AS
BEGIN
    UPDATE TimeFutebol
    SET pontuacao = (SELECT 
                        CASE 
                            WHEN gols_mandante > gols_visitante THEN pontuacao + 5
                            WHEN gols_mandante < gols_visitante THEN pontuacao + 3
                            ELSE pontuacao + 1
                        END
                    FROM PartidaFutebol
                    WHERE PartidaFutebol.time_mandante = TimeFutebol.nome_time_futebol OR PartidaFutebol.time_visitante = TimeFutebol.nome_time_futebol);
END;
---------------------------------------------------------------------------------------
CREATE PROCEDURE ObterTimeMaisGolsFutebol
AS
BEGIN
    SELECT TOP 1 * FROM TimeFutebol
    ORDER BY gols_marcados DESC;
END;
---------------------------------------------------------------------------------------
CREATE PROCEDURE ObterTimeMenosGolsFutebol
AS
BEGIN
    SELECT TOP 1 * FROM TimeFutebol
    ORDER BY gols_sofridos ASC;
END;
---------------------------------------------------------------------------------------
CREATE PROCEDURE ObterTimeMenosGols
AS
BEGIN
    SELECT TOP 1 * FROM Time_Futebol
    ORDER BY gols_sofridos ASC;
END;
---------------------------------------------------------------------------------------
CREATE PROCEDURE ObterPartidaMaisGolsFutebol
AS
BEGIN
    SELECT TOP 1 * FROM PartidaFutebol
    ORDER BY gols_mandante + gols_visitante DESC;
END;
---------------------------------------------------------------------------------------
CREATE PROCEDURE ObterTimeCampeaoFutebol
AS
BEGIN
    SELECT TOP 1 nome_time_futebol FROM TimeFutebol
    ORDER BY pontuacao DESC;
END;
---------------------------------------------------------------------------------------
-- Trigger
CREATE TRIGGER AtualizarGolsTimesFutebol
ON PartidaFutebol
AFTER UPDATE
AS
BEGIN
    DECLARE @id_partida_futebol INT;
    DECLARE @gols_mandante INT;
    DECLARE @gols_visitante INT;
    DECLARE @nome_time_mandante VARCHAR(100);
    DECLARE @nome_time_visitante VARCHAR(100);

    SELECT @id_partida_futebol = inserted.id_partida_futebol,
           @gols_mandante = inserted.gols_mandante,
           @gols_visitante = inserted.gols_visitante,
           @nome_time_mandante = inserted.time_mandante,
           @nome_time_visitante = inserted.time_visitante
    FROM inserted;

    EXEC InserirAtualizarGolsFutebol @id_partida_futebol, @gols_mandante, @gols_visitante, @nome_time_mandante;
    EXEC InserirAtualizarGolsFutebol @id_partida_futebol, @gols_visitante, @gols_mandante, @nome_time_visitante;
END;

select * from InserirTimeFutebol