SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Agendamento](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_medico] [int] NOT NULL,
	[id_usuario] [int] NOT NULL,
	[id_especialidade] [int] NOT NULL,
	[data_agendamento] [smalldatetime] NOT NULL,
	[start] [bigint] NOT NULL,
	[end] [bigint] NULL,
	[valor] [decimal](10, 2) NOT NULL,
	[status] [int] NOT NULL,
	[sticker] [varchar](20) NULL,
	[observacao] [varchar](500) NULL,
 CONSTRAINT [PK_Agendamento] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AgendamentoStatus](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_agendamento] [int] NOT NULL,
	[data] [smalldatetime] NOT NULL,
	[status] [int] NOT NULL,
 CONSTRAINT [PK_AgendamentoStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NULL,
	[TwoFactorEnabled] [bit] NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NULL,
	[AccessFailedCount] [int] NULL,
	[UserName] [nvarchar](256) NULL,
	[LockoutEnd] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Discriminator] [nvarchar](128) NULL,
	[ConcurrencyStamp] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Especialidade](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](60) NOT NULL,
	[descricao] [varchar](max) NULL,
 CONSTRAINT [PK_Especialidade] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Medico](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_especialidade] [int] NOT NULL,
	[nome] [varchar](100) NOT NULL,
	[sexo] [varchar](30) NOT NULL,
	[cpfcnpj] [varchar](20) NULL,
	[rg] [varchar](30) NULL,
	[crm] [varchar](30) NULL,
	[data_nascimento] [varchar](30) NULL,
	[obs] [varchar](400) NULL,
	[inativo] [bit] NULL,
 CONSTRAINT [PK_Medico] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Ubs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nome_fantasia] [varchar](100) NULL,
	[razao_social] [varchar](100) NULL,
	[cnpj] [varchar](20) NULL,
	[responsavel] [varchar](100) NULL,
	[cpf] [varchar](20) NULL,
	[inscricao_estadual] [varchar](50) NULL,
	[inscricao_municipal] [varchar](50) NULL,
	[email_ubs] [varchar](100) NULL,
	[imagem] [varchar](max) NULL,
 CONSTRAINT [PK_Ubs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Usuario](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](150) NOT NULL,
	[sexo] [char](1) NULL,
	[inativo] [bit] NOT NULL,
	[id_aspnetuser] [nvarchar](128) NOT NULL,
	[tipo_usuario] [int] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW  [vwLocAgendamento]
AS
SELECT 
	a.id_usuario,
	m.id as 'id_medico',
	e.id as 'is_especialidade',	
	m.nome as 'nome_medico',
	e.nome as 'nome_especialidade',
	CONVERT(VARCHAR(5), DATEADD(HOUR, -3, DATEADD(MILLISECOND, a.start % 1000, DATEADD(SECOND, a.start / 1000, '19700101'))), 8) as horario_inicio,
    CONVERT(VARCHAR(5), DATEADD(HOUR, -3,DATEADD(MILLISECOND, a.[end] % 1000, DATEADD(SECOND, a.[end] / 1000, '19700101'))), 8) as horario_fim
	FROM Agendamento a inner join 
		Medico m on m.id = a.id_medico inner join
		Especialidade e on e.id = a.id_especialidade  
GO
ALTER TABLE [Agendamento]  WITH CHECK ADD  CONSTRAINT [FK_Agendamento_Especialidade] FOREIGN KEY([id_especialidade])
REFERENCES [Especialidade] ([id])
GO
ALTER TABLE [Agendamento] CHECK CONSTRAINT [FK_Agendamento_Especialidade]
GO
ALTER TABLE [Agendamento]  WITH CHECK ADD  CONSTRAINT [FK_Agendamento_Medico] FOREIGN KEY([id_medico])
REFERENCES [Medico] ([id])
GO
ALTER TABLE [Agendamento] CHECK CONSTRAINT [FK_Agendamento_Medico]
GO
ALTER TABLE [Agendamento]  WITH CHECK ADD  CONSTRAINT [FK_Agendamento_Paciente] FOREIGN KEY([id_usuario])
REFERENCES [Usuario] ([id])
GO
ALTER TABLE [Agendamento] CHECK CONSTRAINT [FK_Agendamento_Paciente]
GO
ALTER TABLE [AgendamentoStatus]  WITH CHECK ADD  CONSTRAINT [FK_AgendamentoStatus_Agendamento] FOREIGN KEY([id_agendamento])
REFERENCES [Agendamento] ([id])
GO
ALTER TABLE [AgendamentoStatus] CHECK CONSTRAINT [FK_AgendamentoStatus_Agendamento]
GO
ALTER TABLE [Medico]  WITH CHECK ADD  CONSTRAINT [FK_Medico_Especialidade] FOREIGN KEY([id_especialidade])
REFERENCES [Especialidade] ([id])
GO
ALTER TABLE [Medico] CHECK CONSTRAINT [FK_Medico_Especialidade]
GO
ALTER TABLE [Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_AspNet] FOREIGN KEY([id_aspnetuser])
REFERENCES [AspNetUsers] ([Id])
GO
ALTER TABLE [Usuario] CHECK CONSTRAINT [FK_Usuario_AspNet]
GO
